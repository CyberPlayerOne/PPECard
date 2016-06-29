using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
//
using System.Data.OleDb;

namespace PPECard
{
    public partial class DisplayCardForm : Form,IDisposable
    {
        //字段声明
        private bool ItsLocalUser;
        private OleDbConnection oleDbCon;
        private string connectionStr;//最好使用System.IO.Directory.GetCurrentDirectory()！！！
        private OleDbCommand oleDbCmd;
        private OleDbDataReader oleDbCardReader;       
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DisplayCardForm()
        {
            InitializeComponent();
            //ItsLocalUser = false;//是其他人
            ////禁止修改otherGuyECardForm各个控件内容
            //foreach (Control control in this.Controls)
            //{
            //    if (control is TextBox) ((TextBox)control).ReadOnly = true;
            //    if (control is RichTextBox) ((RichTextBox)control).ReadOnly = true;
            //}

            //CurrentDirectory = System.IO.Directory.GetCurrentDirectory();//E.g.  "C:\program files"
            ////数据库存在的目录
            //connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "\"" + CurrentDirectory + @"\LocalData.mdb" + "\"";
        }
        /// <summary>
        /// 本地用户调用时使用isLocalUser为true
        /// 本窗体在PPECard  v2.0中仅用于本地用户，作名片数据修改之用0-++++++++++++++++++++++
        /// 名片显示使用CardForm
        /// </summary>
        /// <param name="isLocalUser"></param>
        public DisplayCardForm(bool isLocalUser)
        {
            InitializeComponent();
            ItsLocalUser = true;//是本地用户,（直接加true）

            //数据库存在的目录
            connectionStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + "\"" +MainForm.CurrentDirectory + @"\LocalData.mdb" + "\"";
            //access数据库路径如果有空格可以加双引号！！！！！！！！！
            //CurrentDirectory必须放在LoadFromDB方法前此方法才能载入图片，因为此方法里要用到这个图片。
            //connectionStr必须放在前面LoadFromDB 才能用
            LoadFromDB(true);//默认是中文名片;路径
            radioButtonChinese.Checked = true;
            radioButtonEnglish.Checked = false;

            
            
        }
        private void LoadFromDB(bool isChineseData)
        {
            try
            {
                oleDbCon = new OleDbConnection();
                oleDbCon.ConnectionString = connectionStr;
                oleDbCon.Open();
                oleDbCmd = oleDbCon.CreateCommand();
                string getLocalUserCardStr = (isChineseData ? "select * from LocalUser where ID=1" : "select * from LocalUser where ID=2");
                oleDbCmd.CommandText = getLocalUserCardStr;
                oleDbCardReader = oleDbCmd.ExecuteReader();
                while (oleDbCardReader.Read())
                {
                    //将数据库中的信息显示在名片显示窗体上//不管是中文英文都这样
                    textBoxUserName.Text = (string)oleDbCardReader["名字"];
                    textBoxCellphone.Text = (string)oleDbCardReader["移动电话"];
                    textBoxTelephone.Text = (string)oleDbCardReader["固定电话"];
                    textBoxQQ.Text = (string)oleDbCardReader["QQ"];
                    textBoxEmail.Text = (string)oleDbCardReader["Email"];
                    comboBoxDiploma.SelectedIndex = int.Parse((string)oleDbCardReader["学历"]);
                    textBoxTitle.Text = (string)oleDbCardReader["职称"];
                    textBoxCompany.Text = (string)oleDbCardReader["单位名称"];
                    linkLabelBlogAddress.Text = (string)oleDbCardReader["网址"];
                    textBoxSetBlogAddress.Text = (string)oleDbCardReader["网址"];
                    textBoxLocation.Text = (string)oleDbCardReader["地址"];
                    //try
                    //{
                    //    //在读取时使用流读取，因为如果修改了就要删除原来的旧图像文件，使用流就不会“正在又另一进程使用”
                    //    System.IO.FileStream fs = new System.IO.FileStream((string)oleDbCardReader["LOGO"], System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Delete);
                    //    //如果在此处发生异常（上一句）则下面的（且try内的）都不执行
                    //    pictureBoxDisplayPic.BackgroundImage = Image.FromStream(fs);//窗体加载时先显示数据库中的图片地址的图片
                    //    fs.Dispose();//释放资源，释放文件
                    //    pictureBoxDisplayPic.Tag = (string)oleDbCardReader["LOGO"];//保存原始路径，以备后用
                    //}
                    //catch (System.IO.IOException ex)
                    //{
                    //    pictureBoxDisplayPic.Tag = null;
                    //    MessageBox.Show(ex.Message, "在以下路径未能找到图像文件!");
                    //}
                    //catch (Exception ex)
                    //{
                    //    pictureBoxDisplayPic.Tag = null;
                    //    MessageBox.Show(ex.Message, "在载入图片时发生错误!");
                    //}

                    try//显示图片
                    {
                        pictureBoxDisplayPic.Image = Image.FromFile(MainForm.CurrentDirectory + @"\Images\" + (string)oleDbCardReader["LOGO"]);
                        pictureBoxDisplayPic.Tag = (string)oleDbCardReader["LOGO"];//Tag只保存文件名
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message,"找不到图片文件"); }

                    //  textBoxSetDisplayPicUrl.Text = (string)oleDbCardReader["LOGO"];//隐藏控件，用于保存图片位置-》已经过时
                    //0-++++++++++++++++++++++去掉
                    richTextBoxAboutMe.Text = (string)oleDbCardReader["个人简介"];
                }
                //在数据库提取并改变TextBox之后再disable
                buttonSave.Enabled = false;
            }
            
            catch (OleDbException ex) { MessageBox.Show(ex.Message, "连接数据库错误！"); }
            catch (InvalidOperationException ex) { MessageBox.Show(ex.Message, "非法操作！"); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "发生错误！"); }
            finally
            {
                if (oleDbCon != null) oleDbCon.Dispose();
                if (oleDbCardReader != null) oleDbCardReader.Dispose();
            }
        }
        private void textBoxUserName_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (radioButtonChinese.Checked) SaveIntoDB(true);
            else SaveIntoDB(false);
            buttonSave.Enabled = false;
        }
        private void SaveIntoDB(bool isChineseData)
        {
            if (ItsLocalUser)
            {
                try
                {
                    oleDbCon = new OleDbConnection();
                    oleDbCon.ConnectionString = connectionStr;
                    oleDbCon.Open();
                    oleDbCmd = oleDbCon.CreateCommand();
                    string StrSaveLocalUserCard = "update LocalUser set ";
                    string StrInsertValue = "";
                    StrInsertValue += "名字=" + "\"" + textBoxUserName.Text + "\"" + ",";
                    StrInsertValue += "移动电话=" + "\"" + textBoxCellphone.Text + "\"" + ",";
                    StrInsertValue += "固定电话=" + "\"" + textBoxTelephone.Text + "\"" + ",";
                    StrInsertValue += "QQ=" + "\"" + textBoxQQ.Text + "\"" + ",";
                    StrInsertValue += "Email=" + "\"" + textBoxEmail.Text + "\"" + ",";
                    StrInsertValue += "网址=" + "\"" + textBoxSetBlogAddress.Text + "\"" + ",";//!!!!!!!!应该是、修改后的
                    StrInsertValue += "学历=" + "\"" + comboBoxDiploma.SelectedIndex.ToString() + "\"" + ",";
                    StrInsertValue += "职称=" + "\"" + textBoxTitle.Text + "\"" + ",";
                    StrInsertValue += "单位名称=" + "\"" + textBoxCompany.Text + "\"" + ",";
                    StrInsertValue += "地址=" + "\"" + textBoxLocation.Text + "\"" + ",";
                    StrInsertValue += "LOGO=" + "\"" + pictureBoxDisplayPic.Tag.ToString() + "\"" + ",";//!!!!!!!修改后的LOGO图片文件名已经通过Tag保存
                    //、、++++++++++++++++++++++++++++++++可以用一个公共变量来存储
                    StrInsertValue += "个人简介=" + "\"" + richTextBoxAboutMe.Text + "\"";
                    StrSaveLocalUserCard += StrInsertValue;
                    StrSaveLocalUserCard += (isChineseData ? " where id=1" : " where id=2");//确定是本地用户表的中文还是英文记录

                    oleDbCmd.CommandText = StrSaveLocalUserCard;
                    if (oleDbCmd.ExecuteNonQuery() == 1)
                    {
                        this.Text += "  --更新名片成功！";
                        buttonSave.Enabled = false;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "更新名片失败");
                }
                finally
                {
                    oleDbCon.Dispose();
                }
                //this.Close();//保存完关闭窗体。-》已过时
            }//end_if
            //else//->已过时
            //{//not local user
            //    this.Close();
            //}//end.else
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void pictureBoxDisplayPic_DoubleClick(object sender, EventArgs e)
        //{
        //    //如果是本地用户则显示此textbox以用于编辑
        //    if (ItsLocalUser)
        //    {
        //        textBoxSetDisplayPicUrl.Visible = true;
        //    }
        //}

        private void buttonEditBlogAddress_Click(object sender, EventArgs e)
        {
            if (ItsLocalUser)
            {
                textBoxSetBlogAddress.Width = 245;
                textBoxSetBlogAddress.Visible = true;
            }
        }

        private void linkLabelBlogAddress_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(this.linkLabelBlogAddress.Text);
                this.linkLabelBlogAddress.LinkVisited = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void richTextBoxAboutMe_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(e.LinkText);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButtonChinese_CheckedChanged(object sender, EventArgs e)
        {
            string lang;
            if (radioButtonChinese.Checked)
                lang = "ENGLISH Business Card";//刚才修改的（反着）
            else
                lang = "中文版名片";
            if (buttonSave.Enabled == true)//说明修改了还没有保存
            {
                if (MessageBox.Show("您刚刚修改了" + lang + "的内容但未保存，是否保存？"
                    , "修改未保存", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    //根据是中文还是英文存在相应数据库里
                    //0-+++++++++++++++++++++++++++++++++
                    SaveIntoDB(true);
                }//如果不保存则继续
                else
                {
                    buttonSave.Enabled = false;
                }
            }
            SetFormLanguage("Chinese");
            LoadFromDB(true);
        }

        private void radioButtonEnglish_CheckedChanged(object sender, EventArgs e)
        {
            string lang;
            if (radioButtonChinese.Checked)
                lang = "ENGLISH Business Card";//刚才修改的（反着）
            else
                lang = "中文版名片";
            if (buttonSave.Enabled == true)//说明修改了还没有保存
            {
                if (MessageBox.Show("您刚刚修改了" + lang + "的内容但未保存，是否保存？"
                    , "修改未保存", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    //根据是中文还是英文存在相应数据库里
                    //0-+++++++++++++++++++++++++++++++++
                    SaveIntoDB(false);
                }//如果不保存则继续
                else
                {
                    buttonSave.Enabled = false;
                }
            }
            SetFormLanguage("English");
            LoadFromDB(false);
        }

        private void SetFormLanguage(string lang)
        {
            //先改变界面语言，然后从各数据库中提取数据
            if (lang == "Chinese")
            {
                this.Text = "修改名片数据";
                label1.Text = "请选择要修改的名片:";
                labelUserName.Text = "姓名：";
                labelCellphone.Text = "移动电话：";
                labelTelephone.Text = "固定电话：";
                labelQQ.Text = "QQ：";
                labelEmail.Text = "电邮：";
                labelDiploma.Text = "学历：";
                comboBoxDiploma.BeginUpdate();
                comboBoxDiploma.Items.Clear();
                comboBoxDiploma.Items.Add("保密");
                comboBoxDiploma.Items.Add("中学");
                comboBoxDiploma.Items.Add("大学本科");
                comboBoxDiploma.Items.Add("硕士");
                comboBoxDiploma.Items.Add("博士及以上");
                comboBoxDiploma.EndUpdate();
                labelTitle.Text = "职称：";
                labelCompany.Text = "单位名称：";
                labelBlogAddress.Text = "网址：";
                labelLocation.Text = "地址：";
                labelProfile.Text = "简介：";
                buttonLoadImageFile.Text = "更换 Logo...";
                buttonEditBlogAddress.Text = "编辑";
                buttonSave.Text = "保存";
                buttonPreview.Text = "名片预览";
                buttonCancel.Text = "退出";
            }
            else//lang == English
            {
                this.Text = "Editing Business Card Data";
                label1.Text = "Select an item to edit:";
                labelUserName.Text = "Full Name：";
                labelCellphone.Text = "Mob：";
                labelTelephone.Text = "TEL：";
                labelQQ.Text = "QQ：";
                labelEmail.Text = "Email：";
                labelDiploma.Text = "Diploma：";
                comboBoxDiploma.BeginUpdate();
                comboBoxDiploma.Items.Clear();
                comboBoxDiploma.Items.Add("Secret");
                comboBoxDiploma.Items.Add("Middle School");
                comboBoxDiploma.Items.Add("College Degree");
                comboBoxDiploma.Items.Add("Master Degree");
                comboBoxDiploma.Items.Add("Doctor Degree");
                comboBoxDiploma.EndUpdate();
                labelTitle.Text = "Title：";
                labelCompany.Text = "Company：";
                labelBlogAddress.Text = "Website：";
                labelLocation.Text = "Location：";
                labelProfile.Text = "Remarks：";
                buttonLoadImageFile.Text = "Change Logo...";
                buttonEditBlogAddress.Text = "EDIT";
                buttonSave.Text = "SAVE";
                buttonPreview.Text = "PREVIEW";
                buttonCancel.Text = "EXIT";
            }
        }

        private void buttonLoadImageFile_Click(object sender, EventArgs e)
        {
            openFileDialogImage.Title = "选择一个图片来作为名片LOGO：";
            openFileDialogImage.InitialDirectory = MainForm.CurrentDirectory+ @"\images\";
            openFileDialogImage.Filter = "*.JPG|*.jpg";//解码并生成文件时还不会判断是jpg还是bmp什么的格式啊！！暂时用只用jpg
            openFileDialogImage.Multiselect = false;
            if (openFileDialogImage.ShowDialog() == DialogResult.OK)
            {
                string FullFileName = openFileDialogImage.FileName;
                string FileName = (new RandomString()).getRandomString(10)+".jpg";//新文件名，不包含路径
                string OldFileName = null;
                if (pictureBoxDisplayPic.Tag != null)//说明旧图像文件存在
                {
                    OldFileName = pictureBoxDisplayPic.Tag.ToString();//保存旧文件路径以用于删除
                }
                pictureBoxDisplayPic.Image = null;
                System.IO.File.Copy(FullFileName, MainForm.CurrentDirectory + @"\Images\" + FileName, true);
                //允许overwrite,但如果与当前显示的图片文件名一样，则会异常：当前文件正在使用！
                //目前仅用jpg


                pictureBoxDisplayPic.Image = Image.FromFile(MainForm.CurrentDirectory + @"\Images\" + FileName);
                pictureBoxDisplayPic.Tag = FileName;//修改了图片后：本句用于保存图片文件名，以备后用
               // MessageBox.Show(CurrFullFileName,FullFileName);//test only
                buttonSave.Enabled = true;//表示已经修改；
                //if (OldFullFileName != null)//旧图像文件存在，则...删除
                //{
                //   // System.IO.File.Delete(OldFullFileName);//已经纠正异常：文件“E:\0.png”正由另一进程使用，因此该进程无法访问该文件。
                //}
            }
        }

        private void buttonPreview_Click(object sender, EventArgs e)
        {
            CardForm cf = new CardForm(true);//先使用中文吧,会在后面设置窗体语言
            cf.Controls["buttonCn"].Visible = false;
            cf.Controls["buttonEn"].Visible = false;
            //根据是英文还是中文要禁用语言选择按钮；即只可以预览一种语言版本
            if (radioButtonChinese.Checked)//要预览中文
            {
                //设置字段
                cf.name1=textBoxUserName.Text;
                cf.cellphone1=textBoxCellphone.Text;
                cf.telephone1=textBoxTelephone.Text;
                cf.qq1=textBoxQQ.Text;
                cf.email1=textBoxEmail.Text;
                cf.website1=textBoxSetBlogAddress.Text;
                switch (this.comboBoxDiploma.SelectedIndex)
	                {
                    case 0:cf.diploma1="保密";break;
                    case 1:cf.diploma1="中学";break;
                    case 2:cf.diploma1="大学本科";break;
                    case 3:cf.diploma1="硕士";break;
                    case 4:cf.diploma1="博士及以上";break;

		                default:cf.diploma1="";break;
	                }
                
                cf.title1=textBoxTitle.Text;
                cf.com1=textBoxCompany.Text;
                cf.location1=textBoxLocation.Text;
                cf.logo1 = MainForm.CurrentDirectory + @"\Images\" + pictureBoxDisplayPic.Tag.ToString();
                cf.detail1 = richTextBoxAboutMe.Text;
                //输入到控件
                cf.InitControlTextChinese();
                cf.CurrLangIsChn = true;
                cf.SetFormLanguage();
                cf.Show();
            }
            else
            {
                //设置字段
                cf.name2 = textBoxUserName.Text;
                cf.cellphone2 = textBoxCellphone.Text;
                cf.telephone2 = textBoxTelephone.Text;
                cf.qq2 = textBoxQQ.Text;
                cf.email2 = textBoxEmail.Text;
                cf.website2 = textBoxSetBlogAddress.Text;
                switch (this.comboBoxDiploma.SelectedIndex)
                {
                    case 0: cf.diploma2 = "Secret"; break;
                    case 1: cf.diploma2 = "Middle School"; break;
                    case 2: cf.diploma2 = "College Degree"; break;
                    case 3: cf.diploma2 = "Master Degree"; break;
                    case 4: cf.diploma2 = "Doctor Degree"; break;

                    default: cf.diploma2 = ""; break;
                }

                cf.title2 = textBoxTitle.Text;
                cf.com2 = textBoxCompany.Text;
                cf.location2 = textBoxLocation.Text;
                cf.logo2 = MainForm.CurrentDirectory + @"\Images\" + pictureBoxDisplayPic.Tag.ToString();
                cf.detail2 = richTextBoxAboutMe.Text;
                //输入到控件
                cf.InitControlTextEnglish();
                cf.CurrLangIsChn = false;
                cf.SetFormLanguage();
                cf.Show();
            }
        }

        /// <summary>
        /// 在关闭本窗体时，一定要把所有资源确定全部释放，特别是图像文件；
        /// 因为假如是本机与本机通信，在请求文件或更新时服务器方要读取图像文件并产生Base64编码，
        /// 如果文件没有被释放，就会无法进行并产生异常，！！！
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayCardForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //I'm not sure about the result 'cause there're 2 Image Objects(Chinese & English),And I dispose only one of them.
            //But the result turns out not bad.
            pictureBoxDisplayPic.Image.Dispose();
           // this.Dispose(true);
            GC.Collect();//只能释放一次
        }
    }
    /// <summary>
    /// 用于生成指定长度的随机字符串，我只用第二个方法
    /// </summary>
    class RandomString
    {
        Random m_rnd = new Random();
        public char getRandomChar()
        {
            int ret = m_rnd.Next(122);
            while (ret < 48 || (ret > 57 && ret < 65) || (ret > 90 && ret < 97))
            {
                ret = m_rnd.Next(122);
            }
            return (char)ret;
        }
        public string getRandomString(int length)
        {
            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(getRandomChar());
            }
            return sb.ToString();
        }
    }
}