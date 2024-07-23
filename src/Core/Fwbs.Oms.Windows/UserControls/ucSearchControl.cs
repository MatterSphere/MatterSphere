using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.UI.Windows;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.SearchEngine;
using FWBS.OMS.UI.UserControls.ConflictSearch;
using FWBS.OMS.UI.UserControls.MDIMenu.V2Host.TreeView_Navigation;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// This user control is used as a commonly used template for searching and listing entities
    /// within the system.  The data source for the search is wrappered up by the search list
    /// object within the business layer.
    /// </summary>
    public class ucSearchControl : System.Windows.Forms.UserControl, FWBS.OMS.UI.Windows.Interfaces.IOpenOMSType, FWBS.OMS.UI.Windows.Interfaces.IOMSItem, ISupportRightToLeft, IOBjectDirty, ITabControl, IConflictSearcher
    {
		#region Events
		/// <summary>
		/// Button Enabled Rules Applied
		/// </summary>
		public event EventHandler ButtonEnabledRulesApplied = null;

		/// <summary>
		/// This event gets raised when the Command Buttons are Clicked
		/// </summary>
		public event SearchButtonEventHandler SearchButtonCommands = null;

		/// <summary>
		/// This event gets raised when the Command Buttons are Clicked
		/// </summary>
		public event CommandExecutingEventHandler CommandExecuting = null;

		/// <summary>
		/// This event gets raised when the Command Buttons are Clicked
		/// </summary>
		public event CommandExecutedEventHandler CommandExecuted = null;

		/// <summary>
		/// This event gets raised when the search type gets changed.  This is usually
		/// changed when the search type combo box changes.
		/// </summary>
		public event EventHandler SearchTypeChanged = null;

		/// <summary>
		/// This event gets raised when an item gets successfully picked form the search list.
		/// </summary>
		public event EventHandler ItemSelected = null;

		/// <summary>
		/// This event gets raised when an item gets successfully picked form the search list.
		/// </summary>
		public event SearchItemHoverEventHandler ItemHover = null;

		/// <summary>
		/// This event gets raised after an item gets successfully picked from the search list.
		/// </summary>
		public event EventHandler ItemHovered = null;

		/// <summary>
		/// An event that indicates whether the event is in select / search mode.
		/// </summary>
		public event SearchStateChangedEventHandler StateChanged = null;

		/// <summary>
		/// An event that gets raised when a new OMS type object needs to be opened in
		/// a navigational format on the dialog form.
		/// </summary>
		public event NewOMSTypeWindowEventHandler NewOMSTypeWindow = null;

		/// <summary>
		/// An event that gets raised when the search control completes a search.
		/// </summary>
		public event SearchCompletedEventHandler SearchCompleted = null;

		/// <summary>
		/// An event that gets chucked when a OMS Item is Opened
		/// </summary>
		public event EventHandler OpenedOMSItem = null;

		/// <summary>
		/// An event that gets chucked when a OMS Item is Closed
		/// </summary>
		public event EventHandler ClosedOMSItem = null;

		/// <summary>
		/// Search List Selected Item Double Click
		/// </summary>
		public event EventHandler SelectedItemDoubleClick = null;

		/// <summary>
		/// Reports a Dirty Somthing
		/// </summary>
		public event EventHandler Dirty = null;

		/// <summary>
		/// on Search List Object Load
		/// </summary>
		public event EventHandler SearchListLoad = null;

		/// <summary>
		/// Quick Filter Changed Event
		/// </summary>
		public event EventHandler FilterChanged = null;

		/// <summary>
		/// Before the Cell is Displayed Event
		/// </summary>
		public event CellDisplayEventHandler BeforeCellDisplay;

        /// <summary>
        /// Columns or direction of sorting Changed Event
        /// </summary>
        public event EventHandler<DataGridViewEx.SortDataEventArgs> SortChanged;

        #endregion

        #region Fields
        private ActiveState _trashactivestate = ActiveState.All;
		private string _searchlistcode = "";
		private string _searchlisttype = "";
        private SaveSearchType _saveSearch = SaveSearchType.Never;

		private Common.TriState _multiSelect = Common.TriState.Null;
		private bool _quickFilterVisible = true;
		private bool _autojump = true;
		private System.Windows.Forms.Panel pnlMain;
		private System.Windows.Forms.Panel pnlSearch;
		private FWBS.Common.UI.Windows.eXPPanel pnlResultsBack;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.ComponentModel.IContainer components;
		private Control _quickfiltercontrol;
		private bool _pnlcbselector = true;
		private bool _tdpgraphicaltools = true;
		private bool _pnlsearchtop = true;

		private bool _ecmdsearch = false;
		private bool _ecmdselect = false;
		private bool _ecmdadd = false;
		private bool _ecmdedit = false;
		private bool _ecmdactive  = false;
		private bool _ecmdtrash   = false;
		private bool _ecmdrestore = false;
		private bool _ecmddelete  = false;
		
		private Button _cmdsearch = null;
		private Button _cmdselect = null;
		private Button _cmdadd = null;
		private Button _cmdedit = null;
		private Button _cmdactive  = null;
		private Button _cmdtrash   = null;
		private Button _cmdrestore = null;
		private Button _cmddelete  = null;
		private LinkLabel _cmdresetall  = null;

		private OMSToolBarButton _tbactive  = null;
		private OMSToolBarButton _tbtrash   = null;
		private OMSToolBarButton _tbrestore = null;
		private OMSToolBarButton _tbdelete  = null;
		private OMSToolBarButton _tbselect  = null;
		private OMSToolBarButton _tbedit    = null;
		private Hashtable _incsearch = new Hashtable();
        private int pnlbutCount = 0;
        private EnquiryForm _enqsearch = null;

		private Form _parentform = null;
		private IButtonControl _lastacceptbutton = null;

		private frmHourGlass hrg;
        private NavigationPopupContainer _columnSettingContainer;
        private UserControls.ColumnSettings.ColumnSettingsPopUp _columnSettingsPopUp;

        private ArrayList _disabledonnowrows = new ArrayList();
		private ArrayList _disabledomultiselect = new ArrayList();

		private SortedList _multireports = new SortedList();

		private int _lastsearchtype = -1;
        private bool resetPagination = true;

        private DataTable VisibleResults;

		/// <summary>
		/// Search DataTable 
		/// </summary>;
		private DataTable dtCT = new DataTable();

		/// <summary>
		/// A reference to the last row selected when the column sort is used.
		/// </summary>
		private DataRow[] lastSelected = null;

		/// <summary>
		/// A panel that holds the buttons.
		/// </summary>
		private System.Windows.Forms.FlowLayoutPanel pnlButtons;
		/// <summary>
		/// The Main DataGrid
		/// </summary>
		public DataGridViewEx dgSearchResults;
		/// <summary>
		/// The current search list object being used to run the search.
		/// </summary>
		private FWBS.OMS.SearchEngine.SearchList _searchList = null;
        /// <summary>
		/// Holds the return values from the search.
		/// </summary>
		private FWBS.Common.KeyValueCollection _returnValues = null;
		/// <summary>
		/// Refreshes the search list types or not.
		/// </summary>
		private bool _refresh = true;
		/// <summary>
		/// Holds the hit test information for the current row chosen.
		/// </summary>
		private DataGridView.HitTestInfo _currentItem = null;
		/// <summary>
		/// Tooltip control for items within the control.
		/// </summary>
		private System.Windows.Forms.ToolTip toolTip1;
		/// <summary>
		/// Parameter array of parameters to bereplaces by the %n% logic.
		/// </summary>
		private FWBS.Common.KeyValueCollection _parameters = null;
		/// <summary>
		/// Current parent object for named parameters.
		/// </summary>
		private object _parent = null;
		/// <summary>
		/// The double click action on the result set of the list.
		/// </summary>
		private string _dblClickAction = "None";
		/// <summary>
		/// An item that may be used to open another OMS item in its separate window.
		/// </summary>
		private ucOMSItemBase _itm = null;
		/// <summary>
		/// 
		/// </summary>
		private System.Windows.Forms.Panel pnlCbSelector;
		private System.Windows.Forms.ComboBox cbSearchType;
		private System.Windows.Forms.Label lblSearchType;
		private System.Windows.Forms.Timer timWait;
		/// <summary>
		/// A boolean that flags that the search has already happened.
		/// </summary>
		private bool _alreadySearched = false;

		/// <summary>
		/// Current selected row id.
		/// </summary>
		private int _cur = -1;

		/// <summary>
		/// A variable which keeps a flag to decide whether to capture or raise an error.
		/// </summary>
		private bool _captureException = true;
        private List<DataGridViewColumn> _columns = new List<DataGridViewColumn>();
        private FWBS.OMS.UI.Windows.EnquiryForm intenqSearch;
		private System.Windows.Forms.Panel pnlSearchTop;
		private FWBS.Common.UI.Windows.eTextBox2 txtSearch;
		private System.Windows.Forms.ContextMenu mnuInfo;
		private System.Windows.Forms.MenuItem mnuSearchName;
        private bool _toberefresh = false;

        private string _imageColumn = "";
		private omsImageLists _omsimagelists = omsImageLists.None;
		private int _imageindex =-1;
		private Button btnDefault;
		private ucErrorBox ucErrorBox1;
        private HelpProvider helpProvider1;
        private Label lblCaption;
        /// <summary>
        /// Additional filter.
        /// </summary>
        private string _externalfilter = "";

        /// <summary>
        /// The ID of the last opened search
        /// </summary>
        private long _lastOpenedSearch = -1;

        private const string _resetAllButtonName = "cmdResetAll";
        private UserControls.ContextMenu.ToolBarActionsConverter _converter;
        private Button filterButton;
        private UserControls.Common.ucSearchPagination searchPagination;
        protected eToolbars tbcLists;
        private Panel pnlFilter;
        private Font _buttonPlainFont = new System.Drawing.Font("Segoe UI", 10F, FontStyle.Regular);

        private DataGridViewLabelColumn _columnSetting;
		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// The constructor to be run to initialise a search list code user control in design mode.
		/// </summary>
		public ucSearchControl()
		{
			Debug.WriteLine("ucSearchControl Constructor Start");
            InitializeComponent();
            this.tbcLists.ImageList = ImageListSelector.GetImageList();
            SetIcons();
            AllowReorderColumns = true;
            Debug.WriteLine("ucSearchControl Constructor End");
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            if (factor.Width != 1 && _enqsearch != null)
            {
                _enqsearch.AutoScrollPosition = Point.Empty;
            }
            base.ScaleControl(factor,specified);
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            SetIcons();
            base.OnDpiChangedAfterParent(e);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _parentform = null;

                    if (_columns != null)
                    {
                        foreach (DataGridViewColumn column in _columns)
                        {
                            column.Dispose();
                        }
                        _columns.Clear();
                    }

                    if (_columnSettingsPopUp != null)
                    {
                        if (_columnSettingContainer != null)
                        {
                            _columnSettingContainer.Closing -= _columnSettingsPopUp.Update;
                            _columnSettingContainer.Dispose();
                            _columnSettingContainer = null;
                        }
                        _columnSettingsPopUp.ResetColumns -= ColumnSettingsPopUpResetColumns;
                        _columnSettingsPopUp.Dispose();
                        _columnSettingsPopUp = null;
                    }

                    if (tbcLists != null)
                    {
                        tbcLists.OMSButtonClick -= this.tbcLists_ButtonClick;
                        tbcLists.Dispose();
                    }

                    if (_buttonPlainFont != null)
                    {
                        _buttonPlainFont.Dispose();
                        _buttonPlainFont = null;
                    }

                    if (_enquirydisconnect != null)
                    {
                        _enquirydisconnect.Refreshed -= Enquiry_Refreshed;
                        _enquirydisconnect = null;
                    }

                    if (_searchList != null)
                    {
                        _searchList.Error -= searchList_Error;
                        _searchList.Searching -= searchList_Searching;
                        _searchList.Searched -= searchList_Searched;
                        _searchList.Dispose();
                    }

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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.ucErrorBox1 = new FWBS.OMS.UI.Windows.ucErrorBox();
            this.pnlResultsBack = new FWBS.Common.UI.Windows.eXPPanel();
            this.dgSearchResults = new FWBS.OMS.UI.Windows.DataGridViewEx();
            this.lblCaption = new System.Windows.Forms.Label();
            this.pnlSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new FWBS.Common.UI.Windows.eTextBox2();
            this.pnlButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.mnuInfo = new System.Windows.Forms.ContextMenu();
            this.mnuSearchName = new System.Windows.Forms.MenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblSearchType = new System.Windows.Forms.Label();
            this.pnlCbSelector = new System.Windows.Forms.Panel();
            this.btnDefault = new System.Windows.Forms.Button();
            this.cbSearchType = new System.Windows.Forms.ComboBox();
            this.timWait = new System.Windows.Forms.Timer(this.components);
            this.pnlSearchTop = new System.Windows.Forms.Panel();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.filterButton = new System.Windows.Forms.Button();
            this.pnlFilter = new System.Windows.Forms.Panel();
            this.searchPagination = new FWBS.OMS.UI.UserControls.Common.ucSearchPagination();
            this.tbcLists = new FWBS.OMS.UI.Windows.eToolbars();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlMain.SuspendLayout();
            this.pnlResultsBack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgSearchResults)).BeginInit();
            this.pnlSearch.SuspendLayout();
            this.pnlCbSelector.SuspendLayout();
            this.pnlFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.ucErrorBox1);
            this.pnlMain.Controls.Add(this.pnlResultsBack);
            this.pnlMain.Controls.Add(this.pnlSearch);
            this.pnlMain.Controls.Add(this.pnlButtons);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlMain.Location = new System.Drawing.Point(5, 94);
            this.pnlMain.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.pnlMain.Size = new System.Drawing.Size(540, 379);
            this.pnlMain.TabIndex = 7;
            // 
            // ucErrorBox1
            // 
            this.ucErrorBox1.BackColor = System.Drawing.Color.White;
            this.ucErrorBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ucErrorBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucErrorBox1.Location = new System.Drawing.Point(0, 40);
            this.ucErrorBox1.Name = "ucErrorBox1";
            this.ucErrorBox1.Size = new System.Drawing.Size(536, 112);
            this.ucErrorBox1.TabIndex = 205;
            this.ucErrorBox1.Visible = false;
            // 
            // pnlResultsBack
            // 
            this.pnlResultsBack.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlResultsBack.Controls.Add(this.dgSearchResults);
            this.pnlResultsBack.Controls.Add(this.lblCaption);
            this.pnlResultsBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlResultsBack.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlResultsBack.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlResultsBack.Location = new System.Drawing.Point(0, 40);
            this.pnlResultsBack.Name = "pnlResultsBack";
            this.pnlResultsBack.Padding = new System.Windows.Forms.Padding(1);
            this.pnlResultsBack.Size = new System.Drawing.Size(536, 339);
            this.pnlResultsBack.TabIndex = 18;
            // 
            // dgSearchResults
            // 
            this.dgSearchResults.AllowUserToAddRows = false;
            this.dgSearchResults.AllowUserToDeleteRows = false;
            this.dgSearchResults.AllowUserToResizeRows = false;
            this.dgSearchResults.AutoGenerateColumns = false;
            this.dgSearchResults.BackgroundColor = System.Drawing.Color.White;
            this.dgSearchResults.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgSearchResults.CaptionLabel = this.lblCaption;
            this.dgSearchResults.CaptionVisible = true;
            this.dgSearchResults.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgSearchResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgSearchResults.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgSearchResults.ColumnHeadersHeight = 36;
            this.dgSearchResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3, 5, 5, 3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgSearchResults.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgSearchResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgSearchResults.EnableHeadersVisualStyles = false;
            this.dgSearchResults.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgSearchResults.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.dgSearchResults.Location = new System.Drawing.Point(1, 24);
            this.dgSearchResults.Name = "dgSearchResults";
            this.dgSearchResults.ReadOnly = true;
            this.dgSearchResults.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgSearchResults.RowHeadersVisible = false;
            this.dgSearchResults.RowHeadersWidth = 32;
            this.dgSearchResults.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgSearchResults.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgSearchResults.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.dgSearchResults.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgSearchResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgSearchResults.Size = new System.Drawing.Size(534, 314);
            this.dgSearchResults.TabIndex = 2;
            this.dgSearchResults.SortChanged += new System.EventHandler<FWBS.OMS.UI.Windows.DataGridViewEx.SortDataEventArgs>(this.dgSearchResults_OnSortChanged);
            this.dgSearchResults.Sorted += new System.EventHandler(this.dgSearchResults_Sorted);
            this.dgSearchResults.Enter += new System.EventHandler(this.dgSearchResults_Enter);
            this.dgSearchResults.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgSearchResults_KeyDown);
            this.dgSearchResults.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgSearchResults_KeyUp);
            this.dgSearchResults.Leave += new System.EventHandler(this.dgSearchResults_Leave);
            this.dgSearchResults.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgSearchResults_MouseDoubleClick);
            this.dgSearchResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgSearchResults_MouseDown);
            this.dgSearchResults.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgSearchResults_MouseUp);
            // 
            // lblCaption
            // 
            this.lblCaption.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCaption.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCaption.Location = new System.Drawing.Point(1, 1);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.lblCaption.Size = new System.Drawing.Size(534, 23);
            this.lblCaption.TabIndex = 3;
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSearch
            // 
            this.pnlSearch.Controls.Add(this.txtSearch);
            this.pnlSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearch.Location = new System.Drawing.Point(0, 5);
            this.pnlSearch.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Padding = new System.Windows.Forms.Padding(0, 6, 3, 6);
            this.pnlSearch.Size = new System.Drawing.Size(536, 35);
            this.pnlSearch.TabIndex = 200;
            // 
            // txtSearch
            // 
            this.txtSearch.CaptionWidth = 90;
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(0, 6);
            this.resourceLookup1.SetLookup(this.txtSearch, new FWBS.OMS.UI.Windows.ResourceLookupItem("txtSearch", "&Quick Filter : ", ""));
            this.txtSearch.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.txtSearch.MaxLength = 32767;
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(533, 23);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.Text = "&Quick Filter : ";
            // 
            // pnlButtons
            // 
            this.pnlButtons.AutoSize = true;
            this.pnlButtons.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(252)))));
            this.pnlButtons.ContextMenu = this.mnuInfo;
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(536, 5);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(0, 0, 4, 4);
            this.pnlButtons.Size = new System.Drawing.Size(4, 374);
            this.pnlButtons.TabIndex = 17;
            this.pnlButtons.WrapContents = false;
            // 
            // mnuInfo
            // 
            this.mnuInfo.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuSearchName});
            this.mnuInfo.Popup += new System.EventHandler(this.mnuInfo_Popup);
            // 
            // mnuSearchName
            // 
            this.mnuSearchName.Index = 0;
            this.mnuSearchName.Text = "[Name]";
            this.mnuSearchName.Click += new System.EventHandler(this.mnuSearchName_Click);
            // 
            // lblSearchType
            // 
            this.lblSearchType.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSearchType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSearchType.Location = new System.Drawing.Point(0, 8);
            this.resourceLookup1.SetLookup(this.lblSearchType, new FWBS.OMS.UI.Windows.ResourceLookupItem("SearchListTypes", "Search Types : ", ""));
            this.lblSearchType.Name = "lblSearchType";
            this.lblSearchType.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblSearchType.Size = new System.Drawing.Size(100, 28);
            this.lblSearchType.TabIndex = 13;
            this.lblSearchType.Text = "Search Types : ";
            // 
            // pnlCbSelector
            // 
            this.pnlCbSelector.Controls.Add(this.btnDefault);
            this.pnlCbSelector.Controls.Add(this.cbSearchType);
            this.pnlCbSelector.Controls.Add(this.lblSearchType);
            this.pnlCbSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCbSelector.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.pnlCbSelector.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.pnlCbSelector.Location = new System.Drawing.Point(5, 31);
            this.pnlCbSelector.Name = "pnlCbSelector";
            this.pnlCbSelector.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.pnlCbSelector.Size = new System.Drawing.Size(540, 44);
            this.pnlCbSelector.TabIndex = 1;
            // 
            // btnDefault
            // 
            this.btnDefault.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnDefault.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnDefault.Location = new System.Drawing.Point(340, 8);
            this.resourceLookup1.SetLookup(this.btnDefault, new FWBS.OMS.UI.Windows.ResourceLookupItem("SDEFAULT", "Default", ""));
            this.btnDefault.Name = "btnDefault";
            this.btnDefault.Size = new System.Drawing.Size(59, 28);
            this.btnDefault.TabIndex = 14;
            this.btnDefault.Text = "Default";
            this.btnDefault.UseVisualStyleBackColor = false;
            this.btnDefault.Visible = false;
            this.btnDefault.Click += new System.EventHandler(this.btnDefault_Click);
            // 
            // cbSearchType
            // 
            this.cbSearchType.BackColor = System.Drawing.SystemColors.Window;
            this.cbSearchType.Dock = System.Windows.Forms.DockStyle.Left;
            this.cbSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSearchType.ItemHeight = 19;
            this.cbSearchType.Location = new System.Drawing.Point(100, 8);
            this.cbSearchType.Name = "cbSearchType";
            this.cbSearchType.Size = new System.Drawing.Size(240, 27);
            this.cbSearchType.TabIndex = 1;
            this.cbSearchType.SelectionChangeCommitted += new System.EventHandler(this.cbSearchType_SelectionChangeCommitted);
            // 
            // timWait
            // 
            this.timWait.Interval = 1000;
            this.timWait.Tick += new System.EventHandler(this.timWait_Tick);
            // 
            // pnlSearchTop
            // 
            this.pnlSearchTop.AutoScroll = true;
            this.pnlSearchTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(246)))), ((int)(((byte)(252)))));
            this.pnlSearchTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSearchTop.Location = new System.Drawing.Point(5, 75);
            this.pnlSearchTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSearchTop.Name = "pnlSearchTop";
            this.pnlSearchTop.Size = new System.Drawing.Size(540, 19);
            this.pnlSearchTop.TabIndex = 0;
            this.pnlSearchTop.TabStop = true;
            this.pnlSearchTop.Visible = false;
            this.pnlSearchTop.SizeChanged += new System.EventHandler(this.pnlSearchTop_SizeChanged);
            this.pnlSearchTop.VisibleChanged += new System.EventHandler(this.pnlSearchTop_VisibleChanged);
            // 
            // filterButton
            // 
            this.filterButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(207)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.filterButton.FlatAppearance.BorderSize = 0;
            this.filterButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.filterButton.Location = new System.Drawing.Point(0, 0);
            this.filterButton.Margin = new System.Windows.Forms.Padding(0);
            this.filterButton.Name = "filterButton";
            this.filterButton.Size = new System.Drawing.Size(26, 26);
            this.filterButton.TabIndex = 206;
            this.filterButton.UseVisualStyleBackColor = false;
            this.filterButton.Click += new System.EventHandler(this.filterButton_Click);
            // 
            // pnlFilter
            // 
            this.pnlFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFilter.Controls.Add(this.filterButton);
            this.pnlFilter.Location = new System.Drawing.Point(507, 3);
            this.pnlFilter.Margin = new System.Windows.Forms.Padding(0);
            this.pnlFilter.Name = "pnlFilter";
            this.pnlFilter.Size = new System.Drawing.Size(40, 26);
            this.pnlFilter.TabIndex = 207;
            this.pnlFilter.Visible = false;
            // 
            // searchPagination
            // 
            this.searchPagination.BackColor = System.Drawing.Color.White;
            this.searchPagination.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.searchPagination.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.searchPagination.Location = new System.Drawing.Point(5, 429);
            this.searchPagination.Name = "searchPagination";
            this.searchPagination.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.searchPagination.Size = new System.Drawing.Size(540, 44);
            this.searchPagination.TabIndex = 4;
            this.searchPagination.Visible = false;
            this.searchPagination.onPageSettingsChanged += new System.EventHandler(this.onPageSettingsChanged);
            this.searchPagination.Paint += new System.Windows.Forms.PaintEventHandler(this.PaginationLine_Paint);
            // 
            // tbcLists
            // 
            this.tbcLists.BottomDivider = false;
            this.tbcLists.ButtonsXML = "";
            this.tbcLists.ContextMenu = this.mnuInfo;
            this.tbcLists.Divider = false;
            this.tbcLists.DropDownArrows = true;
            this.tbcLists.Font = new System.Drawing.Font("Segoe UI", 10.5F);
            this.tbcLists.Location = new System.Drawing.Point(5, 5);
            this.tbcLists.Name = "tbcLists";
            this.tbcLists.NavCommandPanel = null;
            this.tbcLists.ShowToolTips = true;
            this.tbcLists.Size = new System.Drawing.Size(540, 26);
            this.tbcLists.TabIndex = 196;
            this.tbcLists.TopDivider = false;
            this.tbcLists.OMSButtonClick += new FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventHandler(this.tbcLists_ButtonClick);
            // 
            // ucSearchControl
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlFilter);
            this.Controls.Add(this.searchPagination);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlSearchTop);
            this.Controls.Add(this.pnlCbSelector);
            this.Controls.Add(this.tbcLists);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ucSearchControl";
            this.Padding = new System.Windows.Forms.Padding(5, 5, 0, 5);
            this.Size = new System.Drawing.Size(545, 478);
            this.Load += new System.EventHandler(this.ucSearchControl_Load);
            this.ParentChanged += new System.EventHandler(this.ucSearchControl_ParentChanged);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlResultsBack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgSearchResults)).EndInit();
            this.pnlSearch.ResumeLayout(false);
            this.pnlCbSelector.ResumeLayout(false);
            this.pnlFilter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

		#endregion

		#endregion

		#region Methods
		/// <summary>
		/// Change Search List within the Search List Group
		/// </summary>
		/// <param name="Code">Search List Code</param>
		public void SwitchSearchList(string Code)
		{
			cbSearchType.SelectedValue = Code;
			cbSearchType_SelectionChangeCommitted(this,EventArgs.Empty);
		}

		/// <summary>
		/// Gets the OMSToolBar Button by its Name
		/// </summary>
		/// <param name="name">The Name of the Button to Find</param>
		/// <returns>Returns a OMSToolBarButton or Null if not found</returns>
		public OMSToolBarButton GetOMSToolBarButton(string name)
		{
			return tbcLists.GetButton(name);
		}

		/// <summary>
		/// This will cancel the Search Thread
		/// </summary>
		public void HaltSearch()
		{
			if (timWait.Enabled)
			{
				timWait.Enabled = false;
			}
		}
		
		/// <summary>
		/// Selects the Row by a KeyValueCollection
		/// </summary>
		/// <param name="Keys">The Keys and Values to Search By</param>
		public void SelectRowByKey(FWBS.Common.KeyValueCollection Keys)
		{
			if (this.QuickFilterContol.Text != "")
				this.QuickFilterContol.Text = "";

			for (int y = dtCT.DefaultView.Count-1; y >= 0; y--)
			{
				DataRowView rw = dtCT.DefaultView[y];
				for (int i = 0; i < Keys.Count; i++)
					if (Convert.ToString(rw[Keys[i].Key]) == Convert.ToString(Keys[i].Value))
					{
                        dgSearchResults.CurrentRowIndex = y;
                        break;
					}
			}
            ApplyButtonEnabledRules();
            OnItemHover();
            OnItemHovered();
		}

        public void SelectRowByIndex(int index)
        {
            if (index < 0 || index > SearchList.ResultCount - 1)
                return;
            dgSearchResults.CurrentRowIndex = index;
            CurrentItem();
            ApplyButtonEnabledRules();
            OnItemHover();
            OnItemHovered();
        }

		/// <summary>
		/// Shows the Panel Buttons
		/// </summary>
		public void ShowPanelButtons()
		{
			tbcLists.ShowPanelButtons();
		}

		/// <summary>
		/// Removes the Panel Buttons
		/// </summary>
		public void HidePanelButtons()
		{
			tbcLists.RemovePanelButtons();
		}

		/// <summary>
		/// Populates the search type combo box.
		/// </summary>
		/// <param name="type">Search list types.  For instance client related searches will only be displayed in the combo box.</param>
		/// <param name="parent">The parent object to associate to the search for parameter / field replacement.</param>
		public void SetSearchListType(string type, object parent, FWBS.Common.KeyValueCollection parameters)
		{
			SetSearchListType(type, parent, parameters, true);
		}

		/// <summary>
		/// Sets / changes the replacement parameters.
		/// </summary>
		/// <param name="parameters">The parameter values to use.</param>
		public void SetParameters(FWBS.Common.KeyValueCollection parameters)
		{
			_parameters= parameters;
			if (_searchList != null)
				_searchList.ChangeParameters(_parameters);
		}

		/// <summary>
		/// Populates the search type combo box.
		/// </summary>
		/// <param name="type">Search list types.  For instance client related searches will only be displayed in the combo box.</param>
		/// <param name="parente">The parent object to associate to the search for parameter / field replacement.</param>
		/// <param name="force">Forces the types to be refreshed.</param>
		internal void SetSearchListType(string type, object parent, FWBS.Common.KeyValueCollection parameters,  bool force)
		{
			//Set the parameter array.
			_lastsearchtype = -1;
			_parent = parent;
			_parameters = parameters;

			if (force || cbSearchType.Items.Count == 0)
			{
				cbSearchType.DisplayMember = "schdesc";
				cbSearchType.ValueMember = "schcode";
				DataView dv = FWBS.OMS.SearchEngine.SearchList.GetSearchLists(type).DefaultView;
				dv.Sort = "schdesc";
				cbSearchType.DataSource = dv;
				if (dv.Count == 0)
					throw new OMSException2("ERRSGRPNOTFND","Search Group '%1%' not found...",new Exception(),true,type);

				try
				{
					if (SearchList != null)
						this.TypeSelectorVisible = FWBS.Common.ConvertDef.ToBoolean(SearchList.ListViewHeader.Rows[0]["lvtypevisible"],true);
					else
						this.TypeSelectorVisible = false;
				} 
				catch
				{
					this.TypeSelectorVisible = false;
				}

				try
				{
					if (SearchList != null)
					{
						Favourites fav = new Favourites("SL-" + SearchList.SearchListType);
						if (fav.Count > 0)
							cbSearchType.SelectedValue = fav.Description(0);
					}
				}
				catch { }

				cbSearchType_SelectionChangeCommitted(cbSearchType, EventArgs.Empty);
			}
			if (_cmdselect != null && _searchList.ResultCount == 0)
				_cmdselect.Enabled=false;
			
		}

		/// <summary>
		/// Sets the current search list object.
		/// </summary>
		/// <param name="parent">The parent object to associate to the search for parameter / field replacement.</param>
		/// <param name="parameters">A list of parameters to use %n% parameters to be replaced.</param>
		public void SetSearchList(string code, object parent, FWBS.Common.KeyValueCollection parameters)
		{
			try
			{
				SuspendLayout();
				SetSearchList(null, code, true, parent, parameters);
			}
			finally
			{
				ResumeLayout();
			}
		}

		
		/// <summary>
		/// Sets the current search list object.
		/// </summary>
		/// <param name="code">Search list to be used.</param>
		/// <param name="SetTypeSelectorVisible">Type selctor visiblity option.</param>
		/// <param name="parent">The parent object to associate to the search for parameter / field replacement.</param>
		/// <param name="parameters">A list of parameters to use %n% parameters to be replaced.</param>
		public void SetSearchList(string code, bool SetTypeSelectorVisible, object parent, FWBS.Common.KeyValueCollection parameters)
		{
			try
			{
				SuspendLayout();
				SetSearchList(null, code, SetTypeSelectorVisible, parent, parameters);
			}
			finally
			{
				ResumeLayout();
			}
		}


		/// <summary>
		/// Sets the current search list object.
		/// 		/// </summary>
		/// <param name="SearchList">Search list object to be used.</param>
		/// <param name="SetTypeSelectorVisible">Type selctor visiblity option.</param>
		/// <param name="parameters">A list of parameters to use %n% parameters to be replaced.</param>
		public void SetSearchList(SearchList SearchList, bool SetTypeSelectorVisible, FWBS.Common.KeyValueCollection parameters)
		{
			try
			{
				SuspendLayout();
				SetSearchList(SearchList, String.Empty, SetTypeSelectorVisible, null, parameters);
			}
			finally
			{
				ResumeLayout();
			}
		}

		/// <summary>
		/// Sets the current search list object.
		/// </summary>
		/// <param name="SearchList">Search list object to be used.</param>
		/// <param name="code">Search list to be used.</param>
		/// <param name="SetTypeSelectorVisible">Type selctor visiblity option.</param>
		/// <param name="parent">The parent object to associate to the search for parameter / field replacement.</param>
		/// <param name="parameters">A list of parameters to use %n% parameters to be replaced.</param>
		private void SetSearchList(SearchList SearchList, string code, bool SetTypeSelectorVisible, object parent, FWBS.Common.KeyValueCollection  parameters)
		{
			if (_itm != null)
			{
				OMSItem_Close(_itm, new NewOMSTypeCloseEventArgs(ClosingWhy.Cancel));
			}

			//Set the parameter array.
			_parent = parent;
			_parameters = parameters;
			_multireports.Clear();

			//Check to see if the current search list has a script to dispose before you change it.
			if (_searchList != null)
			{
				if (_searchList.HasScript)
				{
					_searchList.Script.Dispose();
				}
			}

			if (SearchList != null)

				_searchList = SearchList;
			else
			{
				//Create the search list object and fetch the search enquiry form.
				if (_searchList != null)
				{
					_searchList.Dispose();
				}
				_searchList = new SearchList(code, parent, parameters);
				OnSearchTypeChanged();
			}

            if (!string.IsNullOrEmpty(_externalfilter))
            {
                _searchList.ExternalFilter = _externalfilter;
            }

            // Set the Events for the Thread Search
            _searchList.Error +=new MessageEventHandler (searchList_Error);
			_searchList.Searching +=new CancelEventHandler(searchList_Searching);
			_searchList.Searched +=new SearchedEventHandler(searchList_Searched);

			//Set the dobule click action based on the underlying search object.
			DoubleClickAction = _searchList.DoubleClickAction;
            dgSearchResults.Columns.Clear();
            _columns.Clear();
            tbcLists.RemovePanelButtons();
			tbcLists.ButtonsXML = "";

			_multiSelect = (_searchList.MultiSelect ? Common.TriState.True : Common.TriState.False);
            dgSearchResults.MultiSelect = _multiSelect == Common.TriState.True ? true : false;
            lblCaption.Visible = _searchList.CaptionVisible;
            searchPagination.Visible = _searchList.PaginationVisible;
            lblCaption.Visible = !_searchList.PaginationVisible;
            IsActionColumnEnabled = _searchList.ActionButtonVisible;
            SetCaption();
            _imageindex = _searchList.ColumnImageIndex;
			_imageColumn = _searchList.ColumnImageColumn;
			_omsimagelists = (omsImageLists)FWBS.Common.ConvertDef.ToEnum(_searchList.ColumnImageResource,omsImageLists.None);

			_quickFilterVisible = _searchList.QuickSearch;
			pnlSearch.Visible = _searchList.QuickSearch;

			_incsearch.Clear();

			//Loop through the rows within the list view table
			//and create the columns to be displayed.
			DataTable dt = AllowReorderColumns && _searchList.IsUserCutomized 
                ? _searchList.UserCustomizaitons
                : _searchList.ListView;

			bool _first = false;
			User _user = Session.CurrentSession.CurrentUser;
            var _sortMode = _searchList.IsOrderingSupported 
                ? DataGridViewColumnSortMode.Programmatic 
                : DataGridViewColumnSortMode.Automatic;
            SetRowHeight();

			bool warned = false;
			foreach (DataRow row in dt.Rows)
			{
				if ((dt.Columns.Contains("roles") == false || _user.IsInRoles(Convert.ToString(row["roles"])) && (dt.Columns.Contains("conditions") == false || Session.CurrentSession.ValidateConditional(_parent,Convert.ToString(row["conditions"]).Split(Environment.NewLine.ToCharArray())))))
				{
                    bool sortable = true;
                    DataGridViewLabelColumn col = new DataGridViewLabelColumn();
                    if (_first == false)
					{
                        col.Resources = _omsimagelists;
						if (_imageColumn == "")
							col.ImageIndex = _imageindex;
						else
							col.ImageColumn = _imageColumn;
						_first = true;
					}
                    if (row.Table.Columns.Contains("displayAs") == false || row.Table.Columns.Contains("sourceIs") == false)
						if (warned == false)
						{
							FWBS.OMS.UI.Windows.MessageBox.ShowInformation("WARSTPOD1", "There is a System Stored Procedure out of date that needs updating to maintain full compatibility, please contact your system administrator to report this code “%1%”, the system will try and continue operation as normal but functionality maybe reduced.", "sprSearchListBuilder");
							warned = true;
						}
					if (row.Table.Columns.Contains("displayAs") && row["displayAs"] != DBNull.Value)
						col.DisplayDateAs = (SearchColumnsDateIs)FWBS.Common.ConvDef.ToEnum(row["displayAs"], SearchColumnsDateIs.Local);
					if (row.Table.Columns.Contains("sourceIs") && row["sourceIs"] != DBNull.Value)
						col.SourceDateIs = (SearchColumnsDateIs)FWBS.Common.ConvDef.ToEnum(row["sourceIs"], SearchColumnsDateIs.UTC);
                    dgSearchResults.MultiSelect = (_multiSelect == Common.TriState.True ? true : false);
                    col.ReadOnly = true;

                    if (row.Table.Columns.Contains("visible") && row["visible"] != DBNull.Value)
                    {
                        col.Visible = ConvertDef.ToBoolean(row["visible"], true);
                    }

                    if (row.Table.Columns.Contains("lvsortable"))
                    {
                        sortable = ConvDef.ToBoolean(row["lvsortable"], true);
                    }
                    col.SortMode = sortable ? _sortMode : DataGridViewColumnSortMode.NotSortable;

                    if (row.Table.Columns.Contains("orderIndex") && row["orderIndex"] != DBNull.Value)
                    {
                        col.DisplayIndex = ConvertDef.ToInt32(row["orderIndex"], 0);
                    }

                    if (row["lvwidth"] != DBNull.Value)
                    {
                        int width = (int) row["lvwidth"];
                        if (width == 0)
                        {
                            col.MinimumWidth = DataGridViewEx.MinimumColumnWidth;
                            col.Visible = false;
                        }
                        col.Width = LogicalToDeviceUnits(width);
                    }
                    col.HeaderText = Session.CurrentSession.Terminology.Parse(row["lvdesc"].ToString(),true);
					col.DataPropertyName = row["lvmapping"].ToString();
					col.DefaultCellStyle.NullValue = Convert.ToString(row["lvnulltext"]);
					if (Convert.ToString(row["lvdatalistname"]) != "")
					{
						col.SearchList = this.SearchList;
						col.DataListName = Convert.ToString(row["lvdatalistname"]);
					}
					else if (Convert.ToString(row["lvdatacodetype"]) != "")
					{
						col.DataCodeType = Convert.ToString(row["lvdatacodetype"]);
					}
					try
					{
						if (Convert.ToString(row["lvformat"]) == "Number")
						{
							col.DefaultCellStyle.Format = "F";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            col.FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                            col.HeaderText += ".";
						}
						else if (Convert.ToString(row["lvformat"]) == "Currency")
						{
							col.DefaultCellStyle.Format = "c";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            col.FormatInfo = GetCultureFrom(parent);
                            col.HeaderText += ".";
						}
						else if (Convert.ToString(row["lvformat"]) == "DateTimeRgt")
						{
                            col.DefaultCellStyle.Format = "g";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            col.FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                            col.HeaderText += ".";
						}
						else if (Convert.ToString(row["lvformat"]) == "DateLongLft")
						{
							col.DefaultCellStyle.Format = "D";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            col.FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                        }						
						else if (Convert.ToString(row["lvformat"]) == "DateTimeLft")
						{
							col.DefaultCellStyle.Format = "g";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            col.FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                        }
						else if (Convert.ToString(row["lvformat"]) == "DateLft")
						{
							col.DefaultCellStyle.Format = "d";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            col.FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                        }
						else if (Convert.ToString(row["lvformat"]) == "TimeLft")
						{
							col.DefaultCellStyle.Format = "t";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            col.FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                        }					
						else if (Convert.ToString(row["lvformat"]) == "Date" || Convert.ToString(row["lvformat"]) == "DateRgt")
						{
							col.DefaultCellStyle.Format = "d";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            col.FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                            col.HeaderText += ".";
						}
						else if (Convert.ToString(row["lvformat"]) == "Time" || Convert.ToString(row["lvformat"]) == "TimeRgt")
						{
							col.DefaultCellStyle.Format = "t";
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            col.FormatInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
                            col.HeaderText += ".";
						}
						else if (Convert.ToString(row["lvformat"]) == "RightAlign")
						{
                            col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            col.HeaderText += ".";
						}
					}
					catch (Exception ex)
					{
						System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Search List Column Format Error: " + ex.Message);
					}
                    try
					{
						bool use = FWBS.Common.ConvertDef.ToBoolean(row["lvincquicksearch"],true);
						_incsearch.Add(col, use);
					}
					catch
					{
						_incsearch.Add(col, true);
					}
                    _columns.Add(col);
                }
			}

            //Loop through the rows within the list view buttons table
            //and set their attributes.
            dt = _searchList.ButtonsTable;
            if (_ecmdadd == false) _cmdadd = null;
			if (_ecmdedit == false) _cmdedit = null;
			if (_ecmdsearch == false) _cmdsearch = null;
			if (_ecmdselect == false) _cmdselect = null;
			if (_ecmdtrash == false) _cmdtrash = null;
			if (_ecmdactive == false) _cmdactive = null;
            _tbactive  = null;
			_tbtrash   = null;
			_tbrestore = null;
			_tbdelete  = null;
            _cmdresetall = null;
            Global.RemoveAndDisposeControls(pnlButtons);
            tbcLists.Clear();
            _disabledonnowrows.Clear();
			_disabledomultiselect.Clear();
			pnlButtons.Visible=false;
			tbcLists.Visible = false;
			pnlbutCount = 0;
            foreach (DataRow row in dt.Rows)
			{
				string parentcode = null;
				if (dt.Columns.Contains("parent"))
					parentcode = Convert.ToString(row["parent"]);

                bool permissionAllow = true;
                if(dt.Columns.Contains("roles"))
                {
                    permissionAllow = _user.IsInRoles(Convert.ToString(row["roles"]));
                }
				if ((permissionAllow && (dt.Columns.Contains("buttonConditions") == false || Session.CurrentSession.ValidateConditional(_parent,Convert.ToString(row["buttonConditions"]).Split(Environment.NewLine.ToCharArray())))))
				{
					string _buttonstyle = "";
					if (dt.Columns.Contains("buttonStyle")) _buttonstyle = Convert.ToString(row["buttonStyle"]);
					SearchButtonArgs ne = new SearchButtonArgs((ButtonActions)FWBS.Common.ConvertDef.ToEnum(row["mode"],ButtonActions.None),Convert.ToString(row["parameter"]));
					if (ne.Action == ButtonActions.Seperator && _buttonstyle == ButtonStyle.Graphical.ToString())
					{
						OMSToolBarButton tb = new OMSToolBarButton(tbcLists);
						tb.Style = ToolBarButtonStyle.Separator;
						tb.ParentCode = parentcode;
						tbcLists.AddButton(tb);
					}
                    else if (ne.Action == ButtonActions.Seperator && _buttonstyle == ButtonStyle.Plain.ToString())
                    {
                        System.Windows.Forms.Panel pl = new System.Windows.Forms.Panel();
                        pl.Height = 8;
                        pl.Dock = DockStyle.Top;
                        pnlButtons.Controls.Add(pl, true);
                        pl.BringToFront();
                    }
                    else
					{
						Button current = null;
						if (ne.Action == ButtonActions.Add && _cmdadd != null && _ecmdadd) current = _cmdadd; else 
							if (ne.Action == ButtonActions.Edit && _cmdedit != null && _ecmdedit ) current = _cmdedit; else
							if (ne.Action == ButtonActions.Select && _cmdselect != null && _ecmdselect) current = _cmdselect; else
							if (ne.Action == ButtonActions.Search && _cmdsearch != null && _ecmdsearch) current = _cmdsearch; else 
							if (ne.Action == ButtonActions.TrashDelete && _cmddelete != null && _ecmddelete) current = _cmddelete; else
							if (ne.Action == ButtonActions.ViewActive && _cmdactive != null && _ecmdactive) current = _cmdactive; else
							if (ne.Action == ButtonActions.ViewTrash && _cmdtrash != null && _ecmdtrash) current = _cmdtrash; else
							if (ne.Action == ButtonActions.Restore && _cmdrestore != null && _ecmdrestore) current = _cmdrestore; else
							current = new Button();
                        #region Graphic Button Contructors
						// If the button style is Graphical the Contruct It
						if (_buttonstyle == ButtonStyle.Graphical.ToString())
						{
							OMSToolBarButton tbcurrent = new OMSToolBarButton(tbcLists);
							tbcurrent.Text = Convert.ToString(row["btndesc"]);
							tbcurrent.ToolTipText = Convert.ToString(row["btnhelp"]);
							tbcurrent.ImageIndex = FWBS.Common.ConvertDef.ToInt32(row["buttonGlyph"],-1);
							tbcurrent.Name = Convert.ToString(row["btnname"]);
							tbcurrent.ParentCode = parentcode;
							tbcurrent.HasContextMenu = Convert.ToBoolean(row["contextMenuVisible"]);
							tbcurrent.PanelButtonCaption = Convert.ToString(row["pnlBtnCaptionDesc"]);
							tbcurrent.PanelButtonVisible = FWBS.Common.ConvertDef.ToBoolean(row["pnlBtnVisible"],false);
							tbcurrent.Visible = FWBS.Common.ConvertDef.ToBoolean(row["btnvisible"],true);
							if (tbcurrent.PanelButtonVisible)
								pnlbutCount++;
							tbcurrent.PanelButtonImageIndex = FWBS.Common.ConvertDef.ToInt32(row["pnlBtnIndex"],-1);

							if (ne.Action == ButtonActions.Select) 
								_tbselect = tbcurrent;
							else if (ne.Action == ButtonActions.Edit) 
								_tbedit = tbcurrent;
							else if (ne.Action == ButtonActions.ViewActive || ne.Action == ButtonActions.ViewTrash)
							{
								tbcurrent.Style = ToolBarButtonStyle.ToggleButton;
								if (ne.Action == ButtonActions.ViewActive)
								{
									_tbactive = tbcurrent;
									_tbactive.Pushed =true;
								}
								if (ne.Action == ButtonActions.ViewTrash) _tbtrash = tbcurrent;
							}
							else if (ne.Action == ButtonActions.Restore || Convert.ToString(row["btnname"]) == "cmdRestore") 
							{
								_tbrestore = tbcurrent;
								_tbrestore.Enabled=false;
							}
							else if (ne.Action == ButtonActions.Delete || ne.Action == ButtonActions.TrashDelete || Convert.ToString(row["btnname"]) == "cmdDelete") 
							{
								_tbdelete = tbcurrent;
							}
							else if (ne.Action == ButtonActions.ReportMulti)
							{
								ContextMenu _popup = new ContextMenu();
								string[] reports = ne.Parameters.Split(',');
								foreach (string rep in reports)
								{
									MenuItem _item = new MenuItem();
									_item.Text = Session.CurrentSession.Terminology.Parse(CodeLookup.GetLookup("OMSSEARCH",rep),true);
									_item.Click +=new EventHandler(Report_item_Click);
									_multireports.Add(_item.Handle.ToInt64(),rep);
									_popup.MenuItems.Add(_item);
								}
								tbcurrent.DropDownMenu = _popup;
							}

							if (Convert.ToBoolean(row["enabledWithNoRows"]) == false)
								_disabledonnowrows.Add(tbcurrent);

							if (row.Table.Columns.Contains("enabledMultiSelect"))
							{
								if (Convert.ToBoolean(row["enabledMultiSelect"]) == false)
									_disabledomultiselect.Add(tbcurrent);
							}

                            //If Save Search Type is Never, disable SavedSearch toolbar buttons.
                            if (ne.Action == ButtonActions.SaveSearch && this.SearchList.SaveSearch == SaveSearchType.Never.ToString())
                                tbcurrent.Visible = false;
                            
                            if (ne.Action == ButtonActions.OpenSearch && this.SearchList.SaveSearch == SaveSearchType.Never.ToString())
                                tbcurrent.Visible = false;

							int i = 1;
							string name = tbcurrent.Name;
							while (tbcLists.Buttons.ContainsKey(tbcurrent.Name))
							{
								tbcurrent.Name = string.Format("{0}{1}", name, i);
								i++;
							}
							tbcLists.AddButton(tbcurrent);

                            tbcLists.Visible = true;
							current.Height=0;
							tbcurrent.Tag = current;
						}
                        #endregion

                        current.Font = _buttonPlainFont;

                        current.Name = Convert.ToString(row["btnname"]);
						current.Text = Convert.ToString(row["btndesc"]);
                        if(current.Height != 0)
                        {
                            current.Size = new Size(75, 30);
                        }
                        current.FlatStyle = FlatStyle.Flat;
                        current.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
                        current.FlatAppearance.BorderSize = 0;
                        current.BackColor = Color.FromArgb(216, 216, 216);
                        current.Margin = new Padding(0);
                        current.TextAlign = ContentAlignment.MiddleCenter;
                        current.Click += new EventHandler(cmdButton_Click);
						if (ne.Action == ButtonActions.Add && _cmdadd == null) _cmdadd = current; else 
							if (ne.Action == ButtonActions.Edit && _cmdedit == null) _cmdedit = current; else 
							if (ne.Action == ButtonActions.Select && _cmdselect == null) _cmdselect = current; else
							if (ne.Action == ButtonActions.Search && _cmdsearch == null) _cmdsearch = current; else
							if (ne.Action == ButtonActions.TrashDelete && _cmddelete == null) _cmddelete = current; else
							if (ne.Action == ButtonActions.ViewActive && _cmdactive == null) _cmdactive = current; else
							if (ne.Action == ButtonActions.ViewTrash && _cmdtrash == null) _cmdtrash = current; else
							if (ne.Action == ButtonActions.Restore && _cmdrestore == null) _cmdrestore = current;
						current.Tag = ne;
						current.Visible = FWBS.Common.ConvertDef.ToBoolean(row["btnvisible"],true);

                        //If Save Search Type is Never, disable SavedSearch toolbar buttons.
                        if (ne.Action == ButtonActions.SaveSearch && this.SearchList.SaveSearch == SaveSearchType.Never.ToString())
                            current.Visible = false;        
                        if (ne.Action == ButtonActions.OpenSearch && this.SearchList.SaveSearch == SaveSearchType.Never.ToString())
                            current.Visible = false;
  
						if (current.Parent == null)
						{
                            pnlButtons.Controls.Add(current,true);
							current.Dock = DockStyle.None;
                        }
						current.BringToFront();
						toolTip1.SetToolTip(current, Convert.ToString(row["btnhelp"]));

						if (Convert.ToBoolean(row["enabledWithNoRows"]) == false)
							_disabledonnowrows.Add(current);
						
						if (row.Table.Columns.Contains("enabledMultiSelect"))
						{
							if (Convert.ToBoolean(row["enabledMultiSelect"]) == false)
								_disabledomultiselect.Add(current);
						}
                        this.EnquiryForm.VerticalScroll.Maximum = 0;
                        this.EnquiryForm.AutoScroll = false;
                        this.EnquiryForm.VerticalScroll.Visible = false;
                        this.EnquiryForm.AutoScroll = true;
					}
				}
			}

            if (IsActionColumnEnabled)
            {
                _converter = new UserControls.ContextMenu.ToolBarActionsConverter(tbcLists);
                var actionColumn = new DataGridViewControls.DataGridViewActionsColumn()
                {
                    Name = "ActionColumn",
                    HeaderCell = new DataGridViewControls.DataGridViewExColumnHeaderCell(),
                    HeaderText = Session.CurrentSession.Resources.GetResource("ACTIONS", "Actions", "").Text
                 };
                actionColumn.GetActions = PopulateActionPopup;
                _columns.Add(actionColumn);
            }

            this.dgSearchResults.AllowUserToOrderColumns = AllowReorderColumns;
            if (AllowReorderColumns)
            {
                var headerImageCell = new DataGridViewColumnHeaderImageCell
                {
                    Image = Images.GetCommonIcon(DeviceDpi, "columnsettings"),
                    Style = new DataGridViewCellStyle
                    {
                        Padding = new Padding(0, 2, 0, 2),
                        Alignment = RightToLeft == RightToLeft.Yes ? DataGridViewContentAlignment.MiddleLeft : DataGridViewContentAlignment.MiddleRight
                    }
                };
                headerImageCell.ImageClick += ImageHeader_ImageClick;

                _columnSetting = new DataGridViewLabelColumn
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    HeaderCell = headerImageCell,
                    MinimumWidth = LogicalToDeviceUnits(32),
                    Resizable = DataGridViewTriState.False,
                    SortMode = DataGridViewColumnSortMode.NotSortable
                };

                _columns.Add(_columnSetting);
            }

			AttachBeforeCellDisplayEvent();
            
            if (tbcLists.NavCommandPanel != null)
            {
                tbcLists.NavCommandPanel.Refresh();
            }
            if (_cmdadd == null) _cmdadd = new Button();
			if (_cmdedit == null ) _cmdedit = new Button();
			if (_cmdselect == null) _cmdselect = new Button();
			if (_cmdsearch == null) _cmdsearch = new Button(); else if (ParentForm != null) ParentForm.AcceptButton = _cmdsearch;
			if (_cmdtrash != null && _cmdtrash.Enabled && _cmdactive != null) 
			{
				if (_cmdactive != null)
					_cmdactive.Enabled=false;
				
				if (_cmdrestore != null)
					_cmdrestore.Enabled=false;
				_trashactivestate = ActiveState.Active;
			}

			//Build the search list combo up.
			_refresh = false;
			if (SetTypeSelectorVisible)
			{
				SetSearchListType(_searchList.SearchListType, _parent,parameters, false);
				cbSearchType.SelectedValue = _searchList.Code;
			}
			else
			{
				cbSearchType.SelectedValue = _searchList.Code;
				cbSearchType.Visible = false;
			}

			try
			{
				this.TypeSelectorVisible = FWBS.Common.ConvertDef.ToBoolean(_searchList.ListViewHeader.Rows[0]["lvtypevisible"],true);
			} 
			catch
			{
				this.TypeSelectorVisible = true;
			}

			_refresh = true;

			//Get the criteria enquiry form if the seach style is a search style.
			if (_searchList.Style == SearchListStyle.Search || _searchList.Style == SearchListStyle.Filter)
			{
				if (_enqsearch == intenqSearch)
				{
					cmdSearch.Visible = true;
					cmdSelect.Visible = true;

					if (_searchList.CriteriaForm == null)
					{
						pnlSearchTop.Visible = false;
                        pnlButtons.Parent = pnlMain;
                        pnlFilter.Visible = false;
					}
					else
					{
						this.EnquiryForm.Enquiry = _searchList.CriteriaForm;
						pnlSearchTop.Visible = true;
                        pnlFilter.Visible = tbcLists.Visible;
                        pnlButtons.Parent = pnlSearchTop;
                        this.EnquiryForm.BringToFront();
                        this.EnquiryForm.Focus();
                    }
                }
				else
				{
					cmdSearch.Visible = true;
					cmdSelect.Visible = true;
					this.EnquiryForm.Enquiry = _searchList.CriteriaForm;
					pnlSearchTop.Visible = false;
                    pnlFilter.Visible = false;
                    pnlButtons.Parent = pnlMain;
                    dgSearchResults.BringToFront();
					this.EnquiryForm.Focus();
				}
			}
			else
			{
				if (this.EnquiryForm != null)
					this.EnquiryForm.Enquiry = null;

                pnlFilter.Visible = false;
                cmdSearch.Visible = false;
				pnlSearchTop.Visible = false;
                pnlButtons.Parent = pnlMain;
                if (RightToLeft == RightToLeft.Yes)
                {
                    pnlButtons.Dock = DockStyle.Left;
                }
                else
				{
                    pnlButtons.Dock = DockStyle.Right;
                }
            }

            AddResetButton();

            if (_tbselect != null) _tbselect.Enabled = false;
			if (_cmdselect != null) _cmdselect.Enabled = false;
			if (_cmdedit != null) _cmdedit.Enabled = false;
			if (_tbedit != null) _tbedit.Enabled = false;
			if (_tbdelete != null) _tbdelete.Enabled = false;
			if (_cmddelete != null) _cmddelete.Enabled = false;
            dgSearchResults.RowsDefaultCellStyle.SelectionBackColor = SystemColors.InactiveCaption;
            pnlMain.Visible=true;
			dgSearchResults.ContextMenu = tbcLists.ContextMenuOutput;
			tbcLists.ShowPanelButtons();
			pnlMain.BringToFront();

			if (_searchList.HasScript)
			{
				Script.SearchListScriptType script = _searchList.Script.Scriptlet as Script.SearchListScriptType;
				if (script != null)
					script.SetSearchListControl(this);
				else if (!_searchList.InDesignMode)
				{
					pnlMain.Visible = false;
					return;
				}
			}

			if (_searchList.ButtonsTable.Columns.Contains("buttonStyle"))
            {
                DataView bfinder = new DataView(_searchList.ButtonsTable, "buttonStyle = 'Plain'",null,DataViewRowState.CurrentRows);
                pnlButtons.Visible = bfinder.Count > 0;

                if (!pnlButtons.Visible && pnlButtons.Controls.ContainsKey(_resetAllButtonName))
                {
                    pnlButtons.Visible = true;
                }
            }
            else
            {
                pnlButtons.Visible = true;
            }

            //Call HelpFilePathSetup helper
            HelpFilePathSetUp();

            if (_columnSettingsPopUp != null)
            {
                _columnSettingsPopUp.ResetColumns -= ColumnSettingsPopUpResetColumns;
				dgSearchResults.ColumnDisplayIndexChanged -= DgSearchResults_ColumnDisplayIndexChanged; 
                if (_columnSettingContainer != null) 
                { 
                    _columnSettingContainer.Closing -= _columnSettingsPopUp.Update;
                }
            }
            _columnSettingsPopUp = new UserControls.ColumnSettings.ColumnSettingsPopUp(_columns, _searchList.Code);
            _columnSettingsPopUp.ResetColumns += ColumnSettingsPopUpResetColumns;
            _columnSettingContainer = new NavigationPopupContainer(_columnSettingsPopUp);
            _columnSettingContainer.Closing += _columnSettingsPopUp.Update;
            dgSearchResults.Columns.AddRange(_columns.ToArray());
            dgSearchResults.ColumnDisplayIndexChanged += DgSearchResults_ColumnDisplayIndexChanged;
            ResetSorting();
            OnSearchListLoad();
            AdjustFilterButtonsLayout();
		}

        /// <summary>
        /// Attach before cell display event to the column if it's enabled in search list script
        /// </summary>
        private void AttachBeforeCellDisplayEvent()
        {
            if (_searchList.HasScript)
            {
                Script.SearchListScriptType script = _searchList.Script.Scriptlet as Script.SearchListScriptType;
                if (script != null && script.EnableBeforeDisplayCellEvent)
                {
                    foreach (var column in _columns)
                    {
                        var col = column as IBeforeCellDisplayable;
                        if (col != null)
                        {
                            col.BeforeCellDisplayEvent += col_BeforeCellDisplayEvent;
                        }
                    }
                }
            }
        }

        private void SetRowHeight()
        {
            dgSearchResults.RowTemplate.Height = dgSearchResults.RowTemplate.MinimumHeight = GetRowHeight();
            dgSearchResults.RowTemplate.DefaultCellStyle.WrapMode = _searchList.RowHeight > 1 
                ? DataGridViewTriState.True 
                : DataGridViewTriState.False;
        }

        private int GetRowHeight()
        {
            return LogicalToDeviceUnits(_searchList.RowHeight * 22 + 10);
        }

        private void HelpFilePathSetUp()
        {
            if (string.IsNullOrEmpty(helpProvider1.HelpNamespace))
            {
                string helpPath = Session.CurrentSession.GetHelpPath(_searchList.Code);
                if (String.IsNullOrEmpty(helpPath))
                {
                    helpProvider1.SetShowHelp(this, false);
                }
                else
                {
                    helpProvider1.HelpNamespace = helpPath;
                    helpProvider1.SetShowHelp(this, true);
                    helpProvider1.SetHelpKeyword(this, _searchList.HelpKeyword);
                    helpProvider1.SetHelpNavigator(this, HelpNavigator.KeywordIndex);
                }
            }
        }

        private void SetIcons()
        {
           this.filterButton.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "filter");
        }

        private void AdjustFilterButtonsLayout()
        {
            if (pnlButtons.Visible)
            {
                var defaultPadding = LogicalToDeviceUnits(8);
                if (EnquiryForm != null && pnlButtons.Parent == pnlSearchTop)
                {
                    var isHorizontal = isHorizontalLayout();
                    var isDocTab = SearchList.Code == "SCHDOCSCHTRANS";
                    pnlButtons.FlowDirection = (isHorizontal || isDocTab)
                        ? FlowDirection.RightToLeft 
                        : FlowDirection.BottomUp;
                    pnlButtons.Dock = isDocTab
                        ? DockStyle.Bottom
                        : DockStyle.Right;
                    var buttonsHeight = GetContentHeight(isHorizontal);
                    var offset = (EnquiryForm.Height - buttonsHeight) / 2;
                    if (isHorizontal)
                    {
                        _cmdresetall.Margin = new Padding(0, 0, defaultPadding, 0);
                        pnlButtons.Padding = new Padding(0, offset, defaultPadding, 0);
                    }
                    else
                    {
                        pnlButtons.Padding = isDocTab
                            ? new Padding(0, 0, 3 * defaultPadding, 2 * defaultPadding)
                            : new Padding(defaultPadding, 0, defaultPadding, offset);
                        ApplyAutoSize(isDocTab);
                    }
                    if (buttonsHeight > pnlSearchTop.Height)
                    {
                        pnlSearchTop.Height = buttonsHeight;
                    }
                }
                else if (pnlButtons.Parent == pnlMain)
                {
                    pnlButtons.Dock = DockStyle.Right;
                    pnlButtons.FlowDirection = FlowDirection.TopDown;
                    pnlButtons.Padding = new Padding(defaultPadding, 0, defaultPadding, 0);
                    GetDisplayedButtonsCount();
                    ApplyAutoSize(false);
                }
            }
        }

        private IFormatProvider GetCultureFrom(object source)
		{
			NumberFormatInfo _currency = System.Globalization.CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture).NumberFormat;
			if (source is OMSFile)
				_currency = ((OMSFile)source).CurrencyFormat;
            else if (source is OMSDocument)
				_currency = ((OMSDocument)source).OMSFile.CurrencyFormat;
			else if (source is FeeEarner)
				_currency = ((FeeEarner)source).CurrencyFormat;
			else if (source is TimeRecord)
				_currency = ((TimeRecord)source).OMSFile.CurrencyFormat;
			return _currency;
		}

		private void col_BeforeCellDisplayEvent(object sender, FWBS.Common.UI.Windows.CellDisplayEventArgs e)
		{
			try
			{
				OnBeforeCellDisplay(e);
            }
			catch (Exception ex)
			{
				Trace.WriteLine(String.Format("ERR: {0}",ex.Message));
			}
		}

		/// <summary>
		/// Performs the search method.
		/// </summary>
		public void Search()
		{
			Search(false,true,true);
		}

        public void Search(int deletedRecordsCount)
        {
            if (searchPagination.Visible)
            {
                int lastPage = System.Math.Max((int)System.Math.Ceiling((searchPagination.TotalItems - deletedRecordsCount) / (double)searchPagination.PageSize), 1);
                if (searchPagination.CurrentPage > lastPage)
                {
                    searchPagination.CurrentPage--;
                }
            }
            Search(false, true, true);
        }

		/// <summary>
		/// Performs the search method.
		/// </summary>
		public void Search(bool keepCursor,bool CaptureError)
		{
			Search(keepCursor,CaptureError,true);
		}

		/// <summary>
		/// Performs the search method.
		/// </summary>
		/// <param name="keepCursor">Keeps the cursor in the same position.</param>
		public void Search(bool keepCursor,bool CaptureError, bool Async)
		{
			try
			{
				if (_searchList == null) return;
				this.EnquiryForm.ReBind();
                _captureException = CaptureError;
				if (keepCursor)
                    _cur = dgSearchResults.CurrentRowIndex;
                else
					_cur = -1;

                if (_searchList.SaveSearch == SaveSearchType.Always.ToString())
                {
                    string _obj = "";
                    long? _objID = 0;
                    SavedSearches.Tools.GetParentObjectTypeAndID(this.SearchList.Code, this.SearchList.Parent, ref _obj, ref _objID);
                    SavedSearches.SaveForcedSearch(FWBS.OMS.SavedSearches.Tools.BuildSearchCriteriaXML(this.EnquiryForm), this.SearchList.Code, "SEARCHLIST", _obj, _objID);
                }

                if (resetPagination)
                {
                    searchPagination.ResetPages(0);
                    resetPagination = false;
                }
                SetPaginationParams();

				_searchList.Search(Async);
			}
			catch (Exception ex)
			{
				StopTimer();
				Application.DoEvents();
				if (CaptureError)
					ErrorBox.Show(ParentForm, ex);
				else
					throw ex;
			}
			
		}

		/// <summary>
		/// Performs the select method.
		/// </summary>
		/// <returns>A boolean flag that indicates whether a valid item has been picked from the list.</returns>
		public bool SelectRowItem()
		{
			try
			{
				if (_searchList == null) return false;
                _returnValues = _searchList.Select(dgSearchResults.CurrentRowIndex);
            }
			catch
			{
				return false;
			}

			if (_returnValues == null || _returnValues.Count == 0)
				return false;
			else
			{
                OnItemSelected();
				return true;
			}
		}

		/// <summary>
		/// Performs the select method.
		/// </summary>
		/// <returns>A boolean flag that indicates whether a valid item has been picked from the list.</returns>
		public FWBS.Common.KeyValueCollection CurrentItem()
		{
			try
			{
				if (_searchList == null) return null;
                _returnValues = _searchList.Select(dgSearchResults.CurrentRowIndex);
                return this.ReturnValues;
			}
			catch (SearchException)
			{
				return null;
			}
		}
		#endregion

		#region Private Methods

		private void DgSearchResults_ColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            var property = typeof(DataGridViewColumn).GetProperty("DisplayIndexHasChanged", BindingFlags.NonPublic | BindingFlags.Instance);
            if (property != null)
            {
                foreach (DataGridViewColumn column in dgSearchResults.Columns)
                {
                    if (Convert.ToBoolean(property.GetValue(column)))
                    {
                        return;
                    }
                }
            }

            int dataGridLastColumnIndex = dgSearchResults.ColumnCount - 1;
            if (_columnSetting.DisplayIndex != dataGridLastColumnIndex)
            {
                BeginInvoke((MethodInvoker)(() => { _columnSetting.DisplayIndex = dataGridLastColumnIndex; }));
                return;
            }

            _columnSettingsPopUp.Update(sender, e);
        }

        private void ColumnSettingsPopUpResetColumns(object sender, EventArgs e)
        {
            dgSearchResults.ColumnDisplayIndexChanged -= DgSearchResults_ColumnDisplayIndexChanged;

            _columnSettingsPopUp.ResetColumnsSettings();

			dgSearchResults.ColumnDisplayIndexChanged += DgSearchResults_ColumnDisplayIndexChanged;
        }

		private void ImageHeader_ImageClick(object sender, EventArgs e)
        {
            _columnSettingsPopUp.BuildContent();
            _columnSettingContainer.Show(Cursor.Position, RightToLeft == RightToLeft.Yes ? ToolStripDropDownDirection.BelowRight : ToolStripDropDownDirection.BelowLeft);
        }

        private void filterButton_Click(object sender, System.EventArgs e)
        {
            pnlSearchTop.Visible = !pnlSearchTop.Visible;
            filterButton.BackColor = pnlSearchTop.Visible
                ? Color.FromArgb(207, 224, 242)
                : Color.Transparent;
        }

        /// <summary>
        /// Add button to reset data in filter
        /// </summary>
        private void AddResetButton()
        {
            if (EnquiryForm != null && pnlButtons.Parent == pnlSearchTop)
            {
                _cmdresetall = new LinkLabel
                {
                    ActiveLinkColor = Color.FromArgb(108, 169, 216),
                    DisabledLinkColor = Color.FromArgb(165, 195, 230),
                    Dock = DockStyle.Fill,
                    Font = new System.Drawing.Font("Segoe UI", 9F),
                    LinkBehavior = LinkBehavior.NeverUnderline,
                    LinkColor = Color.FromArgb(21, 101, 192),
                    Margin = new Padding(0),
                    Name = _resetAllButtonName,
                    Size = new Size(75, 30),
                    Text = Session.CurrentSession.Resources.GetResource("RESETALL", "Reset All", "").Text,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                _cmdresetall.LinkClicked += (sender, args) =>
                {
                    EnquiryForm.RefreshItem();
                    _cmdresetall.Links[0].Enabled = false;
                };

                _cmdresetall.Links[0].Enabled = false;

                pnlButtons.Controls.Add(_cmdresetall, true);
            }
        }

        /// <summary>
        /// Set the size of the buttons on the toolbar.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void SetButtonSize(int width, int height)
        {
            this.tbcLists.ButtonSize = new Size(width, height);
        }

		/// <summary>
		/// Fetches the new search screen based on the item set in the search type combo box.
		/// </summary>
		/// <param name="sender">The calling combo box.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cbSearchType_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
            try
            {
                if (_lastsearchtype != cbSearchType.SelectedIndex)
                {
                    ucErrorBox1.Visible = false;
                    pnlSearchTop.Visible = true;
                    pnlFilter.Visible = true;
                    pnlResultsBack.Visible = true;
                    tbcLists.Enabled = true;

                    if (_quickfiltercontrol != null)
                        _quickfiltercontrol.Text = "";
                    if (dgSearchResults.DataSource is System.Data.DataTable)
                    {
                        ((DataTable)dgSearchResults.DataSource).DefaultView.RowFilter = "";
                    }

                    if (cbSearchType.SelectedIndex == -1)
                        return;

                    if (_refresh == false)
                        return;

                    dgSearchResults.DataSource = null;
                    dgSearchResults.Refresh();
                    searchPagination.ResetPages(0);
                    SetSearchList(Convert.ToString(cbSearchType.SelectedValue), true, _parent, _parameters);
                    if (_searchList.Style == SearchListStyle.List || _searchList.Style == SearchListStyle.Filter) Search();
                    if (this.QuickFilterContol != null && this.QuickFilterContol.Visible) this.QuickFilterContol.Focus(); else dgSearchResults.Focus();
                    _lastsearchtype = cbSearchType.SelectedIndex;
                    ApplyButtonEnabledRules();
                    AdjustFilterButtonsLayout();
                }
            }
            catch (Exception ex)
            {
                tbcLists.Enabled = false;
                pnlSearchTop.Visible = false;
                pnlSearch.Visible = false;
                pnlFilter.Visible = false;
                pnlButtons.Visible = false;
                pnlResultsBack.Visible = false;
                ucErrorBox1.SetErrorBox(ex);
                ucErrorBox1.Visible = true;
            }
		}

		/// <summary>
		/// Capture the double click of the grid control and see if an item from the
		/// list can be selected and returned to the client.
		/// </summary>
		/// <param name="sender">Search data grid.</param>
		/// <param name="e">Empty event argumetns.</param>
		private void dgSearchResults_MouseDoubleClick(object sender, MouseEventArgs e)
		{
            if (_currentItem != null && _currentItem.RowIndex > -1 && e.Button == MouseButtons.Left)
            {
				ExecuteDefaultAction();
			}
		}

		private void ExecuteDefaultAction()
		{
			try
			{
				dgSearchResults.Enabled = false;
				Cursor = Cursors.WaitCursor;
                foreach (Control btc in pnlButtons.Controls)
				{
					if (btc is Button && btc.Name == this.DoubleClickAction)
					{
						Button btn = (Button)btc;

						var toolbutton = this.GetOMSToolBarButton(btc.Name);

						if (toolbutton != null && toolbutton.Enabled)
						{
							cmdButton_Click(btn, EventArgs.Empty);
						}
						else if (toolbutton == null)
						{
							cmdButton_Click(btn, EventArgs.Empty);
						}
						return;
					}
				}
				if (this.DoubleClickAction != "None")
					ErrorBox.Show(ParentForm, new FWBS.OMS.OMSException(HelpIndexes.SearchMissingButton, true, this.DoubleClickAction));
			}
			finally
			{
				dgSearchResults.Enabled = true;
				Cursor = Cursors.Default;
				OnSelectedItemDoubleClick();
			}
		}

		/// <summary>
		/// If the data grid has the focus then change the search state to select.
		/// </summary>
		/// <param name="sender">Search data grid.</param>
		/// <param name="e">Empty event arguments.</param>
		private void dgSearchResults_Enter(object sender, System.EventArgs e)
		{
			if (_parentform != null && cmdSelect != null && cmdSelect.Parent != null)
			{
				if (_lastacceptbutton != cmdSearch && _lastacceptbutton != cmdSelect)
					_lastacceptbutton = _parentform.AcceptButton;
				_parentform.AcceptButton = cmdSelect;
			}
            if (dgSearchResults.Columns.Count > 0)
            {
                dgSearchResults.RowsDefaultCellStyle.SelectionBackColor = SystemColors.ActiveCaption;
            }

            OnStateChanged();
		}

		/// <summary>
		/// Set the Height of th pnlSearchForm when the Enquiry Property of the Enquiry form is changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void enqSearch_EnquiryPropertyChanged(object sender, EventArgs e)
		{
            pnlSearchTop.Height = LogicalToDeviceUnits(this.EnquiryForm.Enquiry.CanvasSize.Height) + SystemInformation.GetHorizontalScrollBarHeightForDpi(DeviceDpi);
            AdjustFilterButtonsLayout();
        }

		/// <summary>
		/// If the criteria enquiry form has the focus then change the search state to select.
		/// </summary>
		/// <param name="sender">Criteria enquiry form.</param>
		/// <param name="e">Empty event arguments.</param>
		private void enqSearch_Enter(object sender, System.EventArgs e)
		{
			if (_parentform != null && cmdSearch != null && cmdSearch.Parent != null)
			{
				if (_lastacceptbutton != cmdSearch && _lastacceptbutton != cmdSelect)
					_lastacceptbutton = _parentform.AcceptButton;
				_parentform.AcceptButton = cmdSearch;
			}
			OnStateChanged();
		}

		/// <summary>
		/// If Enquiry for leave then return Accept button to previous
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void enqSearch_Leave(object sender, System.EventArgs e)
		{
			if (_parentform != null)
			{
				_parentform.AcceptButton = _lastacceptbutton;
			}
		}

		/// <summary>
		/// Finds the current item in the search list using the hit test method.
		/// </summary>
		/// <param name="sender">Search data grid.</param>
		/// <param name="e">Mouse event arguments with mouse co-ordinates.</param>
		private void dgSearchResults_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{

            if (e.Button == MouseButtons.Left)
			{
                DataGridView.HitTestInfo hit = dgSearchResults.HitTest(e.X, e.Y);
                if (hit.Type == DataGridViewHitTestType.ColumnHeader)
                {
                    if (dgSearchResults.Columns.Count > 0 
                        && dgSearchResults.CurrentRowIndex >= 0
                        && dgSearchResults.CurrentCell != null
                        && dgSearchResults.Columns[dgSearchResults.CurrentCell.ColumnIndex].SortMode == DataGridViewColumnSortMode.Automatic)
                    {
						ArrayList selected = new ArrayList();
						DataView vw = dtCT.DefaultView;

                        if (dgSearchResults.CurrentRowIndex >= 0 
                             && dgSearchResults.CurrentRowIndex < vw.Count)
                        {
                            selected.Add(vw[dgSearchResults.CurrentRowIndex].Row);
                        }

                        for (int ctr = 0; ctr < System.Math.Min(vw.Count, dgSearchResults.Rows.Count); ctr++)
                        {
                            if (dgSearchResults.Rows[ctr].Selected)
                                selected.Add(vw[ctr].Row);
                        }

                        if (selected.Count > 0)
						{
							lastSelected = new DataRow[selected.Count];
							selected.CopyTo(lastSelected);
						}
					}
				}
                
                if (e.X > (dgSearchResults.RowHeadersVisible ? dgSearchResults.RowHeadersWidth : 0))
                {
                    _currentItem = hit;
                }

                else
					_currentItem = null;

                if (dgSearchResults.MultiSelect && hit.Type == DataGridViewHitTestType.None)
                {
                    if (dgSearchResults.RowHeadersVisible && e.X <= dgSearchResults.RowHeadersWidth)
                    {
                        dgSearchResults.SelectAll();
                    }
                }
            }
            else if (e.Button == MouseButtons.Right && _currentItem != null
                    && _currentItem.RowIndex > -1
                    && _currentItem.RowIndex < dgSearchResults.RowCount
                    && _currentItem.RowIndex != dgSearchResults.CurrentRowIndex)
            {
                dgSearchResults.Rows[_currentItem.RowIndex].Selected = true;
            }
        }

        /// <summary>
		/// Deal with Multi Select clicking and Selecting rows then while holding down the shift select more rows further down
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void dgSearchResults_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            DataGridView.HitTestInfo test = dgSearchResults.HitTest(e.X, e.Y);
            if (e.Button == MouseButtons.Left && test.Type != DataGridViewHitTestType.ColumnHeader)
            {
				ApplyButtonEnabledRules();
                OnItemHover();
                OnItemHovered();
			}
            else if (e.Button == MouseButtons.Right && e.Clicks == 1 && test.Type == DataGridViewHitTestType.Cell)
            {
                dgSearchResults.ContextMenu.Show((Control)sender, e.Location);
            }
		}

        private void dgSearchResults_Sorted(object sender, EventArgs e)
        {
            ApplyButtonEnabledRules();
            OnItemHover();
            OnItemHovered();
        }

        private IEnumerable<UserControls.ContextMenu.ContextMenuButton> PopulateActionPopup(DataGridViewControls.DataGridViewActionsCell cell)
        {
            return _converter.Convert();
        }

        /// <summary>
        /// Deal with Multi Select with Shift and Cursor keys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgSearchResults_KeyUp(object sender, KeyEventArgs e)
		{
			if (IsNavigationKey(e.KeyCode))
			{
				ApplyButtonEnabledRules();
                OnItemHover();
                OnItemHovered();
			}
		}

        private bool IsNavigationKey(Keys keys)
		{
			return keys == Keys.Up 
                   || keys == Keys.Down 
                   || keys == Keys.Left 
                   || keys == Keys.Right 
                   || keys == Keys.PageUp 
                   || keys == Keys.PageDown 
                   || keys == Keys.Home 
                   || keys == Keys.End;
		}

		/// <summary>
		/// Incase the editing of an item is using an ucOMItem control, capture the close event 
		/// and destroy the control and then reapply the search.
		/// </summary>
		/// <param name="sender">ucOMSItem control.</param>
		/// <param name="e">Empty event arguments.</param>
		internal void OMSItem_Close(object sender, NewOMSTypeCloseEventArgs e)
		{
			if (!Disposing)
			{
                if (e.Why != ClosingWhy.Cancel)
                {
                    Search(true, true, true);
                }
                ucOMSItemBase itm = (ucOMSItemBase)sender;
                var offset = LogicalToDeviceUnits(5);
                this.Padding = new Padding(offset, offset, 0, offset);
				pnlCbSelector.Visible = _pnlcbselector;
				tbcLists.Visible = (tbcLists.Buttons.Count > 0);
				if ((this.SearchList.Style == SearchListStyle.Search || this.SearchList.Style == SearchListStyle.Filter) 
                    && this.SearchList.CriteriaForm != null)
				{
                    pnlFilter.Visible = true;
                    pnlSearchTop.Visible = true;
				}
                searchPagination.Visible = _searchList.PaginationVisible;
				pnlMain.Visible = true;
				itm.Dispose();
				if (ClosedOMSItem != null) ClosedOMSItem(this, EventArgs.Empty);
				_itm = null;
			}
		}


		/// <summary>
		/// Quick Search Code on Text Changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtSearch_TextChanged(object sender, System.EventArgs e)
		{
			try
			{
				Filter(_quickfiltercontrol.Text);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "txtSearch_TextChanged: " + ex.Message);
			}
		}

        /// <summary>
        /// Columns or direction of sorting Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgSearchResults_OnSortChanged(object sender, DataGridViewEx.SortDataEventArgs e)
        {
            SortChanged?.Invoke(this, e);
            UpdateSortingParams(e);
            Search();
        }

        /// <summary>
        /// Filters the underlying result set data tables default view.
        /// </summary>
        /// <param name="text">Text to filter by.</param>
        public void Filter(string text)
		{
			try 
			{
                if (dgSearchResults.CurrentRowIndex > -1)
                {
                    dgSearchResults.ClearSelection();
                } 
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Filter(string text)1: " + ex.Message);
			}
			string filter = "";
			try
			{
				foreach (DataGridViewColumn column in dgSearchResults.Columns)
				{
					if (String.IsNullOrEmpty(column.DataPropertyName) == false && dtCT.Columns.Contains(column.DataPropertyName) 
                                                                               && (dtCT.Columns[column.DataPropertyName].DataType == typeof(string) 
                                                                               || IsComputedColumn(column.DataPropertyName))
                                                                               && Convert.ToBoolean(_incsearch[column]))
					{
						string formattedText = text;
						formattedText = formattedText.Replace("'", "''");

						formattedText = formattedText.Replace("[", "");
						formattedText = formattedText.Replace("]", "");

						DataGridViewLabelColumn labelcolumn = column as DataGridViewLabelColumn;
						if (labelcolumn != null && labelcolumn.DataListTable != null && string.IsNullOrEmpty(formattedText) == false)
						{
							string descriptionColumn = "";
							if (labelcolumn.DataListTable.Columns.Count > 1)
								descriptionColumn = labelcolumn.DataListTable.Columns[1].ColumnName;
							else
								descriptionColumn = labelcolumn.DataListTable.Columns[0].ColumnName;
							using (DataView dv = new DataView(labelcolumn.DataListTable))
							{
								dv.RowFilter = String.Format("{0} Like '{1}{2}%'", descriptionColumn, _searchList.QuickSearchPrefix, formattedText);
								bool anymatches = false;
                                var d = new DataView(dtCT).ToTable(true, column.DataPropertyName);
                                foreach (DataRowView drv in dv)
								{
                                    if (_searchList.IsValueInColumn(d, column.DataPropertyName, Convert.ToString(drv[0])))
                                    {
                                        filter = string.Format("{0}[{1}] = '{2}' or ", filter, column.DataPropertyName, drv[0]);
                                        anymatches = true;
                                    }
                                }
								if (anymatches == false)
                                    filter = string.Format("{0}[{1}] = null or ", filter, column.DataPropertyName, _searchList.QuickSearchPrefix, formattedText);
                            }
						}
						else
                            filter = string.Format("{0}[{1}] Like '{2}{3}%' or ", filter, column.DataPropertyName, _searchList.QuickSearchPrefix, formattedText);
                    }
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Filter(string text)2: " + ex.Message);
			}

			if (filter.Length >= 4)
				filter = filter.Substring(0, filter.Length - 4);
			if (String.IsNullOrEmpty(text))
				filter = String.Empty;
            _searchList.Filter(filter);
			ApplyButtonEnabledRules();
			SetCaption();
			OnFilterChanged();
			OnItemHover();
			OnItemHovered();
		}

		private bool IsComputedColumn(string columnName)
		{
			foreach (DataGridViewColumn column in dgSearchResults.Columns)
			{
                if (column.DataPropertyName == columnName)
                {
                    DataGridViewLabelColumn c = column as DataGridViewLabelColumn;
                    if (c != null && (String.IsNullOrEmpty(c.DataListName) == false || string.IsNullOrEmpty(c.DataCodeType) == false))
                        return true;
                }
            }
			return false;
		}

		/// <summary>
		/// Search List Navigation
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void txtSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
			{
				dgSearchResults.Focus();
				e.Handled = true;
			}
		}

		private void tbcLists_ButtonClick(object sender, OMSToolBarButtonClickEventArgs e)
		{
			BeginInvoke((MethodInvoker)(() =>
			{
				tbcLists.Enabled = false;
				Button b = (Button)e.Button.Tag; // In the graphical button is a reference to the Standard Button
				if (b == null)
					return;

				if (((SearchButtonArgs)b.Tag).Action == ButtonActions.ReportMulti)
				{
					ContextMenu _popup = e.Button.DropDownMenu as ContextMenu;
					Point _loc = e.Button.Rectangle.Location;
					_loc.Offset(0, e.Button.Rectangle.Height);
					_popup.Show(e.Button.Parent, _loc);
				}
				else
				{
					cmdButton_Click(b, EventArgs.Empty);
				}
				if (!tbcLists.IsDisposed)
					tbcLists.Enabled = true;
			}));
		}

        private void ucSearchControl_Load(object sender, System.EventArgs e)
		{
			if (Session.CurrentSession.IsLoggedIn)
			{
                if (RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                    SetRTLInternal();
                
                if (_quickfiltercontrol == null) this.QuickFilterContol = txtSearch.Control as Control;

                if (_enquirydisconnect != null && _enquirydisconnect.InDesignMode) return;

				if (_searchList != null)
				{
					Favourites fav = new Favourites("SL-" + SearchList.SearchListType);
					if (fav.Count > 0)
					{
						cbSearchType.SelectedValue = fav.Description(0);
						cbSearchType_SelectionChangeCommitted(sender, e);
					}
					else
					{
						if (_searchList.Style == SearchListStyle.List || _searchList.Style == SearchListStyle.Filter)
						{
							//DM - 04/06/04 - Only execute a search if not already searched.
							if (_searchList.NotSearched)
								Search();
							else
								Searched(_searchList.DataTable);
						}
						else
						{
							if (_searchList.NotSearched)
								dgSearchResults.DataSource = null;
							else
								Searched(_searchList.DataTable);
						}

						if (_quickfiltercontrol == null) this.QuickFilterContol = txtSearch.Control as Control;
						_parentform = Global.GetParentForm(this);
						ApplyButtonEnabledRules();
                        AdjustFilterButtonsLayout();
					}
				}
			}
		}

		private void dgSearchResults_Leave(object sender, System.EventArgs e)
		{
            dgSearchResults.RowsDefaultCellStyle.SelectionBackColor = SystemColors.InactiveCaption;
            if (_parentform != null && _lastacceptbutton != null)
			{
				_parentform.AcceptButton = _lastacceptbutton;
			}
		}

		private void ApplyActiveTrashFilter(Button tb)
		{
			SearchButtonArgs b = (SearchButtonArgs)tb.Tag;
            switch (b.Action)
			{
				case ButtonActions.ViewTrash:
					_trashactivestate = ActiveState.Inactive;
					break;
				case ButtonActions.ViewActive:
					_trashactivestate = ActiveState.Active;
					break;
			}
			_searchList.ApplyActiveFilter(_trashactivestate);
            ApplyButtonEnabledRules();
            if (_searchList.PaginationVisible)
            {
                resetPagination = true;
                Search();
            }
			SetCaption();
			OnItemHover();
			OnItemHovered();
		}

		private void SetCaption()
		{
			if (_searchList == null)
				return;
            int returnedRecords = 0, totalRecords = 0;
            if (_searchList.DataTable != null)
            {
                returnedRecords = _searchList.DataTable.Rows.Count;
                if (returnedRecords != 0)
                {
                    if (dgSearchResults.RowCount > 0) SelectRowByIndex(0);
                    totalRecords = Convert.ToInt32(_returnValues?["total"]?.Value);
                }
            }
            if (_searchList.PaginationVisible)
            {
                searchPagination.UpdatePageControls(totalRecords);
            }
            string captionText = _searchList.Caption.Replace("%RECCOUNT%", returnedRecords.ToString());
			captionText = captionText.Replace("%FILTERCOUNT%", _searchList.ResultCount.ToString());
			captionText = captionText.Replace("%FILTERDESC%", ExternalFilter);
            lblCaption.Text = captionText;
        }

		private void SetButtonEnabled(object o, bool enabled)
		{
			Button btn = o as Button;
			if (btn != null)
			{
				btn.Enabled = enabled;
				return;
			}
            OMSToolBarButton tbbtn = o as OMSToolBarButton;
			if (tbbtn != null)
			{
				tbbtn.Enabled = enabled;
				return;
			}
            //Need to check the grouped buttons
		}

		private void ApplyButtonEnabledRules()
		{
			if (_searchList != null)
			{
				tbcLists.SuspendLayout();
                int visibleRowCount = this.dgSearchResults.VisibleRowCount;
                foreach (object o in _disabledonnowrows)
				{
                    SetButtonEnabled(o, (visibleRowCount > 0));
                }
                SetButtonEnabled(_tbedit, visibleRowCount > 0);
                SetButtonEnabled(_cmdedit, visibleRowCount > 0);
                SetButtonEnabled(_cmdselect, visibleRowCount > 0);
                SetButtonEnabled(_tbselect, visibleRowCount > 0);
                switch (_trashactivestate)
				{
					case ActiveState.All:
                        SetButtonEnabled(_cmdrestore, visibleRowCount > 0);
                        SetButtonEnabled(_tbrestore, visibleRowCount > 0);
                        SetButtonEnabled(_cmddelete, visibleRowCount > 0);
                        SetButtonEnabled(_tbdelete, visibleRowCount > 0);
                        break;
					case ActiveState.Active:
						if (_tbtrash != null) _tbtrash.Pushed = false;
						if (_tbactive != null) _tbactive.Pushed = true;
                        SetButtonEnabled(_tbdelete, visibleRowCount > 0);
                        SetButtonEnabled(_tbrestore, visibleRowCount > 0);
                        SetButtonEnabled(_tbrestore, false);
						SetButtonEnabled(_cmdrestore, false);
						break;
					case ActiveState.Inactive:
						if (_tbactive != null) _tbactive.Pushed = false;
						if (_tbtrash != null) _tbtrash.Pushed = true;
						SetButtonEnabled(_tbdelete, false);
						SetButtonEnabled(_cmddelete, false);
                        SetButtonEnabled(_tbrestore, visibleRowCount > 0);
                        SetButtonEnabled(_cmdrestore, visibleRowCount > 0);
                        break;
					default:
						break;
				}
                if (this.dgSearchResults.VisibleRowCount > 0)
                {
					if (_multiSelect == Common.TriState.True ? true : false)
					{
						int count = SelectedItems.Length;
						foreach (object o in _disabledomultiselect)
							SetButtonEnabled(o, (count <= 1));
                    }
				}
                OnButtonEnabledRulesApplied();
				tbcLists.ResumeLayout();
            }
		}

		private void pnlSearchTop_VisibleChanged(object sender, System.EventArgs e)
		{
            if (pnlSearchTop.Visible)
            {
                if (Controls.IndexOf(pnlSearchTop) > Controls.IndexOf(pnlCbSelector))
                {
                    Controls.SetChildIndex(pnlSearchTop, Controls.IndexOf(pnlCbSelector));
                    pnlFilter.BringToFront();
                }
                pnlFilter.Visible = tbcLists.Visible;
                AdjustFilterButtonsLayout();
            }
            if (this.intenqSearch != null && Convert.ToString(this.intenqSearch.Tag) == "Internal")
            {
                this.intenqSearch.Dock = DockStyle.Fill;
            }
            pnlMain.BringToFront();
        }

        private void pnlSearchTop_SizeChanged(object sender, System.EventArgs e)
		{
			pnlSearchTop.Invalidate();
        }

        private void lblSearch_Enter(object sender, System.EventArgs e)
		{
			QuickFilterContol.Focus();
		}

		private void mnuInfo_Popup(object sender, System.EventArgs e)
		{
			mnuSearchName.Text = "SL Name : " + this.SearchList.Code;
		}

		private void dgSearchResults_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
            if (dgSearchResults.MultiSelect && e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                e.Handled = true;
                dgSearchResults.SelectAll();
                ApplyButtonEnabledRules();
                OnItemHover();
                OnItemHovered();
                return;
            }
            if (e.Alt || e.Control || e.Shift)
            {
                return;
            }
            if (IsNavigationKey(e.KeyCode))
            {
                ApplyButtonEnabledRules();
                OnItemHover();
                OnItemHovered();
                return;
            }
            if (e.KeyValue == 13  && dgSearchResults.CurrentRowIndex > -1 && dgSearchResults.Focused)
            {
				ExecuteDefaultAction();
			}
			if (QuickFilterContol != null && QuickFilterContol.Visible && e.KeyValue != 46 &&  e.KeyValue >= 21 && e.KeyValue <= 109)
			{
				Char n = (Char)e.KeyValue;
				Application.DoEvents();
				if (QuickFilterContol.Focused == false)
				{
					ComboBox ctrl1 = QuickFilterContol as ComboBox;
					if (ctrl1 != null)
					{
						ctrl1.Text = n.ToString();
						QuickFilterContol.Focus();
						ctrl1.SelectionStart = 1;
					}
					else
					{
						TextBox ctrl2 = QuickFilterContol as TextBox;
						if (ctrl2 != null)
						{
							ctrl2.Text = n.ToString();
							QuickFilterContol.Focus();
							ctrl2.SelectionStart = 1;
						}
					}
				}
			}
            this._cur = dgSearchResults.CurrentRowIndex;
        }

		private void Report_item_Click(object sender, EventArgs e)
		{
            if (dgSearchResults.VisibleRowCount > 0 && this.SearchList.ResultCount > 0)
            {
				try
				{
					_returnValues = CurrentItem();
				}
				catch (Exception ex)
				{
					System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "Report_item_Click: " + ex.Message);
				}
				if (_returnValues == null) _returnValues = new Common.KeyValueCollection();
				for (int ctr = 0; ctr < _searchList.ReplacementParameters.Count; ctr++)
				{
					Common.KeyValueItem val = _searchList.ReplacementParameters[ctr];
					if (_returnValues.Contains(val.Key) == false)
						_returnValues.Add(val.Key, val.Value);
                }
                if (this.EnquiryForm != null && this.EnquiryForm.Enquiry != null)
				{
					foreach (DataColumn c in this.EnquiryForm.Enquiry.Source.Tables["DATA"].Columns)
					{
						_returnValues.Remove(c.ColumnName);
						_returnValues.Add(c.ColumnName, this.EnquiryForm.Enquiry.Source.Tables["DATA"].Rows[0][c.ColumnName]);
					}
				}
			}
            try
			{
				FWBS.OMS.UI.Windows.Services.Reports.OpenReport(Convert.ToString(_multireports[((MenuItem)sender).Handle.ToInt64()]), SearchList.Parent, _returnValues, false, this.ParentForm);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
		}

		private Enquiry _enquirydisconnect;
        private bool _convertUnits = true;

        /// <summary>
        /// Converts a Logical DPI value to its equivalent DeviceUnit DPI value if necessary.
        /// </summary>
        /// <param name="value">The Logical value to convert.</param>
        /// <returns>The resulting DeviceUnit value.</returns>
        private new int LogicalToDeviceUnits(int value)
        {
            return _convertUnits ? base.LogicalToDeviceUnits(value) : value;
        }

        private void ucSearchControl_ParentChanged(object sender, System.EventArgs e)
		{
			if (Session.CurrentSession.IsLoggedIn)
			{
                _convertUnits = false;
                try
				{
					if (this.Parent is EnquiryForm && _enquirydisconnect == null)
					{
						if (RefreshOnEnquiryFormRefreshEvent)
						{
							EnquiryForm _enqform = this.Parent as EnquiryForm;
							if (_enqform.Enquiry != null)
							{
								_enqform.Enquiry.Refreshed += new EventHandler(Enquiry_Refreshed);
								_enquirydisconnect = _enqform.Enquiry;
							}
						}
					}
					else if (this.Parent == null && _enquirydisconnect != null)
					{
						if (RefreshOnEnquiryFormRefreshEvent)
						{
							_enquirydisconnect.Refreshed -= new EventHandler(Enquiry_Refreshed);
						}
					}

					if (this.Parent is EnquiryForm && _searchlistcode != "")
					{
						EnquiryForm _enqform = this.Parent as EnquiryForm;

						if (_enqform.Enquiry != null)
						{
							_enqform.Enquiry.Refreshed += new EventHandler(Enquiry_Refreshed);
							_enquirydisconnect = _enqform.Enquiry;
						}

						SetSearchList(_searchlistcode, _enqform.Enquiry.Parent, _enqform.Enquiry.ReplacementParameters);
						if (_enqform.Enquiry.InDesignMode)
						{
							_searchList.ParameterHandler += new FWBS.OMS.SourceEngine.SetParameterHandler(this.SetParameterHandler);
						}
					}
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
                _convertUnits = true;
            }
		}

		private void Enquiry_Refreshed(object sender, EventArgs e)
		{
			System.Diagnostics.Trace.WriteLine(String.Format("Enquiry Form Refreshed {0}", this.SearchList.Code));
			this.Search(true,true,true);
		}

		private void SetParameterHandler(string name, out object value)
		{
			if (name == "#UI")
				value = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
			else
				value = DBNull.Value;
		}
        #endregion

        #region Event Methods

        public void OnQueryChanged(object sender, string e)
        {
            _quickfiltercontrol.Text = e;
        }

        /// <summary>
        /// A method used to raise the Selected Item Double Click 
        /// </summary>
        protected void OnSelectedItemDoubleClick()
		{
			if (SelectedItemDoubleClick != null)
				SelectedItemDoubleClick(this,EventArgs.Empty);
		}

		/// <summary>
		/// A method used to raise the search type changed event.
		/// </summary>
		protected void OnSearchTypeChanged()
		{
			if (SearchTypeChanged != null)
				SearchTypeChanged (this, EventArgs.Empty);
		}

		/// <summary>
		/// A method used to raise the search list load event
		/// </summary>
		protected void OnSearchListLoad()
		{
			if (SearchListLoad != null)
				SearchListLoad(this, EventArgs.Empty);
		}

		/// <summary>
		/// A method used to raise the ItemSelected event.
		/// </summary>
		protected void OnItemSelected()
		{
			if (ItemSelected != null)
				ItemSelected (this, EventArgs.Empty);
		}

		protected void OnItemHovered()
		{
			if (ItemHovered != null)
			{
				ItemHovered(this, EventArgs.Empty);
			}
		}

		/// <summary>
		/// A method used to raise the ItemHover event.
		/// </summary>
		protected void OnItemHover()
		{
			if (ItemHover != null)
				ItemHover (this, new SearchItemHoverEventArgs(this.CurrentItem()));
		}


		/// <summary>
		/// A method used to raise the StateChanged event.
		/// </summary>
		protected void OnStateChanged()
		{
			if (StateChanged != null)
				StateChanged (this, new SearchStateEventArgs(this.State));
		}

		/// <summary>
		/// Raises the OnNewOMSTypeWindow event with the specified event arguments.
		/// </summary>
		/// <param name="sender">This control reference.</param>
		/// <param name="e">NewOMSTypeWindow Event arguments.</param>
		protected void OnNewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
		{
			if (NewOMSTypeWindow != null)
				NewOMSTypeWindow(this, e);
			else
			{
				Services.ShowOMSType(e.OMSObject, e.DefaultPage);
				this.Search();
			}
		}

		public void OnNewOMSTypeWindow(NewOMSTypeWindowEventArgs e)
		{
			this.OnNewOMSTypeWindow(this, e);
		}

		/// <summary>
		/// A method used to raise the SearchSompleted event.
		/// </summary>
		protected void OnSearchCompleted()
		{
			if (SearchCompleted != null)
			{
				int count = 0;
				System.Text.StringBuilder ext = new System.Text.StringBuilder();
				if (_searchList != null && _searchList.CriteriaForm != null)
				{

					DataTable questions = _searchList.CriteriaForm.Source.Tables["QUESTIONS"];
					DataTable criteria = _searchList.CriteriaForm.Source.Tables["DATA"];
				
					foreach (DataRow q in questions.Rows)
					{
						try
						{
							string val = Convert.ToString(criteria.Rows[0][Convert.ToString(q["quname"])]);
							if (val == String.Empty)
								continue;
							else
							{
								ext.Append(Convert.ToString(q["qudesc"]));
								ext.Append(" - ");
								ext.Append(val);
								ext.Append(Environment.NewLine);
							}
						}
						catch
						{
							continue;
						}
					}
				}
				count = _searchList.ResultCount;
				SearchCompleted (this, new SearchCompletedEventArgs(count, ext.ToString()));
                this.txtSearch.Invalidate();
			}
		}

		protected void OnBeforeCellDisplay(FWBS.Common.UI.Windows.CellDisplayEventArgs args)
		{
			CellDisplayEventHandler ev = this.BeforeCellDisplay;
			if (ev != null)
			{
				ev(this, args);
			}
		}
		
		protected void OnButtonEnabledRulesApplied()
		{
			if (ButtonEnabledRulesApplied != null)
				ButtonEnabledRulesApplied(this,EventArgs.Empty);
		}

		protected void OnDirty(object sender, EventArgs e)
		{
			if (Dirty != null)
				Dirty(this,EventArgs.Empty);
		}

		protected void OnFilterChanged()
		{
			if (FilterChanged != null)
				FilterChanged(this,EventArgs.Empty);
		}

        private void OnEnquiryFormDirty(object sender, EventArgs e)
        {
            if (_cmdresetall != null)
            {
                _cmdresetall.Links[0].Enabled = ((EnquiryForm)sender).IsDirty;
            }

            if (_searchList.PaginationVisible)
            {
                resetPagination = true;
            }
        }
        #endregion

        #region Properties
        [DefaultValue(false), Category("Auto Load")]
		public bool RefreshOnEnquiryFormRefreshEvent
		{
			get;
			set;
		}

		[Browsable(false)]
		public eToolbars ToolBar
		{
			get
			{
				return tbcLists;
			}
		}
		
		[DefaultValue(null),Category("Auto Load")]
		public string SearchListCode
		{
			get
			{
				return _searchlistcode;
			}
			set
			{
				_searchlistcode = value;
				_searchlisttype = "";
			}
		}

		
		[DefaultValue(null),Category("Auto Load")]
		[RefreshProperties(RefreshProperties.All)]
		public string SearchListType
		{
			get
			{
				return _searchlisttype;
			}
			set
			{
				_searchlisttype = value;
				_searchlistcode = "";
			}
		}

        [DefaultValue(SaveSearchType.Never), Category("Auto Load")]
        [RefreshProperties(RefreshProperties.All)]
        public SaveSearchType SaveSearch
        {
            get
            {
                return _saveSearch;
            }
            set
            {
                _saveSearch = value;
            }
        }


		[DefaultValue(null),Category("Buttons")]
		[RefreshProperties(RefreshProperties.All)]
		public Button cmdSearch
		{
			get
			{
				return _cmdsearch;
			}
			set
			{
				_cmdsearch = value;
				_ecmdsearch = true;
			}
		}

		[DefaultValue(null),Category("Buttons")]
		public Button cmdSelect
		{
			get
			{
				return _cmdselect;
			}
			set
			{
				_cmdselect = value;
				_ecmdselect = true;
			}
		}

		[DefaultValue(null),Category("Buttons")]
		public Button cmdAdd
		{
			get
			{
				return _cmdadd;
			}
			set
			{
				_cmdadd = value;
				_ecmdadd = true;
			}
		}

		[DefaultValue(null),Category("Buttons")]
		public Button cmdEdit
		{
			get
			{
				return _cmdedit;
			}
			set
			{
				_cmdedit = value;
				_ecmdedit = true;
			}
		}

		[DefaultValue(null),Category("Buttons")]
		public Button cmdActive
		{
			get
			{
				return _cmdactive;
			}
			set
			{
				_cmdactive = value;
				_ecmdactive = true;
			}
		}

		[DefaultValue(null),Category("Buttons")]
		public Button cmdTrash
		{
			get
			{
				return _cmdtrash;
			}
			set
			{
				_cmdtrash = value;
				_ecmdtrash = true;
			}
		}

		[DefaultValue(null),Category("Buttons")]
		public Button cmdRestore
		{
			get
			{
				return _cmdrestore;
			}
			set
			{
				_cmdrestore = value;
				_ecmdrestore = true;
			}
		}

		[DefaultValue(null),Category("Buttons")]
		public Button cmdDelete
		{
			get
			{
				return _cmddelete;
			}
			set
			{
				_cmddelete = value;
				_ecmddelete = true;
			}
		}
		

		/// <summary>
		/// Nav Command Panel
		/// </summary>
		[Category("Panel")]
		public ucNavCommands NavCommandPanel
		{
			get
			{
				return tbcLists.NavCommandPanel;
			}
			set
			{
				tbcLists.NavCommandPanel = value;
				if (this.Parent != null) tbcLists.Refresh();
			}
		}

		/// <summary>
		/// Gets a reference to the enquiry form that is being used as the criteria form.
		/// </summary>
		public EnquiryForm EnquiryForm 
		{
			get
			{
				if (_enqsearch == null)
				{
					this.intenqSearch = new FWBS.OMS.UI.Windows.EnquiryForm();
					this.intenqSearch.Tag = "Internal";
					// 
					// intenqSearch
					// 
					this.intenqSearch.AutoScroll = false;
					this.intenqSearch.IsDirty = false;
					this.intenqSearch.Location = new System.Drawing.Point(3, 3);
					this.intenqSearch.Name = "intenqSearch";
					this.intenqSearch.Size = new System.Drawing.Size(528, 163);
					this.intenqSearch.TabIndex = 0;
					this.intenqSearch.ToBeRefreshed = false;
					this.pnlSearchTop.Controls.Add(this.intenqSearch);
					this.EnquiryForm = intenqSearch;
				}
				
				return _enqsearch;
			}
			set
			{
				_enqsearch = value;
				_enqsearch.Enter -= new System.EventHandler(this.enqSearch_Enter);
				_enqsearch.Enter += new System.EventHandler(this.enqSearch_Enter);
				_enqsearch.Leave -= new System.EventHandler(this.enqSearch_Leave);
				_enqsearch.Leave += new System.EventHandler(this.enqSearch_Leave);
				_enqsearch.EnquiryPropertyChanged -=new EventHandler(this.enqSearch_EnquiryPropertyChanged);
				_enqsearch.EnquiryPropertyChanged +=new EventHandler(this.enqSearch_EnquiryPropertyChanged);
                _enqsearch.Dirty -= OnEnquiryFormDirty;
                _enqsearch.Dirty += OnEnquiryFormDirty;

            }
        }

		[Category("OMS")]
		[Description("Use to Set the Search Control")]
		[DefaultValue(null)]
		[Browsable(false)]
		public Control QuickFilterContol
		{
			get
			{
				return _quickfiltercontrol;
			}
			set
			{
				if (_quickfiltercontrol != null)
				{
					_quickfiltercontrol.KeyDown -= new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
					_quickfiltercontrol.TextChanged -= new System.EventHandler(this.txtSearch_TextChanged);
					_quickfiltercontrol.Enter -= new System.EventHandler(this.dgSearchResults_Enter);
					_quickfiltercontrol.Leave -= new System.EventHandler(this.dgSearchResults_Leave);
				}
				if (value != null)
					_quickfiltercontrol = value;
				else
					_quickfiltercontrol = txtSearch.Control as Control;
				_quickfiltercontrol.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
				_quickfiltercontrol.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

				_quickfiltercontrol.Enter += new System.EventHandler(this.dgSearchResults_Enter);
				_quickfiltercontrol.Leave += new System.EventHandler(this.dgSearchResults_Leave);
			}
		}
		/// <summary>
		/// Gets or Sets the current double click action of the search list.  Changing this
		/// value will not change the underlying search business objects equivalent value.
		/// </summary>
		[DefaultValue("")]
		public string DoubleClickAction
		{
			get
			{
				return _dblClickAction;
			}
			set
			{
				_dblClickAction = value;
			}
		}

		/// <summary>
		/// Gets the current selected values that have been picked from the searched list.
		/// </summary>
		[Browsable(false)]
		public FWBS.Common.KeyValueCollection ReturnValues
		{
			get
			{
				return _returnValues;
			}
		}

		/// <summary>
		/// Gets the current selected values that have been picked from the searched list.
		/// </summary>
		[Browsable(false)]
		public FWBS.Common.KeyValueCollection[] SelectedItems
		{
			get
			{
                KeyValueCollection[] selectedItems = null;
                try
				{
                    if (_searchList != null)
                    {
                        List<int> selected = new List<int>();
                        foreach (DataGridViewRow row in dgSearchResults.SelectedRows)
                            selected.Add(row.Index);

                        if (selected.Count == 0 && dgSearchResults.CurrentRowIndex >= 0)
                            selected.Add(dgSearchResults.CurrentRowIndex);

                        selectedItems = _searchList.Select(selected.ToArray());
                    }
				}
				catch(SearchException)
				{
                    selectedItems = null;
				}
                return selectedItems ?? new KeyValueCollection[0];
			}
		}

		/// <summary>
		/// Gets the current search type descriptions.
		/// </summary>
		[Browsable(false)]
		public string CurrentSearchText
		{
			get
			{
				if (_searchList == null)
					return String.Empty;
				else
					return _searchList.Description;
			}
		}

		/// <summary>
		/// Gets the current state of the search Select / Search.
		/// </summary>
		[Browsable(false)]
		public SearchState State
		{
			get
			{
				if ((ActiveControl is DataGrid) || (this.QuickFilterContol.Focused))
					return SearchState.Select;
				else
					return SearchState.Search;
			}
		}

		[DefaultValue(true)]
		[LocCategory("OMS")]
		public bool AutoJumpToQuickSearch
		{
			get
			{
				return _autojump;
			}
			set
			{
				_autojump = value;
			}
		}

		[DefaultValue(Common.TriState.Null),Category("OMS")]
		public Common.TriState MultiSelect
		{
			get
			{
				return _multiSelect;
			}
			set
			{
				_multiSelect = value;
			}
		}

		[DefaultValue(true)]
		[Category("OMS")]
		[Description("Show or hide the Quick Filter panel")]
		public bool QuickFilterVisible
		{
			get
			{
				return _quickFilterVisible;
			}
			set
			{
				_quickFilterVisible = value;
				pnlSearch.Visible = _quickFilterVisible;
			}
		}

		/// <summary>
		/// Gets the search list object associated to the control.
		/// </summary>
		[Browsable(false)]
		public SearchList SearchList
        {
            get { return _searchList; }
        }

        /// <summary>
		/// Gets or Sets the Button Panel Visible or not.
		/// </summary>
		[DefaultValue(false)]
		[Category("OMS")]
		[Description("Show or hide the Buttons")]
		public bool ButtonPanelVisible
		{
			get
			{
				return pnlButtons.Visible;
			}
			set
			{
				pnlButtons.Visible = value;
			}
		}

		/// <summary>
		/// Gets or Sets the Button Panel Visible or not.
		/// </summary>
		[DefaultValue(false)]
		[Category("OMS")]
		[Description("Show or hide the Graphical Buttons")]
		public bool GraphicalPanelVisible
		{
			get
			{
				return _tdpgraphicaltools;
			}
			set
			{
				tbcLists.Visible = value;
				_tdpgraphicaltools = value;
			}
		}

		/// <summary>
		/// Gets or Sets the Search Panel Visible or not
		/// </summary>
		[DefaultValue(true)]
		[Category("OMS")]
		[Description("Show or hide the Search Form Area")]
		public bool SearchPanelVisible
		{
			get
			{
				return _pnlsearchtop;
			}
			set
			{
				_pnlsearchtop = value;
				pnlMain.Visible=value;
			}
		}

		/// <summary>
		/// Gets or Sets the Results Caption Visible or not
		/// </summary>
		[DefaultValue(true)]
		[Category("OMS")]
		[Description("Show or hide the Results Caption")]
		public bool DisplayResultsCaption
		{
            get
            {
                return lblCaption.Visible;
            }
            set
            {
                lblCaption.Visible = value;
            }
        }

		
		/// <summary>
		/// Gets or sets the visiblity of the search / list type selector.
		/// </summary>
		[DefaultValue(true)]
		[Category("OMS")]
		[Description("Show or hide the Search Type Selector")]
		public bool TypeSelectorVisible
		{
			get
			{
				return _pnlcbselector;
			}
			set
			{
				if (cbSearchType.Items.Count > 1)
				{
					_pnlcbselector = value;
					pnlCbSelector.Visible = value;
				}
				else
				{
					pnlCbSelector.Visible = false;
					_pnlcbselector = false;
				}
			}
		}

		/// <summary>
		/// Access to the Returned Data Table
		/// </summary>
		[Browsable(false)]
		public DataTable DataTable
        {
            get { return dtCT; }
        }


        /// <summary>
		/// A Filter that is included in the Search Lists Filters
		/// you do not need to use the DefaultView of ther DataTable Property
		/// The Filter is fitted into the any existing filters
		/// </summary>
		[Browsable(false)]
		[DefaultValue("")]
		public string ExternalFilter
		{
			get
			{
				if (_searchList != null)
					return _searchList.ExternalFilter;
				else
					return _externalfilter;
			}
			set
			{
				if (_searchList != null)
					_searchList.ExternalFilter = value;
				else
					_externalfilter = value;
				ApplyButtonEnabledRules();
				SetCaption();
			}
		}

		[Browsable(false)]
		public int PanelButtonCount
        {
            get
            {
                return pnlbutCount;
            }
        }

        /// <summary>
        /// Enables action column in DataGridView
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="specified"></param>
        [DefaultValue(false)]
        public bool IsActionColumnEnabled { get; set; }

        [Browsable(false), DefaultValue(true), EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool AllowReorderColumns { get; set; }

        #endregion

        #region IOMSItem Implementation

        /// <summary>
        /// IOMSItem Member: Refreshes the data within this object.
        /// </summary>
        public void RefreshItem()
		{
			if (_itm == null)
				Search(true,true);
			else
				_itm.RefreshItem();
		}

		/// <summary>
		/// IOMSItem Member: Closes the OMS Item Control.
		/// </summary>
		public void CloseOMSItem()
		{
			if (_itm != null)
				_itm.CancelUIItem();
		}

        /// <summary>
		/// IOMSItem Member: Updates the data within this object.
		/// </summary>
		public void UpdateItem()
		{
			if (_itm != null)
				_itm.UpdateItem();
		}

        /// <summary>
		/// IOMSItem Member: Cancels the data within this object.
		/// </summary>
		public void CancelItem()
		{
			if (_itm != null)
				_itm.CancelItem();
		}

		/// <summary>
		/// IOMSItem Member: Called when the tab that this object sits on is clicked upon 
		/// from the OMS type dialog.
		/// </summary>
		public void SelectItem()
		{
			//DJRM - I added the _alreadySearched flag to this criteria to stop the search continually being searched when selected.
			if (_itm == null && _alreadySearched == false && (_searchList.Style == SearchListStyle.List || _searchList.Style == SearchListStyle.Filter))
				Search(true,true);

		}


		/// <summary>
		/// IOMSItem Member: Gets a boolean value that indicates whether this class is holding any
		/// unsaved dirty data.
		/// </summary>
		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				if (_itm == null)
					return false;
				else
					return _itm.IsDirty;
			}
		}

        /// <summary>
        /// Refresh the Control next time it become active
        /// </summary>
        [Browsable(false)]
		public bool ToBeRefreshed
        {
            get
            {
                return _toberefresh;
            }
            set
            {
                _toberefresh = value;
            }
        }

        [Browsable(false)]
        public List<DataGridViewColumn> Columns
        {
            get { return _columns; }
        }

        [Category("Appearance")]
		public Color BackGroundColor
		{
			get
			{
				return dgSearchResults.BackgroundColor;
			}
			set
			{
				dgSearchResults.BackgroundColor = value;
			}
		}
		#endregion

		#region Threading

		/// <summary>
		/// Stops the timer that is used to display the hourglass wait window.
		/// </summary>
		private void StopTimer()
		{
			//Stop the timer and close the form if exists.
			timWait.Enabled = false;
			if (hrg != null)
			{
				Form p = FWBS.OMS.UI.Windows.Global.GetParentForm(this);
				if (p != null) p.BringToFront();
				hrg.Close();
				hrg = null;
			}
		}

        /// <summary>
        /// Captures an error from the search object whilst threading.
        /// </summary>
        /// <param name="sender">The search object.</param>
        /// <param name="e">Message event arguments.</param>
        private void searchList_Error(object sender, MessageEventArgs e)
		{
            MessageEventHandler err = Main_searchList_Error;
			try
			{
				//Invoke across threads using the forms invoke method.
				this.Invoke(err,new object [2]{sender, e});
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "searchList_Error: " + ex.Message);
			}
		}
		
		/// <summary>
		/// The main thread on error captured event.
		/// </summary>
		/// <param name="sender">The search list.</param>
		/// <param name="e">Message event arguments.</param>
		private void Main_searchList_Error (object sender, MessageEventArgs e)
		{
			//Enable the search list toolbar.
			tbcLists.Enabled = true;
			StopTimer();
			Application.DoEvents();

			if (_captureException)
				ErrorBox.Show(this.ParentForm, new Exception(String.Format("Unexpected error in Search List '{0}'. Please contact support.",this.SearchList.Code), e.Exception));
			else
				throw e.Exception;
		}


		/// <summary>
		/// Captures the search lists searching event.
		/// </summary>
		/// <param name="sender">The search list calling.</param>
		/// <param name="e">Cancel event arguments.</param>
		private void searchList_Searching(object sender, CancelEventArgs e)
		{
			try
			{
				if (e.Cancel) return;
				//MODIFIED DMB 5/Jul/2005 Moved into try block as threading exception thrown in .Net 2.0
				//Disables the toolbar whilst the search is going on.
				// MODIFIED DCT 18/4/2006 - Instead of a Try catch code to disable tool bar is now move the the Main_searchList_Searching
				CancelEventHandler sch = new CancelEventHandler(this.Main_searchList_Searching);
				this.Invoke(sch, new object[2] { sender, e });
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "searchList_Searching: " + ex.Message);
			}
		}
		
		/// <summary>
		/// The main thread captured event for searching.
		/// </summary>
		/// <param name="sender">The search list calling.</param>
		/// <param name="e">Cancel event arguments.</param>
		private void Main_searchList_Searching(object sender, CancelEventArgs e)
		{
			timWait.Enabled = true;
			tbcLists.Enabled = false;
		}
        
		/// <summary>
		/// Captures the search lists searched event, when the search has finished.
		/// </summary>
		/// <param name="sender">The search list calling.</param>
		/// <param name="e">Searched event arguments.</param>
		private void searchList_Searched(object sender, SearchedEventArgs e)
		{
            try
			{
				SearchedEventHandler sch = new SearchedEventHandler(this.Main_searchList_Searched);
				this.Invoke(sch, new object [2] {sender, e});
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "searchList_Searched: " + ex.Message);
			}
		}

		/// <summary>
		/// Captures the main threads search lists searched event, when the search has finished.
		/// </summary>
		/// <param name="sender">The search list calling.</param>
		/// <param name="e">Searched event arguments.</param>
		private void Main_searchList_Searched(object sender, SearchedEventArgs e)
		{
			//Enable the toolbar when the search is completed.
			tbcLists.Enabled = true;
			Searched(e.Data);
            if ((Parent is FWBS.OMS.UI.Windows.EnquiryForm) == false)
			{
				if (QuickFilterContol != null)
					QuickFilterContol.Focus();
				else
					dgSearchResults.Focus();
			}
		}

		private void Searched(DataTable dt)
		{
			//Stop the timer so that the hourglass wait form does not appear, or closes if already
			//loaded.
			StopTimer();
            _alreadySearched = true;
			dgSearchResults.DataSource = dt;
			dtCT = dt;

            if (_quickfiltercontrol != null && _quickfiltercontrol.Text.Length > 0)
				Filter(_quickfiltercontrol.Text);
            SetCaption();
            if (_cur == -1) _cur = dgSearchResults.CurrentRowIndex;
            if (_cur > -1)
			{
				if (_cur > _searchList.ResultCount - 1)
					_cur = _searchList.ResultCount - 1;

                if (dgSearchResults.Rows.Count > 0)
                {
                    dgSearchResults.CurrentRowIndex = _cur == -1 ? 0 : _cur; //RA Don't set CurrentRowIndex to be -1 as this cause an Exception in the datagrid which doesn't get thrown until the user next clicks on a row in the datagrid
                }
            }
			ApplyButtonEnabledRules();			
			OnStateChanged();
			OnSearchCompleted();
			OnItemHover();
			OnItemHovered();
		}

		/// <summary>
		/// Captures the timer event so that the hourglass wait form can appear whilst 
		/// the search occurs, but only if a certain amount of time has already elapsed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void timWait_Tick(object sender, System.EventArgs e)
		{
			timWait.Enabled = false;
			if (this.Visible)
			{
				if (hrg == null)
				{
					hrg = new frmHourGlass(this);
					hrg.ShowDialog(FWBS.OMS.UI.Windows.Global.GetParentForm(this));
				}
			}
		}

		/// <summary>
		/// Captures the hourglass wait windows closed event.
		/// </summary>
		/// <param name="sender">The current wait window.</param>
		/// <param name="e">Empty event arguments.</param>
		private void hrg_Closed(object sender, EventArgs e)
		{
		}

        private int GetDisplayedButtonsCount()
        {
            var ctrlCount = 0;
            foreach (Control ctrl in pnlButtons.Controls)
            {
                if (ctrl.Height == 0 || string.IsNullOrEmpty(ctrl.Text))
                {
                    ctrl.Visible = false;
                }

                if (ctrl.Visible)
                {
                    ctrlCount++;
                }
            }
            return ctrlCount;
        }

        private bool isHorizontalLayout()
        {
            const int LeftToRightButtonsCount = 2;      
            return GetDisplayedButtonsCount() == LeftToRightButtonsCount;
        }

        private int GetContentHeight(bool isHorizontal)
        {
            if (pnlButtons?.Controls?.Count > 0)
            {
                if (!isHorizontal)
                {
                    var horizontalAlign = SearchList.Code == "SCHDOCSCHTRANS";
                    ApplyAutoSize(horizontalAlign);
                }
                int bottomElement;
                var topElement = bottomElement = pnlButtons.Controls[pnlButtons.Controls.Count - 1].Bounds.Bottom;
                foreach (Control control in pnlButtons.Controls)
                {
                    if (control.Visible)
                    {
                        bottomElement = System.Math.Max(bottomElement, control.Bounds.Bottom);
                        topElement = System.Math.Min(topElement, control.Bounds.Top);
                    }
                }
                return bottomElement - topElement;
            }
            return 0;
        }

        private void ApplyAutoSize(bool isHorizontal)
        {
            foreach (Control control in pnlButtons.Controls)
            {
                if (control.Visible)
                {
                    var ctrl = control as Button;
                    if (ctrl != null)
                    {
                        ctrl.AutoSize = true;
                        ctrl.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                        ctrl.Dock = DockStyle.Fill;
                        ctrl.Margin = isHorizontal
                            ? new Padding(LogicalToDeviceUnits(12), 0, 0, 0)
                            : new Padding(0, LogicalToDeviceUnits(8), 0, 0);
                    }
                }
            }
        }

        internal void EnableTimer()
        {
            timWait.Enabled = true;
            tbcLists.Enabled = false;
        }

        internal void HideProgress()
        {
            tbcLists.Enabled = true;
            if (hrg != null)
            {
                hrg.Close();
                hrg = null;
            }
        }

        #endregion

        #region Button Click Events

        /// <summary>
        /// Single Button Click Event using the Name of the Button for the Calling Text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdButton_Click(object sender, System.EventArgs e)
		{
			try
			{
				Button b = (Button)sender;
				SearchButtonArgs tag = b.Tag as SearchButtonArgs;
				if (tag == null) return;

				if (SearchButtonCommands != null)
				{
					SearchButtonEventArgs ee = new SearchButtonEventArgs(b.Name, tag.Action);
					SearchButtonCommands(this, ee);
					if (ee.Cancel) return;
				}
                if (CommandExecuting != null)
				{
					CommandExecutingEventArgs ee = new CommandExecutingEventArgs(b.Name, tag.Action);
					CommandExecuting(this, ee);
					if (ee.Cancel) return;
				}

                switch (tag.Action)
                {
                    case ButtonActions.ViewActive:
                        ApplyActiveTrashFilter(_cmdactive);
                        break;
                    case ButtonActions.ViewTrash:
                        ApplyActiveTrashFilter(_cmdtrash);
                        break;
                    case ButtonActions.Add:
                        cmdAdd_Click(sender, e);
                        break;
                    case ButtonActions.AddFrom:    // GDM 01/04/2004 Added to allow adding to the selected item of a search list to allow for internal links
                        cmdAddFromSelection_Click(sender, e);
                        break;
                    case ButtonActions.Edit:
                        cmdEdit_Click(sender, e);
                        break;
                    case ButtonActions.EditWizard: // DMB 23/02/2004 added to allow edit wizard
                        cmdEditWizard_Click(sender, e);
                        break;
                    case ButtonActions.EditDialog: // GDM 01/04/2004 added to allow edit dialog
                        cmdEditDialog_Click(sender, e);
                        break;
                    case ButtonActions.Search:
                        cmdSearch_Click(sender, e);
                        break;
                    case ButtonActions.Select:
                        cmdSelect_Click(sender, e);
                        break;
                    case ButtonActions.Service:
                        try { cmdService_Click(sender, e); }
                        catch (Exception ex) { Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "cmdService_Click: " + ex.Message); }
                        break;
                    case ButtonActions.Delete:
                    case ButtonActions.TrashDelete:
                        cmdDelete_Click(sender, e);
                        break;
                    case ButtonActions.Restore:
                        cmdRestore_Click(sender, e);
                        break;
                    case ButtonActions.Report:
                        cmdReport_Click(sender, e);
                        break;
                    case ButtonActions.ReportingServer:
                        cmdReportingServer_Click(sender, e);
                        break;
                    case ButtonActions.SearchList:
                        cmdSearchList_Click(sender, e);
                        break;
                    case ButtonActions.Workflow:
                        cmdWorkflow_Click(sender, e);
                        break;
                    case ButtonActions.SaveSearch:
                        cmdSaveSearch_Click(sender, e);
                        break;
                    case ButtonActions.OpenSearch:
                        cmdOpenSearch_Click(sender, e);
                        break;
                    case ButtonActions.FilterSwing:
                        cmdFilterSwing_Click(sender, e);
                        break;
                }

                if (CommandExecuted != null)
				{
					CommandExecutedEventArgs ee = new CommandExecutedEventArgs(b.Name, tag.Action);
					CommandExecuted(this, ee);
				}
			}
			catch (Exception ex)
			{
                if (!isWorkflowRelatedFileNotFoundException(ex,"ELITE.WORKFLOW.FRAMEWORK.CLIENT.ADMINISTRATOR"))
				    ErrorBox.Show(ParentForm, ex);
			}

		}

        /// <summary>
        /// Condition to test if the Exception is of FileNotFound type and the source is Workflow
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        private static bool isWorkflowRelatedFileNotFoundException(Exception ex, string file)
        {
            Debug.WriteLine(string.Format("{0} dll not found and will be skipped: [{1}]", file, ex.Message), "Workflow");
            return (ex is System.IO.FileNotFoundException) && (ex.Source.ToUpperInvariant() == "FWBS.OMS.WORKFLOW") && (((System.IO.FileNotFoundException)ex).FileName.ToUpperInvariant().StartsWith(file));
        }
        

		/// <summary>
		/// Execute Workflow
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdWorkflow_Click(object sender, EventArgs e)
		{
			try
			{
				SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
				var name = Convert.ToString(b.Parameters);
				Type t = FWBS.OMS.Session.CurrentSession.TypeManager.Load("FWBS.OMS.Workflow.WFRuntime,FWBS.OMS.Workflow");
				MethodInfo mi = t.GetMethod("Execute", new Type[] { typeof(string), typeof(TimeSpan), typeof(IDictionary<string, object>), typeof(FWBS.OMS.IContext) });
				ContextFactory factory = new ContextFactory();
				var ctx = factory.CreateContext(this.SelectedItems, this.SearchList.Parent);
				mi.Invoke(null, new object[] { name, TimeSpan.FromMilliseconds(Int32.MaxValue), null, ctx });
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
		}

        private void cmdSaveSearch_Click(object sender, EventArgs e)
        {
            try 
            {
                Debug.WriteLine("cmdSaveSearch_Click");
                string _xml = SavedSearches.Tools.BuildSearchCriteriaXML(this.EnquiryForm);
                if (ConvertDef.ToInt64(this._lastOpenedSearch, -1) != -1 && (SavedSearches.Tools.IsUpdateToLastLoadedSearchRequired()))
                {
                    Debug.WriteLine(string.Format("--cmdSaveSearch_Click - already saved as ID {0}", this._lastOpenedSearch));
                    SavedSearch ss = SavedSearch.GetSavedSearch(this._lastOpenedSearch);
                    ss.CriteriaXML = _xml;
                    ss.Update();
                    Debug.WriteLine("Updated");
                }
                else
                {
                    Debug.WriteLine(string.Format("--cmdSaveSearch_Click - new search"));
                    string _desc = SavedSearches.Tools.SaveSearchDescription(this);
                    if (String.IsNullOrWhiteSpace(_desc))
                        return;
                    bool _globalsave = SavedSearches.Tools.IsGlobalSearchRequired();
                    string _obj = "";
                    long? _objID = 0;
                    SavedSearches.Tools.GetParentObjectTypeAndID(this.SearchList.Code, this.SearchList.Parent, ref _obj, ref _objID);
                    SavedSearches.SaveSearch(_desc, _xml, this.SearchList.Code, "SEARCHLIST", _obj, _objID, _globalsave);
                    Debug.WriteLine("Saved");
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void cmdOpenSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SavedSearches.OpenSavedSearchAndPopulateForm(this, this.SearchList.Code, this.EnquiryForm, ref this._lastOpenedSearch);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        /// <summary>
		/// Captures the search click of the user control.
		/// </summary>
		/// <param name="sender">Search button object.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdSearch_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				Search();
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

        /// <summary>
        /// Expand/collapse filter area.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmdFilterSwing_Click(object sender, System.EventArgs e)
        {
            pnlSearchTop.Visible = !pnlSearchTop.Visible;
        }

        /// <summary>
        /// Run Service Command
        /// </summary>
        /// <param name="sender">Add button.</param>
        /// <param name="e">Empty event arguments.</param>
        private void cmdService_Click(object sender, System.EventArgs e)
		{
			object ret = null;
			try
			{
				int lastcount = dtCT.Rows.Count;
				Cursor = Cursors.WaitCursor;
				SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
				string parms = b.Parameters;
				if (parms == "")
				{
					return;
				}
				string[] parmstr = parms.Split(";".ToCharArray());
				try
				{
					//Get the current items return value from the selected item and add to the 
					//key name collection all of the parameters from the parent search list.
					try
					{
						_returnValues = CurrentItem();
					}
					catch (Exception ex)
					{
						System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "cmdService_Click:2 " + ex.Message);
					}
					if (_returnValues == null) _returnValues = new Common.KeyValueCollection();

					for (int ctr = 0 ; ctr < _searchList.ReplacementParameters.Count; ctr++)
					{
						Common.KeyValueItem val = _searchList.ReplacementParameters[ctr];
						if (_returnValues.Contains(val.Key) == false)
							_returnValues.Add(val.Key, val.Value);
						
					}
                    //Run the services layer command.
					ret = Services.Run(parmstr[0], SearchList.Parent, _returnValues);
                    //If the return item is an ucOMItem control then assign to its event and replace
					//the look of the search form.
                    var omsItem = ret as ucOMSItemBase;
					if (omsItem != null)
					{
                        OpenOMSItem(omsItem);
					}
					else if (ret is FWBS.OMS.Interfaces.IOMSType)
					{
						OnNewOMSTypeWindow(this, new NewOMSTypeWindowEventArgs((FWBS.OMS.Interfaces.IOMSType)ret));
					}
					else
					{
						Search(true,true,false);
						if (lastcount < dtCT.Rows.Count)
						{
							dtCT.DefaultView.Sort = "";
                            dgSearchResults.CurrentRowIndex = dtCT.DefaultView.Count - 1;
                            ApplyButtonEnabledRules();
                            OnItemHover();
                            OnItemHovered();
						}
					}
				}
				catch (MissingFieldException ex)
				{
					if (SearchList.ResultCount > 0)
					{
						ErrorBox.Show(ParentForm, ex);
					}
					if (ret is ucOMSItemBase)
						OMSItem_Close(_itm, new NewOMSTypeCloseEventArgs(ClosingWhy.Cancel));
				}
				catch (System.Reflection.TargetInvocationException targex)
				{
					ErrorBox.Show(ParentForm, targex.InnerException);
					OMSItem_Close(_itm, new NewOMSTypeCloseEventArgs(ClosingWhy.Cancel));
				}
				catch(Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
					OMSItem_Close(_itm, new NewOMSTypeCloseEventArgs(ClosingWhy.Cancel));
				}
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}
		
		/// <summary>
		/// Adds an item of the type in the list using an enquiry command.
		/// </summary>
		/// <param name="sender">Add button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdAdd_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (dtCT == null)
				{
					ErrorBox.Show(ParentForm, new OMSException2("ERRNOINTSEARCH","Error in Scriping Run the Search() Method of the SearchList after setting the Search list code. Or if done then a error has occured when the search was called please check the Search List",new Exception(),false));
					return;
				}
				int lastcount = dtCT.Rows.Count;
				Cursor = Cursors.WaitCursor;
				SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
                OMSItemWizardCall(b);
				if (lastcount < dtCT.Rows.Count)
				{
					dtCT.DefaultView.Sort = "";
                    if (dtCT.DefaultView.Count - 1 < dgSearchResults.RowCount)
                    {
                        dgSearchResults.CurrentRowIndex = dtCT.DefaultView.Count - 1;
                    }
                    ApplyButtonEnabledRules();
                    OnItemHover();
                    OnItemHovered();
                }
            }           
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void OMSItemWizardCall(SearchButtonArgs b)
        {
            string parms = b.Parameters;
            string[] parmstr = parms.Split(";".ToCharArray());
            if (OMSItemFactory.FormContainer(parmstr[0]) == OMSItemFactory.FormType.OMSItem)
            {
                try
                {
                    object ret = null;
                    if (parmstr.Length > 1 && parmstr[1] == "New")
                        ret = FWBS.OMS.UI.Windows.Services.ShowOMSItem(parmstr[0], _searchList.Parent, EnquiryMode.Add, _returnValues);
                    else
                        ret = FWBS.OMS.UI.Windows.Services.GetOMSItemControl(parmstr[0], _searchList.Parent, EnquiryEngine.EnquiryMode.Add, false, _returnValues);
                    //If the return item is an ucOMItem control then assign to its event and replace
                    //the look of the search form.
                    var omsItem = ret as ucOMSItemBase;
                    if (omsItem != null)
                    {
                        OpenOMSItem(omsItem);
                    }
                    else if (ret is FWBS.OMS.Interfaces.IOMSType)
                    {
                        OnNewOMSTypeWindow(this, new NewOMSTypeWindowEventArgs((FWBS.OMS.Interfaces.IOMSType)ret));
                    }
                    else
                    {
                        Search(true, true);
                    }
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                    OMSItem_Close(_itm, new NewOMSTypeCloseEventArgs(ClosingWhy.Cancel));
                }
            }
            else if (OMSItemFactory.FormContainer(parmstr[0]) == OMSItemFactory.FormType.InPlaceWizard && parmstr.Length > 1)
            { 
                var frm = OMSApp.CreateModelessWizard(parmstr[1], WizardStyle.InPlace, SearchList.Parent) as frmWizard;
                if (frm != null)
                {
                    var omsItem = new ucOMSItemWizard(frm);
                    OpenOMSItem(omsItem);
                }
            }
            else
            {
                Services.Wizards.GetWizard(parmstr[0], SearchList.Parent, EnquiryMode.Add, _parameters);
                Search(true, true, false);
            }
        }

		/// <summary>
		/// Delete the Selected row from the Underling Data Table
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmdDelete_Click(object sender, EventArgs e)
		{
			SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
            if (dgSearchResults.VisibleRowCount > 0 && b.Parameters != "")
            {
				FWBS.Common.ConfigSetting cfg = new FWBS.Common.ConfigSetting(b.Parameters);
				string delmessage = cfg.GetSetting("message","deleteMessage","AreYouSure");
				string errmessage = cfg.GetSetting("message","errorMessage","errdelete");
				string select = cfg.GetSetting("dataBuilder","call","");
                if(this.SearchList.Code == "ADMSEARCHLISTS")
                {
                    if(!CheckLockStateOfObject(Convert.ToString(this.CurrentItem()["schCode"].Value), this.SearchList.Code))
                        ExecuteDeletion(b, cfg, delmessage, errmessage, select);
				}
                else
                    ExecuteDeletion(b, cfg, delmessage, errmessage, select);
            }
			ApplyButtonEnabledRules();
		}

        private void ExecuteDeletion(SearchButtonArgs b, FWBS.Common.ConfigSetting cfg, string delmessage, string errmessage, string select)
        {
            if (FWBS.OMS.UI.Windows.MessageBox.Show(FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText(delmessage, "Are you sure you wish to Delete %1%", "", ""), FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (b.Action == ButtonActions.Delete)
                        DeleteRow(select);
                    else if (b.Action == ButtonActions.TrashDelete)
                        TrashField(cfg, select);
                    dgSearchResults.Focus();
                    if (dgSearchResults.Rows.Count > 0 
                        && dgSearchResults.CurrentRowIndex > -1)
                    {
                        dgSearchResults.CurrentCell = dgSearchResults.Rows[dgSearchResults.CurrentRowIndex].Cells[0];
                    }
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, new Exception(FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText(errmessage, "Error Deleting. Please contact support", "", ""), ex));
                    Search(true, true);
                }
            }
        }

        private void DeleteRow(string select)
        {
            if (dgSearchResults.CurrentRow != null)
            {
                this.SearchList.DeleteRow(dgSearchResults.CurrentRow.Index);
                if (select == "")
                    this.SearchList.UpdateData();
                else
                    this.SearchList.UpdateData(select);
            }
        }

        private void TrashField(FWBS.Common.ConfigSetting cfg, string select)
        {
            if (dgSearchResults.CurrentRow != null)
            {
                this.SearchList.TrashField(dgSearchResults.CurrentRow.Index,
                    cfg.GetSetting("trashCan", "fieldname", ""),
                    cfg.GetSetting("trashCan", "changeValue", ""));
                if (select == "")
                    this.SearchList.UpdateData();
                else
                    this.SearchList.UpdateData(select);
            }
        }

        private bool CheckLockStateOfObject(string selectedschCode, string schCode)
        {
            bool result = false;
            if (FWBS.OMS.Session.CurrentSession.ObjectLocking)
            {
                LockState ls = new LockState();
                LockableObjects lockabletype = GetCurrentObjectType(schCode);
                if (lockabletype != LockableObjects.None)
                {
                    if (ls.CheckObjectLockState(selectedschCode, lockabletype))
                        return true;
                }
            }
            return result;
        }

        private LockableObjects GetCurrentObjectType(string schCode)
        {
            switch (schCode)
            {
                case "ADMSEARCHLISTS":
                    return LockableObjects.SearchList;
                default:
                    return LockableObjects.None;
            }
        }

        private void PaginationLine_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(216, 216, 216), searchPagination.Padding.Top))
            {
                e.Graphics.DrawLine(pen, e.ClipRectangle.Left, e.ClipRectangle.Top,
                    e.ClipRectangle.Left + e.ClipRectangle.Width, e.ClipRectangle.Top);
            }
        }

		private void cmdRestore_Click(object sender, EventArgs e)
		{
			SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
            if (dgSearchResults.VisibleRowCount > 0 && this.SearchList.ResultCount > 0 && b.Parameters != "")
            {
				FWBS.Common.ConfigSetting cfg = new FWBS.Common.ConfigSetting(b.Parameters);
				string select = cfg.GetSetting("dataBuilder","call","");
                this.SearchList.TrashField(dgSearchResults.CurrentRowIndex, cfg.GetSetting("trashCan", "fieldname", ""), cfg.GetSetting("trashCan", "changeValue", ""));
                if (select == "")
					this.SearchList.UpdateData();
				else
					this.SearchList.UpdateData(select);
			}

			ApplyButtonEnabledRules();
		}

		private void cmdSearchList_Click(object sender, EventArgs e)
		{
			SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
            if (dgSearchResults.VisibleRowCount > 0 && this.SearchList.ResultCount > 0 && b.Parameters != "")
            {
                try
				{
					_returnValues = CurrentItem();
				}
				catch (Exception ex)
				{
					System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "cmdSearchList_Click: " + ex.Message);
				}
				if (_returnValues == null) _returnValues = new Common.KeyValueCollection();
				for (int ctr = 0 ; ctr < _searchList.ReplacementParameters.Count; ctr++)
				{
					Common.KeyValueItem val = _searchList.ReplacementParameters[ctr];
					if (_returnValues.Contains(val.Key) == false)
						_returnValues.Add(val.Key, val.Value);
                }
			}
			try
			{
				dgSearchResults.DataSource = null;
				this.SetSearchList(b.Parameters,SearchList.Parent,_returnValues);
				if (this.QuickFilterContol != null)
					this.QuickFilterContol.Text = "";
				if (_searchList.Style != SearchListStyle.Search)
					Search();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
        }

        private void cmdReport_Click(object sender, EventArgs e)
		{
			SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
            if (_returnValues == null) _returnValues = new Common.KeyValueCollection();
            if (dgSearchResults.VisibleRowCount > 0 && this.SearchList.ResultCount > 0)
            {
				try
				{
					_returnValues = CurrentItem();
				}
				catch (Exception ex)
				{
					System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "cmdReport_Click: " + ex.Message);
				}
				for (int ctr = 0; ctr < _searchList.ReplacementParameters.Count; ctr++)
				{
					Common.KeyValueItem val = _searchList.ReplacementParameters[ctr];
					if (_returnValues.Contains(val.Key) == false)
						_returnValues.Add(val.Key, val.Value);
                }
			}
            if (this.EnquiryForm != null && this.EnquiryForm.Enquiry != null)
			{
				foreach (DataColumn c in this.EnquiryForm.Enquiry.Source.Tables["DATA"].Columns)
				{
					_returnValues.Remove(c.ColumnName);
					_returnValues.Add(c.ColumnName, this.EnquiryForm.Enquiry.Source.Tables["DATA"].Rows[0][c.ColumnName]);
				}
			}
            try
			{
				FWBS.OMS.UI.Windows.Services.Reports.OpenReport(b.Parameters, SearchList.Parent, _returnValues, false, this.ParentForm);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}

		}
		
		private void cmdReportingServer_Click(object sender, EventArgs e)
		{
			SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
            if (_returnValues == null) _returnValues = new Common.KeyValueCollection();
            if (dgSearchResults.VisibleRowCount > 0 && this.SearchList.ResultCount > 0)
            {
				try
				{
					_returnValues = CurrentItem();
				}
				catch (Exception ex)
				{
					System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, "cmdReportingServer_Click: " + ex.Message);
				}
				for (int ctr = 0; ctr < _searchList.ReplacementParameters.Count; ctr++)
				{
					Common.KeyValueItem val = _searchList.ReplacementParameters[ctr];
					if (_returnValues.Contains(val.Key) == false)
						_returnValues.Add(val.Key, val.Value);
                }
			}
            if (this.EnquiryForm != null && this.EnquiryForm.Enquiry != null)
			{
				foreach (DataColumn c in this.EnquiryForm.Enquiry.Source.Tables["DATA"].Columns)
				{
					_returnValues.Remove(c.ColumnName);
					_returnValues.Add(c.ColumnName, this.EnquiryForm.Enquiry.Source.Tables["DATA"].Rows[0][c.ColumnName]);
				}
			}
            try
			{
				FWBS.OMS.UI.Windows.Services.Reports.OpenReportingServerReport(b.Parameters, SearchList.Parent, _returnValues, false);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}

		}

		/// <summary>
		/// Edits an item of the type in the list using an enquiry command.
		/// </summary>
		/// <param name="sender">Edit button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdEdit_Click(object sender, System.EventArgs e)
		{
            if (dgSearchResults.VisibleRowCount > 0)
            {
				try
				{
					SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
					Cursor = Cursors.WaitCursor;
					if (SelectRowItem())
					{
						string parms = b.Parameters;
						string[] parmstr = parms.Split(";".ToCharArray());
						object ret = null;
					
						try
						{
							if (parmstr.Length > 1 && parmstr[1] == "New")
								ret = FWBS.OMS.UI.Windows.Services.ShowOMSItem(parmstr[0], _searchList.Parent, EnquiryMode.Edit,_returnValues);
							else
								ret = FWBS.OMS.UI.Windows.Services.GetOMSItemControl(parmstr[0], _searchList.Parent, EnquiryEngine.EnquiryMode.Edit,false,_returnValues);
                            //If the return item is an ucOMItem control then assign to its event and replace
							//the look of the search form.
							if (ret is ucOMSItemBase)
							{
								OpenOMSItem(ret as ucOMSItemBase);
                                ucOMSItemBase n = ret as ucOMSItemBase;
							}
							else if (ret is FWBS.OMS.Interfaces.IOMSType)
							{
								OnNewOMSTypeWindow(this, new NewOMSTypeWindowEventArgs((FWBS.OMS.Interfaces.IOMSType)ret));
							}
							else
								Search(true,true);
						}
						catch (Exception ex)
						{
							ErrorBox.Show(ParentForm, ex);
							OMSItem_Close(_itm,new NewOMSTypeCloseEventArgs(ClosingWhy.Cancel));
						}
					}
					else
					{
						ErrorBox.Show(ParentForm, new SearchException(HelpIndexes.SearchNoReturnFields,this.SearchList.Code));
					}
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

		private void cmdEditDialog_Click(object sender, System.EventArgs e)
		{
            if (dgSearchResults.VisibleRowCount > 0)
            {
				try
				{
					SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
					Cursor = Cursors.WaitCursor;
					if (SelectRowItem())
					{
						string parms = b.Parameters;
						string[] parmstr = parms.Split(";".ToCharArray());
						object ret = null;
					
						try
						{
							ret = FWBS.OMS.UI.Windows.Services.ShowOMSItem(parmstr[0], _searchList.Parent, EnquiryMode.Edit,_returnValues);
                            //If the return item is an ucOMItem control then assign to its event and replace
							//the look of the search form.
							if (ret is ucOMSItemBase)
							{
								OpenOMSItem(ret as ucOMSItemBase);
							}
							else if (ret is FWBS.OMS.Interfaces.IOMSType)
							{
								OnNewOMSTypeWindow(this, new NewOMSTypeWindowEventArgs((FWBS.OMS.Interfaces.IOMSType)ret));
							}
							else
								Search(true,true);
						}
						catch (Exception ex)
						{
							ErrorBox.Show(ParentForm, ex);
							OMSItem_Close(_itm,new NewOMSTypeCloseEventArgs(ClosingWhy.Cancel));
						}
					}
					else
					{
						ErrorBox.Show(ParentForm, new SearchException(HelpIndexes.SearchNoReturnFields,this.SearchList.Code));
					}
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}
		
		/// <summary>
		/// Edits an item of the type in the list using an enquiry command.
		/// </summary>
		/// <param name="sender">Edit button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdEditWizard_Click(object sender, System.EventArgs e)
		{
            if (dgSearchResults.VisibleRowCount > 0)
			{
				try
				{
					SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
					Cursor = Cursors.WaitCursor;
					if (SelectRowItem())
					{
						string parms = b.Parameters;
						string[] parmstr = parms.Split(";".ToCharArray());
						try
						{
							FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(parmstr[0], SearchList.Parent, EnquiryEngine.EnquiryMode.Edit, _returnValues);
							Search(true, true);
						}
						catch (Exception ex)
						{
							ErrorBox.Show(ParentForm, ex);
							OMSItem_Close(_itm, new NewOMSTypeCloseEventArgs(ClosingWhy.Cancel));
						}
					}
					else
					{
						ErrorBox.Show(ParentForm, new SearchException(HelpIndexes.SearchNoReturnFields, this.SearchList.Code));
					}
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

        private void cmdAddFromSelection_Click(object sender, System.EventArgs e)
		{
            if (dgSearchResults.VisibleRowCount > 0)
            {
				try
				{
					SearchButtonArgs b = ((SearchButtonArgs)((Control)sender).Tag);
					Cursor = Cursors.WaitCursor;
					if (SelectRowItem())
					{
						string parms = b.Parameters;
						string[] parmstr = parms.Split(";".ToCharArray());
						try
						{
							FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(parmstr[0], SearchList.Parent, EnquiryEngine.EnquiryMode.Add, _returnValues);
							Search(true, true);
						}
						catch (Exception ex)
						{
							ErrorBox.Show(ParentForm, ex);
							OMSItem_Close(_itm, new NewOMSTypeCloseEventArgs(ClosingWhy.Cancel));
						}
					}
					else
					{
						ErrorBox.Show(ParentForm, new SearchException(HelpIndexes.SearchNoReturnFields, this.SearchList.Code));
					}
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

        public void OpenOMSItem(ucOMSItemBase ret)
		{
			if (_itm != null) return;
			pnlMain.Visible=false;
			pnlCbSelector.Visible=false;
			tbcLists.Visible=false;
            pnlFilter.Visible = false;
            pnlSearchTop.Visible=false;
            searchPagination.Visible = false;
            this.Padding = new Padding(0);
			_itm = ret;
			_itm.Close += this.OMSItem_Close;
			_itm.NewOMSTypeWindow += this.OnNewOMSTypeWindow;
			_itm.Visible = false;
			this.Controls.Add(_itm);
			_itm.Dock = DockStyle.Fill;
			_itm.BringToFront();
			_itm.Visible = true;
            Global.RightToLeftControlConverter(_itm, ParentForm);
            if (OpenedOMSItem != null) OpenedOMSItem(this, EventArgs.Empty);
		}

		/// <summary>
		/// Selects the currently chosen item in the grid to then return back an object to the
		/// calling form.
		/// </summary>
		/// <param name="sender">Select command button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdSelect_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				SelectRowItem();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

        #endregion

        #region Pagination 

        private void onPageSettingsChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void SetPaginationParams()
        {
            if (_searchList.ReplacementParameters != null)
            {
                _searchList.ReplacementParameters.Remove("MAX_RECORDS");
                _searchList.ReplacementParameters.Add(new KeyValueItem("MAX_RECORDS", searchPagination.PageSize));
                _searchList.ReplacementParameters.Remove("PageNo");
                _searchList.ReplacementParameters.Add(new KeyValueItem("PageNo", searchPagination.CurrentPage));
                _searchList.ReplacementParameters.Remove("ACTIVE");
                _searchList.ReplacementParameters.Add(new KeyValueItem("ACTIVE", _trashactivestate == ActiveState.Inactive ? "0" : "1"));
            }
        }

        private void UpdateSortingParams(DataGridViewEx.SortDataEventArgs e)
        {
            if (_searchList.ReplacementParameters != null)
            {
                _searchList.ReplacementParameters.Remove("ORDERBY");
                if (e.Order != SortOrder.None)
                {
                    var order = e.Order == SortOrder.Ascending ? "ASC" : "DESC";
                    _searchList.ReplacementParameters.Add(new KeyValueItem("ORDERBY", $"{e.Column} {order}"));
                }
            }
        }

        private void ResetSorting()
        {
            dgSearchResults.SortedColumn = null;
            if (_searchList.ReplacementParameters != null)
            {
                _searchList.ReplacementParameters.Remove("ORDERBY");
            }
        }

        #endregion

		private void btnDefault_Click(object sender, EventArgs e)
		{
			Favourites fav = new Favourites("SL-" + SearchList.SearchListType);
			if (fav.Count > 0)
				fav.Description(0, Convert.ToString(cbSearchType.SelectedValue));
			else
				fav.AddFavourite(Convert.ToString(cbSearchType.SelectedValue), "");
			fav.Update();
		}

		/// <summary>
		/// Create a CSV file from the SearchList displayed DataTable containing headers
		/// </summary>
		/// <param name="fileName">file to create the CSV as</param>
		public void CreateCSV(string fileName)
		{
			CreateCSV(fileName, false);
		}
		/// <summary>
		/// Create a CSV file from the SearchList displayed DataTable
		/// </summary>
		/// <param name="fileName">file to be created</param>
		/// <param name="excludeHeaders">exclude headers</param>
		public void CreateCSV(string fileName, bool excludeHeaders)
		{
			if (DataTable == null)
				throw new DataException("Data Table is not set");

			if (excludeHeaders)
			{
				List<string> data = new List<string>();
				DataTable dt = _searchList.ListView;
                foreach (DataRow row in dt.Rows)
				{
					data.Add(Convert.ToString(row["lvmapping"]));
				}

				CreateCSV(fileName, excludeHeaders, data);
			  
			}
			else
			{
				Dictionary<string, string> data = new Dictionary<string, string>();
    			DataTable dt = _searchList.ListView;
				foreach (DataRow row in dt.Rows)
				{
					data.Add(Convert.ToString(row["lvmapping"]), Convert.ToString(row["lvdesc"]));
				}

				CreateCSV(fileName,  data);
			}
		}

		/// <summary>
		/// Create a CSV from the search lists datatable specifying the columns to create
		/// </summary>
		/// <param name="fileName">file to be created</param>
		/// <param name="excludeHeaders">exclude the header row</param>
		/// <param name="columns">columns to include in the csv</param>
		public void CreateCSV(string fileName, bool excludeHeaders, List<string> columns)
		{
            GetVisibleSearchListRows();
			FWBS.Common.CSVBuilder.FromDataTable(VisibleResults, fileName, excludeHeaders, columns);
        }

		/// <summary>
		/// Create a CSV from the search lists datatable specifying the columns to create
		/// </summary>
		/// <param name="fileName">file to be created</param>
		/// <param name="columns">columns to include in the csv, alias to be displayed</param>
		public void CreateCSV(string fileName,Dictionary<string, string> columns)
		{
            GetVisibleSearchListRows();
			FWBS.Common.CSVBuilder.FromDataTable(VisibleResults, fileName, columns);
        }

        private void GetVisibleSearchListRows()
        {
            ArrayList selected = new ArrayList();
            DataView vw = dtCT.DefaultView;
            if (dgSearchResults.CurrentRowIndex > -1  && dgSearchResults.CurrentRowIndex < vw.Count)
                selected.Add(vw[dgSearchResults.CurrentRowIndex].Row);
            VisibleResults = vw.ToTable();
        }

		private void mnuSearchName_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(this.SearchList.Code);
			MessageBox.ShowInformation("Name copied to clipboard.");
		}

        private void SetRTLInternal()
        {
            foreach (Control item in this.Controls)
            {
                Global.RightToLeftControlConverter(item, ParentForm);
            }
        }

        public void SetRTL(Form parentform)
        {

        }

        public bool IsObjectDirty()
        {
            if (this.IsDirty)
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show("Changes have been detected, would you like to save?", "3E MatterSphere Administration", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        UpdateItem();
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ParentForm, ex);
                        return false;
                    }
                }
                if (dr == DialogResult.Cancel) return false;
            }
            return true;
        }

	    public DialogResult? CheckDialogResult()
	    {
            if (this.IsDirty)
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show("Changes have been detected, would you like to save?", "3E MatterSphere Administration", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        UpdateItem();
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ParentForm, ex);
                        return null;
                    }
                    return DialogResult.Yes;
                }
                if (dr == DialogResult.No)
                {
                    return DialogResult.No;
                }
                if (dr == DialogResult.Cancel)
                {
                    return DialogResult.Cancel;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Search state options.
    /// </summary>
    public enum SearchState
	{
		Select,
		Search
	}

	/// <summary>
	/// Button Style and Location e.g. Plain Down the Side Graphical above
	/// </summary>
	public enum ButtonStyle
	{
		Plain,
		Graphical
	}

	/// <summary>
	/// Command Executing Event Handler
	/// </summary>
	public delegate void CommandExecutingEventHandler(object sender, CommandExecutingEventArgs e);

	public class CommandExecutingEventArgs : SearchButtonEventArgs
	{
		public CommandExecutingEventArgs(string buttonName, ButtonActions action) : base (buttonName,action)
		{
		}
	}

	/// <summary>  
	/// Command Executing Event Handler
	/// </summary>
	public delegate void CommandExecutedEventHandler(object sender, CommandExecutedEventArgs e);

	public class CommandExecutedEventArgs : EventArgs
	{
		public CommandExecutedEventArgs(string buttonName, ButtonActions action)
		{
			ButtonName = buttonName;
			Action = action;
		}

		public string ButtonName { get; private set; }
		public ButtonActions Action { get; private set; }
	}
	
	/// <summary>
	/// Search state delegate.
	/// </summary>
	public delegate void SearchItemHoverEventHandler (object sender, SearchItemHoverEventArgs e);

	/// <summary>
	/// Search state delegate.
	/// </summary>
	public delegate void SearchButtonEventHandler(object sender, SearchButtonEventArgs e);
	
	/// <summary>
	/// State changed event arguments of the search control.
	/// </summary>
	public class SearchItemHoverEventArgs : EventArgs
	{
		private FWBS.Common.KeyValueCollection _curitem;

        internal SearchItemHoverEventArgs (FWBS.Common.KeyValueCollection inlist)
		{
			_curitem = inlist;
		}

		public FWBS.Common.KeyValueCollection ItemList
        {
            get
            {
                return _curitem;
            }
        }
    }

	/// <summary>
	/// Search Button Args
	/// </summary>
	public class SearchButtonEventArgs : System.ComponentModel.CancelEventArgs
	{
		protected string _curitem;
		protected ButtonActions _action = ButtonActions.None;

        public SearchButtonEventArgs (string ButtonName, ButtonActions action)
		{
			_curitem = ButtonName;
			_action = action;
		}

		public string ButtonName
        {
            get
            {
                return _curitem;
            }
        }

        public ButtonActions Action
        {
            get
            {
                return _action;
            }
        }
    }

	/// <summary>
	/// Search state delegate.
	/// </summary>
	public delegate void SearchStateChangedEventHandler (object sender, SearchStateEventArgs e);

	/// <summary>
	/// State changed event arguments of the search control.
	/// </summary>
	public class SearchStateEventArgs : EventArgs
	{
		private SearchState _state;
		
		private SearchStateEventArgs(){}

		internal SearchStateEventArgs (SearchState state)
		{
			_state = state;
		}

        public SearchState State
        {
            get
            {
                return _state;
            }
        }
    }


	/// <summary>
	/// Search completed delegate.
	/// </summary>
	public delegate void SearchCompletedEventHandler (object sender, SearchCompletedEventArgs e);

	/// <summary>
	/// Search completed changed event arguments of the search control.
	/// </summary>d
	public class SearchCompletedEventArgs : EventArgs
	{
		private int _count = 0;
		private string _criteria = "";

		private SearchCompletedEventArgs(){}

		internal SearchCompletedEventArgs (int count, string criteria)
		{
			_count = count;
			_criteria = criteria;
		}

		public int Count
        {
            get
            {
                return _count;
            }
        }


        public string Criteria
        {
            get
            {
                return _criteria;
            }
        }
    }

    public delegate void CellDisplayEventHandler(object sender, FWBS.Common.UI.Windows.CellDisplayEventArgs e);
}
