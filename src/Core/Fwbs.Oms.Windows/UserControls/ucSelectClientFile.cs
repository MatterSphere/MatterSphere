using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.Windows
{

    /// <summary>
    /// A user control that is used to pick a client or client and file together.
    /// </summary>
    public class ucSelectClientFile : System.Windows.Forms.UserControl
	{
      
        #region Fields

		private System.ComponentModel.IContainer components;
		private FWBS.Common.UI.Windows.eXPFrame pnlFileInfo;
		private FWBS.Common.UI.Windows.eXPFrame pnlMain;
		private FWBS.Common.UI.Windows.eXPComboBox cboFileList;
		private System.Windows.Forms.Label lblfile;
		internal System.Windows.Forms.TextBox txtClientNo;
		private System.Windows.Forms.Label lblclient;
		private System.Windows.Forms.CheckBox chkAllFiles;
		private System.Windows.Forms.ErrorProvider epMessage;
		private FWBS.OMS.UI.RichTextBox lblclientInfo;
        private FWBS.OMS.UI.RichTextBox lblfileinfo;
		private System.Windows.Forms.ToolTip toolTip1;
		private bool _CloseAlreadySet = false;
		private SelectClientFileSearchType _searchtype = SelectClientFileSearchType.File;
		private Button btnCancel = null;
		private Button btnOK = null;
		private Button btnFind = null;
		private Button btnViewClient = null;
		private Button btnViewFile = null;
		private string g_cldesc = "";
		private string g_filedesc = "";

		/// <summary>
		/// Current fiel / client favourites list for the user.
		/// </summary>
		private Favourites _favourites;
		private bool _autosave = true;
		private bool _favvisible = true;
		private int _favheight = 32;
		private string _favname = "";
		private bool _showphases = true;
        private bool _selectFileVisible = true;

		/// <summary>
		/// Used to display a list of files depending on some citeria.
		/// This is used to filter out DEAD files.
		/// </summary>
		private DataView _fileview = null;

		/// <summary>
		/// Used to display a list of file phases depending on the selected file.
		/// </summary>
		private DataView _filephases = null;

		/// <summary>
		/// The current client object used within the control.
		/// </summary>
		private Client _client = null;
		private Int64 _clid = 0;
		private omsSplitter splitter1;
		private System.Windows.Forms.Panel pnlFavourites;
		private System.Windows.Forms.Button btnExpand;
		private FWBS.OMS.UI.Windows.ucPanelNav ucPanelNav1;
		private FWBS.OMS.UI.Windows.ucNavCommands ucNavCommands1;
		private FWBS.Common.UI.Windows.eXPFrame pnlButtonBack;
		private System.Windows.Forms.Panel pnlSplitter3;
		private System.Windows.Forms.Splitter pnlSplitter2;
		private FWBS.Common.UI.Windows.eXPFrame pnlFind;
		private System.Windows.Forms.Panel pnlSplitter1;
		private System.Windows.Forms.Panel pnlBack;
		private FWBS.OMS.UI.Windows.ucPanelNavTop ucPanelNav2;
		private FWBS.OMS.UI.Windows.ucNavCommands ucNavCommands2;
		private ClientFileState _cfstate = ClientFileState.None;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem mnuRemoveFav;
		private System.Windows.Forms.ComboBox eFavType;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.MenuItem mnuClearList;
        private System.Windows.Forms.Panel panel1;
		private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavAdd;
		private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavAddGlobal;
		private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavAddDept;
		private FWBS.OMS.UI.Windows.ucNavCmdButtons ucNavClear;
		private System.Windows.Forms.MenuItem mnuRename;
		private FWBS.Common.UI.Windows.eXPComboBox cboPhases;
        private System.Windows.Forms.Label lblPhases;
        private Timer timLoad;
        private Panel panel2;
        private LinkLabel lnkRefresh;
        private PictureBox picIcon; // 26/02/2010 - Added Timer so the Question to Show All Matters is done after the Form is Visible

		/// <summary>
		/// The current file object used within the control.
		/// </summary>
		private OMSFile _file = null;

		#endregion

		#region Events

		/// <summary>
		/// An event that could be raised when an alert has been set for a certain client / file.
		/// An alert could be in the form of the client not having paid yet.
		/// </summary>
		public event AlertEventHandler Alert = null;
		/// <summary>
		/// An event that is raised when a file cannot be found for the client.
		/// </summary>
		public event AlertEventHandler FileNotFound = null;

		/// <summary>
		/// An event that gets raised when the client / file user control
		/// changes state, so that it can tell the container form that
		/// the control state id ready to proceed etc...
		/// </summary>
		public event ClientFileStateChangedEventHandler StateChanged = null;

        public event EventHandler DetailsDisplayed;

        protected void OnDetailsDisplayed()
        {
            if (DetailsDisplayed != null)
                DetailsDisplayed(this, EventArgs.Empty);
        }

		#endregion

		#region Constructors & Destructors
		
		/// <summary>
		/// Initialises the user control by building the favourites list if it set to be seen.
		/// </summary>
		public ucSelectClientFile()
		{
			InitializeComponent();
            SetImages();
            SetContextMenuOptions();
            
            ucPanelNav2.Theme = ExtColorTheme.Auto;
            ucPanelNav1.Theme = ExtColorTheme.Auto;
		}


        private void SetContextMenuOptions()
        {
            Res resources = Session.CurrentSession.Resources;
            if (resources != null)
            {
                this.mnuRename.Text = resources.GetResource("SCF_RENAME", "Rename", "").Text;
                this.mnuRemoveFav.Text = GetRemoveItemLabel();
                this.mnuClearList.Text = resources.GetResource("SCF_CLEARLIST", "Clear List", "").Text;
            }
        }

        /// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
                if (_favourites != null)
                    _favourites.Dispose();
            }
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucSelectClientFile));
            this.pnlFileInfo = new FWBS.Common.UI.Windows.eXPFrame();
            this.lblfileinfo = new FWBS.OMS.UI.RichTextBox();
            this.pnlMain = new FWBS.Common.UI.Windows.eXPFrame();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lnkRefresh = new System.Windows.Forms.LinkLabel();
            this.picIcon = new System.Windows.Forms.PictureBox();
            this.lblclientInfo = new FWBS.OMS.UI.RichTextBox();
            this.cboFileList = new FWBS.Common.UI.Windows.eXPComboBox();
            this.lblfile = new System.Windows.Forms.Label();
            this.txtClientNo = new System.Windows.Forms.TextBox();
            this.lblclient = new System.Windows.Forms.Label();
            this.chkAllFiles = new System.Windows.Forms.CheckBox();
            this.epMessage = new System.Windows.Forms.ErrorProvider(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlFavourites = new System.Windows.Forms.Panel();
            this.ucPanelNav1 = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavCommands1 = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.ucPanelNav2 = new FWBS.OMS.UI.Windows.ucPanelNavTop();
            this.ucNavCommands2 = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.eFavType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ucNavAdd = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucNavAddGlobal = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucNavAddDept = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucNavClear = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.pnlButtonBack = new FWBS.Common.UI.Windows.eXPFrame();
            this.btnExpand = new System.Windows.Forms.Button();
            this.pnlSplitter3 = new System.Windows.Forms.Panel();
            this.pnlSplitter2 = new System.Windows.Forms.Splitter();
            this.pnlFind = new FWBS.Common.UI.Windows.eXPFrame();
            this.cboPhases = new FWBS.Common.UI.Windows.eXPComboBox();
            this.lblPhases = new System.Windows.Forms.Label();
            this.pnlSplitter1 = new System.Windows.Forms.Panel();
            this.pnlBack = new System.Windows.Forms.Panel();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.mnuRename = new System.Windows.Forms.MenuItem();
            this.mnuRemoveFav = new System.Windows.Forms.MenuItem();
            this.mnuClearList = new System.Windows.Forms.MenuItem();
            this.timLoad = new System.Windows.Forms.Timer(this.components);
            this.splitter1 = new FWBS.OMS.UI.Windows.omsSplitter();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlFileInfo.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epMessage)).BeginInit();
            this.pnlFavourites.SuspendLayout();
            this.ucPanelNav1.SuspendLayout();
            this.ucPanelNav2.SuspendLayout();
            this.ucNavCommands2.SuspendLayout();
            this.pnlButtonBack.SuspendLayout();
            this.pnlFind.SuspendLayout();
            this.pnlBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFileInfo
            // 
            this.pnlFileInfo.Controls.Add(this.lblfileinfo);
            this.pnlFileInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFileInfo.Location = new System.Drawing.Point(5, 291);
            this.resourceLookup1.SetLookup(this.pnlFileInfo, new FWBS.OMS.UI.Windows.ResourceLookupItem("pnlFileInfo", "%FILE% Details", ""));
            this.pnlFileInfo.Name = "pnlFileInfo";
            this.pnlFileInfo.Padding = new System.Windows.Forms.Padding(12, 20, 12, 5);
            this.pnlFileInfo.Size = new System.Drawing.Size(257, 111);
            this.pnlFileInfo.TabIndex = 2;
            this.pnlFileInfo.Text = "%FILE% Details";
            // 
            // lblfileinfo
            // 
            this.lblfileinfo.BackColor = System.Drawing.Color.White;
            this.lblfileinfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblfileinfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblfileinfo.Location = new System.Drawing.Point(12, 20);
            this.lblfileinfo.Name = "lblfileinfo";
            this.lblfileinfo.ReadOnly = true;
            this.lblfileinfo.Size = new System.Drawing.Size(233, 86);
            this.lblfileinfo.TabIndex = 2;
            this.lblfileinfo.TabStop = false;
            this.lblfileinfo.Text = "";
            this.lblfileinfo.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.lbl_LinkClicked);
            this.lblfileinfo.VisibleChanged += new System.EventHandler(this.DisplayFileData);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Controls.Add(this.lblclientInfo);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(5, 130);
            this.resourceLookup1.SetLookup(this.pnlMain, new FWBS.OMS.UI.Windows.ResourceLookupItem("pnlMain", "%CLIENT% Details", ""));
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(12, 20, 12, 5);
            this.pnlMain.Size = new System.Drawing.Size(257, 158);
            this.pnlMain.TabIndex = 1;
            this.pnlMain.Text = "%CLIENT% Details";
            // 
            // panel2
            // 
            this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel2.Controls.Add(this.lnkRefresh);
            this.panel2.Controls.Add(this.picIcon);
            this.panel2.Location = new System.Drawing.Point(160, 48);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(64, 72);
            this.panel2.TabIndex = 26;
            // 
            // lnkRefresh
            // 
            this.lnkRefresh.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkRefresh.Location = new System.Drawing.Point(2, 52);
            this.resourceLookup1.SetLookup(this.lnkRefresh, new FWBS.OMS.UI.Windows.ResourceLookupItem("lnkRefresh", "Refresh", ""));
            this.lnkRefresh.Name = "lnkRefresh";
            this.lnkRefresh.Size = new System.Drawing.Size(60, 19);
            this.lnkRefresh.TabIndex = 26;
            this.lnkRefresh.TabStop = true;
            this.lnkRefresh.Text = "Refresh";
            this.lnkRefresh.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkRefresh.Visible = false;
            this.lnkRefresh.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkRefresh.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRefresh_LinkClicked);
            // 
            // picIcon
            // 
            this.picIcon.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.picIcon.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picIcon.Location = new System.Drawing.Point(16, 12);
            this.picIcon.Name = "picIcon";
            this.picIcon.Size = new System.Drawing.Size(32, 32);
            this.picIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picIcon.TabIndex = 27;
            this.picIcon.TabStop = false;
            // 
            // lblclientInfo
            // 
            this.lblclientInfo.BackColor = System.Drawing.Color.White;
            this.lblclientInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lblclientInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblclientInfo.Location = new System.Drawing.Point(12, 20);
            this.lblclientInfo.Name = "lblclientInfo";
            this.lblclientInfo.ReadOnly = true;
            this.lblclientInfo.Size = new System.Drawing.Size(233, 133);
            this.lblclientInfo.TabIndex = 1;
            this.lblclientInfo.TabStop = false;
            this.lblclientInfo.Text = "";
            this.lblclientInfo.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.lbl_LinkClicked);
            this.lblclientInfo.VisibleChanged += new System.EventHandler(this.lblclientInfo_VisibleChanged);
            // 
            // cboFileList
            // 
            this.cboFileList.ActiveSearchEnabled = true;
            this.cboFileList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFileList.CaptionWidth = 0;
            this.cboFileList.IsDirty = true;
            this.cboFileList.Location = new System.Drawing.Point(96, 37);
            this.cboFileList.MaxLength = 0;
            this.cboFileList.Name = "cboFileList";
            this.cboFileList.Size = new System.Drawing.Size(147, 23);
            this.cboFileList.TabIndex = 1;
            this.cboFileList.ActiveChanged += new System.EventHandler(this.cboFileList_SelectedIndexChanged);
            // 
            // lblfile
            // 
            this.lblfile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblfile.Location = new System.Drawing.Point(7, 37);
            this.resourceLookup1.SetLookup(this.lblfile, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblfile", "%FILE% No :", ""));
            this.lblfile.Name = "lblfile";
            this.lblfile.Size = new System.Drawing.Size(90, 21);
            this.lblfile.TabIndex = 23;
            this.lblfile.Text = "%FILE% No :";
            this.lblfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtClientNo
            // 
            this.txtClientNo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtClientNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtClientNo.Location = new System.Drawing.Point(96, 8);
            this.txtClientNo.Name = "txtClientNo";
            this.txtClientNo.Size = new System.Drawing.Size(152, 23);
            this.txtClientNo.TabIndex = 0;
            this.txtClientNo.Enter += new System.EventHandler(this.txtClientNo_Enter);
            this.txtClientNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtClientNo_KeyDown);
            this.txtClientNo.Leave += new System.EventHandler(this.txtClientNo_Leave);
            this.txtClientNo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtClientNo_MouseDown);
            // 
            // lblclient
            // 
            this.lblclient.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblclient.Location = new System.Drawing.Point(7, 8);
            this.resourceLookup1.SetLookup(this.lblclient, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblclient", "%CLIENT% No :", ""));
            this.lblclient.Name = "lblclient";
            this.lblclient.Size = new System.Drawing.Size(90, 20);
            this.lblclient.TabIndex = 22;
            this.lblclient.Text = "%CLIENT% No :";
            this.lblclient.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkAllFiles
            // 
            this.chkAllFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllFiles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAllFiles.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAllFiles.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkAllFiles.Location = new System.Drawing.Point(47, 66);
            this.resourceLookup1.SetLookup(this.chkAllFiles, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkAllFiles", "Show all %FILES%", ""));
            this.chkAllFiles.Name = "chkAllFiles";
            this.chkAllFiles.Size = new System.Drawing.Size(196, 21);
            this.chkAllFiles.TabIndex = 2;
            this.chkAllFiles.Text = "Show all %FILES%";
            this.chkAllFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAllFiles.Click += new System.EventHandler(this.chkAllFiles_Click);
            // 
            // epMessage
            // 
            this.epMessage.ContainerControl = this;
            this.epMessage.Icon = ((System.Drawing.Icon)(resources.GetObject("epMessage.Icon")));
            // 
            // pnlFavourites
            // 
            this.pnlFavourites.AutoScroll = true;
            this.pnlFavourites.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFavourites.Controls.Add(this.ucPanelNav1);
            this.pnlFavourites.Controls.Add(this.ucPanelNav2);
            this.pnlFavourites.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFavourites.Location = new System.Drawing.Point(0, 0);
            this.pnlFavourites.Name = "pnlFavourites";
            this.pnlFavourites.Padding = new System.Windows.Forms.Padding(8);
            this.pnlFavourites.Size = new System.Drawing.Size(207, 446);
            this.pnlFavourites.TabIndex = 41;
            // 
            // ucPanelNav1
            // 
            this.ucPanelNav1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNav1.Brightness = 90;
            this.ucPanelNav1.Controls.Add(this.ucNavCommands1);
            this.ucPanelNav1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNav1.ExpandedHeight = 31;
            this.ucPanelNav1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNav1.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanelNav1.Location = new System.Drawing.Point(8, 173);
            this.ucPanelNav1.Name = "ucPanelNav1";
            this.ucPanelNav1.Size = new System.Drawing.Size(189, 31);
            this.ucPanelNav1.TabIndex = 18;
            this.ucPanelNav1.TabStop = false;
            this.ucPanelNav1.Text = "Favourites";
            // 
            // ucNavCommands1
            // 
            this.ucNavCommands1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavCommands1.Location = new System.Drawing.Point(0, 24);
            this.ucNavCommands1.Name = "ucNavCommands1";
            this.ucNavCommands1.Padding = new System.Windows.Forms.Padding(9);
            this.ucNavCommands1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavCommands1.Size = new System.Drawing.Size(189, 0);
            this.ucNavCommands1.TabIndex = 15;
            this.ucNavCommands1.TabStop = false;
            this.ucNavCommands1.LinkClicked += new FWBS.OMS.UI.Windows.LinkEventHandler(this.ucNavCommands1_LinkClicked);
            // 
            // ucPanelNav2
            // 
            this.ucPanelNav2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNav2.Brightness = 90;
            this.ucPanelNav2.Controls.Add(this.ucNavCommands2);
            this.ucPanelNav2.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNav2.ExpandedHeight = 165;
            this.ucPanelNav2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNav2.HeaderBrightness = -10;
            this.ucPanelNav2.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.ucPanelNav2.Image = null;
            this.ucPanelNav2.Location = new System.Drawing.Point(8, 8);
            this.resourceLookup1.SetLookup(this.ucPanelNav2, new FWBS.OMS.UI.Windows.ResourceLookupItem("ucPanelNav2", "Organise", ""));
            this.ucPanelNav2.Name = "ucPanelNav2";
            this.ucPanelNav2.Size = new System.Drawing.Size(189, 165);
            this.ucPanelNav2.TabIndex = 19;
            this.ucPanelNav2.Text = "Organise";
            // 
            // ucNavCommands2
            // 
            this.ucNavCommands2.Controls.Add(this.eFavType);
            this.ucNavCommands2.Controls.Add(this.panel1);
            this.ucNavCommands2.Controls.Add(this.ucNavAdd);
            this.ucNavCommands2.Controls.Add(this.ucNavAddGlobal);
            this.ucNavCommands2.Controls.Add(this.ucNavAddDept);
            this.ucNavCommands2.Controls.Add(this.ucNavClear);
            this.ucNavCommands2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavCommands2.Location = new System.Drawing.Point(0, 32);
            this.ucNavCommands2.Name = "ucNavCommands2";
            this.ucNavCommands2.Padding = new System.Windows.Forms.Padding(6);
            this.ucNavCommands2.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavCommands2.Size = new System.Drawing.Size(189, 126);
            this.ucNavCommands2.TabIndex = 15;
            this.ucNavCommands2.TabStop = false;
            this.ucNavCommands2.LinkClicked += new FWBS.OMS.UI.Windows.LinkEventHandler(this.ucNavCommands2_LinkClicked);
            // 
            // eFavType
            // 
            this.eFavType.DisplayMember = "cddesc";
            this.eFavType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.eFavType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eFavType.Location = new System.Drawing.Point(6, 4);
            this.eFavType.Name = "eFavType";
            this.eFavType.Size = new System.Drawing.Size(177, 23);
            this.eFavType.TabIndex = 2;
            this.eFavType.ValueMember = "cdcode";
            this.eFavType.SelectedIndexChanged += new System.EventHandler(this.eXPComboBox2_Changed);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(6, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(177, 5);
            this.panel1.TabIndex = 5;
            // 
            // ucNavAdd
            // 
            this.ucNavAdd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavAdd.ImageIndex = 20;
            this.ucNavAdd.Key = "Add";
            this.ucNavAdd.Location = new System.Drawing.Point(6, 32);
            this.resourceLookup1.SetLookup(this.ucNavAdd, new FWBS.OMS.UI.Windows.ResourceLookupItem("ADDTOFAV", "Add To Favourites", ""));
            this.ucNavAdd.Name = "ucNavAdd";
            this.ucNavAdd.Size = new System.Drawing.Size(177, 22);
            this.ucNavAdd.TabIndex = 6;
            this.ucNavAdd.Text = "Add To Favourites";
            // 
            // ucNavAddGlobal
            // 
            this.ucNavAddGlobal.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavAddGlobal.ImageIndex = 20;
            this.ucNavAddGlobal.Key = "AddGlobal";
            this.ucNavAddGlobal.Location = new System.Drawing.Point(6, 54);
            this.resourceLookup1.SetLookup(this.ucNavAddGlobal, new FWBS.OMS.UI.Windows.ResourceLookupItem("ADDFAVGLOBAL", "Add To Global Favourites", ""));
            this.ucNavAddGlobal.Name = "ucNavAddGlobal";
            this.ucNavAddGlobal.Size = new System.Drawing.Size(177, 22);
            this.ucNavAddGlobal.TabIndex = 6;
            this.ucNavAddGlobal.Text = "Add To Global Favourites";
            // 
            // ucNavAddDept
            // 
            this.ucNavAddDept.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavAddDept.ImageIndex = 20;
            this.ucNavAddDept.Key = "AddDept";
            this.ucNavAddDept.Location = new System.Drawing.Point(6, 76);
            this.resourceLookup1.SetLookup(this.ucNavAddDept, new FWBS.OMS.UI.Windows.ResourceLookupItem("ADDFAVDEPT", "Add To Dept Favourites", ""));
            this.ucNavAddDept.Name = "ucNavAddDept";
            this.ucNavAddDept.Size = new System.Drawing.Size(177, 22);
            this.ucNavAddDept.TabIndex = 6;
            this.ucNavAddDept.Text = "Add To Dept Favourites";
            // 
            // ucNavClear
            // 
            this.ucNavClear.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavClear.ImageIndex = 6;
            this.ucNavClear.Key = "Clear";
            this.ucNavClear.Location = new System.Drawing.Point(6, 98);
            this.resourceLookup1.SetLookup(this.ucNavClear, new FWBS.OMS.UI.Windows.ResourceLookupItem("ucNavClear", "Clear List", ""));
            this.ucNavClear.Name = "ucNavClear";
            this.ucNavClear.Size = new System.Drawing.Size(177, 22);
            this.ucNavClear.TabIndex = 7;
            this.ucNavClear.Text = "Clear List";
            // 
            // pnlButtonBack
            // 
            this.pnlButtonBack.Controls.Add(this.btnExpand);
            this.pnlButtonBack.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtonBack.Location = new System.Drawing.Point(5, 405);
            this.pnlButtonBack.Name = "pnlButtonBack";
            this.pnlButtonBack.Size = new System.Drawing.Size(257, 34);
            this.pnlButtonBack.TabIndex = 3;
            this.pnlButtonBack.Visible = false;
            // 
            // btnExpand
            // 
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExpand.Location = new System.Drawing.Point(6, 6);
            this.resourceLookup1.SetLookup(this.btnExpand, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnExpand", "Shrink >>", ""));
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(75, 23);
            this.btnExpand.TabIndex = 0;
            this.btnExpand.Text = "Shrink >>";
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // pnlSplitter3
            // 
            this.pnlSplitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSplitter3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlSplitter3.Location = new System.Drawing.Point(5, 402);
            this.pnlSplitter3.Name = "pnlSplitter3";
            this.pnlSplitter3.Size = new System.Drawing.Size(257, 3);
            this.pnlSplitter3.TabIndex = 44;
            // 
            // pnlSplitter2
            // 
            this.pnlSplitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSplitter2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlSplitter2.Location = new System.Drawing.Point(5, 288);
            this.pnlSplitter2.Name = "pnlSplitter2";
            this.pnlSplitter2.Size = new System.Drawing.Size(257, 3);
            this.pnlSplitter2.TabIndex = 45;
            this.pnlSplitter2.TabStop = false;
            this.pnlSplitter2.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter2_SplitterMoved);
            // 
            // pnlFind
            // 
            this.pnlFind.Controls.Add(this.cboPhases);
            this.pnlFind.Controls.Add(this.txtClientNo);
            this.pnlFind.Controls.Add(this.lblclient);
            this.pnlFind.Controls.Add(this.lblPhases);
            this.pnlFind.Controls.Add(this.chkAllFiles);
            this.pnlFind.Controls.Add(this.cboFileList);
            this.pnlFind.Controls.Add(this.lblfile);
            this.pnlFind.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFind.Location = new System.Drawing.Point(5, 5);
            this.pnlFind.Name = "pnlFind";
            this.pnlFind.Size = new System.Drawing.Size(257, 125);
            this.pnlFind.TabIndex = 0;
            // 
            // cboPhases
            // 
            this.cboPhases.ActiveSearchEnabled = true;
            this.cboPhases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPhases.CaptionWidth = 0;
            this.cboPhases.IsDirty = false;
            this.cboPhases.Location = new System.Drawing.Point(96, 91);
            this.cboPhases.MaxLength = 0;
            this.cboPhases.Name = "cboPhases";
            this.cboPhases.Size = new System.Drawing.Size(147, 23);
            this.cboPhases.TabIndex = 2;
            // 
            // lblPhases
            // 
            this.lblPhases.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPhases.Location = new System.Drawing.Point(7, 91);
            this.resourceLookup1.SetLookup(this.lblPhases, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblPhases", "Phases :", ""));
            this.lblPhases.Name = "lblPhases";
            this.lblPhases.Size = new System.Drawing.Size(90, 21);
            this.lblPhases.TabIndex = 25;
            this.lblPhases.Text = "Phases :";
            this.lblPhases.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSplitter1
            // 
            this.pnlSplitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSplitter1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pnlSplitter1.Location = new System.Drawing.Point(5, 130);
            this.pnlSplitter1.Name = "pnlSplitter1";
            this.pnlSplitter1.Size = new System.Drawing.Size(257, 3);
            this.pnlSplitter1.TabIndex = 47;
            // 
            // pnlBack
            // 
            this.pnlBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBack.Controls.Add(this.pnlSplitter1);
            this.pnlBack.Controls.Add(this.pnlMain);
            this.pnlBack.Controls.Add(this.pnlFind);
            this.pnlBack.Controls.Add(this.pnlSplitter2);
            this.pnlBack.Controls.Add(this.pnlFileInfo);
            this.pnlBack.Controls.Add(this.pnlSplitter3);
            this.pnlBack.Controls.Add(this.pnlButtonBack);
            this.pnlBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBack.Location = new System.Drawing.Point(211, 0);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Padding = new System.Windows.Forms.Padding(5);
            this.pnlBack.Size = new System.Drawing.Size(269, 446);
            this.pnlBack.TabIndex = 49;
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuRename,
            this.mnuRemoveFav,
            this.mnuClearList});
            // 
            // mnuRename
            // 
            this.mnuRename.Index = 0;
            this.mnuRename.Text = "";
            this.mnuRename.Click += new System.EventHandler(this.mnuRename_Click);
            // 
            // mnuRemoveFav
            // 
            this.mnuRemoveFav.Index = 1;
            this.mnuRemoveFav.Text = "";
            this.mnuRemoveFav.Click += new System.EventHandler(this.mnuRemoveFav_Click);
            // 
            // mnuClearList
            // 
            this.mnuClearList.Index = 2;
            this.mnuClearList.Text = "";
            this.mnuClearList.Click += new System.EventHandler(this.mnuClearList_Click);
            // 
            // timLoad
            // 
            this.timLoad.Interval = 500;
            this.timLoad.Tick += new System.EventHandler(this.timLoad_Tick);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(207, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 446);
            this.splitter1.TabIndex = 42;
            this.splitter1.TabStop = false;
            this.splitter1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitter1_SplitterMoved);
            // 
            // ucSelectClientFile
            // 
            this.Controls.Add(this.pnlBack);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlFavourites);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "ucSelectClientFile";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.Size = new System.Drawing.Size(485, 446);
            this.Load += new System.EventHandler(this.ucSelectClientFile_Load);
            this.Enter += new System.EventHandler(this.ucSelectClientFile_Enter);
            this.ParentChanged += new System.EventHandler(this.ucSelectClientFile_ParentChanged);
            this.pnlFileInfo.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epMessage)).EndInit();
            this.pnlFavourites.ResumeLayout(false);
            this.ucPanelNav1.ResumeLayout(false);
            this.ucPanelNav2.ResumeLayout(false);
            this.ucNavCommands2.ResumeLayout(false);
            this.pnlButtonBack.ResumeLayout(false);
            this.pnlFind.ResumeLayout(false);
            this.pnlFind.PerformLayout();
            this.pnlBack.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion


		#endregion

		#region Private Methods

        private void SetImages()
        {
            ucPanelNav2.Image = null;
            this.ucNavCommands1.Resources = ImageListSelector.GetOMSImageList();
            this.ucNavCommands2.Resources = ImageListSelector.GetOMSImageList();
        }

       
        private bool firstTimePasswordCancel = true;

		/// <summary>
		/// Grabs the file information of the file that has just been picked from the files list.
		/// </summary>
		/// <param name="sender">File list combo box control.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cboFileList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (cboFileList.SelectedValue != null)
            {
                try
                {
                    if (Convert.ToInt64(_fileview[cboFileList.SelectedIndex]["fileid"]) > -1)
                    {
                        long fileID;
                        //GM & CM - 11/03/14 - First time password check required to eliminate multiple password requests (in some scenarios) 
                        if (firstTimePasswordCancel == false)
                        {
                            fileID = Convert.ToInt64(_fileview[cboFileList.SelectedIndex]["fileid"]);
                            OMSFile f = FWBS.OMS.OMSFile.GetFile(Convert.ToInt64(_fileview[cboFileList.SelectedIndex]["fileid"]));
                            firstTimePasswordCancel = false;
                        }
                        //CM - 11/03/14 - Update tag to ensure matter information is updated on the Select Client/File Screen 
                        cboFileList.Tag = cboFileList.SelectedValue;
                    }
                }
                catch (OMSException ex)
                {
                    //GM & CM - 11/03/14 - Reset matter information on the Select Client/File Screen
                    lblfileinfo.Text = "";
                 
                    IgnoreOnStateChange = true;
                    this.ClientFileState = FWBS.OMS.UI.Windows.ClientFileState.None;
                    IgnoreOnStateChange = false;
                    cboFileList.Tag = null;
                          
                    if(firstTimePasswordCancel == false)
                        ErrorBox.Show(ParentForm, ex);
                    firstTimePasswordCancel = false;

                    return;
                }

                firstTimePasswordCancel = false;
            }
            try
            {
                if (renderFileInfo)
                {
                    if (cboFileList.SelectedValue == DBNull.Value)
                        return;
                    if (cboFileList.Tag == null)
                        return;

                    DisplayFileData(sender, e);
                }
            }
            catch (OMSException ex)
            {
                if (ex.HelpID == HelpIndexes.PasswordRequestCancelled)
                {
                    throw new Security.SecurityException(HelpIndexes.PasswordRequestCancelled);
                }
            }
		}

		private void DisplayFileData(object sender, EventArgs e)
		{
			if (lblfileinfo.Visible)
			{
				if (cboFileList.DataSource == null)
				{
					return;
				}
                
                g_filedesc = "";


				if (Convert.ToString(cboFileList.SelectedValue) != "")
				{
                    try
                    {
                        if (_client != null && this.Visible)
                        {
                            g_filedesc = _client.FileQuickDescription(_fileview[cboFileList.SelectedIndex]);

                            lblfileinfo.Tag = _fileview[cboFileList.SelectedIndex]["fileid"];

                            if (_showphases)
                            {
                                _filephases = _client.GetFilePhases(Convert.ToInt64(lblfileinfo.Tag));

                                //Display the phases combo.
                                lblPhases.Visible = (_filephases.Count > 1);
                                cboPhases.Visible = lblPhases.Visible;

                                if (lblPhases.Visible)
                                {
                                    pnlFind.Height = LogicalToDeviceUnits(120);
                                    cboPhases.BeginUpdate();
                                    cboPhases.DataSource = _filephases;
                                    cboPhases.DisplayMember = "phDesc";
                                    cboPhases.ValueMember = "phID";
                                    try
                                    {
                                        cboPhases.SelectedValue = _fileview[cboFileList.SelectedIndex]["phid"];
                                    }
                                    catch { }
                                    cboPhases.EndUpdate();
                                }
                                else
                                {
                                    pnlFind.Height = LogicalToDeviceUnits(85);
                                    cboPhases.DataSource = null;
                                    if (_filephases != null)
                                        _filephases = null;
                                }
                            }

                            AlertEventArgs args = new AlertEventArgs(_client.FileAlerts(_fileview[cboFileList.SelectedIndex]));
                            OnAlert(args);
                        }
                    }
                    catch (OMSException ex)
                    {
                        if (ex.HelpID == HelpIndexes.PasswordRequestCancelled)
                        {
                            throw new Security.SecurityException(HelpIndexes.PasswordRequestCancelled);
                        }
                        else
                        {
                            try
                            {
                                AlertEventArgs args = new AlertEventArgs(_client.FileAlerts(_fileview[cboFileList.SelectedIndex]));
                                OnAlert(args);
                            }
                            catch { }
                        }
                    }
                    finally { }
				}
			}

			try
			{
				lblfileinfo.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(g_filedesc);
                
                Global.SetRichTextBoxRightToLeft(lblfileinfo);
                lblfileinfo.BackColor = lblclientInfo.BackColor;
            }
			catch
			{
				lblfileinfo.Text = g_filedesc;
			}
			
		}

		/// <summary>
		/// Executes the mailto to link in the rich textbox control. 
		/// The shell therefore decides the best way to use this functionality by using the default email client.
		/// </summary>
		/// <param name="sender">Calling control.</param>
		/// <param name="e">Link event arguments.</param>
		private void lbl_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.LinkText);
		}

		/// <summary>
		/// Raised when the all files checkbox is clicked.  Another area in code where the form
		/// is updated with the clients information.
		/// </summary>
		/// <param name="sender">All files checkbox control.</param>
		/// <param name="e">Empty event arguments.</param>
		private void chkAllFiles_Click(object sender, System.EventArgs e)
		{
			if (_client != null)
			{
				_file = null;
                lastclientid = -1;
                try { updateformdetails(); } catch { }
			}
		}

		/// <summary>
		/// Raises when the client number edit box has bee left.  The client of the specified number
		/// will try and be found, if not an error is shown.
		/// </summary>
		/// <param name="sender">Calling text edit control.</param>
		/// <param name="e">Empty event arguments.</param>
		private void txtClientNo_Leave(object sender, System.EventArgs e)
		{
            firstTimePasswordCancel = true;

			if (txtClientNo.Text != "")
				timDraw_Tick(sender,e);
		}

		/// <summary>
		/// Populates the favourites list from the favourites object.
		/// </summary>
		private void InitFavourites()
		{
			if (Session.CurrentSession.IsLoggedIn == false) return;
            bool hasrole = ((Session.CurrentSession.CurrentUser.IsInRoles(new string[] { User.ROLE_PARTNER, User.ROLE_ADMIN, "MANAGER" })));
            ucNavAddGlobal.Enabled = hasrole;
            ucNavAddDept.Enabled = hasrole;
            

            switch (GetCurrentFavTypeCode())
            {
                case "GLOBAL":
                case "DEPT":
                    {
                        ucNavClear.Enabled = hasrole;
                        mnuClearList.Enabled = hasrole;
                        break;
                    }
                default:
                    {
                        ucNavClear.Enabled = true;
                        mnuClearList.Enabled = true;
                        break;
                    }

            }
         

            if (_favname != "")
			{
				string code = SetFavourites();

				try
				{
					ucPanelNav1.Text = eFavType.Text;
                    Global.RemoveAndDisposeControls(ucNavCommands1);
                    txtClientNo.AutoCompleteCustomSource.Clear();

                    DataView dv = _favourites.GetDataView();

                    if(GetCurrentFavTypeCode() == "LAST10")
                        dv.Sort = "usrFavObjParam4 asc";
                    else
                        dv.Sort = "usrFavObjParam1 asc";
                    DataTable dt = dv.ToTable();

					for (int i = 0; i < dt.Rows.Count; i++)
					{
                        string key = "";
                        if (cboFileList.Visible)
                            key = Convert.ToString(dt.Rows[i]["usrFavObjParam2"]) + " " + Convert.ToString(dt.Rows[i]["usrFavObjParam3"]);
                        else
                            key = Convert.ToString(dt.Rows[i]["usrFavObjParam2"]);
                        ucNavCmdButtons navButton = new ucNavCmdButtons();
                        txtClientNo.AutoCompleteCustomSource.Add(key);
                        navButton.Text = Convert.ToString(dt.Rows[i]["usrFavObjParam1"]);
                        navButton.ImageIndex = Convert.ToInt32(dt.Rows[i]["usrFavGlyph"]);

                        navButton.ImageList = ucNavCommands1.ImageList;
                        navButton.Height = _favheight;
                        navButton.Key = key;
                        navButton.Tag = Convert.ToString(dt.Rows[i]["FavID"]);
                        switch (GetCurrentFavTypeCode())
                        {
                            case "GLOBAL":
                                if (hasrole)
                                    navButton.ContextMenu = contextMenu1;
                                else
                                    navButton.ContextMenu = null;
                                break;
                            case "DEPT":
                                if (hasrole)
                                    navButton.ContextMenu = contextMenu1;
                                else
                                    navButton.ContextMenu = null;
                                break;
                            default:
                                navButton.ContextMenu = contextMenu1;
                                break;
                        }
                        mnuRemoveFav.Visible = true;
                        navButton.TabIndex = i;
                        toolTip1.SetToolTip(navButton, navButton.Text);
                        ucNavCommands1.Controls.Add(navButton, true);
					}
                    dv.Dispose();
                    dt.Dispose();
                }
				catch{}
				ucNavCommands1.Refresh();

			}
		
			this.ucNavCommands2.Refresh(false);

		}

		private bool renderFileInfo = true;

        private long lastclientid = -1;

        private void SetupFileData(string filter, string sort, bool allFiles)
        {
            _fileview = _client.GetFilesDataView(filter, "", allFiles);
            string error = "";
            if (!_client.HasAllLiveFiles)
                error = FWBS.OMS.Session.CurrentSession.Resources.GetMessage("SCRCLIFINOTAF", "All %FILES% have not been returned. Please select 'Show All %FILES%' or use Search", "").Text;
            epMessage.SetError(cboFileList, error);
        }

		/// <summary>
		/// Updates the form with details on the client / file for the relevant labels and text boxes.
		/// Alerts and messages are shown at this point as the client object gets scanned.
		/// </summary>
		private void updateformdetails()
		{
            try
            {
                if (!Session.CurrentSession.IsLoggedIn)
                    return;

                ResourceItem res_fnf = Session.CurrentSession.Resources.GetResource("OMSNFSF", "There are no %FILES% available for %CLIENT% No : %1%", "", txtClientNo.Text);

                if (_client != null)
                {
                    Alert[] alerts = _client.Alerts;
                    _clid = _client.ClientID;
                    if (alerts.Length > 0)
                    {
                        if (Alert != null)
                        {
                            OnAlert(new AlertEventArgs(alerts));
                        }
                        else
                        {
                            FWBS.OMS.UI.Windows.MessageBox.Show(alerts.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                    {
                        if (Alert != null)
                        {
                            OnAlert(new AlertEventArgs(new Alert[0]));
                        }
                    }

                    txtClientNo.Text = _client.ClientNo;
                    lblclientInfo_VisibleChanged(this, null);
                    chkAllFiles.Enabled = true;
                    picIcon.Visible = true;
                    lnkRefresh.Visible = true;
                    try
                    {
                        var type = _client.GetOMSType();
                        if (type != null)
                        {
                            var image = Images.GetEntityIcon(type.Glyph, true);
                            if (image != null)
                                picIcon.Image = image.ToBitmap();
                        }
                    }
                    catch
                    {
                    }
                    toolTip1.SetToolTip(picIcon, _client.ClientTypeDescription);
                    epMessage.SetError(txtClientNo, "");

                    // if matter combo is visible then check Files list
                    if (pnlFileInfo.Visible)
                    {
                        if (lastclientid != _client.ID)
                        {
                            if (chkAllFiles.Checked == true && lastclientid != -1)
                                chkAllFiles.Checked = false;

                            string filter = string.Empty;

                            if (!chkAllFiles.Checked)
                                filter = "filestatus Like 'LIVE%'";

                            SetupFileData(filter, "created", chkAllFiles.Checked);
                            lastclientid = _client.ID;

                            try
                            {
                                renderFileInfo = false;
                                BindFilesCombo();
                                if (cboFileList.Tag == null)
                                    cboFileList.Tag = cboFileList.SelectedValue;
                            }
                            finally
                            {
                                renderFileInfo = true;
                            }
                        }

                        // File No is specified within the clientno and stored in the tag so try
                        // and select the additional File No from the Combo
                        if (cboFileList.Tag != null)
                        {
                            cboFileList.SelectedValue = cboFileList.Tag;
                            if (cboFileList.SelectedIndex == -1)
                            {
                                if (chkAllFiles.Checked == false)
                                {
                                    SetupFileData("", "created", true);
                                    chkAllFiles.Checked = true;
                                    BindFilesCombo();
                                    cboFileList.SelectedValue = cboFileList.Tag;
                                    if (cboFileList.SelectedIndex != -1)
                                    {
                                        OnStateChanged(ClientFileState.None);
                                        cboFileList.Tag = null;
                                        DisplayFileData(lblfileinfo, EventArgs.Empty);
                                        return;
                                    }
                                }

                                if (cboFileList.Items.Count > 0)
                                {
                                    var cantFindFileOnClient = Session.CurrentSession.Resources.GetResource("OMSNFSFOC", "Could not find %FILE% %2% for %CLIENT% %1%. It may not exist or you have insufficient permissions to view the %FILE%.", "", _client.ClientNo, Convert.ToString(cboFileList.Tag));

                                    if (FileNotFound != null)
                                    {
                                        OnFileNotFound(new AlertEventArgs(new Alert[1] { new Alert(cantFindFileOnClient.Text, FWBS.OMS.Alert.AlertStatus.Amber) }));
                                    }
                                    else
                                    {
                                        FWBS.OMS.UI.Windows.MessageBox.Show(cantFindFileOnClient, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    cboFileList.SelectedIndex = 0;
                                }
                                else
                                {
                                    OnStateChanged(ClientFileState.None);
                                    cboFileList.Tag = null;
                                    DisplayFileData(lblfileinfo, EventArgs.Empty);
                                    return;
                                }

                            }
                            DisplayFileData(lblfileinfo, EventArgs.Empty);
                            cboFileList.Tag = null;
                        }
                        else
                        {
                            // File No not specified on the clienttxt textbox so check if there
                            // are any files listed if not display or fire event message and check for
                            // all files option if not specified.
                            if (cboFileList.Items.Count == 0)
                            {
                                if (chkAllFiles.Checked) // All Files has been searched and there is still no Files for this client
                                {
                                    if (FileNotFound != null)
                                    {
                                        OnFileNotFound(new AlertEventArgs(new Alert[1] { new FWBS.OMS.Alert(res_fnf.Text, FWBS.OMS.Alert.AlertStatus.Amber) }));
                                    }
                                    else
                                    {
                                        FWBS.OMS.UI.Windows.MessageBox.Show(res_fnf, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        txtClientNo.Focus();
                                        epMessage.SetError(txtClientNo, res_fnf.Text);
                                        txtClientNo.Text = _client.ClientNo;
                                        OnStateChanged(ClientFileState.None);
                                    }
                                }
                                else
                                {
                                    // Check if the show all files option is visible if so check and resubmit file List and 
                                    // re check UpdateFormDetails
                                    if (chkAllFiles.Visible)
                                    {
                                        if (MessageBox.Show(Session.CurrentSession.Resources.GetMessage("OMSFNFLA", "There are no Live %FILES% available for %CLIENT% No : %1%, would you like to List all %FILES%?", "", txtClientNo.Text).Text, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                                        {
                                            if (FileNotFound != null)
                                            {
                                                OnFileNotFound(new AlertEventArgs(new Alert[1] { new FWBS.OMS.Alert(res_fnf.Text, FWBS.OMS.Alert.AlertStatus.Amber) }));
                                            }
                                            else
                                            {
                                                FWBS.OMS.UI.Windows.MessageBox.Show(res_fnf, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                epMessage.SetError(txtClientNo, res_fnf.Text);
                                                ClearActive();
                                            }
                                        }
                                        else
                                        {
                                            chkAllFiles.Checked = true;
                                            SetupFileData("", "created", chkAllFiles.Checked);
                                            BindFilesCombo();
                                            this.GetClient();
                                        }
                                    }
                                    else
                                    {
                                        // Chkallfiles isn't visible presume the programmer wishes not to display object
                                        // fire the event or display OMSNoFilesFound
                                        if (FileNotFound != null)
                                        {
                                            OnFileNotFound(new AlertEventArgs(new Alert[1] { new FWBS.OMS.Alert(res_fnf.Text, FWBS.OMS.Alert.AlertStatus.Amber) }));
                                        }
                                        else
                                        {
                                            FWBS.OMS.UI.Windows.MessageBox.Show(res_fnf, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            epMessage.SetError(txtClientNo, res_fnf.Text);
                                        }
                                    }
                                }

                                DisplayFileData(lblfileinfo, EventArgs.Empty);
                            }
                        }
                    }
                }
                else
                {

                    // Reset the Screen as there is no _Client Object initialised
                    picIcon.Visible = false;
                    lnkRefresh.Visible = false;
                    lblfileinfo.Text = "";
                    lblclientInfo.Text = "";
                    chkAllFiles.Checked = false;
                    chkAllFiles.Enabled = false;
                    cboFileList.BeginUpdate();
                    cboFileList.DataSource = null;
                    epMessage.SetError(cboFileList, string.Empty);
                    cboFileList.EndUpdate();
                    if (_fileview != null)
                        _fileview = null;
                }
            }
            catch (OMSException ex)
            {
                if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                {
                    throw new FWBS.OMS.Security.SecurityException(FWBS.OMS.HelpIndexes.PasswordRequestCancelled);
                }
                else if (ex.HelpID == HelpIndexes.ContactNotFound)
                {
                    throw;
                }
            }
		}

        private void BindFilesCombo()
        {
            
            cboFileList.BeginUpdate();
            cboFileList.DataSource = null;
            cboFileList.DisplayMember = "fileJointDesc";
            cboFileList.ValueMember = "fileNo";
            cboFileList.DataSource = _fileview;
            if (cboFileList.Items.Count > 0)
                cboFileList.SelectedIndex = 0;
            cboFileList.EndUpdate();
        }

		public new void Focus()
		{
			txtClientNo.Focus();
		}

		private void ucSelectClientFile_Enter(object sender, System.EventArgs e)
		{
            if (Session.CurrentSession.IsLoggedIn && _client == null && Session.CurrentSession.CurrentUser.RemberLastClientnFile)
            {
                _favourites = new Favourites(_favname, "LAST10");
                DataView dt = _favourites.GetDataView();
                dt.RowFilter = dt.RowFilter + " and  usrFavObjParam4 = 0";
                if (_favourites.Count > 0)
                {
                    string key;
                    if (cboFileList.Visible)
                        key = String.Format("{0} {1}", dt[0]["usrFavObjParam2"], dt[0]["usrFavObjParam3"]);
                    else
                        key = String.Format("{0}", dt[0]["usrFavObjParam2"]);
                    txtClientNo.Text = key;
                    txtClientNo_Leave(sender, e);
                }
            }
		}

        private void ucNavCommands2_LinkClicked(FWBS.OMS.UI.Windows.ucNavCmdButtons LinkButton)
        {
            string code = "";

            if (LinkButton.Key == "Clear")
            {
                mnuClearList_Click(null, new EventArgs());
            }
            else if (LinkButton.Key == "AddGlobal")
            {
                if (ValidateSelection())
                {
                    eFavType.SelectedValue = "GLOBAL";
                    InitFavourites();
                    code = SetFavourites();
                    goto AddFavourite;
                }
            }
            else if (LinkButton.Key == "AddDept")
            {
                if (ValidateSelection())
                {
                    eFavType.SelectedValue = "DEPT";
                    InitFavourites();
                    code = SetFavourites();
                    goto AddFavourite;
                }
            }
            else if (LinkButton.Key == "Add")
            {
                if (ValidateSelection())
                {
                    eFavType.SelectedValue = "FAVOURITE";
                    InitFavourites();
                    code = SetFavourites();
                    goto AddFavourite;
                }
            }



			return;

			AddFavourite:
				if (pnlFileInfo.Visible && _client != null)
				{
					try
					{
						switch(code)
						{
							case "GLOBAL":
								_favourites.ApplyFilter("GLOBAL", "[usrFavObjParam2] = '" + txtClientNo.Text.Replace("'","''") + "' AND [usrFavObjParam3] = '" + Convert.ToString(cboFileList.SelectedValue).Replace("'","''") + "'");
								break;
							case "DEPT":
								_favourites.ApplyFilter(Session.CurrentSession.CurrentFeeEarner.DefaultDepartment, "[usrFavObjParam2] = '" + txtClientNo.Text.Replace("'","''") + "' AND [usrFavObjParam3] = '" + Convert.ToString(cboFileList.SelectedValue).Replace("'","''") + "'");
								break;
							default:
								_favourites.ApplyFilter("[usrFavObjParam2] = '" + txtClientNo.Text.Replace("'","''") + "' AND [usrFavObjParam3] = '" + Convert.ToString(cboFileList.SelectedValue).Replace("'","''") + "'");
								break;
						}

						
						string caption = String.Empty;
						if (_fileview.Table.Columns.Contains("filenickname"))
							caption = Convert.ToString(_fileview[cboFileList.SelectedIndex]["filenickname"]).Trim();
						if (caption ==String.Empty) caption = txtClientNo.Text + "/" + cboFileList.SelectedValue.ToString() + " : " + _client.ClientName + Environment.NewLine + _fileview[cboFileList.SelectedIndex]["fileDesc"];


						if (_favourites.Count==0)
						{
							switch(code)
							{
								case "GLOBAL":
									_favourites.AddGlobalFavourite(code, _client.GetOMSType().Glyph.ToString(), "GLOBAL", caption, txtClientNo.Text,(string)cboFileList.SelectedValue);
									break;
								case "DEPT":
									_favourites.AddGlobalFavourite(code, _client.GetOMSType().Glyph.ToString(), Session.CurrentSession.CurrentFeeEarner.DefaultDepartment, caption, txtClientNo.Text,(string)cboFileList.SelectedValue);
									break;
								default:
									_favourites.AddFavourite("MYFAV", _client.GetOMSType().Glyph.ToString(), caption,txtClientNo.Text,(string)cboFileList.SelectedValue);
									break;
							}
                            
                            InitFavourites();
						}
					}
					finally
					{
						_favourites.ResetFilter();
					}
				}
				else if (_client != null)
				{
					try
					{
						switch(code)
						{
							case "GLOBAL":
								_favourites.ApplyFilter("GLOBAL", "[usrFavObjParam2] = '" + txtClientNo.Text.Replace("'","''") + "'");
								break;
							case "DEPT":
								_favourites.ApplyFilter(Session.CurrentSession.CurrentFeeEarner.DefaultDepartment, "[usrFavObjParam2] = '" + txtClientNo.Text.Replace("'","''") + "'");
								break;
							default:
								_favourites.ApplyFilter("[usrFavObjParam2] = '" + txtClientNo.Text.Replace("'","''") + "'");
								break;
						}

						string caption = _client.Nickname;
						if (caption ==String.Empty) caption =  txtClientNo.Text + " : " + _client.ClientName;

						if (_favourites.Count==0)
						{
							switch(code)
							{
								case "GLOBAL":
									_favourites.AddGlobalFavourite(code, _client.GetOMSType().Glyph.ToString(), "GLOBAL", caption, txtClientNo.Text);
									break;
								case "DEPT":
									_favourites.AddGlobalFavourite(code, _client.GetOMSType().Glyph.ToString(), Session.CurrentSession.CurrentFeeEarner.DefaultDepartment, caption, txtClientNo.Text);
									break;
								default:
									_favourites.AddFavourite("MYFAV", _client.GetOMSType().Glyph.ToString(), caption,txtClientNo.Text);
									break;
							}
                            InitFavourites();
						}
					}
					finally
					{
						_favourites.ResetFilter();
    				}
				}
		}


        private bool ValidateSelection()
        {
            if (this.cboFileList.Visible)
            {
                return CheckForClientAndFile();
            }
            else
            {
                return CheckForClient();
            }
        }


        private bool CheckForClientAndFile()
        {
            if (!string.IsNullOrWhiteSpace(this.txtClientNo.Text) && (!string.IsNullOrWhiteSpace(Convert.ToString(this.cboFileList.Value))))
                return true;
            else
            {
                System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("SCF_NODATAFIL", "Please ensure that you have selected both a Client and a Matter.", "").Text, Session.CurrentSession.Resources.GetResource("SCF_NODATACAPTN", "Insufficient Data", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }


        private bool CheckForClient()
        {
            if (!string.IsNullOrWhiteSpace(this.txtClientNo.Text))
                return true;
            else
            {
                System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("SCF_NODATACLI", "Please ensure that you have selected a Client.", "").Text, Session.CurrentSession.Resources.GetResource("SCF_NODATACAPTN", "Insufficient Data", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }


		private void ucNavCommands1_LinkClicked(FWBS.OMS.UI.Windows.ucNavCmdButtons LinkButton)
		{
			try
			{
				txtClientNo.Text = LinkButton.Key;
                this.GetClient();
				this.ClientFileState = ClientFileState.Proceed;
				txtClientNo.Focus();
			}
			catch (Exception ex)
			{
				_client = null;
				updateformdetails();
				ErrorBox.Show(ParentForm, ex);
			}
		}

		private void btnExpand_Click(object sender, System.EventArgs e)
		{
			if (pnlFavourites.Visible)
			{
				this.FavouritesVisible = false;
				if (ParentForm != null)
				{
					ParentForm.ClientSize  = new Size(ParentForm.ClientSize.Width - pnlFavourites.Width,ParentForm.ClientSize.Height);
					ParentForm.Left = ParentForm.Left + (pnlFavourites.Width / 2);
				}
			}
			else
			{
				this.FavouritesVisible = true;
				if (ParentForm != null)
				{
					ParentForm.ClientSize = new Size(ParentForm.ClientSize.Width + pnlFavourites.Width,ParentForm.ClientSize.Height);
					ParentForm.Left = ParentForm.Left - (pnlFavourites.Width / 2);
				}
			}
			txtClientNo.Focus();
		}

		private void mnuRemoveFav_Click(object sender, System.EventArgs e)
		{
            try
            {
                ucNavCmdButtons lnkbut = (ucNavCmdButtons)contextMenu1.SourceControl;

                string code = GetCurrentFavTypeCode();
                var message = code == "LAST10"
                    ? ResourceLookup.GetLookupText("LstRUSUREDelete", "Are you sure you wish to Delete the '%1%' from the Last 10", "", lnkbut.Text)
                    : ResourceLookup.GetLookupText("FavRUSUREDelete", "Are you sure you wish to Delete the Favourite '%1%'", "", lnkbut.Text);

                if (MessageBox.Show(message, FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SetFavourites();

                    _favourites.RemoveFavourite(lnkbut.Tag.ToString());
                    InitFavourites();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

		private void mnuRename_Click(object sender, System.EventArgs e)
		{
			try
			{
				ucNavCmdButtons lnkbut = (ucNavCmdButtons)contextMenu1.SourceControl;
				string ret = InputBox.Show(this.ParentForm, Session.CurrentSession.Resources.GetMessage("FAVRENAME","Please rename the favourite link to the name of your liking.", "").Text,"",lnkbut.Text);
			
				Cursor = Cursors.WaitCursor;

				if (ret != InputBox.CancelText)
				{
                    UpdateFavouriteInDatabase(lnkbut.Text, ret);
					SetFavourites();
                    InitFavourites();
				}
			}
			catch (Exception ex)
			{
				Cursor = Cursors.Default;
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

        private void UpdateFavouriteInDatabase(string originalText, string newText)
        {
            string favFilter = "";
            string favType = GetCurrentFavTypeCode();
            if (favType == "FAVOURITE")
                favFilter = "MYFAV";
            else
                favFilter = favType;
            UpdateFavouriteData(originalText, newText, favFilter);
        }

        private DataTable UpdateFavouriteData(string originalText, string newText, string favFilter)
        {
            List<IDataParameter> parList = new List<IDataParameter>();
            FWBS.OMS.Data.IConnection cnn = Session.CurrentSession.CurrentConnection;
            
            parList.Add(cnn.CreateParameter("@USRID", Session.CurrentSession.CurrentUser.ID));
            parList.Add(cnn.CreateParameter("@FAVTYPE", _favourites.CurrentFavouriteType));
            parList.Add(cnn.CreateParameter("@FAVFILTER", favFilter)); 
            parList.Add(cnn.CreateParameter("@ORIGINALTEXT", originalText));
            parList.Add(cnn.CreateParameter("@NEWTEXT", newText));

            string sql = "update dbUserFavourites set usrFavObjParam1 = @NEWTEXT where usrID = @USRID and usrFavType = @FAVTYPE and usrFavDesc = @FAVFILTER and usrFavObjParam1 = @ORIGINALTEXT";
            return cnn.ExecuteSQL(sql, parList);
        }

        private void DeleteLast10Record(long favID)
        {
            List<IDataParameter> parList = new List<IDataParameter>();
            FWBS.OMS.Data.IConnection cnn = Session.CurrentSession.CurrentConnection;
            parList.Add(cnn.CreateParameter("@favID", favID));
            string sql = "delete from dbUserFavourites where favID =  @favID";
            cnn.ExecuteSQL(sql, parList);
        }

		private void eXPComboBox2_Changed(object sender, System.EventArgs e)
		{
            this.mnuRemoveFav.Text = GetRemoveItemLabel();
            if (!this.IsLoading)
                InitFavourites();
		}

		private void ucSelectClientFile_ParentChanged(object sender, System.EventArgs e)
		{
			if (ParentForm != null && _CloseAlreadySet == false && _autosave)
			{
				_CloseAlreadySet = true;
                lblfile.BackColor = Color.FromArgb(this.BackColor.R,this.BackColor.G,this.BackColor.B);
                lblclientInfo.BackColor = lblfile.BackColor;
			}
		}


		#endregion

		#region Properties

		public SelectClientFileSearchType SelectClientFileSearchType
		{
			get
			{
				return _searchtype;
			}
			set
			{
				_searchtype = value;
			}
		}


		[DefaultValue(true)]
		public bool AutoSave
		{
			get
			{
				return _autosave;
			}
			set
			{
				_autosave = value;
			}
		}

		/// <summary>
		/// Gets or Sets whether the screen will display the phases combo box, if there are any to show.
		/// </summary>
		[DefaultValue(true)]
		public bool ShowPhases
		{
			get
			{
				return _showphases;
			}
			set
			{
				_showphases = value;
			}
		}

		/// <summary>
		/// View File Information or not will remove the File Information option and File Picker drop down
		/// </summary>
		public bool SelectFileVisible
		{
			get
			{
                return _selectFileVisible;
			}
			set
			{
				if (pnlMain.Tag == null)
				{
					// Store Original Settings for GBSize
					pnlMain.Tag = pnlMain.Size.Height;
				}

				if (value)
				{
                    _selectFileVisible = true;
					pnlFileInfo.Visible = true;
					lblfile.Visible = true;
					cboFileList.Visible = true;
					lblPhases.Visible = false;
					cboPhases.Visible = false;
					pnlFind.Height = 85;
					this.CheckAllFileOptionVisible = true;
					if (!this.IsLoading)
                        this.InitFavourites();
				}
				else
				{
                    _selectFileVisible = false;
					pnlFileInfo.Visible = false;
					lblfile.Visible = false;
					cboFileList.Visible = false;
					lblPhases.Visible = false;
					cboPhases.Visible = false;
					pnlFind.Height = 85;
					this.CheckAllFileOptionVisible = false;
                    if (!this.IsLoading)
                        this.InitFavourites();
				}
				
			}
		}

		/// <summary>
		/// View the Show All %FILES% option on the Designer at Runtime
		/// </summary>
		[DefaultValue(true)]
		public bool CheckAllFileOptionVisible
		{
			get
			{
				return chkAllFiles.Visible;
			}
			set
			{
				chkAllFiles.Visible = value;
			}
		}

		/// <summary>
		/// If True will display the Favourites Option which will allow the UI to Display the options
		/// </summary>
		[DefaultValue(true)]
		public bool FavouritesVisible
		{
			get
			{
				return _favvisible;
			}
			set
			{
				pnlFavourites.Visible = value;
				_favvisible = value;
				if (Session.CurrentSession.IsLoggedIn == false) return;
				if (_favvisible) 
				{
					this.resourceLookup1.SetLookup(this.btnExpand,new FWBS.OMS.UI.Windows.ResourceLookupItem("Shrink","Shrink",""));
                    if (!this.IsLoading)
                        this.InitFavourites();
				}
				else
				{
					this.resourceLookup1.SetLookup(this.btnExpand,new FWBS.OMS.UI.Windows.ResourceLookupItem("Expand","Expand",""));
				}
			}
		}

        private bool IgnoreOnStateChange = false;

		public ClientFileState ClientFileState
		{
			get
			{
				return _cfstate;
			}
			set
			{
				_cfstate = value;
                if(IgnoreOnStateChange == false)
    				OnStateChanged(_cfstate);
			}
		}

		/// <summary>
		/// Will Turn the All Files searched option when true will set FileStatus = '%' else FileStatus = 'LIVE'
		/// </summary>
		[DefaultValue(true)]
		public bool CheckAllFiles
		{
			get
			{
				return chkAllFiles.Checked;
			}
			set
			{
				chkAllFiles.Checked = value;
			}
		}

		/// <summary>
		/// Gets or Sets the currently selected client object.
		/// </summary>
		[DefaultValue(null)]
		[Browsable(false)]
		public Client SelectedClient
		{
			get
			{
				if (_client != null)
					return _client;
				else
					return null;
			}
			set
			{
				_client = value;
				updateformdetails();
			}
		}

		/// <summary>
		/// Gets the currently selected file object.
		/// </summary>
		[Browsable(false)]
        public OMSFile SelectedFile
		{
			get
			{
				if (_file == null)
                    try
                    {
                        if (_fileview == null)
                            return null;

                        if (cboFileList.SelectedIndex > -1)
                        {
                            _file = OMSFile.GetFile((long)_fileview[cboFileList.SelectedIndex].Row["fileid"]);
                            return _file;
                        }
                        else
                            return null;
                    }
                    catch (OMSException ex)
                    {
                        if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                        {
                            throw new FWBS.OMS.Security.SecurityException(FWBS.OMS.HelpIndexes.PasswordRequestCancelled);
                        }
                        else
                            return null;
                    }

				else
				{
                    if (cboFileList.SelectedIndex == -1)
                    {
                        // 26/02/2010 - Dont show Error if not checked all Files
                        if (chkAllFiles.Checked)
                            ErrorBox.Show(ParentForm, new OMSException2("ERRFILNTFND", "%FILE% not found ... "));
                        cboFileList.SelectedValue = cboFileList.Tag;
                        return null;
                    }
                    else if (_file.ID == (long)_fileview[cboFileList.SelectedIndex].Row["fileid"])
                        return _file;
                    else
                        return OMSFile.GetFile((long)_fileview[cboFileList.SelectedIndex].Row["fileid"]);
				}
			}
		}

		[Category("Buttons")]
		public Button CancelButton
		{
			get
			{
				if (btnCancel == null) btnCancel = new Button();
				return btnCancel;
			}
			set
			{
				btnCancel = value;
				btnCancel.Click +=new EventHandler(btnCancel_Click);
			}
		}

		[Category("Buttons")]
		public Button OKButton
		{
			get
			{
				if (btnOK == null) btnOK = new Button();
				return btnOK;
			}
			set
			{
				btnOK = value;
                btnOK.Click -= new EventHandler(timDraw_Tick);
                btnOK.Click += new EventHandler(timDraw_Tick);
			}
		}

		[Category("Buttons")]
		public Button FindButton
		{
			get
			{
				if (btnFind == null) btnFind = new Button();
				return btnFind;
			}
			set
			{
				btnFind = value;
				btnFind.Click += new EventHandler(btnFind_Click);
			}
		}

		[Category("Buttons")]
		public Button ViewFileButton
		{
			get
			{
				if (btnViewFile == null) btnViewFile = new Button();
				return btnViewFile;
			}
			set
			{
				btnViewFile = value;
				btnViewFile.Enabled=false;
				btnViewFile.Click +=new EventHandler(btnViewFile_Click);
			}
		}
		
		[Category("Buttons")]
		public Button ViewClientButton
		{
			get
			{
				if (btnViewClient == null) btnViewClient = new Button();
				return btnViewClient;
			}
			set
			{
				btnViewClient = value;
				btnViewClient.Enabled=false;
				btnViewClient.Click +=new EventHandler(btnViewClient_Click);
			}
		}

		[Category("Favourites")]
		public int FavouriteHeight
		{
			get
			{
				return _favheight;
			}
			set
			{
				_favheight = value;
			}
		}
		
		#endregion

		#region Methods

		/// <summary>
		/// Sets the currently selected file phase to the file object.
		/// </summary>
        /// GJ Change 07th March 2011 - Correct File Not Passed through.
		public void SetFilePhase()
		{
			try
			{
                if (_showphases && SelectedFile != null) // && cboPhases.Visible MNW change
				{
                    object orig = SelectedFile.GetExtraInfo("phid");
					object newval = cboPhases.SelectedValue;
					if (orig.Equals(newval) == false && newval != null)
					{
                        SelectedFile.SetExtraInfo("phid", newval);
                        SelectedFile.Update();
					}
				}
			}
			catch 
			{}
		}


		public void AddToTop10()
		{
			try
			{
				if (_client != null)
				{
					DataView dv = null;

					try
					{
						_favourites = new Favourites(_favname,"LAST10");
						dv = _favourites.GetDataView();

                        if (SelectFileVisible)
						{
							_favourites.ApplyFilter("[usrFavObjParam2] = '" + _client.ClientNo.Replace("'","''") + "' AND [usrFavObjParam3] = '" + Convert.ToString(cboFileList.SelectedValue).Replace("'","''") + "'");

							string caption = String.Empty;
							if (_fileview.Table.Columns.Contains("filenickname"))
								caption = Convert.ToString(_fileview[cboFileList.SelectedIndex]["filenickname"]).Trim();
							if (caption == String.Empty) caption = txtClientNo.Text + "/" 
                                                                    + cboFileList.SelectedValue.ToString() 
                                                                    + " : " + _client.ClientName 
                                                                    + Environment.NewLine 
                                                                    + _fileview[cboFileList.SelectedIndex]["fileDesc"];
							
							if (_favourites.Count==0)
								_favourites.AddFavourite("LAST10",_file.GetOMSType().Glyph.ToString(),caption, _client.ClientNo,(string)cboFileList.SelectedValue,"-1");
							else
								_favourites.Param4(0, -1);
						}
						else
						{
							_favourites.ApplyFilter("[usrFavObjParam2] = '" + _client.ClientNo.Replace("'","''") + "'");
							
							string caption = _client.Nickname;
							if (caption ==String.Empty) caption =  txtClientNo.Text + " : " + _client.ClientName;

							if (dv.Count==0)
								_favourites.AddFavourite("LAST10",_client.GetOMSType().Glyph.ToString(),caption ,_client.ClientNo,"","-1");
							else
								_favourites.Param4(0, -1);
						}

						_favourites.ResetFilter();
                        dv = _favourites.GetDataView();

						dv.Sort = "usrFavObjParam4";
						for (int i = dv.Count-1; i >= 0; i--)
						{
							dv[i]["usrFavObjParam4"] = i;
						}
						if (dv.Count > 10)
						{
							for (int i = dv.Count-1 ; i >= 10; i--)
							{
                                dv.Delete(i);
							}
						}
                        _favourites.Update();

						if (eFavType.SelectedIndex != -1)
						{
							_favourites = new Favourites(_favname,"FAVINDEX");
                            if (_favourites.Count > 0)
                            {
                                DataView favDV = _favourites.GetDataView();
                                _favourites.UpdateFavouriteType(eFavType.SelectedIndex, Convert.ToInt64(favDV[0][0]));
                            }
                            else
                                _favourites.AddFavourite("FAVINDEX", "", "", "", "", Convert.ToString(eFavType.SelectedIndex));

						}
					}
					finally
					{
						_favourites.ResetFilter();
					}
				}
			}
			catch (Exception ex)
			{
                ErrorBox.Show(ParentForm, new OMSException2("LAST10ERR", "The Last 10 has become corrupted.\n\nPlease click Clear List the next time you use this dialog. Sorry for any inconvenience.", ex, false));
			}
		}


		/// <summary>
		/// Gets the client and populates all areas of information within the user control.
		/// </summary>
		/// <param name="clNo">Client number used to filter the client down.</param>
		/// <returns>A boolean value indicating whether the client has been found or not.</returns>
		public bool GetClient (string clNo)
		{
			_client = Client.GetClient(clNo);
			txtClientNo.Tag = txtClientNo.Text;

			return GetClient(false);
		}

		/// <summary>
		/// Gets the client and populates all areas of information within the user control.
		/// </summary>
		/// <param name="clId">Client identifier used to filter the client down.</param>
		/// <returns>A boolean value indicating whether the client has been found or not.</returns>
		public bool GetClient (long clId)
		{
			_client = Client.GetClient(clId);
			return GetClient(false);
		}

		/// <summary>
		/// Populates the user controls informational areas with the specific client object.
		/// </summary>
		/// <returns>A boolean value indicating whether the client has been found or not.</returns>
		public bool GetClient ()
		{
			return GetClient(true);
		}

		/// <summary>
		/// Populates the user controls informational areas with the specific client object.
		/// </summary>
		/// <param name="client">A client whos information is to be displayed.</param>
		/// <returns>A boolean value indicating whether the client has been found or not.</returns>
		public bool GetClient (Client client)
		{
			_client = client;
			return GetClient(false);
		}

		/// <summary>
		/// Gets a client object based on whatever has been picked from the favourites combo box
		/// or from the editable text field.
		/// </summary>
		/// <param name="force">Forces the client object to be refound using the favourites list or the editable field.</param>
		/// <returns>A boolean value indicating whether the client has been found or not.</returns>
		public bool GetClient(bool force)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				if (force)
				{
					// Parse the clientno and split
					string[] splitstr = new string[2];
					char[] delimiter;
					//TODO: ConfigSetting-/ClientSearch/ClientDelimiter
					delimiter = FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("/config/clientSearch/clientDelimiter"," ./:").ToCharArray();
					try
					{
						txtClientNo.Text = txtClientNo.Text.Trim();
						splitstr = txtClientNo.Text.Split(delimiter,2);
						txtClientNo.Text =  splitstr[0];
						cboFileList.Tag = splitstr[1];
					}
					catch 
					{
						txtClientNo.Text = splitstr[0];
						cboFileList.Tag = null;
					}
                    try
                    {
                        _client = Client.GetClient(txtClientNo.Text);
                    }
                    catch (Exception ex)
                    {
                        var cantFindClient = Session.CurrentSession.Resources.GetResource("OMSNFSC", "Could not find %CLIENT% %1%. It may not exist or you have insufficient permissions to view the %CLIENT%.", "", txtClientNo.Text);
                        throw new Exception(cantFindClient.Text, ex);
                    }
                    if (FWBS.OMS.Session.CurrentSession.CurrentFileGatheredFromAlternative)
					{	// File information gathered from alternative FILE Need to populate
						// the combo box to the 
						cboFileList.Tag = FWBS.OMS.Session.CurrentSession.CurrentFile.FileNo;
						FWBS.OMS.Session.CurrentSession.CurrentFileGatheredFromAlternative = false;
					}

				}
                // 26/02/2010 - Removed code that Turn on Show All Files every time
				updateformdetails();
			
				ViewClientButton.Enabled = true;
				ViewFileButton.Enabled = true;
				return true;

			}
			catch(Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
				ViewClientButton.Enabled = false;
				ViewFileButton.Enabled = false;
                ClearActive();
                txtClientNo.Text = "";
				return false;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}


		/// <summary>
		/// Gets the client and file based on a file identifier. Then populates all areas of information within the user control.
		/// </summary>
		/// <param name="fileId">File identifier used to filter the client / file down.</param>
		/// <returns>A boolean value indicating whether the client / file has been found or not.</returns>
		public bool GetFile (long fileId)
		{
			_file = OMSFile.GetFile(fileId);
			return GetFile(_file);
		}

		/// <summary>
		/// Populates the user controls informational areas with the specific client and file objects.
		/// </summary>
		/// <param name="file">A file object whos information is to be displayed.</param>
		/// <returns>A boolean value indicating whether the client / file has been found or not.</returns>
		public bool GetFile (OMSFile file)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				_file = file;
				if (_file != null)
				{
                    _client = file.Client;
                    // 26/02/2010 - Remove the Show All Files Checked True
					txtClientNo.Text = _client.ClientNo + " " + _file.FileNo;
                    return GetClient();
				}
			}
            catch (OMSException ex)
            {
                if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                {
                    ErrorBox.Show(ParentForm, ex);
                    ViewClientButton.Enabled = false;
                    ViewFileButton.Enabled = false;
                    ClearActive();
                    txtClientNo.Text = "";
                    return false;
                }
            }
            finally
            {
                Cursor = Cursors.Default;

            }
			return false;
		}

        private bool IsLoading = true;

		/// <summary>
		/// Captures the load event of the usercontrol to initiate the favourites.
		/// </summary>
		/// <param name="sender">This user control instance.</param>
		/// <param name="e">Empty event arguments.</param>
		private void ucSelectClientFile_Load(object sender, System.EventArgs e)
		{
            try
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    DataTable dt = CodeLookup.GetLookups("EFAVTYPES");
                    eFavType.DataSource = dt;

                    // 26/02/2010 - Changed to Fire the Timer if an Client has been Set Externally
                    if (_client != null)
                    {
                        timLoad.Enabled = true;
                    }
                    if (pnlFileInfo.Visible)
                        _favname = "CLINETFILEFT";
                    else
                        _favname = "CLINETFT";

                    _favourites = new Favourites(_favname, "FAVINDEX");
                    if (_favourites.Count > 0)
                    {
                        int index = FWBS.Common.ConvertDef.ToInt32(_favourites.Param4(0), 0);
                        if (_favourites.Count > 0 && index < eFavType.Items.Count)
                        {
                            eFavType.SelectedIndex = index;
                        }
                        else if (_favourites.Count > 0)
                            eFavType.SelectedIndex = 0;
                    }
                    else
                        eFavType.SelectedIndex = 0;

                    _favourites = new Favourites(_favname, "PNLFAVSIZE");
                    if (_favourites.Count > 0)
                        pnlFavourites.Width = Convert.ToInt32(_favourites.Glyph(0));


                    ucPanelNav1.Text = eFavType.Text;
                    if (ParentForm != null && _CloseAlreadySet == false && _autosave)
                    {
                        _CloseAlreadySet = true;
                    }

                    if (pnlFileInfo.Visible)
                    {

                        _favourites = new Favourites(_favname, "FILINFOHEIGHT");
                        if (_favourites.Count > 0)
                            pnlFileInfo.Height = Common.ConvertDef.ToInt32(_favourites.Param1(0), pnlFileInfo.Height);
                    }

                    InitFavourites();
                    lblclientInfo_VisibleChanged(lblclientInfo, EventArgs.Empty);
                    DisplayFileData(lblfileinfo, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
            finally
            {
                IsLoading = false;
            }
		}

		#endregion

		#region Event Methods

		/// <summary>
		/// Raises the alert event.
		/// </summary>
		protected void OnAlert(AlertEventArgs args)
		{
			if (Alert != null)
				Alert(this, args);
		}

		/// <summary>
		/// Raises the no file found event.
		/// </summary>
		protected void OnFileNotFound(AlertEventArgs args)
		{
			if (FileNotFound != null)
				FileNotFound(this, args);
		}

		/// <summary>
		/// Raises the OnStateChanged event.
		/// </summary>
		protected void OnStateChanged(ClientFileState State)
		{
            if (pnlFileInfo.Visible && SelectedFile == null)
            {
                _cfstate = ClientFileState.None;
                _file = null;
            }
            else
                _cfstate = State;
			if (StateChanged != null)
			{
				StateChanged(this,new ClientFileStateEventArgs(_cfstate));
			}
		}

        #endregion

        #region Private

        private string GetRemoveItemLabel()
        {
            return GetCurrentFavTypeCode() == "LAST10"
                ? Session.CurrentSession.Resources.GetResource("SCF_REMOVELST", "Remove from Last 10", "").Text
                : Session.CurrentSession.Resources.GetResource("SCF_REMOVEFAV", "Remove Favourite", "").Text;
        }

        public string SetFavourites()
		{
			string code = GetCurrentFavTypeCode();
			
			switch (code)
			{
				case "FAVOURITE":
					_favourites = new Favourites(_favname,"MYFAV");
					break;
				case "GLOBAL":
					_favourites = new Favourites(_favname,code);
					_favourites.ApplyFilter("GLOBAL", "");
					break;
				case "DEPT":
					_favourites = new Favourites(_favname,code);
					_favourites.ApplyFilter(Session.CurrentSession.CurrentFeeEarner.DefaultDepartment, "");
					break;
				default:
					_favourites = new Favourites(_favname,code);
					break;
			}
			
            if(_favourites.CurrentFavouriteType.ToUpper() != "CLINETFILEFT")
			    _favourites.GetDataView().Sort = "usrFavObjParam4";

			return code;
		}

		private void mnuClearList_Click(object sender, System.EventArgs e)
		{
            string code = GetCurrentFavTypeCode();
            bool hasrole = ((Session.CurrentSession.CurrentUser.IsInRoles(new string[] { User.ROLE_PARTNER, User.ROLE_ADMIN, "MANAGER" })));

            switch (code)
            {
                case "FAVOURITE":
                    break;
                case "GLOBAL":
                    if (hasrole == false) return;
                    break;
                case "DEPT":
                    if (hasrole == false) return;
                    break;
            }
            
            if (MessageBox.Show(ResourceLookup.GetLookupText("FavRUSUREClear"),FWBS.OMS.Global.ApplicationName,MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
			{
				SetFavourites();

				DataView  vw = _favourites.GetDataView();
				for (int i = vw.Count-1; i > -1; i--)
				{
					try
					{
						vw.Delete(i);
					}
					catch{}
				}
                _favourites.Update();
				InitFavourites();
			}
		}

        private string GetCurrentFavTypeCode()
        {
            return Convert.ToString(eFavType.SelectedValue).ToUpper();
        }

        private void lblclientInfo_VisibleChanged(object sender, System.EventArgs e)
		{
			if (_client == null || !lblclientInfo.Visible)
                return;

            try
            {
                if (Convert.ToString(lblclientInfo.Tag) != Convert.ToString(_client.ClientID))
                {
                    _client.DefaultContact.Refresh();
                    _client.Refresh();
                    g_cldesc = _client.ClientDescription;
                    lblclientInfo.Tag = _client.ClientID;
                }
            }
            catch
            {
                ClearActive();
                txtClientNo.Text = "";
                g_cldesc = string.Empty;
                if (e == null)
                    throw;
            }

			try 
			{
				//INFO: A work around to get RTF to be RTL.  You must set the text whilst it is LTR first.
				lblclientInfo.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(g_cldesc);
                Global.SetRichTextBoxRightToLeft(lblclientInfo);
			}
			catch
			{
				lblclientInfo.Text = g_cldesc;
			}

		}

		private void timDraw_Tick(object sender, System.EventArgs e)
		{
			if (_client == null && this.GetClient() == false) 
			{
				OnStateChanged(ClientFileState.None);
				txtClientNo.Focus();
			}
            else
                OnStateChanged(ClientFileState.Proceed);
            OnDetailsDisplayed();
        }

		private void btnCancel_Click(object sender, EventArgs e)
		{
			txtClientNo.Text="";
		}

		private void btnFind_Click(object sender, EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
                lastclientid = 0;
                //Activate the file search.
				if (_searchtype == SelectClientFileSearchType.File)
				{
					OMSFile f = Services.Searches.FindFile();
					if (f != null)
					{
						txtClientNo.Text="";
                        _client = f.Client;
                        this.GetFile(f);
						OnStateChanged(ClientFileState.Proceed);
					}
				}
				else if (_searchtype == SelectClientFileSearchType.Client)
				{
					Client f = Services.Searches.FindClient();
					if (f != null)
					{
						txtClientNo.Text="";
						this.GetClient(f);
						OnStateChanged(ClientFileState.Proceed);
					}
				}		
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
				btnOK.Focus();
			}

		}

		private void btnViewClient_Click(object sender, EventArgs e)
		{
			if (ViewClientButton.Enabled)
			{
				try
				{
					Cursor = Cursors.WaitCursor;
					FWBS.OMS.UI.Windows.Services.ShowClient(this.SelectedClient);
				}
				catch{}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

		private void btnViewFile_Click(object sender, EventArgs e)
		{
			if (ViewFileButton.Enabled)
			{
				try
				{
					Cursor = Cursors.WaitCursor;
					FWBS.OMS.UI.Windows.Services.ShowFile(this.SelectedFile);
				}
				catch{}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

		#endregion

		private void ClearActive()
		{
            lastclientid = 0;
            ViewClientButton.Enabled = false;
			ViewFileButton.Enabled = false;
			OKButton.Enabled=true;
			try
			{
				if (_file != null)
				{
					_file = null;
				}

				if (_client != null)
				{
					_client = null;
					_fileview = null;
					_filephases = null;
					cboFileList.SelectedIndex = -1;
					cboPhases.SelectedIndex = -1;
					lblPhases.Visible = false;
					cboPhases.Visible = false;
					pnlFind.Height = LogicalToDeviceUnits(85);
				}

				updateformdetails();
            }
			catch {}
			OnStateChanged(ClientFileState.None);
		}

		private void splitter1_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			_favourites = new Favourites(_favname,"PNLFAVSIZE");
			if (_favourites.Count > 0)
				_favourites.Glyph(0,pnlFavourites.Width.ToString());
			else
				_favourites.AddFavourite("PNLFAVSIZE",pnlFavourites.Width.ToString());
		
		}

		private void splitter2_SplitterMoved(object sender, System.Windows.Forms.SplitterEventArgs e)
		{
			if (pnlFileInfo.Visible)
			{
				_favourites = new Favourites(_favname,"FILINFOHEIGHT");
				if (_favourites.Count > 0)
					_favourites.Param1(0,pnlFileInfo.Height.ToString());
				else
					_favourites.AddFavourite("FILINFOHEIGHT","",pnlFileInfo.Height.ToString());
			}
		}

		private void lnkRefresh_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
            try
            {
                bool ret = false;

                if (_client != null)
                {
                   
                    ret = true;
                    _client.DefaultContact.Refresh();
                    _client.Refresh();
                    g_cldesc = _client.ClientDescription;                  
                 
                }
                if (_file != null)
                {
                    _file.Refresh();
                    ret = true;
                }

                if (ret == true)
                    updateformdetails();
            }
            catch { }          

		}

        private void txtClientNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt)
                return;
            if (e.Shift && e.KeyCode == Keys.ShiftKey)
                return;
            if (e.Control && e.KeyCode == Keys.ControlKey)
                return;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    break;
                case Keys.Down:
                    break;
                case Keys.Left:
                    break;
                case Keys.Right:
                    break;
                case Keys.Shift:
                    break;
                case Keys.Alt:
                    break;
                case Keys.Home:
                    break;
                case Keys.End:
                    break;
                case Keys.F1:
                    break;
                default:
                    ClearActive();
                    break;
            }
        }

        private void txtClientNo_Enter(object sender, EventArgs e)
        {
            txtClientNo.SelectAll();
        }

        private void txtClientNo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                ClearActive();
        }

        // 26/02/2010 - Added Timer Event so that the Question to Show All Matters is done after the Form is Visible
        private void timLoad_Tick(object sender, EventArgs e)
        {
            timLoad.Enabled = false;
            try { updateformdetails(); } catch { }
            OnStateChanged(ClientFileState.Proceed);

        }

	}



	/// <summary>
	/// Client / file state options.
	/// </summary>
	public enum ClientFileState
	{
		None,
		Confirm,
		Proceed
	}

	/// <summary>
	/// Client / File state delegate.
	/// </summary>
	public delegate void ClientFileStateChangedEventHandler (object sender, ClientFileStateEventArgs e);

	/// <summary>
	/// State changed event arguments of the Client / File control.
	/// </summary>
	public class ClientFileStateEventArgs : EventArgs
	{
		private ClientFileState _state;
		
		private ClientFileStateEventArgs(){}

		internal ClientFileStateEventArgs (ClientFileState state)
		{
			_state = state;
		}

		public ClientFileState State
		{
			get
			{
				return _state;
			}
		}
	}
}
