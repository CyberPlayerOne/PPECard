namespace PPECard
{
    partial class MainForm
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
            try { base.Dispose(disposing); }
            catch { }
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("好友", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("陌生人", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("当前检索结果", System.Windows.Forms.HorizontalAlignment.Left);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.状态ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上线ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.下线ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MyCardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.操作LToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.同右键ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更新名片GToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除名片SToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.请求此人的名片QToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.向他发送我的名片XToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.将电子ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xML格式XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.软件运行日志RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.错误日志EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开发日志KToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助主题ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listViewUsers = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.查看名片CToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.更新名片GToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除名片SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.请求此人的名片QToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.向他发送我的名片XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.图标排列ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.分组显示FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.将此人的电子名片导出为XML格式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.XML格式ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListLarge = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.timerUdpBroadcast = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelExceptions = new System.Windows.Forms.ToolStripStatusLabel();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.timerShowStatusLabelStatus = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFind = new System.Windows.Forms.TextBox();
            this.listViewFind = new System.Windows.Forms.ListView();
            this.timerAutoDock = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.状态ToolStripMenuItem,
            this.MyCardToolStripMenuItem,
            this.操作LToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(373, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 状态ToolStripMenuItem
            // 
            this.状态ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.上线ToolStripMenuItem1,
            this.下线ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.退出XToolStripMenuItem});
            this.状态ToolStripMenuItem.Name = "状态ToolStripMenuItem";
            this.状态ToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.状态ToolStripMenuItem.Text = "状态(&S)";
            // 
            // 上线ToolStripMenuItem1
            // 
            this.上线ToolStripMenuItem1.Checked = true;
            this.上线ToolStripMenuItem1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.上线ToolStripMenuItem1.Name = "上线ToolStripMenuItem1";
            this.上线ToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.上线ToolStripMenuItem1.Text = "上线(&N)";
            this.上线ToolStripMenuItem1.Click += new System.EventHandler(this.上线ToolStripMenuItem1_Click);
            // 
            // 下线ToolStripMenuItem
            // 
            this.下线ToolStripMenuItem.Name = "下线ToolStripMenuItem";
            this.下线ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.下线ToolStripMenuItem.Text = "下线(&F)";
            this.下线ToolStripMenuItem.Click += new System.EventHandler(this.下线ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(109, 6);
            // 
            // 退出XToolStripMenuItem
            // 
            this.退出XToolStripMenuItem.Name = "退出XToolStripMenuItem";
            this.退出XToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.退出XToolStripMenuItem.Text = "退出(&X)";
            this.退出XToolStripMenuItem.Click += new System.EventHandler(this.退出XToolStripMenuItem_Click);
            // 
            // MyCardToolStripMenuItem
            // 
            this.MyCardToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.修改ToolStripMenuItem});
            this.MyCardToolStripMenuItem.Name = "MyCardToolStripMenuItem";
            this.MyCardToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.MyCardToolStripMenuItem.Text = "我的名片(&C)";
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("修改ToolStripMenuItem.Image")));
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.修改ToolStripMenuItem.Text = "修改我的名片(&M)";
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.修改ToolStripMenuItem_Click);
            // 
            // 操作LToolStripMenuItem
            // 
            this.操作LToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.同右键ToolStripMenuItem,
            this.更新名片GToolStripMenuItem1,
            this.删除名片SToolStripMenuItem1,
            this.请求此人的名片QToolStripMenuItem1,
            this.向他发送我的名片XToolStripMenuItem1,
            this.toolStripMenuItem4,
            this.将电子ToolStripMenuItem});
            this.操作LToolStripMenuItem.Name = "操作LToolStripMenuItem";
            this.操作LToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.操作LToolStripMenuItem.Text = "操作(&L)";
            this.操作LToolStripMenuItem.DropDownOpening += new System.EventHandler(this.操作LToolStripMenuItem_DropDownOpening);
            // 
            // 同右键ToolStripMenuItem
            // 
            this.同右键ToolStripMenuItem.Name = "同右键ToolStripMenuItem";
            this.同右键ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.同右键ToolStripMenuItem.Text = "查看名片(&C)";
            this.同右键ToolStripMenuItem.Click += new System.EventHandler(this.查看名片CToolStripMenuItem_Click);
            // 
            // 更新名片GToolStripMenuItem1
            // 
            this.更新名片GToolStripMenuItem1.Name = "更新名片GToolStripMenuItem1";
            this.更新名片GToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.更新名片GToolStripMenuItem1.Text = "更新名片(&G)";
            this.更新名片GToolStripMenuItem1.Click += new System.EventHandler(this.更新名片GToolStripMenuItem_Click);
            // 
            // 删除名片SToolStripMenuItem1
            // 
            this.删除名片SToolStripMenuItem1.Name = "删除名片SToolStripMenuItem1";
            this.删除名片SToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.删除名片SToolStripMenuItem1.Text = "删除名片(&S)";
            this.删除名片SToolStripMenuItem1.Click += new System.EventHandler(this.删除名片SToolStripMenuItem_Click);
            // 
            // 请求此人的名片QToolStripMenuItem1
            // 
            this.请求此人的名片QToolStripMenuItem1.Name = "请求此人的名片QToolStripMenuItem1";
            this.请求此人的名片QToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.请求此人的名片QToolStripMenuItem1.Text = "请求此人的名片(&Q)";
            this.请求此人的名片QToolStripMenuItem1.Click += new System.EventHandler(this.请求此人的名片QToolStripMenuItem_Click);
            // 
            // 向他发送我的名片XToolStripMenuItem1
            // 
            this.向他发送我的名片XToolStripMenuItem1.Name = "向他发送我的名片XToolStripMenuItem1";
            this.向他发送我的名片XToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
            this.向他发送我的名片XToolStripMenuItem1.Text = "向他发送我的名片(&X)";
            this.向他发送我的名片XToolStripMenuItem1.Click += new System.EventHandler(this.向他发送我的名片XToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(181, 6);
            // 
            // 将电子ToolStripMenuItem
            // 
            this.将电子ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xML格式XToolStripMenuItem});
            this.将电子ToolStripMenuItem.Name = "将电子ToolStripMenuItem";
            this.将电子ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.将电子ToolStripMenuItem.Text = "名片导出为(&D)";
            // 
            // xML格式XToolStripMenuItem
            // 
            this.xML格式XToolStripMenuItem.Name = "xML格式XToolStripMenuItem";
            this.xML格式XToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.xML格式XToolStripMenuItem.Text = "XML格式(&X)";
            this.xML格式XToolStripMenuItem.Click += new System.EventHandler(this.XML格式ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.软件运行日志RToolStripMenuItem,
            this.错误日志EToolStripMenuItem,
            this.开发日志KToolStripMenuItem,
            this.帮助主题ToolStripMenuItem,
            this.关于AToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.帮助ToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 软件运行日志RToolStripMenuItem
            // 
            this.软件运行日志RToolStripMenuItem.Name = "软件运行日志RToolStripMenuItem";
            this.软件运行日志RToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.软件运行日志RToolStripMenuItem.Text = "软件运行日志(&R)";
            this.软件运行日志RToolStripMenuItem.Click += new System.EventHandler(this.软件运行日志RToolStripMenuItem_Click);
            // 
            // 错误日志EToolStripMenuItem
            // 
            this.错误日志EToolStripMenuItem.Name = "错误日志EToolStripMenuItem";
            this.错误日志EToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.错误日志EToolStripMenuItem.Text = "错误日志(&C)";
            this.错误日志EToolStripMenuItem.Click += new System.EventHandler(this.错误日志EToolStripMenuItem_Click);
            // 
            // 开发日志KToolStripMenuItem
            // 
            this.开发日志KToolStripMenuItem.Name = "开发日志KToolStripMenuItem";
            this.开发日志KToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.开发日志KToolStripMenuItem.Text = "开发日志(&K)";
            this.开发日志KToolStripMenuItem.Click += new System.EventHandler(this.开发日志KToolStripMenuItem_Click);
            // 
            // 帮助主题ToolStripMenuItem
            // 
            this.帮助主题ToolStripMenuItem.Name = "帮助主题ToolStripMenuItem";
            this.帮助主题ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.帮助主题ToolStripMenuItem.Text = "帮助主题(&H)";
            this.帮助主题ToolStripMenuItem.Click += new System.EventHandler(this.帮助主题ToolStripMenuItem_Click);
            // 
            // 关于AToolStripMenuItem
            // 
            this.关于AToolStripMenuItem.Name = "关于AToolStripMenuItem";
            this.关于AToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.关于AToolStripMenuItem.Text = "关于(&A)";
            this.关于AToolStripMenuItem.Click += new System.EventHandler(this.关于AToolStripMenuItem_Click);
            // 
            // listViewUsers
            // 
            this.listViewUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewUsers.ContextMenuStrip = this.contextMenuStrip1;
            listViewGroup1.Header = "好友";
            listViewGroup1.Name = "listViewGroupFriends";
            listViewGroup2.Header = "陌生人";
            listViewGroup2.Name = "listViewGroupStrange";
            this.listViewUsers.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2});
            this.listViewUsers.LargeImageList = this.imageListLarge;
            this.listViewUsers.Location = new System.Drawing.Point(0, 25);
            this.listViewUsers.Name = "listViewUsers";
            this.listViewUsers.ShowItemToolTips = true;
            this.listViewUsers.Size = new System.Drawing.Size(373, 423);
            this.listViewUsers.SmallImageList = this.imageListSmall;
            this.listViewUsers.TabIndex = 1;
            this.listViewUsers.UseCompatibleStateImageBehavior = false;
            this.listViewUsers.ItemActivate += new System.EventHandler(this.listViewUsers_ItemActivate);
            this.listViewUsers.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listViewUsers_KeyPress);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看名片CToolStripMenuItem,
            this.更新名片GToolStripMenuItem,
            this.删除名片SToolStripMenuItem,
            this.请求此人的名片QToolStripMenuItem,
            this.向他发送我的名片XToolStripMenuItem,
            this.toolStripMenuItem2,
            this.图标排列ToolStripMenuItem,
            this.toolStripMenuItem3,
            this.分组显示FToolStripMenuItem,
            this.将此人的电子名片导出为XML格式ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(185, 192);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // 查看名片CToolStripMenuItem
            // 
            this.查看名片CToolStripMenuItem.Name = "查看名片CToolStripMenuItem";
            this.查看名片CToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.查看名片CToolStripMenuItem.Text = "查看名片(&C)";
            this.查看名片CToolStripMenuItem.Click += new System.EventHandler(this.查看名片CToolStripMenuItem_Click);
            // 
            // 更新名片GToolStripMenuItem
            // 
            this.更新名片GToolStripMenuItem.Name = "更新名片GToolStripMenuItem";
            this.更新名片GToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.更新名片GToolStripMenuItem.Text = "更新名片(&G)";
            this.更新名片GToolStripMenuItem.Click += new System.EventHandler(this.更新名片GToolStripMenuItem_Click);
            // 
            // 删除名片SToolStripMenuItem
            // 
            this.删除名片SToolStripMenuItem.Name = "删除名片SToolStripMenuItem";
            this.删除名片SToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.删除名片SToolStripMenuItem.Text = "删除名片(&S)";
            this.删除名片SToolStripMenuItem.Click += new System.EventHandler(this.删除名片SToolStripMenuItem_Click);
            // 
            // 请求此人的名片QToolStripMenuItem
            // 
            this.请求此人的名片QToolStripMenuItem.Name = "请求此人的名片QToolStripMenuItem";
            this.请求此人的名片QToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.请求此人的名片QToolStripMenuItem.Text = "请求此人的名片(&Q)";
            this.请求此人的名片QToolStripMenuItem.Click += new System.EventHandler(this.请求此人的名片QToolStripMenuItem_Click);
            // 
            // 向他发送我的名片XToolStripMenuItem
            // 
            this.向他发送我的名片XToolStripMenuItem.Name = "向他发送我的名片XToolStripMenuItem";
            this.向他发送我的名片XToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.向他发送我的名片XToolStripMenuItem.Text = "向他发送我的名片(&X)";
            this.向他发送我的名片XToolStripMenuItem.Click += new System.EventHandler(this.向他发送我的名片XToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 6);
            // 
            // 图标排列ToolStripMenuItem
            // 
            this.图标排列ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.XToolStripMenuItem,
            this.DToolStripMenuItem});
            this.图标排列ToolStripMenuItem.Name = "图标排列ToolStripMenuItem";
            this.图标排列ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.图标排列ToolStripMenuItem.Text = "排列图标(&P)";
            // 
            // XToolStripMenuItem
            // 
            this.XToolStripMenuItem.Name = "XToolStripMenuItem";
            this.XToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.XToolStripMenuItem.Text = "详细信息(&X)";
            this.XToolStripMenuItem.Click += new System.EventHandler(this.XToolStripMenuItem_Click);
            // 
            // DToolStripMenuItem
            // 
            this.DToolStripMenuItem.Checked = true;
            this.DToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DToolStripMenuItem.Name = "DToolStripMenuItem";
            this.DToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.DToolStripMenuItem.Text = "大图标(&D)";
            this.DToolStripMenuItem.Click += new System.EventHandler(this.DToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 6);
            // 
            // 分组显示FToolStripMenuItem
            // 
            this.分组显示FToolStripMenuItem.Checked = true;
            this.分组显示FToolStripMenuItem.CheckOnClick = true;
            this.分组显示FToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.分组显示FToolStripMenuItem.Name = "分组显示FToolStripMenuItem";
            this.分组显示FToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.分组显示FToolStripMenuItem.Text = "分组显示(&F)";
            this.分组显示FToolStripMenuItem.Click += new System.EventHandler(this.分组显示FToolStripMenuItem_Click);
            // 
            // 将此人的电子名片导出为XML格式ToolStripMenuItem
            // 
            this.将此人的电子名片导出为XML格式ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.XML格式ToolStripMenuItem});
            this.将此人的电子名片导出为XML格式ToolStripMenuItem.Name = "将此人的电子名片导出为XML格式ToolStripMenuItem";
            this.将此人的电子名片导出为XML格式ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.将此人的电子名片导出为XML格式ToolStripMenuItem.Text = "将名片导出为(&D)";
            // 
            // XML格式ToolStripMenuItem
            // 
            this.XML格式ToolStripMenuItem.Name = "XML格式ToolStripMenuItem";
            this.XML格式ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.XML格式ToolStripMenuItem.Text = "XML格式(&X)";
            this.XML格式ToolStripMenuItem.Click += new System.EventHandler(this.XML格式ToolStripMenuItem_Click);
            // 
            // imageListLarge
            // 
            this.imageListLarge.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLarge.ImageStream")));
            this.imageListLarge.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListLarge.Images.SetKeyName(0, "avatar10.jpg");
            // 
            // imageListSmall
            // 
            this.imageListSmall.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmall.ImageStream")));
            this.imageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSmall.Images.SetKeyName(0, "avatar10.jpg");
            // 
            // timerUdpBroadcast
            // 
            this.timerUdpBroadcast.Enabled = true;
            this.timerUdpBroadcast.Interval = 1000;
            this.timerUdpBroadcast.Tick += new System.EventHandler(this.timerUdpBroadcast_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripStatusLabelExceptions});
            this.statusStrip1.Location = new System.Drawing.Point(0, 477);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(373, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(54, 17);
            this.toolStripStatusLabel.Text = "Welcome!";
            // 
            // toolStripStatusLabelExceptions
            // 
            this.toolStripStatusLabelExceptions.Name = "toolStripStatusLabelExceptions";
            this.toolStripStatusLabelExceptions.Size = new System.Drawing.Size(0, 17);
            // 
            // timerShowStatusLabelStatus
            // 
            this.timerShowStatusLabelStatus.Enabled = true;
            this.timerShowStatusLabelStatus.Interval = 5000;
            this.timerShowStatusLabelStatus.Tick += new System.EventHandler(this.timerShowStatusLabelStatus_Tick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(-2, 455);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "检索用户:";
            // 
            // textBoxFind
            // 
            this.textBoxFind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFind.Location = new System.Drawing.Point(63, 452);
            this.textBoxFind.Name = "textBoxFind";
            this.textBoxFind.Size = new System.Drawing.Size(310, 21);
            this.textBoxFind.TabIndex = 4;
            this.textBoxFind.TextChanged += new System.EventHandler(this.textBoxFind_TextChanged);
            // 
            // listViewFind
            // 
            this.listViewFind.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewFind.ContextMenuStrip = this.contextMenuStrip1;
            listViewGroup3.Header = "当前检索结果";
            listViewGroup3.Name = "listViewGroupFind";
            this.listViewFind.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup3});
            this.listViewFind.LargeImageList = this.imageListLarge;
            this.listViewFind.Location = new System.Drawing.Point(0, 25);
            this.listViewFind.Name = "listViewFind";
            this.listViewFind.ShowItemToolTips = true;
            this.listViewFind.Size = new System.Drawing.Size(373, 423);
            this.listViewFind.SmallImageList = this.imageListSmall;
            this.listViewFind.TabIndex = 5;
            this.listViewFind.UseCompatibleStateImageBehavior = false;
            this.listViewFind.Visible = false;
            this.listViewFind.ItemActivate += new System.EventHandler(this.listViewUsers_ItemActivate);
            // 
            // timerAutoDock
            // 
            this.timerAutoDock.Tick += new System.EventHandler(this.timerAutoDock_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(373, 499);
            this.Controls.Add(this.listViewFind);
            this.Controls.Add(this.textBoxFind);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listViewUsers);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(8, 35);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PPECard 2008";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 状态ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上线ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem MyCardToolStripMenuItem;
        private System.Windows.Forms.ListView listViewUsers;
        private System.Windows.Forms.Timer timerUdpBroadcast;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 查看名片CToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更新名片GToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除名片SToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 图标排列ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListLarge;
        private System.Windows.Forms.ImageList imageListSmall;
        private System.Windows.Forms.ToolStripMenuItem 下线ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 操作LToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 同右键ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 分组显示FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 将此人的电子名片导出为XML格式ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem XML格式ToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Timer timerShowStatusLabelStatus;
        private System.Windows.Forms.ToolStripMenuItem 请求此人的名片QToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助主题ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于AToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 更新名片GToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 请求此人的名片QToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 将电子ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem xML格式XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除名片SToolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxFind;
        private System.Windows.Forms.ListView listViewFind;
        private System.Windows.Forms.ToolStripMenuItem 向他发送我的名片XToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 向他发送我的名片XToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 软件运行日志RToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 错误日志EToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 开发日志KToolStripMenuItem;
        private System.Windows.Forms.Timer timerAutoDock;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelExceptions;
    }
}

