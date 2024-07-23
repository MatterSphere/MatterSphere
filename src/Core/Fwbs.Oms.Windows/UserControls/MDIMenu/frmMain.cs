using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmMain : BaseForm, IMenuMerge
	{
		private Color ClosedColor;
		private Color OpenedColor;
        public static bool PartnerAccess = false;


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
		public frmMenu frmMenu1 = null;
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
		/// Don't Know Don't Care ;-)
		/// </summary>
		private System.ComponentModel.IContainer components;
		public eToolbars OMSToolbars;
		protected System.Windows.Forms.ToolStripMenuItem mnuLogOff;
		protected System.Windows.Forms.ToolStripMenuItem mnuConnect;
		/// <summary>
		/// The History Mdi Active Window Index
		/// </summary>
		private int mdipos = 0;
		/// <summary>
		/// Used to stop the Control Running Live inside Visual Studio When Inherited
		/// IMPORTANT : Set this Field on application Start or nothing Works
		/// </summary>
		public static bool RunLive = false;
		/// <summary>
		/// Stores the Unique Menu Code that identifies all settings
		/// </summary>
		private string _menucode = "";
		/// <summary>
		/// Stores the Enquiry form Name used to Create or Edit a New Folder
		/// </summary>
		private string _addfolder = "";
		protected System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.Timer timWindowCount;
		protected System.Windows.Forms.ToolStripMenuItem mnuHelp;
		protected System.Windows.Forms.ToolStripMenuItem mnuAbout;
		protected ToolStripMenuItem mnuExit;
		protected ToolStripMenuItem mnuGC;
		protected Panel pnlMain;
		private ToolStripSeparator toolStripSeparator1;
		private ToolStripSeparator menuItem2;
		/// <summary>
		/// Stores the Enquiry Form Name used to Create or Edit a Menu Item
		/// </summary>
		private string _additem = "";
		#endregion

		#region Events
		public event OpenFolderEventHandler FolderOpening;


		protected virtual void OnFolderOpening(object sender, OpenFolderEventArgs e)
		{
			if (FolderOpening != null)
				FolderOpening(sender,e);

		}

		public event EventHandler AddRemoveAddinClick;

		public event EventHandler SystemUpdateClick;

		public event EventHandler ApplicationPanelClear;

		public event LinkEventHandler ApplicationLinkClicked;

		public event EventHandler MenuLoaded;

        #region Events

        public event EventHandler frmMainClosing;

        #endregion Events


        public void OnfrmMainClosing()
        {
            if (frmMainClosing != null)
            {
                Delegate[] dels = frmMainClosing.GetInvocationList();
                frmMainClosing(this, EventArgs.Empty);
            }
        }

		protected virtual void OnMenuLoaded(object sender, EventArgs e) 
		{
            if (MenuLoaded != null)
                MenuLoaded(sender, e);
		}

		protected virtual void OnLinkClicked(ucNavCmdButtons sender) 
		{
			if (ApplicationLinkClicked != null)
				ApplicationLinkClicked(sender);
		}

		public event HandledEventHandler GoClick;

		public void OnGoClick(object sender, HandledEventArgs e)
		{
			if (GoClick!= null)
				GoClick(sender,e);
		}

		/// <summary>
		/// Menu Changed Event Passed from the Menu Form
		/// </summary>
		public event EventHandler MenuChanged;

		public void OnMenuChanged(object sender, EventArgs e)
		{
			if (MenuChanged != null)
				MenuChanged(sender,e);
		}

		/// <summary>
		/// Menu Actions passed from the Menu Form
		/// </summary>
		public event MenuEventHandler MenuActioned;

		public void OnMenuActioned(object sender, MenuEventArgs e)
		{
			try
			{
				this.Cursor = Cursors.WaitCursor;
				if (MenuActioned != null)
					MenuActioned(sender,e);
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		public event EventHandler ParentEnabledChanged;

		/// <summary>
		/// When the Parent is Click Folder is Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnParentEnabledChanged(object sender, EventArgs e)
		{
			if (ParentEnabledChanged != null)
				ParentEnabledChanged(sender,e);
		}

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

		public bool ShowMenuDeveloper
		{
			get { return frmMenu1.ucHome.ShowMenuDeveloper; }
			set { frmMenu1.ucHome.ShowMenuDeveloper = value; }
		}

		#endregion
		
		#region Constructors
		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.timStart = new System.Windows.Forms.Timer(this.components);
            this.timWindowCount = new System.Windows.Forms.Timer(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.OMSToolbars = new FWBS.OMS.UI.Windows.eToolbars();
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
            this.pnlMain.SuspendLayout();
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
            // pnlMain
            // 
            this.pnlMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlMain.Controls.Add(this.OMSToolbars);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(0, 24);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(5, 0, 5, 1);
            this.pnlMain.Size = new System.Drawing.Size(944, 28);
            this.pnlMain.TabIndex = 2;
            // 
            // OMSToolbars
            // 
            this.OMSToolbars.BottomDivider = true;
            this.OMSToolbars.ButtonsXML = resources.GetString("OMSToolbars.ButtonsXML");
            this.OMSToolbars.Divider = false;
            this.OMSToolbars.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.OMSToolbars.DropDownArrows = true;
            this.OMSToolbars.ImageLists = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.OMSToolbars.Location = new System.Drawing.Point(5, 1);
            this.OMSToolbars.Name = "OMSToolbars";
            this.OMSToolbars.NavCommandPanel = null;
            this.OMSToolbars.ShowToolTips = true;
            this.OMSToolbars.Size = new System.Drawing.Size(934, 26);
            this.OMSToolbars.TabIndex = 0;
            this.OMSToolbars.TopDivider = false;
            this.OMSToolbars.OMSButtonClick += new FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventHandler(this.OMSToolbars_OMSButtonClick);
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
            // frmMain
            // 
            this.ClientSize = new System.Drawing.Size(944, 642);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.mainMenu1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.mainMenu1;
            this.Name = "frmMain";
            this.Text = FWBS.OMS.Branding.APPLICATION_NAME + " Reports";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MdiChildActivate += new System.EventHandler(this.frmMain_MdiChildActivate);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.mainMenu1.ResumeLayout(false);
            this.mainMenu1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion
		#endregion

		#region Private Events
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
		/// Fires when a MDI Child is Activated
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmMain_MdiChildActivate(object sender, System.EventArgs e)
		{
			if (RunLive) 
			{
                if (this.ActiveMdiChild is FWBS.OMS.UI.Windows.frmMenu)
                {
                    if (frmMenu1 != null && frmMenu1.ucHome != null)
                    {
                        frmMenu1.ucHome.RefreshLast10();
                        frmMenu1.ucHome.RefreshFavorites();
                    }
                }
                // If the Active Child is the Menu turn on only the Menu Group
				OMSToolbars.GroupVisible("Menu",this.ActiveMdiChild is frmMenu);
                this.OMSToolbars.GetButton("tbDetails").Visible = false;
                this.OMSToolbars.GetButton("tbDetails").Pushed = true;
                this.OMSToolbars.GetButton("tbIcons").Visible = false;
				// Use of this Timer is because the MDIChildren.Length does not change if the Form is Closed
				// I need a way of capturing all MDI Child Form Closes
				timWindowCount.Enabled = true;

				// Find the Index of the Child
				for (int x =0; x < this.MdiChildren.Length;x++)
				{
					if (this.ActiveMdiChild == this.MdiChildren[x])
					{
						mdipos = x;
						break;
					}
				}
			}
		}

		/// <summary>
		/// Toggles Forward or Backwards through the Children or Actives the Menu Form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OMSToolbars_OMSButtonClick(object sender, FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventArgs e)
		{
			if (RunLive) 
			{
               	if (e.Button.Name == "tbBack")
				{
					mdipos--;
					if (mdipos < 0) mdipos = this.MdiChildren.Length-1;
					this.MdiChildren[mdipos].BringToFront();
				}
				else if (e.Button.Name == "tbForward")
				{
					mdipos++;
					if (mdipos > this.MdiChildren.Length-1) mdipos = 0;
					this.MdiChildren[mdipos].BringToFront();
				}
				else if (e.Button.Name == "tbDetails")
				{
					OMSToolbars.GetButton("tbDetails").Pushed = true;
					frmMenu1.ucHome.DetailView();
					FWBS.OMS.Favourites fav = new Favourites("LISTVIEW_" + _menucode);
					if (fav.Count == 0)
						fav.AddFavourite("tbDetails", "");
					else
						fav.Description(0, "tbDetails");
					fav.Update();
				}
				else if (e.Button.Name == "tbHome")
				{
					if (frmMenu1 != null)
					{
						if (frmMenu1.ucHome.SearchPanel.Visible)
						{
							frmMenu1.ucHome.SearchPanel.Visible = false;
							if (frmMenu1.ucHome.IsRoot == false)
								OMSToolbars.GetButton("tbParent").Enabled = true;
						}
						else if (this.ActiveMdiChild == frmMenu1)
							frmMenu1.ucHome.RootFolder();
						else
						{
							frmMenu1.Show();
							frmMenu1.BringToFront();
						}
					}
					else
					{
						LoadMenu();
					}

				}
				else if (e.Button.Name == "tbLogon")
				{
					mnuConnect_Click(this, EventArgs.Empty);
				}
			}
		}

		/// <summary>
		/// Fires the Log Off Procedure
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuLogOff_Click(object sender, System.EventArgs e)
		{
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

					//Sets the parent form, just incase the command centre form is visible on login.
					Common.Functions.SetParentWindow(this, Services.MainWindow);
					OMSToolbars.GroupVisible("Logon",false);
					OMSToolbars.GroupVisible("Main",true);
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
			OMSToolbars.Refresh();
			LoadMenu();
		}

		/// <summary>
		/// Loads and Wires up the Menu Form
		/// </summary>
		private void LoadMenu()
		{
			frmMenu1 = new frmMenu(this.MenuCode,this);
            frmMenu1.RightToLeft = this.RightToLeft;
			frmMenu1.MdiParent = this;
			frmMenu1.MenuChanged +=new EventHandler(OnMenuChanged);
			frmMenu1.MenuActioned +=new MenuEventHandler(OnMenuActioned);
			frmMenu1.Closed +=new EventHandler(frmMenu1_Closed);
			frmMenu1.GoClick +=new HandledEventHandler(OnGoClick);
			frmMenu1.ParentEnabledChanged +=new EventHandler(OnParentEnabledChanged);
			frmMenu1.ucHome.AddEditMenuFolder = this.AddEditMenuFolder;
			frmMenu1.ucHome.AddEditItem = this.AddEditItem;
			frmMenu1.ucHome.LabelMenuName.Text = this.Text;
			frmMenu1.ucHome.ApplicationLinkClicked +=new LinkEventHandler(OnLinkClicked);
			frmMenu1.ucHome.SystemUpdateClick +=new EventHandler(ucHome1_SystemUpdateClick);
			frmMenu1.ucHome.AddRemoveAddinClick +=new EventHandler(ucHome1_AddRemoveAddinClick);
			frmMenu1.ucHome.ApplicationPanelClear +=new EventHandler(ucHome1_ApplicationPanelClear);
			frmMenu1.FolderOpening +=new OpenFolderEventHandler(OnFolderOpening);
			OnMenuLoaded(this, EventArgs.Empty);
			frmMenu1.Show();
		}

		/// <summary>
		/// Starts the Start Timer so the UI has time to Draw
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmMain_Load(object sender, System.EventArgs e)
		{
			if (RunLive) timStart.Enabled = true;
		}

		
		/// <summary>
		/// If the Menu is Closed destroy uterly the frmMenu1 Variable
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmMenu1_Closed(object sender, EventArgs e)
		{
			frmMenu1.Dispose();
			frmMenu1 = null;
		}

		
		private void ucHome1_SystemUpdateClick(object sender, EventArgs e)
		{
			if (SystemUpdateClick != null)
				SystemUpdateClick(sender,e);
		}

		private void ucHome1_AddRemoveAddinClick(object sender, EventArgs e)
		{
			if (AddRemoveAddinClick != null)
				AddRemoveAddinClick(sender,e);
		}

		private void timWindowCount_Tick(object sender, System.EventArgs e)
		{
			timWindowCount.Enabled = false;
			OMSToolbars.GetButton("tbBack").Enabled = (this.MdiChildren.Length > 1);
			OMSToolbars.GetButton("tbForward").Enabled = (this.MdiChildren.Length > 1);
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			FWBS.OMS.UI.Windows.Services.ShowAbout();
		}
		#endregion

		#region Properties
		[Category("OMS")]
		public ucHome MenuControl
		{
			get
			{
				return frmMenu1.ucHome;
			}
		}

		/// <summary>
		/// Gets Is the Parent Button Enabled
		/// </summary>
		[Category("OMS")]
		public bool IsParentEnabled
		{
			get
			{
				return frmMenu1.ucHome.IsParentEnabled;
			}
		}

		/// <summary>
		/// Gets or Sets the Unique Menu Code
		/// </summary>
		[Category("OMS")]
		public string MenuCode
		{
			get
			{
				return _menucode;
			}
			set
			{
				_menucode = value;
			}
		}

		/// <summary>
		/// Gets or Sets the Add/Edit Folder Enquiry Form Name
		/// </summary>
		[Category("OMS")]
		[DefaultValue("")]
		public string AddEditMenuFolder
		{
			get
			{
				return _addfolder;
			}
			set
			{
				_addfolder = value;
			}
		}
		
		/// <summary>
		/// Gets or Set the Add/Edit Menu Item Enquiry Form Name
		/// </summary>
		[Category("OMS")]
		[DefaultValue("")]
		public string AddEditItem
		{
			get
			{
				return _additem;
			}
			set
			{
				_additem = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SearchText
		{
			get
			{
				return frmMenu1.ucHome.SearchText;
			}
			set
			{
				frmMenu1.ucHome.SearchText = value;
			}
		}

		public DataTable MenuDataTable
		{
			get
			{
				return frmMenu1.ucHome.MenuDataTable;
			}
		}
		#endregion

		private void ucHome1_ApplicationPanelClear(object sender, EventArgs e)
		{
			if (ApplicationPanelClear != null)
				ApplicationPanelClear(sender,e);
		}

		private void mnuExit_Click(object sender, System.EventArgs e)
		{
			try
			{
				this.Close();
				Application.DoEvents();
				Application.Exit();
			}
			catch
			{
				
			}
		}

		public void Disconnect()
		{
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
				MessageBox.ShowInformation("Garbage Collection Successful...");
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		void frmMain_FormClosed(object sender, FormClosedEventArgs e)
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


    }
}
