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
    /// ѡ����Ϣ����ƬԤ�������ͻ�ȡ������ɺ���ʾ�����
    /// </summary>
    enum Steps { step1, step2, step3, step4 ,Quit};
   
    public partial class Wizard : Form
    {
        private Steps CurrentStep;

        #region ��Ӣ����Ƭ�ֶΡ���Form_Load�¼���һ���Ի�ȡ�������ݣ�Ȼ������ڡ���һ���������ġ���English����ť�ж��ʹ��
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
        /// �˶��������ڽ�ͼƬ�ļ�Ԥ��������ͷ�
        /// </summary>
        private Image Image1, Image2;

        //�������ݿ���� 
        OleDbConnection ODCon;
        OleDbCommand ODCmd;
        OleDbDataReader ODDataReader;
        private string connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=LocalData.mdb";//���ʹ��System.IO.Directory.GetCurrentDirectory()������

        string RemoteServerIP;

       //------------------
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServerIP">Զ��Ŀ��������IP</param>
        public Wizard(string ServerIP)
        {
            InitializeComponent();
            RemoteServerIP = ServerIP;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            //���彥��---------
            this.Opacity = 0d;
            timer1.Start();

            //----------��ʼʱΪ��һ��-----------            
            CurrentStep = Steps.step1;
            //�տ�ʼʱû����һ��
            button1.Visible = false;

            label1.Font = new Font(label1.Font, FontStyle.Bold);
            pictureBox1.Visible = pictureBox2.Visible = pictureBox3.Visible = pictureBox4.Visible = false;
            //��ȡ�������ݣ��ŵ�˽���ֶ���
            connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "\"" + MainForm.CurrentDirectory + "\\LocalData.mdb" + "\"";//���ݿ�ʹ����ȫ·���޶�
            ODCon = new OleDbConnection();
            ODCon.ConnectionString = connectionStr;
            ODCon.Open();
            ODCmd = new OleDbCommand("Select * from LocalUser", ODCon);//�������м�¼
            ODDataReader = ODCmd.ExecuteReader();
            ODDataReader.Read();
            //����
            name1 = ODDataReader[1].ToString(); ;//0����ID
            cellphone1 = ODDataReader[2].ToString(); ;
            telephone1 = ODDataReader[3].ToString(); ;
            qq1 = ODDataReader[4].ToString(); ;
            email1 = ODDataReader[5].ToString(); ;
            website1 = ODDataReader[6].ToString(); ;
            switch (ODDataReader[7].ToString())
            {
                case "0": diploma1 = "����"; break;
                case "1": diploma1 = "��ѧ"; break;
                case "2": diploma1 = "��ѧ����"; break;
                case "3": diploma1 = "˶ʿ"; break;
                case "4": diploma1 = "��ʿ������"; break;

                default: diploma1 = ""; break;
            }
            title1 = ODDataReader[8].ToString();
            com1 = ODDataReader[9].ToString();
            location1 = ODDataReader[10].ToString();
            logo1 = MainForm.CurrentDirectory + @"\Images\" + ODDataReader[11].ToString();//���ݿ��Ӧ�ֶ�ֻ�Ǳ������ļ���logoҪ��Ϊȫ·��
            detail1 = ODDataReader[12].ToString();
            //English
            ODDataReader.Read();
            name2 = ODDataReader[1].ToString(); ;//0����ID
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
            //�Դˣ�����Ӣ����Ƭ�Ѿ�ȫ�����
            ODDataReader.Dispose();
            ODCon.Dispose();
            //------------------------------panel�ǵ�λ�ó�ʼ��
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
            //ѡ����Ӣ����Ƭ��������
            for (int i = 0; i < 2; i++)
            {
                treeViewCardSelect.Nodes[i].Checked = true;
                foreach (TreeNode node in treeViewCardSelect.Nodes[i].Nodes)
                {
                    node.Checked = true;
                }
            }
        }
        #region ���彥��
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

        /// <summary>����¼���Ӧ����ʹtreeView��ʹ�÷�Ӧ��һ��Windows�е�һ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewCardSelect_AfterCheck(object sender, TreeViewEventArgs e)
        {
            //�������˹������ŷ�Ӧ(���̻����)�����������ô���(root�����ӳ��child��)Ҳ������ѡ��ȡ��ѡ���¼�
            if (e.Action == TreeViewAction.ByKeyboard||e.Action==TreeViewAction.ByMouse)
            {
                //�������������㣬�� ѡ�С�ȡ��ѡ�� ���ܷ�ӳ�����ӽڵ���
                if (e.Node.Name == "RootChinese" || e.Node.Name == "RootEnglish")//Ҳ������e.Node.Nodes.Count>0�ж��Ǹ����ڵ�
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
                else//�ӽڵ�
                {
                    if (e.Node.Name == "Node01NameCn" && e.Node.Checked != true)
                    {
                        e.Node.Checked = true;
                    }
                    if (e.Node.Checked)
                    {
                        e.Node.Parent.Checked = true;
                    }
                    else//û��ѡ��
                    {
                        //��������ƽ���ڵ�ѡ������������û��ѡ�У���ȡ��ѡ����parent
                        foreach (TreeNode node in e.Node.Parent.Nodes)
                        {
                            if (node.Checked)//������һ��ѡ����
                            {
                                return;
                            }
                        }
                        //ƽ���ӽڵ㶼û��ѡ�У�����ִ�е��˴�
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
                        //---------------------------------------------------�������
                        //��һ����ť������
                        button1.Visible = false;
                        button2.Text = "��һ��";//�Է���һ
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
                        //---------------------------------------------------�������
                        //��һ����ť����
                        button1.Visible = true;
                        button2.Text = "��һ��";//�Է���һ
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
                case Steps.step3://�������¼�����Ϊ����Ϊ���˶�����step3�ǲ��������
                    MessageBox.Show("���ǲ����ܳ��ֵĲ��裡", "���ǲ����ڵ�����...");
                    break;
                case Steps.step4://not possible!
                    MessageBox.Show("������˲���˼��֮����", "!@#$%^&%");
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
                    {//�Ӵ�����ֻ��Ϊ���׶�
                        //������,��������++���step1
                        MessageBox.Show("�����ܵĴ��������!"+" button2 step1");
                    }
                    break;
                case Steps.step2:
                    //��ôԭ������step1
                    {
                        //---------------------------------------------------�������
                        //��һ����ť����
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
                        //--------------------------------------------------ʵ������
                        //��ʼpreview��--------------------
                        //�������Ҫ����label��Text
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
                        //���Ҫ��ȡѡȡ����һЩ�ڵ�
                        //��ѡ�е�Ҫ��ʾ��δѡ�еĲ���ʾ
                        //��ֵȻ����ʾ��
                        //��һ����ťĬ����ʾ����
                        SetLabelNFormTextIn2Languages(true);
                        GetCheckedNodeAndShowThem(true);

                        
                    }
                    break;
                case Steps.step3:
                    {
                        //---------------------------------------------------�������
                        button2.Text = "����";

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
                        //--------------------------------------------------ʵ������
                        
                        
                    }
                    break;
                case Steps.step4://������ͺ� ��(����)�˲����ʷ���Ӧ�ڴ˲�����
                    {
                        //---------------------------------------------------�������
                        button1.Visible = false;
                        button2.Text = "��ȴ�...";
                        button2.Enabled = false;//�ȷ������ſ���
                        button3.Visible = false;
                        labelResultText.Text="���ڷ����У���ȴ�...";

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
                        //���,��ʾ���ͽ��

                        //Ҫ����XML��������    
                        //���������Ҫ�ͷ�ͼ���ļ����
                        if (Image1 != null) Image1.Dispose();//����һ������/�쳣ʱ����������İ�ť���ֵ�Ӣ�İ�ť�ͻ���һ��û�����õ�Image�����������޷��ͷ�
                        if (Image2 != null) Image2.Dispose();
                        //���Ȼ�ȡѡ������Щ�ڵ㣬������Ƭ��Ȼ����ָ����IPEndPoint����֮

                        XmlString = GetAllCheckedNodesAndGenerateXml();
                        ConnectServerAndSendXml(RemoteServerIP);

                        button2.Text = "���";
                        button2.Enabled = true;
                        button2.Focus();
                        labelResultText.Text = "��" + RemoteServerIP + "������Ƭ�ɹ���";
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

        /// <summary>����TreeView�ؼ���ѡ�������������������ƬԤ����panel��label�ǵ�visible����
        /// </summary>
        /// <param name="IsChinese"></param>
        private void GetCheckedNodeAndShowThem(bool IsChinese)
        {
            int i = 0;
            i = (IsChinese ? 0 : 1);
            //������һ��ѡ�У���Ϊ����
                labelName.Visible = treeViewCardSelect.Nodes[i].Nodes[0].Checked;
                labelCellphone.Visible = label6.Visible = treeViewCardSelect.Nodes[i].Nodes[1].Checked;
                labelTelephone.Visible = label8.Visible = treeViewCardSelect.Nodes[i].Nodes[2].Checked;
                labelQQ.Visible = label10.Visible = treeViewCardSelect.Nodes[i].Nodes[3].Checked;
                labelEmail.Visible = label12.Visible = treeViewCardSelect.Nodes[i].Nodes[4].Checked;
                linkLabelWebsite.Visible = label14.Visible = treeViewCardSelect.Nodes[i].Nodes[5].Checked;
                //ѧ��Ŀǰ�Ȳ���ʾ
                labelTitle.Visible = treeViewCardSelect.Nodes[i].Nodes[7].Checked;
                labelCompany.Visible = treeViewCardSelect.Nodes[i].Nodes[8].Checked;
                labelLocation.Visible = label7.Visible = treeViewCardSelect.Nodes[i].Nodes[9].Checked;
                pictureBoxLogo.Visible = treeViewCardSelect.Nodes[i].Nodes[10].Checked;
                //���˼�����ϸ��Ϣ�������Ȳ���ʾ//����ʾ��Ƭ���ܷ�ҳ
            
            //if it is English,��Ȼʹ���˴˺�������һ��Ҫ��ѡ��
            
        }
        /// <summary>
        /// ���ô������е����ԣ����������ֶ�label,��ʾlabel,��ͼƬ��ַ·��
        /// </summary>
        /// <param name="isChinese">���ķ�</param>
        private void SetLabelNFormTextIn2Languages(bool isChinese)
        {
            //��ֵlabel
            //�ı�Form���ԣ����õ�label�ı���
            if (isChinese)
            {
                label7.Text = "��ַ:";
                label6.Text = "�ֻ�:";
                label8.Text = "�̶��绰:";
                label10.Text = "QQ:";
                label12.Text = "����:";
                label14.Text = "��ַ:";

                //��������label��ʾ
                labelName.Text = name1;
                labelTelephone.Text = telephone1;
                labelCellphone.Text = cellphone1;
                labelQQ.Text = qq1;
                labelEmail.Text = email1;
                linkLabelWebsite.Text = website1;
                //û��diploma
                labelTitle.Text = title1;
                labelCompany .Text= com1;
                labelLocation.Text = location1;
                if (logo1 != null && File.Exists(logo1))
                {
                    pictureBoxLogo.Image = Image1 = Image.FromFile(logo1);
                }
                //����ʾDetail
            }
            else
            {
                label7.Text = "LOCATION:";
                label6.Text = "MOB:";
                label8.Text = "TEL:";
                label10.Text = "QQ:";
                label12.Text = "EMAIL:";
                label14.Text = "WEBSITE:";
                //��������label��ʾ
                labelName.Text = name2;
                labelTelephone.Text = telephone2;
                labelCellphone.Text = cellphone2;
                labelQQ.Text = qq2;
                labelEmail.Text = email2;
                linkLabelWebsite.Text = website2;
                //û��diploma
                labelTitle.Text = title2;
                labelCompany.Text = com2;
                labelLocation.Text = location2;
                if (logo2 != null && File.Exists(logo2))
                {
                    pictureBoxLogo.Image = Image2 = Image.FromFile(logo2);
                }
                //catch (Exception ex) { MessageBox.Show(ex.Message); }
                //����ʾDetail
            }
        }
        /// <summary>
        /// Ӧ��GetAllCheckedNodesAndGenerateXml()����ֵ
        /// </summary>
        private string XmlString;
        /// <summary>
        /// ��ȡ������Ӣ��ѡ�еĽڵ㣬ѡ���������XML��Ϊѡ�������"",Ȼ�󷵻����ɵ�XML��ʽ�� String
        /// </summary>
        /// <returns>�������ɵ�XML��ʽ�� String</returns>
        private string GetAllCheckedNodesAndGenerateXml()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "\t";
            //����XML�ı��еı�����
            settings.Encoding = System.Text.Encoding.Default;//���ﲻ���ã���Ϊ������StringBuilder��//��Ϊ��MemoryStream�����
            //���������⣺��IE��������֧�ִӵ�ǰ���뵽ָ��������л���UTF8��UTF-16(Unicode)
            StringBuilder sbXml = new StringBuilder();//StringBuilder���񲻺��ð�,����Stream�����
           // TextWriter ws = new StringWriter(sbXml);            
            
            using (XmlWriter writer = XmlWriter.Create(sbXml, settings))//����mstream,ֱ������XML�ļ�����"ֱ�����ɵ�XML�ļ�.xml"
            {                         /*����Ҫ��������ֱ�����ɵ�XML��ʽ���ã�����ֱ����IE������������˵����MemoryStream�������⣡����*/
                writer.WriteStartDocument();
                writer.WriteStartElement("ECard");//root
                //----------------------//ûѡ�о�д�ո�
                writer.WriteStartElement("MyCard");//<MyCard>����
                writer.WriteElementString("����", (treeViewCardSelect.Nodes[0].Nodes[0].Checked ? name1 : " "));
                writer.WriteElementString("�ƶ��绰", (treeViewCardSelect.Nodes[0].Nodes[1].Checked ? cellphone1 : " "));
                writer.WriteElementString("�̶��绰", (treeViewCardSelect.Nodes[0].Nodes[2].Checked ? telephone1 : " "));
                writer.WriteElementString("QQ", (treeViewCardSelect.Nodes[0].Nodes[3].Checked ? qq1 : " "));
                writer.WriteElementString("Email", (treeViewCardSelect.Nodes[0].Nodes[4].Checked ? email1 : " "));
                writer.WriteElementString("��ַ", (treeViewCardSelect.Nodes[0].Nodes[5].Checked ? website1 : " "));
                writer.WriteElementString("ѧ��", (treeViewCardSelect.Nodes[0].Nodes[6].Checked ? diploma1 : " "));
                writer.WriteElementString("ְ��", (treeViewCardSelect.Nodes[0].Nodes[7].Checked ? title1 : " "));
                writer.WriteElementString("��λ����", (treeViewCardSelect.Nodes[0].Nodes[8].Checked ? com1 : " "));
                writer.WriteElementString("��ַ", (treeViewCardSelect.Nodes[0].Nodes[9].Checked ? location1 : " "));
                writer.WriteStartElement("LOGO");
                if (treeViewCardSelect.Nodes[0].Nodes[10].Checked)
                {
                    string FullFileName = logo1;//logo1������ȫ·��
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
                    catch { writer.WriteString("д��ͼƬ�������."); MessageBox.Show("д��ͼƬ�������.", "Chinese;��������,��Wizard"); }
                }
                else
                { 
                    writer.WriteString(" "); 
                }
                writer.WriteEndElement();
                writer.WriteElementString("���˼��", (treeViewCardSelect.Nodes[0].Nodes[11].Checked ? detail1 : " "));
                writer.WriteEndElement();//</Mycard>����
                //--------------------------------
              //  writer.Flush();
                //--------------------------------
                writer.WriteStartElement("MyCard");//<MyCard>Ӣ��
                writer.WriteElementString("����", (treeViewCardSelect.Nodes[1].Nodes[0].Checked ? name2 : " "));
                writer.WriteElementString("�ƶ��绰", (treeViewCardSelect.Nodes[1].Nodes[1].Checked ? cellphone2 : " "));
                writer.WriteElementString("�̶��绰", (treeViewCardSelect.Nodes[1].Nodes[2].Checked ? telephone2 : " "));
                writer.WriteElementString("QQ", (treeViewCardSelect.Nodes[1].Nodes[3].Checked ? qq2 : " "));
                writer.WriteElementString("Email", (treeViewCardSelect.Nodes[1].Nodes[4].Checked ? email2 : " "));
                writer.WriteElementString("��ַ", (treeViewCardSelect.Nodes[1].Nodes[5].Checked ? website2 : " "));
                writer.WriteElementString("ѧ��", (treeViewCardSelect.Nodes[1].Nodes[6].Checked ? diploma2 : " "));
                writer.WriteElementString("ְ��", (treeViewCardSelect.Nodes[1].Nodes[7].Checked ? title2 : " "));
                writer.WriteElementString("��λ����", (treeViewCardSelect.Nodes[1].Nodes[8].Checked ? com2: " "));
                writer.WriteElementString("��ַ", (treeViewCardSelect.Nodes[1].Nodes[9].Checked ? location2 : " "));
                writer.WriteStartElement("LOGO"); 
                if (treeViewCardSelect.Nodes[1].Nodes[10].Checked)
                {
                    string FullFileName = logo2;//logo1������ȫ·��
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
                    catch { writer.WriteString("д��ͼƬ�������."); MessageBox.Show("д��ͼƬ�������.", "English;��������,��Wizard"); }
                }
                else
                { writer.WriteString(" "); }
                writer.WriteEndElement();
                writer.WriteElementString("���˼��", (treeViewCardSelect.Nodes[1].Nodes[11].Checked ? detail2 : " "));
                writer.WriteEndElement();//</Mycard>Ӣ��

                writer.WriteEndDocument();
            }
            
            //Ҫ���͵Ķ���sbXml����
            sbXml.Replace("encoding=\"utf-16\"", "encoding=\"utf-8\"");//ֻ���õ���StringBuilder..

            return sbXml.ToString();
        }

        private EventWaitHandle allDone = new EventWaitHandle(false, EventResetMode.ManualReset);

        /// <summary>
        /// ����Server������XML String,XmlString����Ӧ����GetAllCheckedNodesAndGenerateXml()�ķ���ֵ
        /// </summary>
        /// <param name="ServerIP">Ӧ����˽���ֶ�RemoteServerIP,���Ѿ��ڹ��캯���б���ֵ</param>
        private void ConnectServerAndSendXml(Object ServerIP) 
        { //��������MainForm.cs����Ϊ�ͻ��˷��͵Ĳ��֣�������Ҫ���������ַ���������ֱ�ӷ���XML String
            TcpClient client = new TcpClient(AddressFamily.InterNetwork);
            allDone.Reset();
            client.BeginConnect(IPAddress.Parse(ServerIP.ToString()), MainForm.PortTcpServer, new AsyncCallback(ConnectCallback), client);
            allDone.WaitOne();
        }
        private void ConnectCallback(IAsyncResult ar)
        {
            //ִ�е���˵���Ѿ����ӵ�����������
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
            //����˰�ť�Ѵ������Ժ�label�Ƕ��ĳ�Ӣ��
            GetCheckedNodeAndShowThem(false);
            SetLabelNFormTextIn2Languages(false);
        }

        private void buttonCn_Click(object sender, EventArgs e)
        {
            //����˰�ť�Ѵ������Ժ�label�Ƕ��ĳ�����
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
        //    if (treeViewCardSelect.Nodes/*��*/.SelectedNode != null)
        //    {
        //        //treeViewCardSelect.SelectedNode�Ǹ��ڵ㣨�� ���ġ�Ӣ�ģ�
        //        string nodes="";
        //        MessageBox.Show(treeViewCardSelect.SelectedNode.GetType().ToString());
        //    }
        //    treeViewCardSelect.SelectedNode.Nodes;
        //}
    }
}