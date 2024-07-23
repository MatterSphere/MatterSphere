using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Data;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    public delegate void OpenFolderEventHandler(object sender, OpenFolderEventArgs e);
	public delegate void HandledEventHandler(object sender, HandledEventArgs e);
	
	public class ucHome : System.Windows.Forms.UserControl
	{
		#region Controls
        private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel pnlMain;
		private FWBS.OMS.UI.ListView listView1;
        private FWBS.OMS.UI.Windows.ucPanelNav ucPanelNavMyFav; //my favourties
        private FWBS.OMS.UI.Windows.ucPanelNav ucPanelNavLast10; //last 10
		private FWBS.OMS.UI.Windows.ucNavCommands pnlFavButtons; //used for My Favourites
        private FWBS.OMS.UI.Windows.ucNavCommands pnlLast10Buttons; //used for Last 10
        private FWBS.OMS.UI.Windows.ucPanelNav ucConsolemgr;
        private ucNavCommands ucNavCommands2;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavCmdButtons10;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavCmdButtons11;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavCmdButtons12;
		private FWBS.OMS.UI.Windows.ucPanelNav ucPanelNavTop1;
		private FWBS.OMS.UI.Windows.ucNavCommands pnlCommands;
		private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavCmdButtons1;
		private FWBS.OMS.UI.TabControl tabControl1;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.Panel pnlPanels;
		private System.Windows.Forms.Panel pnlBack;
		private System.Windows.Forms.Panel pnlHomeTop;
		private System.Windows.Forms.Button btnSearchGo;
        private System.Windows.Forms.TextBox txtSearch;
        public Label labSearch;
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
		private FWBS.OMS.UI.Windows.omsSplitter splitter1;
        private System.ComponentModel.IContainer components;
        public System.Windows.Forms.Panel SearchPanel;
        private FWBS.OMS.UI.Windows.ResourceLookup ResourceObject;

        

        #endregion

		#region Fields
		/// <summary>
		/// The Current Folder id
		/// </summary>
		private string currentfolder = "1";
		/// <summary>
		/// The Unique Menu Code
		/// </summary>
		private string _menuname = "";
		/// <summary>
		/// The Enquiry Form Name to Add or Edit a Folder
		/// </summary>
		private string _addfolder = "";
		/// <summary>
		/// The Enquiry Form Name to Add or Edit a Menu Item
		/// </summary>
		private string _additem = "";
        /// <summary>
        /// The Enquiry Form Name to Add or Edit a Menu Item
        /// </summary>
        private string _addconsole = "";
		/// <summary>
		/// The Data Table that contains the current menu records
		/// </summary>
		private DataTable dtmenu;
		/// <summary>
		/// The Data List used to Access the Data Layer for the Menu Items
		/// </summary>
		private FWBS.OMS.EnquiryEngine.DataLists menu;
		/// <summary>
		/// The List View currently Selected Item on a Click
		/// </summary>
		private ListViewItem clickfolder;
		/// <summary>
		/// Meunus Parent Options
		/// </summary>
		private ArrayList history = new ArrayList();
		private System.Windows.Forms.MenuItem mnuRefreshFav;
        private System.Windows.Forms.MenuItem mnuRefreshLast10;
		private FWBS.OMS.UI.Windows.ucNavCommands ucNavApplications;
		private FWBS.OMS.UI.Windows.ucNavCmdButtons ucAddRemove;
		private FWBS.OMS.UI.Windows.ucNavCmdButtons ucSystemUpdate;
		private System.Windows.Forms.ContextMenu mnuApplications;
		private FWBS.OMS.UI.Windows.ucPanelNav ucApplications;
		private System.Windows.Forms.MenuItem mnuRegApp;
		private System.Windows.Forms.MenuItem mnuRemApp;
        private FWBS.Common.UI.Windows.eWebBrowser WebBrowser;
        private ColumnHeader colName;
        private Panel panel5;
        public Label LabelMenuName;
		/// <summary>
		/// The Parent Main Form
		/// </summary>
		private frmMain _mainparent= null;
		#endregion

		#region Events
		public event EventHandler AddRemoveAddinClick;

		public event EventHandler SystemUpdateClick;

		public event EventHandler ApplicationPanelClear;

		public event OpenFolderEventHandler FolderOpening;

		public event LinkEventHandler ApplicationLinkClicked;

        public event CancelEventHandler LoadBrowser;


        protected virtual bool OnLoadBrowser()
        {
            CancelEventArgs e = new CancelEventArgs();
            if (LoadBrowser != null)
            {
                LoadBrowser(SearchPanel, e);
                HideAdminKitSearchFunction();
                return (e.Cancel == false);
            }
            else
                return true;
        }


        private void HideAdminKitSearchFunction()
        {
            if (_menuname == "ADMIN")
            {
                bool showSearch = CheckAKSearchVisibility();
                btnSearchGo.Visible = showSearch;
                txtSearch.Visible = showSearch;
                labSearch.Visible = showSearch;
            }
        }


        private bool CheckAKSearchVisibility()
        {
            if(!Session.CurrentSession.CurrentTerminal.IsRegistered)
                return false;

            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            System.Data.DataTable dt = connection.ExecuteSQL("select * from dbsearchlistconfig where schCode = 'LADMKITSEARCH'",null);
            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }


        protected virtual bool OnFolderOpening(string Name)
		{
			if (FolderOpening != null)
			{
				OpenFolderEventArgs e = new OpenFolderEventArgs();
				e.Name = Name;
				FolderOpening(this,e);
				return e.Cancel;
			}
			else
				return false;
		}
		
		protected virtual void OnLinkClicked(ucNavCmdButtons sender) 
		{
			if (ApplicationLinkClicked != null)
				ApplicationLinkClicked(sender);
		}
		
		public event HandledEventHandler GoClick;

	    public bool OnGoClick()
		{
			HandledEventArgs e = new HandledEventArgs();
			if (GoClick!= null)
				GoClick(this,e);
			return e.Handled;
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

		/// <summary>
		/// The Event that is Fired when a Menu Item is Double Clicked Entered on or Clicked from the Favorites / Last 10
		/// </summary>
		public event MenuEventHandler MenuActioned;

		public void OnMenuActioned(MenuEventArgs item)
		{
			if (MenuActioned != null)
				MenuActioned(this,item);
		}
        
		/// <summary>
		/// The Event that is Fired when the Parent Action is Used
		/// </summary>
		public event EventHandler ParentEnabledChanged;

		public void OnParentEnabledChanged()
		{
			if (ParentEnabledChanged != null)
				ParentEnabledChanged(this,EventArgs.Empty);
		}

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                listView1.RightToLeftLayout = true;
            base.OnRightToLeftChanged(e);
        }
        #endregion

		#region Properties
        
        /// <summary>
		/// Is the Parent Folder Available
		/// </summary>
		public bool IsParentEnabled
		{
			get
			{
				return history.Count!=0;
			}
		}

		/// <summary>
		/// The Unique Menu Code
		/// </summary>
		[Browsable(false)]
		public string MenuCode
		{
			get
			{
				return _menuname;
			}
			set
			{
				_menuname = value;
			}
		}

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
	

		public DataTable MenuDataTable
		{
			get
			{
				return dtmenu.Copy();
			}
		}

		public ucNavCommands ApplicationCommands
		{
			get
			{
				return ucNavApplications;
			}
		}

        public FWBS.OMS.UI.Windows.ucNavCmdButtons AddRemoveButton
        {
            get
            {
                return ucAddRemove;
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Entities"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "System"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Designer"}, -1, System.Drawing.SystemColors.WindowText, System.Drawing.SystemColors.Window, new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnlBack = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.SearchPanel = new System.Windows.Forms.Panel();
            this.listView1 = new FWBS.OMS.UI.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel5 = new System.Windows.Forms.Panel();
            this.tabControl1 = new FWBS.OMS.UI.TabControl();
            this.panel8 = new System.Windows.Forms.Panel();
            this.splitter1 = new FWBS.OMS.UI.Windows.omsSplitter();
            this.pnlPanels = new System.Windows.Forms.Panel();
            this.ucPanelNavMyFav = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.pnlFavButtons = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucPanelNavLast10 = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.pnlLast10Buttons = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucConsolemgr = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavCommands2 = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucNavCmdButtons10 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucNavCmdButtons11 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucNavCmdButtons12 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucApplications = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.mnuApplications = new System.Windows.Forms.ContextMenu();
            this.mnuRegApp = new System.Windows.Forms.MenuItem();
            this.mnuRemApp = new System.Windows.Forms.MenuItem();
            this.ucNavApplications = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucPanelNavTop1 = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.pnlCommands = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucNavCmdButtons1 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucAddRemove = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucSystemUpdate = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
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
            this.pnlHomeTop = new System.Windows.Forms.Panel();
            this.LabelMenuName = new System.Windows.Forms.Label();
            this.btnSearchGo = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.labSearch = new System.Windows.Forms.Label();
            this.mnuDyRemFav = new System.Windows.Forms.MenuItem();
            this.mnuRefreshFav = new System.Windows.Forms.MenuItem();
            this.mnuDynamicFav = new System.Windows.Forms.ContextMenu();
            this.mnuLast10 = new System.Windows.Forms.ContextMenu();
            this.mnuRefreshLast10 = new System.Windows.Forms.MenuItem();
            this.ResourceObject = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlBack.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlPanels.SuspendLayout();
            this.ucPanelNavMyFav.SuspendLayout();
            this.ucPanelNavLast10.SuspendLayout();
            this.ucConsolemgr.SuspendLayout();
            this.ucNavCommands2.SuspendLayout();
            this.ucApplications.SuspendLayout();
            this.ucPanelNavTop1.SuspendLayout();
            this.pnlCommands.SuspendLayout();
            this.pnlCulture.SuspendLayout();
            this.pnlServer.SuspendLayout();
            this.pnlDatabase.SuspendLayout();
            this.pnlUser.SuspendLayout();
            this.pnlHomeTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(689, 405);
            this.panel1.TabIndex = 21;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.pnlBack);
            this.panel2.Controls.Add(this.pnlHomeTop);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(689, 405);
            this.panel2.TabIndex = 0;
            // 
            // pnlBack
            // 
            this.pnlBack.Controls.Add(this.pnlMain);
            this.pnlBack.Controls.Add(this.splitter1);
            this.pnlBack.Controls.Add(this.pnlPanels);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Location = new System.Drawing.Point(0, 33);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.pnlBack.Size = new System.Drawing.Size(689, 372);
            this.pnlBack.TabIndex = 1;
            this.pnlBack.Enter += new System.EventHandler(this.ucHome_Enter);
            // 
            // pnlMain
            // 
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.SearchPanel);
            this.pnlMain.Controls.Add(this.listView1);
            this.pnlMain.Controls.Add(this.panel5);
            this.pnlMain.Controls.Add(this.tabControl1);
            this.pnlMain.Controls.Add(this.panel8);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(199, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(485, 367);
            this.pnlMain.TabIndex = 2;
            // 
            // SearchPanel
            // 
            this.SearchPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SearchPanel.Location = new System.Drawing.Point(6, 8);
            this.SearchPanel.Name = "SearchPanel";
            this.SearchPanel.Size = new System.Drawing.Size(477, 357);
            this.SearchPanel.TabIndex = 0;
            this.SearchPanel.Visible = false;
            // 
            // listView1
            // 
            this.listView1.AllowColumnReorder = true;
            this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3});
            this.listView1.Location = new System.Drawing.Point(6, 8);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(477, 357);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            this.listView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.listView1_KeyPress);
            // 
            // colName
            // 
            this.colName.Text = "Consoles";
            this.colName.Width = 400;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.Window;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(6, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(477, 8);
            this.panel5.TabIndex = 27;
            // 
            // tabControl1
            // 
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(477, 365);
            this.tabControl1.TabIndex = 2;
            this.tabControl1.Visible = false;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.SystemColors.Window;
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(6, 365);
            this.panel8.TabIndex = 26;
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.Window;
            this.splitter1.Location = new System.Drawing.Point(194, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 367);
            this.splitter1.TabIndex = 16;
            this.splitter1.TabStop = false;
            // 
            // pnlPanels
            // 
            this.pnlPanels.AutoScroll = true;
            this.pnlPanels.AutoScrollMargin = new System.Drawing.Size(25, 0);
            this.pnlPanels.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlPanels.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPanels.Controls.Add(this.ucPanelNavMyFav);
            this.pnlPanels.Controls.Add(this.ucPanelNavLast10);
            this.pnlPanels.Controls.Add(this.ucConsolemgr);
            this.pnlPanels.Controls.Add(this.ucApplications);
            this.pnlPanels.Controls.Add(this.ucPanelNavTop1);
            this.pnlPanels.Controls.Add(this.pnlCulture);
            this.pnlPanels.Controls.Add(this.pnlServer);
            this.pnlPanels.Controls.Add(this.pnlDatabase);
            this.pnlPanels.Controls.Add(this.pnlUser);
            this.pnlPanels.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPanels.Location = new System.Drawing.Point(5, 0);
            this.pnlPanels.Name = "pnlPanels";
            this.pnlPanels.Padding = new System.Windows.Forms.Padding(8, 2, 8, 8);
            this.pnlPanels.Size = new System.Drawing.Size(189, 367);
            this.pnlPanels.TabIndex = 0;
            this.pnlPanels.BackColorChanged += new System.EventHandler(this.pnlPanels_BackColorChanged);
            // 
            // ucPanelNavMyFav
            // 
            this.ucPanelNavMyFav.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNavMyFav.Controls.Add(this.pnlFavButtons);
            this.ucPanelNavMyFav.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNavMyFav.ExpandedHeight = 31;
            this.ucPanelNavMyFav.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNavMyFav.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanelNavMyFav.Location = new System.Drawing.Point(8, 202);
            this.ResourceObject.SetLookup(this.ucPanelNavMyFav, new FWBS.OMS.UI.Windows.ResourceLookupItem("MYFAVOURITES", "My Favourites", ""));
            this.ucPanelNavMyFav.Name = "ucPanelNavMyFav";
            this.ucPanelNavMyFav.Size = new System.Drawing.Size(171, 31);
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
            this.pnlFavButtons.Size = new System.Drawing.Size(171, 0);
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
            this.ucPanelNavLast10.Location = new System.Drawing.Point(8, 171);
            this.ResourceObject.SetLookup(this.ucPanelNavLast10, new FWBS.OMS.UI.Windows.ResourceLookupItem("TOP10", "Last 10", ""));
            this.ucPanelNavLast10.Name = "ucPanelNavLast10";
            this.ucPanelNavLast10.Size = new System.Drawing.Size(171, 31);
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
            this.pnlLast10Buttons.Size = new System.Drawing.Size(171, 0);
            this.pnlLast10Buttons.TabIndex = 5;
            this.pnlLast10Buttons.TabStop = false;
            this.pnlLast10Buttons.LinkClicked += new FWBS.OMS.UI.Windows.LinkEventHandler(this.pnlLast10Buttons_LinkClicked);
            // 
            // ucConsolemgr
            // 
            this.ucConsolemgr.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucConsolemgr.Controls.Add(this.ucNavCommands2);
            this.ucConsolemgr.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucConsolemgr.Expanded = false;
            this.ucConsolemgr.ExpandedHeight = 107;
            this.ucConsolemgr.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucConsolemgr.HeaderColor = System.Drawing.Color.Empty;
            this.ucConsolemgr.Location = new System.Drawing.Point(8, 140);
            this.ResourceObject.SetLookup(this.ucConsolemgr, new FWBS.OMS.UI.Windows.ResourceLookupItem("CONSOLEMANAGER", "Console Manager", ""));
            this.ucConsolemgr.Name = "ucConsolemgr";
            this.ucConsolemgr.Size = new System.Drawing.Size(171, 31);
            this.ucConsolemgr.TabIndex = 31;
            this.ucConsolemgr.TabStop = false;
            this.ucConsolemgr.Text = "#Console Manager";
            this.ucConsolemgr.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ucNavCommands2
            // 
            this.ucNavCommands2.Controls.Add(this.ucNavCmdButtons10);
            this.ucNavCommands2.Controls.Add(this.ucNavCmdButtons11);
            this.ucNavCommands2.Controls.Add(this.ucNavCmdButtons12);
            this.ucNavCommands2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavCommands2.Location = new System.Drawing.Point(0, 24);
            this.ucNavCommands2.Name = "ucNavCommands2";
            this.ucNavCommands2.Padding = new System.Windows.Forms.Padding(5);
            this.ucNavCommands2.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavCommands2.Size = new System.Drawing.Size(171, 0);
            this.ucNavCommands2.TabIndex = 15;
            this.ucNavCommands2.TabStop = false;
            this.ucNavCommands2.LinkClicked += new FWBS.OMS.UI.Windows.LinkEventHandler(this.ucNavCommands2_LinkClicked);
            // 
            // ucNavCmdButtons10
            // 
            this.ucNavCmdButtons10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavCmdButtons10.ImageIndex = -1;
            this.ucNavCmdButtons10.Key = "NewConsole";
            this.ucNavCmdButtons10.Location = new System.Drawing.Point(5, -71);
            this.ResourceObject.SetLookup(this.ucNavCmdButtons10, new FWBS.OMS.UI.Windows.ResourceLookupItem("NEWCONSOLE", "Create New Console", ""));
            this.ucNavCmdButtons10.Name = "ucNavCmdButtons10";
            this.ucNavCmdButtons10.Size = new System.Drawing.Size(161, 22);
            this.ucNavCmdButtons10.TabIndex = 1;
            this.ucNavCmdButtons10.Text = "Create New Console";
            // 
            // ucNavCmdButtons11
            // 
            this.ucNavCmdButtons11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavCmdButtons11.ImageIndex = -1;
            this.ucNavCmdButtons11.Key = "EditConsole";
            this.ucNavCmdButtons11.Location = new System.Drawing.Point(5, -49);
            this.ResourceObject.SetLookup(this.ucNavCmdButtons11, new FWBS.OMS.UI.Windows.ResourceLookupItem("EDITCONSOLE", "Edit Console", ""));
            this.ucNavCmdButtons11.Name = "ucNavCmdButtons11";
            this.ucNavCmdButtons11.Size = new System.Drawing.Size(161, 22);
            this.ucNavCmdButtons11.TabIndex = 6;
            this.ucNavCmdButtons11.Text = "Edit Console";
            // 
            // ucNavCmdButtons12
            // 
            this.ucNavCmdButtons12.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavCmdButtons12.ImageIndex = -1;
            this.ucNavCmdButtons12.Key = "DeleteConsole";
            this.ucNavCmdButtons12.Location = new System.Drawing.Point(5, -27);
            this.ResourceObject.SetLookup(this.ucNavCmdButtons12, new FWBS.OMS.UI.Windows.ResourceLookupItem("DELETECONSOLE", "Delete Console", ""));
            this.ucNavCmdButtons12.Name = "ucNavCmdButtons12";
            this.ucNavCmdButtons12.Size = new System.Drawing.Size(161, 22);
            this.ucNavCmdButtons12.TabIndex = 5;
            this.ucNavCmdButtons12.Text = "Delete Console";
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
            this.ucApplications.Size = new System.Drawing.Size(171, 31);
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
            this.ucNavApplications.Size = new System.Drawing.Size(171, 0);
            this.ucNavApplications.TabIndex = 15;
            this.ucNavApplications.TabStop = false;
            this.ucNavApplications.LinkClicked += new FWBS.OMS.UI.Windows.LinkEventHandler(this.OnLinkClicked);
            // 
            // ucPanelNavTop1
            // 
            this.ucPanelNavTop1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNavTop1.Controls.Add(this.pnlCommands);
            this.ucPanelNavTop1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNavTop1.ExpandedHeight = 107;
            this.ucPanelNavTop1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNavTop1.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNavTop1.Location = new System.Drawing.Point(8, 2);
            this.ResourceObject.SetLookup(this.ucPanelNavTop1, new FWBS.OMS.UI.Windows.ResourceLookupItem("UCSYSTASKS", "System Tasks", ""));
            this.ucPanelNavTop1.Name = "ucPanelNavTop1";
            this.ucPanelNavTop1.Size = new System.Drawing.Size(171, 107);
            this.ucPanelNavTop1.TabIndex = 31;
            this.ucPanelNavTop1.TabStop = false;
            this.ucPanelNavTop1.Text = "#System Tasks";
            this.ucPanelNavTop1.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // pnlCommands
            // 
            this.pnlCommands.Controls.Add(this.ucNavCmdButtons1);
            this.pnlCommands.Controls.Add(this.ucAddRemove);
            this.pnlCommands.Controls.Add(this.ucSystemUpdate);
            this.pnlCommands.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCommands.Location = new System.Drawing.Point(0, 24);
            this.pnlCommands.Name = "pnlCommands";
            this.pnlCommands.Padding = new System.Windows.Forms.Padding(5);
            this.pnlCommands.PanelBackColor = System.Drawing.Color.Empty;
            this.pnlCommands.Size = new System.Drawing.Size(171, 76);
            this.pnlCommands.TabIndex = 16;
            this.pnlCommands.TabStop = false;
            // 
            // ucNavCmdButtons1
            // 
            this.ucNavCmdButtons1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavCmdButtons1.ImageIndex = -1;
            this.ucNavCmdButtons1.Key = "SystemInf";
            this.ucNavCmdButtons1.Location = new System.Drawing.Point(5, 5);
            this.ResourceObject.SetLookup(this.ucNavCmdButtons1, new FWBS.OMS.UI.Windows.ResourceLookupItem("SYSINFO", "System Information", ""));
            this.ucNavCmdButtons1.Name = "ucNavCmdButtons1";
            this.ucNavCmdButtons1.Size = new System.Drawing.Size(161, 22);
            this.ucNavCmdButtons1.TabIndex = 35;
            this.ucNavCmdButtons1.Text = "#System Information";
            this.ucNavCmdButtons1.LinkClicked += new System.EventHandler(this.ucNavCmdButtons1_LinkClicked);
            // 
            // ucAddRemove
            // 
            this.ucAddRemove.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucAddRemove.ImageIndex = -1;
            this.ucAddRemove.Key = "AddRemove";
            this.ucAddRemove.Location = new System.Drawing.Point(5, 27);
            this.ResourceObject.SetLookup(this.ucAddRemove, new FWBS.OMS.UI.Windows.ResourceLookupItem("ADDREMOVE", "Add & Remove Addins", ""));
            this.ucAddRemove.Name = "ucAddRemove";
            this.ucAddRemove.Size = new System.Drawing.Size(161, 22);
            this.ucAddRemove.TabIndex = 36;
            this.ucAddRemove.Text = "#Add & Remove Addins";
            this.ucAddRemove.LinkClicked += new System.EventHandler(this.ucAddRemove_LinkClicked);
            // 
            // ucSystemUpdate
            // 
            this.ucSystemUpdate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucSystemUpdate.ImageIndex = -1;
            this.ucSystemUpdate.Location = new System.Drawing.Point(5, 49);
            this.ResourceObject.SetLookup(this.ucSystemUpdate, new FWBS.OMS.UI.Windows.ResourceLookupItem("SYSUPD", "System Update", ""));
            this.ucSystemUpdate.Name = "ucSystemUpdate";
            this.ucSystemUpdate.Size = new System.Drawing.Size(161, 22);
            this.ucSystemUpdate.TabIndex = 37;
            this.ucSystemUpdate.Text = "#System Update";
            this.ucSystemUpdate.LinkClicked += new System.EventHandler(this.ucSystemUpdate_LinkClicked);
            // 
            // pnlCulture
            // 
            this.pnlCulture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCulture.Controls.Add(this.labCulture);
            this.pnlCulture.Controls.Add(this.label2);
            this.pnlCulture.Location = new System.Drawing.Point(9, 292);
            this.pnlCulture.Name = "pnlCulture";
            this.pnlCulture.Size = new System.Drawing.Size(171, 16);
            this.pnlCulture.TabIndex = 35;
            // 
            // labCulture
            // 
            this.labCulture.AutoSize = true;
            this.labCulture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labCulture.Location = new System.Drawing.Point(62, 0);
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
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Culture : ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlServer
            // 
            this.pnlServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlServer.Controls.Add(this.labServer);
            this.pnlServer.Controls.Add(this.labServerCap);
            this.pnlServer.Location = new System.Drawing.Point(9, 308);
            this.pnlServer.Name = "pnlServer";
            this.pnlServer.Size = new System.Drawing.Size(171, 16);
            this.pnlServer.TabIndex = 34;
            // 
            // labServer
            // 
            this.labServer.AutoSize = true;
            this.labServer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labServer.Location = new System.Drawing.Point(62, 0);
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
            this.labServerCap.Size = new System.Drawing.Size(62, 16);
            this.labServerCap.TabIndex = 1;
            this.labServerCap.Text = "Server : ";
            this.labServerCap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlDatabase
            // 
            this.pnlDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDatabase.Controls.Add(this.labDatabase);
            this.pnlDatabase.Controls.Add(this.labDatabaseCap);
            this.pnlDatabase.Location = new System.Drawing.Point(9, 324);
            this.pnlDatabase.Name = "pnlDatabase";
            this.pnlDatabase.Size = new System.Drawing.Size(171, 16);
            this.pnlDatabase.TabIndex = 33;
            // 
            // labDatabase
            // 
            this.labDatabase.AutoSize = true;
            this.labDatabase.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labDatabase.Location = new System.Drawing.Point(62, 0);
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
            this.labDatabaseCap.Size = new System.Drawing.Size(62, 16);
            this.labDatabaseCap.TabIndex = 1;
            this.labDatabaseCap.Text = "Database : ";
            this.labDatabaseCap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlUser
            // 
            this.pnlUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlUser.Controls.Add(this.labUser);
            this.pnlUser.Controls.Add(this.labUserCap);
            this.pnlUser.Location = new System.Drawing.Point(9, 340);
            this.pnlUser.Name = "pnlUser";
            this.pnlUser.Size = new System.Drawing.Size(171, 16);
            this.pnlUser.TabIndex = 32;
            // 
            // labUser
            // 
            this.labUser.AutoSize = true;
            this.labUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labUser.Location = new System.Drawing.Point(62, 0);
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
            this.labUserCap.Size = new System.Drawing.Size(62, 16);
            this.labUserCap.TabIndex = 1;
            this.labUserCap.Text = "User : ";
            this.labUserCap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlHomeTop
            // 
            this.pnlHomeTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(145)))), ((int)(((byte)(0)))));
            this.pnlHomeTop.Controls.Add(this.LabelMenuName);
            this.pnlHomeTop.Controls.Add(this.btnSearchGo);
            this.pnlHomeTop.Controls.Add(this.txtSearch);
            this.pnlHomeTop.Controls.Add(this.labSearch);
            this.pnlHomeTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHomeTop.Location = new System.Drawing.Point(0, 0);
            this.pnlHomeTop.Name = "pnlHomeTop";
            this.pnlHomeTop.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.pnlHomeTop.Size = new System.Drawing.Size(689, 33);
            this.pnlHomeTop.TabIndex = 24;
            // 
            // LabelMenuName
            // 
            this.LabelMenuName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelMenuName.BackColor = System.Drawing.Color.Transparent;
            this.LabelMenuName.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelMenuName.ForeColor = System.Drawing.Color.White;
            this.LabelMenuName.Location = new System.Drawing.Point(513, 0);
            this.LabelMenuName.Name = "LabelMenuName";
            this.LabelMenuName.Size = new System.Drawing.Size(162, 31);
            this.LabelMenuName.TabIndex = 4;
            this.LabelMenuName.Text = "3E MatterSphere";
            this.LabelMenuName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnSearchGo
            // 
            this.btnSearchGo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSearchGo.Location = new System.Drawing.Point(300, 6);
            this.ResourceObject.SetLookup(this.btnSearchGo, new FWBS.OMS.UI.Windows.ResourceLookupItem("GO", "GO", ""));
            this.btnSearchGo.Name = "btnSearchGo";
            this.btnSearchGo.Size = new System.Drawing.Size(36, 21);
            this.btnSearchGo.TabIndex = 2;
            this.btnSearchGo.Text = "#Go";
            this.btnSearchGo.Click += new System.EventHandler(this.btnSearchGo_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(104, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(190, 23);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.Enter += new System.EventHandler(this.txtSearch_Enter);
            this.txtSearch.Leave += new System.EventHandler(this.txtSearch_Leave);
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
            // ucHome
            // 
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ucHome";
            this.Size = new System.Drawing.Size(689, 405);
            this.Load += new System.EventHandler(this.ucHome_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlBack.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlPanels.ResumeLayout(false);
            this.ucPanelNavMyFav.ResumeLayout(false);
            this.ucPanelNavLast10.ResumeLayout(false);
            this.ucConsolemgr.ResumeLayout(false);
            this.ucNavCommands2.ResumeLayout(false);
            this.ucApplications.ResumeLayout(false);
            this.ucPanelNavTop1.ResumeLayout(false);
            this.pnlCommands.ResumeLayout(false);
            this.pnlCulture.ResumeLayout(false);
            this.pnlCulture.PerformLayout();
            this.pnlServer.ResumeLayout(false);
            this.pnlServer.PerformLayout();
            this.pnlDatabase.ResumeLayout(false);
            this.pnlDatabase.PerformLayout();
            this.pnlUser.ResumeLayout(false);
            this.pnlUser.PerformLayout();
            this.pnlHomeTop.ResumeLayout(false);
            this.pnlHomeTop.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Contructors
		internal ucHome()
		{
			InitializeComponent();
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

                    if (listView1.LargeImageList != null)
                    {
                        listView1.LargeImageList.Dispose();
                        listView1.LargeImageList = null;
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

                    _mainparent = null;

                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
            }
            finally
            {
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

		public void InitalizeSetup(string MenuCode, frmMain mainparent)
		{
			if (Session.CurrentSession.IsLoggedIn)
			{
				_mainparent = mainparent;
				_menuname = MenuCode;
				listView1.LargeImageList = FWBS.OMS.UI.Windows.Images.AdminMenu48();
                listView1.SmallImageList = FWBS.OMS.UI.Windows.Images.AdminMenu16();
                listView1.FullRowSelect = true;
				if (DataLists.Exists("DSADMENU") == false)
					throw new FWBS.OMS.OMSException(FWBS.OMS.HelpIndexes.DataListCodeDoesNotExist,true,"DSADMENU");
				
				menu = new DataLists("DSADMENU");
				FWBS.Common.KeyValueCollection _params = new FWBS.Common.KeyValueCollection();
				_params.Add("MENUNAME",_menuname);
				menu.ChangeParameters(_params);
				
				dtmenu = menu.GetTable();
				LoadMenu("1",true);
				history.Clear();
				RefreshFavorites();
                RefreshLast10();
				ucNavApplications.LinkClicked +=new LinkEventHandler(ucNavApplications_LinkClicked);
				
				_addfolder = Session.CurrentSession.DefaultSystemForm(SystemForms.MenuFolder);
				_additem = Session.CurrentSession.DefaultSystemForm(SystemForms.MenuItem);
                _addconsole = Session.CurrentSession.DefaultSystemForm(SystemForms.ConsoleItem);
			}
		}

        /// <summary>
        /// Detail View
        /// </summary>
        public void DetailView()
        {
            listView1.View = View.Details;
        }

        /// <summary>
        /// Icon View
        /// </summary>
        public void IconView()
        {
            listView1.View = View.LargeIcon;
        }

		/// <summary>
		/// Redrwaw the Menu
		/// </summary>
		public override void Refresh()
		{
			dtmenu = menu.GetTable();
			LoadMenu(currentfolder,true);
			base.Refresh();
		}

		/// <summary>
		/// Loads a Menu
		/// </summary>
		/// <param name="folder">The Menu ID</param>
		public void LoadMenu(string folder)
		{
			LoadMenu(folder,false);
		}
			
		/// <summary>
		/// Loads a Menu
		/// </summary>
		/// <param name="folder">The Menu ID</param>
		/// <param name="NoHistory">Option not to Store the Parent History</param>
		public void LoadMenu(string folder, bool NoHistory)
		{
			MenuEventArgs e = new MenuEventArgs(folder,"","",0,false,true,0,"",true,"");
			LoadMenu(e,NoHistory);
		}

		/// <summary>
		/// Moves to the Parent Folder
		/// </summary>
		public void ParentFolder()
		{
			if (history.Count >0)
			{
				currentfolder = Convert.ToString(history[history.Count-1]);
				history.RemoveAt(history.Count-1);
				if (history.Count == 0)
					OnParentEnabledChanged();
				// Here is an example of why it should not log the History because it would get into
				// a infinate loop
				LoadMenu(currentfolder,true);	
			}
		}

		/// <summary>
		/// Moves to the Root Folder
		/// </summary>
		public void RootFolder()
		{
			if (history.Count >0)
			{
				history.RemoveRange(1,history.Count-1);
				currentfolder = Convert.ToString(history[history.Count-1]);
				history.RemoveAt(history.Count-1);
				OnParentEnabledChanged();
				LoadMenu(currentfolder,true);	
			}
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
				FWBS.OMS.Favourites fw = new FWBS.OMS.Favourites(_menuname + "FAV",Convert.ToString(e.ReturnKey));
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
			FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(_menuname + "FAV");
			DataView dv = fav.GetDataView();
			dv.Sort = "usrFavObjParam4 DESC";
			int i=0;
            if (pnlFavButtons.ImageList == null)
                pnlFavButtons.ImageList = FWBS.OMS.UI.Windows.Images.AdminMenu16();
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
					pnlFavButtons.Controls.Add(ctrl);
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
            FWBS.OMS.Favourites last = new FWBS.OMS.Favourites(_menuname + "LAST10");
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
                    pnlLast10Buttons.Controls.Add(ctrl);
                    i++;
                    if (i > 9) break;
                }
            }
            last.Update();
            pnlLast10Buttons.Refresh();
        }


		/// <summary>
		/// Refreshs the Application Favourites Panel
		/// </summary>
		private void RefreshApplicationFavorites()
		{
			Global.RemoveAndDisposeControls(ucNavApplications);
			if (ApplicationPanelClear != null) ApplicationPanelClear(this,EventArgs.Empty);
			FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(_menuname + "APFAV");
			DataView dv = fav.GetDataView();

            foreach (DataRowView favKey in dv)
			{
				FWBS.OMS.UI.Windows.ucNavCmdButtons ctrl = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
				ctrl.Text = FWBS.OMS.Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("APPMENU",Convert.ToString(favKey["usrFavDesc"])),true);
				ctrl.Tag = Convert.ToString(favKey["usrFavObjParam1"]);
				ctrl.ContextMenu = mnuApplications;
				ucNavApplications.Controls.Add(ctrl);
				ctrl.SendToBack();
			}
			ucNavApplications.Refresh();
		}
		
		/// <summary>
		/// Passed by the List View so if it's a Folder it will Change
		/// </summary>
		/// <param name="foldere"></param>
		private void LoadMenu(MenuEventArgs foldere)
		{
			LoadMenu(foldere,false);
		}

		/// <summary>
		/// Passed by the List View so if it's a Folder it will Change
		/// </summary>
		/// <param name="foldere">Folders Menu Events Args</param>
		/// <param name="NoHistory">Option not to Stores the History</param>
		private void LoadMenu(MenuEventArgs foldere, bool NoHistory)
		{
			try
			{
				bool isreg = Session.CurrentSession.CurrentTerminal.IsRegistered;

				if (foldere.IsFolder)
				{
					if (OnFolderOpening(Convert.ToString(foldere.ButtonCode))) return;

					if (!NoHistory) 
					{
						history.Add(currentfolder);
						OnParentEnabledChanged();
					}
					listView1.BeginUpdate();
					listView1.Items.Clear();
					dtmenu.DefaultView.RowFilter = "admnuParent = " + foldere.ReturnKey;
					foreach(DataRowView drv in dtmenu.DefaultView)
					{
						//Make sure the licensing option is always avaialable.
						string command = Convert.ToString(drv["admnuSearchListCode"]);

						if ((Session.CurrentSession.CurrentUser.IsInRoles(Convert.ToString(drv["admnuRoles"])) && isreg) || command == "LI")
						{
							string name  = FWBS.OMS.Session.CurrentSession.Terminology.Parse(Convert.ToString(drv["menudesc"]),true);
                            int index = Convert.ToInt32(drv["admnuImageIndex"]);
                            ListViewItem lvi = new ListViewItem(new string[1] { name }, index);
                            lvi.Font = new System.Drawing.Font(FWBS.OMS.CurrentUIVersion.Font, FWBS.OMS.CurrentUIVersion.FontSize);
							if (command == "")
								lvi.Tag = new MenuEventArgs(Convert.ToString(drv["admnuID"]), Convert.ToString(drv["admnuCode"]), lvi.Text, lvi.ImageIndex,Convert.ToBoolean(drv["admnuIncFav"]),true,Convert.ToInt32(drv["admnuID"]), Convert.ToString(drv["admnuRoles"]),Convert.ToBoolean(drv["admnuSystem"]));
							else
								lvi.Tag = new MenuEventArgs(command, Convert.ToString(drv["admnuCode"]), lvi.Text, lvi.ImageIndex,Convert.ToBoolean(drv["admnuIncFav"]),false,Convert.ToInt32(drv["admnuID"]),Convert.ToString(drv["admnuRoles"]),Convert.ToBoolean(drv["admnuSystem"]));
							listView1.Items.Add(lvi);
						}
					}
					if (foldere.ButtonCaption == "")
					{
						if (ParentForm != null) ParentForm.Text = "Main Menu";
					}
					else
						if (ParentForm != null) ParentForm.Text = "Main Menu - " + foldere.ButtonCaption;
					listView1.EndUpdate();
					currentfolder = Convert.ToString(foldere.ReturnKey);
					if (!NoHistory) OnMenuChanged();
					listView1.Focus();
				}
				else
				{
					if (clickfolder != null)
					{
						UpdateFavorites(foldere);
						OnMenuActioned(foldere);
					}
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
		}

		/// <summary>
		/// Sets the click folder
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (listView1.SelectedItems.Count > 0)
                clickfolder = listView1.SelectedItems[0];
            else
                clickfolder = null;
		}

		/// <summary>
		/// Loads the Folder or runs the Menu Action
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_DoubleClick(object sender, System.EventArgs e)
		{
			try
			{
				MenuEventArgs ne = (MenuEventArgs)clickfolder.Tag;
				if (clickfolder != null)
				{
					LoadMenu(ne);
				}
			}
			catch{}
		}

		/// <summary>
		/// Loads the Folder or runs the Menu Action
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void listView1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13 && clickfolder != null)
			{
				MenuEventArgs ne = (MenuEventArgs)clickfolder.Tag;
				if (clickfolder != null) LoadMenu(ne);
			}
			else if (e.KeyChar == 8)
			{
				ParentFolder();
			}
		}


		/// <summary>
		/// Loads the Menu Action from the Favorites
		/// </summary>
		/// <param name="LinkButton"></param>
		private void pnlFavButtons_LinkClicked(FWBS.OMS.UI.Windows.ucNavCmdButtons LinkButton)
		{
			MenuEventArgs e = (MenuEventArgs)LinkButton.Tag;
			UpdateFavorites(e);
			OnMenuActioned(e);
		}


        /// <summary>
        /// Loads the Menu Action from the Last 10 list
        /// </summary>
        /// <param name="LinkButton"></param>
        private void pnlLast10Buttons_LinkClicked(FWBS.OMS.UI.Windows.ucNavCmdButtons LinkButton)
        {
            MenuEventArgs e = (MenuEventArgs)LinkButton.Tag;
            OnMenuActioned(e);
        }

        /// <summary>
        /// Console Manager
        /// </summary>
        /// <param name="LinkButton"></param>
        private void ucNavCommands2_LinkClicked(FWBS.OMS.UI.Windows.ucNavCmdButtons LinkButton)
        {
            if (LinkButton.Key == "NewConsole")
            {
                FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                param.Add("admnuParent", currentfolder);
                param.Add("MENUNAME", _menuname);

                FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(_addconsole, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, param);
                dtmenu = menu.GetTable();
                LoadMenu(currentfolder, true);
            }
            
            if (LinkButton.Key == "EditConsole" && clickfolder != null)
            {
                MenuEventArgs e = (MenuEventArgs)clickfolder.Tag;

                FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                param.Add("admnuID", e.MenuID);

                if (!e.IsFolder)
                {
                    object obj;
                    obj = FWBS.OMS.UI.Windows.Services.ShowOMSItem(Session.CurrentSession.DefaultSystemForm(FWBS.OMS.SystemForms.ConsoleItem), null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, false, param);
                    dtmenu = menu.GetTable();
                    LoadMenu(currentfolder, true);
                }
            }
            
            if (LinkButton.Key == "DeleteConsole" && clickfolder != null)
            {
                MenuEventArgs tag = (MenuEventArgs)clickfolder.Tag;
                if (!tag.System)
                {
                    if (MessageBox.Show(FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText("RUSUREDELETE"), FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        for (int i = listView1.SelectedItems.Count - 1; i > -1; i--)
                        {
                            MenuEventArgs e = (MenuEventArgs)listView1.SelectedItems[i].Tag;
                            listView1.Items.Remove(listView1.SelectedItems[i]);

                            dtmenu.DefaultView.Sort = "admnuID";
                            int r = dtmenu.DefaultView.Find(e.MenuID);
                            if (r > -1) dtmenu.DefaultView[r].Delete();
                        }
                        menu.UpdateData("SELECT * FROM dbAdminMenu");
                        LoadMenu(currentfolder, true);
                    }
                }
                else
                    MessageBox.Show(FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText("NODELSYSCONSOLE"), 
                        FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


		/// <summary>
		/// Removes the Favorite Item from the Command Bar
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mnuDyRemFav_Click(object sender, System.EventArgs e)
		{
			MenuEventArgs ee = (MenuEventArgs)mnuDynamicFav.SourceControl.Tag;
			FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(_menuname + "FAV");
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
		private void ucNavCmdButtons1_LinkClicked(object sender, System.EventArgs e)
		{
			FWBS.OMS.UI.Windows.Services.ShowAbout();
		}

		/// <summary>
		/// Loads the Settings for the Controls on the Bottom Right and set the State of the Parent Button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucHome_Load(object sender, System.EventArgs e)
		{
			if (Session.CurrentSession.IsLoggedIn)
			{
				labUser.Text = FWBS.OMS.Session.CurrentSession.CurrentUser.FullName;
                labDatabase.Text = FWBS.OMS.Session.CurrentSession.CurrentDatabase.DatabaseName.ToUpper();
                labServer.Text = FWBS.OMS.Session.CurrentSession.CurrentDatabase.Server;
                labCulture.Text = FWBS.OMS.Session.CurrentSession.DefaultCulture;
                LoadResources();
				_mainparent.OMSToolbars.GetButton("tbParent").Enabled=false;
				RefreshApplicationFavorites();
                if (OnLoadBrowser())
                {
                    this.WebBrowser = new FWBS.Common.UI.Windows.eWebBrowser();
                    // 
                    // WebBrowser
                    // 
                    this.WebBrowser.CaptionWidth = 0;
                    this.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.WebBrowser.DockPadding.All = 10;
                    this.WebBrowser.IsDirty = false;
                    this.WebBrowser.Location = new System.Drawing.Point(6, 10);
                    this.WebBrowser.Name = "WebBrowser";
                    this.WebBrowser.omsDesignMode = false;
                    this.WebBrowser.ReadOnly = false;
                    this.WebBrowser.Required = false;
                    this.WebBrowser.Size = new System.Drawing.Size(494, 395);
                    this.WebBrowser.TabIndex = 27;
                    this.WebBrowser.Value = null;
                    this.SearchPanel.Controls.Add(this.WebBrowser);
                    
                }
			}
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


		private void btnSearchGo_Click(object sender, System.EventArgs e)
		{
			if (OnGoClick() == false)
			{
			}
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
			if (SystemUpdateClick != null)
				SystemUpdateClick(this,e);
		}

		private void ucAddRemove_LinkClicked(object sender, System.EventArgs e)
		{
			if (AddRemoveAddinClick != null)
				AddRemoveAddinClick(this,e);
		}

		private void mnuRegApp_Click(object sender, System.EventArgs e)
		{
			DataTable dt = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.AddApplication),null,FWBS.OMS.EnquiryEngine.EnquiryMode.Add,new FWBS.Common.KeyValueCollection()) as DataTable;
			if (dt != null)
			{
				FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(_menuname + "APFAV");
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
			FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(_menuname + "APFAV");
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

		private void ucHome_Enter(object sender, System.EventArgs e)
		{
			if (this.Visible)
			{
			}
		}

	}

	#region MenuEvent
	/// <summary>
	/// The Menu Event Handler
	/// </summary>
	public delegate void MenuEventHandler(object sender, MenuEventArgs e);
	/// <summary>
	/// The Menu Event Handlers Arguments with the User :-)
	/// </summary>
	public class MenuEventArgs : EventArgs 
	{  
		/// <summary>
		/// The Return Key for Menu Button
		/// </summary>
		private string _returnkey;
		/// <summary>
		/// The Localized Menu Item Name
		/// </summary>
		private string _caption;
		/// <summary>
		/// The Code Lookup Code for the Menu ItemName
		/// </summary>
		private string _buttoncode;
		/// <summary>
		/// The Image Index number of AdminImages
		/// </summary>
		private int  _imageindex;
		/// <summary>
		/// Include in Favorites System for Consideration
		/// </summary>
		private bool _include;
		/// <summary>
		/// The Current Folder or New Folder to Option
		/// </summary>
		private bool _folder;
		/// <summary>
		/// The Unique Menu ID
		/// </summary>
		private int _menuid;
		/// <summary>
		/// The Roles that can see this Menu Item
		/// </summary>
		private string _roles;
        /// <summary>
        /// Whether or not the current item is a system object
        /// </summary>
        private bool _system;
        /// <summary>
        /// The nodeID and consoleID for the item i.e. 29;223
        /// </summary>
        private string _itemIDs;

        public MenuEventArgs(string ReturnKey, string ButtonCode, string ButtonCaption, int ImageIndex, bool IncFavorites, bool IsFolder, int menuID, string roles)
        {
            _returnkey = ReturnKey;
            _buttoncode = ButtonCode;
            _caption = ButtonCaption;
            _imageindex = ImageIndex;
            _include = IncFavorites;
            _folder = IsFolder;
            _menuid = menuID;
            _roles = roles;
            _system = false;
        }


        public MenuEventArgs(string ReturnKey, string ButtonCode, string ButtonCaption, int ImageIndex, bool IncFavorites, bool IsFolder, int menuID, string roles, bool System)
        {
            _returnkey = ReturnKey;
            _buttoncode = ButtonCode;
            _caption = ButtonCaption;
            _imageindex = ImageIndex;
            _include = IncFavorites;
            _folder = IsFolder;
            _menuid = menuID;
            _roles = roles;
            _system = System;
            _itemIDs = itemIDs;
        }


        public MenuEventArgs(string ReturnKey, string ButtonCode, string ButtonCaption, int ImageIndex, bool IncFavorites, bool IsFolder, int menuID, string roles, bool System, string itemIDs)
        {
            _returnkey = ReturnKey;
            _buttoncode = ButtonCode;
            _caption = ButtonCaption;
            _imageindex = ImageIndex;
            _include = IncFavorites;
            _folder = IsFolder;
            _menuid = menuID;
            _roles = roles;
            _system = System;
            _itemIDs = itemIDs;
        }


		public object ReturnKey
		{
			get
			{
				return _returnkey;
			}
		}

		public string ButtonCode
		{
			get
			{
				return _buttoncode;
			}
		}
		
		public string ButtonCaption
		{
			get
			{
				return _caption;
			}
		}

		public int ImageIndex
		{
			get
			{
				return _imageindex;
			}
		}

		public bool IncFavorites
		{
			get
			{
				return _include;
			}
		}

		public bool IsFolder
		{
			get
			{
				return _folder;
			}
		}

		public int MenuID
		{
			get
			{
				return _menuid;
			}
		}

		public string Roles
		{
			get
			{
				return _roles;
			}
		}

        public bool System
        {
            get
            {
                return _system;
            }
        }

        public string itemIDs
        {
            get
            {
                return _itemIDs;
            }
        }
	}
	#endregion

	public class OpenFolderEventArgs : CancelEventArgs
	{
		public string Name;
	}

	public class HandledEventArgs : EventArgs
	{
		public bool Handled = false;
	}
}
