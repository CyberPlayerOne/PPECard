using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Data.OleDb;
using System.IO;
using System.Xml;
using CustomUIControls;

namespace PPECard
{
    public partial class MainForm : Form
    {
        //字段声明
        private OleDbConnection oleDbCon;
        private string connectionStr;//将在构造函数中进行初始化//其实ConnectionStr也完全可以像CurrentDirectory那样设为静态变量，不再改变，以供全局使用！！
        private OleDbCommand oleDbCmd;
        private OleDbDataReader oleDbCardReader;

        private int PortReceiveUdp = 8080;
        private UdpClient udpClient;

        public static int PortTcpServer = 9999;//as server
        private TcpListener listener;

        private string LocalUserIsOnline = "true";
        private string MyName;//用于存储本地用户的名字，这样就不需要每次UDP广播时连接数据库获取名字，只需一次连接就足够

        //private TcpClient tclient;//as client  不再用ArrayList里代替！而在每个客户端线程函数里用他的时候新建一个TcpClient且通过参数传递之
        //private NetworkStream tnetworkStream; 同样的，此NetworkStream对象通过tcpClient对象的GetStream() 方法获取。ConnectCallback(IAsyncResult ar)方法里传递的末个参数为client以使其回调函数可以获取NetworkStream对象

        //System.Collections.ArrayList TcpClientList = new System.Collections.ArrayList();//当前本机作为TCP Client与多个服务器建立连接时的TcpClient对象列表
        //Dictionary<string, TcpClient> TcpClientDic = new Dictionary<string, TcpClient>();//本机作为客户端，连接到n个服务器的在本机上的n个TcpClient对象

        private bool isExit = false;
        

        //用于线程同步，初始状态设为非终止状态，使用手动重置方式
        private EventWaitHandle allDone = new EventWaitHandle(false, EventResetMode.ManualReset);//as server
        private EventWaitHandle allDoneClient = new EventWaitHandle(false, EventResetMode.ManualReset);//as client

        //其他偶尔使用的变量
        bool isFirstTimeOffLine = true;//第267行使用

        //当前工作目录
        public static  string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

        //--CustomUIControls.TaskbarNotifier
        private TaskbarNotifier taskbarNotifier1;
        //--

        //--------------------------------------------------------------------
        void CloseClick(object obj, EventArgs ea)
        {
            taskbarNotifier1.Hide();
        }

        void TitleClick(object obj, EventArgs ea)//已经禁止title可点击
        {
            MessageBox.Show("Title was Clicked");
        }

        void ContentClick(object obj, EventArgs ea)
        {
           // MessageBox.Show("如何显示刚上线用户的名片呢？");
            ListViewItem item = GetItemByIpString(taskbarNotifier1.CurrentUserIpString);
           item.Selected = true;//能选中
            //item.Focused = true;//能选中,但不能使用下面的事件函数
            //item.Checked = true;//也能选中
            listViewUsers_ItemActivate(null, null);

        }



        public MainForm()
        {
            InitializeComponent();

            ///////////////////////
            InitListViewUsers();

            //--CustomUIControls.TaskbarNotifier 初始化设置代码
            taskbarNotifier1 = new TaskbarNotifier();
            //
            //taskbarNotifier1.SetBackgroundBitmap(new Bitmap("skin.bmp"), Color.FromArgb(255, 0, 255));
            //taskbarNotifier1.SetCloseBitmap(new Bitmap("close.bmp"), Color.FromArgb(255, 0, 255), new Point(127, 8));
            //taskbarNotifier1.TitleRectangle = new Rectangle(40, 9, 70, 25);
            //taskbarNotifier1.ContentRectangle = new Rectangle(8, 41, 133, 68);
            //taskbarNotifier1.TitleClick += new EventHandler(TitleClick);
            //taskbarNotifier1.ContentClick += new EventHandler(ContentClick);
            //taskbarNotifier1.CloseClick += new EventHandler(CloseClick);

            //用skin3
            taskbarNotifier1.SetBackgroundBitmap(new Bitmap("skin3.bmp"), Color.FromArgb(255, 0, 255));
            taskbarNotifier1.SetCloseBitmap(new Bitmap("close.bmp"), Color.FromArgb(255, 0, 255), new Point(280, 57));
            taskbarNotifier1.TitleRectangle = new Rectangle(150, 57, 125, 28);
            taskbarNotifier1.ContentRectangle = new Rectangle(75, 92, 215, 55);
            taskbarNotifier1.TitleClick += new EventHandler(TitleClick);
            taskbarNotifier1.ContentClick += new EventHandler(ContentClick);
            taskbarNotifier1.CloseClick += new EventHandler(CloseClick);
            //-
            taskbarNotifier1.CloseClickable = true;
            taskbarNotifier1.TitleClickable = false;//title is not clickable!!
            taskbarNotifier1.ContentClickable = true;
            taskbarNotifier1.EnableSelectionRectangle = true;
            taskbarNotifier1.KeepVisibleOnMousOver = true;	// Added Rev 002
            taskbarNotifier1.ReShowOnMouseOver = true;			// Added Rev 002
            //--            
            connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CurrentDirectory + "\\" + "LocalData.mdb";

            #region 从数据库中获取本地用户的名字,并存在string变量MyName里
            OleDbConnection ConGetName = new OleDbConnection();
            ConGetName.ConnectionString = connectionStr;
            ConGetName.Open();
            OleDbCommand CmdGetName = ConGetName.CreateCommand();
            CmdGetName.CommandText = "select 名字 from LocalUser";
            MyName = (string)CmdGetName.ExecuteScalar();
            ConGetName.Dispose();
            #endregion

            
        }

        ////////////////////////
        /// <summary>
        /// 初始化ListViewUsers,填充ListView结构
        /// </summary>
        private void InitListViewUsers()
        {
            ColumnHeader UserName = new ColumnHeader();
            UserName.Text = "名片用户名";
            UserName.Width = 130;
            ColumnHeader isFriend = new ColumnHeader();
            isFriend.Text = "朋友关系";
            isFriend.Width = 100;
            ColumnHeader isOnline = new ColumnHeader();
            isOnline.Text = "在线";

            listViewUsers.View = View.LargeIcon;
            listViewUsers.Columns.Add(UserName);
            listViewUsers.Columns.Add(isFriend);
            listViewUsers.Columns.Add(isOnline);

            listViewFind.View = View.LargeIcon;
            listViewFind.Columns.Add((ColumnHeader)UserName.Clone());
            listViewFind.Columns.Add((ColumnHeader)isFriend.Clone());
            listViewFind.Columns.Add((ColumnHeader)isOnline.Clone());
        }
        

        /// <summary>
        /// 后台线程函数:接收UDP广播,获取在线用户状态信息.在后台运行进行接收.
        /// </summary>
        private void ReceiveUserStates()
        {
            //在本机指定端口接受UDP广播
            udpClient = new UdpClient(PortReceiveUdp);
            IPEndPoint remote = null;

             
            //接收
            while (true)//在线则显示,不在线则不显示
            {
                //try
                //{
                    //关闭UdpClient时此句异常
                    byte[] bytes = udpClient.Receive(ref remote);
                    string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    //用户状态字符串的格式为
                    //用户名(string):在线否(bool)
                    string UserName = "";
                    bool isOnline;
                    string isOnlineStr = "";
                    int NameLength = 0;
                    NameLength = str.IndexOf(":");

                    UserName = str.Substring(0, NameLength);
                    isOnlineStr = str.Substring(NameLength + 1);

                    if (isOnlineStr == "true")
                    {
                        isOnline = true;
                    }
                    else if (isOnlineStr == "false")
                    {
                        isOnline = false;
                    }
                    else { MessageBox.Show("error"); break; }
                    
                    ListViewItem thisUserItem = (ListViewItem)CheckUserItem(UserName,remote,true);
                    bool isFriend = CheckIsFriend(remote.Address.ToString());

                    if (thisUserItem == null && isOnline)//如果用户不存在于ListViewUsers，并且上线！！如果是下线，则不管。
                    {//不是好友，但上线，并且在好友列表中：的情况有问题！！？已解决
                        //如果在ListView中尚无此用户，添加之
                        AddListViewUserItem(UserName,remote,isFriend,true);//一般不是好友，好友已经从数据库中加上了；但特殊情况是：
                                                                           //刚刚加为好友，对方从在线变为下线，后又变为上线  .这时需要修改分组！ 

                        taskbarNotifier1.CurrentUserIpString = remote.Address.ToString();
                        taskbarNotifier1.Show("上线通知","用户"+ UserName+ "上线了！\n\n【点此查看此人名片】", 500, 3000, 500);
                         //日志记录
                            LogRoutine("用户" + UserName + "上线.");                        
                    }
                    else//在ListViewUsers里
                    {
                        
                        //如果已经在ListView中有此用户,设置其是否在线状态
                        SetUserState(thisUserItem,isFriend, isOnline);//--
                    }

                //}
                ////catch(Exception ex)
                //{
                //        //MessageBox.Show(ex.Message);//test only
                //        throw;
                //    //退出循环,结束线程
                //    break;
                //}
            }
        }
        delegate ListViewItem CheckUserItemCallbackDelegate(string UserName, IPEndPoint remote, bool isOnline);//委托的返回值要与被委托的方法的返回值一致！！！！！！
        CheckUserItemCallbackDelegate CheckUserItemCallback;
        /// <summary>
        /// 使用传递的参数，创建并且检查listViewItem是否已经在ListViewUsers已经存在
        /// </summary>
        /// <param name="UserName">User's Name string</param>
        /// <param name="remote">对方的IPEndPoint,获取其IP字符串并保存在Tag中</param>
        /// <param name="isOnline">bool值，是否在线</param>
        private ListViewItem CheckUserItem(string UserName, IPEndPoint remote, bool isOnline)
        {
            if (listViewUsers.InvokeRequired&&this!=null)//线程外操作此控件//?????????????????????????????????????????????????有异常
            {
                CheckUserItemCallback = new CheckUserItemCallbackDelegate(CheckUserItem);
               return (ListViewItem)this.listViewUsers.Invoke(CheckUserItemCallback, UserName,remote,true);//Invoke方法的返回值---返回的是被委托的方法的返回值！！！！！！！
            }
            else
            {
               //ListViewItem lvi=(ListViewItem)listViewUsers.FindItemWithText(UserName);//要使用Tag来对比！！！！！！
                //新建一个ListViewItem -> 遍历已有ListViewItem,察看Tag并对比remote里的Ip字符串：如果相同则返回null，不同则返回新建的ListViewItem
                ListViewItem thisItem = new ListViewItem(UserName);
                thisItem.Tag = remote.Address.ToString();//用Tag保存其IP字符串
                thisItem.ImageIndex = 0;
                ListViewItem.ListViewSubItem lvsiFriend = new ListViewItem.ListViewSubItem();
                lvsiFriend.Text = "新建的ListViewItem的是否朋友的SubItem，正常则不会显示！";//!!没有用，因为新的这个ListViewItem不会使用
                ListViewItem.ListViewSubItem lvsiOnline = new ListViewItem.ListViewSubItem();
                lvsiOnline.Text = "√";
                thisItem.SubItems.Add(lvsiFriend);
                thisItem.SubItems.Add(lvsiOnline);
                //创建新的ListViewItem完毕，开始对比Tag
                foreach (ListViewItem item in listViewUsers.Items)
                {
                    if (item.Tag.Equals(thisItem.Tag))//已经显示了此人(此人在线，但是否好友要用数据库检查)//不能用"item.Tag==thisItem.Tag"直接对比？？？？
                    {
                       // return thisItem;//错误！这样在ReceiveUserState中设置其在线否将无意义！(设置到一个未加入ListView的Items集合中的ListViewItem)
                        return item;//应该返回这个已经加入的ListViewItem
                    }
                }
                return null;//没有找到
            }
        }
        private delegate bool CheckIsFriendCallback(string ip);
        private CheckIsFriendCallback checkIsFriendCallback;
        /// <summary>
        /// [已允许跨线程]从本地数据库中检查此IP对应用户是不是好友
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private bool CheckIsFriend(string ip)
        {
            if (this.listViewUsers.InvokeRequired)
            {
                checkIsFriendCallback = new CheckIsFriendCallback(CheckIsFriend);
                return (bool)this.Invoke(checkIsFriendCallback, ip);
            }
            else//!!!!!!!!!!这样-检查数据库中是否有此条记录！！！
            {
                try
                {
                    OleDbConnection oc = new OleDbConnection(connectionStr);
                    oc.Open();
                    OleDbCommand oCmd = new OleDbCommand("select count(*) from MyFriendsData where IP=" + "\"" + ip + "\"", oc);
                    int intRt = (int)oCmd.ExecuteScalar();//如果数据库中由此人则intRt为1，无则为0
                    oc.Close();
                    return (intRt == 0 ? false : true);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR IN Function CheckIsFriend"); return false; }
                
            }
        }

        delegate void AddListViewUserItemCallbackDelegate(string UserName, IPEndPoint remote,bool isFriend,bool isOnline);
        AddListViewUserItemCallbackDelegate AddListViewUserItemCallback;
        /// <summary>
        /// 用于此用户不在ListViewUsers里的情况。本函数添加对应要求的listViewItem,在线否用参数传递。
        /// 
        /// 主要用于：
        /// （陌生人上线，以及从本地数据库中的好友未上线）
        /// </summary>
        /// <param name="UserName">User's Name string</param>
        /// <param name="remote">对方的IPEndPoint,获取其IP字符串并保存在Tag中</param>
        /// <param name="isFriend">bool值，是否好友</param>
        /// <param name="isOnline">bool值，是否在线</param>
        private void AddListViewUserItem(string UserName, IPEndPoint remote,bool isFriend,bool isOnline)
        {
            try
            {
                if (listViewUsers.InvokeRequired)//线程外操作此控件
                {
                    AddListViewUserItemCallback = new AddListViewUserItemCallbackDelegate(AddListViewUserItem);
                    this.Invoke(AddListViewUserItemCallback, UserName, remote,isFriend,isOnline);
                }
                else//根据IP确定一个用户
                {
                    listViewUsers.BeginUpdate();
                    ListViewItem thisUserItem = new ListViewItem(UserName);
                    thisUserItem.Tag = remote.Address.ToString();//用Tag保存其IP字符串
                    thisUserItem.ImageIndex = 0;

                    ListViewItem.ListViewSubItem lvsiFriend = new ListViewItem.ListViewSubItem();
                    lvsiFriend.Text = isFriend ? "√" : "";//!!!!!!是朋友否 
                    thisUserItem.Group = (isFriend ? listViewUsers.Groups["listViewGroupFriends"] : listViewUsers.Groups["listViewGroupStrange"]);//??
                    ListViewItem.ListViewSubItem lvsiOnline = new ListViewItem.ListViewSubItem();
                    if (isOnline)//在线否
                    {
                        lvsiOnline.Text = "√";
                    }
                    else
                    {
                        lvsiOnline.Text = "";
                    }

                    thisUserItem.SubItems.Add(lvsiFriend);
                    thisUserItem.SubItems.Add(lvsiOnline);
                    listViewUsers.Items.Add(thisUserItem);
                    listViewUsers.EndUpdate();
                    //test only
                    //MessageBox.Show(thisUserItem.Tag.ToString());//remote.Address.ToString()
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private delegate void SetUserStateCallbackDelegate(ListViewItem item, bool isFriend,bool isOnline);
        SetUserStateCallbackDelegate SetUserStateCallback;
        /// <summary>
        /// 对存在于ListViewUsers里的Item设置用户在线情况,如果不在线并且不是好友则决定删除此项目
        /// </summary>
        /// <param name="item"></param>
        /// <param name="isOnline"></param>
        private void SetUserState(ListViewItem item, bool isFriend,bool isOnline)
        {
            if (listViewUsers.InvokeRequired)
            {
                SetUserStateCallback = new SetUserStateCallbackDelegate(SetUserState);
                this.Invoke(SetUserStateCallback, item, isFriend,isOnline);
            }
            else
            {
                if(item!=null)
                {
                    if (isOnline == false)
                    {
                        if (!isFriend)
                        {
                            SetItemGroup(item.Tag.ToString(), false);
                            listViewUsers.Items.Remove(item);//MSDN:如果 ArrayList 不包含指定对象，则 ArrayList 保持不变。不引发异常。
                            return;
                        }
                        //不在线，但是好友
                        SetItemGroup(item.Tag.ToString(), true);
                        item.SubItems[1].Text = "√";//是好友
                        item.SubItems[2].Text = "";//不在线
                    }
                    else//上线
                    {
                        if (isFriend) //是好友、不在线、进行删除后，item被移除-成功解决，但是item会为空，故此处解决。
                        { 
                            //
                            if (item.SubItems[2].Text == "")//目前为""说明这次上线是第一次上线！！！那么就可以弹出taskbarNotifier1
                            {
                                taskbarNotifier1.CurrentUserIpString = item.Tag.ToString();
                                taskbarNotifier1.Show("上线通知", "用户" + item.Text + "上线了！\n\n【点此查看此人名片】", 500, 3000, 500);
                            }
                            //

                            SetItemGroup(item.Tag.ToString(), true);
                            item.SubItems[1].Text = "√";//是好友
                            item.SubItems[2].Text = "√";//在线
                        }
                        else//不是好友（是一种特殊情况：刚刚添加对方为好友，对方下线后又上线时）
                        {
                            SetItemGroup(item.Tag.ToString(), false);
                            item.SubItems[1].Text = "";//不是好友
                            item.SubItems[2].Text = "√";//在线
                        }
                        //taskbarNotifier1.Show("上线通知", item.Text+ "上线了！", 500, 3000, 500);这样会一遍又一遍，每隔五秒钟
                    }
                }
            }
        }
        
        /// <summary>
        /// 广播本地用户状态信息
        /// </summary>
        private void LocalUserStateBroadcast(string isOnline)
        {
            UdpClient myUdpClient = new UdpClient();
            try
            {
                //自动提供子网IP广播地址
                IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, PortReceiveUdp);
                //允许发送和接收广播数据报
                myUdpClient.EnableBroadcast = true;

                ////test only
                //IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PortReceiveUdp);                

                //发送 我在线的信息
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(MyName + ":" + isOnline);//需修改???????????????????????????
                //向子网发送广播(用户名:在线否)
                myUdpClient.Send(bytes, bytes.Length, iep);
            }
            catch (Exception ex)
            {
                if (isFirstTimeOffLine)//本地用户是第一次下线
                {
                    isFirstTimeOffLine = false;

                    try
                    {
                        string time = DateTime.Now.ToString();
                        System.IO.StreamWriter sw = System.IO.File.AppendText(CurrentDirectory + @"\log\" + "error_log.log");
                        sw.WriteLine("【" + time + "】" + "[" + "一般错误" + "]" + ex.Message + "网络连接有问题,", "未能上线!");
                        sw.Dispose();
                    }
                    catch { }

                    MessageBox.Show("相关信息:" + ex.Message + "。请检查网络连接！", "无法上线");
                }//第一次发现没有网络连接之后便不再MessageBox但仍然尝试发送上线的广播
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //接收UDP广播的线程
            Thread ThreadReceiveUdp = new Thread(new ThreadStart(ReceiveUserStates));
            ThreadReceiveUdp.IsBackground = true;
            ThreadReceiveUdp.Start();
            //定时广播
            timerUdpBroadcast.Start();

            //监听TCP请求的线程
            Thread ThreadListenTcp = new Thread(new ThreadStart(AcceptTcpConnect));
            ThreadListenTcp.IsBackground = true;
            ThreadListenTcp.Start();

            //从数据库中获取所有已经成为好友的用户，并作为ListViewItem显示在ListView中
            ListViewAllFriendsFromLocalDataBase();

            //如果没有此目录则建立之，用于存放头像
            if (!Directory.Exists( CurrentDirectory+ "\\Images")) Directory.CreateDirectory(CurrentDirectory+ "\\Images");
            if (!Directory.Exists(CurrentDirectory + "\\log")) Directory.CreateDirectory(CurrentDirectory + "\\log");
            if(!File.Exists(CurrentDirectory+ @"\log\" + "run.log")) File.Create(CurrentDirectory + @"\log\" + "run.log");
            string errLog =CurrentDirectory + @"\log\" + "error_log.log";
            if (!File.Exists(errLog)) File.Create(CurrentDirectory + @"\log\" + "error_log.log");
            try
            {
                //记录日志
                string time = DateTime.Now.ToString();
                System.IO.StreamWriter sw = System.IO.File.AppendText(CurrentDirectory + @"\log\" + "run.log");
                sw.WriteLine("【" + time + "】" + "程序开始运行.");
                sw.Dispose();
            }
            catch { }
        }
        /// <summary>
        /// 从数据库中获取所有已经成为好友的用户，并显示在ListView中
        /// </summary>
        private void ListViewAllFriendsFromLocalDataBase()
        {
            try
            {
                //用while(DataReader.Read())循环获取并显示依已存的用户
                oleDbCon = new OleDbConnection();
                oleDbCon.ConnectionString = connectionStr;
                oleDbCon.Open();
                oleDbCmd = oleDbCon.CreateCommand();
                oleDbCmd.CommandText = "select * from MyFriendsData";
                oleDbCardReader = oleDbCmd.ExecuteReader();

                //提取用户名，把Ip写入ListViewItem的Tag里.用IP作为关键字               
                //ListViewItem的SubItem要包括　用户名、是否好友、是否在线　三个字段
                while (oleDbCardReader.Read())
                {
                    string strUserName = (string)oleDbCardReader["名字"];
                    string strIPAdress = (string)oleDbCardReader["IP"];
                    //使用     AddListViewUserItem　方法
                    AddListViewUserItem(strUserName, new IPEndPoint(IPAddress.Parse(strIPAdress),65535),true, false);//端口号无用,是好友true
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
               if(oleDbCon!=null) oleDbCon.Close();
                if(oleDbCardReader!=null) oleDbCardReader.Close();
            }
        }

        private void timerUdpBroadcast_Tick(object sender, EventArgs e)
        {
            LocalUserStateBroadcast(LocalUserIsOnline);//不停广播发送我在线的信息
        }

        private void timerShowStatusLabelStatus_Tick(object sender, EventArgs e)
        {
            if (上线ToolStripMenuItem1.Checked) toolStripStatusLabel.Text = "当前是在线状态...";
            else toolStripStatusLabel.Text = "当前是离线状态。";            
        }

       /// <summary>
       /// 修改ListView的View属性！！！
       /// </summary>
        private void SetListViewViewType(object sender)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ListView lv = (ListView)(listViewUsers.Visible ? listViewUsers : listViewFind);

            if (tsmi.Equals(XToolStripMenuItem))//选择了详细信息
            {
                XToolStripMenuItem.Checked = true;//手动选中，而不自动选中（当用两个Checkbox时比较好用；如果是一个，则自动选中）
                DToolStripMenuItem.Checked = false;

                lv.View = View.Details;                
            }
            else if (tsmi.Equals(DToolStripMenuItem))
            {
                DToolStripMenuItem.Checked = true;
                XToolStripMenuItem.Checked = false;

                lv.View = View.LargeIcon;                
            }
            else
            {
                MessageBox.Show("error SetListViewViewType()");
            }
        }
        private void XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetListViewViewType(sender);
        }

        private void DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetListViewViewType(sender);
        }

        private void 上线ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            tsmi.Checked = true;
            下线ToolStripMenuItem.Checked = false;

            toolStripStatusLabel.Text = "已上线...";
            //
            timerUdpBroadcast.Enabled = false;//先停止广播状态
            LocalUserIsOnline = "true";//再修改状态
            timerUdpBroadcast.Enabled = true;//再允许广播状态
            LocalUserStateBroadcast(LocalUserIsOnline);//上线            
        }

        private void 下线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            tsmi.Checked = true;
            上线ToolStripMenuItem1.Checked = false;
            toolStripStatusLabel.Text = "已下线。";
            //
            timerUdpBroadcast.Enabled = false;
            LocalUserIsOnline = "false";
            timerUdpBroadcast.Enabled = true;
            LocalUserStateBroadcast(LocalUserIsOnline);//下线

        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Application.Run(new ModifyMyCardForm());
            (new DisplayCardForm(true)).ShowDialog();
        }

        private void listViewUsers_ItemActivate(object sender, EventArgs e)
        {
            // ListView lv = (ListView)sender;如果发起者sender是菜单栏的ToolStripMenuItem，那么这样转换的判断就不行了
            //因此修改为
            ListView lv = listViewUsers.Visible ? listViewUsers : listViewFind;
            if (lv.SelectedItems.Count > 0)
            {
                //允许一次察看多个用户名片(使用右键才行)
                foreach (ListViewItem eachItem in lv.SelectedItems)
                {
                    //string UserName = listViewUsers.SelectedItems[0].SubItems[0].Text;//没有Name，因为没有设置对此对象的引用!!!
                    string UserName = eachItem.SubItems[0].Text;//第一个SubItem：用户名
                    CardForm cf = new CardForm(true);

                    //根据ListViewItem的Tag中的IP值在数据库中定位一个用户并且获取其名片信息
                    //string StrIpOfListViewItem = (string)listViewUsers.SelectedItems[0].Tag;
                    string StrIpOfListViewItem = (string)eachItem.Tag;
                    //MessageBox.Show(StrIpOfListViewItem);//test only
                    using (OleDbConnection ConGetCard = new OleDbConnection())//本来可以用本类的私有字段oleDbCon,这里新建，有些混乱
                    {
                        try
                        {
                            ConGetCard.ConnectionString = connectionStr;
                            ConGetCard.Open();
                            OleDbCommand CmdGetCard = ConGetCard.CreateCommand();
                            CmdGetCard.CommandText = "select * from MyFriendsData where IP=" + "\"" + StrIpOfListViewItem + "\"";
                            oleDbCardReader = CmdGetCard.ExecuteReader();
                            if (oleDbCardReader.Read())//正常的话应该有且只有一行
                            {
                                cf.name1 = oleDbCardReader["名字"].ToString();
                                cf.cellphone1 = oleDbCardReader["移动电话"].ToString();
                                cf.telephone1 = oleDbCardReader["固定电话"].ToString();
                                cf.qq1 = oleDbCardReader["QQ"].ToString();
                                cf.email1 = oleDbCardReader[5].ToString();//序号为0-base//oleDbCardReader["电子邮件帐户名"].ToString();为什么不行？？？？？
                                cf.website1 = oleDbCardReader["网址"].ToString();
                                //cf.diploma1暂不显示
                                cf.title1 = oleDbCardReader["职称"].ToString();
                                cf.com1 = oleDbCardReader["单位名称"].ToString();
                                cf.location1 = oleDbCardReader["地址"].ToString();
                                cf.logo1 = CurrentDirectory+@"\images\"+ oleDbCardReader["LOGO"].ToString();//用Base64后无需更改！
                                //+++++++++++++++++++++++++++++++++++++窗体也要改变
                                cf.detail1 = oleDbCardReader["个人简介"].ToString();
                            }
                            else//没有在数据库中记录，故不是好友
                            {
                                MessageBox.Show("用户\"" + UserName
                                    + "\"不是您的好友,不能查看他/她的电子名片！\n但您可以通过向" + UserName + "发送请求来获取他的名片。",
                                    "无法查看此人的名片",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);//
                                //取消了添加：是否对陌生人的名片进行请求？
                                //添加：对作为自己的Item的响应（需要否？）
                                return;
                            }
                            //读取英文名片
                            try
                            {
                                CmdGetCard = ConGetCard.CreateCommand();
                                CmdGetCard.CommandText = "select * from MyFriendsDataEn where IP=" + "\"" + StrIpOfListViewItem + "\"";
                                oleDbCardReader = CmdGetCard.ExecuteReader();
                                if (oleDbCardReader.Read())//正常的话应该有且只有一行
                                {
                                    cf.name2 = oleDbCardReader["名字"].ToString();
                                    cf.cellphone2 = oleDbCardReader["移动电话"].ToString();
                                    cf.telephone2 = oleDbCardReader["固定电话"].ToString();
                                    cf.qq2 = oleDbCardReader["QQ"].ToString();
                                    cf.email2 = oleDbCardReader[5].ToString();//序号为0-base//oleDbCardReader["电子邮件帐户名"].ToString();为什么不行？？？？？
                                    cf.website2 = oleDbCardReader["网址"].ToString();
                                    //cf.diploma2暂不显示
                                    cf.title2 = oleDbCardReader["职称"].ToString();
                                    cf.com2 = oleDbCardReader["单位名称"].ToString();
                                    cf.location2 = oleDbCardReader["地址"].ToString();
                                    cf.logo2 = CurrentDirectory + @"\images\" + oleDbCardReader["LOGO"].ToString();//用Base64后无需更改！
                                    //+++++++++++++++++++++++++++++++++++++窗体也要改变
                                    cf.detail2 = oleDbCardReader["个人简介"].ToString();
                                }
                            }
                            catch //如果找不到记录就全部设置为空
                            {
                                cf.name2 = cf.cellphone2 = cf.telephone2 = cf.qq2 = cf.email2 = cf.website2 = cf.title2 = cf.com2 = cf.location2 = cf.logo2 = cf.detail2 = ""; 
                            }
                        }
                        catch (OleDbException ex) { MessageBox.Show(ex.Message, "连接数据库发生错误"); }
                        catch (InvalidOperationException ex) { MessageBox.Show(ex.Message, "非法操作"); }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "发生错误"); }
                        finally
                        {
                            if (ConGetCard != null) { ConGetCard.Dispose(); }
                            oleDbCardReader.Dispose();
                        }
                    }

                    #region 已过时：V1.0代码
                    //DisplayCardFrom窗体内容初始化
                    //otherGuyECardForm.Controls["buttonEditBlogAddress"].Visible = false;
                    //otherGuyECardForm.Text = UserName + "的名片";
                    //otherGuyECardForm.Controls["buttonSave"].Text = "确定";

                    //修改toolTips内容／／？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？？Container为空！！！
                    //MessageBox.Show(otherGuyECardForm.Container.Components.Count.ToString());/*.SetToolTip(
                    //  ((PictureBox)otherGuyECardForm.Controls["pictureBoxDisplayPic"]), UserName + "的照片");*/
                    //禁止修改otherGuyECardForm各个控件内容在构造函数中实现
                    #endregion
                    //cf.Text = cf.name1 + "(" + cf.name2 + ")" + "的电子名片";
                    cf.InitControlTextChinese();
                    cf.Show();
                    cf.Controls["panel1"].Controls["buttonDetail"].Focus();
                }//foreach
            }//if
        }//listViewUsers_ItemActivate

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LocalUserIsOnline = "false";
            LocalUserStateBroadcast("false");//下线
            udpClient.Close();

            //使有关TCP的线程都自动结束
            isExit = true;
            allDone.Set();
            allDoneClient.Set();
        }

        private void 查看名片CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //允许一次察看多个用户名片(如果没有选中任何用户则不会反应)
            listViewUsers_ItemActivate(sender, e);            
        }

        private void 分组显示FToolStripMenuItem_Click(object sender, EventArgs e)//For ListViewUsers only
        {
            if (分组显示FToolStripMenuItem.Checked == true)
            {
                listViewUsers.ShowGroups = true;
            }
            else
            {
                listViewUsers.ShowGroups = false;
            }
        }



        private void XML格式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv = listViewUsers.Visible ? listViewUsers : listViewFind;

             //一次只生成一个用户的名片XML
            if (lv.SelectedItems.Count > 0)//确有选中用户
            {
                string StrIP = (string)lv.SelectedItems[0].Tag;
                OleDbDataAdapter OleDbDA;
                DataSet DS;
                string StrXmlPath;
                //用此 IP作为关键字在本地数据库中进行查询并读取到DataSet中
                using (oleDbCon = new OleDbConnection())
                {
                    try
                    {
                        oleDbCon.ConnectionString = connectionStr;

                        OleDbDA = new OleDbDataAdapter("select 名字,移动电话,固定电话,QQ,Email,网址,地址,LOGO,个人简介 from MyFriendsData where IP=" + "\"" + StrIP + "\"", oleDbCon);
                        //++++++++++++++++++++++++也不能用DataSet了
                        DS = new DataSet("ECard");
                        OleDbDA.Fill(DS, "MyFriendsData");
                        if (DS.Tables[0].Rows.Count == 0)
                        {
                            MessageBox.Show("此人不是您的好友，在本地数据库没有存储此人的名片！", "没有找到");
                            return;
                        }
                        //获取要存放XML的路径
                        folderBrowserDialog1.Description = "选择一个位置以存放此人的名片XML:";
                        folderBrowserDialog1.ShowNewFolderButton = true;
                        if (folderBrowserDialog1.ShowDialog()==DialogResult.OK)
                        {
                            StrXmlPath = folderBrowserDialog1.SelectedPath;
                            DS.WriteXml(StrXmlPath +@"\"+ DS.Tables[0].Rows[0]["名字"] + "的名片XML.xml");//直接使用WriteXml方法的不好处是之保存了logo的文件名而不是Base64编码
                            toolStripStatusLabel.Text="已经成功生成"+DS.Tables[0].Rows[0]["名字"] + "的名片XML文件！";
                        }
                        DS.Dispose();
                        OleDbDA.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"错误");
                    }
                    finally
                    {
                        if (oleDbCon != null) oleDbCon.Dispose();//DataSet与DataAdapter要释放吗？？？？？？？？？？？？？？？？？这里找不到
                    }
                }
            }
            //else { MessageBox.Show(""); }//test only
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ListView lv = listViewUsers.Visible ? listViewUsers : listViewFind;

            if(lv.SelectedItems.Count>0)//有选中
            {
                this.contextMenuStrip1.Items["toolStripMenuItem2"].Visible = true;
                将此人的电子名片导出为XML格式ToolStripMenuItem.Visible = true;
                toolStripMenuItem3.Visible = true;
                //如果选中Item属于陌生人组
                if (lv.SelectedItems[0].Group == lv.Groups["listViewGroupStrange"])
                {
                    for (int i = 0; i < 3; i++)
                    {
                        this.contextMenuStrip1.Items[i].Visible = false;//隐藏只用于朋友组的右键菜单
                    }
                    this.contextMenuStrip1.Items[3].Visible = true;
                    this.contextMenuStrip1.Items[8].Visible = false;//不能导出
                    this.contextMenuStrip1.Items[4].Visible = true;//横条
                }
                else if (lv.SelectedItems[0].Group == lv.Groups["listViewGroupFriends"])
                {
                    for (int i = 0; i < 3; i++)
                    {
                        this.contextMenuStrip1.Items[i].Visible = true;//显示只用于朋友组的右键菜单
                    }
                    this.contextMenuStrip1.Items[3].Visible = false;
                    this.contextMenuStrip1.Items[8].Visible = true;//可导出
                    this.contextMenuStrip1.Items[4].Visible = true;//横条
                }               
            }
            else//没有选中任何Item
            {
                for (int i = 0; i < 3; i++)
                {
                    this.contextMenuStrip1.Items[i].Visible = false;
                }
                this.contextMenuStrip1.Items[3].Visible = false;
                this.contextMenuStrip1.Items[8].Visible = false;//不能导出
                this.contextMenuStrip1.Items[4].Visible = false;//横条

                this.contextMenuStrip1.Items["toolStripMenuItem2"].Visible = false;
                this.contextMenuStrip1.Items["分组显示FToolStripMenuItem"].Visible = false;
                将此人的电子名片导出为XML格式ToolStripMenuItem.Visible = false;               
            }
            分组显示FToolStripMenuItem.Visible = true;//一直都可以看到分组显示
        }


        ///////////////////////////////TCP发送与接收////////////////////////////////////////

        #region-----------------as Server-----------------------
        /// <summary> 线程函数，作为服务器端，每个窗体创建就运行此线程；此线程通过while(isExit==false)循环保持在窗体未退出的情况下一直运行，即一直监听TCP连接请求。
        ///用于接收TCP请求：
        /// 接收请求指令为
        /// REQUEST 陌生人请求本人的名片
        /// UPDATE  朋友要求获取本人名片更新
        /// XMLCARD 接收到XML文件
        /// </summary>
        private void AcceptTcpConnect()
        {
            IPAddress[] LocalIP = Dns.GetHostAddresses(Dns.GetHostName());
            listener = new TcpListener(LocalIP[0],PortTcpServer);
            listener.Start();
            while (isExit==false)
            {
                try
                {
                    //必须本线程执行完才能执行其他线程
                    //将事件设置为非终止,其他线程阻塞
                    allDone.Reset();
                    //引用异步操作完成时调用的回调方法
                    //开始一个异步操作接受传入的连接尝试
                    listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientCallback), listener); 
                    //阻止当前线程直到收到信号
                    allDone.WaitOne();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "1S", MessageBoxButtons.OK, MessageBoxIcon.Error);//"1S"表示作为服务器端的第一处代码错误
                    break;
                }
            }
        }
        //string XmlReceivedDirectly = "";//也可以在上一个函数进行初始化，可以防止过时的垃圾XML数据积累
        private void AcceptTcpClientCallback(IAsyncResult ar)
        {
            //XmlReceivedDirectly = "";
            try
            {
                //能调用此方法，说明接收到了连接请求
                //将事件状态设为终止状态，允许等待线程（AcceptTcpConnect()方法）继续
                allDone.Set();
                TcpListener myListener = (TcpListener)ar.AsyncState;

                //异步接收传入的连接，并创建新的TcpClient对象处理远程主机通信
                TcpClient client = myListener.EndAcceptTcpClient(ar);//本机作为服务器已经接受连接，创建的client并非这个类的字段client！！这个类的字段client是本机作为客户端创建的TcpClient!
                ReadWriteObject rwObj = new ReadWriteObject(client);//一个连接对应一个ReadWriteObject!!!!
                //ClientNString cns = new ClientNString(client);
                //开始一个异步操作读取字符串(NetworkStream的方法)
                rwObj.netStream.BeginRead(rwObj.readBytes, 0, rwObj.readBytes.Length, new AsyncCallback(ReadCallback), rwObj);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"2S");
            }
        }
        private void ReadCallback(IAsyncResult ar)
        {
            ReadWriteObject rwObj = (ReadWriteObject)ar.AsyncState;
            try
            {                
                int count = rwObj.netStream.EndRead(ar);
                string StrReceive = System.Text.Encoding.UTF8.GetString(rwObj.readBytes, 0, count);

                if (StrReceive.StartsWith("UPDATE"))
                {
                    //MessageBox.Show("from client:UPDATE","test only");//test only
                    SendMyCard(rwObj.client.GetStream());//------------------------------
                    //远程主机客户端要求得到本地用户名片的更新，则只在状态栏通知本地用户后直接进行传输
                    toolStripStatusLabel.Text = "用户\""+GetItemByIpString(((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString()).Text+"\"正在获取您的电子名片的更新...";
                }
                else if (StrReceive.StartsWith("REQUEST"))
                {
                    //MessageBox.Show(StrReceive,"from client:REQUEST(test only)");//test only
                    DialogResult dr = MessageBox.Show("陌生用户\"" + GetItemByIpString(((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString()).Text + "\"请求获取您的电子名片，请问是否向对方发送？", "收到名片请求",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
                    // 
                    if (dr == DialogResult.Yes)
                    {
                        //MessageBox.Show("您回答了yes");
                        SendMyCard(rwObj.client.GetStream());//同意发送更新
                        toolStripStatusLabel.Text = "已经向用户\"" + GetItemByIpString(((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString()).Text + "\"发送了您的电子名片！";
                    }
                    else if (dr == DialogResult.No)
                    {
                        //   MessageBox.Show("您回答了No");
                        SendDeny(rwObj.client.GetStream());//发送表示拒绝的XML
                        toolStripStatusLabel.Text = "已经拒绝了用户\"" + GetItemByIpString(((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString()).Text + "\"关于您的电子名片的请求！";
                    }
                }
                else//收到主动发来的电子名片XML(本机仍作为服务器)
                {
                    //读取XML字串数据并进行解析,然后存储即可+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //与ConnectCallback类似
                   
                    
                    //在这里Show到的只是本一次接收到的，肯定不是全的XML
             //       MessageBox.Show(StrReceive);//test only//还是有点问题：如果图片文件比较大，就不行了收到 的是乱的！！

                    rwObj.memoryStream.Write(rwObj.readBytes, 0, count);
                    ////////////////既然是XML,就可能需要循环读取，因为可能比较长，一次收不完//////////////////////////////////
                    ///<重要！！！>、、、但这样的问题是如果连着发送两次，就会让XmlReceivedDirectly有多了的XML数据
                    ///解决办法还是字符串查找且替换，这次是只取XmlReceivedDirectly字符串中从最后一个"<?xml>"到末尾
                    /// 而且要在全部接受完后即else区域中进行这一段处理
                    /// 
                    /// 还有一种办法是在本接收函数ReadCallback的前一个函数即ConnectCallback中对XmlReceivedDirectly进行初始化，
                    /// 这样也可以。而且似乎是更合理的～，因为每次建立连接并接收都是用新的XmlreceivedDirectly
                    ///</重要！！！>
                    if (isExit == false && rwObj.netStream.DataAvailable)
                    {
                        rwObj.InitReadArray();//为什么要用新的空间进行存储呢？使用原来的不行？？
                        rwObj.netStream.BeginRead(rwObj.readBytes, 0, rwObj.readBytes.Length, new AsyncCallback(ReadCallback), rwObj);
                        //当isExit，则循环readCallback进行读取。通过无限循环，不断进行读取。。以后的函数方能被调用
                    }
                    else//已经data not availble了,收取完毕，可以用XML了
                    {
                        byte[] Bytes = rwObj.memoryStream.GetBuffer();
                        string XmlReceivedDirectly = Encoding.UTF8.GetString(Bytes);
                        int location = XmlReceivedDirectly.IndexOf("</ECard>");
                        XmlReceivedDirectly = XmlReceivedDirectly.Remove(location);
                        XmlReceivedDirectly += "</ECard>";

                        #region Test only 测试成功,
                        //FileStream fs = new FileStream("主动发来的XML.xml", FileMode.Create);
                        //StreamWriter sw = new StreamWriter(fs);
                        //sw.Write(XmlReceivedDirectly);//因为往往一次接收不玩，要分两次接收
                        //sw.Close(); fs.Close();
                        #endregion

                        //已经获取XML了，可以进行读取了

                        #region 用自定义类进行读取并输入到数据库中
                        ReadXmlIntoDB rxid;
                        string RemoteIpString = ((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString();
                        string RemoteUserName = GetItemByIpString(RemoteIpString).Text;
                        rxid = new ReadXmlIntoDB(XmlReceivedDirectly, RemoteIpString);
                        bool alreadyExists = CheckIsFriend(RemoteIpString);//CheckIsFriend仅仅从中文数据库中检查

                        //if (rxid.CheckResponse())//其实作为服务器的本机接收到的必定是XML名片，而不是拒绝
                        //{
                        if (!alreadyExists)//不存在记录
                        {
                            rxid.UpdateOrInsertIntoDB(false);

                            toolStripStatusLabel.Text = "用户\"" + RemoteUserName + "\"向您发来了名片，其名片已经在本地存储完毕！";

                            SetItemGroup(RemoteIpString, true);
                        }
                        else//存在记录
                        {
                            //删除相同记录，添加新纪录!
                            rxid.UpdateOrInsertIntoDB(true);
                            toolStripStatusLabel.Text = "用户\"" + RemoteUserName + "\"向您发来了名片，其名片已经在本地更新完毕！";

                        }
                        LogRoutine("用户\"" + RemoteUserName + "\"向您发来了名片，存储完毕！");
                        //}
                        //else//对方拒绝
                        //{
                        //    MessageBox.Show("抱歉，用户\""
                        //          + GetItemByIpString(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).Text + "\"拒绝了您关于其名片的请求!:(");
                        //}

                        rxid.Dispose();
                        #endregion


                    }
                }

                ////////////////循环读取(原来的、公共的)//////////////////////////////////
                //if (isExit == false&&rwObj.netStream.DataAvailable)
                //{
                //    rwObj.InitReadArray();//为什么要用新的空间进行存储呢？使用原来的不行？？
                //    rwObj.netStream.BeginRead(rwObj.readBytes, 0, rwObj.readBytes.Length, new AsyncCallback(ReadCallback), rwObj);
                //    //当isExit，则循环readCallback进行读取。通过无限循环，不断进行读取。。以后的函数方能被调用
                //}
            }
            catch (Exception ex)
            {//在这里应该把在本机作为服务器端上生成的代表客户端的TcpClient关闭，但没有做
                rwObj.Dispose();
                //IOException为
                //无法从传输连接中读取数据: 远程主机强迫关闭了一个现有的连接。
                //原因为
                //远程主机（客户端）的连接并读取的线程结束了，连接也随之丢弃
                //MessageBox.Show(ex.Message, "3S");//经常出现异常的地方。。。。。。。。。。。。。。。。
                //toolStripStatusLabel.Text = ex.Message; 非法跨线程
                //为客户端加了client.close()方法后，2.0版本仍然偶尔出现               
                LogError(ErrorLevel.三级一般错误, ex.Message+"\n<唐小宇：如果是连接异常则一般是正常的，一般是远程客户端接收进程执行完毕。>"); return;
                
            }
        }
        private void SendDeny(NetworkStream networkStream)
        {
            byte[] ResponseBuffer = System.Text.Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<DENY/>");
            networkStream.BeginWrite(ResponseBuffer, 0, ResponseBuffer.Length, new AsyncCallback(WriteCallback), networkStream);
        }
        /// <summary>V2.0+通过已经建立的TCP连接，向远程主机（客户端）发送本地用户的Card</summary>
        /// <param name="networkStream">向此流中写入（本机作为服务器，从客户端TcpClient对象获取的网络流）</param>
        private void SendMyCard(NetworkStream networkStream)
        {
            
            //从数据库中获取本人的电子名片并生成XML
            DataSet DS = new DataSet("ECard");

                using (oleDbCon = new OleDbConnection())
                {
                    try
                    {
                        oleDbCon.ConnectionString = connectionStr;

                        //这个OleDbDataAdapter填充到MyCard表时，表内记录将为2行：中文与英文
                        OleDbDataAdapter OleDbDA = new OleDbDataAdapter("select 名字,移动电话,固定电话,QQ,Email,网址,学历,职称,单位名称,地址,LOGO,个人简介 from LocalUser", oleDbCon);
                        //不能再用DataSet直接生成XML文件了
                        //因为要发送base64 因此要改为用XmlWriter一个一个写
                        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++2008052110点
                        
                        OleDbDA.Fill(DS, "MyCard");//这个表中应该有2条记录

                        //string XmlFileName = DS.Tables[0].Rows[0][0] + ".xml";
                        //已过时DS.WriteXml(XmlFileName);//---------------------------------------------------------------可修改不用生成文件
                        //??????????????????????很奇怪文件还没有生成在Explorer里显示的时候，怎么已经发送出去了？？？？？？？？？？
                        //byte[] ResponseBuffer = GetXmlFileBytes(XmlFileName);//"XML:" +
                        //---------V2.0之后----------仍然从DataSet中读取数据，然后使用XmlWriter写一个XML

                        #region 从DataSet中读取数据，然后使用XmlWriter写一个XML
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.IndentChars = "\t";
                        //这是XML文本中的编码标记
                        settings.Encoding = System.Text.Encoding.Default;//这里不管用，因为存在了StringBuilder里//改为用MemoryStream后可用
                        //编码有问题：：IE报错：“不支持从当前编码到指定编码的切换”UTF8与UTF-16(Unicode)
                        StringBuilder sbXml = new StringBuilder();//StringBuilder好像不好用啊,现用Stream类对象
                        //TextWriter ws = new StringWriter(sbXml);//竟然没用到？！！
                        
                        //MemoryStream mstream = new MemoryStream();
                        //TextWriter tw = new StreamWriter(mstream);
                        using (XmlWriter writer = XmlWriter.Create(sbXml, settings))//不用mstream,直接生成XML文件试试"直接生成的XML文件.xml"
                        {                         /*《重要》！！！直接生成的XML格式良好，可以直接由IE解析出来！！说明用MemoryStream就有问题！！！*/
                            writer.WriteStartDocument();
                            writer.WriteStartElement("ECard");//root
                            //----------------------
                            writer.WriteStartElement("MyCard");//<MyCard>中文
                            writer.WriteStartElement("名字");
                            writer.WriteString(DS.Tables["MyCard"].Rows[0][0].ToString());//DataSet中的表中并没有从数据库中获取ID字段
                            writer.WriteEndElement();
                            writer.WriteElementString("移动电话", DS.Tables["MyCard"].Rows[0][1].ToString());
                            writer.WriteElementString("固定电话", DS.Tables["MyCard"].Rows[0][2].ToString());
                            writer.WriteElementString("QQ", DS.Tables["MyCard"].Rows[0][3].ToString());
                            writer.WriteElementString("Email", DS.Tables["MyCard"].Rows[0][4].ToString());
                            writer.WriteElementString("网址", DS.Tables["MyCard"].Rows[0][5].ToString());
                            writer.WriteElementString("学历", DS.Tables["MyCard"].Rows[0][6].ToString());
                            writer.WriteElementString("职称", DS.Tables["MyCard"].Rows[0][7].ToString());
                            writer.WriteElementString("单位名称", DS.Tables["MyCard"].Rows[0][8].ToString());
                            writer.WriteElementString("地址", DS.Tables["MyCard"].Rows[0][9].ToString());
                            writer.WriteStartElement("LOGO");
                            string fileName = DS.Tables["MyCard"].Rows[0][10].ToString();
                            string FullFileName = CurrentDirectory + @"\images\" + fileName;
                            byte[] buffer = new byte[10240];
                            int bufferLength;
                            try
                            {
                                using (FileStream stream = new FileStream(FullFileName, FileMode.Open))
                                {
                                    while ((bufferLength = stream.Read(buffer, 0, buffer.Length)) > 0)
                                    {
                                        writer.WriteBase64(buffer, 0, bufferLength);
                                    }
                                }
                            }
                            catch { writer.WriteString("写入图片编码出错."); MessageBox.Show("写入图片编码出错.", "Chinese;服务器端"); }
                            writer.WriteEndElement();
                            writer.WriteElementString("个人简介", DS.Tables["MyCard"].Rows[0][11].ToString());
                            writer.WriteEndElement();//</Mycard>中文
                            //--------------------------------
                          //  writer.Flush();
                            //--------------------------------
                            writer.WriteStartElement("MyCard");//<MyCard>英文
                            writer.WriteStartElement("名字");
                            writer.WriteString(DS.Tables["MyCard"].Rows[1][0].ToString());//DataSet中的表中并没有从数据库中获取ID字段
                            writer.WriteEndElement();
                            writer.WriteElementString("移动电话", DS.Tables["MyCard"].Rows[1][1].ToString());
                            writer.WriteElementString("固定电话", DS.Tables["MyCard"].Rows[1][2].ToString());
                            writer.WriteElementString("QQ", DS.Tables["MyCard"].Rows[1][3].ToString());
                            writer.WriteElementString("Email", DS.Tables["MyCard"].Rows[1][4].ToString());
                            writer.WriteElementString("网址", DS.Tables["MyCard"].Rows[1][5].ToString());
                            writer.WriteElementString("学历", DS.Tables["MyCard"].Rows[1][6].ToString());
                            writer.WriteElementString("职称", DS.Tables["MyCard"].Rows[1][7].ToString());
                            writer.WriteElementString("单位名称", DS.Tables["MyCard"].Rows[1][8].ToString());
                            writer.WriteElementString("地址", DS.Tables["MyCard"].Rows[1][9].ToString());
                            string fileName2 = DS.Tables["MyCard"].Rows[1][10].ToString();
                            string FullFileName2 = CurrentDirectory + @"\images\" + fileName2;
                            byte[] buffer2 = new byte[10240];
                            int bufferLength2;
                            writer.WriteStartElement("LOGO");
                            try
                            {
                                using (FileStream stream2 = new FileStream(FullFileName2, FileMode.Open))
                                {
                                    while ((bufferLength2 = stream2.Read(buffer2, 0, buffer2.Length)) > 0)
                                    {
                                        writer.WriteBase64(buffer2, 0, bufferLength2);
                                    }
                                }
                            }
                            catch { writer.WriteString("写入图片编码出错."); MessageBox.Show("写入图片编码出错.", "English；服务器端"); }
                            writer.WriteEndElement();
                            writer.WriteElementString("个人简介", DS.Tables["MyCard"].Rows[1][11].ToString());
                            writer.WriteEndElement();//</MyCard>英文
                            //--------------------------------
                            writer.WriteEndElement();//</ECard>
                            writer.WriteEndDocument();
                           // writer.Flush();
                        }                   
                        #endregion

                        //要发送的都在sbXml里了
                        sbXml.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");//只因用的是StringBuilder..
                        byte[] ResponseBuffer = System.Text.Encoding.UTF8.GetBytes(sbXml.ToString());
                        //-----/肯定是这个MemoryStream有一个默认的长度比较大，或者它有个增幅超过了恰好的长度；这样留下了byte空格。。。。//----------/
                        //byte[] ResponseBuffer = mstream.GetBuffer();//new byte[mstream.Length]; // mstream.GetBuffer();//“返回创建此流的无符号字节数组”。什么意思？？                     
                         
                        //MessageBox.Show(ResponseBuffer.Length.ToString());
                        //mstream.Read(ResponseBuffer, 0,ResponseBuffer.Length);

                        //string strbuffer = System.Text.Encoding.UTF8.GetString(ResponseBuffer);
                        //strbuffer = strbuffer.TrimEnd(new char[] { ' ' });
                        //ResponseBuffer = System.Text.Encoding.UTF8.GetBytes(strbuffer);
                        #region Test only : 看生成的XML String是否正确
                        //string testStr = System.Text.Encoding.Default.GetString(ResponseBuffer, 0, ResponseBuffer.Length);
                        //MessageBox.Show(testStr, "testStr");
                        ////try
                        ////{
                        ////    FileStream fs = new FileStream("服务器端test生成.xml", FileMode.Create);
                        ////    fs.Write(ResponseBuffer, 0, ResponseBuffer.Length);
                        ////    fs.Close();
                        ////    string str = System.Text.Encoding.UTF8.GetString(ResponseBuffer);//这个str是完全正确的，没有后面的非法空格；生成的XML文件也是可以由IE解析的。！！
                        ////    //MessageBox.Show(str,"ff");//在这里str确实是正确的XML字符串，但不知道为什么mbox不显示。是不是string类型参数太大了？而且发现程序停滞了，但却不抛出异常！！
                        ////    //在这里设置断点后 ：ReadNetStream(TcpClient Client)里就会抛出异常，说连接已经断了 ～～～～～～～～～～～～～！！！！！！！！！！
                        ////}
                        ////catch (Exception ex) { MessageBox.Show(ex.Message, "测试XML错误！"); }
                        #endregion       

                        networkStream.BeginWrite(ResponseBuffer, 0, ResponseBuffer.Length, new AsyncCallback(WriteCallback), networkStream); networkStream.Flush();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message, "SendMyCard"); }
                    finally
                    {
                        if (DS != null) DS.Dispose();
                        if (oleDbCon != null) oleDbCon.Dispose();
                    }
                }
        }
        /// <summary>已过时：在V2.0中已经无用了
        /// </summary>
        /// <param name="XmlFileName"></param>
        /// <returns></returns>
        private byte[] GetXmlFileBytes(string XmlFileName)
        {
            FileInfo fileInfo = new FileInfo(XmlFileName);
            byte[] fileBuffer = new byte[fileInfo.Length];
            using (FileStream fileStream=fileInfo.OpenRead())
            {
                fileStream.Read(fileBuffer, 0, fileBuffer.Length);
            }
            return fileBuffer;
        }

        private void WriteCallback(IAsyncResult ar) 
        {
            try
            {
                NetworkStream ns = (NetworkStream)ar.AsyncState;
                ns.EndWrite(ar);// ns.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private delegate ListViewItem GetItemByIpStringCallback(string IpString);
        private GetItemByIpStringCallback getItemByIpStringCallback;
        /// <summary>输入Ip字符串，返回对应的ListViewItem</summary>
        /// <param name="IpString"></param>
        /// <returns></returns>
        private ListViewItem GetItemByIpString(string IpString)
        {
            if (listViewUsers.InvokeRequired)
            {
                getItemByIpStringCallback = new GetItemByIpStringCallback(GetItemByIpString);
                return (ListViewItem)this.listViewUsers.Invoke(getItemByIpStringCallback, IpString);
            }
            else
            {
                foreach (ListViewItem item in listViewUsers.Items)
                {
                    if (item.Tag.ToString() == IpString)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        #endregion as server


        #region -----------------as Client-----------------------

        /// <summary>REQUEST 陌生人请求本人的名片；UPDATE  朋友要求获取本人名片更新；
        /// </summary>
        private string StrClientCmd = "";

  
        private void SetTcpCmd(string StrCommand)
        {
            StrClientCmd = StrCommand;
        }
        /// <summary>主动与服务器建立连接，并接着发送StrCommandCmd里的指令字符串（ UPDATE 或 REQUEST ）。
        /// 作为有参数的线程函数，其参数须为object
        /// </summary>
        /// <param name="serverIP"></param>
        private void ConnectServerAndSendRequest(Object serverIP)//作为线程函数，其参数须为object
        {
            TcpClient client = new TcpClient(AddressFamily.InterNetwork);
           // client.SendBufferSize = 20000; client.ReceiveBufferSize = 20000;//GH
            //TcpClientDic.Add(serverIP.ToString(), client);

            //将事件设置为非终止状态
            allDoneClient.Reset();
            //开始一个对远程主机的异步请求
            client.BeginConnect((IPAddress)serverIP, PortTcpServer, new AsyncCallback(ConnectCallback), client);
            //阻塞当前线程,直到BeginConnect所建线程释放互斥对象
            allDoneClient.WaitOne();
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            //异步操作执行到此处说明调用BeginConnect已完成
            //解除阻塞
            allDoneClient.Set();
            try
            {
               TcpClient  client = (TcpClient)ar.AsyncState;//通过参数传递获取TcpCLient对象
                client.EndConnect(ar);//连接成功
                //客户端网络流networkStream
                NetworkStream networkStream = client.GetStream();
                //转换要发送到指令字符串
                byte[] byteCmd = System.Text.Encoding.UTF8.GetBytes(StrClientCmd);
                networkStream.BeginWrite(byteCmd, 0, byteCmd.Length, new AsyncCallback(WriteClientCallback),client);//所传递的参数并非NetworkStream而是client对象，client包含NetworkStream
                networkStream.Flush();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"4C");
            }
        }
        private void WriteClientCallback(IAsyncResult ar)
        {
            try
            {
                TcpClient client = (TcpClient)ar.AsyncState;
                client.GetStream().EndWrite(ar);

                ClientNString cns = new ClientNString(client);
            client.GetStream().BeginRead(cns.bytes, 0, cns.bytes.Length, new AsyncCallback(ReadClientCallback), cns);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "5C");
            }
        }
        //MemoryStream ms = new MemoryStream();
        private void ReadClientCallback(IAsyncResult ar)
        {
            ClientNString cns = (ClientNString)ar.AsyncState;
            TcpClient client = cns.client;
            NetworkStream ns = client.GetStream();
            try
            {
                ns.EndRead(ar);//IOException
            }
            catch (Exception ex) 
            {
                LogError(ErrorLevel.一级灾难错误, ex.Message); 
            }

            cns.memoryStream.Write(cns.bytes, 0, cns.bytes.Length);

           // ClientNString cns2 = new ClientNString(client);
            if (ns.DataAvailable)
            { 
                ns.BeginRead(cns.bytes, 0, cns.bytes.Length, new AsyncCallback(ReadClientCallback), cns);
            }
            else
            {
                byte[] buffer = cns.memoryStream.GetBuffer();
                cns.memoryStream.Dispose();
                string StrXml = Encoding.UTF8.GetString(buffer);
                //MessageBox.Show(StrXml);

                #region 去掉因为用MemoryStream而在尾部产生的空格 
                int i;
                if (StrXml.IndexOf("<DENY/>") == -1)//不是拒绝的XML信息/////////这里有个bug
                {
                    if ((i = StrXml.IndexOf("</ECard>",1)) == -1)//保存位置
                    {
                        string err = "生成的XML不完整，没有找到</ECard>";//在这里设置断点，如果出错，才会在这里中断
                        MessageBox.Show(err);
                        LogError(ErrorLevel.二级严重错误, err);

                        return;
                    }
                    StrXml = StrXml.Remove(i);//[未处理异常]：“StartIndex 不能小于 0。”原来是StrXml里面的XML被截断了-----可能原因是远程连接被关闭了；！！！！！！！！！！
                    StrXml += "</ECard>";
                }
                #endregion

                #region 生成XML文件，仅供测试：：这里的XML并没有去掉尾部的空格，因为他是直接从MemoryStream来的byte[]
                //FileStream fStream = new FileStream("刚刚接收到还没有连接数据库.xml", FileMode.Create);
                //fStream.Write(buffer, 0, buffer.Length);
                //fStream.Dispose();
                #endregion


                ReadXmlIntoDB rxid;
                rxid = new ReadXmlIntoDB(StrXml, ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                bool alreadyExists = CheckIsFriend(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());//CheckIsFriend仅仅从中文数据库中检查

                if (rxid.CheckResponse())//对方同意发送
                {
                    if (!alreadyExists)//不存在记录，本次操作是"REQUEST"
                    {
                        string NewName = rxid.UpdateOrInsertIntoDB(false);

                        toolStripStatusLabel.Text = "已经成功添加此人为好友，其名片已经在本地存储完毕！";

                        SetItemGroup(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), true);
                        if (NewName != "")
                        {
                            RenameItemText(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), NewName);
                        }
                    }
                    else//存在记录，本次操作是"UPDATE"
                    {
                        //删除相同记录，添加新纪录!
                        string NewName = rxid.UpdateOrInsertIntoDB(true);
                        if (NewName != "")
                        { 
                            RenameItemText(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(),NewName);
                        }

                        MessageBox.Show("恭喜！已经成功更新了此人的名片！", "更新成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        toolStripStatusLabel.Text = "更新成功，新的名片已经在本地存储完毕！";
                        LogRoutine("成功获取名片更新。");
                    }
                }
                else//对方拒绝
                {
                    MessageBox.Show("抱歉，用户\""
                          + GetItemByIpString(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).Text + "\"拒绝了您关于其名片的请求!:(");
                }

                rxid.Dispose();
               // ns.Dispose();//加此句CPU就100%了。～～～？？
            }
        }
        delegate void RenameItemTextCallback(string Ip, string newName);
        RenameItemTextCallback renameItemTextCallback;
        /// <summary>
        /// [允许跨线程]修改ListViewItem的Text属性
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="NewName"></param>
        private void RenameItemText(string Ip, string NewName)
        {
            if (this.InvokeRequired)
            {
                renameItemTextCallback = new RenameItemTextCallback(RenameItemText);
                this.Invoke(renameItemTextCallback, Ip, NewName);
            }
            else
            {
                GetItemByIpString(Ip).Text = NewName;
            }
        }

        ///// <summary> 同步读取包含XML的流并且输入数据库，完成添加好友. 同步读取不知道会不会有问题！-----》根据V2.2改正用异步读取后正常的结果，认为可能是有问题的。
        ///// </summary>
        //private void ReadNetStream(TcpClient Client)
        //{
        //    TcpClient client = Client;
        //    NetworkStream ns=client.GetStream();
        //    if (!isExit)
        //    {
        //        byte[] fileBuffer = new byte[1024];//如果不用while,小了不行如1024，只要大就行如4096.为什么？
        //        int count;
        //        MemoryStream mem = new MemoryStream();
                
        //                do
        //                {
        //                    count = ns.Read(fileBuffer, 0, fileBuffer.Length);
        //                    if (count > 0)
        //                        mem.Write(fileBuffer, 0, count);
        //                    else
        //                        goto sss;
        //                }while(ns.DataAvailable);

        //                sss:
        //        byte[] buffer = mem.GetBuffer();

        //        string StrXml;//以下四行是为了去掉因为使用MemoryStream而产生的空格！！！！！！！！！！！！！！！！！！！！！！
        //        StrXml = System.Text.Encoding.UTF8.GetString(buffer);

        //        mem.Dispose();
        //        //StrXml = sb.ToString();

        //        //sw.Dispose();


        //        int i;
        //        if (StrXml.IndexOf("<DENY/>") == -1)//不是拒绝的XML信息/////////这里有个bug
        //        {
        //            if ((i = StrXml.IndexOf("</ECard>")) == -1)
        //            {
        //                string err = "生成的XML不完整，没有找到</ECard>";//在这里设置断点，如果出错，才会在这里中断
        //                MessageBox.Show(err);
        //                LogError(ErrorLevel.二级严重错误, err);

        //                return;
        //            }
        //            StrXml = StrXml.Remove(i);//[未处理异常]：“StartIndex 不能小于 0。”原来是StrXml里面的XML被截断了-----可能原因是远程连接被关闭了；！！！！！！！！！！
        //            StrXml += "</ECard>";
        //        }   
                
               
        //        //XmlDocument xd = new XmlDocument();
        //        ////xd.Load(stream);//载入XML文件流为什么不行？？
        //        //xd.LoadXml(Str);//这样也行？？放在帮助类里了test only 0
        //        //----需要先查看是否存在相同记录------------
        //        ReadXmlIntoDB rxid;
        //        rxid = new ReadXmlIntoDB(StrXml, ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                
        //        bool alreadyExists = CheckIsFriend(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());//CheckIsFriend仅仅从中文数据库中检查

        //        if (rxid.CheckResponse())//对方同意发送
        //        {
        //            if (!alreadyExists)//不存在记录，本次操作是"REQUEST"
        //            {
        //                rxid.UpdateOrInsertIntoDB(false);

        //                toolStripStatusLabel.Text = "已经成功添加此人为好友，其名片已经在本地存储完毕！";

        //                SetItemGroup(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), true);
        //            }
        //            else//存在记录，本次操作是"UPDATE"
        //            {
        //                //删除相同记录，添加新纪录!
        //                rxid.UpdateOrInsertIntoDB(true);

        //                MessageBox.Show("恭喜！已经成功更新了此人的名片！", "更新成功",MessageBoxButtons.OK,MessageBoxIcon.Information);
        //                toolStripStatusLabel.Text = "更新成功，新的名片已经在本地存储完毕！";
        //                LogRoutine("成功获取名片更新。");
        //            }                    
        //        }
        //        else//对方拒绝
        //            MessageBox.Show("抱歉，用户\""
        //                + GetItemByIpString(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).Text + "\"拒绝了您关于其名片的请求!:(");

        //        rxid.Dispose();
                  
        //     //   MessageBox.Show(xd.DocumentElement.HasChildNodes.ToString());
        //      // MessageBox.Show(xd.DocumentElement.Name);//test only 0
        //     //   MessageBox.Show(dst.Tables[0].Rows[0][0].ToString(),"DataSet");//test only
        //    }
        //   // client.Close();//线程结束，关闭连接---（每当新建作为客户端的ConnectServerAndSendRequest线程，就会新建一个client）

        //    //如果加了client.Close(),请求执行后，CPU会占用率100%,去掉就没事。
        //    //还有，往往是刚开始调试的第一次名片请求能成功；马上再更新就XML不完整了!！！
        //    //是不是进程还没死，连接还没断，网络流里面还有数据没有清空？？？？


        //}

        private delegate void SetItemGroupCallback(string tagIp, bool FriendsGroup);
        private SetItemGroupCallback setItemGroupCallback;
        /// <summary>
        /// 找到含此tagIp的Tag的Item，并改变其Group为好友组
        /// </summary>
        /// <param name="tagIp"></param>
        private void SetItemGroup(string tagIp,bool FriendsGroup)
        {
            if(listViewUsers.InvokeRequired)
            {
                setItemGroupCallback = new SetItemGroupCallback(SetItemGroup);
                this.Invoke(setItemGroupCallback, tagIp,FriendsGroup);
            }
            else
            {
                //找到含此tagIp的Tag的Item，并改变其Group
                foreach (ListViewItem item in listViewUsers.Items)
                {
                    if ((string)item.Tag == tagIp)
                    {
                        item.Group = (FriendsGroup ? listViewUsers.Groups["listViewGroupFriends"] : listViewUsers.Groups["listViewGroupStrange"]);
                        //找到一个就return
                        return;
                    }
                }
                //MessageBox.Show(item.Group.ToString());
            }
        }
        //private void WriteIntoDB(string XmlFileName)
        //{
        //    MessageBox.Show(fileName, "File Name");
        //    FileInfo file = new FileInfo(XmlFileName);
        //    if (file.Exists)
        //    {
        //        MessageBox.Show("OK");
        //        while (file.Length==0)
        //        {
                    
        //        }
        //        MessageBox.Show(file.Length.ToString());
        //    }
        //}
        #endregion as client
        
        //用单独的线程发起连接请求，不会让主线程阻塞
        //
        private Thread thread;

        private void 更新名片GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv = listViewUsers.Visible ? listViewUsers : listViewFind;
            if (lv.SelectedItems.Count > 0)
            {
                if (lv.SelectedItems[0].SubItems[2].Text == "")
                {
                    //此人不在线，故不能更新
                    MessageBox.Show("对方不在线，无法向对方发送更新请求！");
                    return;
                }
                SetTcpCmd("UPDATE");
                thread = new Thread(new ParameterizedThreadStart(ConnectServerAndSendRequest));
                thread.Start(IPAddress.Parse(lv.SelectedItems[0].Tag.ToString()));
                //每个线程都用了client,networkStream，这会都修改,这样造成了早先建的client无法饮用;
                //应该为每个与服务器连接的TcpClient(本机上的)保存一个列表
            }
        }

        private void 请求此人的名片QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv = listViewUsers.Visible ? listViewUsers : listViewFind;
            if (lv.SelectedItems.Count > 0)
            {
                // SendTcpRequest("REQUEST",IPAddress.Parse( listViewUsers.SelectedItems[0].Tag.ToString()));
                SetTcpCmd("REQUEST");
                thread = new Thread(new ParameterizedThreadStart(ConnectServerAndSendRequest));
                thread.Start(IPAddress.Parse(lv.SelectedItems[0].Tag.ToString()));
            }
        }

        private void 删除名片SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv = listViewUsers.Visible == true ? listViewUsers : listViewFind;
            if (lv.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("请问您是否确定要删除您的好友\"" + lv.SelectedItems[0].SubItems[0].Text
                    + "\"的电子名片？\n如果您点击确定将直接删除数据！！", "删除确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                    == DialogResult.OK)
                {
                    try
                    {
                        oleDbCon = new OleDbConnection(connectionStr);
                        oleDbCon.Open();
                        oleDbCmd = new OleDbCommand("delete * from MyFriendsData where IP=" + "\"" + lv.SelectedItems[0].Tag.ToString() + "\"", oleDbCon);
                        oleDbCmd.ExecuteNonQuery();
                        oleDbCmd = new OleDbCommand("delete * from MyFriendsDataEn where IP=" + "\"" + lv.SelectedItems[0].Tag.ToString() + "\"", oleDbCon);
                        oleDbCmd.ExecuteNonQuery();
                        oleDbCon.Dispose();

                        //log
                        LogRoutine("已经在本地数据库中删除了\"" + lv.SelectedItems[0].Text + "\"的名片。");
                        //删除名片后立即删除对应Item，这样不依靠SetUserState函数的判断了：这对于如下情况是有用的:
                        //一个好友用户，上线后修改其IP的情况。因为修改IP后原来的IP就不能再发送广播了,本机（以及其他机器）就无法获取其状态了
                        lv.Items.Remove(listViewUsers.SelectedItems[0]);                        
                    }
                    catch (Exception ex)
                    {
                        LogError(ErrorLevel.一级灾难错误, "发生了不常见的错误：删除名片出错！");
                        if (MessageBox.Show(ex.Message, "删除错误", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                            删除名片SToolStripMenuItem_Click(sender, e);
                    }
                }
            }
        }

        private void textBoxFind_TextChanged(object sender, EventArgs e)
        {
            if (textBoxFind.Text.Length > 0)
            {
                this.listViewFind.Items.Clear();

                //搜索listviewusers里的item的text,然后clone(),再添加到listViewFind的items里

                this.listViewUsers.Visible = false;
                this.listViewFind.Visible = true;

                listViewFind.Groups["listViewGroupFind"].Header = "\"" + textBoxFind.Text + "\"的查找结果:";

                foreach (ListViewItem item in listViewUsers.Items)
                {
                    if(item.Text.ToLower().Contains(textBoxFind.Text.ToLower()))//使查找不区分字母大小写
                    {
                        ListViewItem CloneItem = (ListViewItem)item.Clone();
                        CloneItem.Group = listViewFind.Groups["listViewGroupFind"];
                        listViewFind.Items.Add(CloneItem);
                    }
                }
            }
            else//没有文字
            {
                this.listViewFind.Items.Clear();

                this.listViewUsers.Visible = true;
                this.listViewFind.Visible = false;                
            }
        }

        private void 向他发送我的名片XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv;
            lv = listViewUsers.Visible == true ? listViewUsers : listViewFind;

            if (lv.SelectedItems.Count > 0)
            {
                try
                {
                    if (lv.SelectedItems[0].SubItems[2].Text == "")//只有ListViewUsers才有~,------但现在不是了，ListViewFind也有了～
                    {
                        //此人不在线，故不能发送
                        MessageBox.Show("对方不在线，无法向对方发送名片！");
                        return;
                    }
                }
                catch { }
                Wizard wizard = new Wizard(lv.SelectedItems[0].Tag.ToString());
                wizard.ShowDialog();
            }
        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutForm()).ShowDialog();
        }

        private void 帮助主题ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("readme.txt"))
                System.Diagnostics.Process.Start("readme.txt");
        }

        private void 操作LToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            ListView lv;
            if (listViewUsers.Visible)
                lv = listViewUsers;
            else
                lv = listViewFind;

            if (lv.SelectedItems.Count == 0)
            {
                同右键ToolStripMenuItem.Enabled = false;
                更新名片GToolStripMenuItem1.Enabled = false;
                删除名片SToolStripMenuItem1.Enabled = false;
                请求此人的名片QToolStripMenuItem1.Enabled = false;
                向他发送我的名片XToolStripMenuItem1.Enabled = false;
                xML格式XToolStripMenuItem.Enabled = false;               
            }
            else//有选中
            {
                同右键ToolStripMenuItem.Enabled = true;
                更新名片GToolStripMenuItem1.Enabled = true;
                删除名片SToolStripMenuItem1.Enabled = true;
                请求此人的名片QToolStripMenuItem1.Enabled = true;
                向他发送我的名片XToolStripMenuItem1.Enabled = true;
                xML格式XToolStripMenuItem.Enabled = true;               
                
                    if (lv.SelectedItems[0].SubItems[1].Text=="")//是陌生人不是好友
                    {
                        同右键ToolStripMenuItem.Enabled = false;
                        更新名片GToolStripMenuItem1.Enabled = false;
                        删除名片SToolStripMenuItem1.Enabled = false;
                    }
               
            }
        }



        //日志记录
        /// <summary>
        /// 日常运行记录
        /// </summary>
        /// <param name="LogContent">日志内容</param>
        static public void LogRoutine(string LogContent)
        {
            try
            {
                //记录日志
                string time = DateTime.Now.ToString();
                System.IO.StreamWriter sw = System.IO.File.AppendText(CurrentDirectory + @"\log\" + "run.log");
                sw.WriteLine("【" + time + "】" + LogContent);
                sw.Dispose();
            }
            catch { }
        }
        /// <summary>
        /// 错误记录
        /// </summary>
        /// <param name="errorLevel">错误等级</param>
        /// <param name="LogContent">错误内容.(如果使用try...catch捕获，可以用Exception.Message)</param>
        static public void LogError(ErrorLevel errorLevel,string LogContent)
        {
            try
            {
                string time = DateTime.Now.ToString();
                System.IO.StreamWriter sw = System.IO.File.AppendText(CurrentDirectory + @"\log\" + "error_log.log");                
                sw.WriteLine("【" + time + "】" + "[" + errorLevel.ToString() + "]" + LogContent);
                sw.Dispose();
            }
            catch { }
        }
        public enum ErrorLevel { 一级灾难错误,二级严重错误,三级一般错误 }

        private void 软件运行日志RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(CurrentDirectory + "\\log\\" + "run.log");
            }
            catch { }
        }

        private void 错误日志EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(CurrentDirectory + "\\log\\" + "error_log.log");
            }
            catch { }
        }

        private void 开发日志KToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"H:\C# blog.txt");
            }
            catch (Exception)
            {
                
            }
        }

        private void timerAutoDock_Tick(object sender, EventArgs e)
        {
            //if(this.size
            if (this.Location.Y < 10) //this.Size = new Size(this.Size.Width, 3);
            {
                this.Size = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size;
                this.Location = new Point(0, 0);
            }
        }

        private void 批量备份为XML文件BToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 恢复名片数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listViewUsers_KeyPress(object sender, KeyPressEventArgs e)//按下Del键可删除
        {
            if (e.KeyChar == (char)Keys.Delete)
            {
                删除名片SToolStripMenuItem_Click(null, null);
                //MessageBox.Show("Test");
            }
          //  MessageBox.Show(((int)e.KeyChar).ToString());
        }

    }
}