using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using FWBS.Common.UI;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Dashboard;
using FWBS.OMS.Data;
using FWBS.OMS.Design.CodeBuilder;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.Windows;
using FWBS.OMS.UI.Windows.Admin;



namespace FWBS.OMS.Design
{
    /// <summary>
    /// 30000 Form Designer for the Admin Kit
    /// </summary>
    public class frmDesigner : FWBS.OMS.UI.Windows.BaseForm
	{
        class FieldsForm : Form
        {
            public FieldsForm()
            {
                InitializeComponent();
            }

            private void InitializeComponent()
            {
                this.SuspendLayout();
                // 
                // FieldsForm
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
                this.ClientSize = new System.Drawing.Size(134, 161);
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
                this.Location = new Point(645, 105);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "FieldsForm";
                this.ShowInTaskbar = false;
                this.StartPosition = FormStartPosition.Manual;
                this.Visible = false;
                this.ResumeLayout(false);

            }
        }

        #region Fields
        private readonly string _statusCanvas = "Canvas";
        private readonly string _statusSelected = "Selected";
        private readonly string _statusReady = "Ready...";
        private readonly string _statusSaved = "Saved...";
        private bool processing = false;
        private ArrayList _controls = new ArrayList();
		private bool _ignorekey = false;
		private Form fieldsForm = new FieldsForm();
        private CodeWindow CodeWindow;
        private Hashtable _nipplecontainers = new Hashtable();
		private enum  NippleMode {acUDLR,acLR,acUD};
		private Point ClickOffset;
		private bool _menustatus = false;
		private Boolean _multi = false;
		private Enquiry _enqobject = null;
		private DataSet _edata = null;
		private int _acceleration = 1;
		private string _selectedcontrol = "";
		private object _repeat = null;
		internal string _currentform = "";
		internal string _currentfolder = @"\";
		private bool _updating = false;
		private Point tbDrag = new Point(0,0);
		private bool _moving = false;

		private ArrayList _favorites = new ArrayList();
        private const int _maxFavoritesCount = 9;
		private bool _isdirty = false;
        private bool _dpiWarning = false;

		private enum _dragsources {scListFields,scToolbox};
		private _dragsources _dragsource;

		private DataView GetPropertyView = null;
		private Control GetPropertyControl = null;
		internal static DataTable UndoTable = null;
		internal static bool NoUndo=false;
		internal static bool UndoOnce=false;
		internal static int grpchg = 0;
		internal static int grpcnt = 0;
		internal static string grpstat = "";
		private Rectangle Lassoo = new Rectangle(-1,-1,0,0);
		private Rectangle OldLassoo = new Rectangle(-1,-1,0,0);
		private Rectangle FinalLassoo = new Rectangle(-1,-1,0,0);

        private FWBS.OMS.UI.Windows.LockState ls = new FWBS.OMS.UI.Windows.LockState();

        FWBS.OMS.UI.Windows.EnquiryFormVersionDataArchiver enquiryformVersionDataArchiver;

        FWBS.OMS.UI.Windows.VersionComparisonSelector vcs;

        private bool alreadylocked;

		#endregion

		#region Private Form Fields
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup;
		private FWBS.OMS.UI.Windows.ThreeDPanel pnlCanvass;
		private FWBS.OMS.UI.Windows.EnquiryForm enquiryForm1;
        private System.Windows.Forms.ContextMenuStrip DesignerPopup;
		private System.ComponentModel.IContainer components = null;
		//
		// Designer Popup Menus
		//
        private System.Windows.Forms.ToolStripMenuItem mnuVersioning;
        private System.Windows.Forms.ToolStripMenuItem mnuCheckin;
        private System.Windows.Forms.ToolStripMenuItem mnuCompare;
		private System.Windows.Forms.ToolStripMenuItem mnuAlignActive;
		private System.Windows.Forms.ToolStripMenuItem menuItem5;
		private System.Windows.Forms.ToolStripMenuItem mnuDistributeActive;
		private System.Windows.Forms.ToolStripMenuItem mnuLeft;
		private System.Windows.Forms.ToolStripMenuItem mnuTop;
		private System.Windows.Forms.ToolStripMenuItem mnuBottom;
		private System.Windows.Forms.ToolStripMenuItem mnuRight;
		private System.Windows.Forms.ToolStripMenuItem mnuVertically;
		private System.Windows.Forms.ToolStripMenuItem mnuHorizontally;
		private System.Windows.Forms.ToolStripMenuItem mnuVIncrease;
		private System.Windows.Forms.ToolStripMenuItem mnuVDecrease;
		private System.Windows.Forms.ToolStripMenuItem mnuUnselect;
		private System.Windows.Forms.ToolStripMenuItem mnuRepeat;
		private System.Windows.Forms.ToolStripMenuItem mnuVTogether;
		private System.Windows.Forms.ToolStripMenuItem mnuHTogether;
		private System.Windows.Forms.ToolStripMenuItem mnuHIncrease;
		private System.Windows.Forms.ToolStripMenuItem mnuHDecrease;
        private System.Windows.Forms.MenuStrip mainMenu1;
		private System.Windows.Forms.ToolStripMenuItem mnuDebug;
		private System.Windows.Forms.ToolStripMenuItem mnuStyle;
        private System.Windows.Forms.ToolStripMenuItem mnuTools;
		private System.Windows.Forms.Panel pnlEnquiry;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label labQuestionPage;
		private System.Windows.Forms.PictureBox pictureBox1;
		private FWBS.OMS.UI.Windows.ThreeDPanel pnlNavigation;
		private FWBS.OMS.UI.Windows.OpenSaveEnquiry OpenSaveEnquiry1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnFinished;
		private System.Windows.Forms.Button btnNext;
		private System.Windows.Forms.Button btnBack;
		private System.Windows.Forms.Panel pnlWelcome;
		private System.Windows.Forms.Label labContinue;
		private System.Windows.Forms.PictureBox picWelcome;
		private System.Windows.Forms.RichTextBox labDescription;
		private System.Windows.Forms.Label labWelcome;
		private FWBS.OMS.UI.Windows.ThreeDPanel pnlWizardPage;
		private System.Windows.Forms.ToolStripMenuItem mnuWizardMode;
		private System.Windows.Forms.ToolStripMenuItem mnuStandardMode;
		private FWBS.OMS.UI.Windows.ThreeDPanel tdpOMS;
		private System.Windows.Forms.ImageList imgOMSTools;
		private System.Windows.Forms.ToolStripMenuItem mnuSelectAll;
		private System.Windows.Forms.ToolStripMenuItem mnuSendToBack;
        private System.Windows.Forms.ToolStripMenuItem mnuBringToFront;
		private System.Windows.Forms.ToolStripMenuItem mnuOpen;
		private System.Windows.Forms.Panel pnlWorkspace;
		private System.Windows.Forms.Panel pnlProperties;
		private System.Windows.Forms.ToolStripMenuItem menuItem11;
		internal System.Windows.Forms.Panel pnlPage;
		private System.Windows.Forms.ToolStripMenuItem menuItem20;
		private System.Windows.Forms.ToolStripMenuItem menuItem28;
		private System.Windows.Forms.Panel panel1;
		internal System.Windows.Forms.ToolStripMenuItem mnuNew;
		private System.Windows.Forms.Panel pnlPages;
		private System.Windows.Forms.ToolStripMenuItem mnuSave;
		private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ImageList imgTools;
		private System.Windows.Forms.ToolStripMenuItem mnuUndo;
		private System.Windows.Forms.ToolStripMenuItem mnuShowUndoList;
		private System.Windows.Forms.ToolStripMenuItem mnuEdit;
		private System.Windows.Forms.ToolStripMenuItem mnuResetProperties;
        private System.Windows.Forms.ToolStripMenuItem mnuResetCPos;
		private System.Windows.Forms.ToolStripMenuItem mnuPageRefresh;
		private System.Windows.Forms.TextBox Blue;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
		private System.Windows.Forms.ToolTip toolTip1;
		private FWBS.OMS.UI.TabControl tabPages;
		private System.Windows.Forms.Panel pnlFields;
		private System.Windows.Forms.ListBox lstListFields;
		private System.Windows.Forms.TabPage tpEnquiryHeader;
		private System.Windows.Forms.PropertyGrid pgMain;
		private FWBS.Common.UI.Windows.eXPPanel pnlActions;
		private System.Windows.Forms.LinkLabel lnkRemovePage;
		private System.Windows.Forms.LinkLabel lnkAddPage;
		private System.Windows.Forms.Splitter splitter2;
		private FWBS.OMS.UI.Windows.Admin.EnquiryPageEditor _page;
		private System.Windows.Forms.TabPage tpCustom;
		private FWBS.OMS.UI.Windows.ucFormStorage frmFieldsListFS;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.ToolStripMenuItem mnuCut;
		private System.Windows.Forms.ToolStripMenuItem mnuCopy;
		private System.Windows.Forms.ToolStripMenuItem mnuPaste;
		private System.Windows.Forms.ToolStripMenuItem mnuShowClipboard;
		private System.Windows.Forms.Panel pnlComponents;
		private System.Windows.Forms.ContextMenuStrip mnuComponents;
		private System.Windows.Forms.ToolStripMenuItem mnuLineUp;
        private System.Windows.Forms.Splitter splitter5;
		private System.Windows.Forms.ToolStripMenuItem mnuPreview;
		private FWBS.Common.UI.Windows.MenuCollection menuCollection1;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
		private System.Windows.Forms.ToolStripMenuItem mnuFav1;
		private System.Windows.Forms.ToolStripMenuItem mnuFav2;
		private System.Windows.Forms.ToolStripMenuItem mnuFav3;
		private System.Windows.Forms.ToolStripMenuItem mnuFav4;
		private System.Windows.Forms.ToolStripMenuItem mnuFav5;
		private System.Windows.Forms.ToolStripMenuItem mnuFav6;
		private System.Windows.Forms.ToolStripMenuItem mnuFav7;
		private System.Windows.Forms.ToolStripMenuItem mnuFav8;
		private System.Windows.Forms.ToolStripMenuItem mnuFav9;
		private System.Windows.Forms.TabPage tpCode;
		private FWBS.OMS.UI.Windows.Admin.EnquiryHeaderEditor _header;
		private System.Windows.Forms.Panel pnlMiddle;
		private System.Windows.Forms.Splitter spRight;
		private System.Windows.Forms.Splitter spLeft;
		private System.Windows.Forms.Panel panel3;
        private FWBS.OMS.Design.CodeBuilder.DataGridEvents dgEvents;
		private System.Windows.Forms.Timer timLineup;
		private System.Windows.Forms.ToolStripMenuItem mnuAutoTabIndex;
		private System.Windows.Forms.ToolStripMenuItem mnuWidest;
		private System.Windows.Forms.ToolStripMenuItem mnuNarrowest;
		private System.Windows.Forms.LinkLabel lnkInsert;
		private System.Windows.Forms.Label label1;
		private FWBS.OMS.UI.StatusBar statusBar1;
		private System.Windows.Forms.StatusBarPanel panCanvas;
		private System.Windows.Forms.StatusBarPanel panSelect;
		private System.Windows.Forms.StatusBarPanel panStatus;
		private System.Windows.Forms.LinkLabel lnkRegister;
        private System.Windows.Forms.ComboBox cmbViewFields;
		private System.Windows.Forms.Timer timRebuildcmbFields;
		private System.Windows.Forms.ComboBox cmbFields;
        private System.Windows.Forms.Label labNotSaved;
        private ToolStripSeparator menuItem27;
        private ToolStripSeparator menuItem7;
        private ToolStripSeparator mnuSpFav;
        private ToolStripSeparator menuItem12;
        private ToolStripSeparator menuItem21;
        private ToolStripSeparator menuItem25;
        private ToolStripSeparator menuItem19;
        private ToolStripSeparator mnuSp6;
        private ToolStripSeparator mnuSp8;
        private ToolStripSeparator mnuSp9;
        private ToolStripSeparator mnuSp7;
        protected Panel pnlMain;
        public UI.Windows.eToolbars OMSToolbars;
		private DataTable Clipboard;
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDesigner));
            this.resourceLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFinished = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.labContinue = new System.Windows.Forms.Label();
            this.mnuRepeat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAlignActive = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuLeft = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRight = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTop = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBottom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDistributeActive = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVertically = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVTogether = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVIncrease = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVDecrease = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHorizontally = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHTogether = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHIncrease = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHDecrease = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWidest = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNarrowest = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuAutoTabIndex = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSendToBack = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuBringToFront = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuResetProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuResetCPos = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuOpen = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem27 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPreview = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSpFav = new System.Windows.Forms.ToolStripSeparator();
            this.mnuFav1 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFav2 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFav3 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFav4 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFav5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFav6 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFav7 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFav8 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFav9 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuEdit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUndo = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem12 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem21 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuCut = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuPaste = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem25 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem19 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPageRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStyle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStandardMode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuWizardMode = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDebug = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowUndoList = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuShowClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuVersioning = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCheckin = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuCompare = new System.Windows.Forms.ToolStripMenuItem();
            this.tpEnquiryHeader = new System.Windows.Forms.TabPage();
            this.pgMain = new System.Windows.Forms.PropertyGrid();
            this._header = new FWBS.OMS.UI.Windows.Admin.EnquiryHeaderEditor();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlActions = new FWBS.Common.UI.Windows.eXPPanel();
            this.lnkInsert = new System.Windows.Forms.LinkLabel();
            this.lnkRemovePage = new System.Windows.Forms.LinkLabel();
            this.lnkAddPage = new System.Windows.Forms.LinkLabel();
            this.lnkRegister = new System.Windows.Forms.LinkLabel();
            this.tpCustom = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.label1 = new System.Windows.Forms.Label();
            this.tpCode = new System.Windows.Forms.TabPage();
            this.labNotSaved = new System.Windows.Forms.Label();
            this.dgEvents = new FWBS.OMS.Design.CodeBuilder.DataGridEvents();
            this.mnuLineUp = new System.Windows.Forms.ToolStripMenuItem();
            this.imgTools = new System.Windows.Forms.ImageList(this.components);
            this.imgOMSTools = new System.Windows.Forms.ImageList(this.components);
            this.pnlCanvass = new FWBS.OMS.UI.Windows.ThreeDPanel();
            this.pnlNavigation = new FWBS.OMS.UI.Windows.ThreeDPanel();
            this.pnlWelcome = new System.Windows.Forms.Panel();
            this.picWelcome = new System.Windows.Forms.PictureBox();
            this.labDescription = new System.Windows.Forms.RichTextBox();
            this.labWelcome = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pnlEnquiry = new System.Windows.Forms.Panel();
            this.pnlWizardPage = new FWBS.OMS.UI.Windows.ThreeDPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labQuestionPage = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.enquiryForm1 = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.tdpOMS = new FWBS.OMS.UI.Windows.ThreeDPanel();
            this.DesignerPopup = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuSp6 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSp8 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSp9 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSp7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUnselect = new System.Windows.Forms.ToolStripMenuItem();
            this.mainMenu1 = new System.Windows.Forms.MenuStrip();
            this.pnlWorkspace = new System.Windows.Forms.Panel();
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.pnlPage = new System.Windows.Forms.Panel();
            this.tabPages = new FWBS.OMS.UI.TabControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbFields = new System.Windows.Forms.ComboBox();
            this.menuItem11 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem20 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem28 = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlPages = new System.Windows.Forms.Panel();
            this.Blue = new System.Windows.Forms.TextBox();
            this.OpenSaveEnquiry1 = new FWBS.OMS.UI.Windows.OpenSaveEnquiry();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pnlFields = new System.Windows.Forms.Panel();
            this.lstListFields = new System.Windows.Forms.ListBox();
            this.cmbViewFields = new System.Windows.Forms.ComboBox();
            this.spRight = new System.Windows.Forms.Splitter();
            this.spLeft = new System.Windows.Forms.Splitter();
            this.frmFieldsListFS = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.pnlComponents = new System.Windows.Forms.Panel();
            this.mnuComponents = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.splitter5 = new System.Windows.Forms.Splitter();
            this.menuCollection1 = new FWBS.Common.UI.Windows.MenuCollection();
            this.pnlMiddle = new System.Windows.Forms.Panel();
            this.timLineup = new System.Windows.Forms.Timer(this.components);
            this.statusBar1 = new FWBS.OMS.UI.StatusBar();
            this.panCanvas = new System.Windows.Forms.StatusBarPanel();
            this.panSelect = new System.Windows.Forms.StatusBarPanel();
            this.panStatus = new System.Windows.Forms.StatusBarPanel();
            this.timRebuildcmbFields = new System.Windows.Forms.Timer(this.components);
            this._page = new FWBS.OMS.UI.Windows.Admin.EnquiryPageEditor();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.OMSToolbars = new FWBS.OMS.UI.Windows.eToolbars();
            this.tpEnquiryHeader.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.tpCustom.SuspendLayout();
            this.tpCode.SuspendLayout();
            this.pnlCanvass.SuspendLayout();
            this.pnlNavigation.SuspendLayout();
            this.pnlWelcome.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWelcome)).BeginInit();
            this.pnlEnquiry.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.DesignerPopup.SuspendLayout();
            this.mainMenu1.SuspendLayout();
            this.pnlWorkspace.SuspendLayout();
            this.pnlProperties.SuspendLayout();
            this.pnlPage.SuspendLayout();
            this.tabPages.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlFields.SuspendLayout();
            this.mnuComponents.SuspendLayout();
            this.pnlMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panCanvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panSelect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panStatus)).BeginInit();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Enabled = false;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(6, 8);
            this.resourceLookup.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cance&l";
            // 
            // btnFinished
            // 
            this.btnFinished.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFinished.Enabled = false;
            this.btnFinished.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFinished.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFinished.Location = new System.Drawing.Point(420, 8);
            this.resourceLookup.SetLookup(this.btnFinished, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnFinished", "&Finish", ""));
            this.btnFinished.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnFinished.Name = "btnFinished";
            this.btnFinished.Size = new System.Drawing.Size(75, 25);
            this.btnFinished.TabIndex = 2;
            this.btnFinished.Text = "&Finish";
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNext.Location = new System.Drawing.Point(336, 8);
            this.resourceLookup.SetLookup(this.btnNext, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnNext", "&Next >", ""));
            this.btnNext.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 25);
            this.btnNext.TabIndex = 1;
            this.btnNext.Text = "&Next >";
            this.btnNext.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnBack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBack.Location = new System.Drawing.Point(252, 8);
            this.resourceLookup.SetLookup(this.btnBack, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnBack", "< &Back", ""));
            this.btnBack.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 25);
            this.btnBack.TabIndex = 0;
            this.btnBack.Text = "< &Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // labContinue
            // 
            this.labContinue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labContinue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labContinue.Location = new System.Drawing.Point(144, 364);
            this.resourceLookup.SetLookup(this.labContinue, new FWBS.OMS.UI.Windows.ResourceLookupItem("labContinue", "To continue, click Next.", ""));
            this.labContinue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labContinue.Name = "labContinue";
            this.labContinue.Size = new System.Drawing.Size(339, 18);
            this.labContinue.TabIndex = 1;
            this.labContinue.Text = "To continue, click Next.";
            this.labContinue.Click += new System.EventHandler(this.labWelcome_Click);
            // 
            // mnuRepeat
            // 
            this.mnuRepeat.Enabled = false;
            this.resourceLookup.SetLookup(this.mnuRepeat, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuRepeat", "Repeat", ""));
            this.mnuRepeat.Name = "mnuRepeat";
            this.mnuRepeat.Size = new System.Drawing.Size(203, 22);
            this.mnuRepeat.Text = "Repeat";
            this.mnuRepeat.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuAlignActive
            // 
            this.mnuAlignActive.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLeft,
            this.mnuRight,
            this.mnuTop,
            this.mnuBottom});
            this.resourceLookup.SetLookup(this.mnuAlignActive, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuAlignActive", "Align Active", ""));
            this.mnuAlignActive.Name = "mnuAlignActive";
            this.mnuAlignActive.Size = new System.Drawing.Size(203, 22);
            this.mnuAlignActive.Text = "Align Active";
            // 
            // mnuLeft
            // 
            this.resourceLookup.SetLookup(this.mnuLeft, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuLeft", "Left", ""));
            this.mnuLeft.Name = "mnuLeft";
            this.mnuLeft.Size = new System.Drawing.Size(114, 22);
            this.mnuLeft.Text = "Left";
            this.mnuLeft.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuRight
            // 
            this.resourceLookup.SetLookup(this.mnuRight, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuRight", "Right", ""));
            this.mnuRight.Name = "mnuRight";
            this.mnuRight.Size = new System.Drawing.Size(114, 22);
            this.mnuRight.Text = "Right";
            this.mnuRight.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuTop
            // 
            this.resourceLookup.SetLookup(this.mnuTop, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuTop", "Top", ""));
            this.mnuTop.Name = "mnuTop";
            this.mnuTop.Size = new System.Drawing.Size(114, 22);
            this.mnuTop.Text = "Top";
            this.mnuTop.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuBottom
            // 
            this.resourceLookup.SetLookup(this.mnuBottom, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuBottom", "Bottom", ""));
            this.mnuBottom.Name = "mnuBottom";
            this.mnuBottom.Size = new System.Drawing.Size(114, 22);
            this.mnuBottom.Text = "Bottom";
            this.mnuBottom.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuDistributeActive
            // 
            this.mnuDistributeActive.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuVertically,
            this.mnuHorizontally});
            this.resourceLookup.SetLookup(this.mnuDistributeActive, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuDistrActive", "Distribute Active", ""));
            this.mnuDistributeActive.Name = "mnuDistributeActive";
            this.mnuDistributeActive.Size = new System.Drawing.Size(203, 22);
            this.mnuDistributeActive.Text = "Distribute Active";
            // 
            // mnuVertically
            // 
            this.mnuVertically.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuVTogether,
            this.mnuVIncrease,
            this.mnuVDecrease});
            this.resourceLookup.SetLookup(this.mnuVertically, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuVertically", "Vertically", ""));
            this.mnuVertically.Name = "mnuVertically";
            this.mnuVertically.Size = new System.Drawing.Size(141, 22);
            this.mnuVertically.Text = "Vertically";
            // 
            // mnuVTogether
            // 
            this.resourceLookup.SetLookup(this.mnuVTogether, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuTogether", "Together", ""));
            this.mnuVTogether.Name = "mnuVTogether";
            this.mnuVTogether.Size = new System.Drawing.Size(121, 22);
            this.mnuVTogether.Text = "Together";
            this.mnuVTogether.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuVIncrease
            // 
            this.resourceLookup.SetLookup(this.mnuVIncrease, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuIncrease", "Increase", ""));
            this.mnuVIncrease.Name = "mnuVIncrease";
            this.mnuVIncrease.Size = new System.Drawing.Size(121, 22);
            this.mnuVIncrease.Text = "Increase";
            this.mnuVIncrease.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuVDecrease
            // 
            this.resourceLookup.SetLookup(this.mnuVDecrease, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuDecrease", "Decrease", ""));
            this.mnuVDecrease.Name = "mnuVDecrease";
            this.mnuVDecrease.Size = new System.Drawing.Size(121, 22);
            this.mnuVDecrease.Text = "Decrease";
            this.mnuVDecrease.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuHorizontally
            // 
            this.mnuHorizontally.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHTogether,
            this.mnuHIncrease,
            this.mnuHDecrease});
            this.resourceLookup.SetLookup(this.mnuHorizontally, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuHorizontally", "Horizontally ", ""));
            this.mnuHorizontally.Name = "mnuHorizontally";
            this.mnuHorizontally.Size = new System.Drawing.Size(141, 22);
            this.mnuHorizontally.Text = "Horizontally ";
            // 
            // mnuHTogether
            // 
            this.resourceLookup.SetLookup(this.mnuHTogether, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuTogether", "Together", ""));
            this.mnuHTogether.Name = "mnuHTogether";
            this.mnuHTogether.Size = new System.Drawing.Size(121, 22);
            this.mnuHTogether.Text = "Together";
            this.mnuHTogether.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuHIncrease
            // 
            this.resourceLookup.SetLookup(this.mnuHIncrease, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuIncrease", "Increase", ""));
            this.mnuHIncrease.Name = "mnuHIncrease";
            this.mnuHIncrease.Size = new System.Drawing.Size(121, 22);
            this.mnuHIncrease.Text = "Increase";
            this.mnuHIncrease.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuHDecrease
            // 
            this.resourceLookup.SetLookup(this.mnuHDecrease, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuDecrease", "Decrease", ""));
            this.mnuHDecrease.Name = "mnuHDecrease";
            this.mnuHDecrease.Size = new System.Drawing.Size(121, 22);
            this.mnuHDecrease.Text = "Decrease";
            this.mnuHDecrease.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuWidest
            // 
            this.resourceLookup.SetLookup(this.mnuWidest, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuWidest", "Size To Widest", ""));
            this.mnuWidest.Name = "mnuWidest";
            this.mnuWidest.Size = new System.Drawing.Size(203, 22);
            this.mnuWidest.Text = "Size To Widest";
            this.mnuWidest.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuNarrowest
            // 
            this.resourceLookup.SetLookup(this.mnuNarrowest, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuNarrowest", "Size To Narrowest", ""));
            this.mnuNarrowest.Name = "mnuNarrowest";
            this.mnuNarrowest.Size = new System.Drawing.Size(203, 22);
            this.mnuNarrowest.Text = "Size To Narrowest";
            this.mnuNarrowest.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuAutoTabIndex
            // 
            this.resourceLookup.SetLookup(this.mnuAutoTabIndex, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuAutoTabIndex", "Auto Tab Index", ""));
            this.mnuAutoTabIndex.Name = "mnuAutoTabIndex";
            this.mnuAutoTabIndex.Size = new System.Drawing.Size(203, 22);
            this.mnuAutoTabIndex.Text = "Auto Tab Index";
            this.mnuAutoTabIndex.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuSendToBack
            // 
            this.resourceLookup.SetLookup(this.mnuSendToBack, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSendToBack", "Send To Back", ""));
            this.mnuSendToBack.Name = "mnuSendToBack";
            this.mnuSendToBack.Size = new System.Drawing.Size(203, 22);
            this.mnuSendToBack.Text = "Send To Back";
            this.mnuSendToBack.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuBringToFront
            // 
            this.resourceLookup.SetLookup(this.mnuBringToFront, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuBringToFront", "Bring to Front", ""));
            this.mnuBringToFront.Name = "mnuBringToFront";
            this.mnuBringToFront.Size = new System.Drawing.Size(203, 22);
            this.mnuBringToFront.Text = "Bring to Front";
            this.mnuBringToFront.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuResetProperties
            // 
            this.resourceLookup.SetLookup(this.mnuResetProperties, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuResetProps", "Reset Custom Properties", ""));
            this.mnuResetProperties.Name = "mnuResetProperties";
            this.mnuResetProperties.Size = new System.Drawing.Size(203, 22);
            this.mnuResetProperties.Text = "Reset Custom Properties";
            this.mnuResetProperties.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuResetCPos
            // 
            this.resourceLookup.SetLookup(this.mnuResetCPos, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuResetCPos", "Reset Control Position", ""));
            this.mnuResetCPos.Name = "mnuResetCPos";
            this.mnuResetCPos.Size = new System.Drawing.Size(203, 22);
            this.mnuResetCPos.Text = "Reset Control Position";
            this.mnuResetCPos.Visible = false;
            this.mnuResetCPos.Click += new System.EventHandler(this.mnuResetCPos_Click);
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuOpen,
            this.menuItem27,
            this.mnuSave,
            this.mnuSaveAs,
            this.menuItem7,
            this.mnuPreview,
            this.mnuSpFav,
            this.mnuFav1,
            this.mnuFav2,
            this.mnuFav3,
            this.mnuFav4,
            this.mnuFav5,
            this.mnuFav6,
            this.mnuFav7,
            this.mnuFav8,
            this.mnuFav9});
            this.resourceLookup.SetLookup(this.mnuFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuFile", "&File", ""));
            this.mnuFile.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.mnuFile.MergeIndex = 0;
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuNew
            // 
            this.resourceLookup.SetLookup(this.mnuNew, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuNew", "&New", ""));
            this.mnuNew.Name = "mnuNew";
            this.mnuNew.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.mnuNew.Size = new System.Drawing.Size(232, 22);
            this.mnuNew.Text = "&New";
            this.mnuNew.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuOpen
            // 
            this.resourceLookup.SetLookup(this.mnuOpen, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuOpen", "&Open", ""));
            this.mnuOpen.Name = "mnuOpen";
            this.mnuOpen.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.mnuOpen.Size = new System.Drawing.Size(232, 22);
            this.mnuOpen.Text = "&Open";
            this.mnuOpen.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // menuItem27
            // 
            this.menuItem27.Name = "menuItem27";
            this.menuItem27.Size = new System.Drawing.Size(229, 6);
            // 
            // mnuSave
            // 
            this.resourceLookup.SetLookup(this.mnuSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSave", "&Save", ""));
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.mnuSave.Size = new System.Drawing.Size(232, 22);
            this.mnuSave.Text = "&Save";
            this.mnuSave.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuSaveAs
            // 
            this.resourceLookup.SetLookup(this.mnuSaveAs, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSaveAs", "Save &As", ""));
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(232, 22);
            this.mnuSaveAs.Text = "Save &As";
            this.mnuSaveAs.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Name = "menuItem7";
            this.menuItem7.Size = new System.Drawing.Size(229, 6);
            // 
            // mnuPreview
            // 
            this.resourceLookup.SetLookup(this.mnuPreview, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuPreview", "&Preview", ""));
            this.mnuPreview.Name = "mnuPreview";
            this.mnuPreview.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.mnuPreview.Size = new System.Drawing.Size(232, 22);
            this.mnuPreview.Text = "&Preview";
            this.mnuPreview.Click += new System.EventHandler(this.mnuPreview_Click);
            // 
            // mnuSpFav
            // 
            this.mnuSpFav.Name = "mnuSpFav";
            this.mnuSpFav.Size = new System.Drawing.Size(229, 6);
            this.mnuSpFav.Visible = false;
            // 
            // mnuFav1
            // 
            this.mnuFav1.Name = "mnuFav1";
            this.mnuFav1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.mnuFav1.Size = new System.Drawing.Size(232, 22);
            this.mnuFav1.Text = "&1 [FORM1] Form1 Title";
            this.mnuFav1.Visible = false;
            this.mnuFav1.Click += new System.EventHandler(this.mnuLoadFavorite);
            // 
            // mnuFav2
            // 
            this.mnuFav2.Name = "mnuFav2";
            this.mnuFav2.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.mnuFav2.Size = new System.Drawing.Size(232, 22);
            this.mnuFav2.Text = "2& [FORM1] Form1 Title";
            this.mnuFav2.Visible = false;
            this.mnuFav2.Click += new System.EventHandler(this.mnuLoadFavorite);
            // 
            // mnuFav3
            // 
            this.mnuFav3.Name = "mnuFav3";
            this.mnuFav3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.mnuFav3.Size = new System.Drawing.Size(232, 22);
            this.mnuFav3.Text = "3& [FORM1] Form1 Title";
            this.mnuFav3.Visible = false;
            this.mnuFav3.Click += new System.EventHandler(this.mnuLoadFavorite);
            // 
            // mnuFav4
            // 
            this.mnuFav4.Name = "mnuFav4";
            this.mnuFav4.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.mnuFav4.Size = new System.Drawing.Size(232, 22);
            this.mnuFav4.Text = "4& [FORM1] Form1 Title";
            this.mnuFav4.Visible = false;
            this.mnuFav4.Click += new System.EventHandler(this.mnuLoadFavorite);
            // 
            // mnuFav5
            // 
            this.mnuFav5.Name = "mnuFav5";
            this.mnuFav5.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D5)));
            this.mnuFav5.Size = new System.Drawing.Size(232, 22);
            this.mnuFav5.Text = "5& [FORM1] Form1 Title";
            this.mnuFav5.Visible = false;
            this.mnuFav5.Click += new System.EventHandler(this.mnuLoadFavorite);
            // 
            // mnuFav6
            // 
            this.mnuFav6.Name = "mnuFav6";
            this.mnuFav6.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D6)));
            this.mnuFav6.Size = new System.Drawing.Size(232, 22);
            this.mnuFav6.Text = "6& [FORM1] Form1 Title";
            this.mnuFav6.Visible = false;
            this.mnuFav6.Click += new System.EventHandler(this.mnuLoadFavorite);
            // 
            // mnuFav7
            // 
            this.mnuFav7.Name = "mnuFav7";
            this.mnuFav7.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D7)));
            this.mnuFav7.Size = new System.Drawing.Size(232, 22);
            this.mnuFav7.Text = "7& [FORM1] Form1 Title";
            this.mnuFav7.Visible = false;
            this.mnuFav7.Click += new System.EventHandler(this.mnuLoadFavorite);
            // 
            // mnuFav8
            // 
            this.mnuFav8.Name = "mnuFav8";
            this.mnuFav8.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D8)));
            this.mnuFav8.Size = new System.Drawing.Size(232, 22);
            this.mnuFav8.Text = "8& [FORM1] Form1 Title";
            this.mnuFav8.Visible = false;
            this.mnuFav8.Click += new System.EventHandler(this.mnuLoadFavorite);
            // 
            // mnuFav9
            // 
            this.mnuFav9.Name = "mnuFav9";
            this.mnuFav9.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D9)));
            this.mnuFav9.Size = new System.Drawing.Size(232, 22);
            this.mnuFav9.Text = "9& [FORM1] Form1 Title";
            this.mnuFav9.Visible = false;
            this.mnuFav9.Click += new System.EventHandler(this.mnuLoadFavorite);
            // 
            // mnuEdit
            // 
            this.mnuEdit.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUndo,
            this.menuItem12,
            this.mnuSelectAll,
            this.menuItem21,
            this.mnuCut,
            this.mnuCopy,
            this.mnuPaste,
            this.menuItem25,
            this.mnuDelete,
            this.menuItem19,
            this.mnuPageRefresh});
            this.resourceLookup.SetLookup(this.mnuEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuEdit", "&Edit", ""));
            this.mnuEdit.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnuEdit.MergeIndex = 1;
            this.mnuEdit.Name = "mnuEdit";
            this.mnuEdit.Size = new System.Drawing.Size(39, 20);
            this.mnuEdit.Text = "&Edit";
            this.mnuEdit.DropDownOpened += new System.EventHandler(this.mnuEdit_Popup);
            // 
            // mnuUndo
            // 
            this.mnuUndo.Enabled = false;
            this.resourceLookup.SetLookup(this.mnuUndo, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuUndo", "&Undo", ""));
            this.mnuUndo.Name = "mnuUndo";
            this.mnuUndo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.mnuUndo.Size = new System.Drawing.Size(164, 22);
            this.mnuUndo.Text = "&Undo";
            this.mnuUndo.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // menuItem12
            // 
            this.menuItem12.Name = "menuItem12";
            this.menuItem12.Size = new System.Drawing.Size(161, 6);
            // 
            // mnuSelectAll
            // 
            this.resourceLookup.SetLookup(this.mnuSelectAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSelectAll", "Select &All", ""));
            this.mnuSelectAll.Name = "mnuSelectAll";
            this.mnuSelectAll.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.mnuSelectAll.Size = new System.Drawing.Size(164, 22);
            this.mnuSelectAll.Text = "Select &All";
            this.mnuSelectAll.Click += new System.EventHandler(this.mnuSelectAll_Click);
            // 
            // menuItem21
            // 
            this.menuItem21.Name = "menuItem21";
            this.menuItem21.Size = new System.Drawing.Size(161, 6);
            // 
            // mnuCut
            // 
            this.resourceLookup.SetLookup(this.mnuCut, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCut", "Cu&t", ""));
            this.mnuCut.Name = "mnuCut";
            this.mnuCut.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.mnuCut.Size = new System.Drawing.Size(164, 22);
            this.mnuCut.Text = "Cu&t";
            this.mnuCut.Click += new System.EventHandler(this.mnuCut_Click);
            // 
            // mnuCopy
            // 
            this.resourceLookup.SetLookup(this.mnuCopy, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCopy", "&Copy", ""));
            this.mnuCopy.Name = "mnuCopy";
            this.mnuCopy.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.mnuCopy.Size = new System.Drawing.Size(164, 22);
            this.mnuCopy.Text = "&Copy";
            this.mnuCopy.Click += new System.EventHandler(this.mnuCopy_Click);
            // 
            // mnuPaste
            // 
            this.mnuPaste.Enabled = false;
            this.resourceLookup.SetLookup(this.mnuPaste, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuPaste", "&Paste", ""));
            this.mnuPaste.Name = "mnuPaste";
            this.mnuPaste.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.mnuPaste.Size = new System.Drawing.Size(164, 22);
            this.mnuPaste.Text = "&Paste";
            this.mnuPaste.Click += new System.EventHandler(this.mnuPaste_Click);
            // 
            // menuItem25
            // 
            this.menuItem25.Name = "menuItem25";
            this.menuItem25.Size = new System.Drawing.Size(161, 6);
            // 
            // mnuDelete
            // 
            this.resourceLookup.SetLookup(this.mnuDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuDelete", "&Delete", ""));
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.mnuDelete.Size = new System.Drawing.Size(164, 22);
            this.mnuDelete.Text = "&Delete";
            this.mnuDelete.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // menuItem19
            // 
            this.menuItem19.Name = "menuItem19";
            this.menuItem19.Size = new System.Drawing.Size(161, 6);
            // 
            // mnuPageRefresh
            // 
            this.resourceLookup.SetLookup(this.mnuPageRefresh, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuPageRefresh", "Page &Refresh", ""));
            this.mnuPageRefresh.Name = "mnuPageRefresh";
            this.mnuPageRefresh.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.mnuPageRefresh.Size = new System.Drawing.Size(164, 22);
            this.mnuPageRefresh.Text = "Page &Refresh";
            this.mnuPageRefresh.Click += new System.EventHandler(this.ActionMenu_Click);
            // 
            // mnuStyle
            // 
            this.mnuStyle.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuStandardMode,
            this.mnuWizardMode});
            this.resourceLookup.SetLookup(this.mnuStyle, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuStyle", "&Style", ""));
            this.mnuStyle.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnuStyle.MergeIndex = 2;
            this.mnuStyle.Name = "mnuStyle";
            this.mnuStyle.Size = new System.Drawing.Size(44, 20);
            this.mnuStyle.Text = "&Style";
            // 
            // mnuStandardMode
            // 
            this.mnuStandardMode.Checked = true;
            this.mnuStandardMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resourceLookup.SetLookup(this.mnuStandardMode, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuStandardMode", "&Standard Mode", ""));
            this.mnuStandardMode.MergeIndex = 1;
            this.mnuStandardMode.Name = "mnuStandardMode";
            this.mnuStandardMode.Size = new System.Drawing.Size(155, 22);
            this.mnuStandardMode.Text = "&Standard Mode";
            this.mnuStandardMode.Click += new System.EventHandler(this.StandardMode_Click);
            // 
            // mnuWizardMode
            // 
            this.resourceLookup.SetLookup(this.mnuWizardMode, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuWizardMode", "&Wizard Mode", ""));
            this.mnuWizardMode.MergeIndex = 1;
            this.mnuWizardMode.Name = "mnuWizardMode";
            this.mnuWizardMode.Size = new System.Drawing.Size(155, 22);
            this.mnuWizardMode.Text = "&Wizard Mode";
            this.mnuWizardMode.Click += new System.EventHandler(this.WizardMode_Click);
            // 
            // mnuTools
            // 
            this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDebug,
            this.mnuShowUndoList,
            this.mnuShowClipboard});
            this.resourceLookup.SetLookup(this.mnuTools, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuTools", "&Tools", ""));
            this.mnuTools.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnuTools.MergeIndex = 3;
            this.mnuTools.Name = "mnuTools";
            this.mnuTools.Size = new System.Drawing.Size(46, 20);
            this.mnuTools.Text = "&Tools";
            // 
            // mnuDebug
            // 
            this.resourceLookup.SetLookup(this.mnuDebug, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuDebug", "&Debug", ""));
            this.mnuDebug.Name = "mnuDebug";
            this.mnuDebug.Size = new System.Drawing.Size(158, 22);
            this.mnuDebug.Text = "&Debug";
            this.mnuDebug.Click += new System.EventHandler(this.mnuDebug_Click);
            // 
            // mnuShowUndoList
            // 
            this.resourceLookup.SetLookup(this.mnuShowUndoList, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuShowUndoList", "Show &Undo List", ""));
            this.mnuShowUndoList.Name = "mnuShowUndoList";
            this.mnuShowUndoList.Size = new System.Drawing.Size(158, 22);
            this.mnuShowUndoList.Text = "Show &Undo List";
            this.mnuShowUndoList.Click += new System.EventHandler(this.mnuShowUndoList_Click);
            // 
            // mnuShowClipboard
            // 
            this.resourceLookup.SetLookup(this.mnuShowClipboard, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuShowClipbrd", "Show &Clipboard", ""));
            this.mnuShowClipboard.Name = "mnuShowClipboard";
            this.mnuShowClipboard.Size = new System.Drawing.Size(158, 22);
            this.mnuShowClipboard.Text = "Show &Clipboard";
            this.mnuShowClipboard.Click += new System.EventHandler(this.mnuShowClipboard_Click);
            // 
            // mnuVersioning
            // 
            this.mnuVersioning.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCheckin,
            this.mnuCompare});
            this.resourceLookup.SetLookup(this.mnuVersioning, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuVersioning", "&Version Control", ""));
            this.mnuVersioning.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.mnuVersioning.MergeIndex = 4;
            this.mnuVersioning.Name = "mnuVersioning";
            this.mnuVersioning.Size = new System.Drawing.Size(100, 20);
            this.mnuVersioning.Text = "&Version Control";
            // 
            // mnuCheckin
            // 
            this.mnuCheckin.Checked = true;
            this.mnuCheckin.CheckState = System.Windows.Forms.CheckState.Checked;
            this.resourceLookup.SetLookup(this.mnuCheckin, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCheckin", "&Check in", ""));
            this.mnuCheckin.MergeIndex = 1;
            this.mnuCheckin.Name = "mnuCheckin";
            this.mnuCheckin.Size = new System.Drawing.Size(194, 22);
            this.mnuCheckin.Text = "&Check in";
            this.mnuCheckin.Click += new System.EventHandler(this.Checkin_Click);
            // 
            // mnuCompare
            // 
            this.resourceLookup.SetLookup(this.mnuCompare, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCompare", "Version &Administration", ""));
            this.mnuCompare.MergeIndex = 1;
            this.mnuCompare.Name = "mnuCompare";
            this.mnuCompare.Size = new System.Drawing.Size(194, 22);
            this.mnuCompare.Text = "Version &Administration";
            this.mnuCompare.Click += new System.EventHandler(this.Compare_Click);
            // 
            // tpEnquiryHeader
            // 
            this.tpEnquiryHeader.BackColor = System.Drawing.Color.White;
            this.tpEnquiryHeader.Controls.Add(this.pgMain);
            this.tpEnquiryHeader.Controls.Add(this.splitter2);
            this.tpEnquiryHeader.Controls.Add(this.pnlActions);
            this.tpEnquiryHeader.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup.SetLookup(this.tpEnquiryHeader, new FWBS.OMS.UI.Windows.ResourceLookupItem("MAIN", "Main", ""));
            this.tpEnquiryHeader.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpEnquiryHeader.Name = "tpEnquiryHeader";
            this.tpEnquiryHeader.Size = new System.Drawing.Size(284, 461);
            this.tpEnquiryHeader.TabIndex = 4;
            this.tpEnquiryHeader.Text = "Main";
            // 
            // pgMain
            // 
            this.pgMain.BackColor = System.Drawing.Color.White;
            this.pgMain.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgMain.HelpBackColor = System.Drawing.Color.White;
            this.pgMain.HelpVisible = false;
            this.pgMain.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.pgMain.Location = new System.Drawing.Point(0, 0);
            this.pgMain.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pgMain.Name = "pgMain";
            this.pgMain.SelectedObject = this._header;
            this.pgMain.Size = new System.Drawing.Size(284, 411);
            this.pgMain.TabIndex = 0;
            this.pgMain.ToolbarVisible = false;
            this.pgMain.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgMain_PropertyValueChanged);
            this.pgMain.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.pgMain_SelectedGridItemChanged);
            this.pgMain.SelectedObjectsChanged += new System.EventHandler(this.pgMain_SelectedObjectsChanged);
            // 
            // _header
            // 
            this._header.Dirty += new System.EventHandler(this._header_Dirty);
            this._header.MarginChange += new System.EventHandler(this._header_MarginChange);
            this._header.DataBuilderChange += new System.EventHandler(this._header_DataBuilderChange);
            this._header.StandardSizeChange += new System.EventHandler(this._header_StandardSizeChange);
            this._header.WizardSizeChange += new System.EventHandler(this._header_WizardSizeChange);
            this._header.WelcomeTextCodeChange += new System.EventHandler(this._header_WelcomeHeaderCodeChange);
            this._header.WelcomeHeaderCodeChange += new System.EventHandler(this._header_WelcomeHeaderCodeChange);
            this._header.ScriptChange += new System.EventHandler(this._header_ScriptChange);
            this._header.WelcomePageImageChange += new System.EventHandler(this._header_WizardImageChange);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 411);
            this.splitter2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(284, 3);
            this.splitter2.TabIndex = 205;
            this.splitter2.TabStop = false;
            // 
            // pnlActions
            // 
            this.pnlActions.BorderLine = true;
            this.pnlActions.Controls.Add(this.lnkInsert);
            this.pnlActions.Controls.Add(this.lnkRemovePage);
            this.pnlActions.Controls.Add(this.lnkAddPage);
            this.pnlActions.Controls.Add(this.lnkRegister);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Location = new System.Drawing.Point(0, 414);
            this.pnlActions.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(10, 8, 10, 5);
            this.pnlActions.Size = new System.Drawing.Size(284, 47);
            this.pnlActions.TabIndex = 204;
            // 
            // lnkInsert
            // 
            this.lnkInsert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lnkInsert.Enabled = false;
            this.lnkInsert.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkInsert.Location = new System.Drawing.Point(102, 8);
            this.lnkInsert.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lnkInsert.Name = "lnkInsert";
            this.lnkInsert.Size = new System.Drawing.Size(80, 17);
            this.lnkInsert.TabIndex = 121;
            this.lnkInsert.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkInsert.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkInsert.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkInsert_LinkClicked);
            // 
            // lnkRemovePage
            // 
            this.lnkRemovePage.Dock = System.Windows.Forms.DockStyle.Right;
            this.lnkRemovePage.Enabled = false;
            this.lnkRemovePage.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkRemovePage.Location = new System.Drawing.Point(182, 8);
            this.resourceLookup.SetLookup(this.lnkRemovePage, new FWBS.OMS.UI.Windows.ResourceLookupItem("DelWizardPage", "Remove Page", ""));
            this.lnkRemovePage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lnkRemovePage.Name = "lnkRemovePage";
            this.lnkRemovePage.Size = new System.Drawing.Size(92, 17);
            this.lnkRemovePage.TabIndex = 120;
            this.lnkRemovePage.TabStop = true;
            this.lnkRemovePage.Text = "Remove Page";
            this.lnkRemovePage.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lnkRemovePage.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkRemovePage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRemovePage_LinkClicked);
            // 
            // lnkAddPage
            // 
            this.lnkAddPage.Dock = System.Windows.Forms.DockStyle.Left;
            this.lnkAddPage.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.lnkAddPage.Location = new System.Drawing.Point(10, 8);
            this.resourceLookup.SetLookup(this.lnkAddPage, new FWBS.OMS.UI.Windows.ResourceLookupItem("AddWizardPage", "Add Page", ""));
            this.lnkAddPage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lnkAddPage.Name = "lnkAddPage";
            this.lnkAddPage.Size = new System.Drawing.Size(92, 17);
            this.lnkAddPage.TabIndex = 119;
            this.lnkAddPage.TabStop = true;
            this.lnkAddPage.Text = "Add Page";
            this.lnkAddPage.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lnkAddPage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddPage_LinkClicked);
            // 
            // lnkRegister
            // 
            this.lnkRegister.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lnkRegister.Location = new System.Drawing.Point(10, 25);
            this.resourceLookup.SetLookup(this.lnkRegister, new FWBS.OMS.UI.Windows.ResourceLookupItem("RegisterEnqForm", "Register Enquiry Form", ""));
            this.lnkRegister.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lnkRegister.Name = "lnkRegister";
            this.lnkRegister.Size = new System.Drawing.Size(264, 17);
            this.lnkRegister.TabIndex = 122;
            this.lnkRegister.TabStop = true;
            this.lnkRegister.Text = "Register Enquiry Form";
            this.lnkRegister.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lnkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegister_LinkClicked);
            // 
            // tpCustom
            // 
            this.tpCustom.Controls.Add(this.propertyGrid1);
            this.tpCustom.Controls.Add(this.label1);
            this.tpCustom.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup.SetLookup(this.tpCustom, new FWBS.OMS.UI.Windows.ResourceLookupItem("ADVANCED", "Advanced", ""));
            this.tpCustom.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpCustom.Name = "tpCustom";
            this.tpCustom.Size = new System.Drawing.Size(284, 461);
            this.tpCustom.TabIndex = 5;
            this.tpCustom.Text = "Advanced";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 16);
            this.propertyGrid1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(284, 445);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Maroon;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("AdvancedProps", "Advanced Properties", ""));
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(284, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Advanced Properties";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tpCode
            // 
            this.tpCode.BackColor = System.Drawing.Color.White;
            this.tpCode.Controls.Add(this.labNotSaved);
            this.tpCode.Controls.Add(this.dgEvents);
            this.tpCode.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup.SetLookup(this.tpCode, new FWBS.OMS.UI.Windows.ResourceLookupItem("CODE", "Code", ""));
            this.tpCode.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tpCode.Name = "tpCode";
            this.tpCode.Size = new System.Drawing.Size(284, 461);
            this.tpCode.TabIndex = 6;
            this.tpCode.Text = "Code";
            // 
            // labNotSaved
            // 
            this.labNotSaved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labNotSaved.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup.SetLookup(this.labNotSaved, new FWBS.OMS.UI.Windows.ResourceLookupItem("labFormNotSaved", "Please Save the Form before adding any script", ""));
            this.labNotSaved.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labNotSaved.Name = "labNotSaved";
            this.labNotSaved.Size = new System.Drawing.Size(284, 461);
            this.labNotSaved.TabIndex = 1;
            this.labNotSaved.Text = "Please Save the Form before adding any script";
            this.labNotSaved.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgEvents
            // 
            this.dgEvents.BackColor = System.Drawing.Color.White;
            this.dgEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgEvents.Location = new System.Drawing.Point(0, 0);
            this.dgEvents.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.dgEvents.Name = "dgEvents";
            this.dgEvents.Padding = new System.Windows.Forms.Padding(1);
            this.dgEvents.Size = new System.Drawing.Size(284, 461);
            this.dgEvents.TabIndex = 0;
            this.dgEvents.NewScript += new System.EventHandler(this.dgEvents_NewScript);
            this.dgEvents.CodeButtonClick += new System.EventHandler(this.Codebuilder);
            // 
            // mnuLineUp
            // 
            this.resourceLookup.SetLookup(this.mnuLineUp, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuLineUp", "Line Up Icons", ""));
            this.mnuLineUp.Name = "mnuLineUp";
            this.mnuLineUp.Size = new System.Drawing.Size(145, 22);
            this.mnuLineUp.Text = "Line Up Icons";
            this.mnuLineUp.Click += new System.EventHandler(this.mnuLineUp_Click);
            // 
            // imgTools
            // 
            this.imgTools.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgTools.ImageStream")));
            this.imgTools.TransparentColor = System.Drawing.Color.Red;
            this.imgTools.Images.SetKeyName(0, "");
            this.imgTools.Images.SetKeyName(1, "");
            this.imgTools.Images.SetKeyName(2, "");
            this.imgTools.Images.SetKeyName(3, "");
            this.imgTools.Images.SetKeyName(4, "");
            this.imgTools.Images.SetKeyName(5, "");
            this.imgTools.Images.SetKeyName(6, "");
            this.imgTools.Images.SetKeyName(7, "");
            this.imgTools.Images.SetKeyName(8, "");
            this.imgTools.Images.SetKeyName(9, "");
            this.imgTools.Images.SetKeyName(10, "");
            this.imgTools.Images.SetKeyName(11, "");
            // 
            // imgOMSTools
            // 
            this.imgOMSTools.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgOMSTools.ImageStream")));
            this.imgOMSTools.TransparentColor = System.Drawing.Color.Red;
            this.imgOMSTools.Images.SetKeyName(0, "");
            this.imgOMSTools.Images.SetKeyName(1, "");
            this.imgOMSTools.Images.SetKeyName(2, "");
            this.imgOMSTools.Images.SetKeyName(3, "");
            this.imgOMSTools.Images.SetKeyName(4, "");
            this.imgOMSTools.Images.SetKeyName(5, "");
            this.imgOMSTools.Images.SetKeyName(6, "");
            this.imgOMSTools.Images.SetKeyName(7, "");
            this.imgOMSTools.Images.SetKeyName(8, "");
            this.imgOMSTools.Images.SetKeyName(9, "");
            this.imgOMSTools.Images.SetKeyName(10, "");
            this.imgOMSTools.Images.SetKeyName(11, "");
            this.imgOMSTools.Images.SetKeyName(12, "");
            this.imgOMSTools.Images.SetKeyName(13, "");
            this.imgOMSTools.Images.SetKeyName(14, "");
            this.imgOMSTools.Images.SetKeyName(15, "");
            this.imgOMSTools.Images.SetKeyName(16, "");
            this.imgOMSTools.Images.SetKeyName(17, "UserControl.bmp");
            // 
            // pnlCanvass
            // 
            this.pnlCanvass.BorderStyle = FWBS.OMS.UI.Windows.ThreeDBorder3DStyle.Flat;
            this.pnlCanvass.Controls.Add(this.pnlNavigation);
            this.pnlCanvass.Controls.Add(this.pnlWelcome);
            this.pnlCanvass.Controls.Add(this.pnlEnquiry);
            this.pnlCanvass.Controls.Add(this.enquiryForm1);
            this.pnlCanvass.Location = new System.Drawing.Point(1, 1);
            this.pnlCanvass.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlCanvass.Name = "pnlCanvass";
            this.pnlCanvass.Padding = new System.Windows.Forms.Padding(2);
            this.pnlCanvass.Size = new System.Drawing.Size(506, 394);
            this.pnlCanvass.TabIndex = 5;
            this.pnlCanvass.ThreeDBorderStyle = FWBS.OMS.UI.Windows.ThreeDBorder3DStyle.Flat;
            this.pnlCanvass.Visible = false;
            this.pnlCanvass.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlCanvass_Paint);
            this.pnlCanvass.Resize += new System.EventHandler(this.pnlCanvass_Resize);
            // 
            // pnlNavigation
            // 
            this.pnlNavigation.BackColor = System.Drawing.SystemColors.Control;
            this.pnlNavigation.BorderSide = FWBS.OMS.UI.Windows.ThreeDBorder3DSide.Top;
            this.pnlNavigation.BorderStyle = FWBS.OMS.UI.Windows.ThreeDBorder3DStyle.Etched;
            this.pnlNavigation.Controls.Add(this.btnCancel);
            this.pnlNavigation.Controls.Add(this.btnFinished);
            this.pnlNavigation.Controls.Add(this.btnNext);
            this.pnlNavigation.Controls.Add(this.btnBack);
            this.pnlNavigation.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlNavigation.Location = new System.Drawing.Point(2, 354);
            this.pnlNavigation.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlNavigation.Name = "pnlNavigation";
            this.pnlNavigation.Padding = new System.Windows.Forms.Padding(2);
            this.pnlNavigation.Size = new System.Drawing.Size(502, 38);
            this.pnlNavigation.TabIndex = 1;
            // 
            // pnlWelcome
            // 
            this.pnlWelcome.BackColor = System.Drawing.Color.White;
            this.pnlWelcome.Controls.Add(this.labContinue);
            this.pnlWelcome.Controls.Add(this.picWelcome);
            this.pnlWelcome.Controls.Add(this.labDescription);
            this.pnlWelcome.Controls.Add(this.labWelcome);
            this.pnlWelcome.Controls.Add(this.panel3);
            this.pnlWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWelcome.Location = new System.Drawing.Point(2, 2);
            this.pnlWelcome.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlWelcome.Name = "pnlWelcome";
            this.pnlWelcome.Size = new System.Drawing.Size(502, 390);
            this.pnlWelcome.TabIndex = 9;
            this.pnlWelcome.Click += new System.EventHandler(this.labWelcome_Click);
            // 
            // picWelcome
            // 
            this.picWelcome.BackColor = System.Drawing.Color.Transparent;
            this.picWelcome.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("picWelcome.BackgroundImage")));
            this.picWelcome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.picWelcome.Dock = System.Windows.Forms.DockStyle.Left;
            this.picWelcome.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picWelcome.Location = new System.Drawing.Point(0, 0);
            this.picWelcome.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.picWelcome.Name = "picWelcome";
            this.picWelcome.Size = new System.Drawing.Size(124, 390);
            this.picWelcome.TabIndex = 6;
            this.picWelcome.TabStop = false;
            this.picWelcome.Visible = false;
            this.picWelcome.Click += new System.EventHandler(this.labWelcome_Click);
            // 
            // labDescription
            // 
            this.labDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labDescription.Location = new System.Drawing.Point(145, 58);
            this.labDescription.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.labDescription.MaxLength = 1;
            this.labDescription.Name = "labDescription";
            this.labDescription.ReadOnly = true;
            this.labDescription.Size = new System.Drawing.Size(333, 283);
            this.labDescription.TabIndex = 5;
            this.labDescription.TabStop = false;
            this.labDescription.Text = "";
            this.labDescription.Click += new System.EventHandler(this.WelcomeTextProperty);
            this.labDescription.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labDescription_MouseUp);
            // 
            // labWelcome
            // 
            this.labWelcome.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labWelcome.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labWelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.labWelcome.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labWelcome.Location = new System.Drawing.Point(145, 15);
            this.labWelcome.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labWelcome.Name = "labWelcome";
            this.labWelcome.Size = new System.Drawing.Size(336, 29);
            this.labWelcome.TabIndex = 2;
            this.labWelcome.Text = "Welcome to %APPNAME% Wizard";
            this.labWelcome.Click += new System.EventHandler(this.WelcomePropety);
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Location = new System.Drawing.Point(144, 57);
            this.panel3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(337, 285);
            this.panel3.TabIndex = 7;
            // 
            // pnlEnquiry
            // 
            this.pnlEnquiry.BackColor = System.Drawing.SystemColors.Control;
            this.pnlEnquiry.Controls.Add(this.pnlWizardPage);
            this.pnlEnquiry.Controls.Add(this.panel2);
            this.pnlEnquiry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEnquiry.Location = new System.Drawing.Point(2, 2);
            this.pnlEnquiry.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlEnquiry.Name = "pnlEnquiry";
            this.pnlEnquiry.Size = new System.Drawing.Size(502, 390);
            this.pnlEnquiry.TabIndex = 8;
            // 
            // pnlWizardPage
            // 
            this.pnlWizardPage.BorderSide = FWBS.OMS.UI.Windows.ThreeDBorder3DSide.Top;
            this.pnlWizardPage.BorderStyle = FWBS.OMS.UI.Windows.ThreeDBorder3DStyle.Etched;
            this.pnlWizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWizardPage.Location = new System.Drawing.Point(0, 70);
            this.pnlWizardPage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlWizardPage.Name = "pnlWizardPage";
            this.pnlWizardPage.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlWizardPage.Size = new System.Drawing.Size(502, 320);
            this.pnlWizardPage.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.labQuestionPage);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(502, 70);
            this.panel2.TabIndex = 0;
            // 
            // labQuestionPage
            // 
            this.labQuestionPage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labQuestionPage.BackColor = System.Drawing.SystemColors.Window;
            this.labQuestionPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labQuestionPage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labQuestionPage.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labQuestionPage.Location = new System.Drawing.Point(10, 10);
            this.labQuestionPage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labQuestionPage.Name = "labQuestionPage";
            this.labQuestionPage.Size = new System.Drawing.Size(421, 48);
            this.labQuestionPage.TabIndex = 1;
            this.labQuestionPage.Text = "Enquiry Page Question Text?";
            this.labQuestionPage.Click += new System.EventHandler(this.labQuestionPage_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.pictureBox1.Location = new System.Drawing.Point(444, 10);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // enquiryForm1
            // 
            this.enquiryForm1.ActionBack = this.btnBack;
            this.enquiryForm1.ActionNext = this.btnNext;
            this.enquiryForm1.AllowDrop = true;
            this.enquiryForm1.AutoScroll = true;
            this.enquiryForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enquiryForm1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.enquiryForm1.IsDirty = false;
            this.enquiryForm1.Location = new System.Drawing.Point(2, 2);
            this.enquiryForm1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.enquiryForm1.Name = "enquiryForm1";
            this.enquiryForm1.PageHeader = this.labQuestionPage;
            this.enquiryForm1.PageHeaderPicture = this.pictureBox1;
            this.enquiryForm1.Size = new System.Drawing.Size(502, 390);
            this.enquiryForm1.TabIndex = 0;
            this.enquiryForm1.ToBeRefreshed = false;
            this.enquiryForm1.WelcomeHeader = this.labWelcome;
            this.enquiryForm1.WelcomePagePicture = this.picWelcome;
            this.enquiryForm1.WelcomeText = this.labDescription;
            this.enquiryForm1.PageChanged += new FWBS.OMS.UI.Windows.PageChangedEventHandler(this.enquiryForm1_PageChanged);
            this.enquiryForm1.Rendering += new System.ComponentModel.CancelEventHandler(this.enquiryForm1_Rendering);
            this.enquiryForm1.DragDrop += new System.Windows.Forms.DragEventHandler(this.enquiryForm1_DragDrop);
            this.enquiryForm1.DragEnter += new System.Windows.Forms.DragEventHandler(this.enquiryForm1_DragEnter);
            this.enquiryForm1.Enter += new System.EventHandler(this.ActivateMenus);
            this.enquiryForm1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.enquiryForm1_MouseDown);
            this.enquiryForm1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.enquiryForm1_MouseMove);
            this.enquiryForm1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.enquiryForm1_MouseUp);
            // 
            // tdpOMS
            // 
            this.tdpOMS.BorderSide = FWBS.OMS.UI.Windows.ThreeDBorder3DSide.Top;
            this.tdpOMS.BorderStyle = FWBS.OMS.UI.Windows.ThreeDBorder3DStyle.Etched;
            this.tdpOMS.Dock = System.Windows.Forms.DockStyle.Top;
            this.tdpOMS.Location = new System.Drawing.Point(0, 23);
            this.tdpOMS.Name = "tdpOMS";
            this.tdpOMS.Padding = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.tdpOMS.Size = new System.Drawing.Size(120, 6);
            this.tdpOMS.TabIndex = 5;
            // 
            // DesignerPopup
            // 
            this.DesignerPopup.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRepeat,
            this.mnuSp6,
            this.mnuAlignActive,
            this.mnuDistributeActive,
            this.mnuWidest,
            this.mnuNarrowest,
            this.mnuAutoTabIndex,
            this.mnuSp8,
            this.mnuSendToBack,
            this.mnuBringToFront,
            this.mnuSp9,
            this.mnuResetProperties,
            this.mnuSp7,
            this.mnuResetCPos});
            this.DesignerPopup.Name = "DesignerPopup";
            this.DesignerPopup.Size = new System.Drawing.Size(204, 248);
            this.DesignerPopup.Opened += new System.EventHandler(this.DesignerPopup_Popup);
            // 
            // mnuSp6
            // 
            this.mnuSp6.Name = "mnuSp6";
            this.mnuSp6.Size = new System.Drawing.Size(200, 6);
            // 
            // mnuSp8
            // 
            this.mnuSp8.Name = "mnuSp8";
            this.mnuSp8.Size = new System.Drawing.Size(200, 6);
            // 
            // mnuSp9
            // 
            this.mnuSp9.Name = "mnuSp9";
            this.mnuSp9.Size = new System.Drawing.Size(200, 6);
            // 
            // mnuSp7
            // 
            this.mnuSp7.Name = "mnuSp7";
            this.mnuSp7.Size = new System.Drawing.Size(200, 6);
            this.mnuSp7.Visible = false;
            // 
            // menuItem5
            // 
            this.menuItem5.Name = "menuItem5";
            this.menuItem5.Size = new System.Drawing.Size(32, 19);
            this.menuItem5.Text = "-";
            // 
            // mnuUnselect
            // 
            this.mnuUnselect.Name = "mnuUnselect";
            this.mnuUnselect.Size = new System.Drawing.Size(32, 19);
            // 
            // mainMenu1
            // 
            this.mainMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuEdit,
            this.mnuStyle,
            this.mnuTools,
            this.mnuVersioning});
            this.mainMenu1.Location = new System.Drawing.Point(0, 0);
            this.mainMenu1.Name = "mainMenu1";
            this.mainMenu1.Size = new System.Drawing.Size(1004, 24);
            this.mainMenu1.TabIndex = 28;
            // 
            // pnlWorkspace
            // 
            this.pnlWorkspace.AutoScroll = true;
            this.pnlWorkspace.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlWorkspace.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlWorkspace.Controls.Add(this.pnlCanvass);
            this.pnlWorkspace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlWorkspace.Location = new System.Drawing.Point(0, 0);
            this.pnlWorkspace.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlWorkspace.Name = "pnlWorkspace";
            this.pnlWorkspace.Size = new System.Drawing.Size(534, 466);
            this.pnlWorkspace.TabIndex = 13;
            this.pnlWorkspace.Click += new System.EventHandler(this.pnlWorkspace_Click);
            // 
            // pnlProperties
            // 
            this.pnlProperties.BackColor = System.Drawing.Color.White;
            this.pnlProperties.Controls.Add(this.pnlPage);
            this.pnlProperties.Controls.Add(this.panel1);
            this.pnlProperties.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlProperties.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlProperties.Location = new System.Drawing.Point(704, 52);
            this.pnlProperties.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(300, 528);
            this.pnlProperties.TabIndex = 6;
            // 
            // pnlPage
            // 
            this.pnlPage.Controls.Add(this.tabPages);
            this.pnlPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPage.Location = new System.Drawing.Point(0, 29);
            this.pnlPage.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlPage.Name = "pnlPage";
            this.pnlPage.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlPage.Size = new System.Drawing.Size(300, 499);
            this.pnlPage.TabIndex = 5;
            // 
            // tabPages
            // 
            this.tabPages.Controls.Add(this.tpEnquiryHeader);
            this.tabPages.Controls.Add(this.tpCustom);
            this.tabPages.Controls.Add(this.tpCode);
            this.tabPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPages.HotTrack = true;
            this.tabPages.Location = new System.Drawing.Point(4, 5);
            this.tabPages.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.tabPages.Name = "tabPages";
            this.tabPages.SelectedIndex = 0;
            this.tabPages.Size = new System.Drawing.Size(292, 489);
            this.tabPages.TabIndex = 114;
            this.tabPages.SelectedIndexChanged += new System.EventHandler(this.tabPages_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbFields);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Size = new System.Drawing.Size(300, 29);
            this.panel1.TabIndex = 7;
            // 
            // cmbFields
            // 
            this.cmbFields.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbFields.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbFields.Location = new System.Drawing.Point(4, 5);
            this.cmbFields.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cmbFields.Name = "cmbFields";
            this.cmbFields.Size = new System.Drawing.Size(292, 23);
            this.cmbFields.TabIndex = 0;
            this.cmbFields.SelectedIndexChanged += new System.EventHandler(this.cmbFields_SelectedIndexChanged);
            this.cmbFields.Enter += new System.EventHandler(this.cmbFields_Enter);
            this.cmbFields.Leave += new System.EventHandler(this.cmbFields_Leave);
            // 
            // menuItem11
            // 
            this.menuItem11.Name = "menuItem11";
            this.menuItem11.Size = new System.Drawing.Size(32, 19);
            this.menuItem11.Text = "Test";
            // 
            // menuItem20
            // 
            this.menuItem20.Name = "menuItem20";
            this.menuItem20.Size = new System.Drawing.Size(32, 19);
            // 
            // menuItem28
            // 
            this.menuItem28.Name = "menuItem28";
            this.menuItem28.Size = new System.Drawing.Size(32, 19);
            this.menuItem28.Text = "-";
            // 
            // pnlPages
            // 
            this.pnlPages.BackColor = System.Drawing.Color.White;
            this.pnlPages.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlPages.Location = new System.Drawing.Point(4, 4);
            this.pnlPages.Name = "pnlPages";
            this.pnlPages.Size = new System.Drawing.Size(192, 39);
            this.pnlPages.TabIndex = 114;
            // 
            // Blue
            // 
            this.Blue.Location = new System.Drawing.Point(232, -27);
            this.Blue.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Blue.Name = "Blue";
            this.Blue.Size = new System.Drawing.Size(100, 22);
            this.Blue.TabIndex = 14;
            this.Blue.Text = "textBox1";
            this.Blue.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Blue_KeyDown);
            this.Blue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Blue_KeyPress);
            // 
            // OpenSaveEnquiry1
            // 
            this.OpenSaveEnquiry1.AllowDelete = true;
            this.OpenSaveEnquiry1.AllowNewFolder = true;
            this.OpenSaveEnquiry1.AllowRename = true;
            // 
            // pnlFields
            // 
            this.pnlFields.BackColor = System.Drawing.Color.White;
            this.pnlFields.Controls.Add(this.lstListFields);
            this.pnlFields.Controls.Add(this.cmbViewFields);
            this.pnlFields.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlFields.Location = new System.Drawing.Point(464, 8);
            this.pnlFields.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Size = new System.Drawing.Size(96, 112);
            this.pnlFields.TabIndex = 17;
            this.pnlFields.Visible = false;
            // 
            // lstListFields
            // 
            this.lstListFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstListFields.IntegralHeight = false;
            this.lstListFields.ItemHeight = 15;
            this.lstListFields.Location = new System.Drawing.Point(0, 23);
            this.lstListFields.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.lstListFields.Name = "lstListFields";
            this.lstListFields.Size = new System.Drawing.Size(96, 89);
            this.lstListFields.TabIndex = 199;
            this.lstListFields.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lstListFields_MouseMove);
            // 
            // cmbViewFields
            // 
            this.cmbViewFields.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbViewFields.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbViewFields.Location = new System.Drawing.Point(0, 0);
            this.cmbViewFields.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cmbViewFields.Name = "cmbViewFields";
            this.cmbViewFields.Size = new System.Drawing.Size(96, 23);
            this.cmbViewFields.TabIndex = 200;
            this.cmbViewFields.Visible = false;
            this.cmbViewFields.SelectionChangeCommitted += new System.EventHandler(this.cmbViewFields_SelectionChangeCommitted);
            // 
            // spRight
            // 
            this.spRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.spRight.Location = new System.Drawing.Point(700, 52);
            this.spRight.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.spRight.Name = "spRight";
            this.spRight.Size = new System.Drawing.Size(4, 528);
            this.spRight.TabIndex = 19;
            this.spRight.TabStop = false;
            // 
            // spLeft
            // 
            this.spLeft.Location = new System.Drawing.Point(164, 52);
            this.spLeft.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.spLeft.Name = "spLeft";
            this.spLeft.Size = new System.Drawing.Size(2, 528);
            this.spLeft.TabIndex = 20;
            this.spLeft.TabStop = false;
            // 
            // frmFieldsListFS
            // 
            this.frmFieldsListFS.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.frmFieldsListFS.UniqueID = "Forms\\FieldList";
            this.frmFieldsListFS.Version = ((long)(0));
            // 
            // pnlComponents
            // 
            this.pnlComponents.AutoScroll = true;
            this.pnlComponents.BackColor = System.Drawing.Color.White;
            this.pnlComponents.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlComponents.ContextMenuStrip = this.mnuComponents;
            this.pnlComponents.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlComponents.Location = new System.Drawing.Point(0, 471);
            this.pnlComponents.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlComponents.Name = "pnlComponents";
            this.pnlComponents.Size = new System.Drawing.Size(534, 57);
            this.pnlComponents.TabIndex = 21;
            this.pnlComponents.Click += new System.EventHandler(this.pnlWorkspace_Click);
            // 
            // mnuComponents
            // 
            this.mnuComponents.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuLineUp});
            this.mnuComponents.Name = "mnuComponents";
            this.mnuComponents.Size = new System.Drawing.Size(146, 26);
            // 
            // splitter5
            // 
            this.splitter5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter5.Location = new System.Drawing.Point(0, 466);
            this.splitter5.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.splitter5.Name = "splitter5";
            this.splitter5.Size = new System.Drawing.Size(534, 5);
            this.splitter5.TabIndex = 23;
            this.splitter5.TabStop = false;
            // 
            // menuCollection1
            // 
            this.menuCollection1.ActiveBar = null;
            this.menuCollection1.AllowDrop = true;
            this.menuCollection1.BackColor = System.Drawing.Color.White;
            this.menuCollection1.ButtonImageList = this.imgOMSTools;
            this.menuCollection1.ButtonStyle = FWBS.Common.UI.Windows.ButtonStyle.mbSmallToggle;
            this.menuCollection1.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuCollection1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuCollection1.Location = new System.Drawing.Point(0, 52);
            this.menuCollection1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.menuCollection1.Name = "menuCollection1";
            this.menuCollection1.Padding = new System.Windows.Forms.Padding(2, 2, 4, 1);
            this.menuCollection1.Size = new System.Drawing.Size(164, 528);
            this.menuCollection1.TabIndex = 24;
            this.menuCollection1.UseLocalisation = false;
            this.menuCollection1.DoubleClick += new FWBS.Common.UI.Windows.ButtonEventHandler(this.menuCollection1_ButtonDoubleClick);
            this.menuCollection1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuCollection1_MouseDown);
            this.menuCollection1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.menuCollection1_MouseMove);
            // 
            // pnlMiddle
            // 
            this.pnlMiddle.Controls.Add(this.pnlWorkspace);
            this.pnlMiddle.Controls.Add(this.splitter5);
            this.pnlMiddle.Controls.Add(this.pnlComponents);
            this.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMiddle.Location = new System.Drawing.Point(166, 52);
            this.pnlMiddle.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Size = new System.Drawing.Size(534, 528);
            this.pnlMiddle.TabIndex = 26;
            // 
            // timLineup
            // 
            this.timLineup.Interval = 500;
            this.timLineup.Tick += new System.EventHandler(this.timLineup_Tick);
            // 
            // statusBar1
            // 
            this.statusBar1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusBar1.Location = new System.Drawing.Point(0, 580);
            this.statusBar1.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.statusBar1.Name = "statusBar1";
            this.statusBar1.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.panCanvas,
            this.panSelect,
            this.panStatus});
            this.statusBar1.ShowPanels = true;
            this.statusBar1.Size = new System.Drawing.Size(1004, 21);
            this.statusBar1.TabIndex = 27;
            this.statusBar1.Text = "Ready...";
            // 
            // panCanvas
            // 
            this.panCanvas.Name = "panCanvas";
            this.panCanvas.Text = "Canvas : ";
            this.panCanvas.Width = 220;
            // 
            // panSelect
            // 
            this.panSelect.Name = "panSelect";
            this.panSelect.Text = "Selected : ";
            this.panSelect.Width = 260;
            // 
            // panStatus
            // 
            this.panStatus.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
            this.panStatus.Name = "panStatus";
            this.panStatus.Text = "Ready...";
            this.panStatus.Width = 507;
            // 
            // timRebuildcmbFields
            // 
            this.timRebuildcmbFields.Interval = 1;
            this.timRebuildcmbFields.Tick += new System.EventHandler(this.timRebuildcmbFields_Tick);
            // 
            // _page
            // 
            this._page.PageHeaderChange += new System.EventHandler(this._page_PageHeaderChange);
            this._page.PageOrderChange += new System.EventHandler(this._page_PageOrderChange);
            this._page.ImageChange += new System.EventHandler(this._page_ImageChange);
            // 
            // pnlMain
            // 
            this.pnlMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlMain.Controls.Add(this.OMSToolbars);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(0, 24);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(5, 0, 5, 1);
            this.pnlMain.Size = new System.Drawing.Size(1004, 28);
            this.pnlMain.TabIndex = 29;
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
            this.OMSToolbars.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.OMSToolbars.Name = "OMSToolbars";
            this.OMSToolbars.NavCommandPanel = null;
            this.OMSToolbars.ShowToolTips = true;
            this.OMSToolbars.Size = new System.Drawing.Size(994, 26);
            this.OMSToolbars.TabIndex = 0;
            this.OMSToolbars.TopDivider = false;
            // 
            // frmDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1004, 601);
            this.Controls.Add(this.pnlFields);
            this.Controls.Add(this.pnlMiddle);
            this.Controls.Add(this.spLeft);
            this.Controls.Add(this.Blue);
            this.Controls.Add(this.menuCollection1);
            this.Controls.Add(this.spRight);
            this.Controls.Add(this.pnlProperties);
            this.Controls.Add(this.statusBar1);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.mainMenu1);
            this.KeyPreview = true;
            this.MainMenuStrip = this.mainMenu1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmDesigner";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Untitled - OMS Designer";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmDesigner_Closing);
            this.Load += new System.EventHandler(this.frmDesigner_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDesigner_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmDesigner_KeyUp);
            this.tpEnquiryHeader.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.tpCustom.ResumeLayout(false);
            this.tpCode.ResumeLayout(false);
            this.pnlCanvass.ResumeLayout(false);
            this.pnlNavigation.ResumeLayout(false);
            this.pnlWelcome.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picWelcome)).EndInit();
            this.pnlEnquiry.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.DesignerPopup.ResumeLayout(false);
            this.mainMenu1.ResumeLayout(false);
            this.mainMenu1.PerformLayout();
            this.pnlWorkspace.ResumeLayout(false);
            this.pnlProperties.ResumeLayout(false);
            this.pnlPage.ResumeLayout(false);
            this.tabPages.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlFields.ResumeLayout(false);
            this.mnuComponents.ResumeLayout(false);
            this.pnlMiddle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panCanvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panSelect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panStatus)).EndInit();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        void _page_ImageChange(object sender, EventArgs e)
        {
            if (pictureBox1.Tag == null)
                pictureBox1.Tag = pictureBox1.Image;

            if (_page.Image != null)
            {
                pictureBox1.Image = _page.Image;
                if (_page.Image.Size.Height > pictureBox1.Size.Height || _page.Image.Size.Width > pictureBox1.Size.Width)
                    pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                else
                    pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            }
            else
            {
                pictureBox1.Image = (Image)pictureBox1.Tag;
            }
        }

        void _header_WizardImageChange(object sender, EventArgs e)
        {
            picWelcome.Image = _header.WelcomePageImage;
            picWelcome.SizeMode = (_header.WelcomePageImage != null) ? PictureBoxSizeMode.StretchImage : PictureBoxSizeMode.Normal;
        }

        private void SetWelcomeBackgroundImage()
        {
            Bitmap welcomeImage = FWBS.OMS.UI.Properties.Resources.V2Wizard_WelcomePageImage;
            if (DeviceDpi != 96)
            {
                ScaleBitmapLogicalToDevice(ref welcomeImage);
            }
            picWelcome.BackgroundImage = welcomeImage;
        }

		#endregion

		#region Constructors e.g. Load, Close
			
		public frmDesigner()
		{
            if (FWBS.OMS.UI.Windows.frmMain.PartnerAccess == true || Session.CurrentSession.IsLicensedFor("SDKALL") == true || Session.CurrentSession.IsLicensedFor("SDKSCD") == true)
			{
				//
				// Required for Windows Form Designer support
				//
				InitializeComponent();
                SetWelcomeBackgroundImage();
                this.OMSToolbars.OMSButtonClick += new FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventHandler(OMSToolbars_OMSButtonClick);

                _statusCanvas = ResourceLookup.GetLookupText("CANVAS", "Canvas", "");
                _statusSelected = ResourceLookup.GetLookupText("SELECTED", "Selected", "");
                _statusReady = ResourceLookup.GetLookupText("INFOREADY", "Ready...", "");
                _statusSaved = ResourceLookup.GetLookupText("INFOSAVED", "Saved...", "");

                if (Session.CurrentSession.IsLicensedFor("SDKALL") == false && FWBS.OMS.UI.Windows.frmMain.PartnerAccess == false)
                    if (Session.CurrentSession.IsLicensedFor("SDKSCD") == true)
                    {
                        this.OMSToolbars.GetButton("tbScript").Group = "";
                        this.OMSToolbars.GetButton("tbScript").Visible = false;
                    }

				// Create the object that manages the docking state
				enquiryForm1.ControlAdded += new ControlEventHandler(this.frmDesigner_ControlAdded);
				enquiryForm1.ControlRemoved += new ControlEventHandler(this.frmDesigner_ControlRemove);
				pnlComponents.ControlRemoved += new ControlEventHandler(this.frmDesigner_ControlRemove);
				pnlComponents.ControlAdded += new ControlEventHandler(frmDesigner_ComponentControlAdded);
				try
				{
					Favourites fav = new Favourites("FAVFORMS","CURRENT");
					if (fav.Count == 0) 
					{
						fav.AddFavourite("CURRENT","","");
						fav.Update();
						fav = new Favourites("FAVFORMS","CURRENT");
					}
			
					_favorites.AddRange(fav.Param1(0).Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries));
				}
				catch
				{}

				frmDesigner_LoadFavForms();
				enquiryForm1.ActionBack = btnBack;
				enquiryForm1.ActionNext = btnNext;
				this.AcceptButton = null;
				tabPages.Controls.Remove(tpCustom);
				pgMain.HelpVisible=true;
				propertyGrid1.HelpVisible=true;

                if (Session.CurrentSession.IsLicensedFor("SDKALL") == false && FWBS.OMS.UI.Windows.frmMain.PartnerAccess == false)
					if (Session.CurrentSession.IsLicensedFor("SDKSCD") == true)
					{
						tabPages.TabPages.Remove(tpCode);
						lnkRegister.Visible = true;
						mnuTools.Visible = false;
                        dgEvents.Visible = false;
					}

                ManageVersioningMenuOptions(false);
			}
			else
			{
				Session.CurrentSession.ValidateLicensedFor("SDKALL");
			}
		}


        private void ManageVersioningMenuOptions(bool state)
        {
            this.mnuCheckin.Enabled = state;
            this.mnuCompare.Enabled = state;
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
                    if (this.enquiryForm1 != null)
                    {
                        if (!string.IsNullOrWhiteSpace(this.enquiryForm1.Code))
                            UnlockCurrentObject(this.enquiryForm1.Code);
                            
                        if (CodeWindow != null)
                        {
                            CodeWindow.Close();
                            CodeWindow.Dispose();
                            CodeWindow = null;
                        }
                    }
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                
            }
            catch { }
            finally
            {
                base.Dispose(disposing);
            }
		}

		/// <summary>
		/// Show Login Dialog and set any Datalists
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmDesigner_Load(object sender, System.EventArgs e)
		{
			// Create a Blank Enquiry Form
			ActionMenu_Click(mnuNew,EventArgs.Empty);
			LoadControlButtons();
			statusBar1.Panels[0].Text = string.Format("{0} : {1}", _statusCanvas, pnlCanvass.Size);
            fieldsForm.Controls.Add(pnlFields, true);
			pnlFields.Dock = DockStyle.Fill;
            pnlFields.Visible = true;
			propertyGrid1.HelpVisible=true;

			//
			// Create the FieldsForm
			//
            fieldsForm.Owner = this;
			frmFieldsListFS.FormToStore = fieldsForm;
			frmFieldsListFS.LoadNow();
            fieldsForm.Closing -= new System.ComponentModel.CancelEventHandler(this.fieldsForm_Closing); 
            fieldsForm.Closing += new System.ComponentModel.CancelEventHandler(this.fieldsForm_Closing);
			fieldsForm.Show();
			UndoTable = _edata.Tables["QUESTIONS"].Clone();
			UndoTable.Columns.Add("GROUP",typeof(String));
			Clipboard = _edata.Tables["QUESTIONS"].Clone();
			UndoTable.Constraints.Clear();
			Clipboard.Constraints.Clear();
			menuCollection1.ActiveBar="General";
		}

        private void fieldsForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (fieldsForm.Tag == null)
			{
                OMSToolbars.GetButton("tbFields").Pushed = false;
                e.Cancel = true; 
                fieldsForm.Hide();
            }
		}

		/// <summary>
		/// Unselect All Save config and get the hell out of here.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmDesigner_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
            if (processing)
            {
                e.Cancel = true;
                return;
            }
            try
			{
				if (IsFormDirty() == false)
				{
					Start.Restart=false;
					e.Cancel=true;
					return;
				}
				cmbFields.DataSource = null;
				_controls.Clear();
				timRebuildcmbFields.Interval = 999999999;
				pnlComponents.ControlRemoved -= new ControlEventHandler(this.frmDesigner_ControlRemove);
				pnlComponents.ControlAdded -=new ControlEventHandler(frmDesigner_ComponentControlAdded);
				Favourites fav = new Favourites("FAVFORMS","CURRENT");
				if (fav.Count == 0) 
				{
					fav.AddFavourite("CURRENT","","");
					fav.Update();
					fav = new Favourites("FAVFORMS","CURRENT");
				}
				fav.Param1(0, string.Join("|", _favorites.ToArray()));
				fav.Update();

                if(!string.IsNullOrWhiteSpace(OpenSaveEnquiry1.Code))
                UnlockCurrentObject(OpenSaveEnquiry1.Code);
			}
			catch
			{}
			
			frmFieldsListFS.SaveNow();

			_updating = true;
			fieldsForm.Tag = 1;
			fieldsForm.Close();
			if (CodeWindow != null)
				CodeWindow.Close();

            this.OMSToolbars.OMSButtonClick -= new FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventHandler(OMSToolbars_OMSButtonClick);

            if (sender is Session)
            {
                this.Closing -= new System.ComponentModel.CancelEventHandler(this.frmDesigner_Closing);
                this.Close();
            }

		}
		#endregion

		#region Properties
		public long EnquiryFormVersion
		{
			get
			{
				if (enquiryForm1 != null && enquiryForm1.Enquiry != null)
				{
					return enquiryForm1.Enquiry.Version;
				}
				else
				{
					return -1;
				}
			}
		}

		public bool IsDirty
		{
			get
			{
				return _isdirty;
			}
			set
			{
				_isdirty = value;
				if (_isdirty)
				{
					statusBar1.Panels[2].Text = string.Format("{0}...", _dpiWarning ? ResourceLookup.GetLookupText("DPIMODIFIED", "Modified on High DPI", "") : ResourceLookup.GetLookupText("MODIFIED", "Modified", ""));
				}
				else
				{
					statusBar1.Panels[2].Text = _statusReady;
				}
			}
		}
		
		public bool IsFormDirty()
		{
            DialogResult dr = DialogResult.None;
            if (IsDirty || CodeWindow != null && CodeWindow.IsDirty)
			{
                if (_dpiWarning)
                    dr = FWBS.OMS.UI.Windows.MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("DIRTYDATADPI", "Changes made on High DPI have been detected, but they cannot be saved.", "").Text, Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                else
                    dr = FWBS.OMS.UI.Windows.MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?", "").Text, Text, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes && CodeWindow != null && CodeWindow.IsDirty)
                {
                    if (!CodeWindow.SaveAndCompile())
                        return false;
                }
				if (dr == DialogResult.Yes && IsDirty) ActionMenu_Click(mnuSave,EventArgs.Empty);
				if (dr == DialogResult.Cancel) return false;
			}		
			return true;
		}
		
		#endregion

		#region Private Methods Favorites
		private void timLineup_Tick(object sender, System.EventArgs e)
		{
			timLineup.Enabled = false;
			mnuLineUp_Click(sender,e);
		}

		private void frmDesigner_ComponentControlAdded(object sender, ControlEventArgs e)
		{
			timLineup.Enabled=true;
		}

		private void frmDesigner_RemoveFavItem(string code)
		{
			for(int i = _favorites.Count-1; i > -1; i--)
				if (Convert.ToString(_favorites[i]) == code)
					_favorites.Remove(_favorites[i]);
		}

		private void frmDesigner_LoadFavForms()
		{
			int i =0;
			ToolStripMenuItem[] mnuFavs = new ToolStripMenuItem[9]{mnuFav1,mnuFav2,mnuFav3,mnuFav4,mnuFav5,mnuFav6,mnuFav7,mnuFav8,mnuFav9};
			foreach(ToolStripMenuItem stfav in mnuFavs)
				stfav.Visible=false;
			string description = "";
			foreach(string stfav in _favorites)
			{
				if (stfav != "")
				{
					description = CodeLookup.GetLookup("ENQHEADER",stfav);
					if (description == "") description = "[" + stfav + "]";
					mnuSpFav.Visible=true;
					mnuFavs[i].Text = "&" + (i + 1).ToString() + " " + description;
					mnuFavs[i].Visible=true;
					i++;
				}
			}
		}

		
		private void mnuLoadFavorite(object sender, System.EventArgs e)
		{
            if (processing) 
                return;

            if (IsFormDirty())
            {
                string code = "";
                if (sender == mnuFav1) code = Convert.ToString(_favorites[0]);
                if (sender == mnuFav2) code = Convert.ToString(_favorites[1]);
                if (sender == mnuFav3) code = Convert.ToString(_favorites[2]);
                if (sender == mnuFav4) code = Convert.ToString(_favorites[3]);
                if (sender == mnuFav5) code = Convert.ToString(_favorites[4]);
                if (sender == mnuFav6) code = Convert.ToString(_favorites[5]);
                if (sender == mnuFav7) code = Convert.ToString(_favorites[6]);
                if (sender == mnuFav8) code = Convert.ToString(_favorites[7]);
                if (sender == mnuFav9) code = Convert.ToString(_favorites[8]);
                if (code != "")
                {
                    OpenSaveEnquiry1.Code = code;
                    this.Cursor = Cursors.WaitCursor;
                    try
                    {
                        if (Session.CurrentSession.ObjectLocking)
                        {
                            if (!ls.CheckObjectLockState(code, FWBS.OMS.UI.Windows.LockableObjects.EnquiryForm))
                            {
                                if(!string.IsNullOrWhiteSpace(_currentform))
                                    UnlockCurrentObject(_currentform);

                                LockEnquiryForm(code);
                                ls.MarkObjectAsOpen(code, UI.Windows.LockableObjects.EnquiryForm);
                                OpenEnquiryFormInDesigner(code);
                                alreadylocked = true;
                            }
                            else
                                OpenSaveEnquiry1.Code = "";
                        }
                        else
                            OpenEnquiryFormInDesigner(code);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
		}

        private void OpenEnquiryFormInDesigner(string code)
        {
            frmDesigner_RemoveFavItem(OpenSaveEnquiry1.Code);
            _favorites.Insert(0, OpenSaveEnquiry1.Code);
            if (_favorites.Count > _maxFavoritesCount) _favorites.RemoveAt(_maxFavoritesCount - 1);
            frmDesigner_LoadFavForms();
            frmDesigner_CleanUp();
            frmDesigner_Initialize(true, OpenSaveEnquiry1.Code, OpenSaveEnquiry1.Folder, "");
            ManageVersioningMenuOptions(true);
        }
		#endregion

		#region Private Methods
		private void labDescription_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			WelcomeTextProperty(sender,EventArgs.Empty);
		}

		/// <summary>
		/// Reconnects the objects after the save to stop Concurrency Errors
		/// </summary>
		private void LoadAfterSave()
		{
			_enqobject = enquiryForm1.Enquiry;
			_edata = _enqobject.Source;	
			enquiryForm1.RenderControls(true);
			enquiryForm1.AutoScroll= false;				
			_header.ResetDescription();
			_header.LoadEnquiryHeaderEditor(_edata.Tables["ENQUIRY"].Rows[0],enquiryForm1);

			frmDesigner_RemoveFavItem(_currentform);
			_favorites.Insert(0,_currentform);
			if (_favorites.Count > _maxFavoritesCount) _favorites.RemoveAt(_maxFavoritesCount - 1);
			frmDesigner_LoadFavForms();
			lnkRegister.Enabled = !(OmsObject.IsObjectRegistered(this.enquiryForm1.Enquiry.Code,OMSObjectTypes.Enquiry));
		}

        /// <summary>
        /// Routine after script save and save saveas to saveasandcompile script
        /// </summary>
        private void RoutineAfterSave()
        {
            if (CodeWindow == null)
            {
                CodeWindow = new CodeWindow();
                CodeWindow.Init(null);
            }

            if (_header.Script == "")
            {
                _header.Script = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_header.Code);
            }
            if (CodeWindow.SaveAndCompile() == false)
            {
                CodeWindow.Show();
                CodeWindow.BringToFront();
                return;
            }
        }


		/// <summary>
		/// Save all Property from the Main Object
		/// </summary>
		private void Property_SaveAll()
		{
			enquiryForm1_MouseUp(null, new MouseEventArgs(MouseButtons.Left,1,0,0,0));
			_updating=false;
		}

		// *************************************************************************************************
		// frmDesigner
		// *************************************************************************************************

		/// <summary>
		/// If the Check Manual Bound property is Set to false
		/// then Allow the Users to Specify the fields that are used
		/// else Auto create the list from the Questions
		/// </summary>
		private void frmDesigner_AutoFieldCheck()
		{
			if (_header.FieldsManualMode == false && _edata != null)
			{
				//INFO: Altered by Dan to cater for multiple columns in a primary key.
				DataTable pk = Session.CurrentSession.GetPrimaryKey(Convert.ToString(_edata.Tables["ENQUIRY"].Rows[0]["enqCall"]));
				string primary = String.Empty;
				if (pk.Rows.Count > 0)
				{
					primary = Convert.ToString(pk.Rows[0]["COLUMN_NAME"]);
				}
                List<string> txtSQLFields = new List<string>();
				DataView tbl = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
                if (primary != "")
                    txtSQLFields.Add(primary);
                else
                    txtSQLFields.Clear();
				foreach(DataRowView rw in tbl)
				{
					string field = Convert.ToString(rw["quFieldName"]);
					if (field != "")
					{
						if (txtSQLFields.Contains(field) == false)
							txtSQLFields.Add(field);
					}
				}
                _header.Fields = txtSQLFields.ToArray();
			}
		}

		private void pnlCanvass_Resize(object sender, System.EventArgs e)
		{
			if (!_updating && _edata != null)
			{
				statusBar1.Panels[0].Text = string.Format("{0} : {1}", _statusCanvas, pnlCanvass.Size);
				if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
				{
					enquiryForm1.AutoScroll = false;
					_header.StandardSize = pnlCanvass.Size;
				}
				else
				{
					enquiryForm1.AutoScroll = true;
					_header.WizardSize = pnlCanvass.Size;
				}
			}
		}

		private void DeActivateMenus(object sender, System.EventArgs e)
		{
			if (_menustatus == true)
			{
				foreach (ToolStripItem item in mnuEdit.DropDownItems)
				{
					item.Enabled=false;
				}
				_menustatus = false;
				pnlCanvass.Invalidate();
				mnuPageRefresh.Enabled=true;
			}
		}

		#endregion
		
		#region Private Document Form Methods e.g. New Open Save
		/// <summary>
		/// Enquiry Form Clean up
		/// Resets the Designers Values ready for a New 
		/// Enquiry to be Loaded or Created
		/// </summary>
		private void frmDesigner_CleanUp()
		{
			enquiryForm1.Focus();
			enquiryForm1.Enquiry = null;
			_enqobject = null;
			_edata = null;
			_repeat = null;
			GetPropertyView = null;
			GetPropertyControl = null;
			UnselectAll();
            _dpiWarning = (DeviceDpi != 96);
			UI.Windows.Global.RemoveAndDisposeControls(enquiryForm1);
			System.GC.Collect();
		}

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            _dpiWarning = true;
            SetWelcomeBackgroundImage();
            base.OnDpiChanged(e);
        }

        public void frmDesigner_Initialize(bool ConfirmPassword, string formname, string folder, string page)
		{
            processing = true;
            try
            {
                pnlComponents.ControlAdded -= new ControlEventHandler(frmDesigner_ComponentControlAdded);
                picWelcome.Image = null;
                _header.WelcomePageImage = null;

                mnuPreview.Enabled = false;

                _currentform = formname;
                _currentfolder = folder;
                timRebuildcmbFields.Interval = 999999999;
                try
                {
                    _enqobject = Enquiry.GetEnquiryInDesign(_currentform);
                    _edata = _enqobject.Source;
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(this, ex);
                    return;
                }

                pgMain.Refresh();
                pgMain.SelectedObject = null;
                pnlCanvass.Visible = false;
                enquiryForm1.Enquiry = null;

                _header.ResetDescription();
                _header.LoadEnquiryHeaderEditor(_edata.Tables["ENQUIRY"].Rows[0], enquiryForm1);
                _header_DataBuilderChange(this, EventArgs.Empty);
                _header_WizardImageChange(this, EventArgs.Empty);
                pgMain.SelectedObject = _header;

                if (CodeWindow != null)
                {
                    CodeWindow.Close();
                    CodeWindow.Dispose();
                    CodeWindow = null;
                }

                CodeWindow = new FWBS.OMS.Design.CodeBuilder.CodeWindow();
                CodeWindow.Init(null);
                try
                {
                    CodeWindow.Load(_enqobject.Script.ScriptType, _enqobject.Script);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(this, ex);
                }
                CodeWindow.Text = Session.CurrentSession.Resources.GetResource("EFCODEBLD", "Enquiry Form Code Builder", "").Text;
                dgEvents.CurrentCodeSurface = CodeWindow;

                labNotSaved.Visible = false;
                this.OMSToolbars.GetButton("tbScript").Enabled = true;

                 var controls = CodeWindow.GetService<ICodeSurfaceControls>();
                 if (controls != null)
                 {
                     controls.Clear();
                     foreach (DataRow rw in _edata.Tables["QUESTIONS"].Rows)
                         if (rw.RowState != DataRowState.Deleted)
                             controls.Attach(Convert.ToString(rw["quName"]));
                 }

                _updating = true;
                pnlPage.Visible = true;
                GetPropertyView = null;
                AddNipplesToObject(pnlCanvass);
                UI.Windows.Global.RemoveAndDisposeControls(pnlComponents);

                _updating = false;
                lnkRemovePage.Enabled = (enquiryForm1.PageCount != 0);
                string mode = _header.Settings.GetSetting("View", "Mode", "Standard");

                if (mode == "Wizard")
                {
                    WizardMode_Click(null, EventArgs.Empty);
                    if (page != "")
                        enquiryForm1.GotoPage(page, true, true, true);
                }
                else
                    StandardMode_Click(null, EventArgs.Empty);

                mnuUndo.Enabled = false;

                if (UndoTable != null && UndoTable.Rows.Count > 0)
                    UndoTable.Rows.Clear();

                SetFileName(_currentform);
                enquiryForm1.AutoScroll = false;

                lnkRegister.Enabled = !(OmsObject.IsObjectRegistered(this.enquiryForm1.Enquiry.Code, OMSObjectTypes.Enquiry));
                timRebuildcmbFields.Interval = 1;
                timRebuildcmbFields.Enabled = true;
                pnlCanvass.Visible = true;
                Application.DoEvents();
                mnuPreview.Enabled = true;
                IsDirty = false;
                pnlComponents.ControlAdded += new ControlEventHandler(frmDesigner_ComponentControlAdded);

                OMSToolbars.GetButton("tbFields").Pushed = true;
                fieldsForm.Show();
            }
            finally
            {
                processing = false;
            }
        }

        private void SetFileName(string FormName)
		{
			_currentform = FormName;
            string name = (_currentform == "") ? Session.CurrentSession.Resources.GetResource("Untitled", "Untitled", null).Text : _currentform;
            this.Text = string.Format("{0} - {1} {2}", name, FWBS.OMS.Branding.APPLICATION_NAME, Session.CurrentSession.Resources.GetResource("DESIGNER", "Designer", null).Text);
        }

		#endregion

		#region Private Document Keyboard Hooks
		/// <summary>
		/// KeyPreview is active and allow access to move and resize selected controls 
		/// on the Enquiry Control
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmDesigner_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.Control && !mnuUndo.Enabled)
				if (UndoTable.Rows.Count > 0)
					mnuUndo.Enabled=true;

			if (_menustatus && Blue.Focused)
			{
				if (e.Control && e.Shift)
				{
					if (e.KeyCode == Keys.Down)
					{
						foreach(Control item in enquiryForm1.Controls)
							if (_nipplecontainers[item] != null)
								if ((item is IBasicEnquiryControl2) && ((IBasicEnquiryControl2)item).LockHeight == false)
									item.Height = item.Height + _acceleration;
								else if (!(item is IBasicEnquiryControl2))
									item.Height = item.Height + _acceleration;
					}
					else if (e.KeyCode == Keys.Up)
					{
						foreach(Control item in enquiryForm1.Controls)
							if (_nipplecontainers[item] != null)
								if ((item is IBasicEnquiryControl2) && ((IBasicEnquiryControl2)item).LockHeight == false)
									item.Height = item.Height - _acceleration;
								else if (!(item is IBasicEnquiryControl2))
									item.Height = item.Height - _acceleration;
					}
					else if (e.KeyCode == Keys.Left)
					{
						foreach(Control item in enquiryForm1.Controls)
							if (_nipplecontainers[item] != null)
								item.Width = item.Width - _acceleration;
					}
					else if (e.KeyCode == Keys.Right)
					{
						foreach(Control item in enquiryForm1.Controls)
							if (_nipplecontainers[item] != null)
								item.Width = item.Width + _acceleration;
					}
				}
				else if (e.Control && e.Shift == false && Blue.Focused)
				{
					if (e.KeyCode == Keys.Down)
					{
						foreach(Control item in enquiryForm1.Controls)
							if (_nipplecontainers[item] != null)
								item.Top = item.Top + _acceleration;
					}
					else if (e.KeyCode == Keys.Up)
					{
						foreach(Control item in enquiryForm1.Controls)
							if (_nipplecontainers[item] != null)
								item.Top = item.Top - _acceleration;
					}
					else if (e.KeyCode == Keys.Left)
					{
						foreach(Control item in enquiryForm1.Controls)
							if (_nipplecontainers[item] != null)
								item.Left = item.Left - _acceleration;
					}
					else if (e.KeyCode == Keys.Right)
					{
						foreach(Control item in enquiryForm1.Controls)
							if (_nipplecontainers[item] != null)
								item.Left = item.Left + _acceleration;
					}
				}
				if (_acceleration < 5 && e.Control && (e.KeyCode == Keys.Left || e.KeyCode == Keys.Up|| e.KeyCode == Keys.Down || e.KeyCode == Keys.Right))
					_acceleration ++;
			}
			if (e.Shift || e.Control) _multi = true;
		}

		/// <summary>
		/// Stop Acceleration of the Controls if moved or reized
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmDesigner_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (_acceleration > 1)
				UpdateSelectedPositionsSizes();
			if (e.KeyCode == Keys.Delete && this.ActiveControl == Blue)
				ActionMenu_Click(mnuDelete,new EventArgs());
			if (e.KeyCode == Keys.Escape && this.ActiveControl == Blue)
			{
				UnselectAll(true);
				frmDesinger_SelectCanvas();
			}
			if (e.Shift==false && e.Control == false) _multi = false;
			_acceleration =1;
		}
		#endregion

		#region Private Document Form Manipulation
		/// <summary>
		/// Used to Select the Canvas of the Enquiry Form
		/// Add the Nipples to the EnquiryForm and remove 
		/// any Nipples for the Controls
		/// </summary>
		private void frmDesinger_SelectCanvas()
		{
			UnselectAll(true);
			_menustatus=true;
			mnuCopy.Enabled=false;
			mnuCut.Enabled=false;
			pnlPage.Visible = true;
			tpEnquiryHeader.Text = ResourceLookup.GetLookupText("MAIN", "Main", "");
			tabPages.Controls.Remove(tpCustom);

            if (Session.CurrentSession.IsLicensedFor("SDKALL") == true || FWBS.OMS.UI.Windows.frmMain.PartnerAccess == true)
                if (!tabPages.Controls.Contains(tpCode))
			        tabPages.Controls.Add(tpCode);

            pgMain.SelectedObject = _header;
			GetPropertyControl = pnlCanvass;
			GetPropertyView = null;
			GetPropertyControl = null;
			AddNipplesToObject(pnlCanvass);
            enquiryForm1.AutoScroll = false;
            try
			{
				cmbFields.SelectedIndex = -1;
			}
			catch
			{
			}
		}

		/// <summary>
		/// If a control is added to the Enquiry from then add its
		/// greedy little hooks into it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmDesigner_ControlAdded(object sender, ControlEventArgs e)
		{
			Control ctrl = e.Control;	
			if (ctrl.ContextMenuStrip  != this.DesignerPopup)
			{
				if (ctrl is IBasicEnquiryControl2)
					((IBasicEnquiryControl2)ctrl).omsDesignMode=true;
				else
					ctrl.Enabled = true;
				
				if (ctrl is Button)
					((Button)ctrl).FlatStyle = FlatStyle.Standard;

				if (ctrl is eComponent == false)
				{
                    ctrl.ContextMenuStrip = this.DesignerPopup;
					ctrl.VisibleChanged += new EventHandler(this.Control_Visible);
					ctrl.MouseDown += new MouseEventHandler(this.Control_MouseDown);
					ctrl.MouseMove += new MouseEventHandler(this.Control_MouseMove);
					ctrl.MouseUp += new MouseEventHandler(this.Control_MouseUp);
				}
				else
				{
					ctrl.MouseDown += new MouseEventHandler(this.Component_MouseDown);
					ctrl.MouseMove += new MouseEventHandler(this.Component_MouseMove);
					ctrl.MouseUp += new MouseEventHandler(this.Component_MouseUp);
				}

				if (ctrl is ScrollableControl)
					((ScrollableControl)ctrl).AutoScroll = false;

				ctrl.Cursor = Cursors.SizeAll;

				if (pgMain.SelectedObject is FWBS.OMS.UI.Windows.Admin.EnquiryControl)
				{
					pgMain.SelectedObject = null;
					propertyGrid1.SelectedObject =null;
					cmbFields.SelectedIndex=-1;
				}

				if (ctrl is eComponent)
				{
					if (ComponentContainerContains(ctrl.Name) == false)
					{
						eComponent n = ((eComponent)ctrl);
						((PictureBox)n.Controls[0]).Image = FWBS.OMS.UI.Windows.Images.CoolButtons16().Images[19];
						ctrl.Width=100;
						pnlComponents.Controls.Add(ctrl);
						n.Visible=true;
					}			
					else
					{
						ctrl.Visible=false;
					}
				}

				ControlItem newc = new ControlItem(e.Control);
				if (_controls.Contains(newc) == false)
					_controls.Add(newc);
				timRebuildcmbFields.Enabled=false;
				timRebuildcmbFields.Enabled=true;

			}
		}

		private bool ComponentContainerContains(string Name)
		{
			foreach (Control ctrl in pnlComponents.Controls)
			{
				if (ctrl.Name == Name)
					return true;
			}
			return false;
		}

		/// <summary>
		/// If a control is removed from the Enquiry from then remove its
		/// greedy little hooks
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void frmDesigner_ControlRemove(object sender, ControlEventArgs e)
		{
			Control ctrl = e.Control;
            if (ctrl.ContextMenuStrip == this.DesignerPopup)
			{
				ctrl.ContextMenu  = null;
				ctrl.MouseDown -= new MouseEventHandler(this.Control_MouseDown);
				ctrl.MouseMove -= new MouseEventHandler(this.Control_MouseMove);
				ctrl.MouseUp -= new MouseEventHandler(this.Control_MouseUp);
				ctrl.VisibleChanged -= new EventHandler(this.Control_Visible);
			}
			frmDesigner_AutoFieldCheck();

			ControlItem newc = new ControlItem(e.Control);
			_controls.Remove(newc);
			timRebuildcmbFields.Enabled=false;
			timRebuildcmbFields.Enabled=true;
		}

		#endregion

		#region Private Common
		/// <summary>
		/// Create a List of Controls that have had Nipples Added
		/// </summary>
		private void frmDesigner_SelectedControls()
		{
			try
			{
                enquiryForm1.AutoScroll = false;
                cmbFields.SelectedIndex=-1;
				ArrayList obj = new ArrayList();
				ArrayList cobj = new ArrayList();
				DataView sitem = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
				tabPages.Controls.Remove(tpCode);
				// DMB 25/02/2004 remove advanced tab as we cannot handle multi select at the moment
				tabPages.Controls.Remove(tpCustom);

				for(int t = enquiryForm1.Controls.Count-1; t >= 0; t--)
				{
					Control item = enquiryForm1.Controls[t];
					if (_nipplecontainers[item] != null)
					{
						sitem.RowFilter =  "quName = '" + item.Name + "'";
						obj.Add(new FWBS.OMS.UI.Windows.Admin.EnquiryControl(sitem[0].Row,item,enquiryForm1));
						cobj.Add(item);
						tpEnquiryHeader.Text = ResourceLookup.GetLookupText("PROPERTIES", "Properties", "");

						mnuCopy.Enabled=true;
						mnuCut.Enabled=true;
					}
				}  
				pgMain.SelectedObjects = (Object[])obj.ToArray(typeof(Object));
				propertyGrid1.SelectedObjects = (Object[])cobj.ToArray(typeof(Object));
				ControlsSelected();
				this.timRebuildcmbFields.Interval = 1;
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}

		}

		/// <summary>
		/// Display Properties of Selected Control
		/// </summary>
		/// <param name="sitem">Control to Display</param>
		private void frmDesigner_SelectedControl(Control sitem)
		{
            enquiryForm1.AutoScroll = false;
            GetPropertyView = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
			GetPropertyView.RowFilter = "quName = '" + sitem.Name + "'";
			if (GetPropertyView.Count == 0) 
			{
                FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("FAILEDFINDCTRL", "Failed to Find Control", ""));
				return;
			}
			pgMain.SelectedObject = new FWBS.OMS.UI.Windows.Admin.EnquiryControl(GetPropertyView[0].Row,sitem,enquiryForm1);
            tpEnquiryHeader.Text = ResourceLookup.GetLookupText("PROPERTIES", "Properties", "");
			if (!tabPages.Controls.Contains(tpCustom)) 
			{
				tabPages.Controls.Remove(tpCode);
				tabPages.Controls.Add(tpCustom);
			}

            if (Session.CurrentSession.IsLicensedFor("SDKALL") == true || FWBS.OMS.UI.Windows.frmMain.PartnerAccess == true)
                if (!tabPages.Controls.Contains(tpCode))
                    tabPages.Controls.Add(tpCode);

			propertyGrid1.SelectedObject = sitem;
			propertyGrid1.Refresh();
            this.cmbFields.SelectedIndexChanged -= new System.EventHandler(this.cmbFields_SelectedIndexChanged);
            try
			{
				cmbFields.SelectedValue = sitem;
			}
			catch
			{

			}
            this.cmbFields.SelectedIndexChanged += new System.EventHandler(this.cmbFields_SelectedIndexChanged);
            GetPropertyControl = sitem;
			ControlsSelected();
			mnuCopy.Enabled=true;
			mnuCut.Enabled=true;
			this.timRebuildcmbFields.Interval = 1;
		}
		
		/// <summary>
		/// Recurivly Disables all controls and child controls
		/// </summary>
		/// <param name="ctrl">The Control to disable</param>
		private void RecursiveDisable(Control ctrl)
		{
			foreach (Control i in ctrl.Controls)
			{
				i.Enabled=false;
				if (i is PictureBox == false && i is IBasicEnquiryControl2 == false && i is Label == false && i is uBar == false && i is GroupBox == false && i is Panel == false && i is UserControl == false)
					i.BackColor = Color.White;
				if (i.Controls.Count > 0)
					RecursiveDisable(i);
			}
		}

		/// <summary>
		/// Copy selected Controls to the Clipboard
		/// </summary>
		private void ClipboardCopy()
		{
			try
			{
				Clipboard.Clear();
				DataView view = new DataView(_edata.Tables["Questions"]);
				for(int t = enquiryForm1.Controls.Count-1; t >= 0; t--)
				{
					Control item = enquiryForm1.Controls[t];
					if (_nipplecontainers[item] != null)
					{
						view.RowFilter =  "quName = '" + item.Name + "'";
						DataRow r = Clipboard.NewRow();
						r.ItemArray = view[0].Row.ItemArray;
						Clipboard.Rows.Add(r);
					}
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
			mnuPaste.Enabled=true;
		}
		
		/// <summary>
		/// Cut Selected Controls to the Clipboard
		/// </summary>
		private void ClipboardCut()
		{
			try
			{
				Clipboard.Clear();
				DataView view = new DataView(_edata.Tables["Questions"]);
				string group = DateTime.Now.ToString();
				for(int t = enquiryForm1.Controls.Count-1; t >= 0; t--)
				{
					Control item = enquiryForm1.Controls[t];
					if (_nipplecontainers[item] != null)
					{
						view.RowFilter =  "quName = '" + item.Name + "'";
						if (view.Count > 0)	CreateUndoRow(view,group);
						DataRow r = Clipboard.NewRow();
						r.ItemArray = view[0].Row.ItemArray;
						Clipboard.Rows.Add(r);
						enquiryForm1.RemoveControl(ref item);
						GetPropertyControl = null;
						GetPropertyView = null;
					}
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
			UnselectAll(true);
			frmDesinger_SelectCanvas();
			IsDirty=true;
			mnuPaste.Enabled=true;
		}

		/// <summary>
		/// Paste Clipboard Controls to the Form
		/// </summary>
		private void ClipboardPaste()
		{
			DataTable dt = _edata.Tables["QUESTIONS"];
			Exception exo;
			int noinf;
			int b;
			bool flag;
			string OrginalName;
			ArrayList newobj = new ArrayList();
			ArrayList obj = new ArrayList();

			foreach(DataRow rw in Clipboard.Rows)
			{
				rw["enqID"] = _edata.Tables["ENQUIRY"].Rows[0]["enqID"];
				if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Wizard)
					if (Convert.ToString(enquiryForm1.CurrentPage["pgeName"]) == "")
						rw["quPage"] = enquiryForm1.CurrentPage["pgeOrder"];
					else
						rw["quPage"] = enquiryForm1.CurrentPage["pgeName"];
				OrginalName = Convert.ToString(rw["quName"]);
				if (OrginalName.Length > 19)
					OrginalName = OrginalName.Substring(0,19);
				noinf = 0;
				b=0;
				exo = null;
				do
				{
					try
					{
						noinf++;
						if (noinf > 1000) break;
						DataRow r = _edata.Tables["QUESTIONS"].NewRow();
						foreach(DataColumn cm in r.Table.Columns)
							if (cm.AutoIncrement == false)
								r[cm] = rw[cm.ColumnName];

						_edata.Tables["QUESTIONS"].Rows.Add(r);
						flag = true;
						r["quY"] = Convert.ToInt32(r["quY"]) + 5;
					}
					catch(Exception ex)
					{
						exo = ex;
						b++;
						rw["quName"] = OrginalName + b.ToString();
						flag = false;
					}
				}
				while(flag == false);
				if (noinf > 1000)
				{
					ErrorBox.Show(this, exo);
					return;
				}


				
				newobj.Add(rw["quName"]);
				rw["quName"] = OrginalName;

			}
			UnselectAll(true);
			enquiryForm1.RenderControls(true);
			enquiryForm1.AutoScroll= false;				
			if (Clipboard.Rows.Count > 1)
			{
				try
				{
					DataView sitem = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
					foreach (object o in newobj)
					{
						foreach(Control ctrl in enquiryForm1.Controls)
						{
							if (ctrl.Name == Convert.ToString(o))
							{
								sitem.RowFilter =  "quName = '" + ctrl.Name + "'";
								AddNipplesToObject(ctrl);
								obj.Add(new FWBS.OMS.UI.Windows.Admin.EnquiryControl(sitem[0].Row,ctrl,enquiryForm1));
							}
						}
					}
					pgMain.SelectedObjects = (Object[])obj.ToArray(typeof(Object));
					propertyGrid1.SelectedObjects = (Object[])obj.ToArray(typeof(Object));
					propertyGrid1.Refresh();
					IsDirty=true;
				}
				catch (Exception ex)
				{
					ErrorBox.Show(this, ex);
				}
			}
			else
			{
				DataView sitem = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
				foreach(Control ctrl in enquiryForm1.Controls)
				{
					if (ctrl.Name == Convert.ToString(newobj[0]))
					{
						sitem.RowFilter =  "quName = '" + ctrl.Name + "'";
						obj.Add(new FWBS.OMS.UI.Windows.Admin.EnquiryControl(sitem[0].Row,ctrl,enquiryForm1));
						AddNipplesToObject(ctrl);
						frmDesigner_SelectedControl(ctrl);
						break;
					}
				}
			}
			ActionMenu_Click(mnuBringToFront,EventArgs.Empty);
		}

        /// <summary>
        /// Create an Undo Record
        /// </summary>
        /// <param name="view">DataView to Create the New Row in</param>
        /// <param name="grpid">Group identifier</param>
        private void CreateUndoRow(DataView view, string grpid)
		{
            IsDirty = true;
            DataRow r = UndoTable.NewRow();
			r.ItemArray = view[0].Row.ItemArray;
			r["GROUP"] = grpid;
			UndoTable.Rows.Add(r);
			mnuUndo.Enabled=true;
		}

		/// <summary>
		/// Static method used by the Business\Designer.cs
		/// </summary>
		/// <param name="view"></param>
		internal static void CreateUndoRowP(DataRow view)
		{
			if (NoUndo == false || UndoOnce == false)
			{
				DataRow r = UndoTable.NewRow();
				r.ItemArray = view.ItemArray;
				r["GROUP"] = grpstat;
				UndoTable.Rows.Add(r);
				if (NoUndo == true) UndoOnce=true;
				if (grpcnt <= 1) 
				{
					grpstat = DateTime.Now.ToString(); 
					grpcnt = grpchg;
				}
				else grpcnt--;
			}
		}
		
		/// <summary>
		/// Set the Size of the Form
		/// </summary>
		private void UpdateSelectedPositions()
		{
			UpdateSelectedPositions(enquiryForm1);
		}

		/// <summary>
		/// Set the Size of a Anchored Control
		/// </summary>
		/// <param name="Container">Control to store size</param>
		private void UpdateAnchoredPositions(Control Container)
		{
			//
			// Set the Anchored Controls Positions
			//
			string group = DateTime.UtcNow.ToString();
			foreach(Control item in Container.Controls)
			{
				DataView view  = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
				view.RowFilter =  "quName = '" + item.Name + "'";
				if (view.Count > 0)
				{
					if (Convert.ToString(view[0]["quanchor"]) != "")
					{
						CreateUndoRow(view,group);
						if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
						{
							view[0]["quX"] = item.Left;
							view[0]["quY"] = item.Top;
						}
						else
						{
							view[0]["quWizX"] = item.Left;
							view[0]["quWizY"] = item.Top;
						}
					}
				}
			}
		}
		
		/// <summary>
		/// Set the Size of a Selected Control
		/// </summary>
		/// <param name="Container">Control to store size</param>
		private void UpdateSelectedPositions(Control Container)
		{
			//
			// Set the Selected Controls Positions
			//
			Control single = null;
			string group = DateTime.UtcNow.ToString();
			foreach(Control item in Container.Controls)
			{
				DataView view  = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
				if (_nipplecontainers[item] != null || Container == pnlComponents)
				{
					single=item;
					view.RowFilter =  "quName = '" + item.Name + "'";
					if (view.Count > 0)
					{
						CreateUndoRow(view,group);
						if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
						{
							view[0]["quX"] = item.Left;
							view[0]["quY"] = item.Top;
						}
						else
						{
							view[0]["quWizX"] = item.Left;
							view[0]["quWizY"] = item.Top;
						}
					}
				}
			}
		}

		/// <summary>
		/// Set any Controls with Nipples their size to the Database
		/// </summary>
		private void UpdateSelectedPositionsSizes()
		{
			//
			// Set the Selected Controls Positions
			//
			int multisel = 0;
			Control single = null;
			string group = DateTime.UtcNow.ToString();
			foreach(Control item in enquiryForm1.Controls)
			{
				DataView view  = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
				if (_nipplecontainers[item] != null)
				{
					single=item;
					view.RowFilter =  "quName = '" + item.Name + "'";
					if (view.Count > 0)
					{
						CreateUndoRow(view,group);
						multisel++;
						if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
						{
							view[0]["quX"] = item.Left;
							view[0]["quY"] = item.Top;
						}
						else
						{
							view[0]["quWizX"] = item.Left;
							view[0]["quWizY"] = item.Top;
						}
						view[0]["quHeight"] = item.Height;
						view[0]["quWidth"] = item.Width;
					}
				}
			}
			IsDirty=true;
			if (multisel == 1) frmDesigner_SelectedControl(single);
		}

		/// <summary>
		/// Remove the Nipples from a Control
		/// </summary>
		/// <param name="item">Control in Question</param>
		private void RemoveNipples(Control item)
		{
			SelectContainer sc = (SelectContainer)_nipplecontainers[item];
			if (sc != null)
			{
				if (sc.TL != null)
					item.Controls.Remove(sc.TL);
				if (sc.TR != null)
					item.Controls.Remove(sc.TR);
				if (sc.TM != null)
					item.Controls.Remove(sc.TM);
				if (sc.ML != null)
					item.Controls.Remove(sc.ML);
				if (sc.MR != null)
					item.Controls.Remove(sc.MR);
				if (sc.BL != null)
					item.Controls.Remove(sc.BL);
				if (sc.BR != null)
					item.Controls.Remove(sc.BR);
				if (sc.BM != null)
					item.Controls.Remove(sc.BM);
				_nipplecontainers.Remove(item);
				SelectContainer.Count--;
			}
		}

		/// <summary>
		/// Add Nipples to the Control
		/// </summary>
		/// <param name="cnt">Control to add Nipples to</param>
		/// <returns>A SelectContainer that is added to the Controls Tag Property</returns>
		private bool AddNipplesToObject(Control cnt)
		{
			return AddNipplesToObject(cnt,false);
		}

		/// <summary>
		/// Add Nipples to the Control but had option to not allow Resize
		/// </summary>
		/// <param name="cnt">Control to add Nipples to</param>
		/// <param name="NoResize">No Resize Option</param>
		/// <returns>A SelectContainer that is added to the Controls Tag Property</returns>
		private bool AddNipplesToObject(Control cnt, bool NoResize)
		{
			if (cnt != null && _nipplecontainers[cnt] == null)
			{
				cnt.SuspendLayout();
				SelectContainer c = null;
				if (NoResize)
				{
					uBar sTL = new uBar();
					sTL.Location = new Point(0,0);
					sTL.Size = new Size(5,5);
					cnt.Controls.Add(sTL);
					cnt.Controls.SetChildIndex(sTL,0);

					uBar sTR = new uBar();
					sTR.Location = new Point(cnt.Width-5,0);
					sTR.Size = new Size(5,5);
					sTR.Anchor = AnchorStyles.Top | AnchorStyles.Right;
					cnt.Controls.Add(sTR);
					cnt.Controls.SetChildIndex(sTR,0);

					uBar sBL = new uBar();
					sBL.Location = new Point(0,cnt.Height-5);
					sBL.Size = new Size(5,5);
					sBL.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
					cnt.Controls.Add(sBL);
					cnt.Controls.SetChildIndex(sBL,0);

					uBar sBR = new uBar();
					sBR.Location = new Point(cnt.Width-5,cnt.Height-5);
					sBR.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
					sBR.Size = new Size(5,5);
					cnt.Controls.Add(sBR);
					cnt.Controls.SetChildIndex(sBR,0);
					c = new SelectContainer(sTL,sTR,null,null,null,sBL,sBR,null);
				}
				else if (cnt is IBasicEnquiryControl2 && ((IBasicEnquiryControl2)cnt).LockHeight == true)
				{
					uBar sML = new uBar();
					sML.Location = new Point(0,(cnt.Height - 5) / 2);
					sML.Size = new Size(5,5);
					sML.Anchor = AnchorStyles.Left;
					cnt.Controls.Add(sML);
					cnt.Controls.SetChildIndex(sML,0);

					uBar sMR = new uBar();
					sMR.Location = new Point(cnt.Width-5,(cnt.Height - 5) / 2);
					sMR.Size = new Size(5,5);
					sMR.Anchor = AnchorStyles.Right;
					AddMoveEvents(sMR,NippleMode.acLR);
					cnt.Controls.Add(sMR);
					cnt.Controls.SetChildIndex(sMR,0);
					c = new SelectContainer(null,null,null,sML,sMR,null,null,null);
				}
				else
				{
					uBar sTL = new uBar();
					sTL.Location = new Point(0,0);
					sTL.Size = new Size(5,5);
					cnt.Controls.Add(sTL);
					cnt.Controls.SetChildIndex(sTL,0);

					uBar sBM = new uBar();
					sBM.Location = new Point((cnt.Width - 5) / 2,cnt.Height-5);
					sBM.Size = new Size(5,5);
					AddMoveEvents(sBM,NippleMode.acUD);
					sBM.Anchor = AnchorStyles.Bottom;
					cnt.Controls.Add(sBM);
					cnt.Controls.SetChildIndex(sBM,0);

					uBar sBR = new uBar();
					sBR.Location = new Point(cnt.Width-5,cnt.Height-5);
					sBR.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
					sBR.Size = new Size(5,5);
					AddMoveEvents(sBR,NippleMode.acUDLR);
					cnt.Controls.Add(sBR);
					cnt.Controls.SetChildIndex(sBR,0);

					uBar sMR = new uBar();
					sMR.Location = new Point(cnt.Width-5,(cnt.Height - 5) / 2);
					sMR.Size = new Size(5,5);
					sMR.Anchor = AnchorStyles.Right;
					AddMoveEvents(sMR,NippleMode.acLR);
					cnt.Controls.Add(sMR);
					cnt.Controls.SetChildIndex(sMR,0);
					c = new SelectContainer(sTL,null,null,null,sMR,null,sBR,sBM);
				}

				cnt.ResumeLayout();
				statusBar1.Panels[1].Text = string.Format("{0} : {1}", _statusSelected, new Rectangle(cnt.Location,cnt.Size));
				_nipplecontainers.Add(cnt,c);
				return true;
			}
			else
				return false;
		}

		/// <summary>
		/// Add Move Events used by the Add Nipple Procedure
		/// </summary>
		/// <param name="ctl">Nipple Control to add the Event to</param>
		/// <param name="ipos">Nipple Position</param>
		private void AddMoveEvents(Control ctl,NippleMode ipos)
		{
			ctl.BackColor = Color.Black;
			switch(ipos)
			{
				case NippleMode.acLR:
				{
					ctl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlCanvasNipple_MouseMoveLR);
					ctl.Cursor = Cursors.SizeWE;
					break;
				}
				case NippleMode.acUDLR:
				{
					ctl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlCanvasNipple_MouseMoveUDLR);
					ctl.Cursor = Cursors.SizeNWSE;
					break;

				}
				case NippleMode.acUD:
				{
					ctl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnlCanvasNipple_MouseMoveUD);
					ctl.Cursor = Cursors.SizeNS;
					break;
				}
			}
			ctl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnlCanvasNipple_MouseDown);
			ctl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnlCanvasNipple_MouseUp);
		}

		/// <summary>
		/// Unselect all Controls that contain nipples
		/// </summary>
		/// <param name="ResetFieldList">Also reset the Drop Down List</param>
		private void UnselectAll(bool ResetFieldList)
		{
			tabPages.Controls.Remove(tpCode);
			tabPages.Controls.Remove(tpCustom);
			foreach(Control item in pnlComponents.Controls)
				RemoveNipples(item);
			foreach(Control item in enquiryForm1.Controls)
				RemoveNipples(item);
			try
			{
				if (ResetFieldList) cmbFields.SelectedIndex = -1;
			}
			catch{}
			RemoveNipples(pnlCanvass);
			RemoveNipples(labQuestionPage);
			RemoveNipples(labWelcome);
			RemoveNipples(labDescription);
			ControlsNone();
		}

		/// <summary>
		/// Unselect all and Reset the Drop Down List
		/// </summary>
		private void UnselectAll()
		{
			UnselectAll(true);
		}
      



		/// <summary>
		/// Add Control to the Enquiry Form
		/// </summary>
		/// <param name="ControlName">Name of the Control</param>
		/// <param name="ControlType">Type of the Control</param>
		/// <param name="ControlID">Controls ID Number</param>
		/// <param name="Left">X Position</param>
		/// <param name="Top">Y Position</param>
		/// <returns>Returns an EnquiryControl Object</returns>
		private EnquiryControl AddControl(string ControlName, string ControlType, int ControlID, int Left, int Top)
		{
			return AddControl(ControlName, ControlType, ControlID, Left, Top, DBNull.Value, DBNull.Value);
		}

		/// <summary>
		/// Add Control to the Enquiry Form
		/// </summary>
		/// <param name="ControlName">Name of the Control</param>
		/// <param name="ControlType">Type of the Control</param>
		/// <param name="ControlID">Controls ID Number</param>
		/// <param name="Left">X Position</param>
		/// <param name="Top">Y Position</param>
		/// <param name="Width">Width</param>
		/// <param name="Height">Height</param>
		/// <returns>Returns an EnquiryControl Object</returns>
		private EnquiryControl AddControl(string ControlName, string ControlType, int ControlID, int Left, int Top, object Width, object Height)
		{
			if (Convert.ToInt32(Left) < 0) Left = 0;
			if (Convert.ToInt32(Top) < 0) Top = 0;

			DataTable dt = _edata.Tables["QUESTIONS"];
			string[] n = ControlType.Split('.');
			int b = 1;
			bool flag = false;
			DataRow rw = dt.NewRow();
			rw["quName"] = RemoveInvalidChars(ControlName.Trim()) + b.ToString();
			rw["quControl"] = ControlType;
			rw["quctrlid"] = ControlID;
			rw["qucode"] = "";
			rw["quSearch"] = true;
			if (_header.Binding == EnquiryBinding.Bound)
				rw["quTable"] = enquiryForm1.Enquiry.Source.Tables["ENQUIRY"].Rows[0]["enqCall"];
			rw["quOrder"] = Math.Min(_edata.Tables["QUESTIONS"].Rows.Count + 1, 255);
			rw["quTabOrder"] = _edata.Tables["QUESTIONS"].Rows.Count + 1;
			if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Wizard)
				rw["quPage"] = enquiryForm1.PageName;
			else
				rw["quPage"] = DBNull.Value;
			if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
			{
				rw["quX"] = Left;
				rw["quY"] = Top;
			}
			else
			{
				rw["quWizX"] = Left;
				rw["quWizY"] = Top;
			}
			rw["quWidth"] = Width;
			rw["quHeight"] = DBNull.Value;
			int noinf =0;
			Exception exo = null;
			do
			{
				try
				{
					noinf++;
					if (noinf > 1000) break;
					dt.Rows.Add(rw);
					flag = true;
				}
				catch(Exception ex)
				{
					exo = ex;
					b++;
					rw["quName"] = RemoveInvalidChars(ControlName.Trim()) + b.ToString();
					flag = false;
				}
			}
			while(flag == false);
			if (noinf > 1000)
			{
				ErrorBox.Show(this, exo);
				return null;
			}
			Control c = null;
			enquiryForm1.RenderControl(ref c,rw);
			RecursiveDisable(c);
			if (Width == DBNull.Value || FWBS.Common.ConvertDef.ToInt32(Width,0) == 0)
				rw["quWidth"] = c.Width;
			else
				c.Width = Convert.ToInt32(Width);
			
			if (Height != DBNull.Value || FWBS.Common.ConvertDef.ToInt32(Height,0) > 0)
			{
				if (c is IBasicEnquiryControl2)
				{
					if (((IBasicEnquiryControl2)c).LockHeight || Height == DBNull.Value)
						rw["quHeight"] = c.Height;
					else
					{
						rw["quHeight"] = Height;
						c.Height = Convert.ToInt32(Height);
					}
				}
				else
				{
					rw["quHeight"] = Height;
					c.Height = Convert.ToInt32(Height);
				}
			}
			else
			{
				rw["quHeight"] = c.Height;
			}
			c.BringToFront();
			if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Wizard)
				enquiryForm1.RenderControls();
			enquiryForm1.AutoScroll= false;				
			menuCollection1.ButtonDown = "Pointer";
			IsDirty=true;
			GetPropertyControl = c;
			return new EnquiryControl(c,rw);
		}

		private string RemoveInvalidChars(object input)
		{
			string output = Convert.ToString(input);
            if (output.Length >= 17)
                output = output.Substring(0, 17);
            
            string test = @"_!""£$%^&*()+=|\/<>.,;'[]{}:;@~#` ";
			for (int i = 0; i<test.Length; i++)
				output = output.Replace(test.Substring(i,1),"");

			return output;
		}

		/// <summary>
		/// Load the Controls Tool Palette into the MenuBar on the Left
		/// </summary>
		private void LoadControlButtons()
		{
			menuCollection1.Items.Clear();
			string oldgroup = "";
			for (int t = _edata.Tables["CONTROLS"].Rows.Count-1; t>-1 ; t--)
			{
				DataRow rw = _edata.Tables["CONTROLS"].Rows[t];
				if (Convert.ToInt16(rw["ctrlID"].ToString()) > 1)
				{
					try
					{
						if (oldgroup != Convert.ToString(rw["ctrlgroup"]))
						{
							menuCollection1.Items.Add(new FWBS.Common.UI.Windows.MenuCollection.ButtonItems(Convert.ToString(rw["ctrlgroupdesc"]), ResourceLookup.GetLookupText("POINTER", "Pointer", ""), "0", 0, false, true, "", Convert.ToString(rw["ctrlgroup"]), "Pointer"));
							oldgroup = Convert.ToString(rw["ctrlgroup"]);
						}
						menuCollection1.Items.Add(new FWBS.Common.UI.Windows.MenuCollection.ButtonItems(Convert.ToString(rw["ctrlgroupdesc"]), Convert.ToString(rw["ctrldesc"]), Convert.ToString(rw["ctrlID"]), Convert.ToInt16(rw["ctrlID"]), false, true, "", Convert.ToString(rw["ctrlgroup"]), Convert.ToString(rw["ctrlcode"])));
					}
					catch (Exception)
					{
                        menuCollection1.Items.Add(new FWBS.Common.UI.Windows.MenuCollection.ButtonItems(Convert.ToString(rw["ctrlgroupdesc"]), Convert.ToString(rw["ctrldesc"]), "2", 2, false, true, "", Convert.ToString(rw["ctrlgroup"]), Convert.ToString(rw["ctrlcode"])));
                    }
				}
			}
			menuCollection1.ButtonDown = "General|Pointer";
		}

		/// <summary>
		/// Enableds the UI when a Control or Controls are Selected
		/// </summary>
		private void ControlsSelected()
		{
            OMSToolbars.GetButton("tbBringtoFront").Enabled = true;
            OMSToolbars.GetButton("tbSendtoBack").Enabled = true;
			Blue.Focus();
		}

		/// <summary>
		/// Disables the UI when no Controls are Selected
		/// </summary>
		private void ControlsNone()
		{
			try
			{
                OMSToolbars.GetButton("tbBringtoFront").Enabled = false;
                OMSToolbars.GetButton("tbSendtoBack").Enabled = false;
			}
			catch
			{}
		}

		/// <summary>
		/// Activates a the UI Menu
		/// </summary>
		private void ActivateMenus()
		{
			ActivateMenus(null,EventArgs.Empty);
		}

		/// <summary>
		/// Click Event Compatible Version of the Active Menus Command
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ActivateMenus(object sender, System.EventArgs e)
		{
			if (_menustatus == false)
			{
				mnuUndo.Enabled = !(UndoTable.Rows.Count==0);
				mnuSelectAll.Enabled = true;
				_menustatus = true;
				pnlCanvass.Invalidate();
				mnuPageRefresh.Enabled=true;
			}
		}

		/// <summary>
		/// De-Activate Menu
		/// </summary>
		private void DeActivateMenus()
		{
			DeActivateMenus(null,EventArgs.Empty);
		}

		
		/// <summary>
		/// Sets a Propery of a Control by its Name
		/// </summary>
		/// <param name="_obj">Control to set the Propery of</param>
		/// <param name="PropName">Name of the Property</param>
		/// <param name="Value">Value to Set</param>
		private void SetPropertyByName(Control _obj, string PropName, object Value)
		{
			Type objtype = null;
			
			const BindingFlags _memberBinding = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			// Creates a Type form the already created object
			objtype = _obj.GetType();

			PropertyInfo prop = null;
			// Gets the Propery Information e.g Name Property Type etc.
			prop = objtype.GetProperty(PropName, _memberBinding);
			prop.SetValue(_obj, Value, null);
		}
		
		#endregion

		#region Private Menu & ToolBar Actions
		
		/// <summary>
		/// Builds a List of Controls by its Size and Position
		/// </summary>
		/// <returns></returns>
		private SortedList GetSelectedControlsByPos()
		{
			try
			{
				SortedList _selctrls  = new SortedList();
				foreach(Control item in enquiryForm1.Controls)
				{
					if (_nipplecontainers[item] != null)
					{
						int x = 10000 + item.Left;
						int y = 10000 + item.Top;
						Int64 pos = Convert.ToInt64(y.ToString() + x.ToString());
						_selctrls.Add(pos,item);
					}
				}
				return _selctrls;
			}
			catch
			{
				return new SortedList();
			}
		}


        private bool ScriptCodeIsEmpty()
        {
            return string.IsNullOrEmpty(enquiryForm1.Enquiry.Script.Code);
        }

		
		/// <summary>
		/// Designer Menu Action
		/// </summary>
		/// <param name="sender">Menu Item</param>
		/// <param name="e">Null</param>
		private void ActionMenu_Click(object sender, System.EventArgs e)
        {
            mainMenu1.Enabled = false;
            try
            {
                _multi = false;

                #region Refresh Page
                if (sender == mnuPageRefresh)
                {
                    this.Refresh();
                }
                #endregion

                #region Undo Control
                if (sender == mnuUndo)
                {
                    try
                    {
                        string g = "";
                        int n = 0;
                        this.Cursor = Cursors.WaitCursor;
                        do
                        {
                            n = UndoTable.Rows.Count - 1;
                            g = Convert.ToString(UndoTable.Rows[n]["GROUP"]);
                            DataView view = new DataView(_edata.Tables["QUESTIONS"]);
                            if (UndoTable.Rows[n]["quID"] == DBNull.Value)
                                view.RowFilter = "[quName] = '" + UndoTable.Rows[n]["quName"] + "'";
                            else
                                view.RowFilter = "[quID] = " + UndoTable.Rows[n]["quID"];

                            if (view.Count > 0)
                            {
                                foreach (DataColumn cm in view.Table.Columns)
                                    view[0].Row[cm] = UndoTable.Rows[n][cm.ColumnName];
                            }
                            else
                            {
                                DataRow r = _edata.Tables["QUESTIONS"].NewRow();
                                foreach (DataColumn cm in view.Table.Columns)
                                    r[cm] = UndoTable.Rows[n][cm.ColumnName];
                                _edata.Tables["QUESTIONS"].Rows.Add(r);
                            }

                            UndoTable.Rows[n].Delete();
                            n = UndoTable.Rows.Count - 1;
                        }
                        while (n > -1 && g == Convert.ToString(UndoTable.Rows[n]["GROUP"]));
                        enquiryForm1.RenderControls();
                        enquiryForm1.AutoScroll = false;
                        pgMain.Refresh();
                        mnuUndo.Enabled = !(UndoTable.Rows.Count == 0);
                        return;
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(this, ex);
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                #endregion

                #region Delete Control
                if (sender == mnuDelete && this.ActiveControl == Blue)
                {
                    try
                    {
                        var controls = CodeWindow.GetService<ICodeSurfaceControls>();

                        DataView view = new DataView(_edata.Tables["Questions"]);
                        string group = DateTime.UtcNow.ToString();
                        for (int t = enquiryForm1.Controls.Count - 1; t >= 0; t--)
                        {
                            Control item = enquiryForm1.Controls[t];
                            if (_nipplecontainers[item] != null)
                            {
                                view.RowFilter = "quName = '" + item.Name + "'";
                                if (controls != null)
                                    controls.Detach(item.Name);
                                if (view.Count > 0) CreateUndoRow(view, group);
                                enquiryForm1.RemoveControl(ref item);
                                GetPropertyControl = null;
                                GetPropertyView = null;
                            }
                        }
                        for (int t = pnlComponents.Controls.Count - 1; t >= 0; t--)
                        {
                            Control item = pnlComponents.Controls[t];
                            if (_nipplecontainers[item] != null)
                            {
                                view.RowFilter = "quName = '" + item.Name + "'";
                                if (view.Count > 0) CreateUndoRow(view, group);
                                enquiryForm1.RemoveControl(ref item);
                                pnlComponents.Controls.Remove(item);
                                GetPropertyControl = null;
                                GetPropertyView = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(this, ex);
                    }
                    UnselectAll(true);
                    frmDesinger_SelectCanvas();
                    IsDirty = true;
                    return;
                }
                #endregion

                #region New Enquiry Form Data
                if (sender == mnuNew)
                {
                    if (IsFormDirty())
                    {
                        if (!string.IsNullOrWhiteSpace(_currentform))
                            UnlockCurrentObject(_currentform);

                        frmDesigner_CleanUp();
                        OpenSaveEnquiry1.Code = "";
                        lstListFields.Items.Clear();
                        frmDesigner_Initialize(false, "", @"\", "");
                        pgMain.Refresh();
                        _header.Margin = new Point(10, 10);
                        _header.LineSpacing = 6;
                        this.OMSToolbars.GetButton("tbScript").Enabled = false;
                        labNotSaved.Visible = true;
                        statusBar1.Panels[2].Text = _statusReady;
                        _isdirty = false;
                    }
                    return;
                }
                #endregion

                #region Save As Enquiry Form Data
                if (sender == mnuSaveAs)
                {
                    Property_SaveAll();
                    Blue.Focus();
                    this.Validate();
                    if (_dpiWarning)
                    {
                        FWBS.OMS.UI.Windows.MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("NORMDPIDESIGNER", "Please use 100% scaling mode (96 DPI) for editing and saving forms.", "").Text, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    OpenSaveEnquiry1.Folder = _currentfolder;
                    OpenSaveEnquiry1.OpenSaveMode = FWBS.OMS.UI.Windows.OpenSaveModes.SaveForm;
                    if (OpenSaveEnquiry1.Execute() == DialogResult.OK)
                    {
                        SetFileName(OpenSaveEnquiry1.Code);
                        _edata.Tables["ENQUIRY"].Rows[0]["enqCode"] = OpenSaveEnquiry1.Code;
                        _edata.Tables["ENQUIRY"].Rows[0]["enqDesc"] = OpenSaveEnquiry1.Caption;
                        _edata.Tables["ENQUIRY"].Rows[0]["enqPath"] = OpenSaveEnquiry1.Folder;
                        _edata.Tables["ENQUIRY"].Rows[0]["enqEngineVersion"] = Enquiry.EngineVersion;

                        try
                        {
                            if (enquiryForm1.Enquiry.Script.Code != "")
                            {
                                FWBS.OMS.UI.Windows.MessageBox show = new FWBS.OMS.UI.Windows.MessageBox(Session.CurrentSession.Resources.GetResource("SCSCRIPTEXISTS", "This Screen has a Script. What would you like to do?", ""));
                                show.Buttons = new string[2] { "New Script", "Copy Script" };
                                show.Caption = FWBS.OMS.Branding.APPLICATION_NAME;
                                show.Icon = FWBS.OMS.UI.Windows.MessageBox.MessageBoxIconGear;
                                string ret = show.Show(this);
                                switch (ret)
                                {
                                    case "New Script":
                                        enquiryForm1.Enquiry.NewScript();
                                        _edata.Tables["ENQUIRY"].Rows[0]["enqScript"] = "";
                                        _header_ScriptChange(this, System.EventArgs.Empty);
                                        break;
                                    case "Copy Script":
                                        string scriptname = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_currentform);
                                        enquiryForm1.Enquiry.CopyScript(scriptname);
                                        _edata.Tables["ENQUIRY"].Rows[0]["enqScript"] = scriptname;
                                        _header_ScriptChange(this, System.EventArgs.Empty);
                                        break;
                                }
                            }
                            _header.Settings.Synchronise();
                            enquiryForm1.Enquiry.SaveAs(_currentform);

                            if (!ScriptCodeIsEmpty())
                            {
                                RoutineAfterSave();
                            }
                           
                            LoadAfterSave();

                            if (Session.CurrentSession.ObjectLocking && !alreadylocked)
                            {
                                LockEnquiryForm(OpenSaveEnquiry1.Code);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorBox.Show(this, ex);
                            return;
                        }
                        labNotSaved.Visible = false;
                        this.OMSToolbars.GetButton("tbScript").Enabled = true;
                        statusBar1.Panels[2].Text = _statusSaved;
                        pgMain.Refresh();

                        IsDirty = false;
                        ManageVersioningMenuOptions(true);
                    }
                    return;
                }
                #endregion

                #region Save Enquiry Form Data
                if (sender == mnuSave)
                {
                    processing = true;
                    try
                    {
                        Property_SaveAll();
                        if (_currentform == "" || _dpiWarning)
                        {
                            ActionMenu_Click(mnuSaveAs, e);
                            return;
                        }

                        Blue.Focus();
                        this.Validate();
                        try
                        {
                            if (!ScriptCodeIsEmpty())
                            {
                                RoutineAfterSave();
                            }

                            CodeWindow.Close();
                            CodeWindow = null;

                            frmDesinger_SelectCanvas();
                            _edata.Tables["ENQUIRY"].Rows[0]["enqEngineVersion"] = Enquiry.EngineVersion;
                            _header.Settings.Synchronise();
                            string oldpage = "";
                            if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Wizard)
                                oldpage = enquiryForm1.PageName;
                            enquiryForm1.Enquiry.Save();

                            frmDesigner_Initialize(true, _currentform, _currentfolder, oldpage);
                            
                            if (Session.CurrentSession.ObjectLocking && !alreadylocked)
                            {
                                LockEnquiryForm(OpenSaveEnquiry1.Code);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorBox.Show(this, ex);
                        }
                        finally
                        {
                        }
                        statusBar1.Panels[2].Text = _statusSaved;
                        pgMain.Refresh();
                        IsDirty = false;
                        return;
                    }
                    finally
                    {
                        processing = false;
                        ManageVersioningMenuOptions(true);
                    }
                }
                #endregion

                #region Open Enquiry Form Data
                if (sender == mnuOpen)
                {
                    if (IsFormDirty())
                    {
                        OpenSaveEnquiry1.OpenSaveMode = FWBS.OMS.UI.Windows.OpenSaveModes.OpenForm;
                        OpenSaveEnquiry1.Folder = _currentfolder;
                        if (OpenSaveEnquiry1.Execute() == DialogResult.OK)
                        {
                            processing = true;
                            try
                            {
                                if (Session.CurrentSession.ObjectLocking)
                                {
                                    if (!ls.CheckObjectLockState(OpenSaveEnquiry1.Code, FWBS.OMS.UI.Windows.LockableObjects.EnquiryForm))
                                    {
                                        if (!string.IsNullOrWhiteSpace(_currentform))
                                            UnlockCurrentObject(_currentform);

                                        OpenEnquiryFormInDesigner(OpenSaveEnquiry1.Code);
                                        IsDirty = false;
                                        LockEnquiryForm(OpenSaveEnquiry1.Code);
                                        alreadylocked = true;
                                    }
                                }
                                else
                                {
                                    OpenEnquiryFormInDesigner(OpenSaveEnquiry1.Code);
                                    IsDirty = false;
                                }
                            }
                            finally
                            {
                                processing = false;
                            }
                            statusBar1.Panels[2].Text = _statusReady;
                        }
                    }
                    return;
                }
                #endregion

                #region Repeat Last Action
                if (sender == mnuRepeat)
                {
                    ActionMenu_Click(_repeat, EventArgs.Empty);
                }
                #endregion

                #region Send to Back
                if (sender == mnuSendToBack)
                {
                    DataView view = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            item.SendToBack();
                            view.RowFilter = "quName = '" + item.Name + "'";
                            if (view.Count > 0)
                            {
                                view[0]["quOrder"] = 0;
                            }
                        }
                    }
                    view.RowFilter = "";
                    view.Sort = "[quOrder]";
                    int t = 1;
                    foreach (DataRowView r in view)
                    {
                        r["quOrder"] = t;
                        if (t <255)
                            t++;

                    }
                }
                #endregion

                #region Bring to Front
                if (sender == mnuBringToFront)
                {
                    DataView view = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
                    for (int i = enquiryForm1.Controls.Count - 1; i > -1; i--)
                    {
                        Control item = enquiryForm1.Controls[i];
                        if (_nipplecontainers[item] != null)
                        {
                            item.BringToFront();
                            view.RowFilter = "quName = '" + item.Name + "'";
                            if (view.Count > 0)
                            {
                                view[0]["quOrder"] = 255;
                            }
                        }
                    }
                    view.RowFilter = "";
                    view.Sort = "[quOrder]";
                    int t = 0;
                    foreach (DataRowView r in view)
                    {
                        r["quOrder"] = t;
                        if (t < 255)
                            t++;
                    }
                }
                #endregion

                #region Reset Custom Properties
                if (sender == mnuResetProperties)
                {
                    FWBS.Common.ConfigSetting cfg = new FWBS.Common.ConfigSetting(GetPropertyView[0].Row, "qufilter");
                    cfg.Current = "custom";
                    cfg.DocCurrent.RemoveAllAttributes();
                    cfg.Synchronise();

                    GetPropertyView[0]["quCustom"] = DBNull.Value;
                    Control n = null;
                    enquiryForm1.RenderControl(ref n, GetPropertyView[0].Row);
                }
                #endregion

                #region Auto Tab Index
                if (sender == mnuAutoTabIndex)
                {
                    int i = 0;
                    foreach (System.Collections.DictionaryEntry itemo in GetSelectedControlsByPos())
                    {
                        i++;
                        Control item = (Control)itemo.Value;
                        Console.WriteLine(item.Name + " - " + itemo.Key);
                        DataView view = new DataView(_edata.Tables["QUESTIONS"]);
                        view.RowFilter = "[quName] = '" + item.Name + "'";
                        view[0]["quTabOrder"] = i;
                    }
                }
                #endregion

                #region Decrease Horizontal Spacing
                if (sender == mnuHDecrease)
                {
                    int x = 0;
                    foreach (System.Collections.DictionaryEntry itemo in GetSelectedControlsByPos())
                    {
                        Control item = (Control)itemo.Value;
                        item.Left = item.Left - x;
                        x = x + 2;
                    }
                }
                #endregion

                #region Increase Horizontal Spacing
                if (sender == mnuHIncrease)
                {
                    int x = 0;
                    foreach (System.Collections.DictionaryEntry itemo in GetSelectedControlsByPos())
                    {
                        Control item = (Control)itemo.Value;
                        item.Left = item.Left + x;
                        x = x + 2;
                    }
                }
                #endregion

                #region Remove Horizontal Spacing
                if (sender == mnuHTogether)
                {
                    int x = 100000;
                    SortedList _selctrls = GetSelectedControlsByPos();
                    foreach (System.Collections.DictionaryEntry itemo in _selctrls)
                    {
                        Control item = (Control)itemo.Value;
                        if (item.Top < x) x = item.Left;
                    }
                    foreach (System.Collections.DictionaryEntry itemo in _selctrls)
                    {
                        Control item = (Control)itemo.Value;
                        item.Left = x;
                        x = x + item.Width;
                    }
                }
                #endregion

                #region Increase Vertical Spacing
                if (sender == mnuVIncrease)
                {
                    int y = 0;
                    foreach (System.Collections.DictionaryEntry itemo in GetSelectedControlsByPos())
                    {
                        Control item = (Control)itemo.Value;
                        item.Top = item.Top + y;
                        y = y + 2;
                    }
                }
                #endregion

                #region Decrease Vertical Spacing
                if (sender == mnuVDecrease)
                {
                    int y = 0;
                    foreach (System.Collections.DictionaryEntry itemo in GetSelectedControlsByPos())
                    {
                        Control item = (Control)itemo.Value;
                        item.Top = item.Top - y;
                        y = y + 2;
                    }
                }
                #endregion

                #region Remove Vertical Spacing
                if (sender == mnuVTogether)
                {
                    SortedList _selctrls = GetSelectedControlsByPos();
                    int y = 100000;
                    foreach (System.Collections.DictionaryEntry itemo in _selctrls)
                    {
                        Control item = (Control)itemo.Value;
                        if (item.Top < y) y = item.Top;
                    }
                    foreach (System.Collections.DictionaryEntry itemo in _selctrls)
                    {
                        Control item = (Control)itemo.Value;
                        item.Top = y;
                        y = y + item.Height;
                    }
                }
                #endregion

                #region Size to Widest
                if (sender == mnuWidest)
                {
                    int w = 0;
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            if (item.Width > w) w = item.Width;
                        }
                    }
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            item.Width = w;
                        }
                    }
                }
                #endregion

                #region Size to Narrowest
                if (sender == mnuNarrowest)
                {
                    int w = 9999999;
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            if (item.Width < w) w = item.Width;
                        }
                    }
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            item.Width = w;
                        }
                    }
                }
                #endregion

                #region Align to Top
                if (sender == mnuTop)
                {
                    int y = 100000;
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            if (item.Top < y) y = item.Top;
                        }
                    }
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            item.Top = y;
                        }
                    }
                }
                #endregion

                #region Align to Left
                if (sender == mnuLeft)
                {
                    int x = 100000;
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            if (item.Left < x) x = item.Left;
                        }
                    }
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            item.Left = x;
                        }
                    }
                }
                #endregion

                #region Align to Right
                if (sender == mnuRight)
                {
                    int x = 0;
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            if (item.Left > x) x = item.Left;
                        }
                    }
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            item.Left = x;
                        }
                    }
                }
                #endregion

                #region Align to Bottom
                if (sender == mnuBottom)
                {
                    int y = 0;
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            if (item.Top > y) y = item.Top;
                        }
                    }
                    foreach (Control item in enquiryForm1.Controls)
                    {
                        if (_nipplecontainers[item] != null)
                        {
                            item.Top = y;
                        }
                    }
                }
                UpdateSelectedPositions();
                #endregion

                #region Set Repeat Menu Action
                if (sender != mnuRepeat)
                {
                    _repeat = sender;
                    mnuRepeat.Enabled = true;
                }
                #endregion
            }
            finally
            {
                mainMenu1.Enabled = true;
            }
        }


		private void mnuDebug_Click(object sender, System.EventArgs e)
		{
			FWBS.OMS.UI.Windows.Design.frmGrid frm = new FWBS.OMS.UI.Windows.Design.frmGrid(_edata);
			frm.Show();
		}

		// ************************************************************************************************
		//
		// WIZARD VIEW 
		//
		// ************************************************************************************************
		private void WizardMode_Click(object sender, System.EventArgs e)
		{
			UnselectAll();
			mnuWizardMode.Checked=true;
			mnuStandardMode.Checked=false;
            OMSToolbars.GetButton("tbWizard").Pushed = true;
			OMSToolbars.GetButton("tbStandard").Pushed = false;

            _updating=true;
			pnlCanvass.Width = LogicalToDeviceUnits((int)_edata.Tables["ENQUIRY"].Rows[0]["enqWizardWidth"]);
			pnlCanvass.Height = LogicalToDeviceUnits((int)_edata.Tables["ENQUIRY"].Rows[0]["enqWizardHeight"]);
			_updating=false;

			pnlEnquiry.Visible=true; // make it temporary visible to be properly resized
			pnlWelcome.Visible=true;
			pnlNavigation.Visible=true;
			pnlWizardPage.Controls.Add(enquiryForm1);
			pnlNavigation.SendToBack();
			pnlEnquiry.Visible=false;

			enquiryForm1.BackColor = SystemColors.Control;
			enquiryForm1.Style = FWBS.OMS.UI.Windows.EnquiryStyle.Wizard;
			enquiryForm1.Enquiry = _enqobject;
			enquiryForm1.AutoScroll = true;
			if (enquiryForm1.PageCount > 0) 
				btnNext.Enabled =true;
		}

		// ************************************************************************************************
		//
		// STANDARD VIEW 
		//
		// ************************************************************************************************

		private void StandardMode_Click(object sender, System.EventArgs e)
		{
			UnselectAll();
            OMSToolbars.GetButton("tbStandard").Pushed = true;
            OMSToolbars.GetButton("tbWizard").Pushed = false;
			mnuStandardMode.Checked=true;
			mnuWizardMode.Checked=false;
			
			pnlEnquiry.Visible=false;
			pnlWelcome.Visible=false;
			pnlNavigation.Visible=false;

			_updating = true;
			pnlCanvass.Width = LogicalToDeviceUnits((int)_edata.Tables["ENQUIRY"].Rows[0]["enqCanvasWidth"]);
			pnlCanvass.Height = LogicalToDeviceUnits((int)_edata.Tables["ENQUIRY"].Rows[0]["enqCanvasHeight"]);
			_updating = false;
			pnlCanvass.Controls.Add(enquiryForm1);

			enquiryForm1.BackColor = SystemColors.Control;
			enquiryForm1.Style = FWBS.OMS.UI.Windows.EnquiryStyle.Standard;
			enquiryForm1.Enquiry = _enqobject;
			enquiryForm1.AutoScroll = false;
			enquiryForm1.RenderControls(true);
			frmDesinger_SelectCanvas();
		}

		// ************************************************************************************************
		//
		// CHECK IN
		//
		// ************************************************************************************************

        private void Checkin_Click(object sender, System.EventArgs e)
        {
            CreateAndArchiveData(true);
        }
        

        private void CreateAndArchiveData(bool CheckInOnly)
        {
            // To go wherever the ‘Save and Version’ button/menu item will be located
            var enquiryFormVersionData = GetEnquiryFormVersionDataSet();
            enquiryformVersionDataArchiver = new FWBS.OMS.UI.Windows.EnquiryFormVersionDataArchiver();
            if(!CheckInOnly)
                enquiryformVersionDataArchiver.Saved += new EventHandler(enquiryformVersionDataArchiver_Saved);
            enquiryformVersionDataArchiver.SaveData(enquiryFormVersionData, enquiryForm1.Code, enquiryForm1.Version, versionID: Guid.NewGuid());
        }

        private void enquiryformVersionDataArchiver_Saved(object sender, EventArgs e)
        {
            enquiryformVersionDataArchiver.Saved -= new EventHandler(enquiryformVersionDataArchiver_Saved);
            OpenComparisonTool();
        }

        // Methods to build the DataSet for the Archiver objects to use.
        private DataTable BuildVersionDataTable(DataTable originalDataTable, string tableName)
        {
            var copiedTable = originalDataTable.Copy();
            copiedTable.TableName = tableName;
            return copiedTable;
        }

        private DataSet GetEnquiryFormVersionDataSet()
        {
            var versionDataToSave = new DataSet();

            var enquiryHeader = BuildVersionDataTable(_edata.Tables["ENQUIRY"], "dbEnquiry");
            versionDataToSave.Tables.Add(enquiryHeader);

            var questions = BuildVersionDataTable(_edata.Tables["QUESTIONS"], "dbEnquiryQuestion");
            versionDataToSave.Tables.Add(questions);

            var pages = BuildVersionDataTable(_edata.Tables["PAGES"], "dbEnquiryPage");
            versionDataToSave.Tables.Add(pages);

            return versionDataToSave;
        }


		// ************************************************************************************************
		//
		// COMPARE
		//
		// ************************************************************************************************

        private void Compare_Click(object sender, System.EventArgs e)
        {
            if (CheckObjectInIfNecessary())
                CreateAndArchiveData(false);
            else
                OpenComparisonTool();
        }

        private bool CheckObjectInIfNecessary()
        {
            string sql = "select * from dbEnquiryVersionData where Code = @code and Version = @version";
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("code", enquiryForm1.Code));
            parList.Add(connection.CreateParameter("version", enquiryForm1.Version));
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            if (dt != null && dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        private void OpenComparisonTool()
        {
            vcs = new FWBS.OMS.UI.Windows.VersionComparisonSelector(enquiryForm1.Code, UI.Windows.LockableObjects.EnquiryForm);
            vcs.RestorationCompleted += new EventHandler<UI.Windows.RestorationCompletedEventArgs>(vcs_RestorationCompleted);
            vcs.FormClosing += new FormClosingEventHandler(vcs_FormClosing);
            var result = vcs.ShowDialog(this);
            vcs.StartPosition = FormStartPosition.CenterScreen;
        }

        void vcs_FormClosing(object sender, FormClosingEventArgs e)
        {
            vcs.RestorationCompleted -= new EventHandler<UI.Windows.RestorationCompletedEventArgs>(vcs_RestorationCompleted);
        }

        private void vcs_RestorationCompleted(object sender, UI.Windows.RestorationCompletedEventArgs e)
        {
            try
            {
                if (e.result == 1)
                    OpenEnquiryFormInDesigner(OpenSaveEnquiry1.Code);
            }
            catch(Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        // ************************************************************************************************


		private void mnuSelectAll_Click(object sender, System.EventArgs e)
		{
			RemoveNipples(pnlCanvass);
			foreach(Control item in enquiryForm1.Controls)
			{
				AddNipplesToObject(item);
			}
            frmDesigner_SelectedControls();
        }

		private void UnselectAll_Click(object sender, System.EventArgs e)
		{
			UnselectAll();
			pnlPage.Visible = true;
		}

		private void menuItem9_Click(object sender, System.EventArgs e)
		{
            UI.Windows.Global.RemoveAndDisposeControls(enquiryForm1);
			enquiryForm1.RenderControls();
			enquiryForm1.AutoScroll= false;				
		}

		private void mnuShowUndoList_Click(object sender, System.EventArgs e)
		{
			FWBS.OMS.UI.Windows.Design.frmGrid frm = new FWBS.OMS.UI.Windows.Design.frmGrid(UndoTable);
			frm.Show();
		}

		private void mnuPreview_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (IsFormDirty())
				{
					FWBS.Common.KeyValueCollection n = new FWBS.Common.KeyValueCollection();
					foreach (FWBS.OMS.UI.Windows.Design.Parameter p in _header.DataBuilder.Parameters)
					{
						string v = p.BoundValue.Replace("%","");
						try
						{
							n.Add(v, p.TestValue);
						}
						catch
						{}
					}
					using (frmPreview frmPreview1 = new frmPreview(_enqobject.Code, LogicalToDeviceUnits(_header.StandardSize), n))
					{
						frmPreview1.ShowDialog();
						byte[] img = frmPreview1.ImageStream;
						if (img == null)
							_edata.Tables["ENQUIRY"].Rows[0]["enqImage"] = DBNull.Value;
						else
							_edata.Tables["ENQUIRY"].Rows[0]["enqImage"] = img;
					}
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void mnuResetCPos_Click(object sender, System.EventArgs e)
		{
			if (GetPropertyView == null)
			{
				DataView view  = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
				string group = DateTime.UtcNow.ToString();
				foreach(Control item in enquiryForm1.Controls)
				{
					if (_nipplecontainers[item] != null)
					{
						view.RowFilter =  "quName = '" + item.Name + "'";
						if (view.Count > 0)
						{
							CreateUndoRow(view,group);
							view[0]["quWizX"] = DBNull.Value;
							view[0]["quWizY"] = DBNull.Value;
						}
					}
				}
			}
			else
			{
				GetPropertyView[0]["quWizX"] = DBNull.Value;
				GetPropertyView[0]["quWizY"] = DBNull.Value;
			}
			enquiryForm1.RenderControls();
			enquiryForm1.AutoScroll= false;				
		}

		private void DesignerPopup_Popup(object sender, System.EventArgs e)
		{
			Control_MouseUpRoot();
			try
			{
				if (GetPropertyView[0]["quCustom"] == DBNull.Value)
					mnuResetProperties.Enabled=false;
				else
					mnuResetProperties.Enabled=true;
			}
			catch
			{
				mnuResetProperties.Enabled=false;
			}

			if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Wizard)
			{
				mnuSp7.Visible=true;
				mnuResetCPos.Visible=true;
				if (GetPropertyView != null)
				{
					if (GetPropertyView[0]["quWizX"] == DBNull.Value || GetPropertyView[0]["quWizY"] == DBNull.Value)
						mnuResetCPos.Enabled=false;
					else
						mnuResetCPos.Enabled=true;
				}
				else
				{
					mnuResetCPos.Enabled=true;

				}
			}
			else
			{
				mnuSp7.Visible=false;
				mnuResetCPos.Visible=false;
			}
		}

		private void mnuEdit_Popup(object sender, System.EventArgs e)
		{
			if (UndoTable.Rows.Count > 0)
				mnuUndo.Enabled=true;
			if (Clipboard.Rows.Count > 0)
				mnuPaste.Enabled=true;
		}

		private void mnuCut_Click(object sender, System.EventArgs e)
		{
			ClipboardCut();
		}

		private void mnuCopy_Click(object sender, System.EventArgs e)
		{
			ClipboardCopy();
		}

		private void mnuShowClipboard_Click(object sender, System.EventArgs e)
		{
			FWBS.OMS.UI.Windows.Design.frmGrid frm = new FWBS.OMS.UI.Windows.Design.frmGrid(Clipboard);
			frm.Show();
		}

		private void mnuPaste_Click(object sender, System.EventArgs e)
		{
			ClipboardPaste();
		}

		private void mnuLineUp_Click(object sender, System.EventArgs e)
		{
			int x=10;
			int y=10;
			foreach (Control ctrl in pnlComponents.Controls)
			{
				ctrl.Location = new Point(x,y);
				x = x + ctrl.Width + 5;
				if (x > pnlComponents.Width) 
				{
					x = 10;
					y = y + ctrl.Height + 10;
				}
			}
			UpdateSelectedPositions(pnlComponents);
		}
		#endregion

		#region Private Control Hooks
		// *************************************************************************************************
		// Nipples
		// *************************************************************************************************

		/// <summary>
		/// Resize Canvass Mouse Up that sets the new Size of the Enquiry Form Canvass
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlCanvasNipple_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Control ocnt = (Control)sender;
			ocnt.Visible = true;
			if (ocnt.Parent == pnlCanvass)
				UpdateAnchoredPositions(enquiryForm1);
			NoUndo=false;
			UndoOnce=false;
			pgMain.Refresh();
			IsDirty=true;
		}

		/// <summary>
		/// The control has been assigned the Up Down Left Right Event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlCanvasNipple_MouseMoveUDLR(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Control ocnt = (Control)sender;
				ocnt.Visible=false;
				Point p = ((Point)ocnt.Parent.PointToClient(MousePosition));
				p.X = p.X - ClickOffset.X;
				p.Y = p.Y - ClickOffset.Y;
				Point n = p;
				n.Y = n.Y +ocnt.Height;
				n.X = n.X +ocnt.Width;
				ocnt.Parent.Size = (Size)n;
				ocnt.Location = p;
			}
		}

		/// <summary>
		/// The Control has been assigned the Up Down Event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlCanvasNipple_MouseMoveUD(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Control ocnt = (Control)sender;
				ocnt.Visible=false;
				Point p = ((Point)ocnt.Parent.PointToClient(MousePosition));
				p.Y = p.Y - ClickOffset.Y;
				p.X = (ocnt.Parent.Width - 5) /2;
				Point n =p;
				n.X = ocnt.Parent.Width;
				n.Y = n.Y + ocnt.Height;
				ocnt.Parent.Size = (Size)n;
				ocnt.Location = p;
			}
		}

		/// <summary>
		/// The Control has been assigned the Left and Right Event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlCanvasNipple_MouseMoveLR(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Control ocnt = (Control)sender;
				ocnt.Visible=false;
				Point p = ((Point)ocnt.Parent.PointToClient(MousePosition));
				p.X = p.X - ClickOffset.X;
				p.Y = (ocnt.Parent.Height - 5) /2;
				Point n =p;
				n.Y = ocnt.Parent.Height;
				n.X = n.X +ocnt.Width;
				ocnt.Parent.Size = (Size)n;
				ocnt.Location = p;
			}
		}

		/// <summary>
		/// Set the Canvass Nipples offset Point
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlCanvasNipple_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			UndoOnce=false;
			NoUndo=true;
			ClickOffset = new Point(e.X,e.Y);
		}

		private void Control_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Control_MouseUpRoot();
		}

		private void Component_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Control sitem = null;
			int sel =0;
			string group = DateTime.UtcNow.ToString();
			foreach(Control item in pnlComponents.Controls)
			{
				if (_nipplecontainers[item] != null)
				{
					if (_moving)
					{
						DataView view  = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
						view.RowFilter =  "quName = '" + item.Name + "'";
						if (view.Count > 0)
						{
							CreateUndoRow(view,group);
							if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
							{
								view[0]["quX"] = item.Left;
								view[0]["quY"] = item.Top;
							}
							else
							{
								view[0]["quWizX"] = item.Left;
								view[0]["quWizY"] = item.Top;
							}
						}
						pgMain.Refresh();
					}

					if (sitem == null)
					{
						sitem = item;
						sel++;
					}
					else 
					{
						sitem = null;
						sel++;
					}
				}
			}
			if (sel == 1)
			{			
				frmDesigner_SelectedControl(sitem);
			}
			else if (sel > 1)
			{
				frmDesigner_SelectedControls();
			}
			else if (sel == 0)
			{
				frmDesinger_SelectCanvas();
			}
			_moving = false;

		}

		private void Control_MouseUpRoot()
		{
			Control sitem = null;
			int sel =0;
			string group = DateTime.UtcNow.ToString();
			foreach(Control item in enquiryForm1.Controls)
			{
				if (_nipplecontainers[item] != null)
				{
					sel++;
					if (sitem == null) sitem = item; else sitem = null;

					if (_moving)
					{
						DataView view  = Global.GetLiveRows(_edata.Tables["QUESTIONS"]);
						view.RowFilter =  "quName = '" + item.Name + "'";
						if (view.Count > 0)
						{
							CreateUndoRow(view, group);
							if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
							{
								view[0]["quX"] = item.Left;
								view[0]["quY"] = item.Top;
							}
							else
							{
								view[0]["quWizX"] = item.Left;
								view[0]["quWizY"] = item.Top;
							}
						}
						pgMain.Refresh();
					}
				}
			}
			if (sel == 1)
			{			
				frmDesigner_SelectedControl(sitem);
			}
			else if (sel > 1)
			{
				frmDesigner_SelectedControls();
			}
			else if (sel == 0)
			{
				frmDesinger_SelectCanvas();
			}
			_moving = false;
		}

		private void Control_Visible(object sender, EventArgs e)
		{
			if (((Control)sender).Visible==true)
				RecursiveDisable((Control)sender);
		}
			
		/// <summary>
		/// Internal Designer Code for Control Movement
		/// Mouse Down Selects and set the Mouse Position Offset
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_selectedcontrol != ((Control)sender).Name)
				_selectedcontrol = ((Control)sender).Name;
	
			ClickOffset = new Point(e.X,e.Y);
			Control cnt = (Control)sender;
			
			if (_multi == false && _nipplecontainers[cnt] == null) UnselectAll();
			if (_multi == true)
			{
				if (_nipplecontainers[cnt] != null) 
					RemoveNipples(cnt);
				else
					AddNipplesToObject(cnt);
			}	
			else
				AddNipplesToObject(cnt);
		}

		/// <summary>
		/// Internal Designer Code for Control Movement
		/// Move Move Moves any Selected Controls
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Control_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
                enquiryForm1.AutoScroll = false;
                Control ocnt = (Control)sender;
				if (_moving == false)
				{
					Rectangle M = new Rectangle(ocnt.PointToClient(MousePosition),new Size(1,1));
					Rectangle B = ocnt.Bounds;
					B.Location = new Point(0,0);
					if (Rectangle.Intersect(B,M) != new Rectangle(0,0,0,0))
						if ((e.Button == MouseButtons.Left && ((Math.Abs(ClickOffset.X - e.X) > 2) || (Math.Abs(ClickOffset.Y - e.Y) > 2))) || _moving)
							_moving=true;
				}
				else
				{
					Point p = ((Point)enquiryForm1.PointToClient(MousePosition));
					p.X = p.X - ClickOffset.X;
					p.Y = p.Y - ClickOffset.Y;
					int ox = ocnt.Left;
					int oy = ocnt.Top;
					foreach(Control item in enquiryForm1.Controls)
					{
						if (_nipplecontainers[item] != null)
						{
							int y = p.Y + (item.Top - oy);
							int x = p.X + (item.Left - ox);
							item.Location = new Point(x,y);
						}
					}
					ocnt.Location = p;
				}
			}
		}


		#endregion

		#region Private Component Hooks
		/// <summary>
		/// Internal Designer Code for Control Component Movement
		/// Mouse Down Selects and set the Mouse Position Offset
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Component_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (_selectedcontrol != ((Control)sender).Name)
			{
				_selectedcontrol = ((Control)sender).Name;
			}
			ClickOffset = new Point(e.X,e.Y);
			Control cnt = (Control)sender;
			if (_multi == false && _nipplecontainers[cnt] == null)
				UnselectAll(true);
			if (_multi == true)
			{
				if (_nipplecontainers[cnt] != null) 
					RemoveNipples(cnt);
				else
					AddNipplesToObject(cnt);
			}	
			else
				AddNipplesToObject(cnt);
		}

		/// <summary>
		/// Internal Designer Code for Component Control Movement
		/// Move Move Moves any Selected Controls
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Component_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Control ocnt = (Control)sender;
				if (_moving == false)
				{
					Rectangle M = new Rectangle(ocnt.PointToClient(MousePosition),new Size(1,1));
					Rectangle B = ocnt.Bounds;
					B.Location = new Point(0,0);
					if (Rectangle.Intersect(B,M) != new Rectangle(0,0,0,0))
						if ((e.Button == MouseButtons.Left && ((Math.Abs(ClickOffset.X - e.X) > 2) || (Math.Abs(ClickOffset.Y - e.Y) > 2))) || _moving)
							_moving=true;
				}
				else
				{
					Point p = ((Point)pnlComponents.PointToClient(MousePosition));
					p.X = p.X - ClickOffset.X;
					p.Y = p.Y - ClickOffset.Y;
					int ox = ocnt.Left;
					int oy = ocnt.Top;
					foreach(Control item in pnlComponents.Controls)
					{
						if (_nipplecontainers[item] != null)
						{
							int y = p.Y + (item.Top - oy);
							int x = p.X + (item.Left - ox);
							item.Location = new Point(x,y);
						}
					}
					ocnt.Location = p;
				}
			}
		}
		#endregion

		#region Private Enquiry Form Hooks
		private void pnlCanvass_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            if (_menustatus)
            {
                using (Pen p1 = new Pen(Color.White, 2))
                {
                    e.Graphics.DrawRectangle(p1, 1, 1, pnlCanvass.Width - 2, pnlCanvass.Height - 2);
                }
            }
		}

		private void enquiryForm1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            try
            {
                if (FinalLassoo.Location == new Point(-1, -1))
                    FinalLassoo = new Rectangle((Point)enquiryForm1.PointToClient(Lassoo.Location), new Size(-1, -1));
                else
                    FinalLassoo = new Rectangle((Point)enquiryForm1.PointToClient(FinalLassoo.Location), FinalLassoo.Size);
            }
            catch { }
            try
			{
				int sel = 0;
				Control sitem = null;
				ActivateMenus();
				Lassoo = new Rectangle(-1,-1,0,0);
				if (menuCollection1.ButtonDown.IndexOf("|Pointer") == -1)
				{
					MenuButton B = menuCollection1.GetMenuButton();
					_edata.Tables["CONTROLS"].DefaultView.RowFilter = "ctrlID = " + B.ReturnKey;
					if (FinalLassoo.Size == new Size(-1,-1))
						AddControl(B.ButtonText,Convert.ToString(_edata.Tables["CONTROLS"].DefaultView[0]["ctrlWinType"]),Convert.ToInt16(B.ReturnKey),FinalLassoo.X,FinalLassoo.Y).Ctrl.Invalidate();
					else
						AddControl(B.ButtonText,Convert.ToString(_edata.Tables["CONTROLS"].DefaultView[0]["ctrlWinType"]),Convert.ToInt16(B.ReturnKey),FinalLassoo.X,FinalLassoo.Y, FinalLassoo.Width,FinalLassoo.Height).Ctrl.Invalidate();
				}
				else
				{										   
					if (_multi==false)
					{
						UnselectAll();
					}
					foreach(Control item in enquiryForm1.Controls)
					{
						if (item.Visible==true)
						{
							Rectangle r = new Rectangle(item.Location,item.Size);
							if (Rectangle.Intersect(r,FinalLassoo) != new Rectangle(0,0,0,0))
							{
								sel++;
								if (AddNipplesToObject(item)) sitem = item;
							}
						}
					}
					if (sel == 0)
					{
						frmDesinger_SelectCanvas();
					}
					else if (sel == 1)
					{
						frmDesigner_SelectedControl(sitem);
					}
					else if (sel > 1)
					{
						frmDesigner_SelectedControls();
					}
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
			finally
			{
				enquiryForm1.Invalidate();
				if (OldLassoo != new Rectangle(-1,-1,0,0))
					Crownwood.Magic.Common.DrawHelper.DrawDragRectangle(OldLassoo,1);
				OldLassoo = new Rectangle(-1,-1,0,0);
				FinalLassoo = new Rectangle(-1,-1,0,0);
			}
		}

		private void enquiryForm1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			Blue.Focus();
			Point XY = MousePosition;
			Lassoo.Location = XY;
		}

		private void enquiryForm1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				if (Lassoo.Location != new Point(-1,-1))
				{
					Point XY = MousePosition;
					XY.X = XY.X - Lassoo.X;
					XY.Y = XY.Y - Lassoo.Y;
					Lassoo.Size = (Size)XY;
					
					if (e.X >= enquiryForm1.Width)
						Lassoo.Width = enquiryForm1.Width - enquiryForm1.PointToClient(Lassoo.Location).X;

					if (e.Y >= enquiryForm1.Height)
						Lassoo.Height = enquiryForm1.Height - enquiryForm1.PointToClient(Lassoo.Location).Y;
				
					if (OldLassoo != new Rectangle(-1,-1,0,0))
						Crownwood.Magic.Common.DrawHelper.DrawDragRectangle(OldLassoo,1);

					if (Lassoo != new Rectangle(-1,-1,0,0))
					{

						
						FinalLassoo = Lassoo;
						if (FinalLassoo.Width <0)
						{
							if (e.X <= 0 )
							{
								FinalLassoo.X = enquiryForm1.PointToScreen(enquiryForm1.Location).X;
								FinalLassoo.Width = OldLassoo.Right - FinalLassoo.Left;
							}
							else
							{
								FinalLassoo.X = FinalLassoo.X + FinalLassoo.Width;
								FinalLassoo.Width = FinalLassoo.Width * -1;
							}
						}
						if (FinalLassoo.Height <0)
						{
							if (e.Y <= 0)
							{
								FinalLassoo.Y = enquiryForm1.PointToScreen(enquiryForm1.Location).Y;
								FinalLassoo.Height = OldLassoo.Bottom - FinalLassoo.Top;
							}
							else
							{
								FinalLassoo.Y = FinalLassoo.Y + FinalLassoo.Height;
								FinalLassoo.Height = FinalLassoo.Height * -1;
							}
						}

						Crownwood.Magic.Common.DrawHelper.DrawDragRectangle(FinalLassoo,1);
					}
					OldLassoo = FinalLassoo;
				}
			}
		}

		private void enquiryForm1_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
		{
			e.Effect = DragDropEffects.Copy;
		}


		private void enquiryForm1_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
		{
			if (_dragsource == _dragsources.scToolbox)
			{
				Point n = new Point(e.X,e.Y);
				n = enquiryForm1.PointToClient(n);
				_edata.Tables["CONTROLS"].DefaultView.RowFilter = "ctrlID = " + menuCollection1.GetMenuButton().ReturnKey;
				AddControl(menuCollection1.GetMenuButton().ButtonText,Convert.ToString(_edata.Tables["CONTROLS"].DefaultView[0]["ctrlWinType"]),Convert.ToInt16(menuCollection1.GetMenuButton().ReturnKey),n.X,n.Y);
			}
			else if (_dragsource == _dragsources.scListFields)
			{
				if (menuCollection1.ButtonDown == "General|Pointer")
					menuCollection1.ButtonDown = "General|TextBox";
				MenuButton B = menuCollection1.GetMenuButton();
				_edata.Tables["CONTROLS"].DefaultView.RowFilter = "ctrlID = " + B.ReturnKey;
				Point n = new Point(e.X,e.Y);
				n = enquiryForm1.PointToClient(n);
                enquiryForm1.ControlAdded -= new ControlEventHandler(this.frmDesigner_ControlAdded);
                EnquiryControl ec = AddControl(B.ButtonText, Convert.ToString(_edata.Tables["CONTROLS"].DefaultView[0]["ctrlWinType"]), Convert.ToInt16(B.ReturnKey), n.X, n.Y, DBNull.Value, DBNull.Value);
                enquiryForm1.ControlAdded += new ControlEventHandler(this.frmDesigner_ControlAdded);
                AddNipplesToObject(ec.Ctrl);
                DataRow rw = ec.Row;
				Control c = ec.Ctrl;
                if (c is eComponent) rw["quWidth"] = 100;
                rw["quTable"] = _header.DataBuilder.Call;
                if (_header.Binding != FWBS.OMS.EnquiryEngine.EnquiryBinding.BusinessMapping)
                    rw["quFieldName"] = lstListFields.Items[lstListFields.SelectedIndex];
                else
                {
                    if (cmbViewFields.SelectedValue == DBNull.Value)
                        rw["quProperty"] = lstListFields.Items[lstListFields.SelectedIndex];
                    else
                    {
                        rw["quExtendedData"] = Convert.ToString(cmbViewFields.SelectedValue);
                        rw["quFieldName"] = lstListFields.Items[lstListFields.SelectedIndex];
                    }
                }
				bool err=false;
				int i=0;
				string si="";
				do
				{
					try
					{
						err=false;
						if (i > 0) si = i.ToString();
						i++;
						rw["quName"] = RemoveInvalidChars(Convert.ToString(lstListFields.Items[lstListFields.SelectedIndex])) + si;
					}
					catch
					{
						err=true;
					}
				}
				while (err);
				c.Name = Convert.ToString(rw["quName"]);
                enquiryForm1.RenderControls(true);
                frmDesigner_AutoFieldCheck();
			}
		}

		private void enquiryForm1_PageChanged(object sender, FWBS.OMS.UI.Windows.PageChangedEventArgs e)
		{
			if (e.PageType == FWBS.OMS.UI.Windows.EnquiryPageType.Start)
			{
				pnlWelcome.Visible=true;
				pnlEnquiry.Visible=false;
			}
			else if (e.PageType == FWBS.OMS.UI.Windows.EnquiryPageType.Enquiry)
			{
				pnlWelcome.Visible=false;
				pnlEnquiry.Visible=true;
			}
			enquiryForm1.Focus();
			//UnselectAll(true);
			lnkRemovePage.Enabled = (enquiryForm1.PageCount != 0);
			//frmDesinger_SelectCanvas();
		}

		#endregion

		#region Private Enquiry Wizard
		private void lnkRemovePage_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
            if (FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetResource("REMOVEPAGE", "Are you sure you want to Remove Page ''%1%''", "", this.enquiryForm1.PageName).Text, Session.CurrentSession.Resources.GetResource("OMSDESIGNER", "OMS Designer", "").Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				short p = enquiryForm1.PageNumber;
				lnkRemovePage.Enabled=false;
				_edata.Tables["Pages"].Rows[p].Delete();
				enquiryForm1.RenderControls();
				enquiryForm1.AutoScroll= false;				
				pgMain.Refresh();
				lnkRemovePage.Enabled = (enquiryForm1.PageCount != 0);
				enquiryForm1.ClearPageFlowHistory();
				enquiryForm1.GotoPage(p,true,true,true);
				frmDesinger_SelectCanvas();
			}
		}
		private void lnkAddPage_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			lnkAddPage.Enabled=false;
			DataRow r =_edata.Tables["Pages"].NewRow();

			if (enquiryForm1.PageCount == 0)
				r["pgeOrder"] = 10;
			else
				r["pgeOrder"] = Convert.ToInt32(_edata.Tables["Pages"].Rows[enquiryForm1.PageCount-1]["pgeOrder"]) + 10;
			
			
			string pagename = "PAGE" + Convert.ToString(r["pgeOrder"]);

			int A = 0;
			DataView _view = new DataView(_edata.Tables["Pages"]);
			_view.RowFilter = "pgeName = '" + pagename + "'";
			while(_view.Count != 0)
			{
				pagename = "PAGE" + Convert.ToString(r["pgeOrder"]) + "A";
				if (A > 0) pagename+=A.ToString();
				_view.RowFilter = "pgeName = '" + pagename + "'";
				A++;
			}
			
			r["pgeName"] = pagename;

			_edata.Tables["Pages"].Rows.Add(r);
			lnkRemovePage.Enabled = (enquiryForm1.PageCount != 0);
			btnNext.Enabled=true;
			pgMain.Refresh();
			lnkAddPage.Enabled=true;
		}

		private void lnkInsert_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
		
		}

		#endregion

		#region Private Fields Form Methods
		private void cmbFields_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (cmbFields.SelectedValue == DBNull.Value)
			{
				frmDesinger_SelectCanvas();
			}
			else if (cmbFields.Text != "")
			{
				UnselectAll(false);
                try
                {
                    if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Wizard)
                        enquiryForm1.GotoControl(cmbFields.Text, false);
                    AddNipplesToObject((Control)cmbFields.SelectedValue);
                    frmDesigner_SelectedControl((Control)cmbFields.SelectedValue);
                }
                catch { }
			}

		}

		private void cmbFields_Enter(object sender, System.EventArgs e)
		{
			cmbFields.Tag = "1";
		}

		private void cmbFields_Leave(object sender, System.EventArgs e)
		{
			cmbFields.Tag = null;
		}

		private void cmbFields_VisibleChanged(object sender, System.EventArgs e)
		{
			if (cmbFields.SelectedValue == null) cmbFields.SelectedIndex =-1;
		}

        string lastSelection = "";

		private void lstListFields_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && ((Math.Abs(tbDrag.X - e.X) > 3) || (Math.Abs(tbDrag.Y - e.Y) > 3)))
			{
				_dragsource = _dragsources.scListFields;

                if (menuCollection1.ButtonDown.EndsWith("Pointer"))
                {
                    if (lastSelection == "") lastSelection = "General|TextBox";
                    menuCollection1.ButtonDown = lastSelection;
                }
                
                lastSelection = menuCollection1.ButtonDown;
				lstListFields.DoDragDrop("TextBox", DragDropEffects.Copy | 
					DragDropEffects.Move);
			}
		}
		#endregion

		#region Private Static Object Clickers

		private void labQuestionPage_Click(object sender, System.EventArgs e)
		{
			UnselectAll();
			pnlPage.Visible=true;
			AddNipplesToObject(labQuestionPage,true);
			_page.EnquiryRow = enquiryForm1.CurrentPage;
			_page.Page = enquiryForm1.PageNumber;
			pgMain.SelectedObject = _page;
			pgMain.Refresh();
		}


		private void pnlWorkspace_Click(object sender, System.EventArgs e)
		{
			DeActivateMenus();
			UnselectAll(true);
		}

		private void labWelcome_Click(object sender, System.EventArgs e)
		{
			frmDesinger_SelectCanvas();
		}

		private void WelcomePropety(object sender, System.EventArgs e)
		{
			UnselectAll(true);
			AddNipplesToObject(labWelcome,true);
			pgMain.SelectedObject = _header;
		}

		private void WelcomeTextProperty(object sender, System.EventArgs e)
		{
			UnselectAll(true);
			AddNipplesToObject(labDescription,true);
			pgMain.SelectedObject = _header;
		}
		#endregion

		#region Private Control Property Editors

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			try
			{
				object n = e.ChangedItem.Parent;
				// DMB 25/02/2004 handler for the format property previously was not being saved
                if (e.ChangedItem.Label == "Format" && e.ChangedItem.Value.GetType() == typeof(string))
				{
					if(Convert.ToString(GetPropertyView[0]["quformat"])!= Convert.ToString(e.ChangedItem.Value))
					{
						GetPropertyView[0]["quformat"]= e.ChangedItem.Value;
						_isdirty =true;
					}	
					return;
				}
				if (e.ChangedItem.Label=="TabStop" || e.ChangedItem.Label=="Anchor" || e.ChangedItem.Label=="Text" || e.ChangedItem.Label=="RightToLeft" || e.ChangedItem.Label=="Visible" || e.ChangedItem.Label=="Cursor" || e.ChangedItem.Label=="Image" || e.ChangedItem.Label =="ReadOnly")
				{
					SetPropertyByName(GetPropertyControl, e.ChangedItem.Label,e.OldValue);
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("PUGTPT", "Property cannot be set, if possible please use the General Tab to set '%1%' property. The property will be reset back.",e.ChangedItem.Label);
					return;
				}
			}
			catch
			{
				propertyGrid1.ResetSelectedProperty();
				return;
			}
				
			if (e.ChangedItem.Label=="Casing")
			{
				string group = DateTime.UtcNow.ToString();
				CreateUndoRow(GetPropertyView,group);
				GetPropertyView[0]["quCasing"] = e.ChangedItem.Value.ToString();
			}
			else if (e.ChangedItem.Label=="Size" && e.ChangedItem.PropertyDescriptor.PropertyType == typeof(System.Drawing.Size))
			{
				string group = DateTime.UtcNow.ToString();
				CreateUndoRow(GetPropertyView,group);
				GetPropertyView[0]["quWidth"] = ((System.Drawing.Size)e.ChangedItem.Value).Width;
				GetPropertyView[0]["quHeight"] = ((System.Drawing.Size)e.ChangedItem.Value).Height;
			}
			else if (e.ChangedItem.Label=="Width")
			{
				string group = DateTime.Now.ToString();
				CreateUndoRow(GetPropertyView,group);
				GetPropertyView[0]["quWidth"] = (int)e.ChangedItem.Value;
			}
			else if (e.ChangedItem.Label=="Height")
			{
				string group = DateTime.UtcNow.ToString();
				CreateUndoRow(GetPropertyView,group);
				GetPropertyView[0]["quHeight"] = (int)e.ChangedItem.Value;
			}
			else if (e.ChangedItem.Label=="Location")
			{
				if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
				{
					string group = DateTime.UtcNow.ToString();
					CreateUndoRow(GetPropertyView,group);
					GetPropertyView[0]["quX"] = ((Point)e.ChangedItem.Value).X;
					GetPropertyView[0]["quY"] = ((Point)e.ChangedItem.Value).Y;
				}
				else
				{
					string group = DateTime.UtcNow.ToString();
					CreateUndoRow(GetPropertyView,group);
					GetPropertyView[0]["quWizX"] = ((Point)e.ChangedItem.Value).X;
					GetPropertyView[0]["quWizY"] = ((Point)e.ChangedItem.Value).Y;
				}
			}
			else if (e.ChangedItem.Label=="X")
			{
				string group = DateTime.UtcNow.ToString();
				CreateUndoRow(GetPropertyView,group);
				if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
					GetPropertyView[0]["quX"] = (int)e.ChangedItem.Value;
				else
					GetPropertyView[0]["quWizX"] = (int)e.ChangedItem.Value;
			}
			else if (e.ChangedItem.Label=="Y")
			{
				string group = DateTime.UtcNow.ToString();
				CreateUndoRow(GetPropertyView,group);
				if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
					GetPropertyView[0]["quY"] = (int)e.ChangedItem.Value;
				else
					GetPropertyView[0]["quWizY"] = (int)e.ChangedItem.Value;
			}
            else if (e.ChangedItem.Label == "CaptionWidth")
            {
                string group = DateTime.UtcNow.ToString();
                CreateUndoRow(GetPropertyView, group);
                GetPropertyView[0]["quCaptionWidth"] = (int)e.ChangedItem.Value;
            }
            else if (e.ChangedItem.Label == "CueText")
            {
                ((FWBS.OMS.UI.Windows.Admin.EnquiryControl)pgMain.SelectedObject).CustomProperties.Add(new FWBS.OMS.UI.Windows.Admin.EnquiryControl.Custom("CueTextCode", ((CodeLookupDisplay)e.ChangedItem.Value).Code));
                _isdirty = true;
            }
            else if (e.ChangedItem.Parent.Label == "CueText")
            {
                if (e.ChangedItem.Label == "Code")
                {
                    ((FWBS.OMS.UI.Windows.Admin.EnquiryControl)pgMain.SelectedObject).CustomProperties.Add(new FWBS.OMS.UI.Windows.Admin.EnquiryControl.Custom("CueTextCode", ((CodeLookupDisplay)e.ChangedItem.Parent.Value).Code));
                    _isdirty = true;
                }
                else if (e.ChangedItem.Label == "Description")
                {
                    if (string.IsNullOrEmpty(e.ChangedItem.Value.ToString()))
                    {
                        ((CodeLookupDisplay)e.ChangedItem.Parent.Value).Code = string.Empty;
                        ((FWBS.OMS.UI.Windows.Admin.EnquiryControl)pgMain.SelectedObject).CustomProperties.Add(new FWBS.OMS.UI.Windows.Admin.EnquiryControl.Custom("CueTextCode", ((CodeLookupDisplay)e.ChangedItem.Parent.Value).Code));

                        _isdirty = true;
                    }
                    else
                    {
                        ((CodeLookupDisplay)e.ChangedItem.Parent.Value).Description = e.ChangedItem.Value.ToString();
                    }
                }
                else if (e.ChangedItem.Label == "Help")
                {
                    ((CodeLookupDisplay)e.ChangedItem.Parent.Value).Help = e.ChangedItem.Value.ToString();
                }
                else if (e.ChangedItem.Label == "UI Culture")
                {
                    ((CodeLookupDisplay)e.ChangedItem.Parent.Value).UICulture = e.ChangedItem.Value.ToString();
                }
            }
            else
			{
				SortedList propertylist;
				if (GetPropertyView[0]["qucustom"] is byte[])
				{
					Byte[] output = (byte[])GetPropertyView[0]["qucustom"];
					MemoryStream istream = new MemoryStream(output,0,output.Length);
					BinaryFormatter formatteri = new BinaryFormatter();
					try
					{
						object input = formatteri.Deserialize(istream);
						istream.Close();
						if (input is SortedList)
							propertylist = (SortedList)input;
						else
							propertylist = new SortedList();
					}
					catch
					{
						propertylist = new SortedList();
					}
				}
				else
					propertylist = new SortedList();

				string group = DateTime.UtcNow.ToString();
				CreateUndoRow(GetPropertyView, group);

                GridItem n = e.ChangedItem;
                if (n.Parent.GridItemType == GridItemType.Property)
                    n = n.Parent;

                propertylist.Remove(n.Label);
				propertylist.Add(n.Label,n.Value);

                try
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.Serialize(stream, propertylist);
                        GetPropertyView[0]["qucustom"] = stream.ToArray();
                    }
                    propertylist = null;
                }
                catch { }

				string vn = "";
				FWBS.Common.ConfigSetting cfg = new FWBS.Common.ConfigSetting(GetPropertyView[0].Row,"qufilter");
				cfg.Current = "custom";
				try
				{
					Type t = GetPropertyControl.GetType();
					System.Reflection.PropertyInfo p = t.GetProperty(n.Label);
					object[] attrs = p.PropertyType.GetCustomAttributes(typeof(System.ComponentModel.TypeConverterAttribute), true);
					if (attrs.Length > 0)
					{
						System.ComponentModel.TypeConverterAttribute typeconv = (System.ComponentModel.TypeConverterAttribute)attrs[0];
                        Type tct = Session.CurrentSession.TypeManager.Load(typeconv.ConverterTypeName);
						System.ComponentModel.TypeConverter conv = (System.ComponentModel.TypeConverter)tct.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
                        vn = conv.ConvertToInvariantString(n.Value);
					}
					else
					{
						vn = Convert.ToString(n.Value, System.Globalization.CultureInfo.InvariantCulture);
					}
				}
				catch
				{
				}
                cfg.SetString(n.Label, vn);
				cfg.Synchronise();

				foreach (object j in pgMain.SelectedObjects)
				{
					((FWBS.OMS.UI.Windows.Admin.EnquiryControl)j).CustomProperties.Add(new FWBS.OMS.UI.Windows.Admin.EnquiryControl.Custom(n.Label,vn));
                    if (n.Label == "CaptionTop")
                    {
                        ((FWBS.OMS.UI.Windows.Admin.EnquiryControl)j).CaptionWidth = Convert.ToBoolean(n.Value) ? 0 : eBase2.DefaultCaptionWidth;
                        pgMain.Refresh();
                    }
				}
			}
		}
        #endregion

		#region Private Enquiry _Header & _Page Methods
		//
		// _page updates the row when changed
		//
		private void _page_PageHeaderChange(object sender, System.EventArgs e)
		{
			labQuestionPage.Text = _page.PageHeader.Description;
		}


		private void _header_MarginChange(object sender, System.EventArgs e)
		{
			enquiryForm1.DockPadding.Top = _header.Margin.Y;
			enquiryForm1.DockPadding.Left = _header.Margin.X;
			enquiryForm1.DockPadding.Right = enquiryForm1.DockPadding.Left;
			enquiryForm1.DockPadding.Bottom = enquiryForm1.DockPadding.Top;
			if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Wizard)
			{
				enquiryForm1.RenderControls();
				enquiryForm1.AutoScroll= false;				
			}
		}

		private void _header_DataBuilderChange(object sender, System.EventArgs e)
		{
				string val = _header.DataBuilder.Call;
				fieldsForm.Text = val;
				try
				{
					if (_header.Binding == EnquiryBinding.BusinessMapping)
					{
						lstListFields.Items.Clear();
                        EnquiryEngine.EnquiryPropertyCollection props = Enquiry.GetObjectProperties(Convert.ToString(_edata.Tables["ENQUIRY"].Rows[0]["enqSource"]));
                        List<string> vals = new List<string>();
                        foreach (EnquiryEngine.EnquiryProperty prop in props)
                            vals.Add(prop.Name);
						lstListFields.Items.AddRange(vals.ToArray());

						cmbViewFields.Visible = true;
						DataTable dt = ExtendedData.GetExtendedDatas(_header.DataBuilder.Source).Copy();
						DataRow dr = dt.NewRow();
						dr["extcode"] = DBNull.Value;
						dr["cddesc"] = Session.CurrentSession.Resources.GetResource("PROPERTIES", "Properties", "").Text;
						dt.Rows.Add(dr);
						dt.DefaultView.Sort = "extcode";
						cmbViewFields.BeginUpdate();
						cmbViewFields.DataSource = dt;
						cmbViewFields.DisplayMember = "cddesc";
						cmbViewFields.ValueMember = "extcode";
						cmbViewFields.EndUpdate();
					}
					else
					{
						cmbViewFields.Visible = false;
						cmbViewFields.DataSource = null;
						lstListFields.Items.Clear();
						lstListFields.Items.AddRange(_header.DataBuilder.TestAndGetFields());
					}
				}
				catch
				{
				}
				
				foreach (DataRow rw in _edata.Tables["QUESTIONS"].Rows)
				{
					if (Convert.ToString(rw["quTable"]) != "")
					{
						rw["quTable"] = _header.DataBuilder.TableName;
					}	
				}
				frmDesigner_AutoFieldCheck();
		}

		private void _header_StandardSizeChange(object sender, System.EventArgs e)
		{
			if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Standard)
			{
				pnlCanvass.Size = _header.StandardSize;
			} 
		}

		private void _header_WizardSizeChange(object sender, System.EventArgs e)
		{
			if (enquiryForm1.Style == FWBS.OMS.UI.Windows.EnquiryStyle.Wizard)
			{
				pnlCanvass.Size = _header.WizardSize;
			} 

		}

		private void _header_WelcomeHeaderCodeChange(object sender, System.EventArgs e)
		{
			if (enquiryForm1.Enquiry != null)
				enquiryForm1.RefreshWizardWelcomePage(); 
		}

		private void _header_Dirty(object sender, System.EventArgs e)
		{
			IsDirty=true;
		}
		#endregion

		#region Private Tool Bar Menu Collection Methods
		private void menuCollection1_ButtonDoubleClick(object sender, FWBS.Common.UI.Windows.MenuButtonEventArgs e)
		{
			if (e.ReturnKey.ToString() != "0")
			{
				_edata.Tables["CONTROLS"].DefaultView.RowFilter = "ctrlID = " + e.ReturnKey;
				AddControl(e.ButtonCaption,Convert.ToString(_edata.Tables["CONTROLS"].DefaultView[0]["ctrlWinType"]),Convert.ToInt16(e.ReturnKey),50,50);
			}
		}

		private void menuCollection1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && ((Math.Abs(tbDrag.X - e.X) > 3) || (Math.Abs(tbDrag.Y - e.Y) > 3)))
			{
				_dragsource = _dragsources.scToolbox;
				lstListFields.DoDragDrop(menuCollection1.GetMenuButton().ButtonText, DragDropEffects.Copy | 
					DragDropEffects.Move);
			}		
		}

		private void menuCollection1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			tbDrag.X = e.X;
			tbDrag.Y = e.Y;
		}
		#endregion

		#region Private Code Window Methods

		private void tabPages_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (tabPages.SelectedTab == tpCode && (GetPropertyControl == null || tpCode.Tag != GetPropertyControl))
			{
                 var controls = CodeWindow.GetService<ICodeSurfaceControls>();
                 if (controls != null)
                 {
                     controls.Clear();
                     foreach (DataRow rw in _edata.Tables["QUESTIONS"].Rows)
                         if (rw.RowState != DataRowState.Deleted)
                             controls.Attach(Convert.ToString(rw["quName"]));
                 }

				// Set the Event List
				if (GetPropertyControl != null) 
                    dgEvents.SelectedObject = GetPropertyControl;
				else
                    dgEvents.SelectedObject = _enqobject.Script.ScriptType;
                

				tpCode.Tag = GetPropertyControl;
			}
		}

		private void Codebuilder(object sender, EventArgs e)
		{
			int pos = dgEvents.CurrentRowIndex;
			if (_header.Code == "")
			{
				ErrorBox.Show(this, new OMSException2("30004","The Enquiry Form must be saved before Scripting can be assigned",new Exception(),true));
				return;
			}
			if (_header.Script == "")
			{
                _header.Script = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_header.Code);
 			}

            CodeWindow.Show();
            CodeWindow.BringToFront();
            if (CodeWindow.HasMethod(dgEvents.SelectedMethodName))
            {
                CodeWindow.GotoMethod(dgEvents.SelectedMethodName);
            }
            else
            {
                CodeWindow.GenerateHandler(dgEvents.SelectedMethodName, new GenerateHandlerInfo() { DelegateType = dgEvents.SelectedMethodData.Delegate });
            }

		}

		private void _header_ScriptChange(object sender, System.EventArgs e)
		{
            CodeWindow.Load(_enqobject.Script.ScriptType, _enqobject.Script);

            var controls = CodeWindow.GetService<ICodeSurfaceControls>();
            if (controls != null)
            {
                controls.Clear();
                foreach (DataRow rw in _edata.Tables["QUESTIONS"].Rows)
                    if (rw.RowState != DataRowState.Deleted)
                        controls.Attach(Convert.ToString(rw["quName"]));
            }
            dgEvents.CurrentCodeSurface = CodeWindow;

            if (GetPropertyControl != null)
                dgEvents.SelectedObject = GetPropertyControl;
            else
                dgEvents.SelectedObject = _enqobject.Script.ScriptType;
			tpCode.Tag = GetPropertyControl;
		}

		#endregion

        #region Private LockState Methods

        private void LockEnquiryForm(string code)
        {
            ls.LockEnquiryFormObject(code);
            ls.MarkObjectAsOpen(code, LockableObjects.EnquiryForm);
        }

        #endregion

        #region public LockState Methods

        public void UnlockCurrentObject(string formCode)
        {
            if (Session.CurrentSession.ObjectLocking)
            {
                if (!string.IsNullOrWhiteSpace(formCode))
                {
                    if (ls.CheckObjectIsLockedByCurrentUser(formCode, LockableObjects.EnquiryForm))
                    {
                        ls.UnlockEnquiryFormObject(formCode);
                        ls.MarkObjectAsClosed(formCode, LockableObjects.EnquiryForm);
                    }
                }
            }
        }


        #endregion



        #region Public
        public new void Refresh()
		{
			
			UnselectAll(true);
			_enqobject.Designer_RefreshQuestions();
			enquiryForm1.RenderControls(true);
			enquiryForm1.AutoScroll= false;				
			base.Refresh();
		}
		#endregion

		private void Blue_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			try
			{
				tpEnquiryHeader.BringToFront();
				if (e.KeyChar == (char)27 && this.ActiveControl == Blue)
				{
					UnselectAll(true);
					frmDesinger_SelectCanvas();
				}
				else if (!_ignorekey)
				{
					pgMain.Focus();
					SendKeys.Send(e.KeyChar.ToString());
				}
				_ignorekey=false;
			}
			catch
			{}
		}

		private void _page_PageOrderChange(object sender, System.EventArgs e)
		{
			enquiryForm1.ClearPageFlowHistory();
			ArrayList pf = new ArrayList();
			DataView pages = new DataView(_edata.Tables["PAGES"],"","pgeOrder",DataViewRowState.CurrentRows);
			int p=0;
			foreach(DataRowView rv in pages)
			{
				if (Convert.ToInt32(rv["pgeOrder"]) <= _page.PageOrder) pf.Add(p); else break;
				p++;
			}
			_page.Page = p;
			enquiryForm1.AddPageFlowRange(pf,_page.Page);

		}

		private void Blue_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
		}

		private void pgMain_SelectedObjectsChanged(object sender, System.EventArgs e)
		{
			grpchg = pgMain.SelectedObjects.Length;
			grpcnt = grpchg;
		}

		private void pgMain_SelectedGridItemChanged(object sender, System.Windows.Forms.SelectedGridItemChangedEventArgs e)
		{
		}

        private void OMSToolbars_OMSButtonClick(object sender, FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventArgs e)
        {
            if (e.Button.Name == "tbScript")
            {
                if (_header.Code == "")
                {
                    ErrorBox.Show(this, new OMSException2("30004", "The Enquiry Form must be saved before Scripting can be assigned", new Exception(), true));
                    return;
                }
                if (_header.Script == "")
                {
                    _header.Script = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_header.Code);
                }

                var controls = CodeWindow.GetService<ICodeSurfaceControls>();
                if (controls != null)
                {
                    controls.Clear();
                    foreach (DataRow rw in _edata.Tables["QUESTIONS"].Rows)
                        if (rw.RowState != DataRowState.Deleted)
                            controls.Attach(Convert.ToString(rw["quName"]));
                }

                CodeWindow.Show();
                CodeWindow.BringToFront();
                ls.MarkObjectAsOpen(_enqobject.Script.Code, UI.Windows.LockableObjects.Script);
                ManageVersioningMenuOptions(false);
                CodeWindow.FormClosing += new FormClosingEventHandler(CodeWindow_FormClosing);
            }
            else if (e.Button.Name == "tbSave")
                ActionMenu_Click(mnuSave, EventArgs.Empty);
            else if (e.Button.Name == "tbNew")
                ActionMenu_Click(mnuNew, EventArgs.Empty);
            else if (e.Button.Name == "tbOpen")
                ActionMenu_Click(mnuOpen, EventArgs.Empty);
            else if (e.Button.Name == "tbSendtoBack")
                ActionMenu_Click(mnuSendToBack, EventArgs.Empty);
            else if (e.Button.Name == "tbBringtoFront")
                ActionMenu_Click(mnuBringToFront, EventArgs.Empty);
            else if (e.Button.Name == "tbToolbox")
                menuCollection1.Visible = OMSToolbars.GetButton("tbToolbox").Pushed;
            else if (e.Button.Name == "tbFields" && OMSToolbars.GetButton("tbFields").Pushed)
            {
                fieldsForm.Show();
            }
            else if (e.Button.Name == "tbFields" && !OMSToolbars.GetButton("tbFields").Pushed)
            {
                fieldsForm.Hide();
            }
            else if (e.Button.Name == "tbProperties")
                pnlProperties.Visible = OMSToolbars.GetButton("tbProperties").Pushed;
            else if (e.Button.Name == "tbStandard")
            {
                OMSToolbars.GetButton("tbWizard").Pushed = false;
                StandardMode_Click(mnuStandardMode, new EventArgs());
                _header.Settings.SetSetting("View", "Mode", FWBS.OMS.UI.Windows.EnquiryStyle.Standard.ToString());
            }
            else if (e.Button.Name == "tbWizard")
            {
                OMSToolbars.GetButton("tbStandard").Pushed = false;
                WizardMode_Click(mnuWizardMode, new EventArgs());
                _header.Settings.SetSetting("View", "Mode", FWBS.OMS.UI.Windows.EnquiryStyle.Wizard.ToString());
            }
        }

        private void CodeWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ls.MarkObjectAsClosed(_enqobject.Script.Code, UI.Windows.LockableObjects.Script);
            ManageVersioningMenuOptions(true);
        }


		private void lnkRegister_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			DataTable dt = null;
			FWBS.Common.KeyValueCollection _param = new FWBS.Common.KeyValueCollection();
			_param.Add("objCode",this.enquiryForm1.Enquiry.Code);
			_param.Add("objHelp","[Enquiry]");

			dt = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.RegisterOMSObject),null,EnquiryMode.Add,false, _param) as DataTable;
			
			if (dt != null)
			{
				string _code = Convert.ToString(dt.Rows[0]["objCode"]);
				string _description = Convert.ToString(dt.Rows[0]["objdescription"]);
				string _type = Convert.ToString(dt.Rows[0]["objType"]);
				string _web = Convert.ToBoolean(dt.Rows[0]["chkWEB"]) == true ? this.enquiryForm1.Enquiry.Code : "";
				string _win = Convert.ToBoolean(dt.Rows[0]["chkWindows"]) == true ? this.enquiryForm1.Enquiry.Code : "";
				string _pda = Convert.ToBoolean(dt.Rows[0]["chkPDA"]) == true ? this.enquiryForm1.Enquiry.Code : "";

                string xml = null;
                if (Convert.ToBoolean(dt.Rows[0]["chkDashboard"]))
                {
                    var height = Convert.ToInt16(dt.Rows[0]["seHeight"]);
                    var width = Convert.ToInt16(dt.Rows[0]["seWidth"]);
                    var priority = Convert.ToInt16(dt.Rows[0]["sePriority"]);
                    xml = DashboardConfigProvider.CreateXml(this.enquiryForm1.Enquiry.Code, new Size(width, height), priority);
                }

                try
				{
					OmsObject.Register(_code,OMSObjectTypes.Enquiry,_type,_description,Convert.ToString(dt.Rows[0]["objHelp"]),_win,_web,_pda, xml);
					lnkRegister.Enabled = !(OmsObject.IsObjectRegistered(this.enquiryForm1.Enquiry.Code,OMSObjectTypes.Enquiry));
				}
				catch (Exception ex)
				{
					ErrorBox.Show(this, ex);
				}
			}
		}

		private void cmbViewFields_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (cmbViewFields.SelectedValue == DBNull.Value)
			{
				lstListFields.Items.Clear();
                EnquiryEngine.EnquiryPropertyCollection props = Enquiry.GetObjectProperties(Convert.ToString(_edata.Tables["ENQUIRY"].Rows[0]["enqSource"]));
                List<string> vals = new List<string>();
                foreach (EnquiryEngine.EnquiryProperty prop in props)
                    vals.Add(prop.Name);
				lstListFields.Items.AddRange(vals.ToArray());
			}
			else
			{
				lstListFields.Items.Clear();
				try
				{
					FWBS.OMS.UI.Windows.Design.ExtendedDataEditor ext = new FWBS.OMS.UI.Windows.Design.ExtendedDataEditor(Convert.ToString(cmbViewFields.SelectedValue));
					lstListFields.Items.AddRange(ext.DataBuilder.TestAndGetFields());
				}
				catch{}
			}
		}

		private void timRebuildcmbFields_Tick(object sender, System.EventArgs e)
		{
			if (timRebuildcmbFields.Interval == 999999999) return;
			timRebuildcmbFields.Enabled=false;
            enquiryForm1.ClearEnquiryControls();
			cmbFields.BeginUpdate();
			cmbFields.DataSource = null;
			cmbFields.DataSource = _controls;
			if (_controls.Count > 0)
			{
				cmbFields.DisplayMember = "Name";
				cmbFields.ValueMember = "Control";
			}		
			cmbFields.EndUpdate();
			try
			{
                if (String.IsNullOrEmpty(cmbFields.ValueMember) == false && GetPropertyControl != null)
                {
                    cmbFields.SelectedValue = GetPropertyControl;
                    cmbFields_SelectedIndexChanged(sender, e);
                }
			}
			catch
			{}
		}

		private void pgMain_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
            if (e.ChangedItem != null && e.ChangedItem.Label == "Name")
				timRebuildcmbFields_Tick(this,EventArgs.Empty);
			this.IsDirty=true;
		}

		private void btnBack_Click(object sender, System.EventArgs e)
		{
			frmDesinger_SelectCanvas();
		}

		private void close_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void enquiryForm1_Rendering(object sender, System.ComponentModel.CancelEventArgs e)
		{
            pnlComponents.Controls.Clear();
		}

        private void dgEvents_NewScript(object sender, EventArgs e)
        {
            if (_header.Script == "")
            {
                _header.Script = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_header.Code);
            }
           
        }
    }

	#region The Nipple Container

	/// <summary>
	/// Value key pair object item.
	/// </summary>
	internal class SelectContainer
	{
		public uBar TL;
		public uBar TR;
		public uBar TM;
		public uBar ML;
		public uBar MR;
		public uBar BL;
		public uBar BR;
		public uBar BM;
		public static int Count = 0;

		public SelectContainer(uBar tl,uBar tr,uBar tm,uBar ml,uBar mr,uBar bl,uBar br,uBar bm)
		{
			TL = tl;
			TR = tr;
			TM = tm;
			ML = ml;
			MR = mr;
			BL = bl;
			BR = br;
			BM = bm;
			Count++;
		}


	}
	#endregion

	#region The Undo Container
	/// <summary>
	/// Undo Class
	/// </summary>
	internal class EnquiryControl
	{
		public Control Ctrl;
		public DataRow Row;

		public EnquiryControl(Control ctrl, DataRow row)
		{
			Ctrl = ctrl;
			Row = row;
		}
	}
	#endregion

	#region ControlItem
	public class ControlItem
	{
		public ControlItem(Control ctrl)
		{
			_ctrl = ctrl;
		}

		private Control _ctrl;

		public string Name
		{
			get
			{
				return _ctrl.Name;
			}
			set
			{
				_ctrl.Name = value;
			}
		}

		public Control Control
		{
			get
			{
				return _ctrl;
			}
			set
			{
				_ctrl = value;
			}
		}

		public override bool Equals(object obj)
		{
			ControlItem cobj = obj as ControlItem;
			return (cobj != null && cobj.Control == this.Control);
		}

		public override int GetHashCode()
		{
			return this.Control.GetHashCode();
		}


	}
	#endregion
}


