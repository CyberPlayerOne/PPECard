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
        //�ֶ�����
        private OleDbConnection oleDbCon;
        private string connectionStr;//���ڹ��캯���н��г�ʼ��//��ʵConnectionStrҲ��ȫ������CurrentDirectory������Ϊ��̬���������ٸı䣬�Թ�ȫ��ʹ�ã���
        private OleDbCommand oleDbCmd;
        private OleDbDataReader oleDbCardReader;

        private int PortReceiveUdp = 8080;
        private UdpClient udpClient;

        public static int PortTcpServer = 9999;//as server
        private TcpListener listener;

        private string LocalUserIsOnline = "true";
        private string MyName;//���ڴ洢�����û������֣������Ͳ���Ҫÿ��UDP�㲥ʱ�������ݿ��ȡ���֣�ֻ��һ�����Ӿ��㹻

        //private TcpClient tclient;//as client  ������ArrayList����棡����ÿ���ͻ����̺߳�����������ʱ���½�һ��TcpClient��ͨ����������֮
        //private NetworkStream tnetworkStream; ͬ���ģ���NetworkStream����ͨ��tcpClient�����GetStream() ������ȡ��ConnectCallback(IAsyncResult ar)�����ﴫ�ݵ�ĩ������Ϊclient��ʹ��ص��������Ի�ȡNetworkStream����

        //System.Collections.ArrayList TcpClientList = new System.Collections.ArrayList();//��ǰ������ΪTCP Client������������������ʱ��TcpClient�����б�
        //Dictionary<string, TcpClient> TcpClientDic = new Dictionary<string, TcpClient>();//������Ϊ�ͻ��ˣ����ӵ�n�����������ڱ����ϵ�n��TcpClient����

        private bool isExit = false;
        

        //�����߳�ͬ������ʼ״̬��Ϊ����ֹ״̬��ʹ���ֶ����÷�ʽ
        private EventWaitHandle allDone = new EventWaitHandle(false, EventResetMode.ManualReset);//as server
        private EventWaitHandle allDoneClient = new EventWaitHandle(false, EventResetMode.ManualReset);//as client

        //����ż��ʹ�õı���
        bool isFirstTimeOffLine = true;//��267��ʹ��

        //��ǰ����Ŀ¼
        public static  string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

        //--CustomUIControls.TaskbarNotifier
        private TaskbarNotifier taskbarNotifier1;
        //--

        //--------------------------------------------------------------------
        void CloseClick(object obj, EventArgs ea)
        {
            taskbarNotifier1.Hide();
        }

        void TitleClick(object obj, EventArgs ea)//�Ѿ���ֹtitle�ɵ��
        {
            MessageBox.Show("Title was Clicked");
        }

        void ContentClick(object obj, EventArgs ea)
        {
           // MessageBox.Show("�����ʾ�������û�����Ƭ�أ�");
            ListViewItem item = GetItemByIpString(taskbarNotifier1.CurrentUserIpString);
           item.Selected = true;//��ѡ��
            //item.Focused = true;//��ѡ��,������ʹ��������¼�����
            //item.Checked = true;//Ҳ��ѡ��
            listViewUsers_ItemActivate(null, null);

        }



        public MainForm()
        {
            InitializeComponent();

            ///////////////////////
            InitListViewUsers();

            //--CustomUIControls.TaskbarNotifier ��ʼ�����ô���
            taskbarNotifier1 = new TaskbarNotifier();
            //
            //taskbarNotifier1.SetBackgroundBitmap(new Bitmap("skin.bmp"), Color.FromArgb(255, 0, 255));
            //taskbarNotifier1.SetCloseBitmap(new Bitmap("close.bmp"), Color.FromArgb(255, 0, 255), new Point(127, 8));
            //taskbarNotifier1.TitleRectangle = new Rectangle(40, 9, 70, 25);
            //taskbarNotifier1.ContentRectangle = new Rectangle(8, 41, 133, 68);
            //taskbarNotifier1.TitleClick += new EventHandler(TitleClick);
            //taskbarNotifier1.ContentClick += new EventHandler(ContentClick);
            //taskbarNotifier1.CloseClick += new EventHandler(CloseClick);

            //��skin3
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

            #region �����ݿ��л�ȡ�����û�������,������string����MyName��
            OleDbConnection ConGetName = new OleDbConnection();
            ConGetName.ConnectionString = connectionStr;
            ConGetName.Open();
            OleDbCommand CmdGetName = ConGetName.CreateCommand();
            CmdGetName.CommandText = "select ���� from LocalUser";
            MyName = (string)CmdGetName.ExecuteScalar();
            ConGetName.Dispose();
            #endregion

            
        }

        ////////////////////////
        /// <summary>
        /// ��ʼ��ListViewUsers,���ListView�ṹ
        /// </summary>
        private void InitListViewUsers()
        {
            ColumnHeader UserName = new ColumnHeader();
            UserName.Text = "��Ƭ�û���";
            UserName.Width = 130;
            ColumnHeader isFriend = new ColumnHeader();
            isFriend.Text = "���ѹ�ϵ";
            isFriend.Width = 100;
            ColumnHeader isOnline = new ColumnHeader();
            isOnline.Text = "����";

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
        /// ��̨�̺߳���:����UDP�㲥,��ȡ�����û�״̬��Ϣ.�ں�̨���н��н���.
        /// </summary>
        private void ReceiveUserStates()
        {
            //�ڱ���ָ���˿ڽ���UDP�㲥
            udpClient = new UdpClient(PortReceiveUdp);
            IPEndPoint remote = null;

             
            //����
            while (true)//��������ʾ,����������ʾ
            {
                //try
                //{
                    //�ر�UdpClientʱ�˾��쳣
                    byte[] bytes = udpClient.Receive(ref remote);
                    string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);

                    //�û�״̬�ַ����ĸ�ʽΪ
                    //�û���(string):���߷�(bool)
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

                    if (thisUserItem == null && isOnline)//����û���������ListViewUsers���������ߣ�����������ߣ��򲻹ܡ�
                    {//���Ǻ��ѣ������ߣ������ں����б��У�����������⣡�����ѽ��
                        //�����ListView�����޴��û������֮
                        AddListViewUserItem(UserName,remote,isFriend,true);//һ�㲻�Ǻ��ѣ������Ѿ������ݿ��м����ˣ�����������ǣ�
                                                                           //�ոռ�Ϊ���ѣ��Է������߱�Ϊ���ߣ����ֱ�Ϊ����  .��ʱ��Ҫ�޸ķ��飡 

                        taskbarNotifier1.CurrentUserIpString = remote.Address.ToString();
                        taskbarNotifier1.Show("����֪ͨ","�û�"+ UserName+ "�����ˣ�\n\n����˲鿴������Ƭ��", 500, 3000, 500);
                         //��־��¼
                            LogRoutine("�û�" + UserName + "����.");                        
                    }
                    else//��ListViewUsers��
                    {
                        
                        //����Ѿ���ListView���д��û�,�������Ƿ�����״̬
                        SetUserState(thisUserItem,isFriend, isOnline);//--
                    }

                //}
                ////catch(Exception ex)
                //{
                //        //MessageBox.Show(ex.Message);//test only
                //        throw;
                //    //�˳�ѭ��,�����߳�
                //    break;
                //}
            }
        }
        delegate ListViewItem CheckUserItemCallbackDelegate(string UserName, IPEndPoint remote, bool isOnline);//ί�еķ���ֵҪ�뱻ί�еķ����ķ���ֵһ�£�����������
        CheckUserItemCallbackDelegate CheckUserItemCallback;
        /// <summary>
        /// ʹ�ô��ݵĲ������������Ҽ��listViewItem�Ƿ��Ѿ���ListViewUsers�Ѿ�����
        /// </summary>
        /// <param name="UserName">User's Name string</param>
        /// <param name="remote">�Է���IPEndPoint,��ȡ��IP�ַ�����������Tag��</param>
        /// <param name="isOnline">boolֵ���Ƿ�����</param>
        private ListViewItem CheckUserItem(string UserName, IPEndPoint remote, bool isOnline)
        {
            if (listViewUsers.InvokeRequired&&this!=null)//�߳�������˿ؼ�//?????????????????????????????????????????????????���쳣
            {
                CheckUserItemCallback = new CheckUserItemCallbackDelegate(CheckUserItem);
               return (ListViewItem)this.listViewUsers.Invoke(CheckUserItemCallback, UserName,remote,true);//Invoke�����ķ���ֵ---���ص��Ǳ�ί�еķ����ķ���ֵ��������������
            }
            else
            {
               //ListViewItem lvi=(ListViewItem)listViewUsers.FindItemWithText(UserName);//Ҫʹ��Tag���Աȣ�����������
                //�½�һ��ListViewItem -> ��������ListViewItem,�쿴Tag���Ա�remote���Ip�ַ����������ͬ�򷵻�null����ͬ�򷵻��½���ListViewItem
                ListViewItem thisItem = new ListViewItem(UserName);
                thisItem.Tag = remote.Address.ToString();//��Tag������IP�ַ���
                thisItem.ImageIndex = 0;
                ListViewItem.ListViewSubItem lvsiFriend = new ListViewItem.ListViewSubItem();
                lvsiFriend.Text = "�½���ListViewItem���Ƿ����ѵ�SubItem�������򲻻���ʾ��";//!!û���ã���Ϊ�µ����ListViewItem����ʹ��
                ListViewItem.ListViewSubItem lvsiOnline = new ListViewItem.ListViewSubItem();
                lvsiOnline.Text = "��";
                thisItem.SubItems.Add(lvsiFriend);
                thisItem.SubItems.Add(lvsiOnline);
                //�����µ�ListViewItem��ϣ���ʼ�Ա�Tag
                foreach (ListViewItem item in listViewUsers.Items)
                {
                    if (item.Tag.Equals(thisItem.Tag))//�Ѿ���ʾ�˴���(�������ߣ����Ƿ����Ҫ�����ݿ���)//������"item.Tag==thisItem.Tag"ֱ�ӶԱȣ�������
                    {
                       // return thisItem;//����������ReceiveUserState�����������߷������壡(���õ�һ��δ����ListView��Items�����е�ListViewItem)
                        return item;//Ӧ�÷�������Ѿ������ListViewItem
                    }
                }
                return null;//û���ҵ�
            }
        }
        private delegate bool CheckIsFriendCallback(string ip);
        private CheckIsFriendCallback checkIsFriendCallback;
        /// <summary>
        /// [��������߳�]�ӱ������ݿ��м���IP��Ӧ�û��ǲ��Ǻ���
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
            else//!!!!!!!!!!����-������ݿ����Ƿ��д�����¼������
            {
                try
                {
                    OleDbConnection oc = new OleDbConnection(connectionStr);
                    oc.Open();
                    OleDbCommand oCmd = new OleDbCommand("select count(*) from MyFriendsData where IP=" + "\"" + ip + "\"", oc);
                    int intRt = (int)oCmd.ExecuteScalar();//������ݿ����ɴ�����intRtΪ1������Ϊ0
                    oc.Close();
                    return (intRt == 0 ? false : true);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR IN Function CheckIsFriend"); return false; }
                
            }
        }

        delegate void AddListViewUserItemCallbackDelegate(string UserName, IPEndPoint remote,bool isFriend,bool isOnline);
        AddListViewUserItemCallbackDelegate AddListViewUserItemCallback;
        /// <summary>
        /// ���ڴ��û�����ListViewUsers����������������Ӷ�ӦҪ���listViewItem,���߷��ò������ݡ�
        /// 
        /// ��Ҫ���ڣ�
        /// ��İ�������ߣ��Լ��ӱ������ݿ��еĺ���δ���ߣ�
        /// </summary>
        /// <param name="UserName">User's Name string</param>
        /// <param name="remote">�Է���IPEndPoint,��ȡ��IP�ַ�����������Tag��</param>
        /// <param name="isFriend">boolֵ���Ƿ����</param>
        /// <param name="isOnline">boolֵ���Ƿ�����</param>
        private void AddListViewUserItem(string UserName, IPEndPoint remote,bool isFriend,bool isOnline)
        {
            try
            {
                if (listViewUsers.InvokeRequired)//�߳�������˿ؼ�
                {
                    AddListViewUserItemCallback = new AddListViewUserItemCallbackDelegate(AddListViewUserItem);
                    this.Invoke(AddListViewUserItemCallback, UserName, remote,isFriend,isOnline);
                }
                else//����IPȷ��һ���û�
                {
                    listViewUsers.BeginUpdate();
                    ListViewItem thisUserItem = new ListViewItem(UserName);
                    thisUserItem.Tag = remote.Address.ToString();//��Tag������IP�ַ���
                    thisUserItem.ImageIndex = 0;

                    ListViewItem.ListViewSubItem lvsiFriend = new ListViewItem.ListViewSubItem();
                    lvsiFriend.Text = isFriend ? "��" : "";//!!!!!!�����ѷ� 
                    thisUserItem.Group = (isFriend ? listViewUsers.Groups["listViewGroupFriends"] : listViewUsers.Groups["listViewGroupStrange"]);//??
                    ListViewItem.ListViewSubItem lvsiOnline = new ListViewItem.ListViewSubItem();
                    if (isOnline)//���߷�
                    {
                        lvsiOnline.Text = "��";
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
        /// �Դ�����ListViewUsers���Item�����û��������,��������߲��Ҳ��Ǻ��������ɾ������Ŀ
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
                            listViewUsers.Items.Remove(item);//MSDN:��� ArrayList ������ָ�������� ArrayList ���ֲ��䡣�������쳣��
                            return;
                        }
                        //�����ߣ����Ǻ���
                        SetItemGroup(item.Tag.ToString(), true);
                        item.SubItems[1].Text = "��";//�Ǻ���
                        item.SubItems[2].Text = "";//������
                    }
                    else//����
                    {
                        if (isFriend) //�Ǻ��ѡ������ߡ�����ɾ����item���Ƴ�-�ɹ����������item��Ϊ�գ��ʴ˴������
                        { 
                            //
                            if (item.SubItems[2].Text == "")//ĿǰΪ""˵����������ǵ�һ�����ߣ�������ô�Ϳ��Ե���taskbarNotifier1
                            {
                                taskbarNotifier1.CurrentUserIpString = item.Tag.ToString();
                                taskbarNotifier1.Show("����֪ͨ", "�û�" + item.Text + "�����ˣ�\n\n����˲鿴������Ƭ��", 500, 3000, 500);
                            }
                            //

                            SetItemGroup(item.Tag.ToString(), true);
                            item.SubItems[1].Text = "��";//�Ǻ���
                            item.SubItems[2].Text = "��";//����
                        }
                        else//���Ǻ��ѣ���һ������������ո���ӶԷ�Ϊ���ѣ��Է����ߺ�������ʱ��
                        {
                            SetItemGroup(item.Tag.ToString(), false);
                            item.SubItems[1].Text = "";//���Ǻ���
                            item.SubItems[2].Text = "��";//����
                        }
                        //taskbarNotifier1.Show("����֪ͨ", item.Text+ "�����ˣ�", 500, 3000, 500);������һ����һ�飬ÿ��������
                    }
                }
            }
        }
        
        /// <summary>
        /// �㲥�����û�״̬��Ϣ
        /// </summary>
        private void LocalUserStateBroadcast(string isOnline)
        {
            UdpClient myUdpClient = new UdpClient();
            try
            {
                //�Զ��ṩ����IP�㲥��ַ
                IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, PortReceiveUdp);
                //�����ͺͽ��չ㲥���ݱ�
                myUdpClient.EnableBroadcast = true;

                ////test only
                //IPEndPoint iep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), PortReceiveUdp);                

                //���� �����ߵ���Ϣ
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(MyName + ":" + isOnline);//���޸�???????????????????????????
                //���������͹㲥(�û���:���߷�)
                myUdpClient.Send(bytes, bytes.Length, iep);
            }
            catch (Exception ex)
            {
                if (isFirstTimeOffLine)//�����û��ǵ�һ������
                {
                    isFirstTimeOffLine = false;

                    try
                    {
                        string time = DateTime.Now.ToString();
                        System.IO.StreamWriter sw = System.IO.File.AppendText(CurrentDirectory + @"\log\" + "error_log.log");
                        sw.WriteLine("��" + time + "��" + "[" + "һ�����" + "]" + ex.Message + "��������������,", "δ������!");
                        sw.Dispose();
                    }
                    catch { }

                    MessageBox.Show("�����Ϣ:" + ex.Message + "�������������ӣ�", "�޷�����");
                }//��һ�η���û����������֮��㲻��MessageBox����Ȼ���Է������ߵĹ㲥
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //����UDP�㲥���߳�
            Thread ThreadReceiveUdp = new Thread(new ThreadStart(ReceiveUserStates));
            ThreadReceiveUdp.IsBackground = true;
            ThreadReceiveUdp.Start();
            //��ʱ�㲥
            timerUdpBroadcast.Start();

            //����TCP������߳�
            Thread ThreadListenTcp = new Thread(new ThreadStart(AcceptTcpConnect));
            ThreadListenTcp.IsBackground = true;
            ThreadListenTcp.Start();

            //�����ݿ��л�ȡ�����Ѿ���Ϊ���ѵ��û�������ΪListViewItem��ʾ��ListView��
            ListViewAllFriendsFromLocalDataBase();

            //���û�д�Ŀ¼����֮�����ڴ��ͷ��
            if (!Directory.Exists( CurrentDirectory+ "\\Images")) Directory.CreateDirectory(CurrentDirectory+ "\\Images");
            if (!Directory.Exists(CurrentDirectory + "\\log")) Directory.CreateDirectory(CurrentDirectory + "\\log");
            if(!File.Exists(CurrentDirectory+ @"\log\" + "run.log")) File.Create(CurrentDirectory + @"\log\" + "run.log");
            string errLog =CurrentDirectory + @"\log\" + "error_log.log";
            if (!File.Exists(errLog)) File.Create(CurrentDirectory + @"\log\" + "error_log.log");
            try
            {
                //��¼��־
                string time = DateTime.Now.ToString();
                System.IO.StreamWriter sw = System.IO.File.AppendText(CurrentDirectory + @"\log\" + "run.log");
                sw.WriteLine("��" + time + "��" + "����ʼ����.");
                sw.Dispose();
            }
            catch { }
        }
        /// <summary>
        /// �����ݿ��л�ȡ�����Ѿ���Ϊ���ѵ��û�������ʾ��ListView��
        /// </summary>
        private void ListViewAllFriendsFromLocalDataBase()
        {
            try
            {
                //��while(DataReader.Read())ѭ����ȡ����ʾ���Ѵ���û�
                oleDbCon = new OleDbConnection();
                oleDbCon.ConnectionString = connectionStr;
                oleDbCon.Open();
                oleDbCmd = oleDbCon.CreateCommand();
                oleDbCmd.CommandText = "select * from MyFriendsData";
                oleDbCardReader = oleDbCmd.ExecuteReader();

                //��ȡ�û�������Ipд��ListViewItem��Tag��.��IP��Ϊ�ؼ���               
                //ListViewItem��SubItemҪ�������û������Ƿ���ѡ��Ƿ����ߡ������ֶ�
                while (oleDbCardReader.Read())
                {
                    string strUserName = (string)oleDbCardReader["����"];
                    string strIPAdress = (string)oleDbCardReader["IP"];
                    //ʹ��     AddListViewUserItem������
                    AddListViewUserItem(strUserName, new IPEndPoint(IPAddress.Parse(strIPAdress),65535),true, false);//�˿ں�����,�Ǻ���true
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
            LocalUserStateBroadcast(LocalUserIsOnline);//��ͣ�㲥���������ߵ���Ϣ
        }

        private void timerShowStatusLabelStatus_Tick(object sender, EventArgs e)
        {
            if (����ToolStripMenuItem1.Checked) toolStripStatusLabel.Text = "��ǰ������״̬...";
            else toolStripStatusLabel.Text = "��ǰ������״̬��";            
        }

       /// <summary>
       /// �޸�ListView��View���ԣ�����
       /// </summary>
        private void SetListViewViewType(object sender)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            ListView lv = (ListView)(listViewUsers.Visible ? listViewUsers : listViewFind);

            if (tsmi.Equals(XToolStripMenuItem))//ѡ������ϸ��Ϣ
            {
                XToolStripMenuItem.Checked = true;//�ֶ�ѡ�У������Զ�ѡ�У���������Checkboxʱ�ȽϺ��ã������һ�������Զ�ѡ�У�
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

        private void ����ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            tsmi.Checked = true;
            ����ToolStripMenuItem.Checked = false;

            toolStripStatusLabel.Text = "������...";
            //
            timerUdpBroadcast.Enabled = false;//��ֹͣ�㲥״̬
            LocalUserIsOnline = "true";//���޸�״̬
            timerUdpBroadcast.Enabled = true;//������㲥״̬
            LocalUserStateBroadcast(LocalUserIsOnline);//����            
        }

        private void ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
            tsmi.Checked = true;
            ����ToolStripMenuItem1.Checked = false;
            toolStripStatusLabel.Text = "�����ߡ�";
            //
            timerUdpBroadcast.Enabled = false;
            LocalUserIsOnline = "false";
            timerUdpBroadcast.Enabled = true;
            LocalUserStateBroadcast(LocalUserIsOnline);//����

        }

        private void �˳�XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void �޸�ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Application.Run(new ModifyMyCardForm());
            (new DisplayCardForm(true)).ShowDialog();
        }

        private void listViewUsers_ItemActivate(object sender, EventArgs e)
        {
            // ListView lv = (ListView)sender;���������sender�ǲ˵�����ToolStripMenuItem����ô����ת�����жϾͲ�����
            //����޸�Ϊ
            ListView lv = listViewUsers.Visible ? listViewUsers : listViewFind;
            if (lv.SelectedItems.Count > 0)
            {
                //����һ�β쿴����û���Ƭ(ʹ���Ҽ�����)
                foreach (ListViewItem eachItem in lv.SelectedItems)
                {
                    //string UserName = listViewUsers.SelectedItems[0].SubItems[0].Text;//û��Name����Ϊû�����öԴ˶��������!!!
                    string UserName = eachItem.SubItems[0].Text;//��һ��SubItem���û���
                    CardForm cf = new CardForm(true);

                    //����ListViewItem��Tag�е�IPֵ�����ݿ��ж�λһ���û����һ�ȡ����Ƭ��Ϣ
                    //string StrIpOfListViewItem = (string)listViewUsers.SelectedItems[0].Tag;
                    string StrIpOfListViewItem = (string)eachItem.Tag;
                    //MessageBox.Show(StrIpOfListViewItem);//test only
                    using (OleDbConnection ConGetCard = new OleDbConnection())//���������ñ����˽���ֶ�oleDbCon,�����½�����Щ����
                    {
                        try
                        {
                            ConGetCard.ConnectionString = connectionStr;
                            ConGetCard.Open();
                            OleDbCommand CmdGetCard = ConGetCard.CreateCommand();
                            CmdGetCard.CommandText = "select * from MyFriendsData where IP=" + "\"" + StrIpOfListViewItem + "\"";
                            oleDbCardReader = CmdGetCard.ExecuteReader();
                            if (oleDbCardReader.Read())//�����Ļ�Ӧ������ֻ��һ��
                            {
                                cf.name1 = oleDbCardReader["����"].ToString();
                                cf.cellphone1 = oleDbCardReader["�ƶ��绰"].ToString();
                                cf.telephone1 = oleDbCardReader["�̶��绰"].ToString();
                                cf.qq1 = oleDbCardReader["QQ"].ToString();
                                cf.email1 = oleDbCardReader[5].ToString();//���Ϊ0-base//oleDbCardReader["�����ʼ��ʻ���"].ToString();Ϊʲô���У���������
                                cf.website1 = oleDbCardReader["��ַ"].ToString();
                                //cf.diploma1�ݲ���ʾ
                                cf.title1 = oleDbCardReader["ְ��"].ToString();
                                cf.com1 = oleDbCardReader["��λ����"].ToString();
                                cf.location1 = oleDbCardReader["��ַ"].ToString();
                                cf.logo1 = CurrentDirectory+@"\images\"+ oleDbCardReader["LOGO"].ToString();//��Base64��������ģ�
                                //+++++++++++++++++++++++++++++++++++++����ҲҪ�ı�
                                cf.detail1 = oleDbCardReader["���˼��"].ToString();
                            }
                            else//û�������ݿ��м�¼���ʲ��Ǻ���
                            {
                                MessageBox.Show("�û�\"" + UserName
                                    + "\"�������ĺ���,���ܲ鿴��/���ĵ�����Ƭ��\n��������ͨ����" + UserName + "������������ȡ������Ƭ��",
                                    "�޷��鿴���˵���Ƭ",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);//
                                //ȡ������ӣ��Ƿ��İ���˵���Ƭ��������
                                //��ӣ�����Ϊ�Լ���Item����Ӧ����Ҫ�񣿣�
                                return;
                            }
                            //��ȡӢ����Ƭ
                            try
                            {
                                CmdGetCard = ConGetCard.CreateCommand();
                                CmdGetCard.CommandText = "select * from MyFriendsDataEn where IP=" + "\"" + StrIpOfListViewItem + "\"";
                                oleDbCardReader = CmdGetCard.ExecuteReader();
                                if (oleDbCardReader.Read())//�����Ļ�Ӧ������ֻ��һ��
                                {
                                    cf.name2 = oleDbCardReader["����"].ToString();
                                    cf.cellphone2 = oleDbCardReader["�ƶ��绰"].ToString();
                                    cf.telephone2 = oleDbCardReader["�̶��绰"].ToString();
                                    cf.qq2 = oleDbCardReader["QQ"].ToString();
                                    cf.email2 = oleDbCardReader[5].ToString();//���Ϊ0-base//oleDbCardReader["�����ʼ��ʻ���"].ToString();Ϊʲô���У���������
                                    cf.website2 = oleDbCardReader["��ַ"].ToString();
                                    //cf.diploma2�ݲ���ʾ
                                    cf.title2 = oleDbCardReader["ְ��"].ToString();
                                    cf.com2 = oleDbCardReader["��λ����"].ToString();
                                    cf.location2 = oleDbCardReader["��ַ"].ToString();
                                    cf.logo2 = CurrentDirectory + @"\images\" + oleDbCardReader["LOGO"].ToString();//��Base64��������ģ�
                                    //+++++++++++++++++++++++++++++++++++++����ҲҪ�ı�
                                    cf.detail2 = oleDbCardReader["���˼��"].ToString();
                                }
                            }
                            catch //����Ҳ�����¼��ȫ������Ϊ��
                            {
                                cf.name2 = cf.cellphone2 = cf.telephone2 = cf.qq2 = cf.email2 = cf.website2 = cf.title2 = cf.com2 = cf.location2 = cf.logo2 = cf.detail2 = ""; 
                            }
                        }
                        catch (OleDbException ex) { MessageBox.Show(ex.Message, "�������ݿⷢ������"); }
                        catch (InvalidOperationException ex) { MessageBox.Show(ex.Message, "�Ƿ�����"); }
                        catch (Exception ex) { MessageBox.Show(ex.Message, "��������"); }
                        finally
                        {
                            if (ConGetCard != null) { ConGetCard.Dispose(); }
                            oleDbCardReader.Dispose();
                        }
                    }

                    #region �ѹ�ʱ��V1.0����
                    //DisplayCardFrom�������ݳ�ʼ��
                    //otherGuyECardForm.Controls["buttonEditBlogAddress"].Visible = false;
                    //otherGuyECardForm.Text = UserName + "����Ƭ";
                    //otherGuyECardForm.Controls["buttonSave"].Text = "ȷ��";

                    //�޸�toolTips���ݣ���������������������������������������������������������������������������������������ContainerΪ�գ�����
                    //MessageBox.Show(otherGuyECardForm.Container.Components.Count.ToString());/*.SetToolTip(
                    //  ((PictureBox)otherGuyECardForm.Controls["pictureBoxDisplayPic"]), UserName + "����Ƭ");*/
                    //��ֹ�޸�otherGuyECardForm�����ؼ������ڹ��캯����ʵ��
                    #endregion
                    //cf.Text = cf.name1 + "(" + cf.name2 + ")" + "�ĵ�����Ƭ";
                    cf.InitControlTextChinese();
                    cf.Show();
                    cf.Controls["panel1"].Controls["buttonDetail"].Focus();
                }//foreach
            }//if
        }//listViewUsers_ItemActivate

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LocalUserIsOnline = "false";
            LocalUserStateBroadcast("false");//����
            udpClient.Close();

            //ʹ�й�TCP���̶߳��Զ�����
            isExit = true;
            allDone.Set();
            allDoneClient.Set();
        }

        private void �鿴��ƬCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //����һ�β쿴����û���Ƭ(���û��ѡ���κ��û��򲻻ᷴӦ)
            listViewUsers_ItemActivate(sender, e);            
        }

        private void ������ʾFToolStripMenuItem_Click(object sender, EventArgs e)//For ListViewUsers only
        {
            if (������ʾFToolStripMenuItem.Checked == true)
            {
                listViewUsers.ShowGroups = true;
            }
            else
            {
                listViewUsers.ShowGroups = false;
            }
        }



        private void XML��ʽToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv = listViewUsers.Visible ? listViewUsers : listViewFind;

             //һ��ֻ����һ���û�����ƬXML
            if (lv.SelectedItems.Count > 0)//ȷ��ѡ���û�
            {
                string StrIP = (string)lv.SelectedItems[0].Tag;
                OleDbDataAdapter OleDbDA;
                DataSet DS;
                string StrXmlPath;
                //�ô� IP��Ϊ�ؼ����ڱ������ݿ��н��в�ѯ����ȡ��DataSet��
                using (oleDbCon = new OleDbConnection())
                {
                    try
                    {
                        oleDbCon.ConnectionString = connectionStr;

                        OleDbDA = new OleDbDataAdapter("select ����,�ƶ��绰,�̶��绰,QQ,Email,��ַ,��ַ,LOGO,���˼�� from MyFriendsData where IP=" + "\"" + StrIP + "\"", oleDbCon);
                        //++++++++++++++++++++++++Ҳ������DataSet��
                        DS = new DataSet("ECard");
                        OleDbDA.Fill(DS, "MyFriendsData");
                        if (DS.Tables[0].Rows.Count == 0)
                        {
                            MessageBox.Show("���˲������ĺ��ѣ��ڱ������ݿ�û�д洢���˵���Ƭ��", "û���ҵ�");
                            return;
                        }
                        //��ȡҪ���XML��·��
                        folderBrowserDialog1.Description = "ѡ��һ��λ���Դ�Ŵ��˵���ƬXML:";
                        folderBrowserDialog1.ShowNewFolderButton = true;
                        if (folderBrowserDialog1.ShowDialog()==DialogResult.OK)
                        {
                            StrXmlPath = folderBrowserDialog1.SelectedPath;
                            DS.WriteXml(StrXmlPath +@"\"+ DS.Tables[0].Rows[0]["����"] + "����ƬXML.xml");//ֱ��ʹ��WriteXml�����Ĳ��ô���֮������logo���ļ���������Base64����
                            toolStripStatusLabel.Text="�Ѿ��ɹ�����"+DS.Tables[0].Rows[0]["����"] + "����ƬXML�ļ���";
                        }
                        DS.Dispose();
                        OleDbDA.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,"����");
                    }
                    finally
                    {
                        if (oleDbCon != null) oleDbCon.Dispose();//DataSet��DataAdapterҪ�ͷ��𣿣������������������������������������Ҳ���
                    }
                }
            }
            //else { MessageBox.Show(""); }//test only
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            ListView lv = listViewUsers.Visible ? listViewUsers : listViewFind;

            if(lv.SelectedItems.Count>0)//��ѡ��
            {
                this.contextMenuStrip1.Items["toolStripMenuItem2"].Visible = true;
                �����˵ĵ�����Ƭ����ΪXML��ʽToolStripMenuItem.Visible = true;
                toolStripMenuItem3.Visible = true;
                //���ѡ��Item����İ������
                if (lv.SelectedItems[0].Group == lv.Groups["listViewGroupStrange"])
                {
                    for (int i = 0; i < 3; i++)
                    {
                        this.contextMenuStrip1.Items[i].Visible = false;//����ֻ������������Ҽ��˵�
                    }
                    this.contextMenuStrip1.Items[3].Visible = true;
                    this.contextMenuStrip1.Items[8].Visible = false;//���ܵ���
                    this.contextMenuStrip1.Items[4].Visible = true;//����
                }
                else if (lv.SelectedItems[0].Group == lv.Groups["listViewGroupFriends"])
                {
                    for (int i = 0; i < 3; i++)
                    {
                        this.contextMenuStrip1.Items[i].Visible = true;//��ʾֻ������������Ҽ��˵�
                    }
                    this.contextMenuStrip1.Items[3].Visible = false;
                    this.contextMenuStrip1.Items[8].Visible = true;//�ɵ���
                    this.contextMenuStrip1.Items[4].Visible = true;//����
                }               
            }
            else//û��ѡ���κ�Item
            {
                for (int i = 0; i < 3; i++)
                {
                    this.contextMenuStrip1.Items[i].Visible = false;
                }
                this.contextMenuStrip1.Items[3].Visible = false;
                this.contextMenuStrip1.Items[8].Visible = false;//���ܵ���
                this.contextMenuStrip1.Items[4].Visible = false;//����

                this.contextMenuStrip1.Items["toolStripMenuItem2"].Visible = false;
                this.contextMenuStrip1.Items["������ʾFToolStripMenuItem"].Visible = false;
                �����˵ĵ�����Ƭ����ΪXML��ʽToolStripMenuItem.Visible = false;               
            }
            ������ʾFToolStripMenuItem.Visible = true;//һֱ�����Կ���������ʾ
        }


        ///////////////////////////////TCP���������////////////////////////////////////////

        #region-----------------as Server-----------------------
        /// <summary> �̺߳�������Ϊ�������ˣ�ÿ�����崴�������д��̣߳����߳�ͨ��while(isExit==false)ѭ�������ڴ���δ�˳��������һֱ���У���һֱ����TCP��������
        ///���ڽ���TCP����
        /// ��������ָ��Ϊ
        /// REQUEST İ���������˵���Ƭ
        /// UPDATE  ����Ҫ���ȡ������Ƭ����
        /// XMLCARD ���յ�XML�ļ�
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
                    //���뱾�߳�ִ�������ִ�������߳�
                    //���¼�����Ϊ����ֹ,�����߳�����
                    allDone.Reset();
                    //�����첽�������ʱ���õĻص�����
                    //��ʼһ���첽�������ܴ�������ӳ���
                    listener.BeginAcceptTcpClient(new AsyncCallback(AcceptTcpClientCallback), listener); 
                    //��ֹ��ǰ�߳�ֱ���յ��ź�
                    allDone.WaitOne();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "1S", MessageBoxButtons.OK, MessageBoxIcon.Error);//"1S"��ʾ��Ϊ�������˵ĵ�һ���������
                    break;
                }
            }
        }
        //string XmlReceivedDirectly = "";//Ҳ��������һ���������г�ʼ�������Է�ֹ��ʱ������XML���ݻ���
        private void AcceptTcpClientCallback(IAsyncResult ar)
        {
            //XmlReceivedDirectly = "";
            try
            {
                //�ܵ��ô˷�����˵�����յ�����������
                //���¼�״̬��Ϊ��ֹ״̬������ȴ��̣߳�AcceptTcpConnect()����������
                allDone.Set();
                TcpListener myListener = (TcpListener)ar.AsyncState;

                //�첽���մ�������ӣ��������µ�TcpClient������Զ������ͨ��
                TcpClient client = myListener.EndAcceptTcpClient(ar);//������Ϊ�������Ѿ��������ӣ�������client�����������ֶ�client�����������ֶ�client�Ǳ�����Ϊ�ͻ��˴�����TcpClient!
                ReadWriteObject rwObj = new ReadWriteObject(client);//һ�����Ӷ�Ӧһ��ReadWriteObject!!!!
                //ClientNString cns = new ClientNString(client);
                //��ʼһ���첽������ȡ�ַ���(NetworkStream�ķ���)
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
                    //Զ�������ͻ���Ҫ��õ������û���Ƭ�ĸ��£���ֻ��״̬��֪ͨ�����û���ֱ�ӽ��д���
                    toolStripStatusLabel.Text = "�û�\""+GetItemByIpString(((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString()).Text+"\"���ڻ�ȡ���ĵ�����Ƭ�ĸ���...";
                }
                else if (StrReceive.StartsWith("REQUEST"))
                {
                    //MessageBox.Show(StrReceive,"from client:REQUEST(test only)");//test only
                    DialogResult dr = MessageBox.Show("İ���û�\"" + GetItemByIpString(((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString()).Text + "\"�����ȡ���ĵ�����Ƭ�������Ƿ���Է����ͣ�", "�յ���Ƭ����",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information,MessageBoxDefaultButton.Button1);
                    // 
                    if (dr == DialogResult.Yes)
                    {
                        //MessageBox.Show("���ش���yes");
                        SendMyCard(rwObj.client.GetStream());//ͬ�ⷢ�͸���
                        toolStripStatusLabel.Text = "�Ѿ����û�\"" + GetItemByIpString(((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString()).Text + "\"���������ĵ�����Ƭ��";
                    }
                    else if (dr == DialogResult.No)
                    {
                        //   MessageBox.Show("���ش���No");
                        SendDeny(rwObj.client.GetStream());//���ͱ�ʾ�ܾ���XML
                        toolStripStatusLabel.Text = "�Ѿ��ܾ����û�\"" + GetItemByIpString(((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString()).Text + "\"�������ĵ�����Ƭ������";
                    }
                }
                else//�յ����������ĵ�����ƬXML(��������Ϊ������)
                {
                    //��ȡXML�ִ����ݲ����н���,Ȼ��洢����+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                    //��ConnectCallback����
                   
                    
                    //������Show����ֻ�Ǳ�һ�ν��յ��ģ��϶�����ȫ��XML
             //       MessageBox.Show(StrReceive);//test only//�����е����⣺���ͼƬ�ļ��Ƚϴ󣬾Ͳ������յ� �����ҵģ���

                    rwObj.memoryStream.Write(rwObj.readBytes, 0, count);
                    ////////////////��Ȼ��XML,�Ϳ�����Ҫѭ����ȡ����Ϊ���ܱȽϳ���һ���ղ���//////////////////////////////////
                    ///<��Ҫ������>��������������������������ŷ������Σ��ͻ���XmlReceivedDirectly�ж��˵�XML����
                    ///����취�����ַ����������滻�������ֻȡXmlReceivedDirectly�ַ����д����һ��"<?xml>"��ĩβ
                    /// ����Ҫ��ȫ���������else�����н�����һ�δ���
                    /// 
                    /// ����һ�ְ취���ڱ����պ���ReadCallback��ǰһ��������ConnectCallback�ж�XmlReceivedDirectly���г�ʼ����
                    /// ����Ҳ���ԡ������ƺ��Ǹ�����ġ�����Ϊÿ�ν������Ӳ����ն������µ�XmlreceivedDirectly
                    ///</��Ҫ������>
                    if (isExit == false && rwObj.netStream.DataAvailable)
                    {
                        rwObj.InitReadArray();//ΪʲôҪ���µĿռ���д洢�أ�ʹ��ԭ���Ĳ��У���
                        rwObj.netStream.BeginRead(rwObj.readBytes, 0, rwObj.readBytes.Length, new AsyncCallback(ReadCallback), rwObj);
                        //��isExit����ѭ��readCallback���ж�ȡ��ͨ������ѭ�������Ͻ��ж�ȡ�����Ժ�ĺ������ܱ�����
                    }
                    else//�Ѿ�data not availble��,��ȡ��ϣ�������XML��
                    {
                        byte[] Bytes = rwObj.memoryStream.GetBuffer();
                        string XmlReceivedDirectly = Encoding.UTF8.GetString(Bytes);
                        int location = XmlReceivedDirectly.IndexOf("</ECard>");
                        XmlReceivedDirectly = XmlReceivedDirectly.Remove(location);
                        XmlReceivedDirectly += "</ECard>";

                        #region Test only ���Գɹ�,
                        //FileStream fs = new FileStream("����������XML.xml", FileMode.Create);
                        //StreamWriter sw = new StreamWriter(fs);
                        //sw.Write(XmlReceivedDirectly);//��Ϊ����һ�ν��ղ��棬Ҫ�����ν���
                        //sw.Close(); fs.Close();
                        #endregion

                        //�Ѿ���ȡXML�ˣ����Խ��ж�ȡ��

                        #region ���Զ�������ж�ȡ�����뵽���ݿ���
                        ReadXmlIntoDB rxid;
                        string RemoteIpString = ((IPEndPoint)rwObj.client.Client.RemoteEndPoint).Address.ToString();
                        string RemoteUserName = GetItemByIpString(RemoteIpString).Text;
                        rxid = new ReadXmlIntoDB(XmlReceivedDirectly, RemoteIpString);
                        bool alreadyExists = CheckIsFriend(RemoteIpString);//CheckIsFriend�������������ݿ��м��

                        //if (rxid.CheckResponse())//��ʵ��Ϊ�������ı������յ��ıض���XML��Ƭ�������Ǿܾ�
                        //{
                        if (!alreadyExists)//�����ڼ�¼
                        {
                            rxid.UpdateOrInsertIntoDB(false);

                            toolStripStatusLabel.Text = "�û�\"" + RemoteUserName + "\"������������Ƭ������Ƭ�Ѿ��ڱ��ش洢��ϣ�";

                            SetItemGroup(RemoteIpString, true);
                        }
                        else//���ڼ�¼
                        {
                            //ɾ����ͬ��¼������¼�¼!
                            rxid.UpdateOrInsertIntoDB(true);
                            toolStripStatusLabel.Text = "�û�\"" + RemoteUserName + "\"������������Ƭ������Ƭ�Ѿ��ڱ��ظ�����ϣ�";

                        }
                        LogRoutine("�û�\"" + RemoteUserName + "\"������������Ƭ���洢��ϣ�");
                        //}
                        //else//�Է��ܾ�
                        //{
                        //    MessageBox.Show("��Ǹ���û�\""
                        //          + GetItemByIpString(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).Text + "\"�ܾ�������������Ƭ������!:(");
                        //}

                        rxid.Dispose();
                        #endregion


                    }
                }

                ////////////////ѭ����ȡ(ԭ���ġ�������)//////////////////////////////////
                //if (isExit == false&&rwObj.netStream.DataAvailable)
                //{
                //    rwObj.InitReadArray();//ΪʲôҪ���µĿռ���д洢�أ�ʹ��ԭ���Ĳ��У���
                //    rwObj.netStream.BeginRead(rwObj.readBytes, 0, rwObj.readBytes.Length, new AsyncCallback(ReadCallback), rwObj);
                //    //��isExit����ѭ��readCallback���ж�ȡ��ͨ������ѭ�������Ͻ��ж�ȡ�����Ժ�ĺ������ܱ�����
                //}
            }
            catch (Exception ex)
            {//������Ӧ�ð��ڱ�����Ϊ�������������ɵĴ���ͻ��˵�TcpClient�رգ���û����
                rwObj.Dispose();
                //IOExceptionΪ
                //�޷��Ӵ��������ж�ȡ����: Զ������ǿ�ȹر���һ�����е����ӡ�
                //ԭ��Ϊ
                //Զ���������ͻ��ˣ������Ӳ���ȡ���߳̽����ˣ�����Ҳ��֮����
                //MessageBox.Show(ex.Message, "3S");//���������쳣�ĵط���������������������������������
                //toolStripStatusLabel.Text = ex.Message; �Ƿ����߳�
                //Ϊ�ͻ��˼���client.close()������2.0�汾��Ȼż������               
                LogError(ErrorLevel.����һ�����, ex.Message+"\n<��С�����������쳣��һ���������ģ�һ����Զ�̿ͻ��˽��ս���ִ����ϡ�>"); return;
                
            }
        }
        private void SendDeny(NetworkStream networkStream)
        {
            byte[] ResponseBuffer = System.Text.Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<DENY/>");
            networkStream.BeginWrite(ResponseBuffer, 0, ResponseBuffer.Length, new AsyncCallback(WriteCallback), networkStream);
        }
        /// <summary>V2.0+ͨ���Ѿ�������TCP���ӣ���Զ���������ͻ��ˣ����ͱ����û���Card</summary>
        /// <param name="networkStream">�������д�루������Ϊ���������ӿͻ���TcpClient�����ȡ����������</param>
        private void SendMyCard(NetworkStream networkStream)
        {
            
            //�����ݿ��л�ȡ���˵ĵ�����Ƭ������XML
            DataSet DS = new DataSet("ECard");

                using (oleDbCon = new OleDbConnection())
                {
                    try
                    {
                        oleDbCon.ConnectionString = connectionStr;

                        //���OleDbDataAdapter��䵽MyCard��ʱ�����ڼ�¼��Ϊ2�У�������Ӣ��
                        OleDbDataAdapter OleDbDA = new OleDbDataAdapter("select ����,�ƶ��绰,�̶��绰,QQ,Email,��ַ,ѧ��,ְ��,��λ����,��ַ,LOGO,���˼�� from LocalUser", oleDbCon);
                        //��������DataSetֱ������XML�ļ���
                        //��ΪҪ����base64 ���Ҫ��Ϊ��XmlWriterһ��һ��д
                        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++2008052110��
                        
                        OleDbDA.Fill(DS, "MyCard");//�������Ӧ����2����¼

                        //string XmlFileName = DS.Tables[0].Rows[0][0] + ".xml";
                        //�ѹ�ʱDS.WriteXml(XmlFileName);//---------------------------------------------------------------���޸Ĳ��������ļ�
                        //??????????????????????������ļ���û��������Explorer����ʾ��ʱ����ô�Ѿ����ͳ�ȥ�ˣ�������������������
                        //byte[] ResponseBuffer = GetXmlFileBytes(XmlFileName);//"XML:" +
                        //---------V2.0֮��----------��Ȼ��DataSet�ж�ȡ���ݣ�Ȼ��ʹ��XmlWriterдһ��XML

                        #region ��DataSet�ж�ȡ���ݣ�Ȼ��ʹ��XmlWriterдһ��XML
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.IndentChars = "\t";
                        //����XML�ı��еı�����
                        settings.Encoding = System.Text.Encoding.Default;//���ﲻ���ã���Ϊ������StringBuilder��//��Ϊ��MemoryStream�����
                        //���������⣺��IE��������֧�ִӵ�ǰ���뵽ָ��������л���UTF8��UTF-16(Unicode)
                        StringBuilder sbXml = new StringBuilder();//StringBuilder���񲻺��ð�,����Stream�����
                        //TextWriter ws = new StringWriter(sbXml);//��Ȼû�õ�������
                        
                        //MemoryStream mstream = new MemoryStream();
                        //TextWriter tw = new StreamWriter(mstream);
                        using (XmlWriter writer = XmlWriter.Create(sbXml, settings))//����mstream,ֱ������XML�ļ�����"ֱ�����ɵ�XML�ļ�.xml"
                        {                         /*����Ҫ��������ֱ�����ɵ�XML��ʽ���ã�����ֱ����IE������������˵����MemoryStream�������⣡����*/
                            writer.WriteStartDocument();
                            writer.WriteStartElement("ECard");//root
                            //----------------------
                            writer.WriteStartElement("MyCard");//<MyCard>����
                            writer.WriteStartElement("����");
                            writer.WriteString(DS.Tables["MyCard"].Rows[0][0].ToString());//DataSet�еı��в�û�д����ݿ��л�ȡID�ֶ�
                            writer.WriteEndElement();
                            writer.WriteElementString("�ƶ��绰", DS.Tables["MyCard"].Rows[0][1].ToString());
                            writer.WriteElementString("�̶��绰", DS.Tables["MyCard"].Rows[0][2].ToString());
                            writer.WriteElementString("QQ", DS.Tables["MyCard"].Rows[0][3].ToString());
                            writer.WriteElementString("Email", DS.Tables["MyCard"].Rows[0][4].ToString());
                            writer.WriteElementString("��ַ", DS.Tables["MyCard"].Rows[0][5].ToString());
                            writer.WriteElementString("ѧ��", DS.Tables["MyCard"].Rows[0][6].ToString());
                            writer.WriteElementString("ְ��", DS.Tables["MyCard"].Rows[0][7].ToString());
                            writer.WriteElementString("��λ����", DS.Tables["MyCard"].Rows[0][8].ToString());
                            writer.WriteElementString("��ַ", DS.Tables["MyCard"].Rows[0][9].ToString());
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
                            catch { writer.WriteString("д��ͼƬ�������."); MessageBox.Show("д��ͼƬ�������.", "Chinese;��������"); }
                            writer.WriteEndElement();
                            writer.WriteElementString("���˼��", DS.Tables["MyCard"].Rows[0][11].ToString());
                            writer.WriteEndElement();//</Mycard>����
                            //--------------------------------
                          //  writer.Flush();
                            //--------------------------------
                            writer.WriteStartElement("MyCard");//<MyCard>Ӣ��
                            writer.WriteStartElement("����");
                            writer.WriteString(DS.Tables["MyCard"].Rows[1][0].ToString());//DataSet�еı��в�û�д����ݿ��л�ȡID�ֶ�
                            writer.WriteEndElement();
                            writer.WriteElementString("�ƶ��绰", DS.Tables["MyCard"].Rows[1][1].ToString());
                            writer.WriteElementString("�̶��绰", DS.Tables["MyCard"].Rows[1][2].ToString());
                            writer.WriteElementString("QQ", DS.Tables["MyCard"].Rows[1][3].ToString());
                            writer.WriteElementString("Email", DS.Tables["MyCard"].Rows[1][4].ToString());
                            writer.WriteElementString("��ַ", DS.Tables["MyCard"].Rows[1][5].ToString());
                            writer.WriteElementString("ѧ��", DS.Tables["MyCard"].Rows[1][6].ToString());
                            writer.WriteElementString("ְ��", DS.Tables["MyCard"].Rows[1][7].ToString());
                            writer.WriteElementString("��λ����", DS.Tables["MyCard"].Rows[1][8].ToString());
                            writer.WriteElementString("��ַ", DS.Tables["MyCard"].Rows[1][9].ToString());
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
                            catch { writer.WriteString("д��ͼƬ�������."); MessageBox.Show("д��ͼƬ�������.", "English����������"); }
                            writer.WriteEndElement();
                            writer.WriteElementString("���˼��", DS.Tables["MyCard"].Rows[1][11].ToString());
                            writer.WriteEndElement();//</MyCard>Ӣ��
                            //--------------------------------
                            writer.WriteEndElement();//</ECard>
                            writer.WriteEndDocument();
                           // writer.Flush();
                        }                   
                        #endregion

                        //Ҫ���͵Ķ���sbXml����
                        sbXml.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");//ֻ���õ���StringBuilder..
                        byte[] ResponseBuffer = System.Text.Encoding.UTF8.GetBytes(sbXml.ToString());
                        //-----/�϶������MemoryStream��һ��Ĭ�ϵĳ��ȱȽϴ󣬻������и�����������ǡ�õĳ��ȣ�����������byte�ո񡣡�����//----------/
                        //byte[] ResponseBuffer = mstream.GetBuffer();//new byte[mstream.Length]; // mstream.GetBuffer();//�����ش����������޷����ֽ����顱��ʲô��˼����                     
                         
                        //MessageBox.Show(ResponseBuffer.Length.ToString());
                        //mstream.Read(ResponseBuffer, 0,ResponseBuffer.Length);

                        //string strbuffer = System.Text.Encoding.UTF8.GetString(ResponseBuffer);
                        //strbuffer = strbuffer.TrimEnd(new char[] { ' ' });
                        //ResponseBuffer = System.Text.Encoding.UTF8.GetBytes(strbuffer);
                        #region Test only : �����ɵ�XML String�Ƿ���ȷ
                        //string testStr = System.Text.Encoding.Default.GetString(ResponseBuffer, 0, ResponseBuffer.Length);
                        //MessageBox.Show(testStr, "testStr");
                        ////try
                        ////{
                        ////    FileStream fs = new FileStream("��������test����.xml", FileMode.Create);
                        ////    fs.Write(ResponseBuffer, 0, ResponseBuffer.Length);
                        ////    fs.Close();
                        ////    string str = System.Text.Encoding.UTF8.GetString(ResponseBuffer);//���str����ȫ��ȷ�ģ�û�к���ķǷ��ո����ɵ�XML�ļ�Ҳ�ǿ�����IE�����ġ�����
                        ////    //MessageBox.Show(str,"ff");//������strȷʵ����ȷ��XML�ַ���������֪��Ϊʲômbox����ʾ���ǲ���string���Ͳ���̫���ˣ����ҷ��ֳ���ͣ���ˣ���ȴ���׳��쳣����
                        ////    //���������öϵ�� ��ReadNetStream(TcpClient Client)��ͻ��׳��쳣��˵�����Ѿ����� ����������������������������������������������
                        ////}
                        ////catch (Exception ex) { MessageBox.Show(ex.Message, "����XML����"); }
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
        /// <summary>�ѹ�ʱ����V2.0���Ѿ�������
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
        /// <summary>����Ip�ַ��������ض�Ӧ��ListViewItem</summary>
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

        /// <summary>REQUEST İ���������˵���Ƭ��UPDATE  ����Ҫ���ȡ������Ƭ���£�
        /// </summary>
        private string StrClientCmd = "";

  
        private void SetTcpCmd(string StrCommand)
        {
            StrClientCmd = StrCommand;
        }
        /// <summary>������������������ӣ������ŷ���StrCommandCmd���ָ���ַ����� UPDATE �� REQUEST ����
        /// ��Ϊ�в������̺߳������������Ϊobject
        /// </summary>
        /// <param name="serverIP"></param>
        private void ConnectServerAndSendRequest(Object serverIP)//��Ϊ�̺߳������������Ϊobject
        {
            TcpClient client = new TcpClient(AddressFamily.InterNetwork);
           // client.SendBufferSize = 20000; client.ReceiveBufferSize = 20000;//GH
            //TcpClientDic.Add(serverIP.ToString(), client);

            //���¼�����Ϊ����ֹ״̬
            allDoneClient.Reset();
            //��ʼһ����Զ���������첽����
            client.BeginConnect((IPAddress)serverIP, PortTcpServer, new AsyncCallback(ConnectCallback), client);
            //������ǰ�߳�,ֱ��BeginConnect�����߳��ͷŻ������
            allDoneClient.WaitOne();
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            //�첽����ִ�е��˴�˵������BeginConnect�����
            //�������
            allDoneClient.Set();
            try
            {
               TcpClient  client = (TcpClient)ar.AsyncState;//ͨ���������ݻ�ȡTcpCLient����
                client.EndConnect(ar);//���ӳɹ�
                //�ͻ���������networkStream
                NetworkStream networkStream = client.GetStream();
                //ת��Ҫ���͵�ָ���ַ���
                byte[] byteCmd = System.Text.Encoding.UTF8.GetBytes(StrClientCmd);
                networkStream.BeginWrite(byteCmd, 0, byteCmd.Length, new AsyncCallback(WriteClientCallback),client);//�����ݵĲ�������NetworkStream����client����client����NetworkStream
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
                LogError(ErrorLevel.һ�����Ѵ���, ex.Message); 
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

                #region ȥ����Ϊ��MemoryStream����β�������Ŀո� 
                int i;
                if (StrXml.IndexOf("<DENY/>") == -1)//���Ǿܾ���XML��Ϣ/////////�����и�bug
                {
                    if ((i = StrXml.IndexOf("</ECard>",1)) == -1)//����λ��
                    {
                        string err = "���ɵ�XML��������û���ҵ�</ECard>";//���������öϵ㣬��������Ż��������ж�
                        MessageBox.Show(err);
                        LogError(ErrorLevel.�������ش���, err);

                        return;
                    }
                    StrXml = StrXml.Remove(i);//[δ�����쳣]����StartIndex ����С�� 0����ԭ����StrXml�����XML���ض���-----����ԭ����Զ�����ӱ��ر��ˣ���������������������
                    StrXml += "</ECard>";
                }
                #endregion

                #region ����XML�ļ����������ԣ��������XML��û��ȥ��β���Ŀո���Ϊ����ֱ�Ӵ�MemoryStream����byte[]
                //FileStream fStream = new FileStream("�ոս��յ���û���������ݿ�.xml", FileMode.Create);
                //fStream.Write(buffer, 0, buffer.Length);
                //fStream.Dispose();
                #endregion


                ReadXmlIntoDB rxid;
                rxid = new ReadXmlIntoDB(StrXml, ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());

                bool alreadyExists = CheckIsFriend(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());//CheckIsFriend�������������ݿ��м��

                if (rxid.CheckResponse())//�Է�ͬ�ⷢ��
                {
                    if (!alreadyExists)//�����ڼ�¼�����β�����"REQUEST"
                    {
                        string NewName = rxid.UpdateOrInsertIntoDB(false);

                        toolStripStatusLabel.Text = "�Ѿ��ɹ���Ӵ���Ϊ���ѣ�����Ƭ�Ѿ��ڱ��ش洢��ϣ�";

                        SetItemGroup(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), true);
                        if (NewName != "")
                        {
                            RenameItemText(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), NewName);
                        }
                    }
                    else//���ڼ�¼�����β�����"UPDATE"
                    {
                        //ɾ����ͬ��¼������¼�¼!
                        string NewName = rxid.UpdateOrInsertIntoDB(true);
                        if (NewName != "")
                        { 
                            RenameItemText(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(),NewName);
                        }

                        MessageBox.Show("��ϲ���Ѿ��ɹ������˴��˵���Ƭ��", "���³ɹ�", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        toolStripStatusLabel.Text = "���³ɹ����µ���Ƭ�Ѿ��ڱ��ش洢��ϣ�";
                        LogRoutine("�ɹ���ȡ��Ƭ���¡�");
                    }
                }
                else//�Է��ܾ�
                {
                    MessageBox.Show("��Ǹ���û�\""
                          + GetItemByIpString(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).Text + "\"�ܾ�������������Ƭ������!:(");
                }

                rxid.Dispose();
               // ns.Dispose();//�Ӵ˾�CPU��100%�ˡ�����������
            }
        }
        delegate void RenameItemTextCallback(string Ip, string newName);
        RenameItemTextCallback renameItemTextCallback;
        /// <summary>
        /// [������߳�]�޸�ListViewItem��Text����
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

        ///// <summary> ͬ����ȡ����XML���������������ݿ⣬�����Ӻ���. ͬ����ȡ��֪���᲻�������⣡-----������V2.2�������첽��ȡ�������Ľ������Ϊ������������ġ�
        ///// </summary>
        //private void ReadNetStream(TcpClient Client)
        //{
        //    TcpClient client = Client;
        //    NetworkStream ns=client.GetStream();
        //    if (!isExit)
        //    {
        //        byte[] fileBuffer = new byte[1024];//�������while,С�˲�����1024��ֻҪ�������4096.Ϊʲô��
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

        //        string StrXml;//����������Ϊ��ȥ����Ϊʹ��MemoryStream�������Ŀո񣡣�����������������������������������������
        //        StrXml = System.Text.Encoding.UTF8.GetString(buffer);

        //        mem.Dispose();
        //        //StrXml = sb.ToString();

        //        //sw.Dispose();


        //        int i;
        //        if (StrXml.IndexOf("<DENY/>") == -1)//���Ǿܾ���XML��Ϣ/////////�����и�bug
        //        {
        //            if ((i = StrXml.IndexOf("</ECard>")) == -1)
        //            {
        //                string err = "���ɵ�XML��������û���ҵ�</ECard>";//���������öϵ㣬��������Ż��������ж�
        //                MessageBox.Show(err);
        //                LogError(ErrorLevel.�������ش���, err);

        //                return;
        //            }
        //            StrXml = StrXml.Remove(i);//[δ�����쳣]����StartIndex ����С�� 0����ԭ����StrXml�����XML���ض���-----����ԭ����Զ�����ӱ��ر��ˣ���������������������
        //            StrXml += "</ECard>";
        //        }   
                
               
        //        //XmlDocument xd = new XmlDocument();
        //        ////xd.Load(stream);//����XML�ļ���Ϊʲô���У���
        //        //xd.LoadXml(Str);//����Ҳ�У������ڰ���������test only 0
        //        //----��Ҫ�Ȳ鿴�Ƿ������ͬ��¼------------
        //        ReadXmlIntoDB rxid;
        //        rxid = new ReadXmlIntoDB(StrXml, ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                
        //        bool alreadyExists = CheckIsFriend(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());//CheckIsFriend�������������ݿ��м��

        //        if (rxid.CheckResponse())//�Է�ͬ�ⷢ��
        //        {
        //            if (!alreadyExists)//�����ڼ�¼�����β�����"REQUEST"
        //            {
        //                rxid.UpdateOrInsertIntoDB(false);

        //                toolStripStatusLabel.Text = "�Ѿ��ɹ���Ӵ���Ϊ���ѣ�����Ƭ�Ѿ��ڱ��ش洢��ϣ�";

        //                SetItemGroup(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString(), true);
        //            }
        //            else//���ڼ�¼�����β�����"UPDATE"
        //            {
        //                //ɾ����ͬ��¼������¼�¼!
        //                rxid.UpdateOrInsertIntoDB(true);

        //                MessageBox.Show("��ϲ���Ѿ��ɹ������˴��˵���Ƭ��", "���³ɹ�",MessageBoxButtons.OK,MessageBoxIcon.Information);
        //                toolStripStatusLabel.Text = "���³ɹ����µ���Ƭ�Ѿ��ڱ��ش洢��ϣ�";
        //                LogRoutine("�ɹ���ȡ��Ƭ���¡�");
        //            }                    
        //        }
        //        else//�Է��ܾ�
        //            MessageBox.Show("��Ǹ���û�\""
        //                + GetItemByIpString(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).Text + "\"�ܾ�������������Ƭ������!:(");

        //        rxid.Dispose();
                  
        //     //   MessageBox.Show(xd.DocumentElement.HasChildNodes.ToString());
        //      // MessageBox.Show(xd.DocumentElement.Name);//test only 0
        //     //   MessageBox.Show(dst.Tables[0].Rows[0][0].ToString(),"DataSet");//test only
        //    }
        //   // client.Close();//�߳̽������ر�����---��ÿ���½���Ϊ�ͻ��˵�ConnectServerAndSendRequest�̣߳��ͻ��½�һ��client��

        //    //�������client.Close(),����ִ�к�CPU��ռ����100%,ȥ����û�¡�
        //    //���У������Ǹտ�ʼ���Եĵ�һ����Ƭ�����ܳɹ��������ٸ��¾�XML��������!����
        //    //�ǲ��ǽ��̻�û�������ӻ�û�ϣ����������滹������û����գ�������


        //}

        private delegate void SetItemGroupCallback(string tagIp, bool FriendsGroup);
        private SetItemGroupCallback setItemGroupCallback;
        /// <summary>
        /// �ҵ�����tagIp��Tag��Item�����ı���GroupΪ������
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
                //�ҵ�����tagIp��Tag��Item�����ı���Group
                foreach (ListViewItem item in listViewUsers.Items)
                {
                    if ((string)item.Tag == tagIp)
                    {
                        item.Group = (FriendsGroup ? listViewUsers.Groups["listViewGroupFriends"] : listViewUsers.Groups["listViewGroupStrange"]);
                        //�ҵ�һ����return
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
        
        //�õ������̷߳����������󣬲��������߳�����
        //
        private Thread thread;

        private void ������ƬGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv = listViewUsers.Visible ? listViewUsers : listViewFind;
            if (lv.SelectedItems.Count > 0)
            {
                if (lv.SelectedItems[0].SubItems[2].Text == "")
                {
                    //���˲����ߣ��ʲ��ܸ���
                    MessageBox.Show("�Է������ߣ��޷���Է����͸�������");
                    return;
                }
                SetTcpCmd("UPDATE");
                thread = new Thread(new ParameterizedThreadStart(ConnectServerAndSendRequest));
                thread.Start(IPAddress.Parse(lv.SelectedItems[0].Tag.ToString()));
                //ÿ���̶߳�����client,networkStream����ᶼ�޸�,������������Ƚ���client�޷�����;
                //Ӧ��Ϊÿ������������ӵ�TcpClient(�����ϵ�)����һ���б�
            }
        }

        private void ������˵���ƬQToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ɾ����ƬSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv = listViewUsers.Visible == true ? listViewUsers : listViewFind;
            if (lv.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("�������Ƿ�ȷ��Ҫɾ�����ĺ���\"" + lv.SelectedItems[0].SubItems[0].Text
                    + "\"�ĵ�����Ƭ��\n��������ȷ����ֱ��ɾ�����ݣ���", "ɾ��ȷ��", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
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
                        LogRoutine("�Ѿ��ڱ������ݿ���ɾ����\"" + lv.SelectedItems[0].Text + "\"����Ƭ��");
                        //ɾ����Ƭ������ɾ����ӦItem������������SetUserState�������ж��ˣ������������������õ�:
                        //һ�������û������ߺ��޸���IP���������Ϊ�޸�IP��ԭ����IP�Ͳ����ٷ��͹㲥��,�������Լ��������������޷���ȡ��״̬��
                        lv.Items.Remove(listViewUsers.SelectedItems[0]);                        
                    }
                    catch (Exception ex)
                    {
                        LogError(ErrorLevel.һ�����Ѵ���, "�����˲������Ĵ���ɾ����Ƭ����");
                        if (MessageBox.Show(ex.Message, "ɾ������", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Retry)
                            ɾ����ƬSToolStripMenuItem_Click(sender, e);
                    }
                }
            }
        }

        private void textBoxFind_TextChanged(object sender, EventArgs e)
        {
            if (textBoxFind.Text.Length > 0)
            {
                this.listViewFind.Items.Clear();

                //����listviewusers���item��text,Ȼ��clone(),����ӵ�listViewFind��items��

                this.listViewUsers.Visible = false;
                this.listViewFind.Visible = true;

                listViewFind.Groups["listViewGroupFind"].Header = "\"" + textBoxFind.Text + "\"�Ĳ��ҽ��:";

                foreach (ListViewItem item in listViewUsers.Items)
                {
                    if(item.Text.ToLower().Contains(textBoxFind.Text.ToLower()))//ʹ���Ҳ�������ĸ��Сд
                    {
                        ListViewItem CloneItem = (ListViewItem)item.Clone();
                        CloneItem.Group = listViewFind.Groups["listViewGroupFind"];
                        listViewFind.Items.Add(CloneItem);
                    }
                }
            }
            else//û������
            {
                this.listViewFind.Items.Clear();

                this.listViewUsers.Visible = true;
                this.listViewFind.Visible = false;                
            }
        }

        private void ���������ҵ���ƬXToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView lv;
            lv = listViewUsers.Visible == true ? listViewUsers : listViewFind;

            if (lv.SelectedItems.Count > 0)
            {
                try
                {
                    if (lv.SelectedItems[0].SubItems[2].Text == "")//ֻ��ListViewUsers����~,------�����ڲ����ˣ�ListViewFindҲ���ˡ�
                    {
                        //���˲����ߣ��ʲ��ܷ���
                        MessageBox.Show("�Է������ߣ��޷���Է�������Ƭ��");
                        return;
                    }
                }
                catch { }
                Wizard wizard = new Wizard(lv.SelectedItems[0].Tag.ToString());
                wizard.ShowDialog();
            }
        }

        private void ����AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new AboutForm()).ShowDialog();
        }

        private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (System.IO.File.Exists("readme.txt"))
                System.Diagnostics.Process.Start("readme.txt");
        }

        private void ����LToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            ListView lv;
            if (listViewUsers.Visible)
                lv = listViewUsers;
            else
                lv = listViewFind;

            if (lv.SelectedItems.Count == 0)
            {
                ͬ�Ҽ�ToolStripMenuItem.Enabled = false;
                ������ƬGToolStripMenuItem1.Enabled = false;
                ɾ����ƬSToolStripMenuItem1.Enabled = false;
                ������˵���ƬQToolStripMenuItem1.Enabled = false;
                ���������ҵ���ƬXToolStripMenuItem1.Enabled = false;
                xML��ʽXToolStripMenuItem.Enabled = false;               
            }
            else//��ѡ��
            {
                ͬ�Ҽ�ToolStripMenuItem.Enabled = true;
                ������ƬGToolStripMenuItem1.Enabled = true;
                ɾ����ƬSToolStripMenuItem1.Enabled = true;
                ������˵���ƬQToolStripMenuItem1.Enabled = true;
                ���������ҵ���ƬXToolStripMenuItem1.Enabled = true;
                xML��ʽXToolStripMenuItem.Enabled = true;               
                
                    if (lv.SelectedItems[0].SubItems[1].Text=="")//��İ���˲��Ǻ���
                    {
                        ͬ�Ҽ�ToolStripMenuItem.Enabled = false;
                        ������ƬGToolStripMenuItem1.Enabled = false;
                        ɾ����ƬSToolStripMenuItem1.Enabled = false;
                    }
               
            }
        }



        //��־��¼
        /// <summary>
        /// �ճ����м�¼
        /// </summary>
        /// <param name="LogContent">��־����</param>
        static public void LogRoutine(string LogContent)
        {
            try
            {
                //��¼��־
                string time = DateTime.Now.ToString();
                System.IO.StreamWriter sw = System.IO.File.AppendText(CurrentDirectory + @"\log\" + "run.log");
                sw.WriteLine("��" + time + "��" + LogContent);
                sw.Dispose();
            }
            catch { }
        }
        /// <summary>
        /// �����¼
        /// </summary>
        /// <param name="errorLevel">����ȼ�</param>
        /// <param name="LogContent">��������.(���ʹ��try...catch���񣬿�����Exception.Message)</param>
        static public void LogError(ErrorLevel errorLevel,string LogContent)
        {
            try
            {
                string time = DateTime.Now.ToString();
                System.IO.StreamWriter sw = System.IO.File.AppendText(CurrentDirectory + @"\log\" + "error_log.log");                
                sw.WriteLine("��" + time + "��" + "[" + errorLevel.ToString() + "]" + LogContent);
                sw.Dispose();
            }
            catch { }
        }
        public enum ErrorLevel { һ�����Ѵ���,�������ش���,����һ����� }

        private void ���������־RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(CurrentDirectory + "\\log\\" + "run.log");
            }
            catch { }
        }

        private void ������־EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(CurrentDirectory + "\\log\\" + "error_log.log");
            }
            catch { }
        }

        private void ������־KToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void ��������ΪXML�ļ�BToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void �ָ���Ƭ����ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void listViewUsers_KeyPress(object sender, KeyPressEventArgs e)//����Del����ɾ��
        {
            if (e.KeyChar == (char)Keys.Delete)
            {
                ɾ����ƬSToolStripMenuItem_Click(null, null);
                //MessageBox.Show("Test");
            }
          //  MessageBox.Show(((int)e.KeyChar).ToString());
        }

    }
}