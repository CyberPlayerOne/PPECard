using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;//for 窗体可拖动代码

namespace PPECard
{
    public partial class CardForm : Form
    {
        #region 窗体可拖动代码
        //窗体可拖动代码
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        //窗体可拖动代码完
        #endregion

        public bool CurrLangIsChn;

        #region 分别为中文与英文的名片的字段,建立对象后一定要给这些字段赋值，否则为0长度字符串！！

        public string name1, name2;
        public string cellphone1, cellphone2;
        public string telephone1, telephone2;
        public string qq1, qq2;
        public string email1, email2;
        public string website1, website2;
        /// <summary>
        ///  "学历"暂不显示在窗体上
        /// </summary>
        public string diploma1, diploma2;
        public string title1, title2;
        public string com1, com2;
        public string location1, location2;
        /// <summary>
        /// CardForm里的Logo字段要求是图片的全路径，而数据库里存的是文件名，要注意！！要在给Logo赋值时补为全路径！
        /// </summary>
        public string logo1, logo2;
        public string detail1, detail2;

        /// <summary>
        /// 为了在窗体关闭时释放pictureBox控件的Image对象所有的图片文件的使用，需要保持这个Image的引用！！
        /// </summary>
        Image ImageLogo1;
        Image ImageLogo2;
        //System.Collections.ArrayList arrayList;

        #endregion

        /// <summary>
        /// 使用方法：先调用构造函数，再给公共的名片字段赋值，最后再调用对应（中文？英文？）的InitControlTextXXX方法。
        /// </summary>
        /// <param name="DefaultLanguageIsChinese"></param>
        public CardForm(bool DefaultLanguageIsChinese)
        {
            InitializeComponent();//系统自动生成的函数！：用于设置“设计时”的设置数据

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.DoubleBuffered = true;

            richTextBoxTestOnly.Visible = false;
            panel1.Controls.Remove(panel2);//设计时被当作子控件了,对panel2的设计时操作相当于白做了
            this.Controls.Add(panel2);//添加到form的Controls集合中才能显示
            panel2.Location = new Point(12, 63);//位置及大小要与panel1相同
            panel2.Size = new Size(523, 295);

            
            foreach (Control control in this.Controls)
            {
                control.BackColor = Color.Transparent;
            }
            labelClose.BackColor = Color.White;
            labelClose.FlatStyle = FlatStyle.Popup;//没用啊？
           
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            //原来panel的控件集合有谁 设计时被自动添加了！！！使用时要注意！！测试一下或看Designer.cs文件！！

            
            //还有一个详细信息，是动态添加的控件
           
            transparentRichTextBox1.Location = new Point(3, 3);
            transparentRichTextBox1.Size = new Size(517, 257);
            transparentRichTextBox1.BorderStyle = BorderStyle.None;
            transparentRichTextBox1.Text = "透明的http://www.baidu.com这里是详细信息的RichTextBox";
            transparentRichTextBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CardForm_MouseDown);
            transparentRichTextBox1.ReadOnly = true;
            //先保留transparentRichTextBox1上的鼠标模样吧

            //初始化时全置""
            name1 = name2 = "";
            cellphone1 = cellphone2 = "";
            telephone1 = telephone2 = "";
            qq1 = qq2 = "";
            email1 = email2 = "";
            website1 = website2 = "";
            diploma1 = diploma2 = "";
            title1 = title2 = "";
            com1 = com2 = "";
            location1 = location2 = "";
            logo1 = logo2 = "";
            detail1 = detail2 = "";

            //设置公共字段，可能在外部用于设置
            CurrLangIsChn = DefaultLanguageIsChinese;
            //设置窗体默认语言
            if (DefaultLanguageIsChinese)
            { SetFormLanguage(true);  }
            else
            { SetFormLanguage(false);  }
        }
        /// <summary> 使用此窗体时，先给中文英文名片字段赋值，再使用本函数把赋的值写到各个控件中以用于显示!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        public void InitControlTextChinese()
        {
            labelName.Text = name1;
            labelCellphone.Text = cellphone1;
            labelTelephone.Text = telephone1;
            labelQQ.Text = qq1;
            labelEmail.Text = email1;
            linkLabelWebsite.Text = website1;
            //diploma1,2 暂不显示在窗体上
            labelTitle.Text = title1;
            labelCompany.Text = com1;
            labelLocation.Text = location1;
            if (logo1 != ""&&logo1!=" " && System.IO.File.Exists(logo1))//当使用DisPlayCardForm修改图片文件后,文件路径变了logo1变为"E:\\TXY\\PPECard_1233 2008-5-24\\PPECard\\bin\\Debug\\Images\\images\\192.168.1.4.jpg"
            {
                try
                {
                    ImageLogo1 = Image.FromFile(logo1);//生成的图片文件自己有错误就又不行了
                }
                catch
                {
                    ImageLogo1 = null;
                }
                //finally
                //{
                //    ImageLogo1.Dispose();
                //}
                //ImageLogo1 = Image.FromFile(logo1);
                pictureBoxLogo.Image = ImageLogo1;
                //}
                //catch (Exception ex)
                //{ 
                //  //  MessageBox.Show(ex.Message, "ERROR HAPPENED IN LOADING LOGO IMAGE(Chinese)"); 
                //}//test only
            }
            transparentRichTextBox1.Text = detail1;
        }
        public void InitControlTextEnglish()
        {
            labelName.Text = name2;
            labelCellphone.Text = cellphone2;
            labelTelephone.Text = telephone2;
            labelQQ.Text = qq2;
            labelEmail.Text = email2;
            linkLabelWebsite.Text = website2;
            //diploma1,2 暂不显示在窗体上
            labelTitle.Text = title2;
            labelCompany.Text = com2;
            labelLocation.Text = location2;
            if (logo2 != ""&&logo2!=" ")//如果有图片文件的记录
            {
                //try 
                //{
                if(System.IO.File.Exists(logo2))//如果确实有这个文件
                {
                    try
                    {
                        ImageLogo2 = Image.FromFile(logo2);//如果图片文件有问题，就不显示（例如主动发送时，如果，没有选择图片，则发送来的XML不包含Base64编码,则会产生问题）
                    }
                    catch
                    {
                        ImageLogo2 = null;
                    }
                    pictureBoxLogo.Image = ImageLogo2;
                }
                //}
                //catch (Exception ex)
                //{
                //    //MessageBox.Show(ex.Message, "ERROR HAPPENED IN LOADING LOGO IMAGE(English)"); 
                //}//test only
            }
            transparentRichTextBox1.Text= detail2;
        }

        private void CardForm_MouseEnter(object sender, EventArgs e)
        {
            labelClose.Visible = true;
        }

        private void labelClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelClose_MouseEnter(object sender, EventArgs e)
        {
            labelClose.Visible = true;
        }

        private void labelClose_MouseLeave(object sender, EventArgs e)
        {
            labelClose.Visible = false;
        }

        private void CardForm_MouseDown(object sender, MouseEventArgs e)//panel1\panel2都调用这个
        {
            //响应窗体拖动
            ReleaseCapture();
            SendMessage(this.Handle, WM_SYSCOMMAND, SC_MOVE + HTCAPTION, 0);
        }

        private void buttonDetail_Click(object sender, EventArgs e)
        {
            
            panel2.Visible = true;
            panel1.Visible = false; transparentRichTextBox1.Visible = true;
            //panel2.BringToFront();不需要了//panel2在panel1的z-index的上面(设计时)
            //panel1.SendToBack();
            #region Test Only：用于查看两个panel的Controls集合
            //string str = "";
            //foreach (Control control in panel1.Controls)
            //{
            //    str += "\n"+control.ToString();
            //    if (control is Panel)
            //    {
            //        MessageBox.Show(control.Name);//这样测试，得知原来在设计时panel2被当作子控件添加到panel1的Controls去了
            //    }
            //}
            //MessageBox.Show(str);
            //string str2="";
            //foreach (Control control in panel2.Controls)
            //{
            //    str2 += "\n" + control.ToString();
            //    if (control is Panel)
            //    {
            //        MessageBox.Show(control.Name);//这样测试，得知原来在设计时panel2被当作子控件添加到panel1的Controls去了
            //    }
            //}
            //MessageBox.Show(str2);
            #endregion
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            panel2.Visible = false;
            //panel1.BringToFront();
            //panel2.SendToBack();
        }

        private void buttonCn_Click(object sender, EventArgs e)
        {
            buttonCn.Enabled = false;
            buttonEn.Enabled = true;
            SetFormLanguage(true);
            InitControlTextChinese();

            panel1.Visible = true;//显示panel1隐藏panel2是为弥补 透明RichTextbox控件 的自身显示缺陷：无法修改其Text(是否重绘什么的可以解决？)
            panel2.Visible = false;//而且似乎这样的操作结果更合常规
        }

        private void buttonEn_Click(object sender, EventArgs e)
        {
            buttonEn.Enabled = false;
            buttonCn.Enabled = true;
            SetFormLanguage(false);
            InitControlTextEnglish();

            panel1.Visible = true;
            panel2.Visible = false;
        }

        /// <summary>第一种设置语言方法：直接在这里参数加
        /// </summary>
        /// <param name="SetToChinese"></param>
        public void SetFormLanguage(bool SetToChinese)
        {
            if (SetToChinese)
            {
                label4.Text = "地址:";
                label6.Text = "手机:";
                label8.Text = "固定电话:";
                label10.Text = "QQ:";
                label12.Text = "电邮:";
                label14.Text = "网址:";
                buttonDetail.Text = "背面>>";
                buttonBack.Text = "<<正面";

                CurrLangIsChn = true;
                transparentRichTextBox1.Text = "";
            }
            else
            {
                label4.Text = "LOCATION:";
                label6.Text = "MOB:";
                label8.Text = "TEL:";
                label10.Text = "QQ:";
                label12.Text = "EMAIL:";
                label14.Text = "WEBSITE:";
                buttonDetail.Text = "BACK>>";
                buttonBack.Text = "<<FRONT";

                CurrLangIsChn = false;
                transparentRichTextBox1.Text = "";
            }
        }
        /// <summary>第二种设置语言方法：先设置CurrLangIsChn，根据公共字段CurrLangIsChn调用重载函数
        /// </summary>
        public void SetFormLanguage()
        {
            SetFormLanguage(CurrLangIsChn);
        }

        private void labelClose_MouseDown(object sender, MouseEventArgs e)
        {
            labelClose.ImageIndex = 1;
        }

        private void labelClose_MouseUp(object sender, MouseEventArgs e)
        {
            labelClose.ImageIndex = 0;
        }

        private void CardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ImageLogo1 != null) 
                ImageLogo1.Dispose();
            if (ImageLogo2 != null) 
                ImageLogo2.Dispose();//如果只看了中文名片,或者根本就没有英文名片的Logo2图片文件存在，则ImageLogo2不会生成

            this.Dispose(true);
            GC.Collect();
        }

        private void linkLabelWebsite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try { System.Diagnostics.Process.Start(linkLabelWebsite.Text); }
            catch (Exception ex)
            {
                try
                {
                    string time = DateTime.Now.ToString();
                    string log = "【" + time + "】" + "[" + "一般错误" + "]" + ex.Message;
                    System.IO.StreamWriter sw = System.IO.File.AppendText(MainForm.CurrentDirectory + "\\error_log.log");
                    sw.WriteLine(log);
                }
                catch { }
            }
        }

        private void transparentRichTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try { System.Diagnostics.Process.Start(e.LinkText); }
            catch (Exception ex)
            {
                try
                {
                    string time = DateTime.Now.ToString();
                    string log = "【" + time + "】" + "[" + "一般错误" + "]" + ex.Message;
                    System.IO.StreamWriter sw = System.IO.File.AppendText(MainForm.CurrentDirectory + "\\error_log.log");
                    sw.WriteLine(log);
                }
                catch { }
            }
        }
        
    }


    class TransparentControl : Control
    {
        public TransparentControl()
        {
            base.SetStyle(ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.DoubleBuffer, true);
            base.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }
    }

    class TransparentRichTextBox : RichTextBox
    {
        public TransparentRichTextBox()
        {
            base.ScrollBars = RichTextBoxScrollBars.None;
        }

        override protected CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        override protected void OnPaintBackground(PaintEventArgs e)
        {
        }
    }

}