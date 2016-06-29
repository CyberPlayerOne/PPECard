namespace PPECard
{
    partial class DisplayCardForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.labelUserName = new System.Windows.Forms.Label();
            this.labelCellphone = new System.Windows.Forms.Label();
            this.labelTelephone = new System.Windows.Forms.Label();
            this.labelQQ = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.labelBlogAddress = new System.Windows.Forms.Label();
            this.labelProfile = new System.Windows.Forms.Label();
            this.textBoxUserName = new System.Windows.Forms.TextBox();
            this.textBoxCellphone = new System.Windows.Forms.TextBox();
            this.textBoxTelephone = new System.Windows.Forms.TextBox();
            this.textBoxQQ = new System.Windows.Forms.TextBox();
            this.textBoxEmail = new System.Windows.Forms.TextBox();
            this.linkLabelBlogAddress = new System.Windows.Forms.LinkLabel();
            this.richTextBoxAboutMe = new System.Windows.Forms.RichTextBox();
            this.pictureBoxDisplayPic = new System.Windows.Forms.PictureBox();
            this.labelLocation = new System.Windows.Forms.Label();
            this.textBoxLocation = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.textBoxSetDisplayPicUrl = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxSetBlogAddress = new System.Windows.Forms.TextBox();
            this.buttonEditBlogAddress = new System.Windows.Forms.Button();
            this.radioButtonChinese = new System.Windows.Forms.RadioButton();
            this.radioButtonEnglish = new System.Windows.Forms.RadioButton();
            this.buttonPreview = new System.Windows.Forms.Button();
            this.comboBoxDiploma = new System.Windows.Forms.ComboBox();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxCompany = new System.Windows.Forms.TextBox();
            this.buttonLoadImageFile = new System.Windows.Forms.Button();
            this.openFileDialogImage = new System.Windows.Forms.OpenFileDialog();
            this.labelDiploma = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisplayPic)).BeginInit();
            this.SuspendLayout();
            // 
            // labelUserName
            // 
            this.labelUserName.AutoSize = true;
            this.labelUserName.Location = new System.Drawing.Point(8, 37);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(41, 12);
            this.labelUserName.TabIndex = 0;
            this.labelUserName.Text = "姓名：";
            // 
            // labelCellphone
            // 
            this.labelCellphone.AutoSize = true;
            this.labelCellphone.Location = new System.Drawing.Point(6, 62);
            this.labelCellphone.Name = "labelCellphone";
            this.labelCellphone.Size = new System.Drawing.Size(65, 12);
            this.labelCellphone.TabIndex = 1;
            this.labelCellphone.Text = "移动电话：";
            // 
            // labelTelephone
            // 
            this.labelTelephone.AutoSize = true;
            this.labelTelephone.Location = new System.Drawing.Point(8, 87);
            this.labelTelephone.Name = "labelTelephone";
            this.labelTelephone.Size = new System.Drawing.Size(65, 12);
            this.labelTelephone.TabIndex = 2;
            this.labelTelephone.Text = "固定电话：";
            // 
            // labelQQ
            // 
            this.labelQQ.AutoSize = true;
            this.labelQQ.Location = new System.Drawing.Point(8, 112);
            this.labelQQ.Name = "labelQQ";
            this.labelQQ.Size = new System.Drawing.Size(29, 12);
            this.labelQQ.TabIndex = 3;
            this.labelQQ.Text = "QQ：";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(8, 137);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(53, 12);
            this.labelEmail.TabIndex = 4;
            this.labelEmail.Text = "E-mail：";
            // 
            // labelBlogAddress
            // 
            this.labelBlogAddress.AutoSize = true;
            this.labelBlogAddress.Location = new System.Drawing.Point(8, 236);
            this.labelBlogAddress.Name = "labelBlogAddress";
            this.labelBlogAddress.Size = new System.Drawing.Size(41, 12);
            this.labelBlogAddress.TabIndex = 5;
            this.labelBlogAddress.Text = "网址：";
            // 
            // labelProfile
            // 
            this.labelProfile.AutoSize = true;
            this.labelProfile.Location = new System.Drawing.Point(8, 285);
            this.labelProfile.Name = "labelProfile";
            this.labelProfile.Size = new System.Drawing.Size(41, 12);
            this.labelProfile.TabIndex = 6;
            this.labelProfile.Text = "简介：";
            // 
            // textBoxUserName
            // 
            this.textBoxUserName.Location = new System.Drawing.Point(82, 34);
            this.textBoxUserName.Name = "textBoxUserName";
            this.textBoxUserName.Size = new System.Drawing.Size(139, 21);
            this.textBoxUserName.TabIndex = 7;
            this.toolTip1.SetToolTip(this.textBoxUserName, "必填字段");
            this.textBoxUserName.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // textBoxCellphone
            // 
            this.textBoxCellphone.Location = new System.Drawing.Point(82, 59);
            this.textBoxCellphone.Name = "textBoxCellphone";
            this.textBoxCellphone.Size = new System.Drawing.Size(139, 21);
            this.textBoxCellphone.TabIndex = 8;
            this.toolTip1.SetToolTip(this.textBoxCellphone, "数字");
            this.textBoxCellphone.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // textBoxTelephone
            // 
            this.textBoxTelephone.Location = new System.Drawing.Point(82, 84);
            this.textBoxTelephone.Name = "textBoxTelephone";
            this.textBoxTelephone.Size = new System.Drawing.Size(139, 21);
            this.textBoxTelephone.TabIndex = 9;
            this.textBoxTelephone.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // textBoxQQ
            // 
            this.textBoxQQ.Location = new System.Drawing.Point(82, 109);
            this.textBoxQQ.Name = "textBoxQQ";
            this.textBoxQQ.Size = new System.Drawing.Size(139, 21);
            this.textBoxQQ.TabIndex = 10;
            this.toolTip1.SetToolTip(this.textBoxQQ, "仅为数字");
            this.textBoxQQ.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // textBoxEmail
            // 
            this.textBoxEmail.Location = new System.Drawing.Point(82, 134);
            this.textBoxEmail.Name = "textBoxEmail";
            this.textBoxEmail.Size = new System.Drawing.Size(139, 21);
            this.textBoxEmail.TabIndex = 11;
            this.toolTip1.SetToolTip(this.textBoxEmail, "中间须有\'@\'字符");
            this.textBoxEmail.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // linkLabelBlogAddress
            // 
            this.linkLabelBlogAddress.AutoSize = true;
            this.linkLabelBlogAddress.Location = new System.Drawing.Point(80, 236);
            this.linkLabelBlogAddress.Name = "linkLabelBlogAddress";
            this.linkLabelBlogAddress.Size = new System.Drawing.Size(155, 12);
            this.linkLabelBlogAddress.TabIndex = 12;
            this.linkLabelBlogAddress.TabStop = true;
            this.linkLabelBlogAddress.Text = "DefaultString:BlogAddress";
            this.toolTip1.SetToolTip(this.linkLabelBlogAddress, "单位网址，或您的博客地址");
            this.linkLabelBlogAddress.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelBlogAddress_LinkClicked);
            // 
            // richTextBoxAboutMe
            // 
            this.richTextBoxAboutMe.Location = new System.Drawing.Point(8, 300);
            this.richTextBoxAboutMe.Name = "richTextBoxAboutMe";
            this.richTextBoxAboutMe.Size = new System.Drawing.Size(386, 108);
            this.richTextBoxAboutMe.TabIndex = 13;
            this.richTextBoxAboutMe.Text = "";
            this.toolTip1.SetToolTip(this.richTextBoxAboutMe, "可以是您自己的个人简介，或您的单位的简介。");
            this.richTextBoxAboutMe.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBoxAboutMe_LinkClicked);
            this.richTextBoxAboutMe.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // pictureBoxDisplayPic
            // 
            this.pictureBoxDisplayPic.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBoxDisplayPic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxDisplayPic.Location = new System.Drawing.Point(227, 34);
            this.pictureBoxDisplayPic.Name = "pictureBoxDisplayPic";
            this.pictureBoxDisplayPic.Size = new System.Drawing.Size(169, 146);
            this.pictureBoxDisplayPic.TabIndex = 14;
            this.pictureBoxDisplayPic.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxDisplayPic, "单位LOGO，或您的头像皆可");
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(8, 261);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(41, 12);
            this.labelLocation.TabIndex = 15;
            this.labelLocation.Text = "地址：";
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.Location = new System.Drawing.Point(82, 258);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.Size = new System.Drawing.Size(312, 21);
            this.textBoxLocation.TabIndex = 16;
            this.toolTip1.SetToolTip(this.textBoxLocation, "您的单位所在地址");
            this.textBoxLocation.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(63, 420);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 17;
            this.buttonSave.Text = "保存";
            this.toolTip1.SetToolTip(this.buttonSave, "点此保存入本地数据库");
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(275, 420);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 18;
            this.buttonCancel.Text = "退出";
            this.toolTip1.SetToolTip(this.buttonCancel, "点此退出，不管是否保存");
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxSetDisplayPicUrl
            // 
            this.textBoxSetDisplayPicUrl.Location = new System.Drawing.Point(340, 186);
            this.textBoxSetDisplayPicUrl.Multiline = true;
            this.textBoxSetDisplayPicUrl.Name = "textBoxSetDisplayPicUrl";
            this.textBoxSetDisplayPicUrl.Size = new System.Drawing.Size(54, 21);
            this.textBoxSetDisplayPicUrl.TabIndex = 19;
            this.textBoxSetDisplayPicUrl.Text = "FileName";
            this.textBoxSetDisplayPicUrl.Visible = false;
            this.textBoxSetDisplayPicUrl.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // textBoxSetBlogAddress
            // 
            this.textBoxSetBlogAddress.Location = new System.Drawing.Point(82, 233);
            this.textBoxSetBlogAddress.Name = "textBoxSetBlogAddress";
            this.textBoxSetBlogAddress.Size = new System.Drawing.Size(100, 21);
            this.textBoxSetBlogAddress.TabIndex = 20;
            this.textBoxSetBlogAddress.Text = "http://";
            this.toolTip1.SetToolTip(this.textBoxSetBlogAddress, "单位网址，或您的博客地址");
            this.textBoxSetBlogAddress.Visible = false;
            this.textBoxSetBlogAddress.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // buttonEditBlogAddress
            // 
            this.buttonEditBlogAddress.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonEditBlogAddress.Location = new System.Drawing.Point(357, 229);
            this.buttonEditBlogAddress.Name = "buttonEditBlogAddress";
            this.buttonEditBlogAddress.Size = new System.Drawing.Size(37, 23);
            this.buttonEditBlogAddress.TabIndex = 21;
            this.buttonEditBlogAddress.Text = "编辑";
            this.toolTip1.SetToolTip(this.buttonEditBlogAddress, "点我可编辑“网址”项");
            this.buttonEditBlogAddress.UseVisualStyleBackColor = true;
            this.buttonEditBlogAddress.Click += new System.EventHandler(this.buttonEditBlogAddress_Click);
            // 
            // radioButtonChinese
            // 
            this.radioButtonChinese.AutoSize = true;
            this.radioButtonChinese.Checked = true;
            this.radioButtonChinese.Location = new System.Drawing.Point(148, 9);
            this.radioButtonChinese.Name = "radioButtonChinese";
            this.radioButtonChinese.Size = new System.Drawing.Size(83, 16);
            this.radioButtonChinese.TabIndex = 22;
            this.radioButtonChinese.TabStop = true;
            this.radioButtonChinese.Text = "中文版名片";
            this.toolTip1.SetToolTip(this.radioButtonChinese, "编辑您的中文名片项目信息");
            this.radioButtonChinese.UseVisualStyleBackColor = true;
            this.radioButtonChinese.CheckedChanged += new System.EventHandler(this.radioButtonChinese_CheckedChanged);
            // 
            // radioButtonEnglish
            // 
            this.radioButtonEnglish.AutoSize = true;
            this.radioButtonEnglish.Location = new System.Drawing.Point(237, 9);
            this.radioButtonEnglish.Name = "radioButtonEnglish";
            this.radioButtonEnglish.Size = new System.Drawing.Size(149, 16);
            this.radioButtonEnglish.TabIndex = 23;
            this.radioButtonEnglish.Text = "ENGLISH Business Card";
            this.toolTip1.SetToolTip(this.radioButtonEnglish, "Edit Your English Business Card");
            this.radioButtonEnglish.UseVisualStyleBackColor = true;
            this.radioButtonEnglish.CheckedChanged += new System.EventHandler(this.radioButtonEnglish_CheckedChanged);
            // 
            // buttonPreview
            // 
            this.buttonPreview.Location = new System.Drawing.Point(169, 420);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(75, 23);
            this.buttonPreview.TabIndex = 24;
            this.buttonPreview.Text = "名片预览";
            this.toolTip1.SetToolTip(this.buttonPreview, "点此预览中文/英文名片效果");
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // comboBoxDiploma
            // 
            this.comboBoxDiploma.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDiploma.FormattingEnabled = true;
            this.comboBoxDiploma.Items.AddRange(new object[] {
            "保密",
            "中学",
            "大学本科",
            "硕士",
            "博士及以上"});
            this.comboBoxDiploma.Location = new System.Drawing.Point(82, 159);
            this.comboBoxDiploma.Name = "comboBoxDiploma";
            this.comboBoxDiploma.Size = new System.Drawing.Size(139, 20);
            this.comboBoxDiploma.TabIndex = 27;
            this.toolTip1.SetToolTip(this.comboBoxDiploma, "请选择条目");
            this.comboBoxDiploma.SelectedIndexChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // textBoxTitle
            // 
            this.textBoxTitle.Location = new System.Drawing.Point(82, 183);
            this.textBoxTitle.Name = "textBoxTitle";
            this.textBoxTitle.Size = new System.Drawing.Size(139, 21);
            this.textBoxTitle.TabIndex = 29;
            this.toolTip1.SetToolTip(this.textBoxTitle, "您在单位的职称");
            this.textBoxTitle.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // textBoxCompany
            // 
            this.textBoxCompany.Location = new System.Drawing.Point(82, 208);
            this.textBoxCompany.Name = "textBoxCompany";
            this.textBoxCompany.Size = new System.Drawing.Size(139, 21);
            this.textBoxCompany.TabIndex = 31;
            this.toolTip1.SetToolTip(this.textBoxCompany, "您所在单位的名称");
            this.textBoxCompany.TextChanged += new System.EventHandler(this.textBoxUserName_TextChanged);
            // 
            // buttonLoadImageFile
            // 
            this.buttonLoadImageFile.Location = new System.Drawing.Point(227, 184);
            this.buttonLoadImageFile.Name = "buttonLoadImageFile";
            this.buttonLoadImageFile.Size = new System.Drawing.Size(107, 23);
            this.buttonLoadImageFile.TabIndex = 25;
            this.buttonLoadImageFile.Text = "更换 Logo...";
            this.buttonLoadImageFile.UseVisualStyleBackColor = true;
            this.buttonLoadImageFile.Click += new System.EventHandler(this.buttonLoadImageFile_Click);
            // 
            // labelDiploma
            // 
            this.labelDiploma.AutoSize = true;
            this.labelDiploma.Location = new System.Drawing.Point(6, 162);
            this.labelDiploma.Name = "labelDiploma";
            this.labelDiploma.Size = new System.Drawing.Size(41, 12);
            this.labelDiploma.TabIndex = 26;
            this.labelDiploma.Text = "学历：";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(8, 186);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(41, 12);
            this.labelTitle.TabIndex = 28;
            this.labelTitle.Text = "职称：";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Location = new System.Drawing.Point(6, 211);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(65, 12);
            this.labelCompany.TabIndex = 30;
            this.labelCompany.Text = "单位名称：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 15);
            this.label1.TabIndex = 32;
            this.label1.Text = "请选择要修改的名片：";
            // 
            // DisplayCardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 467);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCompany);
            this.Controls.Add(this.labelCompany);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.comboBoxDiploma);
            this.Controls.Add(this.labelDiploma);
            this.Controls.Add(this.buttonLoadImageFile);
            this.Controls.Add(this.buttonPreview);
            this.Controls.Add(this.radioButtonEnglish);
            this.Controls.Add(this.radioButtonChinese);
            this.Controls.Add(this.buttonEditBlogAddress);
            this.Controls.Add(this.textBoxSetBlogAddress);
            this.Controls.Add(this.textBoxSetDisplayPicUrl);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.textBoxLocation);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.pictureBoxDisplayPic);
            this.Controls.Add(this.richTextBoxAboutMe);
            this.Controls.Add(this.linkLabelBlogAddress);
            this.Controls.Add(this.textBoxEmail);
            this.Controls.Add(this.textBoxTelephone);
            this.Controls.Add(this.textBoxCellphone);
            this.Controls.Add(this.textBoxUserName);
            this.Controls.Add(this.labelProfile);
            this.Controls.Add(this.labelBlogAddress);
            this.Controls.Add(this.labelEmail);
            this.Controls.Add(this.labelQQ);
            this.Controls.Add(this.labelTelephone);
            this.Controls.Add(this.labelCellphone);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.textBoxQQ);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(420, 530);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(420, 467);
            this.Name = "DisplayCardForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改我的名片数据";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DisplayCardForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisplayPic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.Label labelCellphone;
        private System.Windows.Forms.Label labelTelephone;
        private System.Windows.Forms.Label labelQQ;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label labelBlogAddress;
        private System.Windows.Forms.Label labelProfile;
        private System.Windows.Forms.TextBox textBoxUserName;
        private System.Windows.Forms.TextBox textBoxCellphone;
        private System.Windows.Forms.TextBox textBoxTelephone;
        private System.Windows.Forms.TextBox textBoxQQ;
        private System.Windows.Forms.TextBox textBoxEmail;
        private System.Windows.Forms.LinkLabel linkLabelBlogAddress;
        private System.Windows.Forms.RichTextBox richTextBoxAboutMe;
        private System.Windows.Forms.PictureBox pictureBoxDisplayPic;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.TextBox textBoxLocation;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxSetDisplayPicUrl;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox textBoxSetBlogAddress;
        private System.Windows.Forms.Button buttonEditBlogAddress;
        private System.Windows.Forms.RadioButton radioButtonChinese;
        private System.Windows.Forms.RadioButton radioButtonEnglish;
        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Button buttonLoadImageFile;
        private System.Windows.Forms.OpenFileDialog openFileDialogImage;
        private System.Windows.Forms.Label labelDiploma;
        private System.Windows.Forms.ComboBox comboBoxDiploma;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.TextBox textBoxCompany;
        private System.Windows.Forms.Label label1;
    }
}