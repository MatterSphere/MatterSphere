using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmDockMain : BaseForm, IMenuMerge
	{
		private Color ClosedColor;
        private Color OpenedColor;
        private ucDockMainView mainview;
        public static bool PartnerAccess = false;
        private static FWBS.OMS.Power _power;

        public static FWBS.OMS.Power Power
        {
            get
            {
                return _power;
            }
        }

        public bool sdkaccess
        {
            get { return PartnerAccess; }
            set { PartnerAccess = value; }
        }

		public void MergeMenus(MenuStrip source)
		{
			ToolStripManager.Merge(source, this.mainMenu1);
			mainMenu1.Items.Remove(mnuWindow);
			mainMenu1.Items.Remove(mnuHelp);
			mainMenu1.Items.Add(mnuWindow);
			mainMenu1.Items.Add(mnuHelp);
		}

		public void UnMergeMenus(MenuStrip source)
		{
			ToolStripManager.RevertMerge( this.mainMenu1, source);
			mainMenu1.Items.Clear();
			mainMenu1.Items.Add(mnuFile);
			mainMenu1.Items.Add(mnuWindow);
			mainMenu1.Items.Add(mnuHelp);
		}

		#region Fields
		/// <summary>
		/// The Menu Form
		/// </summary>
		/// <summary>
		/// The Timer that gets all the balls rolling
		/// </summary>
		private System.Windows.Forms.Timer timStart;
		/// <summary>
		/// The Main Menus
		/// </summary>
		protected System.Windows.Forms.MenuStrip mainMenu1;
		protected System.Windows.Forms.ToolStripMenuItem mnuWindow;
        /// <summary>
        /// The Resource Control
        /// </summary>
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;
		protected System.Windows.Forms.ToolStripMenuItem mnuLogOff;
		protected System.Windows.Forms.ToolStripMenuItem mnuConnect;
		/// <summary>
		/// Used to stop the Control Running Live inside Visual Studio When Inherited
		/// IMPORTANT : Set this Field on application Start or nothing Works
		/// </summary>
		public static bool RunLive = false;
		protected System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.Timer timWindowCount;
		protected System.Windows.Forms.ToolStripMenuItem mnuHelp;
		protected System.Windows.Forms.ToolStripMenuItem mnuAbout;
		protected ToolStripMenuItem mnuExit;
        protected ToolStripMenuItem mnuGC;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator menuItem2;
		#endregion



		/// <summary>
		/// Before Disconnect give the option to Cancel Log Off
		/// </summary>
		public event CancelEventHandler BeforeDisconnect;

		public bool OnBeforeDisconnect()
		{
			bool b = false;
			CancelEventArgs e = new CancelEventArgs(b);
			return OnBeforeDisconnect(this,e);
		}
		public bool OnBeforeDisconnect(object sender, CancelEventArgs e)
		{
			if (BeforeDisconnect != null)
			{
				BeforeDisconnect(sender,e);
                return e.Cancel;
			}
			else
				return false;
		}

		/// <summary>
		/// After Connect give the option to Cancel and Terminate
		/// </summary>
		public event CancelEventHandler AfterConnect;

		public bool OnAfterConnect()
		{
			bool b = false;
			CancelEventArgs e = new CancelEventArgs(b);
			return OnAfterConnect(this,e);
		}

		public bool OnAfterConnect(object sender, CancelEventArgs e)
		{
			if (AfterConnect != null)
			{
				AfterConnect(sender,e);
                return e.Cancel;
			}
			else
				return false;
		}

		
		#region Constructors
		public frmDockMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            this.AfterConnect += new CancelEventHandler(frmDockMain_AfterConnect);
            this.BeforeDisconnect += new CancelEventHandler(frmDockMain_BeforeDisconnect);

			this.SuspendLayout();
            foreach (Control ctl in this.Controls)
            {
                var ctlMDI = ctl as MdiClient;
                if (ctlMDI != null)
                {
                    ctlMDI.BackColor = this.BackColor;
                    break;
                }
            }
            foreach (ToolStripMenuItem m in mainMenu1.Items)
            {
                m.ForeColor = ClosedColor;
            }
			this.ResumeLayout();

			if (RunLive)
			{
				FWBS.OMS.UI.Windows.Services.ShowSplash(this, false);
				timStart.Enabled = true;
			}
		}

        public frmDockMain(TreeNavigationActions actions) : this()
        {
            this.actions = actions;
            this.Text = actions.Title;
        }

        public frmDockMain(TreeNavigationActions actions, bool runlive)
            : this()
        {
            this.actions = actions;
            this.Text = actions.Title;
            RunLive = runlive;
        }

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.timStart = new System.Windows.Forms.Timer(this.components);
            this.timWindowCount = new System.Windows.Forms.Timer(this.components);
            this.mainMenu1 = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLogOff = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWindow = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuGC = new System.Windows.Forms.ToolStripMenuItem();
            this.mainview = new FWBS.OMS.UI.Windows.ucDockMainView();
            this.mainMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timStart
            // 
            this.timStart.Tick += new System.EventHandler(this.timStart_Tick);
            // 
            // timWindowCount
            // 
            this.timWindowCount.Tick += new System.EventHandler(this.timWindowCount_Tick);
            // 
            // mainMenu1
            // 
            this.mainMenu1.AllowItemReorder = true;
            this.mainMenu1.ImageScalingSize = new System.Drawing.Size(0, 0);
            this.mainMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuWindow,
            this.mnuHelp});
            this.mainMenu1.Location = new System.Drawing.Point(0, 0);
            this.mainMenu1.MdiWindowListItem = this.mnuWindow;
            this.mainMenu1.Name = "mainMenu1";
            this.mainMenu1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.mainMenu1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.mainMenu1.Size = new System.Drawing.Size(944, 24);
            this.mainMenu1.TabIndex = 4;
            this.mainMenu1.ItemAdded += new System.Windows.Forms.ToolStripItemEventHandler(this.mainMenu1_ItemAdded);
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLogOff,
            this.mnuConnect,
            this.toolStripSeparator1,
            this.mnuExit});
            this.mnuFile.ForeColor = System.Drawing.Color.Black;
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            this.mnuFile.DropDownClosed += new System.EventHandler(this.mnuFile_DropDownClosed);
            this.mnuFile.DropDownOpened += new System.EventHandler(this.mnuFile_DropDownOpened);
            // 
            // mnuLogOff
            // 
            this.mnuLogOff.MergeIndex = 1;
            this.mnuLogOff.Name = "mnuLogOff";
            this.mnuLogOff.Size = new System.Drawing.Size(133, 22);
            this.mnuLogOff.Text = "Disconnect";
            this.mnuLogOff.Click += new System.EventHandler(this.mnuLogOff_Click);
            // 
            // mnuConnect
            // 
            this.mnuConnect.MergeIndex = 1;
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Size = new System.Drawing.Size(133, 22);
            this.mnuConnect.Text = "Connect";
            this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.MergeIndex = 49;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(130, 6);
            // 
            // mnuExit
            // 
            this.mnuExit.MergeIndex = 50;
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(133, 22);
            this.mnuExit.Text = "E&xit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // mnuWindow
            // 
            this.mnuWindow.MergeAction = System.Windows.Forms.MergeAction.Remove;
            this.mnuWindow.MergeIndex = 50;
            this.mnuWindow.Name = "mnuWindow";
            this.mnuWindow.Size = new System.Drawing.Size(63, 20);
            this.mnuWindow.Text = "&Window";
            this.mnuWindow.Visible = false;
            this.mnuWindow.DropDownClosed += new System.EventHandler(this.mnuFile_DropDownClosed);
            this.mnuWindow.DropDownOpened += new System.EventHandler(this.mnuFile_DropDownOpened);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuAbout,
            this.menuItem2,
            this.mnuGC});
            this.mnuHelp.MergeAction = System.Windows.Forms.MergeAction.Remove;
            this.mnuHelp.MergeIndex = 51;
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "Hel&p";
            this.mnuHelp.DropDownClosed += new System.EventHandler(this.mnuFile_DropDownClosed);
            this.mnuHelp.DropDownOpened += new System.EventHandler(this.mnuFile_DropDownOpened);
            // 
            // mnuAbout
            // 
            this.mnuAbout.Name = "mnuAbout";
            this.mnuAbout.Size = new System.Drawing.Size(190, 22);
            this.mnuAbout.Text = "&About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Name = "menuItem2";
            this.menuItem2.Size = new System.Drawing.Size(187, 6);
            // 
            // mnuGC
            // 
            this.mnuGC.Name = "mnuGC";
            this.mnuGC.Size = new System.Drawing.Size(190, 22);
            this.mnuGC.Text = "Force Garbage Collect";
            this.mnuGC.Click += new System.EventHandler(this.mnuGC_Click);
            // 
            // mainview
            // 
            this.mainview.AddEditItem = "";
            this.mainview.AddEditMenuFolder = "";
            this.mainview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainview.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mainview.Location = new System.Drawing.Point(0, 24);
            this.mainview.Name = "mainview";
            this.mainview.ShowMenuDeveloper = false;
            this.mainview.Size = new System.Drawing.Size(944, 618);
            this.mainview.TabIndex = 5;
            this.mainview.Visible = false;
            // 
            // frmDockMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(944, 642);
            this.Controls.Add(this.mainview);
            this.Controls.Add(this.mainMenu1);
            this.MainMenuStrip = this.mainMenu1;
            this.Name = "frmDockMain";
            this.Text = "OMS Base";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmBaseDockMain_FormClosed);
            this.Load += new System.EventHandler(this.frmBaseDockMain_Load);
            this.mainMenu1.ResumeLayout(false);
            this.mainMenu1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}


		#endregion
		#endregion

		#region Private Events

        private void frmDockMain_BeforeDisconnect(object sender, CancelEventArgs e)
        {
            _power = null;
        }

        private void frmDockMain_AfterConnect(object sender, CancelEventArgs e)
        {
            switch (actions.ParentKey.ToUpper())
            {
                case "REPORTS":
                    ReportsViewerPermissionCheck();
                    break;
                case "ADMIN":
                    AdminKitPermissionCheck(e);
                    break;
                default:
                    break;
            }
        }

        
		private void timStart_Tick(object sender, System.EventArgs e)
		{
			if (RunLive) 
			{
				timStart.Enabled = false;
				Application.DoEvents();
				mnuConnect_Click(sender,e);
			}
		}

		/// <summary>
		/// Fires the Log Off Procedure
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuLogOff_Click(object sender, System.EventArgs e)
		{
            if (!this.mainview.CheckForDirtyTabs())
			    Disconnect();
		}

		/// <summary>
		/// Fires the Log On Procedure
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuConnect_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (RunLive) 
				{
					if (!FWBS.OMS.UI.Windows.Services.CheckLogin())
					{
						Close();
						Application.Exit();
						return;
					}

					if (OnAfterConnect())
					{
						return;
					}

                    mnuLogOff.Enabled = true;
                    mnuConnect.Enabled = false;
                    mnuAbout.Enabled = true;
                    mainview.Setup(actions);
                    mainview.Visible = true;
				}
			}
			catch(Exception ex)
			{
				FWBS.OMS.UI.Windows.ErrorBox.Show(this,ex);
				Close();
				Application.Exit();
				return;
			}
			finally
			{
				FWBS.OMS.UI.Windows.Services.CloseSplash();
			}
		}

		/// <summary>
		/// Starts the Start Timer so the UI has time to Draw
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmBaseDockMain_Load(object sender, System.EventArgs e)
		{
			if (RunLive) timStart.Enabled = true;
		}

		private void timWindowCount_Tick(object sender, System.EventArgs e)
		{
			timWindowCount.Enabled = false;
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			FWBS.OMS.UI.Windows.Services.ShowAbout();
		}
		#endregion

        private void ReportsViewerPermissionCheck()
        {
            new SystemPermission(StandardPermissionType.ReportsViewer).Check();
            CultureMenus();
        }

        private void AdminKitPermissionCheck(CancelEventArgs e)
        {
            try
            {
                new SystemPermission(Permission.StandardTypeToString(StandardPermissionType.AdminKit)).Check();
                CultureMenus();
            }
            catch (FWBS.OMS.Security.PermissionsException)
            {
                if (Session.CurrentSession.CurrentUser.IsInRoles("POWER") == false)
                {
                    if (Session.CurrentSession.AdvancedSecurity == false)
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("APROLADKIT", "You must have the Administrator or Power User Role to use the Admin Kit");
                    else
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("APROLADKIT3", "You must have the Administrator Role in Advanced Security or Power User Role to use the Admin Kit");

                    e.Cancel = true;
                    Disconnect();
                }
                else
                {
                    var _power = Session.CurrentSession.CurrentPowerUserSettings;
                    if (_power.IsConfigured == false)
                    {
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("APROLADKIT4", "The Power User settings have not been configured");
                        e.Cancel = true;
                        Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
                e.Cancel = true;
                Disconnect();
            }

        }


        private void CultureMenus()
        {
            this.mnuFile.Text = Session.CurrentSession.Resources.GetResource("mnuFile", this.mnuFile.Text, "").Text;
            this.mnuLogOff.Text = Session.CurrentSession.RegistryRes("Disconnect", this.mnuLogOff.Text);
            this.mnuConnect.Text = Session.CurrentSession.RegistryRes("Connect", this.mnuConnect.Text);
            this.mnuExit.Text = Session.CurrentSession.Resources.GetResource("mnuEXIT", this.mnuExit.Text, "").Text;

            this.mnuWindow.Text = Session.CurrentSession.Resources.GetResource("mnuWindow", this.mnuWindow.Text, "").Text;

            this.mnuHelp.Text = Session.CurrentSession.Resources.GetResource("mnuHelp", this.mnuHelp.Text, "").Text;
            this.mnuAbout.Text = Session.CurrentSession.Resources.GetResource("ABOUT", this.mnuAbout.Text, "").Text;
            this.mnuGC.Text = Session.CurrentSession.Resources.GetResource("ForceGC", this.mnuGC.Text, "").Text;
        }


		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			try
			{
				Application.DoEvents();
				Application.Exit();
            }
			catch
			{
				
			}
		}

		public void Disconnect()
		{
            if (OnBeforeDisconnect())
                return;

            mnuLogOff.Enabled = false;
            mnuConnect.Enabled = true;
            mnuAbout.Enabled = false;
            mainview.Visible = false;
            mainview.RemoveAllTabs();
            mainview.ClearTreeSearch();
            mainview.CloseOpenWindows();
            if(Session.CurrentSession.IsConnected)
                LockState.UnlockObjectsByUser(Session.CurrentSession.CurrentUser.ID);

            if (Session.CurrentSession.IsConnected)
                Session.CurrentSession.Disconnect();
		}

		public virtual object Action(string ActionCmd, string ActionLabel)
		{
			return null;
		}

		private void mnuGC_Click(object sender, EventArgs e)
		{
			try
			{
				System.GC.Collect();
				System.GC.WaitForPendingFinalizers();
				System.GC.Collect();
				MessageBox.ShowInformation(this, "MSGGRBGCLCTED","Garbage Collection Successful...");
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		void frmBaseDockMain_FormClosed(object sender, FormClosedEventArgs e)
		{			
            Disconnect();
		}

		private void mnuFile_DropDownOpened(object sender, EventArgs e)
		{
			((ToolStripMenuItem)sender).ForeColor = OpenedColor;
		}

		private void mnuFile_DropDownClosed(object sender, EventArgs e)
		{
			((ToolStripMenuItem)sender).ForeColor = ClosedColor;

		}

		private void mainMenu1_ItemAdded(object sender, ToolStripItemEventArgs e)
		{
			ToolStripMenuItem ts = e.Item as ToolStripMenuItem;
			if (ts != null)
			{
				ts.ForeColor = ClosedColor;
				ts.DropDownOpened += new EventHandler(mnuFile_DropDownOpened);
				ts.DropDownClosed += new EventHandler(mnuFile_DropDownClosed);
			}
		}


        public virtual Control ConstructAdminElement(string filter, Control parent, string ecmd)
        {
            return null;
        }

        public virtual Control MacroCommands(string ecmd, string filter, out bool result)
        {
            result = false;
            return null;
        }

        private TreeNavigationActions actions { get; set; }
    }
}
