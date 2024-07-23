namespace FWBS.OMS.UI.Windows
{
    partial class ucOMSTypeEmbeded
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        private System.Windows.Forms.ToolBarButton cmdCmdCentre;
        private System.Windows.Forms.ToolBarButton sp1;
        private System.Windows.Forms.ToolBarButton Sp2;
        private System.Windows.Forms.ContextMenu cmenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private FWBS.OMS.UI.StatusBar stBar;
        private System.Windows.Forms.ToolBarButton cmdSearch;
        private System.Windows.Forms.ToolTip toolTip1;
        private FWBS.Common.UI.Windows.ToolBar tbLeft;
        private FWBS.Common.UI.Windows.ToolBar tbRight;
        private System.Windows.Forms.Panel pnlTBRight;
        private System.Windows.Forms.Panel pnlTBLeft;
        private FWBS.OMS.UI.Windows.ThreeDPanel pnlTop;
        private System.Windows.Forms.ToolBarButton cmdBack;
        private System.Windows.Forms.ToolBarButton cmdRefresh;
        private System.Windows.Forms.ToolBarButton cmdSave;
        private System.Windows.Forms.ToolBarButton tbBalance;
        private SearchManager _style = SearchManager.None;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucOMSTypeEmbeded));
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmenu = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.stBar = new FWBS.OMS.UI.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.Blur = new System.Windows.Forms.Button();
            this.ucOMSTypeDefault = new FWBS.OMS.UI.Windows.ucOMSTypeDisplayV2();
            this.pnlTop = new FWBS.OMS.UI.Windows.ThreeDPanel();
            this.pnlTBRight = new System.Windows.Forms.Panel();
            this.tbRight = new FWBS.Common.UI.Windows.ToolBar();
            this.tbBalance = new System.Windows.Forms.ToolBarButton();
            this.cmdSave = new System.Windows.Forms.ToolBarButton();
            this.pnlTBLeft = new System.Windows.Forms.Panel();
            this.tbLeft = new FWBS.Common.UI.Windows.ToolBar();
            this.cmdBack = new System.Windows.Forms.ToolBarButton();
            this.cmdRefresh = new System.Windows.Forms.ToolBarButton();
            this.cmdSearch = new System.Windows.Forms.ToolBarButton();
            this.sp1 = new System.Windows.Forms.ToolBarButton();
            this.cmdCmdCentre = new System.Windows.Forms.ToolBarButton();
            this.Sp2 = new System.Windows.Forms.ToolBarButton();
            this.pnlMain = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            this.pnlTop.SuspendLayout();
            this.pnlTBRight.SuspendLayout();
            this.pnlTBLeft.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuItem1
            // 
            this.menuItem1.Index = -1;
            this.menuItem1.Text = "";
            // 
            // stBar
            // 
            this.stBar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.stBar.Location = new System.Drawing.Point(0, 468);
            this.stBar.Name = "stBar";
            this.stBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1});
            this.stBar.ShowPanels = true;
            this.stBar.Size = new System.Drawing.Size(776, 22);
            this.stBar.TabIndex = 10;
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanel1.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Width = 10;
            // 
            // Blur
            // 
            this.Blur.Location = new System.Drawing.Point(720, -29);
            this.Blur.Name = "Blur";
            this.Blur.Size = new System.Drawing.Size(33, 25);
            this.Blur.TabIndex = 11;
            this.Blur.Text = "Blur";
            this.Blur.UseVisualStyleBackColor = true;
            // 
            // ucOMSTypeDefault
            // 
            this.ucOMSTypeDefault.AlertsVisible = true;
            this.ucOMSTypeDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucOMSTypeDefault.ElasticsearchVisible = false;
            this.ucOMSTypeDefault.InfoPanelCloseVisible = true;
            this.ucOMSTypeDefault.InformationPanelVisible = true;
            this.ucOMSTypeDefault.ipc_BackColor = System.Drawing.Color.White;
            this.ucOMSTypeDefault.ipc_Visible = true;
            this.ucOMSTypeDefault.ipc_Width = 157;
            this.ucOMSTypeDefault.Location = new System.Drawing.Point(4, 4);
            this.ucOMSTypeDefault.Name = "ucOMSTypeDefault";
            this.ucOMSTypeDefault.SearchManagerCloseVisible = true;
            this.ucOMSTypeDefault.SearchManagerVisible = false;
            this.ucOMSTypeDefault.SearchText = null;
            this.ucOMSTypeDefault.Size = new System.Drawing.Size(768, 414);
            this.ucOMSTypeDefault.TabIndex = 0;
            this.ucOMSTypeDefault.TabPositions = System.Windows.Forms.TabAlignment.Top;
            this.ucOMSTypeDefault.ToBeRefreshed = false;
            this.ucOMSTypeDefault.NewOMSTypeWindow += new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.ucOMSTypeDefault_NewOMSTypeWindow);
            this.ucOMSTypeDefault.SearchCompleted += new System.EventHandler(this.ucOMSTypeDefault_SearchCompleted);
            // 
            // pnlTop
            // 
            this.pnlTop.BorderSide = FWBS.OMS.UI.Windows.ThreeDBorder3DSide.Bottom;
            this.pnlTop.BorderStyle = FWBS.OMS.UI.Windows.ThreeDBorder3DStyle.Etched;
            this.pnlTop.Controls.Add(this.pnlTBRight);
            this.pnlTop.Controls.Add(this.pnlTBLeft);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlTop.Location = new System.Drawing.Point(0, 2);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pnlTop.Size = new System.Drawing.Size(776, 44);
            this.pnlTop.TabIndex = 9;
            this.pnlTop.TabStop = false;
            // 
            // pnlTBRight
            // 
            this.pnlTBRight.Controls.Add(this.tbRight);
            this.pnlTBRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTBRight.Location = new System.Drawing.Point(701, 0);
            this.pnlTBRight.Name = "pnlTBRight";
            this.pnlTBRight.Size = new System.Drawing.Size(75, 40);
            this.pnlTBRight.TabIndex = 1;
            // 
            // tbRight
            // 
            this.tbRight.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbRight.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbBalance,
            this.cmdSave});
            this.tbRight.Divider = false;
            this.tbRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbRight.DropDownArrows = true;
            this.tbRight.Location = new System.Drawing.Point(0, 0);
            this.tbRight.Name = "tbRight";
            this.tbRight.ShowToolTips = true;
            this.tbRight.Size = new System.Drawing.Size(75, 34);
            this.tbRight.TabIndex = 0;
            this.tbRight.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbRight.Wrappable = false;
            this.tbRight.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbDialogs_ButtonClick);
            // 
            // tbBalance
            // 
            this.tbBalance.Name = "tbBalance";
            this.tbBalance.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.tbBalance.Text = "Balance";
            this.tbBalance.Visible = false;
            // 
            // cmdSave
            // 
            this.cmdSave.ImageIndex = 2;
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Tag = "cmdSave";
            this.cmdSave.Text = "&Save";
            // 
            // pnlTBLeft
            // 
            this.pnlTBLeft.Controls.Add(this.tbLeft);
            this.pnlTBLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTBLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlTBLeft.Name = "pnlTBLeft";
            this.pnlTBLeft.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.pnlTBLeft.Size = new System.Drawing.Size(482, 40);
            this.pnlTBLeft.TabIndex = 0;
            // 
            // tbLeft
            // 
            this.tbLeft.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbLeft.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.cmdBack,
            this.cmdRefresh,
            this.cmdSearch,
            this.sp1,
            this.cmdCmdCentre,
            this.Sp2});
            this.tbLeft.Divider = false;
            this.tbLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbLeft.DropDownArrows = true;
            this.tbLeft.Location = new System.Drawing.Point(5, 0);
            this.tbLeft.Name = "tbLeft";
            this.tbLeft.ShowToolTips = true;
            this.tbLeft.Size = new System.Drawing.Size(477, 34);
            this.tbLeft.TabIndex = 0;
            this.tbLeft.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbLeft.Wrappable = false;
            this.tbLeft.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbDialogs_ButtonClick);
            // 
            // cmdBack
            // 
            this.cmdBack.DropDownMenu = this.cmenu;
            this.cmdBack.ImageIndex = 17;
            this.cmdBack.Name = "cmdBack";
            this.cmdBack.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.cmdBack.Tag = "cmdBack";
            this.cmdBack.Text = "&Back";
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.ImageIndex = 22;
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Tag = "cmdRefresh";
            this.cmdRefresh.Text = "&Refresh";
            // 
            // cmdSearch
            // 
            this.cmdSearch.ImageIndex = 10;
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.cmdSearch.Text = "Se&arch";
            // 
            // sp1
            // 
            this.sp1.Name = "sp1";
            this.sp1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // cmdCmdCentre
            // 
            this.cmdCmdCentre.ImageIndex = 27;
            this.cmdCmdCentre.Name = "cmdCmdCentre";
            this.cmdCmdCentre.Text = "&Command Centre";
            // 
            // Sp2
            // 
            this.Sp2.Name = "Sp2";
            this.Sp2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // pnlMain
            // 
            this.pnlMain.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnlMain.BackgroundImage")));
            this.pnlMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlMain.Controls.Add(this.ucOMSTypeDefault);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(0, 46);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(4);
            this.pnlMain.Size = new System.Drawing.Size(776, 422);
            this.pnlMain.TabIndex = 12;
            // 
            // ucOMSTypeEmbeded
            // 
            this.Controls.Add(this.Blur);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.stBar);
            this.DoubleBuffered = true;
            this.Name = "ucOMSTypeEmbeded";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.Size = new System.Drawing.Size(776, 490);
            this.Load += new System.EventHandler(this.ucOMSTypeEmbeded_Load);
            this.ParentChanged += new System.EventHandler(this.ucOMSTypeEmbeded_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            this.pnlTop.ResumeLayout(false);
            this.pnlTBRight.ResumeLayout(false);
            this.pnlTBRight.PerformLayout();
            this.pnlTBLeft.ResumeLayout(false);
            this.pnlTBLeft.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        
        private System.Windows.Forms.Button Blur;
        private ucOMSTypeDisplayV2 ucOMSTypeDefault;
        private System.Windows.Forms.Panel pnlMain;
    }
}
