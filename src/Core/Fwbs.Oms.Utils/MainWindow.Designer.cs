namespace FWBS.OMS.Utils
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.lstQueue = new System.Windows.Forms.ListBox();
            this.tmrRunCommand = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stripDisconnected = new System.Windows.Forms.ToolStripStatusLabel();
            this.stripServer = new System.Windows.Forms.ToolStripStatusLabel();
            this.stripDatabase = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuShow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindowPicker = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuMonitorSession = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMonitorSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMonitorSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDisableNotifications = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDisconnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrSession = new System.Windows.Forms.Timer(this.components);
            this.notifyDialog = new System.Windows.Forms.NotifyIcon(this.components);
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.NotifyFilter = ((System.IO.NotifyFilters)((((System.IO.NotifyFilters.FileName | System.IO.NotifyFilters.DirectoryName) 
            | System.IO.NotifyFilters.LastWrite) 
            | System.IO.NotifyFilters.LastAccess)));
            this.fileSystemWatcher1.SynchronizingObject = this;
            this.fileSystemWatcher1.Changed += new System.IO.FileSystemEventHandler(this.fileSystemWatcher1_Changed);
            this.fileSystemWatcher1.Renamed += new System.IO.RenamedEventHandler(this.fileSystemWatcher1_Renamed);
            // 
            // lstQueue
            // 
            resources.ApplyResources(this.lstQueue, "lstQueue");
            this.lstQueue.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstQueue.FormattingEnabled = true;
            this.lstQueue.Name = "lstQueue";
            this.lstQueue.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstQueue_DrawItem);
            this.lstQueue.DpiChangedAfterParent += new System.EventHandler(this.lstQueue_DpiChangedAfterParent);
            // 
            // tmrRunCommand
            // 
            this.tmrRunCommand.Interval = 500;
            this.tmrRunCommand.Tick += new System.EventHandler(this.tmrRunCommand_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripDisconnected,
            this.stripServer,
            this.stripDatabase});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // stripDisconnected
            // 
            this.stripDisconnected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripDisconnected.Name = "stripDisconnected";
            resources.ApplyResources(this.stripDisconnected, "stripDisconnected");
            // 
            // stripServer
            // 
            this.stripServer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripServer.Name = "stripServer";
            resources.ApplyResources(this.stripServer, "stripServer");
            // 
            // stripDatabase
            // 
            this.stripDatabase.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stripDatabase.Name = "stripDatabase";
            resources.ApplyResources(this.stripDatabase, "stripDatabase");
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip1;
            resources.ApplyResources(this.notifyIcon, "notifyIcon");
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifySave_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShow,
            this.mnuAbout,
            this.mnuWindowPicker,
            this.toolStripSeparator1,
            this.mnuMonitorSession,
            this.mnuMonitorSave,
            this.mnuMonitorSaveAs,
            this.mnuDisableNotifications,
            this.toolStripSeparator2,
            this.mnuConnect,
            this.mnuDisconnect,
            this.toolStripSeparator3,
            this.mnuExit,
            this.mnuRestart});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // mnuShow
            // 
            this.mnuShow.CheckOnClick = true;
            this.mnuShow.Name = "mnuShow";
            resources.ApplyResources(this.mnuShow, "mnuShow");
            this.mnuShow.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            resources.ApplyResources(this.mnuAbout, "mnuAbout");
            this.mnuAbout.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuWindowPicker
            // 
            this.mnuWindowPicker.Name = "mnuWindowPicker";
            resources.ApplyResources(this.mnuWindowPicker, "mnuWindowPicker");
            this.mnuWindowPicker.Click += new System.EventHandler(this.MenuClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // mnuMonitorSession
            // 
            this.mnuMonitorSession.Name = "mnuMonitorSession";
            resources.ApplyResources(this.mnuMonitorSession, "mnuMonitorSession");
            this.mnuMonitorSession.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuMonitorSave
            // 
            this.mnuMonitorSave.Name = "mnuMonitorSave";
            resources.ApplyResources(this.mnuMonitorSave, "mnuMonitorSave");
            this.mnuMonitorSave.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuMonitorSaveAs
            // 
            this.mnuMonitorSaveAs.Name = "mnuMonitorSaveAs";
            resources.ApplyResources(this.mnuMonitorSaveAs, "mnuMonitorSaveAs");
            this.mnuMonitorSaveAs.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuDisableNotifications
            // 
            this.mnuDisableNotifications.Name = "mnuDisableNotifications";
            resources.ApplyResources(this.mnuDisableNotifications, "mnuDisableNotifications");
            this.mnuDisableNotifications.Click += new System.EventHandler(this.MenuClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // mnuConnect
            // 
            this.mnuConnect.Name = "mnuConnect";
            resources.ApplyResources(this.mnuConnect, "mnuConnect");
            this.mnuConnect.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuDisconnect
            // 
            resources.ApplyResources(this.mnuDisconnect, "mnuDisconnect");
            this.mnuDisconnect.Name = "mnuDisconnect";
            this.mnuDisconnect.Click += new System.EventHandler(this.MenuClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            resources.ApplyResources(this.mnuExit, "mnuExit");
            this.mnuExit.Click += new System.EventHandler(this.MenuClick);
            // 
            // mnuRestart
            // 
            this.mnuRestart.Name = "mnuRestart";
            resources.ApplyResources(this.mnuRestart, "mnuRestart");
            this.mnuRestart.Click += new System.EventHandler(this.MenuClick);
            // 
            // tmrSession
            // 
            this.tmrSession.Enabled = true;
            this.tmrSession.Interval = 10000;
            this.tmrSession.Tick += new System.EventHandler(this.tmrSession_Tick);
            // 
            // notifyDialog
            // 
            resources.ApplyResources(this.notifyDialog, "notifyDialog");
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // MainWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lstQueue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "MainWindow";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private System.Windows.Forms.ListBox lstQueue;
        private System.Windows.Forms.Timer tmrRunCommand;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stripDisconnected;
        private System.Windows.Forms.ToolStripStatusLabel stripServer;
        private System.Windows.Forms.ToolStripStatusLabel stripDatabase;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuConnect;
        private System.Windows.Forms.ToolStripMenuItem mnuDisconnect;
        private System.Windows.Forms.ToolStripMenuItem mnuShow;
        private System.Windows.Forms.Timer tmrSession;
        private System.Windows.Forms.ToolStripMenuItem mnuAbout;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitorSession;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem mnuRestart;
        private System.Windows.Forms.NotifyIcon notifyDialog;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitorSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuMonitorSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem mnuWindowPicker;
        private System.Windows.Forms.ToolStripMenuItem mnuDisableNotifications;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}