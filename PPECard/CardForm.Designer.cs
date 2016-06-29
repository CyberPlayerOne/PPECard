namespace PPECard
{
    partial class CardForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CardForm));
            this.labelClose = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.labelName = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.richTextBoxTestOnly = new System.Windows.Forms.RichTextBox();
            this.buttonBack = new System.Windows.Forms.Button();
            this.buttonDetail = new System.Windows.Forms.Button();
            this.linkLabelWebsite = new System.Windows.Forms.LinkLabel();
            this.label14 = new System.Windows.Forms.Label();
            this.labelEmail = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.labelQQ = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.labelTelephone = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.labelCellphone = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelCompany = new System.Windows.Forms.Label();
            this.buttonCn = new System.Windows.Forms.Button();
            this.buttonEn = new System.Windows.Forms.Button();
            this.transparentRichTextBox1 = new PPECard.TransparentRichTextBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelClose
            // 
            this.labelClose.AutoSize = true;
            this.labelClose.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelClose.ImageIndex = 0;
            this.labelClose.ImageList = this.imageList1;
            this.labelClose.Location = new System.Drawing.Point(527, 4);
            this.labelClose.Name = "labelClose";
            this.labelClose.Size = new System.Drawing.Size(15, 14);
            this.labelClose.TabIndex = 0;
            this.labelClose.Text = " ";
            this.labelClose.Visible = false;
            this.labelClose.MouseLeave += new System.EventHandler(this.labelClose_MouseLeave);
            this.labelClose.Click += new System.EventHandler(this.labelClose_Click);
            this.labelClose.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelClose_MouseDown);
            this.labelClose.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelClose_MouseUp);
            this.labelClose.MouseEnter += new System.EventHandler(this.labelClose_MouseEnter);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1.bmp");
            this.imageList1.Images.SetKeyName(1, "2.bmp");
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("新宋体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelName.Location = new System.Drawing.Point(2, 11);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(126, 35);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "唐小宇";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(6, 69);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(41, 12);
            this.labelTitle.TabIndex = 2;
            this.labelTitle.Text = "总经理";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.pictureBoxLogo);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.buttonDetail);
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Controls.Add(this.linkLabelWebsite);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.labelEmail);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.labelQQ);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.labelTelephone);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.labelCellphone);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.labelLocation);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.labelCompany);
            this.panel1.Location = new System.Drawing.Point(12, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(523, 295);
            this.panel1.TabIndex = 18;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CardForm_MouseDown);
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxLogo.Location = new System.Drawing.Point(295, 0);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(193, 184);
            this.pictureBoxLogo.TabIndex = 49;
            this.pictureBoxLogo.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.transparentRichTextBox1);
            this.panel2.Controls.Add(this.richTextBoxTestOnly);
            this.panel2.Controls.Add(this.buttonBack);
            this.panel2.Location = new System.Drawing.Point(392, 69);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(523, 295);
            this.panel2.TabIndex = 48;
            this.panel2.Visible = false;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CardForm_MouseDown);
            // 
            // richTextBoxTestOnly
            // 
            this.richTextBoxTestOnly.BackColor = System.Drawing.Color.Fuchsia;
            this.richTextBoxTestOnly.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxTestOnly.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxTestOnly.Name = "richTextBoxTestOnly";
            this.richTextBoxTestOnly.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBoxTestOnly.Size = new System.Drawing.Size(517, 257);
            this.richTextBoxTestOnly.TabIndex = 2;
            this.richTextBoxTestOnly.Text = resources.GetString("richTextBoxTestOnly.Text");
            this.richTextBoxTestOnly.Visible = false;
            // 
            // buttonBack
            // 
            this.buttonBack.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBack.Location = new System.Drawing.Point(8, 265);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 1;
            this.buttonBack.Text = "<<正面";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // buttonDetail
            // 
            this.buttonDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonDetail.Location = new System.Drawing.Point(434, 266);
            this.buttonDetail.Name = "buttonDetail";
            this.buttonDetail.Size = new System.Drawing.Size(75, 23);
            this.buttonDetail.TabIndex = 47;
            this.buttonDetail.Text = "背面>>";
            this.buttonDetail.UseVisualStyleBackColor = true;
            this.buttonDetail.Click += new System.EventHandler(this.buttonDetail_Click);
            // 
            // linkLabelWebsite
            // 
            this.linkLabelWebsite.AutoSize = true;
            this.linkLabelWebsite.Location = new System.Drawing.Point(63, 247);
            this.linkLabelWebsite.Name = "linkLabelWebsite";
            this.linkLabelWebsite.Size = new System.Drawing.Size(131, 12);
            this.linkLabelWebsite.TabIndex = 46;
            this.linkLabelWebsite.TabStop = true;
            this.linkLabelWebsite.Text = "http://www.Google.com";
            this.linkLabelWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelWebsite_LinkClicked);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 247);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 45;
            this.label14.Text = "网 址:";
            // 
            // labelEmail
            // 
            this.labelEmail.AutoSize = true;
            this.labelEmail.Location = new System.Drawing.Point(167, 231);
            this.labelEmail.Name = "labelEmail";
            this.labelEmail.Size = new System.Drawing.Size(95, 12);
            this.labelEmail.TabIndex = 44;
            this.labelEmail.Text = "Lonewar@163.com";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(111, 231);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 43;
            this.label12.Text = "EMAIL:";
            // 
            // labelQQ
            // 
            this.labelQQ.AutoSize = true;
            this.labelQQ.Location = new System.Drawing.Point(34, 231);
            this.labelQQ.Name = "labelQQ";
            this.labelQQ.Size = new System.Drawing.Size(59, 12);
            this.labelQQ.TabIndex = 42;
            this.labelQQ.Text = "348919669";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 231);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 12);
            this.label10.TabIndex = 41;
            this.label10.Text = "QQ:";
            // 
            // labelTelephone
            // 
            this.labelTelephone.AutoSize = true;
            this.labelTelephone.Location = new System.Drawing.Point(167, 219);
            this.labelTelephone.Name = "labelTelephone";
            this.labelTelephone.Size = new System.Drawing.Size(89, 12);
            this.labelTelephone.TabIndex = 40;
            this.labelTelephone.Text = "0532 - 6292561";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(111, 219);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 39;
            this.label8.Text = "固定电话:";
            // 
            // labelCellphone
            // 
            this.labelCellphone.AutoSize = true;
            this.labelCellphone.Location = new System.Drawing.Point(34, 219);
            this.labelCellphone.Name = "labelCellphone";
            this.labelCellphone.Size = new System.Drawing.Size(71, 12);
            this.labelCellphone.TabIndex = 38;
            this.labelCellphone.Text = "15969806865";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 219);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 37;
            this.label6.Text = "手机:";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(63, 202);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(425, 12);
            this.labelLocation.TabIndex = 36;
            this.labelLocation.Text = "北京市太平洋计算机包装设计,请柬设计,专业印刷,北京网站建设,网站建设公司";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 35;
            this.label4.Text = "地 址:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
            this.label3.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.label3.Location = new System.Drawing.Point(4, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(347, 12);
            this.label3.TabIndex = 34;
            this.label3.Text = "                                                         ";
            // 
            // labelCompany
            // 
            this.labelCompany.AutoSize = true;
            this.labelCompany.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCompany.Location = new System.Drawing.Point(3, 176);
            this.labelCompany.Name = "labelCompany";
            this.labelCompany.Size = new System.Drawing.Size(173, 14);
            this.labelCompany.TabIndex = 33;
            this.labelCompany.Text = "太平洋计算机公司(北京)";
            // 
            // buttonCn
            // 
            this.buttonCn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCn.Location = new System.Drawing.Point(12, 12);
            this.buttonCn.Name = "buttonCn";
            this.buttonCn.Size = new System.Drawing.Size(57, 21);
            this.buttonCn.TabIndex = 19;
            this.buttonCn.Text = "中文";
            this.buttonCn.UseVisualStyleBackColor = true;
            this.buttonCn.Click += new System.EventHandler(this.buttonCn_Click);
            // 
            // buttonEn
            // 
            this.buttonEn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEn.Location = new System.Drawing.Point(83, 12);
            this.buttonEn.Name = "buttonEn";
            this.buttonEn.Size = new System.Drawing.Size(57, 21);
            this.buttonEn.TabIndex = 20;
            this.buttonEn.Text = "ENGLISH";
            this.buttonEn.UseVisualStyleBackColor = true;
            this.buttonEn.Click += new System.EventHandler(this.buttonEn_Click);
            // 
            // transparentRichTextBox1
            // 
            this.transparentRichTextBox1.Location = new System.Drawing.Point(28, 121);
            this.transparentRichTextBox1.Name = "transparentRichTextBox1";
            this.transparentRichTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.transparentRichTextBox1.Size = new System.Drawing.Size(100, 96);
            this.transparentRichTextBox1.TabIndex = 3;
            this.transparentRichTextBox1.Tag = "tip:这个控件在运行时定位和设置大小!!!";
            this.transparentRichTextBox1.Text = "";
            this.transparentRichTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.transparentRichTextBox1_LinkClicked);
            // 
            // CardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(547, 370);
            this.ControlBox = false;
            this.Controls.Add(this.buttonEn);
            this.Controls.Add(this.buttonCn);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.labelClose);
            this.DoubleBuffered = true;
            this.Name = "CardForm";
            this.ShowInTaskbar = false;
            this.MouseEnter += new System.EventHandler(this.CardForm_MouseEnter);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CardForm_FormClosed);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CardForm_MouseDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelClose;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonDetail;
        private System.Windows.Forms.LinkLabel linkLabelWebsite;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label labelEmail;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label labelQQ;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label labelTelephone;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelCellphone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label labelLocation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelCompany;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.RichTextBox richTextBoxTestOnly;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private TransparentRichTextBox transparentRichTextBox1;
        private System.Windows.Forms.Button buttonCn;
        private System.Windows.Forms.Button buttonEn;
        private System.Windows.Forms.ImageList imageList1;
    }
}