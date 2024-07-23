using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Data;
using Infragistics.Win;
using Infragistics.Win.UltraWinDock;
using Infragistics.Win.UltraWinTabControl;
using Infragistics.Win.UltraWinTabs;
using Telerik.WinControls.UI;
using swf = System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class ucDockMainView : System.Windows.Forms.UserControl, ITreeViewNavigationHost
	{
		#region Controls
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel pnlMain;
        private FWBS.OMS.UI.Windows.ucPanelNav ucPanelNavMyFav; //my favourties
        private FWBS.OMS.UI.Windows.ucPanelNav ucPanelNavLast10; //last 10
		private FWBS.OMS.UI.Windows.ucNavCommands pnlFavButtons; //used for My Favourites
        private FWBS.OMS.UI.Windows.ucNavCommands pnlLast10Buttons; //used for Last 10
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavCmdButtons10;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavCmdButtons11;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavCmdButtons12;
		private FWBS.OMS.UI.Windows.ucPanelNav ucSystemTasks;
		private FWBS.OMS.UI.Windows.ucNavCommands pnlCommands;
		private FWBS.OMS.UI.Windows.ucNavCmdButtons ucSystemInformation;
		private FWBS.OMS.UI.TabControl tabControl1;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Panel pnlPanels;
        private System.Windows.Forms.Panel pnlBack;
		private System.Windows.Forms.ContextMenu mnuDynamicFav;
        private System.Windows.Forms.ContextMenu mnuLast10;
		private System.Windows.Forms.MenuItem mnuDyRemFav;
		private System.Windows.Forms.Panel pnlUser;
		private System.Windows.Forms.Label labUserCap;
		private System.Windows.Forms.Label labUser;
		private System.Windows.Forms.Panel pnlDatabase;
		private System.Windows.Forms.Label labDatabaseCap;
		private System.Windows.Forms.Label labDatabase;
		private System.Windows.Forms.Panel pnlServer;
		private System.Windows.Forms.Label labServer;
        private System.Windows.Forms.Label labServerCap;
		private System.Windows.Forms.Panel pnlCulture;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label labCulture;
        private System.ComponentModel.IContainer components;
        public System.Windows.Forms.Panel SearchPanel;
        private FWBS.OMS.UI.Windows.ResourceLookup ResourceObject;

        #endregion

		#region Fields
		/// <summary>
		/// The Enquiry Form Name to Add or Edit a Folder
		/// </summary>
		private string _addfolder = "";
		/// <summary>
		/// The Enquiry Form Name to Add or Edit a Menu Item
		/// </summary>
		private string _additem = "";
		/// <summary>
		/// Meunus Parent Options
		/// </summary>
		private ArrayList history = new ArrayList();
		private System.Windows.Forms.MenuItem mnuRefreshFav;
        private System.Windows.Forms.MenuItem mnuRefreshLast10;
        private FWBS.OMS.UI.Windows.ucNavCommands ucNavApplications;
		private FWBS.OMS.UI.Windows.ucNavCmdButtons ucSystemUpdate;
		private System.Windows.Forms.ContextMenu mnuApplications;
		private FWBS.OMS.UI.Windows.ucPanelNav ucApplications;
		private System.Windows.Forms.MenuItem mnuRegApp;
		private System.Windows.Forms.MenuItem mnuRemApp;
        private ColumnHeader colName;
        private Panel panel5;
        public Label LabelMenuName;
        private Button btnSearchGo;
        private TextBox txtSearch;
        public Label labSearch;
        private Infragistics.Win.UltraWinDock.UltraDockManager ultraDockManager1;
        private Infragistics.Win.UltraWinDock.AutoHideControl _ucHome2AutoHideControl;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucHome2UnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucHome2UnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucHome2UnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucHome2UnpinnedTabAreaRight;
        private Panel pnlTreeParent;
        private Panel pnlTreeView;
        private Panel pnlTreeSearch;
        private Panel panel3;
        private eCaptionLine2 eCaptionLine21;
        private Telerik.WinControls.UI.RadTreeView radTreeView;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
		/// <summary>
		/// The Parent Main Form
		/// </summary>
        TreeNavigationBuilder Treenav;
        private UltraTabControl ultraTabControl;
        private UltraTabSharedControlsPage ultraTabSharedControlsPage1;
        private TextBox txtTreeSearch;
        private Button btnRefreshTree;
        private Button btnSearch;
        private TreeNavigationActions actions;
        private Panel pnlDatabaseInfo;
        private swf.ToolTip SearchToolTip;
        private swf.ToolTip RefreshToolTip;

        #endregion

        #region Events

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            SetButtonImages();
            pnlCommands.Refresh();
            ucNavApplications.Refresh();
            pnlLast10Buttons.Refresh();
            pnlFavButtons.Refresh();
        }

        private void SetButtonImages()
        {
            Bitmap imgSearch = global::FWBS.OMS.UI.Properties.Resources._12;
            Bitmap imgRefresh = global::FWBS.OMS.UI.Properties.Resources.Refresh;
            if (DeviceDpi != 96)
            {
                ScaleBitmapLogicalToDevice(ref imgSearch);
                ScaleBitmapLogicalToDevice(ref imgRefresh);
            }
            this.btnSearch.Image = imgSearch;
            this.btnRefreshTree.Image = imgRefresh;
        }

        protected virtual void OnLinkClicked(ucNavCmdButtons sender) 
		{
		    actions.ApplicationLinkClicked(sender);
		}

		/// <summary>
		/// The Event that is Fired when moving up and down the Menu Folders
		/// </summary>
		public event EventHandler MenuChanged;

		public void OnMenuChanged()
		{
			if (MenuChanged != null)
				MenuChanged(this,EventArgs.Empty);
		}

        protected override void OnRightToLeftChanged(EventArgs e)
        {
        }
        #endregion

		#region Properties

		/// <summary>
		/// The Enquiry Form Name to Add or Edit a Folder
		/// </summary>
		[Browsable(false)]
		public string AddEditMenuFolder
		{
			get
			{
				return _addfolder;
			}
			set
			{
				if (value != "")
					_addfolder = value;
			}
		}

        public RadTreeView TreeView
        {
            get { return this.radTreeView; }
        }

        public UltraTabControl TabControl
        {
            get { return this.ultraTabControl; }
        }

        public TreeNavigationActions Actions
        {
            get { return this.actions; }
        }

		/// <summary>
		/// The Enquiry Form Name to Add or Edit a Menu Itme
		/// </summary>
		public string AddEditItem
		{
			get
			{
				return _additem;
			}
			set
			{
				if (value != "")
					_additem = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public string SearchText
		{
			get
			{
				return txtSearch.Text;
			}
			set
			{
				txtSearch.Text = value;
			}
		}

        private bool showmenudeveloper;
        public bool ShowMenuDeveloper
        {
            get { return showmenudeveloper; }
            set { showmenudeveloper = value; }
        }

		public ucNavCommands ApplicationCommands
		{
			get
			{
				return ucNavApplications;
			}
		}

        public FWBS.OMS.UI.Windows.ucNavCmdButtons SystemUpdateButton
        {
            get
            {
                return ucSystemUpdate;
            }
        }

        public ucNavCommands CommandBarPanel
        {
            get
            {
                return pnlCommands;
            }
        }

        #endregion

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.pnlPanels = new System.Windows.Forms.Panel();
            this.pnlDatabaseInfo = new System.Windows.Forms.Panel();
            this.pnlCulture = new System.Windows.Forms.Panel();
            this.labCulture = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlServer = new System.Windows.Forms.Panel();
            this.labServer = new System.Windows.Forms.Label();
            this.labServerCap = new System.Windows.Forms.Label();
            this.pnlDatabase = new System.Windows.Forms.Panel();
            this.labDatabase = new System.Windows.Forms.Label();
            this.labDatabaseCap = new System.Windows.Forms.Label();
            this.pnlUser = new System.Windows.Forms.Panel();
            this.labUser = new System.Windows.Forms.Label();
            this.labUserCap = new System.Windows.Forms.Label();
            this.ucPanelNavMyFav = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.pnlFavButtons = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucPanelNavLast10 = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.pnlLast10Buttons = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucApplications = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.mnuApplications = new System.Windows.Forms.ContextMenu();
            this.mnuRegApp = new System.Windows.Forms.MenuItem();
            this.mnuRemApp = new System.Windows.Forms.MenuItem();
            this.ucNavApplications = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucSystemTasks = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.pnlCommands = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucSystemInformation = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucSystemUpdate = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ultraTabControl = new Infragistics.Win.UltraWinTabControl.UltraTabControl();
            this.ultraTabSharedControlsPage1 = new Infragistics.Win.UltraWinTabControl.UltraTabSharedControlsPage();
            this.pnlTreeParent = new System.Windows.Forms.Panel();
            this.pnlTreeView = new System.Windows.Forms.Panel();
            this.radTreeView = new Telerik.WinControls.UI.RadTreeView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.eCaptionLine21 = new FWBS.Common.UI.Windows.eCaptionLine2();
            this.pnlTreeSearch = new System.Windows.Forms.Panel();
            this.txtTreeSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnRefreshTree = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlBack = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.SearchPanel = new System.Windows.Forms.Panel();
            this.tabControl1 = new FWBS.OMS.UI.TabControl();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuDyRemFav = new System.Windows.Forms.MenuItem();
            this.mnuRefreshFav = new System.Windows.Forms.MenuItem();
            this.mnuDynamicFav = new System.Windows.Forms.ContextMenu();
            this.mnuLast10 = new System.Windows.Forms.ContextMenu();
            this.mnuRefreshLast10 = new System.Windows.Forms.MenuItem();
            this.labSearch = new System.Windows.Forms.Label();
            this.btnSearchGo = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.LabelMenuName = new System.Windows.Forms.Label();
            this.ultraDockManager1 = new Infragistics.Win.UltraWinDock.UltraDockManager(this.components);
            this._ucHome2UnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucHome2UnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucHome2UnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucHome2UnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucHome2AutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.SearchToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.RefreshToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.ucNavCmdButtons10 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucNavCmdButtons11 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucNavCmdButtons12 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ResourceObject = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlPanels.SuspendLayout();
            this.pnlDatabaseInfo.SuspendLayout();
            this.pnlCulture.SuspendLayout();
            this.pnlServer.SuspendLayout();
            this.pnlDatabase.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.ucPanelNavMyFav.SuspendLayout();
            this.ucPanelNavLast10.SuspendLayout();
            this.ucApplications.SuspendLayout();
            this.ucSystemTasks.SuspendLayout();
            this.pnlCommands.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl)).BeginInit();
            this.ultraTabSharedControlsPage1.SuspendLayout();
            this.pnlTreeParent.SuspendLayout();
            this.pnlTreeView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView)).BeginInit();
            this.panel3.SuspendLayout();
            this.pnlTreeSearch.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlBack.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlPanels
            // 
            this.pnlPanels.AutoScroll = true;
            this.pnlPanels.AutoScrollMargin = new System.Drawing.Size(25, 8);
            this.pnlPanels.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlPanels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPanels.Controls.Add(this.pnlDatabaseInfo);
            this.pnlPanels.Controls.Add(this.ucPanelNavMyFav);
            this.pnlPanels.Controls.Add(this.ucPanelNavLast10);
            this.pnlPanels.Controls.Add(this.ucApplications);
            this.pnlPanels.Controls.Add(this.ucSystemTasks);
            this.pnlPanels.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPanels.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlPanels.Location = new System.Drawing.Point(0, 0);
            this.pnlPanels.Name = "pnlPanels";
            this.pnlPanels.Padding = new System.Windows.Forms.Padding(8, 2, 8, 16);
            this.pnlPanels.Size = new System.Drawing.Size(169, 405);
            this.pnlPanels.TabIndex = 0;
            this.pnlPanels.BackColorChanged += new System.EventHandler(this.pnlPanels_BackColorChanged);
            // 
            // pnlDatabaseInfo
            // 
            this.pnlDatabaseInfo.Controls.Add(this.pnlCulture);
            this.pnlDatabaseInfo.Controls.Add(this.pnlServer);
            this.pnlDatabaseInfo.Controls.Add(this.pnlDatabase);
            this.pnlDatabaseInfo.Controls.Add(this.pnlUser);
            this.pnlDatabaseInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDatabaseInfo.Location = new System.Drawing.Point(8, 315);
            this.pnlDatabaseInfo.Name = "pnlDatabaseInfo";
            this.pnlDatabaseInfo.Size = new System.Drawing.Size(151, 72);
            this.pnlDatabaseInfo.TabIndex = 37;
            // 
            // pnlCulture
            // 
            this.pnlCulture.Controls.Add(this.labCulture);
            this.pnlCulture.Controls.Add(this.label2);
            this.pnlCulture.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlCulture.Location = new System.Drawing.Point(0, 8);
            this.pnlCulture.Name = "pnlCulture";
            this.pnlCulture.Size = new System.Drawing.Size(151, 16);
            this.pnlCulture.TabIndex = 35;
            // 
            // labCulture
            // 
            this.labCulture.AutoSize = true;
            this.labCulture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labCulture.Location = new System.Drawing.Point(64, 0);
            this.labCulture.Name = "labCulture";
            this.labCulture.Size = new System.Drawing.Size(0, 15);
            this.labCulture.TabIndex = 0;
            this.labCulture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.ResourceObject.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("CULTUREc", "Culture : ", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Culture : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlServer
            // 
            this.pnlServer.Controls.Add(this.labServer);
            this.pnlServer.Controls.Add(this.labServerCap);
            this.pnlServer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlServer.Location = new System.Drawing.Point(0, 24);
            this.pnlServer.Name = "pnlServer";
            this.pnlServer.Size = new System.Drawing.Size(151, 16);
            this.pnlServer.TabIndex = 34;
            // 
            // labServer
            // 
            this.labServer.AutoSize = true;
            this.labServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labServer.Location = new System.Drawing.Point(64, 0);
            this.labServer.Name = "labServer";
            this.labServer.Size = new System.Drawing.Size(0, 15);
            this.labServer.TabIndex = 0;
            this.labServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labServerCap
            // 
            this.labServerCap.Dock = System.Windows.Forms.DockStyle.Left;
            this.labServerCap.Location = new System.Drawing.Point(0, 0);
            this.ResourceObject.SetLookup(this.labServerCap, new FWBS.OMS.UI.Windows.ResourceLookupItem("SERVERC", "Server : ", ""));
            this.labServerCap.Name = "labServerCap";
            this.labServerCap.Size = new System.Drawing.Size(64, 16);
            this.labServerCap.TabIndex = 1;
            this.labServerCap.Text = "Server : ";
            this.labServerCap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDatabase
            // 
            this.pnlDatabase.Controls.Add(this.labDatabase);
            this.pnlDatabase.Controls.Add(this.labDatabaseCap);
            this.pnlDatabase.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDatabase.Location = new System.Drawing.Point(0, 40);
            this.pnlDatabase.Name = "pnlDatabase";
            this.pnlDatabase.Size = new System.Drawing.Size(151, 16);
            this.pnlDatabase.TabIndex = 33;
            // 
            // labDatabase
            // 
            this.labDatabase.AutoSize = true;
            this.labDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labDatabase.Location = new System.Drawing.Point(64, 0);
            this.labDatabase.Name = "labDatabase";
            this.labDatabase.Size = new System.Drawing.Size(0, 15);
            this.labDatabase.TabIndex = 0;
            // 
            // labDatabaseCap
            // 
            this.labDatabaseCap.Dock = System.Windows.Forms.DockStyle.Left;
            this.labDatabaseCap.Location = new System.Drawing.Point(0, 0);
            this.ResourceObject.SetLookup(this.labDatabaseCap, new FWBS.OMS.UI.Windows.ResourceLookupItem("DATABASEC", "Database : ", ""));
            this.labDatabaseCap.Name = "labDatabaseCap";
            this.labDatabaseCap.Size = new System.Drawing.Size(64, 16);
            this.labDatabaseCap.TabIndex = 1;
            this.labDatabaseCap.Text = "Database : ";
            this.labDatabaseCap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlUser
            // 
            this.pnlUser.Controls.Add(this.labUser);
            this.pnlUser.Controls.Add(this.labUserCap);
            this.pnlUser.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlUser.Location = new System.Drawing.Point(0, 56);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(151, 16);
            this.pnlUser.TabIndex = 32;
            // 
            // labUser
            // 
            this.labUser.AutoSize = true;
            this.labUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labUser.Location = new System.Drawing.Point(64, 0);
            this.labUser.Name = "labUser";
            this.labUser.Size = new System.Drawing.Size(0, 15);
            this.labUser.TabIndex = 0;
            this.labUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labUserCap
            // 
            this.labUserCap.Dock = System.Windows.Forms.DockStyle.Left;
            this.labUserCap.Location = new System.Drawing.Point(0, 0);
            this.ResourceObject.SetLookup(this.labUserCap, new FWBS.OMS.UI.Windows.ResourceLookupItem("USERC", "User : ", ""));
            this.labUserCap.Name = "labUserCap";
            this.labUserCap.Size = new System.Drawing.Size(64, 16);
            this.labUserCap.TabIndex = 1;
            this.labUserCap.Text = "User : ";
            this.labUserCap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucPanelNavMyFav
            // 
            this.ucPanelNavMyFav.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNavMyFav.Controls.Add(this.pnlFavButtons);
            this.ucPanelNavMyFav.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNavMyFav.ExpandedHeight = 31;
            this.ucPanelNavMyFav.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNavMyFav.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanelNavMyFav.Location = new System.Drawing.Point(8, 171);
            this.ResourceObject.SetLookup(this.ucPanelNavMyFav, new FWBS.OMS.UI.Windows.ResourceLookupItem("MYFAVOURITES", "My Favourites", ""));
            this.ucPanelNavMyFav.Name = "ucPanelNavMyFav";
            this.ucPanelNavMyFav.Size = new System.Drawing.Size(151, 31);
            this.ucPanelNavMyFav.TabIndex = 29;
            this.ucPanelNavMyFav.TabStop = false;
            this.ucPanelNavMyFav.Text = "#My Favourites";
            this.ucPanelNavMyFav.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // pnlFavButtons
            // 
            this.pnlFavButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFavButtons.Location = new System.Drawing.Point(0, 24);
            this.pnlFavButtons.Name = "pnlFavButtons";
            this.pnlFavButtons.Padding = new System.Windows.Forms.Padding(5);
            this.pnlFavButtons.PanelBackColor = System.Drawing.Color.Empty;
            this.pnlFavButtons.Size = new System.Drawing.Size(151, 0);
            this.pnlFavButtons.TabIndex = 15;
            this.pnlFavButtons.TabStop = false;
            this.pnlFavButtons.LinkClicked += new FWBS.OMS.UI.Windows.LinkEventHandler(this.pnlFavButtons_LinkClicked);
            // 
            // ucPanelNavLast10
            // 
            this.ucPanelNavLast10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNavLast10.Controls.Add(this.pnlLast10Buttons);
            this.ucPanelNavLast10.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNavLast10.ExpandedHeight = 31;
            this.ucPanelNavLast10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNavLast10.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanelNavLast10.Location = new System.Drawing.Point(8, 140);
            this.ResourceObject.SetLookup(this.ucPanelNavLast10, new FWBS.OMS.UI.Windows.ResourceLookupItem("TOP10", "Last 10", ""));
            this.ucPanelNavLast10.Name = "ucPanelNavLast10";
            this.ucPanelNavLast10.Size = new System.Drawing.Size(151, 31);
            this.ucPanelNavLast10.TabIndex = 29;
            this.ucPanelNavLast10.TabStop = false;
            this.ucPanelNavLast10.Text = "#Last10";
            this.ucPanelNavLast10.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // pnlLast10Buttons
            // 
            this.pnlLast10Buttons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLast10Buttons.Location = new System.Drawing.Point(0, 24);
            this.pnlLast10Buttons.Name = "pnlLast10Buttons";
            this.pnlLast10Buttons.Padding = new System.Windows.Forms.Padding(5);
            this.pnlLast10Buttons.PanelBackColor = System.Drawing.Color.Empty;
            this.pnlLast10Buttons.Size = new System.Drawing.Size(151, 0);
            this.pnlLast10Buttons.TabIndex = 5;
            this.pnlLast10Buttons.TabStop = false;
            this.pnlLast10Buttons.LinkClicked += new FWBS.OMS.UI.Windows.LinkEventHandler(this.pnlLast10Buttons_LinkClicked);
            // 
            // ucApplications
            // 
            this.ucApplications.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucApplications.ContextMenu = this.mnuApplications;
            this.ucApplications.Controls.Add(this.ucNavApplications);
            this.ucApplications.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucApplications.ExpandedHeight = 31;
            this.ucApplications.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucApplications.HeaderColor = System.Drawing.Color.Empty;
            this.ucApplications.Location = new System.Drawing.Point(8, 109);
            this.ResourceObject.SetLookup(this.ucApplications, new FWBS.OMS.UI.Windows.ResourceLookupItem("APPLICATION", "Applications", ""));
            this.ucApplications.Name = "ucApplications";
            this.ucApplications.Size = new System.Drawing.Size(151, 31);
            this.ucApplications.TabIndex = 36;
            this.ucApplications.TabStop = false;
            this.ucApplications.Text = "Applications";
            this.ucApplications.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // mnuApplications
            // 
            this.mnuApplications.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuRegApp,
            this.mnuRemApp});
            this.mnuApplications.Popup += new System.EventHandler(this.mnuApplications_Popup);
            // 
            // mnuRegApp
            // 
            this.mnuRegApp.Index = 0;
            this.mnuRegApp.Text = "Add Application";
            this.mnuRegApp.Click += new System.EventHandler(this.mnuRegApp_Click);
            // 
            // mnuRemApp
            // 
            this.mnuRemApp.Index = 1;
            this.mnuRemApp.Text = "Remove Application";
            this.mnuRemApp.Click += new System.EventHandler(this.mnuRemApp_Click);
            // 
            // ucNavApplications
            // 
            this.ucNavApplications.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavApplications.Location = new System.Drawing.Point(0, 24);
            this.ucNavApplications.Name = "ucNavApplications";
            this.ucNavApplications.Padding = new System.Windows.Forms.Padding(5);
            this.ucNavApplications.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavApplications.Size = new System.Drawing.Size(151, 0);
            this.ucNavApplications.TabIndex = 15;
            this.ucNavApplications.TabStop = false;
            this.ucNavApplications.LinkClicked += new FWBS.OMS.UI.Windows.LinkEventHandler(this.OnLinkClicked);
            // 
            // ucSystemTasks
            // 
            this.ucSystemTasks.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucSystemTasks.Controls.Add(this.pnlCommands);
            this.ucSystemTasks.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucSystemTasks.ExpandedHeight = 107;
            this.ucSystemTasks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucSystemTasks.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucSystemTasks.Location = new System.Drawing.Point(8, 2);
            this.ResourceObject.SetLookup(this.ucSystemTasks, new FWBS.OMS.UI.Windows.ResourceLookupItem("UCSYSTASKS", "System Tasks", ""));
            this.ucSystemTasks.Name = "ucSystemTasks";
            this.ucSystemTasks.Size = new System.Drawing.Size(151, 107);
            this.ucSystemTasks.TabIndex = 31;
            this.ucSystemTasks.TabStop = false;
            this.ucSystemTasks.Text = "#System Tasks";
            this.ucSystemTasks.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // pnlCommands
            // 
            this.pnlCommands.Controls.Add(this.ucSystemInformation);
            this.pnlCommands.Controls.Add(this.ucSystemUpdate);
            this.pnlCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommands.Location = new System.Drawing.Point(0, 24);
            this.pnlCommands.Name = "pnlCommands";
            this.pnlCommands.Padding = new System.Windows.Forms.Padding(5);
            this.pnlCommands.PanelBackColor = System.Drawing.Color.Empty;
            this.pnlCommands.Size = new System.Drawing.Size(151, 76);
            this.pnlCommands.TabIndex = 16;
            this.pnlCommands.TabStop = false;
            // 
            // ucSystemInformation
            // 
            this.ucSystemInformation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucSystemInformation.ImageIndex = -1;
            this.ucSystemInformation.Key = "SystemInf";
            this.ucSystemInformation.Location = new System.Drawing.Point(5, 5);
            this.ResourceObject.SetLookup(this.ucSystemInformation, new FWBS.OMS.UI.Windows.ResourceLookupItem("SYSINFO", "System Information", ""));
            this.ucSystemInformation.Name = "ucSystemInformation";
            this.ucSystemInformation.Size = new System.Drawing.Size(141, 22);
            this.ucSystemInformation.TabIndex = 35;
            this.ucSystemInformation.Text = "#System Information";
            this.ucSystemInformation.LinkClicked += new System.EventHandler(this.ucSystemInformation_LinkClicked);
            // 
            // ucSystemUpdate
            // 
            this.ucSystemUpdate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucSystemUpdate.ImageIndex = -1;
            this.ucSystemUpdate.Location = new System.Drawing.Point(5, 27);
            this.ResourceObject.SetLookup(this.ucSystemUpdate, new FWBS.OMS.UI.Windows.ResourceLookupItem("SYSUPD", "System Update", ""));
            this.ucSystemUpdate.Name = "ucSystemUpdate";
            this.ucSystemUpdate.Size = new System.Drawing.Size(141, 22);
            this.ucSystemUpdate.TabIndex = 37;
            this.ucSystemUpdate.Text = "#System Update";
            this.ucSystemUpdate.LinkClicked += new System.EventHandler(this.ucSystemUpdate_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ultraTabControl);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(169, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(520, 405);
            this.panel1.TabIndex = 21;
            // 
            // ultraTabControl
            // 
            this.ultraTabControl.CloseButtonLocation = Infragistics.Win.UltraWinTabs.TabCloseButtonLocation.Tab;
            this.ultraTabControl.Controls.Add(this.ultraTabSharedControlsPage1);
            this.ultraTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ultraTabControl.Location = new System.Drawing.Point(0, 0);
            this.ultraTabControl.Name = "ultraTabControl";
            this.ultraTabControl.SharedControls.AddRange(new System.Windows.Forms.Control[] {
            this.pnlTreeParent});
            this.ultraTabControl.SharedControlsPage = this.ultraTabSharedControlsPage1;
            this.ultraTabControl.Size = new System.Drawing.Size(520, 405);
            this.ultraTabControl.TabButtonStyle = Infragistics.Win.UIElementButtonStyle.ButtonSoftExtended;
            this.ultraTabControl.TabCloseButtonAlignment = Infragistics.Win.UltraWinTabs.TabCloseButtonAlignment.AfterContent;
            this.ultraTabControl.TabCloseButtonVisibility = Infragistics.Win.UltraWinTabs.TabCloseButtonVisibility.WhenSelected;
            this.ultraTabControl.TabIndex = 3;
            // 
            // ultraTabSharedControlsPage1
            // 
            this.ultraTabSharedControlsPage1.AutoScroll = true;
            this.ultraTabSharedControlsPage1.Controls.Add(this.pnlTreeParent);
            this.ultraTabSharedControlsPage1.Location = new System.Drawing.Point(1, 20);
            this.ultraTabSharedControlsPage1.Name = "ultraTabSharedControlsPage1";
            this.ultraTabSharedControlsPage1.Size = new System.Drawing.Size(516, 382);
            // 
            // pnlTreeParent
            // 
            this.pnlTreeParent.BackColor = System.Drawing.Color.White;
            this.pnlTreeParent.Controls.Add(this.pnlTreeView);
            this.pnlTreeParent.Controls.Add(this.panel3);
            this.pnlTreeParent.Controls.Add(this.pnlTreeSearch);
            this.pnlTreeParent.Location = new System.Drawing.Point(46, 8);
            this.pnlTreeParent.Name = "pnlTreeParent";
            this.pnlTreeParent.Size = new System.Drawing.Size(231, 352);
            this.pnlTreeParent.TabIndex = 0;
            // 
            // pnlTreeView
            // 
            this.pnlTreeView.BackColor = System.Drawing.Color.White;
            this.pnlTreeView.Controls.Add(this.radTreeView);
            this.pnlTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTreeView.Location = new System.Drawing.Point(0, 75);
            this.pnlTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTreeView.Name = "pnlTreeView";
            this.pnlTreeView.Size = new System.Drawing.Size(231, 277);
            this.pnlTreeView.TabIndex = 0;
            // 
            // radTreeView
            // 
            this.radTreeView.BackColor = System.Drawing.Color.White;
            this.radTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTreeView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radTreeView.LineColor = System.Drawing.Color.White;
            this.radTreeView.LineStyle = Telerik.WinControls.UI.TreeLineStyle.Solid;
            this.radTreeView.Location = new System.Drawing.Point(0, 0);
            this.radTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.radTreeView.Name = "radTreeView";
            this.radTreeView.Size = new System.Drawing.Size(231, 277);
            this.radTreeView.SpacingBetweenNodes = 5;
            this.radTreeView.TabIndex = 0;
            this.radTreeView.ThemeName = "Windows8";
            this.radTreeView.ToggleMode = Telerik.WinControls.UI.ToggleMode.SingleClick;
            this.radTreeView.TreeIndent = 10;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.eCaptionLine21);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 65);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(231, 10);
            this.panel3.TabIndex = 1;
            // 
            // eCaptionLine21
            // 
            this.eCaptionLine21.BackColor = System.Drawing.Color.WhiteSmoke;
            this.eCaptionLine21.Dock = System.Windows.Forms.DockStyle.Top;
            this.eCaptionLine21.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eCaptionLine21.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.eCaptionLine21.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(222)))), ((int)(((byte)(214)))));
            this.eCaptionLine21.Location = new System.Drawing.Point(0, 0);
            this.eCaptionLine21.Name = "eCaptionLine21";
            this.eCaptionLine21.Size = new System.Drawing.Size(231, 2);
            this.eCaptionLine21.TabIndex = 9999;
            // 
            // pnlTreeSearch
            // 
            this.pnlTreeSearch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlTreeSearch.Controls.Add(this.txtTreeSearch);
            this.pnlTreeSearch.Controls.Add(this.btnSearch);
            this.pnlTreeSearch.Controls.Add(this.btnRefreshTree);
            this.pnlTreeSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTreeSearch.Location = new System.Drawing.Point(0, 0);
            this.pnlTreeSearch.Name = "pnlTreeSearch";
            this.pnlTreeSearch.Padding = new System.Windows.Forms.Padding(8);
            this.pnlTreeSearch.Size = new System.Drawing.Size(231, 65);
            this.pnlTreeSearch.TabIndex = 0;
            // 
            // txtTreeSearch
            // 
            this.txtTreeSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtTreeSearch.Location = new System.Drawing.Point(8, 8);
            this.txtTreeSearch.Name = "txtTreeSearch";
            this.txtTreeSearch.Size = new System.Drawing.Size(215, 23);
            this.txtTreeSearch.TabIndex = 13;
            this.txtTreeSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTreeSearch_KeyPress);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Image = global::FWBS.OMS.UI.Properties.Resources._12;
            this.btnSearch.Location = new System.Drawing.Point(170, 35);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(24, 24);
            this.btnSearch.TabIndex = 16;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnRefreshTree
            // 
            this.btnRefreshTree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshTree.Image = global::FWBS.OMS.UI.Properties.Resources.Refresh;
            this.btnRefreshTree.Location = new System.Drawing.Point(199, 35);
            this.btnRefreshTree.Name = "btnRefreshTree";
            this.btnRefreshTree.Size = new System.Drawing.Size(24, 24);
            this.btnRefreshTree.TabIndex = 14;
            this.btnRefreshTree.UseVisualStyleBackColor = true;
            this.btnRefreshTree.Click += new System.EventHandler(this.btnRefreshTree_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlBack);
            this.panel2.Location = new System.Drawing.Point(24, 27);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(520, 405);
            this.panel2.TabIndex = 0;
            this.panel2.Visible = false;
            // 
            // pnlBack
            // 
            this.pnlBack.Controls.Add(this.pnlMain);
            this.pnlBack.Location = new System.Drawing.Point(23, 27);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.pnlBack.Size = new System.Drawing.Size(520, 405);
            this.pnlBack.TabIndex = 1;
            this.pnlBack.Visible = false;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.SearchPanel);
            this.pnlMain.Controls.Add(this.tabControl1);
            this.pnlMain.Controls.Add(this.panel8);
            this.pnlMain.Controls.Add(this.panel5);
            this.pnlMain.Location = new System.Drawing.Point(22, 17);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(510, 400);
            this.pnlMain.TabIndex = 2;
            this.pnlMain.Visible = false;
            // 
            // SearchPanel
            // 
            this.SearchPanel.Location = new System.Drawing.Point(6, 72);
            this.SearchPanel.Name = "SearchPanel";
            this.SearchPanel.Size = new System.Drawing.Size(504, 328);
            this.SearchPanel.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(504, 392);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.Visible = false;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.SystemColors.Window;
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 8);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(6, 392);
            this.panel8.TabIndex = 26;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Window;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(510, 8);
            this.panel5.TabIndex = 27;
            // 
            // colName
            // 
            this.colName.Text = "Consoles";
            this.colName.Width = 400;
            // 
            // mnuDyRemFav
            // 
            this.mnuDyRemFav.Index = 0;
            this.mnuDyRemFav.Text = "#Remove Item";
            this.mnuDyRemFav.Click += new System.EventHandler(this.mnuDyRemFav_Click);
            // 
            // mnuRefreshFav
            // 
            this.mnuRefreshFav.Index = 1;
            this.mnuRefreshFav.Text = "Refresh Favourites";
            this.mnuRefreshFav.Click += new System.EventHandler(this.mnuRefreshFav_Click);
            // 
            // mnuDynamicFav
            // 
            this.mnuDynamicFav.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuDyRemFav,
            this.mnuRefreshFav});
            // 
            // mnuLast10
            // 
            this.mnuLast10.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuRefreshLast10});
            // 
            // mnuRefreshLast10
            // 
            this.mnuRefreshLast10.Index = 0;
            this.mnuRefreshLast10.Text = "Refresh Last 10";
            this.mnuRefreshLast10.Click += new System.EventHandler(this.mnuRefreshLast10_Click);
            // 
            // labSearch
            // 
            this.labSearch.BackColor = System.Drawing.Color.Transparent;
            this.labSearch.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSearch.ForeColor = System.Drawing.Color.White;
            this.labSearch.Location = new System.Drawing.Point(11, 0);
            this.ResourceObject.SetLookup(this.labSearch, new FWBS.OMS.UI.Windows.ResourceLookupItem("SEARCH", "Search : ", ""));
            this.labSearch.Name = "labSearch";
            this.labSearch.Size = new System.Drawing.Size(94, 31);
            this.labSearch.TabIndex = 0;
            this.labSearch.Text = "#Search : ";
            this.labSearch.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSearchGo
            // 
            this.btnSearchGo.Location = new System.Drawing.Point(0, 0);
            this.btnSearchGo.Name = "btnSearchGo";
            this.btnSearchGo.Size = new System.Drawing.Size(75, 23);
            this.btnSearchGo.TabIndex = 0;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(104, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(190, 20);
            this.txtSearch.TabIndex = 1;
            // 
            // LabelMenuName
            // 
            this.LabelMenuName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelMenuName.BackColor = System.Drawing.Color.Transparent;
            this.LabelMenuName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.LabelMenuName.ForeColor = System.Drawing.Color.White;
            this.LabelMenuName.Location = new System.Drawing.Point(513, 0);
            this.LabelMenuName.Name = "LabelMenuName";
            this.LabelMenuName.Size = new System.Drawing.Size(162, 31);
            this.LabelMenuName.TabIndex = 4;
            this.LabelMenuName.Text = "3E MatterSphere";
            this.LabelMenuName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ultraDockManager1
            // 
            this.ultraDockManager1.HostControl = this;
            this.ultraDockManager1.SettingsKey = "";
            this.ultraDockManager1.UnpinnedTabStyle = Infragistics.Win.UltraWinTabs.TabStyle.Flat;
            this.ultraDockManager1.UseDefaultContextMenus = false;
            // 
            // _ucHome2UnpinnedTabAreaLeft
            // 
            this._ucHome2UnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._ucHome2UnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._ucHome2UnpinnedTabAreaLeft.Name = "_ucHome2UnpinnedTabAreaLeft";
            this._ucHome2UnpinnedTabAreaLeft.Owner = this.ultraDockManager1;
            this._ucHome2UnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 405);
            this._ucHome2UnpinnedTabAreaLeft.TabIndex = 22;
            // 
            // _ucHome2UnpinnedTabAreaRight
            // 
            this._ucHome2UnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._ucHome2UnpinnedTabAreaRight.Location = new System.Drawing.Point(689, 0);
            this._ucHome2UnpinnedTabAreaRight.Name = "_ucHome2UnpinnedTabAreaRight";
            this._ucHome2UnpinnedTabAreaRight.Owner = this.ultraDockManager1;
            this._ucHome2UnpinnedTabAreaRight.Size = new System.Drawing.Size(0, 405);
            this._ucHome2UnpinnedTabAreaRight.TabIndex = 23;
            // 
            // _ucHome2UnpinnedTabAreaTop
            // 
            this._ucHome2UnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._ucHome2UnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._ucHome2UnpinnedTabAreaTop.Name = "_ucHome2UnpinnedTabAreaTop";
            this._ucHome2UnpinnedTabAreaTop.Owner = this.ultraDockManager1;
            this._ucHome2UnpinnedTabAreaTop.Size = new System.Drawing.Size(689, 0);
            this._ucHome2UnpinnedTabAreaTop.TabIndex = 24;
            // 
            // _ucHome2UnpinnedTabAreaBottom
            // 
            this._ucHome2UnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ucHome2UnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 405);
            this._ucHome2UnpinnedTabAreaBottom.Name = "_ucHome2UnpinnedTabAreaBottom";
            this._ucHome2UnpinnedTabAreaBottom.Owner = this.ultraDockManager1;
            this._ucHome2UnpinnedTabAreaBottom.Size = new System.Drawing.Size(689, 0);
            this._ucHome2UnpinnedTabAreaBottom.TabIndex = 25;
            // 
            // _ucHome2AutoHideControl
            // 
            this._ucHome2AutoHideControl.Location = new System.Drawing.Point(0, 0);
            this._ucHome2AutoHideControl.Name = "_ucHome2AutoHideControl";
            this._ucHome2AutoHideControl.Owner = this.ultraDockManager1;
            this._ucHome2AutoHideControl.TabIndex = 26;
            // 
            // SearchToolTip
            // 
            this.SearchToolTip.IsBalloon = true;
            this.SearchToolTip.ToolTipTitle = "Search";
            // 
            // RefreshToolTip
            // 
            this.RefreshToolTip.IsBalloon = true;
            this.RefreshToolTip.ToolTipTitle = "Refresh Tree";
            // 
            // ucNavCmdButtons10
            // 
            this.ucNavCmdButtons10.ImageIndex = -1;
            this.ucNavCmdButtons10.Location = new System.Drawing.Point(0, 0);
            this.ucNavCmdButtons10.Name = "ucNavCmdButtons10";
            this.ucNavCmdButtons10.Size = new System.Drawing.Size(240, 22);
            this.ucNavCmdButtons10.TabIndex = 0;
            // 
            // ucNavCmdButtons11
            // 
            this.ucNavCmdButtons11.ImageIndex = -1;
            this.ucNavCmdButtons11.Location = new System.Drawing.Point(0, 0);
            this.ucNavCmdButtons11.Name = "ucNavCmdButtons11";
            this.ucNavCmdButtons11.Size = new System.Drawing.Size(240, 22);
            this.ucNavCmdButtons11.TabIndex = 0;
            // 
            // ucNavCmdButtons12
            // 
            this.ucNavCmdButtons12.ImageIndex = -1;
            this.ucNavCmdButtons12.Location = new System.Drawing.Point(0, 0);
            this.ucNavCmdButtons12.Name = "ucNavCmdButtons12";
            this.ucNavCmdButtons12.Size = new System.Drawing.Size(240, 22);
            this.ucNavCmdButtons12.TabIndex = 0;
            // 
            // ucDockMainView
            // 
            this.Controls.Add(this._ucHome2AutoHideControl);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlPanels);
            this.Controls.Add(this._ucHome2UnpinnedTabAreaTop);
            this.Controls.Add(this._ucHome2UnpinnedTabAreaBottom);
            this.Controls.Add(this._ucHome2UnpinnedTabAreaLeft);
            this.Controls.Add(this._ucHome2UnpinnedTabAreaRight);
            this.Name = "ucDockMainView";
            this.Size = new System.Drawing.Size(689, 405);
            this.pnlPanels.ResumeLayout(false);
            this.pnlDatabaseInfo.ResumeLayout(false);
            this.pnlCulture.ResumeLayout(false);
            this.pnlCulture.PerformLayout();
            this.pnlServer.ResumeLayout(false);
            this.pnlServer.PerformLayout();
            this.pnlDatabase.ResumeLayout(false);
            this.pnlDatabase.PerformLayout();
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.ucPanelNavMyFav.ResumeLayout(false);
            this.ucPanelNavMyFav.PerformLayout();
            this.ucPanelNavLast10.ResumeLayout(false);
            this.ucPanelNavLast10.PerformLayout();
            this.ucApplications.ResumeLayout(false);
            this.ucApplications.PerformLayout();
            this.ucSystemTasks.ResumeLayout(false);
            this.ucSystemTasks.PerformLayout();
            this.pnlCommands.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraTabControl)).EndInit();
            this.ultraTabSharedControlsPage1.ResumeLayout(false);
            this.pnlTreeParent.ResumeLayout(false);
            this.pnlTreeView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView)).EndInit();
            this.panel3.ResumeLayout(false);
            this.pnlTreeSearch.ResumeLayout(false);
            this.pnlTreeSearch.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.pnlBack.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ultraDockManager1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region Contructors

        public ucDockMainView()
        {
            InitializeComponent();
            SetButtonImages();
        }

        private bool hasBeenSetup = false;
        private string parentkey;
        private string title;

        internal void Setup(TreeNavigationActions actions)
		{
            SetupTreeNavigationActions(actions);
            ManagePanelVisibility();
            SetupDatabaseInformation();
            LoadResources();

            if (Session.CurrentSession.CurrentTerminal.IsRegistered)
            {
                RefreshApplicationFavorites();
                RefreshFavorites();
                RefreshLast10();
                BuildTreeView();
                ManageSearchVisibility(true);
                ManageCommandVisibility(true);
                ManageTreeViewVisibility(true);
                Treenav.AddedToLast10 += new EventHandler(Treenav_AddedToLast10);
                Treenav.AdminKitTreeNodeEdited += new EventHandler(Treenav_AdminTreeNodeEdited);
                ExpandLists();
            }
            else
            {
                ManageSearchVisibility(false);
                ManageCommandVisibility(false);
                ManageTreeViewVisibility(false);
                MakeListsInvisibile();
            }

            SetupTabControl();
           
            if (hasBeenSetup)
                return;

            CreateDockedPanes();

            ResourceObject.Refresh();

            this.SearchToolTip.SetToolTip(this.btnSearch, Session.CurrentSession.Resources.GetMessage("SEARCHTTT", "TreeView searches will compare both node description and object codes to the search parameter.", "").Text);
            this.RefreshToolTip.SetToolTip(this.btnRefreshTree, Session.CurrentSession.Resources.GetMessage("REFRESHTTT", "The TreeView will be restored to its former state with any matches remaining expanded.", "").Text);

            ParentForm.FormClosing += new FormClosingEventHandler(mainparent_FormClosing);
            hasBeenSetup = true;
		}


        void Treenav_AddedToLast10(object sender, EventArgs e)
        {
            RefreshLast10();
        }


        private void Treenav_AdminTreeNodeEdited(object sender, EventArgs e)
        {
            RefreshLast10();
            RefreshFavorites();
        }

        private void BuildTreeView()
        {
            if (Treenav == null)
                Treenav = new TreeNavigationBuilder(this);

            Treenav.RefreshTreeView();
        }

        private void SetupTreeNavigationActions(TreeNavigationActions Actions)
        {
            this.actions = Actions;
            parentkey = actions.ParentKey;
            title = actions.Title;
            actions.Setup(this.ultraTabControl);
        }

        private void SetupDatabaseInformation()
        {
            labUser.Text = FWBS.OMS.Session.CurrentSession.CurrentUser.FullName;
            labDatabase.Text = FWBS.OMS.Session.CurrentSession.CurrentDatabase.DatabaseName.ToUpper();
            labServer.Text = FWBS.OMS.Session.CurrentSession.CurrentDatabase.Server;
            labCulture.Text = FWBS.OMS.Session.CurrentSession.DefaultCulture;
        }

        private void ManagePanelVisibility()
        {
            ucApplications.Visible = actions.IsApplicationsPanelVisible;
            ucSystemTasks.Visible = actions.IsSystemUpdatePanelVisible;
        }

        private void ManageSearchVisibility(bool visibility)
        {
            this.btnSearch.Visible = visibility;
            this.txtTreeSearch.Visible = visibility;
            this.btnRefreshTree.Visible = visibility;
        }

        private void ManageCommandVisibility(bool visibility)
        {
            this.ucSystemInformation.Visible = visibility;
        }

        private void ManageTreeViewVisibility(bool visibility)
        {
            this.radTreeView.Visible = visibility;
        }

        private void MakeListsInvisibile()
        {
            pnlLast10Buttons.Controls.Clear();
            pnlFavButtons.Controls.Clear();
            ucNavApplications.Controls.Clear();
            ToggleUCPanelNavs();
        }

        private void ToggleUCPanelNavs()
        {
            if(ucPanelNavLast10.Expanded)
                ucPanelNavLast10.ToggleExpand();
            if(ucPanelNavMyFav.Expanded)
                ucPanelNavMyFav.ToggleExpand();
            if (ucApplications.Expanded)
                ucApplications.ToggleExpand();
        }

        private void ExpandLists()
        {
            if (!ucPanelNavLast10.Expanded)
                ucPanelNavLast10.ToggleExpand();
            if (!ucPanelNavMyFav.Expanded)
                ucPanelNavMyFav.ToggleExpand();
            if (!ucApplications.Expanded)
                ucApplications.ToggleExpand();
        }

        void mainparent_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                e.Cancel = CheckForDirtyTabs();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        public bool CheckForDirtyTabs()
        {
            bool result = false;
            List<IObjectUpdate> dirtyobjects = new List<IObjectUpdate>();
            int dirtycount = 0;
            var savelist = new System.Text.StringBuilder();

            if (this.ultraTabControl.Tabs.Count > 0)
            {
                savelist.Append(Session.CurrentSession.Resources.GetMessage("SAVEDIRTYOBJCTS", "Would you like to save the following objects?\n\n", "").Text);
                foreach (UltraTab tab in this.ultraTabControl.Tabs)
                {
                    if (tab.TabPage.Controls[0].Controls.Count > 0)
                    {
                        IOBjectDirty dirt = tab.TabPage.Controls[0].Controls[0] as IOBjectDirty;
                        if (dirt != null)
                        {
                            if (dirt.IsDirty)
                            {
                                dirtycount++;
                                dirtyobjects.Add(dirt as IObjectUpdate);
                                savelist.Append(tab.Text.Replace("&&", "&")).Append("\n");
                            }
                        }
                    }
                }
            }
            if (dirtycount > 0)
                result = DetermineClosureAction(dirtycount, dirtyobjects, savelist.ToString());
            else
                RemoveAllTabs();
            return result;
        }

        private bool DetermineClosureAction(int dirtycount, List<IObjectUpdate> dirtyobjects, string savelist)
        {
            bool result = false;

                DialogResult dr = System.Windows.Forms.MessageBox.Show(savelist, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    UpdateTabObjectData(dirtyobjects);
                    UnlockTabObjects();
                    RemoveAllTabs();
                }
                if (dr == DialogResult.No)
                {
                    UnlockTabObjects();
                    RemoveAllTabs();
                }
                if (dr == DialogResult.Cancel)
                    result = true;

            return result;
        }

        private void UpdateTabObjectData(List<IObjectUpdate> dirtyobjects)
        {
            foreach (IObjectUpdate obj in dirtyobjects)
            {
                obj.UpdateObjectData();
            }
        }

        private void UnlockTabObjects()
        {
            if (Session.CurrentSession.IsConnected && Session.CurrentSession.ObjectLocking)
            {
                foreach (UltraTab tab in this.ultraTabControl.Tabs)
                {
                    if (tab.TabPage.Controls[0].Controls.Count > 0)
                    {
                        IObjectUnlock objectToUnlock = tab.TabPage.Controls[0].Controls[0] as IObjectUnlock;
                        if (objectToUnlock != null)
                            objectToUnlock.UnlockCurrentObject();
                    }
                }
            }
        }

        public void RemoveAllTabs()
        {
            UnlockTabObjects();
            UltraTabsCollection tabs = this.ultraTabControl.Tabs;
            tabs.Clear();
        }

        public void CloseOpenWindows()
        {
            IDisposable window;
            try
            {
                window = actions.OpenWindows["ED"] as IDisposable;
                if (window != null)
                {
                    window.Dispose();
                    actions.OpenWindows.Remove("ED");
                }
            }
            catch
            {

            }
        }

        private void CreateDockedPanes()
        {
            //Left-hand docked panes
            Size size = LogicalToDeviceUnits(new Size(252, 400));

            Infragistics.Win.UltraWinDock.DockableControlPane dockableHome = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("dc3cd904-1486-428e-82f0-7bf763fb36db"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("db624901-0be8-42ab-918d-dabde92f537b"), -1);
            dockableHome.Control = this.pnlPanels;
            dockableHome.OriginalControlBounds = new Rectangle(Point.Empty, size);
            dockableHome.Size = size;
            dockableHome.FlyoutSize = size;
            dockableHome.Text = "Home";
 
            Infragistics.Win.UltraWinDock.DockableControlPane dockableTree = new Infragistics.Win.UltraWinDock.DockableControlPane();
            dockableTree.Control = this.pnlTreeParent;
            dockableTree.OriginalControlBounds = new Rectangle(Point.Empty, size);
            dockableTree.Size = size;
            dockableTree.FlyoutSize = size;
            dockableTree.Text = "Explorer";

            DockableGroupPane treeListGrp = new DockableGroupPane();
            treeListGrp.ChildPaneStyle = ChildPaneStyle.TabGroup;
            treeListGrp.Panes.Add(dockableHome); 
            treeListGrp.Panes.Add(dockableTree); 
            treeListGrp.SelectedTabIndex = 0;
            treeListGrp.GroupSettings.TabSizing = TabSizing.Fixed;

            Infragistics.Win.UltraWinDock.DockAreaPane leftDockArea = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedLeft, new System.Guid("db624901-0be8-42ab-918d-dabde92f537b"));
            leftDockArea.Panes.Add(treeListGrp);
            leftDockArea.Size = size;
            leftDockArea.MinimumSize = size;

            //add group panes to dock manager

            this.ultraDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] { leftDockArea } );
            this.ultraDockManager1.DefaultPaneSettings.AllowDragging = DefaultableBoolean.False;
            this.ultraDockManager1.DefaultPaneSettings.DoubleClickAction = PaneDoubleClickAction.None;
            this.ultraDockManager1.DefaultPaneSettings.TabWidth = LogicalToDeviceUnits(20);
            this.ultraDockManager1.ImageSizeTab = LogicalToDeviceUnits(new Size(16, 16));
            this.ultraDockManager1.ScaleImages = ScaleImage.Always;
            this.ultraDockManager1.ShowCloseButton = false;
            this.ultraDockManager1.ShowMaximizeButton = false;
            this.ultraDockManager1.ShowMinimizeButton = false;
            this.ultraDockManager1.HostControl = this;

            SetDockingFormat(dockableHome,dockableTree);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);

            if (factor.Width != 1 && (specified & BoundsSpecified.Size) != 0 && this.ultraDockManager1.DockAreas.Count != 0)
            {
                DockAreaPane leftDockArea = this.ultraDockManager1.DockAreas[0];
                leftDockArea.MinimumSize = ScaleSize(leftDockArea.MinimumSize, factor);
                leftDockArea.Size = ScaleSize(leftDockArea.Size, factor);

                DockableGroupPane treeListGrp = (DockableGroupPane)leftDockArea.Panes[0];

                DockableControlPane dockableHome = (DockableControlPane)treeListGrp.Panes[0];
                dockableHome.OriginalControlBounds = new Rectangle(Point.Empty, ScaleSize(dockableHome.OriginalControlBounds.Size, factor));
                dockableHome.Size = ScaleSize(dockableHome.Size, factor);
                dockableHome.FlyoutSize = ScaleSize(dockableHome.FlyoutSize, factor);

                DockableControlPane dockableTree = (DockableControlPane)treeListGrp.Panes[1];
                dockableTree.OriginalControlBounds = new Rectangle(Point.Empty, ScaleSize(dockableTree.OriginalControlBounds.Size, factor));
                dockableTree.Size = ScaleSize(dockableTree.Size, factor);
                dockableTree.FlyoutSize = ScaleSize(dockableTree.FlyoutSize, factor);

                ultraDockManager1.DefaultPaneSettings.TabWidth = Convert.ToInt32(ultraDockManager1.DefaultPaneSettings.TabWidth * factor.Width);
                ultraDockManager1.ImageSizeTab = ScaleSize(ultraDockManager1.ImageSizeTab, factor);
            }
        }

        private static Size ScaleSize(Size size, SizeF factor)
        {
            return new Size(Convert.ToInt32(factor.Width * size.Width), Convert.ToInt32(factor.Height * size.Height));
        }

        private void SetupTabControl()
        {
            this.ultraTabControl.BeginUpdate();

            UltraTab WelcomeTab;
            UltraTabsCollection tabs = this.ultraTabControl.Tabs;
            tabs.Clear();
            WelcomeTab = tabs.Add("Welcome - " + System.DateTime.Now.Ticks, title);
            Panel WelcomePanel = CreateWelcomePage();
            WelcomeTab.TabPage.Controls.Add(WelcomePanel);
            this.ultraTabControl.SelectedTab = tabs[WelcomeTab.Key];
            // Workaround to Infragistics UltraTabControl buttons rendering issues on Windows 11
            if (Environment.OSVersion.Version.Major == 10 && Environment.OSVersion.Version.Build >= 22000)
            {
                this.ultraTabControl.ScrollButtonStyle = UIElementButtonStyle.Office2013ScrollbarButton;
                this.ultraTabControl.ScrollButtonAppearance.ThemedElementAlpha = Alpha.Transparent;
                this.ultraTabControl.CloseButtonAppearance.ThemedElementAlpha = Alpha.Transparent;
            }
            this.ultraTabControl.EndUpdate();
        }

        private void SetDockingFormat(DockableControlPane home, DockableControlPane tree)
        {
            var treeNavigationIcons = FWBS.OMS.UI.Windows.Images.TreeViewNavigationIcons();
            var dockableControlConfiguration = new DockableControlConfiguration();
            dockableControlConfiguration.SetDockManagerStyle(ultraDockManager1, new DockManagerSettings());
            dockableControlConfiguration.SetDockPanelStyle(tree, new DockPanelSettings() { TabSettings = new DockPanelTabSettings { Icon = treeNavigationIcons.Images[0] } });
            dockableControlConfiguration.SetDockPanelStyle(home, new DockPanelSettings() { TabSettings = new DockPanelTabSettings { Icon = treeNavigationIcons.Images[1] } });
        }

        private Panel CreateWelcomePage()
        {
            Label lblWelcome1 = CreatelblWelcome1();
            Label lblWelcome2 = CreatelblWelcome2();
            Panel pnlWlecome = new Panel();
            SetupWelcomePanel(pnlWlecome, lblWelcome1, lblWelcome2);
            CheckTerminalRegistration(lblWelcome1, lblWelcome2);
            return pnlWlecome;
        }

        private void SetupWelcomePanel(Panel pnlWelcome, Label lblWelcome1, Label lblWelcome2)
        {
            pnlWelcome.BackColor = System.Drawing.Color.WhiteSmoke;
            pnlWelcome.Controls.Add(lblWelcome2);
            pnlWelcome.Controls.Add(lblWelcome1);
            pnlWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            pnlWelcome.Location = new System.Drawing.Point(0, 0);
            pnlWelcome.Name = "panel3";
            pnlWelcome.Size = new System.Drawing.Size(1249, 114);
            pnlWelcome.TabIndex = 0;
        }

        private Label CreatelblWelcome1()
        {
            Label welcome = new Label();
            welcome.AutoSize = true;
            welcome.BackColor = System.Drawing.Color.WhiteSmoke;
            welcome.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            welcome.Location = new System.Drawing.Point(5, 9);
            welcome.Name = "label1";
            welcome.Size = new System.Drawing.Size(256, 15);
            welcome.TabIndex = 0;
            return welcome;
        }

        private Label CreatelblWelcome2()
        {
            Label welcome = new Label();
            welcome.AutoSize = true;
            welcome.BackColor = System.Drawing.Color.WhiteSmoke;
            welcome.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            welcome.Location = new System.Drawing.Point(5, 36);
            welcome.Name = "label2";
            welcome.Size = new System.Drawing.Size(637, 15);
            welcome.TabIndex = 1;
            return welcome;
        }

        private void CheckTerminalRegistration(Label lblWelcome1, Label lblWelcome2)
        {
            if (Session.CurrentSession.CurrentTerminal.IsRegistered)
                SetupRegisteredWelcomePanel(lblWelcome1, lblWelcome2);
            else
                SetWelcomeToUnregisteredTerminal(lblWelcome1, lblWelcome2);
        }

        private void SetupRegisteredWelcomePanel(Label lblWelcome1, Label lblWelcome2)
        {
            if (parentkey == "ADMIN")
                SetWelcomeToAdministrationEnvironment(lblWelcome1, lblWelcome2);
            else
                SetWelcomeToReportsEnvironment(lblWelcome1, lblWelcome2);
        }

        private void SetWelcomeToAdministrationEnvironment(Label lblWelcome1, Label lblWelcome2)
        {
            lblWelcome1.Text = Session.CurrentSession.Resources.GetResource("AK_WELCOMETO", "Welcome to the ", "").Text + Session.CurrentSession.Resources.GetResource("ADE", "3E MatterSphere Administration && Design Environment.", "").Text;
            lblWelcome1.ForeColor = Color.Black;
            lblWelcome2.Text = Session.CurrentSession.Resources.GetResource("ADE_ADVICE", "Either select a function from the available menus i.e. Last 10 or click the Navigation tab and choose an area of functionality from the tree.", "").Text;
            lblWelcome2.ForeColor = Color.Black;
        }

        private void SetWelcomeToReportsEnvironment(Label lblWelcome1, Label lblWelcome2)
        {
            lblWelcome1.Text = Session.CurrentSession.Resources.GetResource("RPT_WELCOMETO", "Welcome to the ", "").Text + " " + Session.CurrentSession.Resources.GetResource("RPT_DESC", "3E MatterSphere Reports Application.","").Text;
            lblWelcome1.ForeColor = Color.Black;
            lblWelcome2.Text = Session.CurrentSession.Resources.GetResource("RPT_ADVICE", "Either select a report from the available menus i.e. Last 10 or click the Navigation tab and choose a report from the tree.","").Text;
            lblWelcome2.ForeColor = Color.Black;
        }

        private void SetWelcomeToUnregisteredTerminal(Label lblWelcome1, Label lblWelcome2)
        {
            lblWelcome1.Text = Session.CurrentSession.Resources.GetResource("AK_WELCOMETO", "Welcome to the ", "").Text + Session.CurrentSession.Resources.GetResource("ADE", "3E MatterSphere Administration && Design Environment.", "").Text;
            lblWelcome1.ForeColor = Color.Red;
            lblWelcome2.Text = Session.CurrentSession.Resources.GetResource("AK_UNLICTERM", "This terminal is not registered. Please add ", "").Text + Session.CurrentSession.CurrentTerminal.TerminalName + Session.CurrentSession.Resources.GetResource("AK_UNLICTERM2", " as a licensed terminal in License Manager.", "").Text;
            lblWelcome2.ForeColor = Color.Red;
        }

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            try
            {
                if (disposing)
                {
                    if (pnlCommands != null)
                    {
                        pnlCommands.Dispose();
                        pnlCommands = null;
                    }

                    if (pnlFavButtons != null)
                    {
                        pnlFavButtons.Dispose();
                        pnlFavButtons = null;
                    }

                    if (pnlLast10Buttons != null)
                    {
                        pnlLast10Buttons.Dispose();
                        pnlLast10Buttons = null;
                    }

                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
            }
            finally
            {
                if (Treenav != null)
                {
                    Treenav.AddedToLast10 -= new EventHandler(Treenav_AddedToLast10);
                }
                base.Dispose(disposing);
            }
		}

		#endregion

		#region Public
		public bool IsRoot
		{
			get
			{
				return history.Count == 0;
			}
		}

		/// <summary>
		/// Redrwaw the Menu
		/// </summary>
		public override void Refresh()
		{
			base.Refresh();
		}

		#endregion

		#region Private
		/// <summary>
		/// Stores the Favourites Positions
		/// </summary>
		/// <param name="e"></param>
		private void UpdateFavorites(MenuEventArgs e)
		{
			if (e.IncFavorites)
			{
				FWBS.OMS.Favourites fw = new FWBS.OMS.Favourites(actions.ParentKey + "FAV",Convert.ToString(e.ReturnKey));
                if (fw.Count == 0)
					fw.AddFavourite(e.ReturnKey.ToString(),e.ImageIndex.ToString(),"",e.ButtonCode,e.Roles,"0");
				else
				{
					Int64 n = fw.Param4(0);
					fw.Param4(0,++n);
				}
				fw.Update();
			}
		}	

		/// <summary>
		/// Refreshs the Favourites Panel
		/// </summary>
		public void RefreshFavorites()
		{
			bool isreg = Session.CurrentSession.CurrentTerminal.IsRegistered;

			Global.RemoveAndDisposeControls(pnlFavButtons);
            FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(actions.ParentKey + "FAV");
			DataView dv = fav.GetDataView();
			dv.Sort = "usrFavObjParam4 DESC";
			int i=0;
            pnlFavButtons.Controls.Clear();
			foreach (DataRowView favKey in dv)
			{
				if (Session.CurrentSession.CurrentUser.IsInRoles(Convert.ToString(favKey["usrFavObjParam3"])) && isreg)
				{
					FWBS.OMS.UI.Windows.ucNavCmdButtons ctrl = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
					ctrl.Text = FWBS.OMS.Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU",Convert.ToString(favKey["usrFavObjParam2"])),true);
					ctrl.Key = Convert.ToString(favKey["usrFavDesc"]);
                    ctrl.Tag = new MenuEventArgs(ctrl.Key,Convert.ToString(favKey["usrFavObjParam2"]),ctrl.Text,ctrl.ImageIndex,true,false,0,Convert.ToString(favKey["usrFavObjParam3"]), false, Convert.ToString(favKey["usrFavObjParam1"]));
					ctrl.ContextMenu = mnuDynamicFav;
					pnlFavButtons.Controls.Add(ctrl, true);
					i++;
					if (i > 9) break;
				}
			}
			pnlFavButtons.Refresh();
		}


        /// <summary>
        /// Refreshes the Favourites Panel
        /// </summary>
        public void RefreshLast10()
        {
            bool isreg = Session.CurrentSession.CurrentTerminal.IsRegistered;

            Global.RemoveAndDisposeControls(pnlLast10Buttons);
            FWBS.OMS.Favourites last = new FWBS.OMS.Favourites(actions.ParentKey + "LAST10");
            DataView dv = last.GetDataView();
            dv.Sort = "usrFavObjParam4 ASC";
            int i = 0;
            pnlLast10Buttons.Controls.Clear();
            foreach (DataRowView favKey in dv)
            {
                if (Session.CurrentSession.CurrentUser.IsInRoles(Convert.ToString(favKey["usrFavObjParam3"])) && isreg)
                {
                    FWBS.OMS.UI.Windows.ucNavCmdButtons ctrl = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
                    ctrl.Text = FWBS.OMS.Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", Convert.ToString(favKey["usrFavObjParam2"])), true);
                    ctrl.Key = Convert.ToString(favKey["usrFavDesc"]);
                    ctrl.Tag = new MenuEventArgs(ctrl.Key, Convert.ToString(favKey["usrFavObjParam2"]), ctrl.Text, ctrl.ImageIndex, true, false, 0, Convert.ToString(favKey["usrFavObjParam3"]),false,Convert.ToString(favKey["usrFavObjParam1"]));
                    ctrl.ContextMenu = mnuLast10;
                    pnlLast10Buttons.Controls.Add(ctrl, true);
                    i++;
                    if (i > 9) break;
                }
            }
            last.Update();
            pnlLast10Buttons.Refresh();
        }


		/// <summary>
		/// Refreshes the Application Favourites Panel
		/// </summary>
		private void RefreshApplicationFavorites()
		{
			Global.RemoveAndDisposeControls(ucNavApplications);
            actions.BuildApplicationPanels(ucNavApplications);
            FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(actions.ParentKey + "APFAV");
			DataView dv = fav.GetDataView();

            foreach (DataRowView favKey in dv)
			{
				FWBS.OMS.UI.Windows.ucNavCmdButtons ctrl = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
				ctrl.Text = FWBS.OMS.Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("RESOURCE",Convert.ToString(favKey["usrFavDesc"])),true);
			    ctrl.Tag = Convert.ToString(favKey["usrFavObjParam1"]);
				ctrl.ContextMenu = mnuApplications;
				ucNavApplications.Controls.Add(ctrl, true);
				ctrl.SendToBack();
			}
			ucNavApplications.Refresh();
		}
		
    	/// <summary>
		/// Loads the Menu Action from the Favorites
		/// </summary>
		/// <param name="LinkButton"></param>
		private void pnlFavButtons_LinkClicked(FWBS.OMS.UI.Windows.ucNavCmdButtons LinkButton)
		{
			MenuEventArgs e = (MenuEventArgs)LinkButton.Tag;
			UpdateFavorites(e);
            ActionMenuClick(e);
		}

        private long GetSelectedButtonID(string itemIDs)
        {
            string[] IDs = itemIDs.Split(';');
            return Convert.ToInt64(IDs[0]);    
        }

        /// <summary>
        /// Loads the Menu Action from the Last 10 list
        /// </summary>
        /// <param name="LinkButton"></param>
        private void pnlLast10Buttons_LinkClicked(FWBS.OMS.UI.Windows.ucNavCmdButtons LinkButton)
        {
            MenuEventArgs e = (MenuEventArgs)LinkButton.Tag;
            ActionMenuClick(e);
        }

        private void ActionMenuClick(MenuEventArgs e)
        {
            long nodeID = GetSelectedButtonID(e.itemIDs);
            TreeNodeTagData tag = new TreeNodeTagData(nodeID, e.ButtonCode, e.ReturnKey.ToString(), e.System, e.Roles);
            actions.DisplaySelectedFunction(tag);
        }

		/// <summary>
		/// Removes the Favorite Item from the Command Bar
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuDyRemFav_Click(object sender, System.EventArgs e)
		{
			MenuEventArgs ee = (MenuEventArgs)mnuDynamicFav.SourceControl.Tag;
            FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(actions.ParentKey + "FAV");
			DataView dv = fav.GetDataView();
			foreach(DataRowView dr in dv)
				if (Convert.ToString(dr["usrFavDesc"]) == Convert.ToString(ee.ReturnKey)) 
				{
					fav.RemoveFavourite(dr.Row);
                    fav.Update();
					break;
				}
			RefreshFavorites();
            RefreshLast10();
		}

		/// <summary>
		/// Updates the pnlStatus Back Color to match the Parent of the Nav Panels
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlPanels_BackColorChanged(object sender, System.EventArgs e)
		{
			pnlUser.BackColor = pnlPanels.BackColor;
		}

		/// <summary>
		/// Shows the About Box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucSystemInformation_LinkClicked(object sender, System.EventArgs e)
		{
			FWBS.OMS.UI.Windows.Services.ShowAbout();
		}

        private void LoadResources()
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return;

            mnuDyRemFav.Text = Session.CurrentSession.Resources.GetResource("mnuRemFav", "Remove Item", "").Text;
            mnuRefreshFav.Text = Session.CurrentSession.Resources.GetResource("mnuRfshFav", "Refresh Favorites", "").Text;

            mnuRefreshLast10.Text = Session.CurrentSession.Resources.GetResource("mnuRfshLast10", "Refresh Last 10", "").Text;

            mnuRegApp.Text = Session.CurrentSession.Resources.GetResource("mnuAddApp", "Add Application", "").Text;
            mnuRemApp.Text = Session.CurrentSession.Resources.GetResource("mnuRemApp", "Remove Application", "").Text;
        }

		/// <summary>
		/// Refreshes Favorites from the Pop Menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuRefreshFav_Click(object sender, System.EventArgs e)
		{
			RefreshFavorites();
		}

        /// <summary>
        /// Refreshes Last 10 from the Pop Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuRefreshLast10_Click(object sender, System.EventArgs e)
        {
            RefreshLast10();
        }

        private void txtSearch_Enter(object sender, System.EventArgs e)
        {
            ParentForm.AcceptButton = btnSearchGo;
        }

        private void txtSearch_Leave(object sender, System.EventArgs e)
        {
            ParentForm.AcceptButton = null;
        }

		private void ucSystemUpdate_LinkClicked(object sender, System.EventArgs e)
		{
            actions.SystemUpdateClick(this, e);
		}

		private void mnuRegApp_Click(object sender, System.EventArgs e)
		{
			DataTable dt = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.AddApplication),null,FWBS.OMS.EnquiryEngine.EnquiryMode.Add,new FWBS.Common.KeyValueCollection()) as DataTable;
			if (dt != null)
			{
                FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(actions.ParentKey + "APFAV");
				fav.AddFavourite(Convert.ToString(dt.Rows[0]["appCode"]),"",Convert.ToString(dt.Rows[0]["appLocation"]));
				fav.Update();
				RefreshApplicationFavorites();
			}
		}

		private void ucNavApplications_LinkClicked(ucNavCmdButtons LinkButton)
		{
			if (Convert.ToString(LinkButton.Tag) != "")
			{
				try
				{
					System.Diagnostics.Process.Start(Convert.ToString(LinkButton.Tag));
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
			}
		}

		private void mnuRemApp_Click(object sender, System.EventArgs e)
		{
            FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(actions.ParentKey + "APFAV");
			DataView dv = fav.GetDataView();
			foreach(DataRowView dr in dv)
			{
				if (Convert.ToString(dr["usrFavObjParam1"]) == Convert.ToString(mnuApplications.SourceControl.Tag))
				{
					fav.RemoveFavourite(dr.Row);
					break;
				}
			}
			RefreshApplicationFavorites();
		}

		private void mnuApplications_Popup(object sender, System.EventArgs e)
		{
			mnuRemApp.Visible = !(mnuApplications.SourceControl == ucApplications);
		}


		#endregion

        #region TreeView Search

        private void txtTreeSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return && !string.IsNullOrWhiteSpace(this.txtTreeSearch.Text))
                ExecuteSearchProcess();
        }

        private void ExecuteSearchProcess()
        {
            if (!string.IsNullOrWhiteSpace(this.txtTreeSearch.Text))
            {
                Treenav.RemoveSearchMatches();
                Treenav.TreeViewSearch(this.txtTreeSearch.Text.ToUpper());
            }
        }

        private void btnRefreshTree_Click(object sender, EventArgs e)
        {
            if (Treenav != null)
            {
                Treenav.RemoveSearchMatches();
                this.txtTreeSearch.Text = null;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.txtTreeSearch.Text))
            {
                ExecuteSearchProcess();
            }
        }

        #endregion


        #region Old (v7.1) Style Search

        private void SetupAdminKitSearch(ucSearchControl akSearch)
        {
            try
            {
                if (CheckForAKSearchList())
                {
                    akSearch.SetSearchList("LADMKITSEARCH", null, new FWBS.Common.KeyValueCollection());
                    akSearch.SearchButtonCommands -= new SearchButtonEventHandler(akSearch_SearchButtonCommands);
                    akSearch.SearchButtonCommands += new SearchButtonEventHandler(akSearch_SearchButtonCommands);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private bool CheckForAKSearchList()
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            System.Data.DataTable dt = connection.ExecuteSQL("select * from dbsearchlistconfig where schCode = 'LADMKITSEARCH'", null);
            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        private void PerformAdminKitSearch()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.txtTreeSearch.Text))
                {
                    CreateSearchResultsTab();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void akSearch_SearchButtonCommands(object sender, SearchButtonEventArgs e)
        {
            try
            {
                if (e.ButtonName.ToUpper() == "CMDOPEN")
                {
                    try
                    {
                        ucSearchControl cast = sender as ucSearchControl;
                        int nodeID = Convert.ToInt32(cast.CurrentItem()["NodeID"].Value);
                        if (CheckUserHasAccessToFunction(Convert.ToString(cast.CurrentItem()["Roles"].Value)))
                        {
                            string nodeText = Convert.ToString(cast.CurrentItem()["NodeDescription"].Value);
                            if (!CheckForLicenseManager(nodeID))
                            {
                                CreateSelectedFunctionTab(nodeID, nodeText);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ParentForm, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private bool CheckForLicenseManager(int nodeID)
        {
            bool result = false;
            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection();
            kvc.Add("nodeID", nodeID);
            System.Data.DataTable dt = FWBS.OMS.UI.Windows.Services.RunDataList("DGetNodeData", kvc);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["admnuSearchListCode"]) == "LI" && ConvertDef.ToInt32(dt.Rows[0]["admnuParent"], 0) == 1)
                    return true;
            }
            return result;
        }

        private bool CheckUserHasAccessToFunction(string roles)
        {
            bool result = false;
            string[] nodeRoles = Convert.ToString(roles).Split(',');
            if (nodeRoles.Length > 0)
            {
                foreach (string role in nodeRoles)
                {
                    if (IsInRoles(role, FWBS.OMS.Session.CurrentSession.CurrentUser))
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                result = true;
            }
            if (!result)
                swf.MessageBox.Show(ResourceLookup.GetLookupText("AKSRCHNOACCESS"), ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return result;
        }

        private bool IsInRoles(string role, FWBS.OMS.User u)
        {
            if (string.IsNullOrEmpty(role))
                return true;

            string[] vals = u.Roles.Split(',');
            return Array.IndexOf(vals, role) > -1;
        }

        private void CreateSearchResultsTab()
        {
            CheckForExistingSearchResults();

            ucSearchControl akSearch = new ucSearchControl();
            akSearch.Dock = DockStyle.Fill;
            SetupAdminKitSearch(akSearch);

            Panel panel = new Panel();
            panel.Controls.Add(akSearch);
            akSearch.BringToFront();

            UltraTab tab = new UltraTab();
            UltraTabsCollection tabs = this.TabControl.Tabs;
            tab = tabs.Add("searchresults", "Search Results - " + this.txtTreeSearch.Text);
            tab.TabPage.Controls.Add(panel);
            panel.Dock = DockStyle.Fill;
            this.TabControl.SelectedTab = tab;
            UpdateSearchResults(akSearch);
        }

        private void UpdateSearchResults(ucSearchControl akSearch)
        {
            FWBS.Common.KeyValueCollection searchkeys = new FWBS.Common.KeyValueCollection();
            searchkeys.Add("DESC", this.txtTreeSearch.Text);
            searchkeys.Add("TYPE", parentkey);
            akSearch.SearchList.ChangeParameters(searchkeys);
            akSearch.Search();
        }

        private void CheckForExistingSearchResults()
        {
            foreach (UltraTab tab in this.TabControl.Tabs)
            {
                if (tab.Key == "searchresults")
                    tab.Close();
            }
        }

        private string SetNewTabKey()
        {
            if (this.TabControl.Tabs.Count == 0)
                return "1";
            else
                return Convert.ToString(this.TabControl.Tabs.Count + 1);
        }

        private void CreateSelectedFunctionTab(int nodeID, string description)
        {
            TreeNodeTagData tag = new TreeNodeTagData(nodeID, description, "LADMKITSEARCH", true, null);
            actions.DisplaySelectedFunction(tag);
        }

        #endregion

        public void ClearTreeSearch()
        {
            this.txtTreeSearch.Text = null;
        }

    }
}