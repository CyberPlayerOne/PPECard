using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

//using System.Data;
using System.Data.OleDb;


namespace PPECard
{
    /// <summary>
    /// 选择信息；名片预览；发送或取消；完成后显示结果；
    /// </summary>
    enum Steps { step1, step2, step3, step4 ,Quit};
   
    public partial class Wizard : Form
    {
        private Steps CurrentStep;

        #region 中英文名片字段。在Form_Load事件中一次性获取所有数据，然后可以在“下一步”“中文”“English”按钮中多次使用
        public string name1, name2;
        public string cellphone1, cellphone2;
        public string telephone1, telephone2;
        public string qq1, qq2;
        public string email1, email2;
        public string website1, website2;
        public string diploma1, diploma2;
        public string title1, title2;
        public string com1, com2;
        public string location1, location2;
        public string logo1, logo2;
        public string detail1, detail2;
        #endregion

        /// <summary>
        /// 此二对象用于将图片文件预览完后能释放
        /// </summary>
        private Image Image1, Image2;

        //链接数据库的类 
        OleDbConnection ODCon;
        OleDbCommand ODCmd;
        OleDbDataReader ODDataReader;
        private string connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=LocalData.mdb";//最好使用System.IO.Directory.GetCurrentDirectory()！！！

        string RemoteServerIP;

       //------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServerIP">远程目标主机的IP</param>
        public Wizard(string ServerIP)
        {
            InitializeComponent();
            RemoteServerIP = ServerIP;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //窗体渐显---------
            this.Opacity = 0d;
            timer1.Start();

            //----------开始时为第一步-----------            
            CurrentStep = Steps.step1;
            //刚开始时没有上一步
            button1.Visible = false;

            label1.Font = new Font(label1.Font, FontStyle.Bold);
            pictureBox1.Visible = pictureBox2.Visible = pictureBox3.Visible = pictureBox4.Visible = false;
            //获取所有数据，放到私有字段中
            connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "\"" + MainForm.CurrentDirectory + "\\LocalData.mdb" + "\"";//数据库使用完全路径限定
            ODCon = new OleDbConnection();
            ODCon.ConnectionString = connectionStr;
            ODCon.Open();
            ODCmd = new OleDbCommand("Select * from LocalUser", ODCon);//必有两行记录
            ODDataReader = ODCmd.ExecuteReader();
            ODDataReader.Read();
            //中文
            name1 = ODDataReader[1].ToString(); ;//0列是ID
            cellphone1 = ODDataReader[2].ToString(); ;
            telephone1 = ODDataReader[3].ToString(); ;
            qq1 = ODDataReader[4].ToString(); ;
            email1 = ODDataReader[5].ToString(); ;
            website1 = ODDataReader[6].ToString(); ;
            switch (ODDataReader[7].ToString())
            {
                case "0": diploma1 = "保密"; break;
                case "1": diploma1 = "中学"; break;
                case "2": diploma1 = "大学本科"; break;
                case "3": diploma1 = "硕士"; break;
                case "4": diploma1 = "博士及以上"; break;

                default: diploma1 = ""; break;
            }
            title1 = ODDataReader[8].ToString();
            com1 = ODDataReader[9].ToString();
            location1 = ODDataReader[10].ToString();
            logo1 = MainForm.CurrentDirectory + @"\Images\" + ODDataReader[11].ToString();//数据库对应字段只是保存了文件名logo要改为全路径
            detail1 = ODDataReader[12].ToString();
            //English
            ODDataReader.Read();
            name2 = ODDataReader[1].ToString(); ;//0列是ID
            cellphone2 = ODDataReader[2].ToString(); ;
            telephone2 = ODDataReader[3].ToString(); ;
            qq2 = ODDataReader[4].ToString(); ;
            email2 = ODDataReader[5].ToString(); ;
            website2 = ODDataReader[6].ToString(); ;
            switch (ODDataReader[7].ToString())
            {
                case "0": diploma2 = "Secret"; break;
                case "1": diploma2 = "Middle School"; break;
                case "2": diploma2 = "College Degree"; break;
                case "3": diploma2 = "Master Degree"; break;
                case "4": diploma2 = "Doctor Degree"; break;

                default: diploma2 = ""; break;
            }
            title2 = ODDataReader[8].ToString();
            com2 = ODDataReader[9].ToString();
            location2 = ODDataReader[10].ToString();
            logo2 = MainForm.CurrentDirectory + @"\Images\"+ ODDataReader[11].ToString();
            detail2 = ODDataReader[12].ToString();
            //自此，中文英文名片已经全部获得
            ODDataReader.Dispose();
            ODCon.Dispose();
            //------------------------------panel们的位置初始化
            panel1.Dock = DockStyle.Fill;
            panel2.Dock = DockStyle.Fill;
            panel3.Dock = DockStyle.Fill;
            panel4.Dock = DockStyle.Fill;
            panel1.Visible = true;
            panel2.Visible = false; 
            panel3.Visible = false; 
            panel4.Visible = false;
            treeViewCardSelect.Dock = DockStyle.Bottom;
            treeViewCardSelect.Size = new Size(446, 366);
            foreach (Control control in panel2.Controls)
            {
                control.BackColor = Color.Transparent;
            }
            //选中中英文名片所有内容
            for (int i = 0; i < 2; i++)
            {
                treeViewCardSelect.Nodes[i].Checked = true;
                foreach (TreeNode node in treeViewCardSelect.Nodes[i].Nodes)
                {
                    node.Checked = true;
                }
            }
        }
        #region 窗体渐显
        void OpacityUp()
        {
            if (this.Opacity < 1.0d)
            {
                this.Opacity += 0.1d;                
            }
            else
            {
                this.timer1.Stop();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            OpacityUp();
        }
        #endregion 

        /// <summary>这个事件响应函数使treeView的使用反应像一般Windows中的一样
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewCardSelect_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //必须是人工激发才反应(键盘或鼠标)否则下面我用代码(root情况反映到child上)也能引发选中取消选中事件
            if (e.Action == TreeViewAction.ByKeyboard||e.Action==TreeViewAction.ByMouse)
            {
                //如果是两个根结点，其 选中、取消选中 都能反映到其子节点上
                if (e.Node.Name == "RootChinese" || e.Node.Name == "RootEnglish")//也可以用e.Node.Nodes.Count>0判断是个根节点
                {
                    if (e.Node.Checked)
                    {
                        foreach (TreeNode node in e.Node.Nodes)
                        {
                            node.Checked = true;
                        }
                    }
                    else
                    {
                        foreach (TreeNode node in e.Node.Nodes)
                        {
                            node.Checked = false;
                        }
                    }
                    if (e.Node.Name == "RootChinese" && e.Node.Checked != true)
                    {
                        e.Node.Checked = true;
                        e.Node.Nodes["Node01NameCn"].Checked = true;
                    }
                }
                else//子节点
                {
                    if (e.Node.Name == "Node01NameCn" && e.Node.Checked != true)
                    {
                        e.Node.Checked = true;
                    }
                    if (e.Node.Checked)
                    {
                        e.Node.Parent.Checked = true;
                    }
                    else//没有选中
                    {
                        //看看它的平级节点选中情况，如果都没有选中，则取消选中其parent
                        foreach (TreeNode node in e.Node.Parent.Nodes)
                        {
                            if (node.Checked)//至少有一个选中了
                            {
                                return;
                            }
                        }
                        //平级子节点都没有选中，才能执行到此处
                        e.Node.Parent.Checked = false;
                    }
                }
            }     
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentStep--;
            switch (CurrentStep)
            {
                case Steps.step1:
                    {
                        //---------------------------------------------------界面更改
                        //上一步按钮不可用
                        button1.Visible = false;
                        button2.Text = "下一步";//以防万一
                        button3.Visible = true;

                        pictureBox1.Visible = false;
                        pictureBox2.Visible = false;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;

                        label1.Font = new Font(label1.Font, FontStyle.Bold);
                        label2.Font = new Font(label2.Font, FontStyle.Regular);
                        label3.Font = new Font(label3.Font, FontStyle.Regular);
                        label4.Font = new Font(label4.Font, FontStyle.Regular);

                        panel1.Visible = true;
                        panel2.Visible = false;
                        panel3.Visible = false;
                        panel4.Visible = false;
                    }
                    break;
                case Steps.step2:
                    {                        
                        //---------------------------------------------------界面更改
                        //上一步按钮可用
                        button1.Visible = true;
                        button2.Text = "下一步";//以防万一
                        button3.Visible = true;

                        pictureBox1.Visible = true;
                        pictureBox2.Visible = false;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;

                        label1.Font = new Font(label1.Font, FontStyle.Regular);
                        label2.Font = new Font(label2.Font, FontStyle.Bold);
                        label3.Font = new Font(label3.Font, FontStyle.Regular);
                        label4.Font = new Font(label4.Font, FontStyle.Regular);

                        panel1.Visible = false;
                        panel2.Visible = true;
                        panel3.Visible = false;
                        panel4.Visible = false;
                    }
                    break;
                case Steps.step3://不可能事件，因为，因为倒退而到达step3是不被允许的
                    MessageBox.Show("这是不可能出现的步骤！", "这是不存在的世界...");
                    break;
                case Steps.step4://not possible!
                    MessageBox.Show("你进入了不可思议之境！", "!@#$%^&%");
                    break;
                default:
                    MessageBox.Show("This is MATRIX,and u,I guess,may be Neo?!", "THE ONE");
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CurrentStep++;
            switch (CurrentStep)
            {
                case Steps.step1:
                    {//加大括号只是为了易读
                        //不可能,不可能有++后才step1
                        MessageBox.Show("不可能的错误出现了!"+" button2 step1");
                    }
                    break;
                case Steps.step2:
                    //那么原来就是step1
                    {
                        //---------------------------------------------------界面更改
                        //上一步按钮可用
                        button1.Visible = true;
                        pictureBox1.Visible = true;
                        pictureBox2.Visible = false;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;

                        label1.Font = new Font(label1.Font, FontStyle.Regular);
                        label2.Font = new Font(label2.Font, FontStyle.Bold);
                        label3.Font = new Font(label3.Font, FontStyle.Regular);
                        label4.Font = new Font(label4.Font, FontStyle.Regular);

                        panel1.Visible = false;
                        panel2.Visible = true;
                        panel3.Visible = false;
                        panel4.Visible = false;
                        //--------------------------------------------------实际内容
                        //开始preview吧--------------------
                        //首先清空要所有label的Text
                        labelName.Text = "";
                        labelTitle.Text = "";
                        labelCompany.Text = "";
                        labelLocation.Text = "";
                        labelCellphone.Text = "";
                        labelTelephone.Text = "";
                        labelQQ.Text = "";
                        labelEmail.Text = "";
                        linkLabelWebsite.Text = "";
                        pictureBoxLogo.Image = null;
                        //其次要获取选取了哪一些节点
                        //对选中的要显示，未选中的不显示
                        //赋值然后显示，
                        //下一步按钮默认显示中文
                        SetLabelNFormTextIn2Languages(true);
                        GetCheckedNodeAndShowThem(true);

                        
                    }
                    break;
                case Steps.step3:
                    {
                        //---------------------------------------------------界面更改
                        button2.Text = "发送";

                        pictureBox1.Visible = true;
                        pictureBox2.Visible = true;
                        pictureBox3.Visible = false;
                        pictureBox4.Visible = false;

                        label1.Font = new Font(label1.Font, FontStyle.Regular);
                        label2.Font = new Font(label2.Font, FontStyle.Regular);
                        label3.Font = new Font(label3.Font, FontStyle.Bold);
                        label4.Font = new Font(label4.Font, FontStyle.Regular);

                        panel1.Visible = false;
                        panel2.Visible = false;
                        panel3.Visible = true;
                        panel4.Visible = false;
                        //--------------------------------------------------实际内容
                        
                        
                    }
                    break;
                case Steps.step4://点击发送后 到(运行)此步，故发送应在此步进行
                    {
                        //---------------------------------------------------界面更改
                        button1.Visible = false;
                        button2.Text = "请等待...";
                        button2.Enabled = false;//等发送完后才可用
                        button3.Visible = false;
                        labelResultText.Text="正在发送中，请等待...";

                        pictureBox1.Visible = true;
                        pictureBox2.Visible = true;
                        pictureBox3.Visible = true;
                        pictureBox4.Visible = true;

                        label1.Font = new Font(label1.Font, FontStyle.Regular);
                        label2.Font = new Font(label2.Font, FontStyle.Regular);
                        label3.Font = new Font(label3.Font, FontStyle.Regular);
                        label4.Font = new Font(label4.Font, FontStyle.Bold);

                        panel1.Visible = false;
                        panel2.Visible = false;
                        panel3.Visible = false;
                        panel4.Visible = true;
                        //完成,显示发送结果

                        //要生成XML，并发送    
                        //因此在这里要释放图像文件句柄
                        if (Image1 != null) Image1.Dispose();//还有一个问题/异常时：如果点中文按钮，又点英文按钮就会有一个没有引用的Image对象，这样就无法释放
                        if (Image2 != null) Image2.Dispose();
                        //首先获取选中了哪些节点，生成名片，然后向指定的IPEndPoint发送之

                        XmlString = GetAllCheckedNodesAndGenerateXml();
                        ConnectServerAndSendXml(RemoteServerIP);

                        button2.Text = "完成";
                        button2.Enabled = true;
                        button2.Focus();
                        labelResultText.Text = "向" + RemoteServerIP + "发送名片成功！";
                        labelResultText.Visible = true;
                    }
                    break;
                default://Quit
                    if (Image1 != null) Image1.Dispose();
                    if (Image2 != null) Image2.Dispose();
                    this.Close();
                    this.Dispose();
                    GC.Collect();
                    break;
            }
        }

        /// <summary>根据TreeView控件的选中情况，来设置用于名片预览的panel的label们的visible属性
        /// </summary>
        /// <param name="IsChinese"></param>
        private void GetCheckedNodeAndShowThem(bool IsChinese)
        {
            int i = 0;
            i = (IsChinese ? 0 : 1);
            //至少有一个选中，即为名字
                labelName.Visible = treeViewCardSelect.Nodes[i].Nodes[0].Checked;
                labelCellphone.Visible = label6.Visible = treeViewCardSelect.Nodes[i].Nodes[1].Checked;
                labelTelephone.Visible = label8.Visible = treeViewCardSelect.Nodes[i].Nodes[2].Checked;
                labelQQ.Visible = label10.Visible = treeViewCardSelect.Nodes[i].Nodes[3].Checked;
                labelEmail.Visible = label12.Visible = treeViewCardSelect.Nodes[i].Nodes[4].Checked;
                linkLabelWebsite.Visible = label14.Visible = treeViewCardSelect.Nodes[i].Nodes[5].Checked;
                //学历目前先不显示
                labelTitle.Visible = treeViewCardSelect.Nodes[i].Nodes[7].Checked;
                labelCompany.Visible = treeViewCardSelect.Nodes[i].Nodes[8].Checked;
                labelLocation.Visible = label7.Visible = treeViewCardSelect.Nodes[i].Nodes[9].Checked;
                pictureBoxLogo.Visible = treeViewCardSelect.Nodes[i].Nodes[10].Checked;
                //个人简介或详细信息在这里先不显示//即演示名片不能翻页
            
            //if it is English,既然使用了此函数，就一定要有选中
            
        }
        /// <summary>
        /// 设置窗体标记中的语言，包括内容字段label,提示label,和图片地址路径
        /// </summary>
        /// <param name="isChinese">中文否</param>
        private void SetLabelNFormTextIn2Languages(bool isChinese)
        {
            //赋值label
            //改变Form语言（无用的label文本）
            if (isChinese)
            {
                label7.Text = "地址:";
                label6.Text = "手机:";
                label8.Text = "固定电话:";
                label10.Text = "QQ:";
                label12.Text = "电邮:";
                label14.Text = "网址:";

                //设置内容label显示
                labelName.Text = name1;
                labelTelephone.Text = telephone1;
                labelCellphone.Text = cellphone1;
                labelQQ.Text = qq1;
                labelEmail.Text = email1;
                linkLabelWebsite.Text = website1;
                //没有diploma
                labelTitle.Text = title1;
                labelCompany .Text= com1;
                labelLocation.Text = location1;
                if (logo1 != null && File.Exists(logo1))
                {
                    pictureBoxLogo.Image = Image1 = Image.FromFile(logo1);
                }
                //不显示Detail
            }
            else
            {
                label7.Text = "LOCATION:";
                label6.Text = "MOB:";
                label8.Text = "TEL:";
                label10.Text = "QQ:";
                label12.Text = "EMAIL:";
                label14.Text = "WEBSITE:";
                //设置内容label显示
                labelName.Text = name2;
                labelTelephone.Text = telephone2;
                labelCellphone.Text = cellphone2;
                labelQQ.Text = qq2;
                labelEmail.Text = email2;
                linkLabelWebsite.Text = website2;
                //没有diploma
                labelTitle.Text = title2;
                labelCompany.Text = com2;
                labelLocation.Text = location2;
                if (logo2 != null && File.Exists(logo2))
                {
                    pictureBoxLogo.Image = Image2 = Image.FromFile(logo2);
                }
                //catch (Exception ex) { MessageBox.Show(ex.Message); }
                //不显示Detail
            }
        }
        /// <summary>
        /// 应由GetAllCheckedNodesAndGenerateXml()返回值
        /// </summary>
        private string XmlString;
        /// <summary>
        /// 获取中文与英文选中的节点，选中者添加入XML，为选中者添加"",然后返回生成的XML格式化 String
        /// </summary>
        /// <returns>返回生成的XML格式化 String</returns>
        private string GetAllCheckedNodesAndGenerateXml()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            //这是XML文本中的编码标记
            settings.Encoding = System.Text.Encoding.Default;//这里不管用，因为存在了StringBuilder里//改为用MemoryStream后可用
            //编码有问题：：IE报错：“不支持从当前编码到指定编码的切换”UTF8与UTF-16(Unicode)
            StringBuilder sbXml = new StringBuilder();//StringBuilder好像不好用啊,现用Stream类对象
           // TextWriter ws = new StringWriter(sbXml);            
            
            using (XmlWriter writer = XmlWriter.Create(sbXml, settings))//不用mstream,直接生成XML文件试试"直接生成的XML文件.xml"
            {                         /*《重要》！！！直接生成的XML格式良好，可以直接由IE解析出来！！说明用MemoryStream就有问题！！！*/
                writer.WriteStartDocument();
                writer.WriteStartElement("ECard");//root
                //----------------------//没选中就写空格
                writer.WriteStartElement("MyCard");//<MyCard>中文
                writer.WriteElementString("名字", (treeViewCardSelect.Nodes[0].Nodes[0].Checked ? name1 : " "));
                writer.WriteElementString("移动电话", (treeViewCardSelect.Nodes[0].Nodes[1].Checked ? cellphone1 : " "));
                writer.WriteElementString("固定电话", (treeViewCardSelect.Nodes[0].Nodes[2].Checked ? telephone1 : " "));
                writer.WriteElementString("QQ", (treeViewCardSelect.Nodes[0].Nodes[3].Checked ? qq1 : " "));
                writer.WriteElementString("Email", (treeViewCardSelect.Nodes[0].Nodes[4].Checked ? email1 : " "));
                writer.WriteElementString("网址", (treeViewCardSelect.Nodes[0].Nodes[5].Checked ? website1 : " "));
                writer.WriteElementString("学历", (treeViewCardSelect.Nodes[0].Nodes[6].Checked ? diploma1 : " "));
                writer.WriteElementString("职称", (treeViewCardSelect.Nodes[0].Nodes[7].Checked ? title1 : " "));
                writer.WriteElementString("单位名称", (treeViewCardSelect.Nodes[0].Nodes[8].Checked ? com1 : " "));
                writer.WriteElementString("地址", (treeViewCardSelect.Nodes[0].Nodes[9].Checked ? location1 : " "));
                writer.WriteStartElement("LOGO");
                if (treeViewCardSelect.Nodes[0].Nodes[10].Checked)
                {
                    string FullFileName = logo1;//logo1保存了全路径
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
                    catch { writer.WriteString("写入图片编码出错."); MessageBox.Show("写入图片编码出错.", "Chinese;服务器端,在Wizard"); }
                }
                else
                { 
                    writer.WriteString(" "); 
                }
                writer.WriteEndElement();
                writer.WriteElementString("个人简介", (treeViewCardSelect.Nodes[0].Nodes[11].Checked ? detail1 : " "));
                writer.WriteEndElement();//</Mycard>中文
                //--------------------------------
              //  writer.Flush();
                //--------------------------------
                writer.WriteStartElement("MyCard");//<MyCard>英文
                writer.WriteElementString("名字", (treeViewCardSelect.Nodes[1].Nodes[0].Checked ? name2 : " "));
                writer.WriteElementString("移动电话", (treeViewCardSelect.Nodes[1].Nodes[1].Checked ? cellphone2 : " "));
                writer.WriteElementString("固定电话", (treeViewCardSelect.Nodes[1].Nodes[2].Checked ? telephone2 : " "));
                writer.WriteElementString("QQ", (treeViewCardSelect.Nodes[1].Nodes[3].Checked ? qq2 : " "));
                writer.WriteElementString("Email", (treeViewCardSelect.Nodes[1].Nodes[4].Checked ? email2 : " "));
                writer.WriteElementString("网址", (treeViewCardSelect.Nodes[1].Nodes[5].Checked ? website2 : " "));
                writer.WriteElementString("学历", (treeViewCardSelect.Nodes[1].Nodes[6].Checked ? diploma2 : " "));
                writer.WriteElementString("职称", (treeViewCardSelect.Nodes[1].Nodes[7].Checked ? title2 : " "));
                writer.WriteElementString("单位名称", (treeViewCardSelect.Nodes[1].Nodes[8].Checked ? com2: " "));
                writer.WriteElementString("地址", (treeViewCardSelect.Nodes[1].Nodes[9].Checked ? location2 : " "));
                writer.WriteStartElement("LOGO"); 
                if (treeViewCardSelect.Nodes[1].Nodes[10].Checked)
                {
                    string FullFileName = logo2;//logo1保存了全路径
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
                    catch { writer.WriteString("写入图片编码出错."); MessageBox.Show("写入图片编码出错.", "English;服务器端,在Wizard"); }
                }
                else
                { writer.WriteString(" "); }
                writer.WriteEndElement();
                writer.WriteElementString("个人简介", (treeViewCardSelect.Nodes[1].Nodes[11].Checked ? detail2 : " "));
                writer.WriteEndElement();//</Mycard>英文

                writer.WriteEndDocument();
            }
            
            //要发送的都在sbXml里了
            sbXml.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");//只因用的是StringBuilder..

            return sbXml.ToString();
        }

        private EventWaitHandle allDone = new EventWaitHandle(false, EventResetMode.ManualReset);

        /// <summary>
        /// 连接Server并发送XML String,XmlString参数应该是GetAllCheckedNodesAndGenerateXml()的返回值
        /// </summary>
        /// <param name="ServerIP">应当是私有字段RemoteServerIP,它已经在构造函数中被赋值</param>
        private void ConnectServerAndSendXml(Object ServerIP) 
        { //类似于在MainForm.cs中作为客户端发送的部分，但不需要发送命令字符串，而是直接发送XML String
            TcpClient client = new TcpClient(AddressFamily.InterNetwork);
            allDone.Reset();
            client.BeginConnect(IPAddress.Parse(ServerIP.ToString()), MainForm.PortTcpServer, new AsyncCallback(ConnectCallback), client);
            allDone.WaitOne();
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            //执行到此说明已经连接到服务器端了
            allDone.Set();
            TcpClient client = (TcpClient)ar.AsyncState;
            client.EndConnect(ar);

            NetworkStream ns = client.GetStream();
            byte[] bytes = Encoding.UTF8.GetBytes(XmlString);
            ns.BeginWrite(bytes, 0, bytes.Length, new AsyncCallback(WriteCallback), ns);
        }
        private void WriteCallback(IAsyncResult ar)
        {
            NetworkStream ns = (NetworkStream)ar.AsyncState;
            ns.EndWrite(ar);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(Image1!=null) Image1.Dispose();
            if(Image2!=null) Image2.Dispose();
            this.Close();
            this.Dispose();
            GC.Collect();
        }

        private void buttonEn_Click(object sender, EventArgs e)
        {
            //点击此按钮把窗体语言和label们都改成英文
            GetCheckedNodeAndShowThem(false);
            SetLabelNFormTextIn2Languages(false);
        }

        private void buttonCn_Click(object sender, EventArgs e)
        {
            //点击此按钮把窗体语言和label们都改成中文
            GetCheckedNodeAndShowThem(true);
            SetLabelNFormTextIn2Languages(true);
        }

        private void treeViewCardSelect_BeforeCheck(object sender, TreeViewCancelEventArgs e)
        {
            //if (e.Node.Name == "Node01NameCn" & e.Node.Checked != true)
            //{
            //    e.Node.Checked = true;
            //}
        }

        private void Wizard_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(Image1!=null) Image1.Dispose();
            if(Image2!=null) Image2.Dispose();
            this.Dispose(true);
            GC.Collect();
        }

        //private void treeViewCardSelect_AfterCheck(object sender, TreeViewEventArgs e)
        //{
        //    if (treeViewCardSelect.Nodes/*和*/.SelectedNode != null)
        //    {
        //        //treeViewCardSelect.SelectedNode是根节点（即 中文、英文）
        //        string nodes="";
        //        MessageBox.Show(treeViewCardSelect.SelectedNode.GetType().ToString());
        //    }
        //    treeViewCardSelect.SelectedNode.Nodes;
        //}
    }
}