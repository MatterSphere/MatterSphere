namespace MSCacheMgr
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tbCacheMgr = new System.Windows.Forms.TabControl();
            this.Config = new System.Windows.Forms.TabPage();
            this.pnlWarningBaseBorder = new System.Windows.Forms.Panel();
            this.pnlDocWarningText = new System.Windows.Forms.Panel();
            this.lblWarningMessage3 = new FWBS.Common.UI.Windows.eLabel2();
            this.lblWarningMessage2 = new FWBS.Common.UI.Windows.eLabel2();
            this.lblWarningMessage1 = new System.Windows.Forms.Label();
            this.lblWarningTitle = new System.Windows.Forms.Label();
            this.pnlWarningLeft = new System.Windows.Forms.Panel();
            this.pictureBoxWarning = new System.Windows.Forms.PictureBox();
            this.btnClearCache = new System.Windows.Forms.Button();
            this.chkWriteEvents = new System.Windows.Forms.CheckBox();
            this.radTreeView = new Telerik.WinControls.UI.RadTreeView();
            this.Results = new System.Windows.Forms.TabPage();
            this.lstResults = new FWBS.OMS.UI.ListView();
            this.colConnection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCacheFolder = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.FailedToolStrip = new System.Windows.Forms.ToolStrip();
            this.tsBtnReRun = new System.Windows.Forms.ToolStripButton();
            this.tsBtnFileLocation = new System.Windows.Forms.ToolStripButton();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openFileLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rerunDeletionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tbCacheMgr.SuspendLayout();
            this.Config.SuspendLayout();
            this.pnlWarningBaseBorder.SuspendLayout();
            this.pnlDocWarningText.SuspendLayout();
            this.pnlWarningLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView)).BeginInit();
            this.Results.SuspendLayout();
            this.FailedToolStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tbCacheMgr
            // 
            resources.ApplyResources(this.tbCacheMgr, "tbCacheMgr");
            this.tbCacheMgr.Controls.Add(this.Config);
            this.tbCacheMgr.Controls.Add(this.Results);
            this.errorProvider1.SetError(this.tbCacheMgr, resources.GetString("tbCacheMgr.Error"));
            this.errorProvider1.SetIconAlignment(this.tbCacheMgr, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("tbCacheMgr.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.tbCacheMgr, ((int)(resources.GetObject("tbCacheMgr.IconPadding"))));
            this.tbCacheMgr.Name = "tbCacheMgr";
            this.tbCacheMgr.SelectedIndex = 0;
            // 
            // Config
            // 
            resources.ApplyResources(this.Config, "Config");
            this.Config.Controls.Add(this.pnlWarningBaseBorder);
            this.Config.Controls.Add(this.btnClearCache);
            this.Config.Controls.Add(this.chkWriteEvents);
            this.Config.Controls.Add(this.radTreeView);
            this.errorProvider1.SetError(this.Config, resources.GetString("Config.Error"));
            this.errorProvider1.SetIconAlignment(this.Config, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("Config.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.Config, ((int)(resources.GetObject("Config.IconPadding"))));
            this.Config.Name = "Config";
            this.Config.UseVisualStyleBackColor = true;
            // 
            // pnlWarningBaseBorder
            // 
            resources.ApplyResources(this.pnlWarningBaseBorder, "pnlWarningBaseBorder");
            this.pnlWarningBaseBorder.BackColor = System.Drawing.Color.DarkRed;
            this.pnlWarningBaseBorder.Controls.Add(this.pnlDocWarningText);
            this.pnlWarningBaseBorder.Controls.Add(this.pnlWarningLeft);
            this.errorProvider1.SetError(this.pnlWarningBaseBorder, resources.GetString("pnlWarningBaseBorder.Error"));
            this.errorProvider1.SetIconAlignment(this.pnlWarningBaseBorder, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pnlWarningBaseBorder.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.pnlWarningBaseBorder, ((int)(resources.GetObject("pnlWarningBaseBorder.IconPadding"))));
            this.pnlWarningBaseBorder.Name = "pnlWarningBaseBorder";
            // 
            // pnlDocWarningText
            // 
            resources.ApplyResources(this.pnlDocWarningText, "pnlDocWarningText");
            this.pnlDocWarningText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlDocWarningText.Controls.Add(this.lblWarningMessage3);
            this.pnlDocWarningText.Controls.Add(this.lblWarningMessage2);
            this.pnlDocWarningText.Controls.Add(this.lblWarningMessage1);
            this.pnlDocWarningText.Controls.Add(this.lblWarningTitle);
            this.errorProvider1.SetError(this.pnlDocWarningText, resources.GetString("pnlDocWarningText.Error"));
            this.errorProvider1.SetIconAlignment(this.pnlDocWarningText, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pnlDocWarningText.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.pnlDocWarningText, ((int)(resources.GetObject("pnlDocWarningText.IconPadding"))));
            this.pnlDocWarningText.Name = "pnlDocWarningText";
            // 
            // lblWarningMessage3
            // 
            resources.ApplyResources(this.lblWarningMessage3, "lblWarningMessage3");
            this.lblWarningMessage3.CaptionWidth = 480;
            this.errorProvider1.SetError(this.lblWarningMessage3, resources.GetString("lblWarningMessage3.Error"));
            this.lblWarningMessage3.Format = "";
            this.errorProvider1.SetIconAlignment(this.lblWarningMessage3, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblWarningMessage3.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblWarningMessage3, ((int)(resources.GetObject("lblWarningMessage3.IconPadding"))));
            this.lblWarningMessage3.IsDirty = false;
            this.lblWarningMessage3.Name = "lblWarningMessage3";
            this.lblWarningMessage3.ReadOnly = true;
            this.lblWarningMessage3.Value = null;
            // 
            // lblWarningMessage2
            // 
            resources.ApplyResources(this.lblWarningMessage2, "lblWarningMessage2");
            this.lblWarningMessage2.CaptionWidth = 480;
            this.errorProvider1.SetError(this.lblWarningMessage2, resources.GetString("lblWarningMessage2.Error"));
            this.lblWarningMessage2.Format = "";
            this.errorProvider1.SetIconAlignment(this.lblWarningMessage2, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblWarningMessage2.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblWarningMessage2, ((int)(resources.GetObject("lblWarningMessage2.IconPadding"))));
            this.lblWarningMessage2.IsDirty = false;
            this.lblWarningMessage2.Name = "lblWarningMessage2";
            this.lblWarningMessage2.ReadOnly = true;
            this.lblWarningMessage2.Value = null;
            // 
            // lblWarningMessage1
            // 
            resources.ApplyResources(this.lblWarningMessage1, "lblWarningMessage1");
            this.errorProvider1.SetError(this.lblWarningMessage1, resources.GetString("lblWarningMessage1.Error"));
            this.errorProvider1.SetIconAlignment(this.lblWarningMessage1, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblWarningMessage1.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblWarningMessage1, ((int)(resources.GetObject("lblWarningMessage1.IconPadding"))));
            this.lblWarningMessage1.Name = "lblWarningMessage1";
            // 
            // lblWarningTitle
            // 
            resources.ApplyResources(this.lblWarningTitle, "lblWarningTitle");
            this.errorProvider1.SetError(this.lblWarningTitle, resources.GetString("lblWarningTitle.Error"));
            this.errorProvider1.SetIconAlignment(this.lblWarningTitle, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lblWarningTitle.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lblWarningTitle, ((int)(resources.GetObject("lblWarningTitle.IconPadding"))));
            this.lblWarningTitle.Name = "lblWarningTitle";
            // 
            // pnlWarningLeft
            // 
            resources.ApplyResources(this.pnlWarningLeft, "pnlWarningLeft");
            this.pnlWarningLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlWarningLeft.Controls.Add(this.pictureBoxWarning);
            this.errorProvider1.SetError(this.pnlWarningLeft, resources.GetString("pnlWarningLeft.Error"));
            this.errorProvider1.SetIconAlignment(this.pnlWarningLeft, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pnlWarningLeft.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.pnlWarningLeft, ((int)(resources.GetObject("pnlWarningLeft.IconPadding"))));
            this.pnlWarningLeft.Name = "pnlWarningLeft";
            // 
            // pictureBoxWarning
            // 
            resources.ApplyResources(this.pictureBoxWarning, "pictureBoxWarning");
            this.errorProvider1.SetError(this.pictureBoxWarning, resources.GetString("pictureBoxWarning.Error"));
            this.errorProvider1.SetIconAlignment(this.pictureBoxWarning, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("pictureBoxWarning.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.pictureBoxWarning, ((int)(resources.GetObject("pictureBoxWarning.IconPadding"))));
            this.pictureBoxWarning.Image = global::MSCacheMgr.Properties.Resources.exclamation2b;
            this.pictureBoxWarning.Name = "pictureBoxWarning";
            this.pictureBoxWarning.TabStop = false;
            // 
            // btnClearCache
            // 
            resources.ApplyResources(this.btnClearCache, "btnClearCache");
            this.errorProvider1.SetError(this.btnClearCache, resources.GetString("btnClearCache.Error"));
            this.errorProvider1.SetIconAlignment(this.btnClearCache, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("btnClearCache.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.btnClearCache, ((int)(resources.GetObject("btnClearCache.IconPadding"))));
            this.btnClearCache.Name = "btnClearCache";
            this.btnClearCache.UseVisualStyleBackColor = true;
            this.btnClearCache.Click += new System.EventHandler(this.btnClearCache_Click);
            // 
            // chkWriteEvents
            // 
            resources.ApplyResources(this.chkWriteEvents, "chkWriteEvents");
            this.errorProvider1.SetError(this.chkWriteEvents, resources.GetString("chkWriteEvents.Error"));
            this.errorProvider1.SetIconAlignment(this.chkWriteEvents, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("chkWriteEvents.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.chkWriteEvents, ((int)(resources.GetObject("chkWriteEvents.IconPadding"))));
            this.chkWriteEvents.Name = "chkWriteEvents";
            this.chkWriteEvents.UseVisualStyleBackColor = true;
            // 
            // radTreeView
            // 
            resources.ApplyResources(this.radTreeView, "radTreeView");
            this.radTreeView.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.errorProvider1.SetError(this.radTreeView, resources.GetString("radTreeView.Error"));
            this.errorProvider1.SetIconAlignment(this.radTreeView, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("radTreeView.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.radTreeView, ((int)(resources.GetObject("radTreeView.IconPadding"))));
            this.radTreeView.Name = "radTreeView";
            // 
            // 
            // 
            this.radTreeView.RootElement.ControlBounds = new System.Drawing.Rectangle(8, 8, 267, 407);
            this.radTreeView.SpacingBetweenNodes = -1;
            this.radTreeView.NodeCheckedChanged += new Telerik.WinControls.UI.TreeNodeCheckedEventHandler(this.radTreeView_NodeCheckedChanged);
            this.radTreeView.ThemeName = "Windows8";
            // 
            // Results
            // 
            resources.ApplyResources(this.Results, "Results");
            this.Results.Controls.Add(this.lstResults);
            this.Results.Controls.Add(this.progressBar);
            this.Results.Controls.Add(this.FailedToolStrip);
            this.errorProvider1.SetError(this.Results, resources.GetString("Results.Error"));
            this.errorProvider1.SetIconAlignment(this.Results, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("Results.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.Results, ((int)(resources.GetObject("Results.IconPadding"))));
            this.Results.Name = "Results";
            this.Results.UseVisualStyleBackColor = true;
            // 
            // lstResults
            // 
            resources.ApplyResources(this.lstResults, "lstResults");
            this.lstResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colConnection,
            this.colCacheFolder,
            this.colFile});
            this.errorProvider1.SetError(this.lstResults, resources.GetString("lstResults.Error"));
            this.lstResults.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lstResults.Groups"))),
            ((System.Windows.Forms.ListViewGroup)(resources.GetObject("lstResults.Groups1")))});
            this.lstResults.HideSelection = false;
            this.errorProvider1.SetIconAlignment(this.lstResults, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("lstResults.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.lstResults, ((int)(resources.GetObject("lstResults.IconPadding"))));
            this.lstResults.Name = "lstResults";
            this.lstResults.UseCompatibleStateImageBehavior = false;
            this.lstResults.View = System.Windows.Forms.View.Details;
            this.lstResults.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstResults_MouseClick);
            // 
            // colConnection
            // 
            resources.ApplyResources(this.colConnection, "colConnection");
            // 
            // colCacheFolder
            // 
            resources.ApplyResources(this.colCacheFolder, "colCacheFolder");
            // 
            // colFile
            // 
            resources.ApplyResources(this.colFile, "colFile");
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.errorProvider1.SetError(this.progressBar, resources.GetString("progressBar.Error"));
            this.errorProvider1.SetIconAlignment(this.progressBar, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("progressBar.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.progressBar, ((int)(resources.GetObject("progressBar.IconPadding"))));
            this.progressBar.MarqueeAnimationSpeed = 1;
            this.progressBar.Name = "progressBar";
            this.progressBar.Step = 1;
            // 
            // FailedToolStrip
            // 
            resources.ApplyResources(this.FailedToolStrip, "FailedToolStrip");
            this.errorProvider1.SetError(this.FailedToolStrip, resources.GetString("FailedToolStrip.Error"));
            this.FailedToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.errorProvider1.SetIconAlignment(this.FailedToolStrip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("FailedToolStrip.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.FailedToolStrip, ((int)(resources.GetObject("FailedToolStrip.IconPadding"))));
            this.FailedToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsBtnReRun,
            this.tsBtnFileLocation});
            this.FailedToolStrip.Name = "FailedToolStrip";
            // 
            // tsBtnReRun
            // 
            resources.ApplyResources(this.tsBtnReRun, "tsBtnReRun");
            this.tsBtnReRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnReRun.Name = "tsBtnReRun";
            this.tsBtnReRun.Click += new System.EventHandler(this.tsBtnReRun_Click);
            // 
            // tsBtnFileLocation
            // 
            resources.ApplyResources(this.tsBtnFileLocation, "tsBtnFileLocation");
            this.tsBtnFileLocation.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsBtnFileLocation.Name = "tsBtnFileLocation";
            this.tsBtnFileLocation.Click += new System.EventHandler(this.tsBtnFileLocation_Click);
            // 
            // contextMenuStrip
            // 
            resources.ApplyResources(this.contextMenuStrip, "contextMenuStrip");
            this.errorProvider1.SetError(this.contextMenuStrip, resources.GetString("contextMenuStrip.Error"));
            this.errorProvider1.SetIconAlignment(this.contextMenuStrip, ((System.Windows.Forms.ErrorIconAlignment)(resources.GetObject("contextMenuStrip.IconAlignment"))));
            this.errorProvider1.SetIconPadding(this.contextMenuStrip, ((int)(resources.GetObject("contextMenuStrip.IconPadding"))));
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileLocationToolStripMenuItem,
            this.rerunDeletionToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            // 
            // openFileLocationToolStripMenuItem
            // 
            resources.ApplyResources(this.openFileLocationToolStripMenuItem, "openFileLocationToolStripMenuItem");
            this.openFileLocationToolStripMenuItem.Name = "openFileLocationToolStripMenuItem";
            this.openFileLocationToolStripMenuItem.Click += new System.EventHandler(this.openFileLocationToolStripMenuItem_Click);
            // 
            // rerunDeletionToolStripMenuItem
            // 
            resources.ApplyResources(this.rerunDeletionToolStripMenuItem, "rerunDeletionToolStripMenuItem");
            this.rerunDeletionToolStripMenuItem.Name = "rerunDeletionToolStripMenuItem";
            this.rerunDeletionToolStripMenuItem.Click += new System.EventHandler(this.rerunDeletionToolStripMenuItem_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            resources.ApplyResources(this.errorProvider1, "errorProvider1");
            // 
            // Form1
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tbCacheMgr);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.tbCacheMgr.ResumeLayout(false);
            this.Config.ResumeLayout(false);
            this.Config.PerformLayout();
            this.pnlWarningBaseBorder.ResumeLayout(false);
            this.pnlDocWarningText.ResumeLayout(false);
            this.pnlWarningLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView)).EndInit();
            this.Results.ResumeLayout(false);
            this.Results.PerformLayout();
            this.FailedToolStrip.ResumeLayout(false);
            this.FailedToolStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbCacheMgr;
        private System.Windows.Forms.TabPage Config;
        private Telerik.WinControls.UI.RadTreeView radTreeView;
        private System.Windows.Forms.TabPage Results;
        private System.Windows.Forms.CheckBox chkWriteEvents;
        private System.Windows.Forms.Button btnClearCache;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openFileLocationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rerunDeletionToolStripMenuItem;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
        private System.Windows.Forms.Panel pnlWarningBaseBorder;
        private System.Windows.Forms.Panel pnlDocWarningText;
        private System.Windows.Forms.Panel pnlWarningLeft;
        private System.Windows.Forms.PictureBox pictureBoxWarning;
        private System.Windows.Forms.Label lblWarningTitle;
        private System.Windows.Forms.Label lblWarningMessage1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private FWBS.Common.UI.Windows.eLabel2 lblWarningMessage3;
        private FWBS.Common.UI.Windows.eLabel2 lblWarningMessage2;
        private System.Windows.Forms.ToolStrip FailedToolStrip;
        private System.Windows.Forms.ToolStripButton tsBtnReRun;
        private System.Windows.Forms.ToolStripButton tsBtnFileLocation;
        private System.Windows.Forms.ProgressBar progressBar;
        private FWBS.OMS.UI.ListView lstResults;
        private System.Windows.Forms.ColumnHeader colConnection;
        private System.Windows.Forms.ColumnHeader colCacheFolder;
        private System.Windows.Forms.ColumnHeader colFile;
    }
}

