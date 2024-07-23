using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Security.Permissions;
using FWBS.OMS.UI.Elasticsearch;
using FWBS.OMS.UI.UserControls.Common;
using FWBS.OMS.UI.Windows.Interfaces;
using Telerik.WinControls.Themes;

namespace FWBS.OMS.UI.Windows
{
    #region ucOMSTypeDisplayV2
    /// <summary>
    /// 38000 A common user controlthat will display OMS Configurable types.  This form exposes
    /// tabs and side panels of information containing potential enquiry forms and complex
    /// user controls.
    /// </summary>
    public class ucOMSTypeDisplayV2 : System.Windows.Forms.UserControl, Interfaces.IOMSTypeDisplay, IEmbeddedDisplay
    {
        #region Events
        /// <summary>
        /// An event that gets raised when a new OMS type object needs to be opened in
        /// a navigational format on the dialog form.
        /// </summary>
        public event NewOMSTypeWindowEventHandler NewOMSTypeWindow = null;

        /// <summary>
        /// The Visibility of the Search Manager
        /// </summary>
        public event EventHandler SearchManagerVisibleChanged = null;

        /// <summary>
        /// Close Info Panel Event
        /// </summary>
        public event EventHandler InfoPanelClose = null;

        /// <summary>
        /// When the Control is reported Dirty
        /// </summary>
        public event EventHandler Dirty = null;

        public event EventHandler SearchCompleted;

        /// <summary>
        /// When the Display should be shown
        /// </summary>
        internal EventHandler<ViewEnum> DisplayRequested;

        /// <summary>
        /// When the tab page was opened
        /// </summary>
        internal EventHandler<TabPage> SelectedIndexChanged;

        /// <summary>
        /// When the Elasticsearch control should be hidden
        /// </summary>
        internal EventHandler HideElasticsearchRequested;

        #endregion

        #region Controls

        private FlowLayoutPanel tcFlowContainer;
        private TabControl tcEnquiryPages;
        private System.Windows.Forms.Panel pnlInfoBack;
        private System.ComponentModel.IContainer components;
        private ucAlert pnlAlerts;
        private bool overridetheme = false;
        private Dictionary<TabPage, OMSType.Tab> tabcontents;
        public ElasticsearchControl ucElasticsearch;

        #endregion

        #region Fields

        #region Constants

        private const string ADVANCED_SECURITY_TREE_NODE_CODE = "ADVSECURITY";
        private const string INFORMATION_PANEL_CONTAINER = "@InfPanelsContainer";
        private const string INFORMATION_PANEL_EXTENDER = "@InfPanelsExtender";

        #endregion Constants

        /// <summary>
        /// The Info Panel Visibility
        /// </summary>
        private bool _pnlpanelpanel = true;
        /// <summary>
        /// The Search Manager Close Button Visibility
        /// </summary>
        private bool _searchmanagerclosevisible = true;
        /// <summary>
        /// The Parent Form
        /// </summary>
        private Form _parentform = null;
        /// <summary>
        /// First tab recognition.
        /// </summary>
        private bool _first = false;
        /// <summary>
        /// OMS Configurable type object.
        /// </summary>
        private FWBS.OMS.Interfaces.IOMSType _dialogobj = null;
        /// <summary>
        /// Stores the current tab.
        /// </summary>
        private TabPage _currentTab = null;
        /// <summary>
        /// Temporary panels to add and remove on each item select / deselect.
        /// </summary>
        private ucPanelNav[] _addinPanels = null;
        /// <summary>
        /// Current Brightness of the panels collection..
        /// </summary>
        private int _increasebrightness = 105;
        /// <summary>
        /// Current panel button style.
        /// </summary>
        private NavButtonStyle _navstyle = NavButtonStyle.Grey;
        /// <summary>
        /// Data set that holds the object types rendering parameters.
        /// </summary>
        private OMSType view = null;
        
        private Panel informationSidePanel = null;

        /// <summary>
        /// A key paor collection that will holds a reference to a specific
        /// tab page using a unique code.  This will be used so that a specific
        /// page can be jumped to by a key name.
        /// </summary>
        private Common.KeyValueCollection _pageKey = null;
        /// <summary>
        /// The Default Nav Commands Actions
        /// </summary>
        private FWBS.OMS.UI.Windows.ucNavCommands ucNavCommands1;
        /// <summary>
        /// The Container Nav for Actions
        /// </summary>
        internal FWBS.OMS.UI.Windows.ucPanelNav PanelActions;
        /// <summary>
        /// The Search Manager
        /// </summary>
        private FWBS.OMS.UI.Windows.ucSearchManager ucSearchManager1;
        private System.Windows.Forms.Panel pnlBackMain;
        private System.Windows.Forms.Panel pnlPanelPanel;
        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;

        /// <summary>
        /// The default tab page to display on load up.
        /// </summary>
        private string _defaultPage = null;

        /// <summary>
        /// Used to store the tab object assigned to the first child in a tree node group.
        /// The code is then assigned to the parent node of the group
        /// </summary>
        ArrayList FirstNodeTabCodes = new ArrayList();

        private int advancedSecurityTabIndex = -1;

        private Windows8Theme windows8Theme1;
        protected internal FWBS.Common.UI.Windows.ucHorizontalNavigationPanel horizontalNavigationPanel;

        private Button cmdCancel;
        private Button cmdSave;
        private Panel panelBottom;
        private TableLayoutPanel tcMainContainer;
        private ucActionsBlock ucActionsBlock;
        private ISearchAdapter searchAdapter;
        private ucSearchTextControl _titleBarSearch;
        private string _navigationPanelSelectedItem;

        public void SetSelectedItem(string item)
        {
            _navigationPanelSelectedItem = item;
        }

        #endregion Fields

        #region Constructors & Destructors

        /// <summary>
        /// Create a new instance of this control.
        /// </summary>
        internal ucOMSTypeDisplayV2()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.Inherit;
            tcEnquiryPages.isBorderDisabled = true;
            SetResources();
            pnlInfoBack.ControlAdded += new ControlEventHandler(this.AddPanel);
        }

        internal void CreateAdapter(ucSearchTextControl titleBarSearch)
        {
            _titleBarSearch = titleBarSearch;
            if (Session.CurrentSession.IsConnected && Session.CurrentSession.IsSearchConfigured)
            {
                var factory = new SearchBuilderFactory();
                SearchBuilder = factory.CreateSearchBuilder();
                this.ucElasticsearch = new ElasticsearchControl
                {
                    Dock = DockStyle.Fill,
                    Visible = false
                };

                searchAdapter = new SearchAdapter(_titleBarSearch, this.ucElasticsearch, this.SearchBuilder);
            }
            else
            {
                searchAdapter = new NullSearchAdapter();
            }

            _titleBarSearch.SearchStarted += OpenSearch;
        }

        private void OpenSearch(object sender, EventArgs e)
        {
            if (!this.Enabled || searchAdapter == null)
            {
                return;
            }

            DisplayRequested?.Invoke(this, ViewEnum.ElasticSearch);

            searchAdapter.SetPageSource(Object);
            searchAdapter.SearchAsync();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (TabPage p in tcEnquiryPages.TabPages)
                {
                    IDisposable dis = p.Tag as IDisposable;
                    if (dis != null)
                        dis.Dispose();
                }

                for (int i = pnlInfoBack.Controls.Count - 1; i >= 0; i--)
                {
                    IDisposable dis = pnlInfoBack.Controls[i] as IDisposable;
                    if (dis != null)
                        dis.Dispose();
                }

                if (tabcontents != null)
                {
                    tabcontents.Clear();
                    tabcontents = null;
                }

                _parentform = null;

                if (tcEnquiryPages.ImageList != null)
                {
                    tcEnquiryPages.ImageList.Dispose();
                    tcEnquiryPages.ImageList = null;
                }

                if (components != null)
                {
                    components.Dispose();
                }
            }

            if (searchAdapter != null)
            {
                searchAdapter.Dispose();
                searchAdapter = null;
            }

            if (_titleBarSearch != null)
            {
                _titleBarSearch.SearchStarted -= OpenSearch;
                _titleBarSearch = null;
            }

            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// 
        /// If you want to edit in Design Mode Use line Below
        /// this.ultraDockManager1 = new UltraDockManager(this.components);
        /// 
        /// Switch back for Release
        /// this.ultraDockManager1 = new omsDockManager(this.components);
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlPanelPanel = new System.Windows.Forms.Panel();
            this.pnlInfoBack = new System.Windows.Forms.Panel();
            this.PanelActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavCommands1 = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.horizontalNavigationPanel = new FWBS.Common.UI.Windows.ucHorizontalNavigationPanel();
            this.tcMainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.tcFlowContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.tcEnquiryPages = new FWBS.OMS.UI.TabControl();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.pnlBackMain = new System.Windows.Forms.Panel();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.pnlAlerts = new FWBS.OMS.UI.Windows.ucAlert();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlPanelPanel.SuspendLayout();
            this.pnlInfoBack.SuspendLayout();
            this.PanelActions.SuspendLayout();
            this.tcMainContainer.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.pnlBackMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPanelPanel
            // 
            this.pnlPanelPanel.BackColor = System.Drawing.Color.White;
            this.pnlPanelPanel.Controls.Add(this.pnlInfoBack);
            this.pnlPanelPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlPanelPanel.Location = new System.Drawing.Point(0, 20);
            this.pnlPanelPanel.Name = "pnlPanelPanel";
            this.pnlPanelPanel.Size = new System.Drawing.Size(157, 468);
            this.pnlPanelPanel.TabIndex = 1;
            // 
            // pnlInfoBack
            // 
            this.pnlInfoBack.AutoScroll = true;
            this.pnlInfoBack.Controls.Add(this.PanelActions);
            this.pnlInfoBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInfoBack.Location = new System.Drawing.Point(0, 0);
            this.pnlInfoBack.Name = "pnlInfoBack";
            this.pnlInfoBack.Padding = new System.Windows.Forms.Padding(8);
            this.pnlInfoBack.Size = new System.Drawing.Size(157, 468);
            this.pnlInfoBack.TabIndex = 13;
            // 
            // PanelActions
            // 
            this.PanelActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.PanelActions.Controls.Add(this.ucNavCommands1);
            this.PanelActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelActions.ExpandedHeight = 31;
            this.PanelActions.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.PanelActions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.PanelActions.HeaderColor = System.Drawing.Color.Empty;
            this.PanelActions.Location = new System.Drawing.Point(8, 8);
            this.PanelActions.Name = "PanelActions";
            this.PanelActions.Size = new System.Drawing.Size(141, 31);
            this.PanelActions.TabIndex = 0;
            this.PanelActions.TabStop = false;
            this.resourceLookup1.SetLookup(this.PanelActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("ACTIONSHEADER", "Actions", ""));
            this.PanelActions.ModernStyle = ucPanelNav.NavStyle.ModernHeader;
            // 
            // ucNavCommands1
            // 
            this.ucNavCommands1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavCommands1.Location = new System.Drawing.Point(0, 24);
            this.ucNavCommands1.Name = "ucNavCommands1";
            this.ucNavCommands1.Padding = new System.Windows.Forms.Padding(5);
            this.ucNavCommands1.BackColor = System.Drawing.Color.FromArgb(244, 244, 244);
            this.ucNavCommands1.Size = new System.Drawing.Size(141, 0);
            this.ucNavCommands1.TabIndex = 15;
            this.ucNavCommands1.TabStop = false;
            this.ucNavCommands1.ModernStyle = true;
            // 
            // horizontalNavigationPanel
            // 
            this.horizontalNavigationPanel.AutoSize = true;
            this.horizontalNavigationPanel.BackColor = System.Drawing.Color.White;
            this.horizontalNavigationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.horizontalNavigationPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.horizontalNavigationPanel.Location = new System.Drawing.Point(0, 0);
            this.horizontalNavigationPanel.Margin = new System.Windows.Forms.Padding(0);
            this.horizontalNavigationPanel.MinimumSize = new System.Drawing.Size(0, 42);
            this.horizontalNavigationPanel.Name = "horizontalNavigationPanel";
            this.horizontalNavigationPanel.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.horizontalNavigationPanel.Size = new System.Drawing.Size(741, 42);
            this.horizontalNavigationPanel.TabIndex = 30;
            this.horizontalNavigationPanel.Visible = false;
            // 
            // tcMainContainer
            // 
            this.tcMainContainer.ColumnCount = 1;
            this.tcMainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tcMainContainer.Controls.Add(this.horizontalNavigationPanel, 0, 0);
            this.tcMainContainer.Controls.Add(this.tcFlowContainer, 0, 1);
            this.tcMainContainer.Controls.Add(this.tcEnquiryPages, 0, 2);
            this.tcMainContainer.Controls.Add(this.panelBottom, 0, 3);
            this.tcMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMainContainer.Location = new System.Drawing.Point(0, 0);
            this.tcMainContainer.Margin = new System.Windows.Forms.Padding(0);
            this.tcMainContainer.Name = "tcMainContainer";
            this.tcMainContainer.RowCount = 4;
            this.tcMainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tcMainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            this.tcMainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tcMainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tcMainContainer.Size = new System.Drawing.Size(741, 488);
            this.tcMainContainer.TabIndex = 0;
            // 
            // tcFlowContainer
            // 
            this.tcFlowContainer.AutoScroll = true;
            this.tcFlowContainer.AutoSize = true;
            this.tcFlowContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcFlowContainer.Location = new System.Drawing.Point(3, 3);
            this.tcFlowContainer.Name = "tcFlowContainer";
            this.tcFlowContainer.Size = new System.Drawing.Size(735, 1);
            this.tcFlowContainer.TabIndex = 1;
            this.tcFlowContainer.WrapContents = false;
            // 
            // tcEnquiryPages
            // 
            this.tcEnquiryPages.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tcEnquiryPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcEnquiryPages.ItemSize = new System.Drawing.Size(0, 1);
            this.tcEnquiryPages.Location = new System.Drawing.Point(0, 0);
            this.tcEnquiryPages.Margin = new System.Windows.Forms.Padding(0);
            this.tcEnquiryPages.Name = "tcEnquiryPages";
            this.tcEnquiryPages.Padding = new System.Drawing.Point(0, 0);
            this.tcEnquiryPages.SelectedIndex = 0;
            this.tcEnquiryPages.Size = new System.Drawing.Size(741, 446);
            this.tcEnquiryPages.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcEnquiryPages.TabIndex = 0;
            this.tcEnquiryPages.Click += new System.EventHandler(this.tcEnquiryPages_Click);
            this.tcEnquiryPages.DoubleClick += new System.EventHandler(this.tcEnquiryPages_DoubleClick);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.cmdCancel);
            this.panelBottom.Controls.Add(this.cmdSave);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBottom.Location = new System.Drawing.Point(0, 446);
            this.panelBottom.Margin = new System.Windows.Forms.Padding(0);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Padding = new System.Windows.Forms.Padding(8);
            this.panelBottom.Size = new System.Drawing.Size(741, 42);
            this.panelBottom.TabIndex = 19;
            this.panelBottom.Paint += new System.Windows.Forms.PaintEventHandler(this.PanelBottom_Paint);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(216)))), ((int)(((byte)(216)))));
            this.cmdCancel.FlatAppearance.BorderSize = 0;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.cmdCancel.Location = new System.Drawing.Point(575, 6);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(0);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 30);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Tag = "cmdCancel";
            this.cmdCancel.Text = "Cance&l";
            this.cmdCancel.UseVisualStyleBackColor = false;
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.cmdSave.FlatAppearance.BorderSize = 0;
            this.cmdSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdSave.ForeColor = System.Drawing.Color.White;
            this.cmdSave.Location = new System.Drawing.Point(658, 6);
            this.cmdSave.Margin = new System.Windows.Forms.Padding(0);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 30);
            this.cmdSave.TabIndex = 1;
            this.cmdSave.Tag = "cmdSave";
            this.cmdSave.Text = "&Save";
            this.cmdSave.UseVisualStyleBackColor = false;
            // 
            // pnlBackMain
            // 
            this.pnlBackMain.BackColor = System.Drawing.SystemColors.Window;
            this.pnlBackMain.Controls.Add(this.tcMainContainer);
            this.pnlBackMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBackMain.Location = new System.Drawing.Point(0, 0);
            this.pnlBackMain.Name = "pnlBackMain";
            this.pnlBackMain.Size = new System.Drawing.Size(741, 488);
            this.pnlBackMain.TabIndex = 18;
            // 
            // pnlAlerts
            // 
            this.pnlAlerts.BackColor = System.Drawing.Color.Transparent;
            this.pnlAlerts.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAlerts.Location = new System.Drawing.Point(0, 0);
            this.pnlAlerts.Name = "pnlAlerts";
            this.pnlAlerts.Size = new System.Drawing.Size(741, 49);
            this.pnlAlerts.TabIndex = 19;
            this.pnlAlerts.Visible = false;
            // 
            // ucOMSTypeDisplayV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.pnlBackMain);
            this.Controls.Add(this.pnlAlerts);
            this.DoubleBuffered = true;
            this.Name = "ucOMSTypeDisplayV2";
            this.Size = new System.Drawing.Size(741, 488);
            this.Load += new System.EventHandler(this.ucOMSTypeDisplay_Load);
            this.VisibleChanged += new System.EventHandler(this.ucOMSTypeDisplay_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.ucOMSTypeDisplay_ParentChanged);
            this.pnlPanelPanel.ResumeLayout(false);
            this.pnlInfoBack.ResumeLayout(false);
            this.PanelActions.ResumeLayout(false);
            this.tcMainContainer.ResumeLayout(false);
            this.tcMainContainer.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.pnlBackMain.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void PanelBottom_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(251, 251, 251), 2))
            {
                e.Graphics.DrawLine(pen, e.ClipRectangle.Location, new Point(e.ClipRectangle.X + e.ClipRectangle.Width, e.ClipRectangle.Top));
            }
        }

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            var enqControl = tcEnquiryPages?.SelectedTab?.Tag as EnquiryForm;
            if (enqControl != null)
            {
                enqControl.AutoScrollPosition = Point.Empty;
            }
            base.OnDpiChangedBeforeParent(e);
        }

        private void CmdRefresh_Click(object sender, EventArgs e)
        {
            IOMSTypeWindow omsTypeWindow = _parentform as IOMSTypeWindow;

            if (omsTypeWindow != null)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    omsTypeWindow.Refresh();
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

        private void ucOMSTypeDisplay_ParentChanged(object sender, EventArgs e)
        {
            tcEnquiryPages.Alignment = tappositions;
            if (alertsvisible)
                pnlAlerts.Dock = DockStyle.Top;
            else
                pnlAlerts.Dock = DockStyle.None;
        }
        #endregion

        #endregion

        #region Methods

        private OMSType.Tab CreateNewTab(string objectCode, string text)
        {
            OMSType.Tab newTab = new OMSType.Tab(view);
            newTab.Source = objectCode;
            newTab.LocalizedCode = new CodeLookupDisplay(objectCode, "DLGTABCAPTION", text, "", "");
            return newTab;
        }

        private static TabPage CreateNewTabPage(string objectCode, string text, OMSType.Tab newTab)
        {
            TabPage newTabPage = new TabPage(newTab.Description);
            newTabPage.Name = objectCode;
            newTabPage.Text = text;
            newTabPage.ToolTipText = "\u00A0";
            return newTabPage;
        }

        public void RefreshElasticsearch()
        {
            searchAdapter.SearchAsync();
        }


        public void RefreshSearchManager()
        {
            try
            {
                if (ucSearchManager1.EnquiryForm.Enquiry == null)
                {
                    ShowSearchManager(SearchManager.ContactManager);
                }
            }
            catch
            {
                ShowSearchManager(SearchManager.ContactManager);
            }
        }


        public void TabControlFocus()
        {
            tcEnquiryPages.Focus();
        }


        public void TabControlItemFocus()
        {
            tcEnquiryPages.Focus();
            if (tcEnquiryPages.SelectedTab != null)
            {
                ucSearchControl ucsctrl = tcEnquiryPages.SelectedTab.Tag as ucSearchControl;
                if (ucsctrl != null)
                {
                    if (ucsctrl.QuickFilterContol != null && ucsctrl.QuickFilterContol.Visible == true)
                    {
                        Trace.WriteLine("Search List Quick Search Focus");
                        ucsctrl.QuickFilterContol.Focus();
                        return;
                    }
                }
                IOMSTypeAddin iomsta = tcEnquiryPages.SelectedTab.Tag as IOMSTypeAddin;
                if (iomsta != null)
                {
                    Trace.WriteLine("IOMSTypeAddin Focus");
                    //return;
                }

                Control ctrl = tcEnquiryPages.SelectedTab.Tag as Control;
                if (ctrl != null && ctrl.Focused == false)
                {
                    Trace.WriteLine("Control Focus");
                    ctrl.Focus();
                    return;
                }
            }
        }


        void tp_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.BackColor = System.Drawing.SystemColors.Window;
        }


        /// <summary>
        /// Displays an instance of the OMS configurable type object display.
        /// </summary>
        /// <param name="obj">OMS Configurable type type object.</param>
        public void Open(FWBS.OMS.Interfaces.IOMSType obj, OMSType omst)
        {
            tabcontents = new Dictionary<TabPage, OMSType.Tab>();
            Parent.Cursor = Cursors.WaitCursor;
            _addinPanels = null;
            _first = false;
            this.tcEnquiryPages.SelectedIndexChanged -= new System.EventHandler(this.tcEnquiryPages_SelectedIndexChanged);
            this.tcEnquiryPages.TabPages.Clear();

            try
            {
                AssignDialogObject(obj);
                omst = AssignView(omst);
                SetGlobalColorTheme(omst);

                this.ipc_Width = view.PanelWidth;

                if (_defaultPage == null && obj.DefaultTab != null)
                {
                    _defaultPage = obj.DefaultTab;
                }

                //Create a new key pair tab collection.
                _pageKey = new Common.KeyValueCollection();

                Debug.WriteLine("ucOMSTypeDisplayV2 Begin Tab Construction");
                TabConstruction(obj);
                Debug.WriteLine("ucOMSTypeDisplayV2 End Tab Construction");

                Debug.WriteLine("ucOMSTypeDisplayV2 Security Tab Construction");
                SecurityTabConstruction(obj);
                Debug.WriteLine("ucOMSTypeDisplayV2 Security Tab Construction");

                RefreshItem(true);

                Debug.WriteLine("ucOMSTypeDisplayV2 Fire First Page...");

                if (RootDisplay)
                {
                    DisplayRequested?.Invoke(this, ViewEnum.StartPoint);
                }
                if (omst is CommandCentreType)
                {
                    if (IsCommandCentre)
                    {
                        DisplayRequested?.Invoke(this, ViewEnum.Default);
                        tcEnquiryPages_SelectedIndexChanged(this, EventArgs.Empty);
                    }
                    this.tcEnquiryPages.SelectedIndexChanged += new System.EventHandler(this.tcEnquiryPages_SelectedIndexChanged);
                }
                else
                {
                    tcEnquiryPages_SelectedIndexChanged(this, EventArgs.Empty);
                    this.tcEnquiryPages.SelectedIndexChanged += new System.EventHandler(this.tcEnquiryPages_SelectedIndexChanged);
                    if (tcEnquiryPages.TabCount > 0)
                    {
                        var factory = new NavigationPanelFactory(horizontalNavigationPanel);
                        var builder = factory.CreateBuilder(tabcontents, tcEnquiryPages, advancedSecurityTabIndex, omst);
                        builder.Build(string.IsNullOrWhiteSpace(_defaultPage) ? _currentTab.Name : _defaultPage);
                        SubscribeToHorizontalNavigationEvents();
                        this.tcMainContainer.RowStyles[0] = new RowStyle(SizeType.Absolute, LogicalToDeviceUnits(42));
                    }
                }

                Debug.WriteLine("ucOMSTypeDisplayV2 Begin Panel Construction");
                PanelConstruction(obj);
                Debug.WriteLine("ucOMSTypeDisplayV2 End Panel Construction");
            }
            finally
            {
                Parent.Cursor = Cursors.Default;
            }
        }

        internal void SelectPage(object sender, System.EventArgs e)
        {
            if (sender is ucNavigationPanel)
            {
                var panel = sender as ucNavigationPanel;
                if (panel.IsSearchPageOpened)
                {
                    DisplayRequested?.Invoke(this, ViewEnum.ElasticSearch);
                    tcEnquiryPages_SelectedIndexChanged(sender, e);
                    return;
                }
            }

            DisplayRequested?.Invoke(this, ViewEnum.Default);
            tcEnquiryPages_SelectedIndexChanged(sender, e);
        }

        private void AssignDialogObject(FWBS.OMS.Interfaces.IOMSType obj)
        {
            //Assign the global form object.
            _dialogobj = obj;

            //Makes sure that a valid object is passed.
            if (_dialogobj == null)
            {
                throw new OMSException2("DIAGNOOBJ", "No object passed to the type window");
            }
        }

        private OMSType AssignView(OMSType omst)
        {
            //Get the config type table structure.
            if (omst == null)
                omst = _dialogobj.GetOMSType();

            view = omst;

            return omst;
        }

        private void SetGlobalColorTheme(OMSType omst)
        {
            //Set global color theme of the panel as a whole.
            overridetheme = omst.OverrideTheme;
            if (omst.OverrideTheme)
            {
                this.ipc_BackColor = view.PanelBackColour;
                this.ipc_PanelBrightness = view.PanelBrightness;
            }
        }

        private void PanelConstruction(FWBS.OMS.Interfaces.IOMSType obj)
        {
            if (view.Panels.Count > 0)
            {
                informationSidePanel = new Panel
                {
                    AutoScroll = false,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Dock = DockStyle.Top,
                    Height = 0
                };

                using (ImageList en = FWBS.OMS.UI.Windows.Images.Entities32())
                {
                    for (int i = view.Panels.Count - 1; i >= 0; i--)
                    {
                        var pnl = view.Panels[i];
                        if (Session.CurrentSession.ValidateConditional(obj, pnl.Conditional))
                        {
                            switch (pnl.PanelType)
                            {
                                case OMSType.PanelTypes.TimeStatistics:
                                    ProcessTimeStatistics(pnl, obj);
                                    break;
                                case OMSType.PanelTypes.Addin:
                                    ProcessAddin(pnl);
                                    break;
                                case OMSType.PanelTypes.DataList:
                                    ProcessDataList(pnl);
                                    break;
                                default:
                                    ProcessStringProperty(pnl);
                                    break;
                            }
                        }
                    }
                }

                var ef = tcEnquiryPages.SelectedTab.Tag as EnquiryForm;
                if (ef != null)
                {
                    ShowInformationPanel(ef);
                }
            }
        }

        private void ProcessStringProperty(OMSType.Panel pnl)
        {
            AddInfoPanel(CreateStringPropertyPanel(pnl));
        }

        private ucPanelNav CreateStringPropertyPanel(OMSType.Panel pnl)
        {
            var panelNav = new ucPanelNav
            {
                ModernStyle = ucPanelNav.NavStyle.ModernHeader,
                Tag = pnl,
                Theme = ExtColorTheme.None,
                TabStop = false,
                Text = pnl.Description,
                Expanded = pnl.Expanded
            };

            ucNavRichText navRichText = new ucNavRichText
            {
                ModernStyle = true,
                Font = new System.Drawing.Font(CurrentUIVersion.Font, CurrentUIVersion.FontSize),
                Dock = DockStyle.Fill,
                Padding = new Padding(3,5,3,3)
            };

            panelNav.Controls.Add(navRichText);
            panelNav.pContainer = navRichText;
            pnlInfoBack.Controls.Add(panelNav);
            RefreshItem(panelNav);
            return panelNav;
        }

        private void AddAddinPanels(ucPanelNav[] addinPanels)
        {
            if (addinPanels != null)
            {
                foreach (ucPanelNav panel in addinPanels)
                {
                    panel.ModernStyle = ucPanelNav.NavStyle.ModernHeader;
                    panel.SetBackColor(Color.FromArgb(244, 244, 244));
                    pnlInfoBack.Controls.Add(panel);
                }
            }
        }

        private void ProcessTimeStatistics(OMSType.Panel pnl, FWBS.OMS.Interfaces.IOMSType obj)
        {
            AddControlToInformationPanel(CreateTimeStatisticsPanel(pnl, obj, true));
            AddInfoPanel(CreateTimeStatisticsPanel(pnl, obj, false));
        }

        private ucGroupPanel CreateTimeStatisticsPanel(OMSType.Panel pnl, FWBS.OMS.Interfaces.IOMSType obj, bool largeFont)
        {
            var groupPanel = new ucGroupPanel
            {
                TitleLabel = pnl.Description,
                Tag = pnl,
            };
            ucTimeStats timeStats = new ucTimeStats(largeFont);
            timeStats.Connect(obj);
            groupPanel.AddContent(timeStats);

            return groupPanel;
        }

        private void ProcessAddin(OMSType.Panel pnl)
        {
            AddInfoPanel(CreateAddinPanel(pnl));
        }

        private ucPanelNav CreateAddinPanel(OMSType.Panel pnl)
        {
            var panelNav = new ucPanelNav
            {
                Expanded = pnl.Expanded,
                ModernStyle = ucPanelNav.NavStyle.ModernHeader,
                Tag = pnl,
                Text = pnl.Description
            };

            try
            {
                OmsObject omsObject = new OmsObject(pnl.Parameter);
                Type type = Global.CreateType(omsObject.Windows, omsObject.Assembly);
                IOMSTypeAddin addin = (IOMSTypeAddin)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
                addin.Initialise(_dialogobj);
                if (addin.Connect(_dialogobj))
                {
                    if (addin.AddinText != null)
                    {
                        panelNav.Text = addin.AddinText;
                    }
                }
                else
                {
                    ucErrorBox err = new ucErrorBox();
                    err.Dock = DockStyle.Fill;
                    err.SetErrorBox(new OMSException2("38001", "Cannot Connect To Addin '%1%'.", null as Exception, false, pnl.Parameter));
                    err.Location = new Point(10, 20);
                    err.BackColor = panelNav.BackColor;
                    err.ForeColor = panelNav.ForeColor;
                    panelNav.Controls.Add(err);
                }

                Control addinControl = addin.UIElement ?? (Control)addin;
                addinControl.RightToLeft = RightToLeft;
                Global.RightToLeftControlConverter(addinControl, ParentForm);
                panelNav.pContainer = addinControl;
                addinControl.Dock = DockStyle.Fill;
                panelNav.Controls.Add(addinControl);
            }
            catch (Exception ex)
            {
                ucErrorBox err = new ucErrorBox();
                err.Dock = DockStyle.Fill;
                err.SetErrorBox(new OMSException2("38002A", "Error Loading Addin '%1%'." + Environment.NewLine + Environment.NewLine + "%2%", ex, false, pnl.Parameter, ex.Message));
                err.Location = new Point(10, 20);
                err.BackColor = panelNav.BackColor;
                err.ForeColor = panelNav.ForeColor;
                panelNav.Controls.Add(err);
            }

            return panelNav;
        }

        private void ProcessDataList(OMSType.Panel pnl)
        {
            AddInfoPanel(CreateDataListPanel(pnl));
        }

        private ucGroupPanel CreateDataListPanel(OMSType.Panel pnl)
        {
            var groupPanel = new ucGroupPanel(350)
            {
                TitleLabel = pnl.Description,
                Tag = pnl
            };

            var dataList = new EnquiryEngine.DataLists(pnl.Parameter);
            dataList.ChangeParent(_dialogobj);
            var table = dataList.Run(false) as DataTable;
            var grid = new ucDataListPreview
            {
                DataSource = table
            };

            groupPanel.AddContent(grid);
            return groupPanel;
        }

        private void AddControlToInformationPanel(Control cntrl)
        {
            informationSidePanel.Controls.Add(cntrl, true);
            cntrl.Dock = DockStyle.Top;
            cntrl.BringToFront();
        }

        private void AddInfoPanel(Control cntrl)
        {
            pnlInfoBack.Controls.Add(cntrl, true);
            cntrl.Dock = DockStyle.Top;
        }

        private void TabConstruction(FWBS.OMS.Interfaces.IOMSType obj)
        {
            //Loop through each of the tabs and add them to the tab page collection and also initiate a control type
            //to be put on them.
            foreach (OMSType.Tab tab in view.Tabs)
            {
                if (tab.UserRoles != "" &&
                    (tab.UserRoles == "" || !Session.CurrentSession.CurrentUser.IsInRoles(tab.UserRoles)))
                {
                    continue;
                }

                if (!Session.CurrentSession.ValidateConditional(obj, tab.Conditional))
                {
                    continue;
                }

                Debug.WriteLine("ucOMSTypeDisplayV2 Construct Tab :" + tab.Description + " (" + tab.Code + ")");
                //Create a tab page based on the description returned.
                TabPage tp = new TabPage(tab.Description);
                tabcontents[tp] = tab;
                tp.ControlAdded += new ControlEventHandler(tp_ControlAdded);
                tp.BackColor = System.Drawing.SystemColors.Window;
                tp.Name = tab.Source;
                tp.ToolTipText = " ";
                bool construct = true;

                if (tab.SourceParam == "" && !OmsObject.Exists(tab.Source))
                {
                    ucErrorBox err = new ucErrorBox();
                    err.SetErrorBox(new OMSException2("38004", "OMS Object '%1%' does not exist.", new Exception(), false, tab.Source));
                    err.Location = new Point(10, 20);
                    tp.Controls.Add(err);
                    construct = false;
                }

                //DM - 18/08/05 - Construct and Initialise all of the addins.
                if (tab.SourceType == OMSObjectTypes.Addin && construct)
                {
                    Debug.WriteLine("ucOMSTypeDisplayV2 If Addin :" + tab.Description + " (" + tab.Code + ")");
                    try
                    {
                        OmsObject omsobj = new OmsObject(tab.Source);
                        Type type = Global.CreateType(omsobj.Windows, omsobj.Assembly);

                        IOMSTypeAddin addin = (IOMSTypeAddin)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
                        Control addinctrl = addin.UIElement ?? (Control)addin;

                        var codeDependentAddin = addin as IOMSTypeAddin2;
                        if (codeDependentAddin != null)
                        {
                            codeDependentAddin.Code = omsobj.Code;
                        }

                        tp.Tag = addin;
                        addinctrl.Visible = false;
                        addinctrl.RightToLeft = this.RightToLeft;
                        tp.Controls.Add(addinctrl);
                        Global.RightToLeftControlConverter(addinctrl, this.ParentForm);

                        Debug.WriteLine("ucOMSTypeDisplayV2 Addin Initialise :" + tab.Description + " (" + tab.Code + ")");
                        addin.Initialise(_dialogobj);
                        Debug.WriteLine("ucOMSTypeDisplayV2 Addin Initialise Complete :" + tab.Description + " (" + tab.Code + ")");

                        //Ad global panels, these should rarely be used.
                        ucPanelNav[] global_panels = addin.GlobalPanels;

                        if (global_panels != null)
                        {
                            foreach (ucPanelNav panel in global_panels)
                            {
                                if (String.IsNullOrEmpty(panel.Name) == false)
                                {
                                    if (pnlInfoBack.Controls.ContainsKey(panel.Name))
                                    {
                                        continue;
                                    }
                                }

                                pnlInfoBack.Controls.Add(panel);
                            }
                        }

                        global_panels = null;
                    }
                    catch (Exception ex)
                    {
                        ucErrorBox err = new ucErrorBox();
                        err.SetErrorBox(new OMSException2("38002A", "Error Loading Addin '%1%'." + Environment.NewLine + Environment.NewLine + "%2%", ex, false, tab.SourceParam, ex.Message));
                        err.Location = new Point(10, 20);
                        tp.Controls.Add(err);
                    }
                }

                //Get the icon to display on the tab.
                tp.ImageIndex = tab.Glyph;
                tcEnquiryPages.AddTabPage(tp);

                //Add the source code and the tab reference to the page collection so that
                //the page can be later set by a key name.
                string src = tab.Source;
                if (_pageKey.Contains(src) == false)
                {
                    _pageKey.Add(src, tp);
                }

                //if no default is set, set the default - this should fefault to the first page
                if (_defaultPage == null)
                {
                    _defaultPage = src;
                }

                //Set the first tab.
                if (_first == false)
                {
                    Debug.WriteLine("ucOMSTypeDisplayV2 Before DoEvents:" + tab.Description + " (" + tab.Code + ")");
                    Application.DoEvents();
                    Debug.WriteLine("ucOMSTypeDisplayV2 After DoEvents:" + tab.Description + " (" + tab.Code + ")");
                    _first = true;
                }
            }
        }

        private void SecurityTabConstruction(FWBS.OMS.Interfaces.IOMSType obj)
        {
            if (Session.CurrentSession.AdvancedSecurity &&
                ((obj is OMSDocument && Session.CurrentSession.AdvancedSecurityDocumentActive) ||
                 (obj is OMSFile && Session.CurrentSession.AdvancedSecurityFileActive) ||
                 (obj is Contact && Session.CurrentSession.AdvancedSecurityContactActive) ||
                 (obj is Client && Session.CurrentSession.AdvancedSecurityClientActive)))
            {
                TabPage tp = new TabPage(Session.CurrentSession.Resources.GetResource("SECURITY", "Security", "").Text);
                tp.ImageIndex = 40;
                try
                {
                    new SystemPermission(Permission.StandardTypeToString(StandardPermissionType.ViewPermissions)).Check();
                    string sectypename = "FWBS.OMS.Addin.Security.Windows.ucSecurity, FWBS.OMS.Addin.Security";
                    Type type = Session.CurrentSession.TypeManager.Load(sectypename);

                    IOMSTypeAddin addin = (IOMSTypeAddin)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);

                    addin.Initialise(_dialogobj);
                    addin.Connect(_dialogobj);

                    Control addinctrl = addin.UIElement ?? (Control)addin;
                    tp.Name = tp.Text;
                    tp.Tag = addin;
                    tp.Controls.Add(addinctrl);
                    addinctrl.RightToLeft = RightToLeft;
                    Global.RightToLeftControlConverter(addinctrl, ParentForm);
                    addinctrl.Dock = DockStyle.Fill;
                    tcEnquiryPages.AddTabPage(tp);
                    advancedSecurityTabIndex = tcEnquiryPages.TabPages.IndexOf(tp);
                }
                catch (Exception ex)
                {
                    ucErrorBox err = new ucErrorBox();
                    err.SetErrorBox(ex);
                    err.Location = new Point(10, 20);
                    tp.Controls.Add(err);
                }
            }
        }

        private void SelectedHorizontalItemChanged(object sender, System.EventArgs e)
        {
            tcEnquiryPages_SelectedIndexChanged(sender, e);
        }
        
        /// <summary>
        /// Capture the tab page click and refresh each of the controls on the tab.
        /// </summary>
        /// <param name="sender">Tabs control object.</param>
        /// <param name="e">Empty event arguments.</param>
        private void tcEnquiryPages_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TabPage _selectedTab = tcEnquiryPages.SelectedTab;
            _selectedTab = sender is ucNavigationPanel
                ? AlignSelectedNavigationItemAsSelectedTab(_selectedTab)
                : AlignSelectedHorizontalItemAsSelectedTab(_selectedTab);
            IOpenOMSType Iomstype = null;

            if (_selectedTab == null || _selectedTab.IsDisposed)
                return;

            SelectedIndexChanged?.Invoke(this, _selectedTab);
            ShowBottomPanel(!(tabcontents.ContainsKey(_selectedTab)
                && tabcontents[_selectedTab].HideCancelSaveButtons));
            if (informationSidePanel != null)
            {
                ClearInformationPanelGarbage();

                var enquiryForm = _selectedTab.Tag as EnquiryForm;
                if (enquiryForm != null && view.Panels.Count > 0)
                {
                    ShowInformationPanel(enquiryForm);
                }
            }

            try
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    ucNavCommands1.Controls.Clear();
                    PanelActions.Visible = false;
                    //Remove any temporary panels.
                    if (_currentTab != _selectedTab)
                    {
                        if (_addinPanels != null)
                        {
                            foreach (ucPanelNav panel in _addinPanels)
                            {
                                pnlInfoBack.Controls.Remove(panel);
                            }
                        }

                        if (_currentTab != null && _currentTab.Tag is ucSearchControl)
                        {
                            ucSearchControl ctrl = (ucSearchControl)_currentTab.Tag;
                            ctrl.HaltSearch();
                        }
                    }

                    _currentTab = _selectedTab;
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                }
                finally
                {
                    PanelActions.Visible = (ucNavCommands1.Controls.Count > 0);

                    Cursor = Cursors.Default;
                }

                //Loop through each of the tabs and add them to the tab page collection and also initiate a control type
                //to be put on them.

                if (_selectedTab.ToolTipText == " ")
                {
                    tcEnquiryPages.Enabled = false;
                    OMSType.Tab tab = tabcontents[_selectedTab];
                    _selectedTab.ToolTipText = tab.Source;
                    switch (tab.SourceType)
                    {
                        case OMSObjectTypes.Enquiry:
                            //RightToLeft should already be set through the enquiry rendering form.
                            EnquiryForm ctrl = new EnquiryForm();
                            try
                            {
                                _selectedTab.Controls.Add(ctrl);
                                ctrl.Dirty -= new EventHandler(OnDirty);
                                ctrl.Enquiry = _dialogobj.Edit(tab.Source, null);
                                ctrl.Dirty += new EventHandler(OnDirty);
                                ctrl.Dock = DockStyle.Fill;

                                foreach (Control c in ctrl.Controls)
                                {
                                    eToolbars etoolbar = c as eToolbars;
                                    if (etoolbar != null)
                                    {
                                        this.ucActionsBlock = etoolbar.ActionsBlock;
                                        etoolbar.NavCommandPanel = ucNavCommands1;
                                        etoolbar.ShowPanelButtons();
                                        PanelActions.Visible = (ucNavCommands1.Controls.Count > 0);
                                        break;
                                    }
                                    else
                                    {
                                        ucOMSTypeTabs tabsintabs = c as ucOMSTypeTabs;
                                        if (tabsintabs != null)
                                        {
                                            tabsintabs.ActivateCurrentTab();
                                        }
                                    }
                                }

                                if (ctrl.ToBeRefreshed)
                                {
                                    ctrl.RefreshItem();
                                    ctrl.ToBeRefreshed = false;
                                }
                                Iomstype = ctrl as IOpenOMSType;
                                _selectedTab.Tag = ctrl;
                            }
                            catch (Exception ex)
                            {
                                ctrl.Visible = false;
                                ucErrorBox err = new ucErrorBox();
                                err.SetErrorBox(ex);
                                err.Location = new Point(10, 20);
                                _selectedTab.Controls.Add(err, true);
                            }
                            return;
                        case OMSObjectTypes.Addin:
                            //DM - 15/08/05 - The addin should already be constructed and intialised by this point.
                            IOMSTypeAddin addin = _selectedTab.Tag as IOMSTypeAddin;
                            if (addin != null)
                            {
                                Control ictrl = addin.UIElement;
                                if (ictrl != null) ictrl.Dock = DockStyle.Fill;
                                try
                                {
                                    if (addin.Connect(_dialogobj))
                                    {
                                        if (addin.AddinText != null) _selectedTab.Text = addin.AddinText;
                                    }
                                    else
                                    {
                                        ucErrorBox err = new ucErrorBox();
                                        err.SetErrorBox(new OMSException2("38001", "Cannot Connect To Addin '%1%'.", null as Exception, false, tab.SourceParam));
                                        err.Location = new Point(10, 20);
                                        _selectedTab.Controls.Add(err, true);
                                    }

                                    ictrl.RightToLeft = RightToLeft;
                                    Global.RightToLeftControlConverter(ictrl, ParentForm);
                                    ictrl.Visible = true;

                                    _addinPanels = addin.Panels;
                                    AddAddinPanels(_addinPanels);

                                    if (addin.ToBeRefreshed)
                                    {
                                        addin.RefreshItem();
                                        addin.ToBeRefreshed = false;
                                    }
                                    Iomstype = ictrl as IOpenOMSType;
                                }
                                catch (Exception ex)
                                {
                                    ucErrorBox err = new ucErrorBox();
                                    err.SetErrorBox(new OMSException2("38002A", "Error Loading Addin '%1%'." + Environment.NewLine + Environment.NewLine + "%2%", ex, false, tab.SourceParam, ex.Message));
                                    err.Location = new Point(10, 20);
                                    _selectedTab.Controls.Add(err, true);
                                }
                            }
                            return;
                        case OMSObjectTypes.List:
                            ucSearchControl sl = new ucSearchControl();
                            sl.AutoJumpToQuickSearch = false;
                            sl.RightToLeft = RightToLeft;

                            try
                            {
                                if (sl.SearchList == null)
                                {
                                    FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                                    param.Add("ID", _dialogobj.LinkValue);
                                    sl.NavCommandPanel = ucNavCommands1;
                                    _selectedTab.Controls.Add(sl, true);
                                    sl.SetSearchList(tab.Source, true, _dialogobj, param);
                                    sl.Dock = DockStyle.Fill;

                                    sl.ShowPanelButtons();
                                    PanelActions.Visible = (ucNavCommands1.Controls.Count > 0);

                                }
                                if (sl.ToBeRefreshed)
                                {
                                    sl.RefreshItem();
                                    sl.ToBeRefreshed = false;
                                }
                                sl.ShowPanelButtons();
                                Iomstype = sl as IOpenOMSType;
                                _selectedTab.Tag = sl;
                            }
                            catch (Exception ex)
                            {
                                ucErrorBox err = new ucErrorBox();
                                err.SetErrorBox(ex);
                                err.Location = new Point(10, 20);
                                _selectedTab.Controls.Add(err, true);
                            }
                            return;
                        case OMSObjectTypes.ListGroup:
                            goto case OMSObjectTypes.List;

                        case OMSObjectTypes.ExtData:
                            //RightToLeft should already be set through the enquiry rendering form.
                            ExtendedDataForm ext1 = new ExtendedDataForm();
                            try
                            {
                                PanelActions.Visible = false;
                                if (ext1.ExtendedData == null)
                                {
                                    if (_dialogobj is FWBS.OMS.Interfaces.IExtendedDataCompatible)
                                    {
                                        FWBS.OMS.Interfaces.IExtendedDataCompatible ext = (FWBS.OMS.Interfaces.IExtendedDataCompatible)_dialogobj;
                                        try
                                        {
                                            ext1.ExtendedData = ext.ExtendedData[tab.Source];
                                            ext1.Dock = DockStyle.Fill;
                                            _selectedTab.Controls.Add(ext1);
                                        }
                                        catch (Exception ex)
                                        {
                                            ucErrorBox err = new ucErrorBox();
                                            err.SetErrorBox(new OMSException(ex, FWBS.OMS.HelpIndexes.ExtendedDataDoesNotExist, true, new string[] { Convert.ToString(ext1.Tag) }));
                                            err.Location = new Point(10, 20);
                                            _selectedTab.Controls.Add(err, true);
                                        }
                                    }
                                }

                                if (ext1.ToBeRefreshed)
                                {
                                    ext1.RefreshItem();
                                    ext1.ToBeRefreshed = false;
                                }
                                Iomstype = ext1 as IOpenOMSType;
                                _selectedTab.Tag = ext1;
                            }
                            catch (Exception ex)
                            {
                                ucErrorBox err = new ucErrorBox();
                                err.SetErrorBox(ex);
                                err.Location = new Point(10, 20);
                                _selectedTab.Controls.Add(err, true);
                            }
                            return;
                        default:
                            {
                                Type type = Session.CurrentSession.TypeManager.Load(tab.SourceParam);
                                Control ictrl = (Control)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
                                ictrl.Tag = tab.SourceParam;
                                _selectedTab.Tag = ictrl;
                                Iomstype = ictrl as IOpenOMSType;
                            }
                            return;
                    }
                }
                else
                {
                    if (_selectedTab.Tag is EnquiryForm)
                    {
                        EnquiryForm ctrl = _selectedTab.Tag as EnquiryForm;
                        foreach (Control c in ctrl.Controls)
                        {
                            eToolbars etoolbar = c as eToolbars;
                            if (etoolbar != null)
                            {
                                ucActionsBlock = etoolbar.ActionsBlock;
                                etoolbar.ShowPanelButtons();
                                PanelActions.Visible = (ucNavCommands1.Controls.Count > 0);
                                break;
                            }
                            else
                            {
                                ucOMSTypeTabs tabsintabs = c as ucOMSTypeTabs;
                                if (tabsintabs != null)
                                {
                                    tabsintabs.ActivateCurrentTab();
                                }
                            }
                        }
                    }
                    else if (_selectedTab.Tag is ucSearchControl)
                    {
                        ucSearchControl sl = _selectedTab.Tag as ucSearchControl;
                        sl.ShowPanelButtons();
                        PanelActions.Visible = (ucNavCommands1.Controls.Count > 0);
                    }
                    else if (_selectedTab.Tag is IOMSTypeAddin)
                    {
                        IOMSTypeAddin addin = _selectedTab.Tag as IOMSTypeAddin;
                        _addinPanels = addin.Panels;
                        if (_addinPanels != null)
                        {
                            foreach (ucPanelNav panel in _addinPanels)
                            {
                                pnlInfoBack.Controls.Add(panel);
                                panel.BringToFront();
                            }
                        }
                    }
                }
            }
            finally
            {
                tcEnquiryPages.Enabled = true;
                //If the control is an IOpenOMSType then allow the control the opportunity
                //To give the parent form an OMSType object to deal with.
                if (Iomstype != null)
                {
                    Iomstype.NewOMSTypeWindow += new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
                }
                // Code to SelectItem Added by MNW 28/02/2006 to fix TimeRecording UC
                //Select the item control.
                if (_selectedTab.Tag is IOMSItem)
                {
                    try
                    {
                        ((IOMSItem)_selectedTab.Tag).SelectItem();
                    }
                    catch { }
                }
                tcEnquiryPages.Focus();
                
                Debug.WriteLine("Call frmOMSTypeV2.Refresh()");
                var form = FindForm();
                form?.Refresh();
            }
        }
        
        private void ClearInformationPanelGarbage()
        {
            EnquiryForm enquiryForm = informationSidePanel.Parent?.Parent as EnquiryForm;
            if (enquiryForm != null)
            {
                try
                {
                    enquiryForm.Controls.Remove(enquiryForm.GetControl(INFORMATION_PANEL_EXTENDER, EnquiryControlMissing.Exception));
                }
                catch (Exception)
                { }

                try
                {
                    Control container = enquiryForm.GetControl(INFORMATION_PANEL_CONTAINER, EnquiryControlMissing.Exception);
                    enquiryForm.RefreshingControls -= InformationSidePanel_EnquiryForm_RefreshingControls;
                    informationSidePanel.SizeChanged -= InformationSidePanel_SizeChanged;
                    container.Controls.Remove(informationSidePanel);
                    container.Visible = false;
                    container.Width = 0;
                }
                catch (Exception)
                { }
            }
        }

        private void ShowInformationPanel(EnquiryForm enquiryForm)
        {
            if (IsSidePanelVisible(enquiryForm.Code) && view.Panels.Count > 0)
            {
                try
                {
                    enquiryForm.AutoScrollPosition = Point.Empty;
                    Control container = enquiryForm.GetControl(INFORMATION_PANEL_CONTAINER, EnquiryControlMissing.Exception);
                    container.Visible = true;
                    container.Width = LogicalToDeviceUnits(ipc_Width);
                    enquiryForm.RefreshingControls += InformationSidePanel_EnquiryForm_RefreshingControls;
                    informationSidePanel.SizeChanged += InformationSidePanel_SizeChanged;
                    container.Controls.Add(informationSidePanel);
                    UpdateActionsBlock();
                }
                catch (Exception)
                { }
            }
        }

        private void InformationSidePanel_SizeChanged(object sender, EventArgs e)
        {
            if (informationSidePanel.Parent != null)
            {
                var enquiryForm = tcEnquiryPages.SelectedTab.Tag as EnquiryForm;
                if (enquiryForm != null)
                {
                    try
                    {
                        var fakePanel = enquiryForm.GetControl(INFORMATION_PANEL_EXTENDER, EnquiryControlMissing.Exception);
                        fakePanel.Location = new Point(0, informationSidePanel.Height + enquiryForm.DisplayRectangle.Y);
                        if (fakePanel.Parent == null)
                        {
                            enquiryForm.Controls.Add(fakePanel);
                        }
                    }
                    catch (OMSException2)
                    {
                        if (informationSidePanel.Parent.ClientSize.Height < informationSidePanel.Height)
                        {
                            // Fake control to increase DisplayRectangle of EnquiryForm.
                            Panel fakePanel = new Panel
                            {
                                Height = 0,
                                Width = 0,
                                Name = INFORMATION_PANEL_EXTENDER,
                                Location = new Point(0, informationSidePanel.Height + enquiryForm.DisplayRectangle.Y)
                            };
                            enquiryForm.Controls.Add(fakePanel);
                        }
                    }
                    catch (Exception)
                    { }
                }
            }
        }

        private void InformationSidePanel_EnquiryForm_RefreshingControls(object sender, CancelEventArgs e)
        {
            ((EnquiryForm)sender).RefreshedControls += InformationSidePanel_EnquiryForm_RefreshedControls;
            ClearInformationPanelGarbage();
        }

        private void InformationSidePanel_EnquiryForm_RefreshedControls(object sender, EventArgs e)
        {
            ((EnquiryForm)sender).RefreshedControls -= InformationSidePanel_EnquiryForm_RefreshedControls;
            ShowInformationPanel((EnquiryForm)sender);
        }

        private void UpdateActionsBlock()
        {
            if (ucActionsBlock != null)
            {
                ucActionsBlock.Margin = new Padding(LogicalToDeviceUnits(96), 0, 0, 0);
                ucActionsBlock.Dock = DockStyle.Top;
                informationSidePanel.SuspendLayout();
                var informationPanelControls = informationSidePanel.Controls;
                for (var i = 0; i < informationPanelControls.Count; i++)
                {
                    var actionsBlock = informationPanelControls[i] as ucActionsBlock;
                    if (actionsBlock != null)
                    {
                        informationPanelControls.RemoveAt(i);
                        informationPanelControls.Add(ucActionsBlock);
                        informationPanelControls.SetChildIndex(ucActionsBlock, i);
                        break;
                    }
                }
                informationSidePanel.ResumeLayout();
            }
        }

        private bool IsSidePanelVisible(string code)
        {
            switch (code)
            {
                case "SCRCLIDETAILS":
                case "SCRFILMAIN":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// This method gets called when an item is added to the panels collection.
        /// </summary>
        /// <param name="sender">Control Collection Object.</param>
        /// <param name="e">Control event arguments.</param>
        private void AddPanel(object sender, ControlEventArgs e)
        {
            Control pnl = (Control)e.Control;
            pnl.Dock = DockStyle.Top;
        }

        private void HideRedundantPanel(ucPanelNav panel)
        {
            var isActionPanel = false;
            foreach (Control ctrl in panel.Controls)
            {
                if (ctrl is ucNavCommands)
                {
                    isActionPanel = true;
                    break;
                }
            }
            if (!isActionPanel)
            {
                panel.Visible = false;
            }
        }


        /// <summary>
        /// Load event of the control.
        /// </summary>
        /// <param name="sender">This control.</param>
        /// <param name="e">Empty event arguments.</param>
        private void ucOMSTypeDisplay_Load(object sender, System.EventArgs e)
        {
            Debug.WriteLine("ucOMSTypeDisplayV2 Load...");
            Debug.WriteLine("ucOMSTypeDisplayV2 set Parent Form...");
            _parentform = Global.GetParentForm(this);
            Debug.WriteLine("ucOMSTypeDisplayV2 set Parent Form Finished...");

            pnlPanelPanel.Visible = _pnlpanelpanel;
            TabControlItemFocus();
        }


        /// <summary>
        /// Captures the controls visible event so that the specified default page can be set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucOMSTypeDisplay_VisibleChanged(object sender, System.EventArgs e)
        {
            if (this.Visible)
            {
                if (_pageKey != null && !string.IsNullOrEmpty(_defaultPage))
                {
                    if (_pageKey.Contains(_defaultPage))
                    {
                        tcEnquiryPages.SelectedTab = (TabPage)_pageKey[_defaultPage].Value;

                        _defaultPage = null;
                    }
                }
            }
        }


        public void ApplyFilter(int state)
        {
            
        }
        
        private void tcEnquiryPages_Click(object sender, EventArgs e)
        {
            TabControlItemFocus();
        }
        
        /// <summary>
        /// Set text for Save and Cancel buttons from Codelookups
        /// </summary>
        private void SetResources()
        {
            if (Session.CurrentSession.IsConnected)
            {
                this.cmdCancel.Text = Session.CurrentSession.Resources.GetResource("CMDCANCEL", "Cance&l", "").Text;
                this.cmdSave.Text = Session.CurrentSession.Resources.GetResource("CMDSAVE", "&Save", "").Text;
            }
        }

        /// <summary>
        /// Set action for Click Event on Save and Cancel button
        /// </summary>
        /// <param name="action">action for Click Event</param>
        public void CmdButtonsActions(EventHandler actionSave, EventHandler actionCancel)
        {
            this.cmdCancel.Click += actionCancel;
            this.cmdSave.Click += actionSave;
        }

        /// <summary>
        /// Set visible for panel with Save and Cancel buttons
        /// </summary>
        /// <param name="isVisible">true - visible</param>
        public void ShowBottomPanel(bool isVisible)
        {
            this.panelBottom.Visible = isVisible;
        }

        internal OMSTypeDisplayInfo GetInfo()
        {
            return new OMSTypeDisplayInfo(tabcontents, _currentTab, tcEnquiryPages, advancedSecurityTabIndex);
        }

        #endregion Methods

        #region Panel Properties
        /// <summary>
        /// Gets or Sets the visibility of the panels collection.
        /// </summary>
        [Category("Information Panel Control")]
        public bool ipc_Visible
        {
            get
            {
                return pnlPanelPanel.Visible;
            }
            set
            {
                pnlPanelPanel.Visible = value;
            }
        }

        /// <summary>
        /// Gets or sets the back colour of the panels collection.
        /// </summary>
        [Category("Information Panel Control")]
        public Color ipc_BackColor
        {
            get
            {
                return pnlInfoBack.BackColor;
            }
            set
            {
                if (overridetheme)
                {
                    this.PanelActions.Theme = FWBS.Common.UI.Windows.ExtColorTheme.None;
                    pnlInfoBack.BackColor = value;
                }
            }
        }

        /// <summary>
        /// Gets or Sets the width of the panels collection.
        /// </summary>
        [Category("Information Panel Control")]
        public int ipc_Width
        {
            get
            {
                return pnlPanelPanel.Width;
            }
            set
            {
                pnlPanelPanel.Width = value;
            }
        }


        /// <summary>
        /// Gets or Sets the colour brightness of the panels collection.
        /// </summary>
        [Category("Information Panel Control")]
        [DefaultValue(105)]
        public int ipc_PanelBrightness
        {
            get
            {
                return _increasebrightness;
            }
            set
            {
                if (overridetheme)
                {
                    _increasebrightness = value;

                    foreach (ucPanelNav pnl in pnlInfoBack.Controls)
                    {
                        pnl.Brightness = _increasebrightness;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or Sets the button style on the panels collection.
        /// </summary>
        [Category("Information Panel Control")]
        [DefaultValue(NavButtonStyle.Grey)]
        public NavButtonStyle ButtonStyle
        {
            get
            {
                return _navstyle;
            }
            set
            {
                _navstyle = value;
                foreach (ucPanelNav pnl in pnlInfoBack.Controls)
                {
                    pnl.ButtonStyle = _navstyle;
                }
            }
        }

        #endregion

        #region Event Methods
        
        /// <summary>
        /// Raises the OnNewOMSTypeWindow event with the specified event arguments.
        /// </summary>
        /// <param name="sender">Add in cotnrol reference.</param>
        /// <param name="e">NewOMSTypeWindowEventArgs event arguments.</param>
        private void OnNewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
        {
            if (NewOMSTypeWindow != null)
                NewOMSTypeWindow(this, e);
        }

        private void OnDirty(object sender, EventArgs e)
        {
            if (Dirty != null)
                Dirty(this, EventArgs.Empty);
        }

        #endregion

        #region Properties

        [Browsable(false)]
        [DefaultValue(null)]
        public ISearchBuilder SearchBuilder { get; set; }

        /// <summary>
        /// The flag that the current display is Command Centre
        /// </summary>
        [Browsable(false)]
        public bool IsCommandCentre { get; set; }

        /// <summary>
        /// The flag that the current display is User
        /// </summary>
        [Browsable(false)]
        public bool IsUser { get; set; }

        /// <summary>
        /// The flag that the current display is the root display for the OMSType form
        /// </summary>
        [Browsable(false)]
        internal bool RootDisplay { get; set; }

        [Browsable(false)]
        internal TabPage CurrentTab { get; set; }

        /// <summary>
        /// Get the data set that holds the object types rendering parameters
        /// </summary>
        [Browsable(false)]
        public OMSType View
        {
            get
            {
                return view;
            }
        }

        /// <summary>
        /// Shows or Hides the Information Panel
        /// </summary>
        public bool InformationPanelVisible
        {
            get
            {
                return true;
            }
            set
            {

            }
        }

        /// <summary>
        /// Gets the OMS Type
        /// </summary>
        public FWBS.OMS.Interfaces.IOMSType Object
        {
            get
            {
                return _dialogobj;
            }
        }

        private TabAlignment tappositions = TabAlignment.Top;
        public TabAlignment TabPositions
        {
            get
            {
                return tappositions;
            }
            set
            {
                tappositions = value;
                if (Parent != null)
                    tcEnquiryPages.Alignment = tappositions;
            }
        }

        private bool alertsvisible = true;
        public bool AlertsVisible
        {
            get
            {
                return alertsvisible;
            }
            set
            {
                alertsvisible = value;
                if (Parent != null)
                {
                    if (alertsvisible)
                        pnlAlerts.Dock = DockStyle.Top;
                    else
                        pnlAlerts.Dock = DockStyle.None;
                }

            }
        }

        /// <summary>
        /// Gets or Sets the Visiblity of the Close Button on the Search Manager
        /// </summary>
        [Category("Search")]
        public bool SearchManagerCloseVisible
        {
            get
            {
                return _searchmanagerclosevisible;
            }
            set
            {
                _searchmanagerclosevisible = value;
            }
        }

        /// <summary>
        /// Gets or Sets the Visibility of the Show Elasticsearch
        /// </summary>
        public bool ElasticsearchVisible
        {
            get
            {
                return ucElasticsearch != null
                    ? ucElasticsearch.Visible
                    : false;
            }
            set
            {
                if (ucElasticsearch != null)
                {
                    if (value)
                    {
                        DisplayRequested?.Invoke(this, ViewEnum.ElasticSearch);
                    }
                    else
                    {
                        HideElasticsearchRequested?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the icon to be displayed on the parent form.
        /// </summary>
        public Icon ObjectTypeIcon
        {
            get
            {
                int icon = _dialogobj.GetOMSType().Glyph;
                if (icon == -1)
                    return _dialogobj.GetOMSType().GetAlternateGlyph();
                try
                {
                    return Images.GetEntityIcon(icon);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLineIf(Global.LogSwitch.TraceInfo, ex.Message);
                    return Images.GetEntityIcon(0);
                }
            }
        }

        /// <summary>
        /// Gets the OMS object type description which may be used as a forms caption.
        /// </summary>
        public string ObjectTypeDescription
        {
            get
            {
                string objectTypeDescription;
                try
                {
                    if (ucSearchManager1 != null && ucSearchManager1.Visible)
                        objectTypeDescription = ucSearchManager1.SearchForText;
                    else
                        objectTypeDescription = string.Format("{0} - {1}", Session.CurrentSession.Terminology.Parse(view.FormDescription, true), _dialogobj);
                }
                catch
                {
                    objectTypeDescription = string.Empty;
                }
                return objectTypeDescription.Replace(Environment.NewLine, " ");
            }
        }

        /// <summary>
        /// Gets the OMS object description which may be used as a forms caption.
        /// </summary>
        private string ObjectDescription
        {
            get
            {
                string objectDescription;
                try
                {
                    if (ucSearchManager1 != null && ucSearchManager1.Visible)
                        objectDescription = ucSearchManager1.SearchForText;
                    else if (view is CommandCentreType)
                        objectDescription = view.FormDescription;
                    else
                        objectDescription = _dialogobj.ToString();
                }
                catch
                {
                    objectDescription = string.Empty;
                }
                return objectDescription.Replace(Environment.NewLine, " ");
            }
        }

        /// <summary>
        /// Gets the caption to be displayed on the parent form and Windows taskbar.
        /// </summary>
        public string ObjectTypeCaption
        {
            get
            {
                bool succinct = false;
                switch (Session.CurrentSession.CurrentUser.SuccinctTypeDisplayFormCaption)
                {
                    case TriState.True:  succinct = true; break;
                    case TriState.False: succinct = false; break;
                    case TriState.Null:  succinct = Session.CurrentSession.SuccinctTypeDisplayFormCaption; break;
                }
                return succinct ? ObjectDescription : ObjectTypeDescription;
            }
        }

        #endregion Properties

        #region IOMSItem Implementation

        /// <summary>
        /// Updates all of the controls on the OMS type display.
        /// </summary>
        public void UpdateItem()
        {
            foreach (TabPage tab in tcEnquiryPages.TabPages)
            {
                if (tab.Tag != null)
                {
                    if (tab.Tag is IOMSItem)
                    {
                        IOMSItem itm = (IOMSItem)tab.Tag;
                        frmOMSTypeV2.savetrace.Add(String.Format("    {0} - {1}", tab.Text, itm.ToString()));
                        if (itm.IsDirty)
                        {
                            itm.UpdateItem();
                            frmOMSTypeV2.savetrace[frmOMSTypeV2.savetrace.Count - 1] += " : Updated Successful";
                        }
                        else
                        {
                            frmOMSTypeV2.savetrace[frmOMSTypeV2.savetrace.Count - 1] += " : Not Dirty";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Refreshes all of the controls on the OMS type display.
        /// </summary>
        public void RefreshItem()
        {
            RefreshItem(false);
        }

        /// <summary>
        /// Refreshes all of the controls on the OMS type display.
        /// </summary>
        public void RefreshItem(bool PanelsAlertsOnly)
        {

            if (PanelsAlertsOnly == false)
            {
                // ADDED THIS TO REFRESH THE OBJECT
                // Just in case the tab you refresh is
                // a search list which when refreshed 
                // just gets its data.
                if (_dialogobj != null) // Note it may be null if SearchManager is used
                    _dialogobj.Refresh();

                if (ucSearchManager1 != null && ucSearchManager1.Visible)
                {
                    ucSearchManager1.Search();
                }
                else
                {
                    foreach (TabPage tab in tcEnquiryPages.TabPages)
                    {
                        if (tab.Tag != null)
                        {
                            if (tab == tcEnquiryPages.SelectedTab)
                            {
                                IOMSItem itm = (IOMSItem)tab.Tag;
                                itm.ToBeRefreshed = true;
                                itm.RefreshItem();
                            }
                            else
                            {
                                IOMSItem itm = (IOMSItem)tab.Tag;
                                itm.ToBeRefreshed = true;
                            }
                        }
                    }
                }
            }

            if (_dialogobj is FWBS.OMS.Interfaces.IAlert)
            {
                FWBS.OMS.Interfaces.IAlert al = _dialogobj as FWBS.OMS.Interfaces.IAlert;
                pnlAlerts.SetAlerts(al.Alerts);
            }

            foreach (Control ctrl in tcFlowContainer.Controls)
            {
                RefreshItem(ctrl);
            }

            foreach (Control ctrl in tcMainContainer.Controls)
            {
                RefreshItem(ctrl);
            }
        }

        private void RefreshItem(Control ctrl)
        {
            if (ctrl is ucPanelNav)
            {
                var c = ctrl as ucPanelNav;
                if (c.Tag is OMSType.Panel)
                {
                    string output = string.Empty;
                    OMSType.Panel pnl = c.Tag as OMSType.Panel;

                    if (pnl.PanelType == OMSType.PanelTypes.Property)
                    {
                        try
                        {
                            if (pnl.Parameter != "")
                            {
                                output = Convert.ToString(_dialogobj.GetType().InvokeMember(pnl.Parameter,
                                    System.Reflection.BindingFlags.GetProperty, null, _dialogobj, Type.EmptyTypes));
                            }
                        }
                        catch
                        {

                        }
                    }

                    if (c.pContainer is ucNavRichText)
                    {
                        try
                        {
                            if (output.ToLower().StartsWith(@"{\\rtf1", StringComparison.OrdinalIgnoreCase)
                                || output.ToLower().StartsWith(@"{\rtf1", StringComparison.OrdinalIgnoreCase))
                            {
                                ((ucNavRichText)c.pContainer).Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(output);
                            }
                            else
                            {
                                ((ucNavRichText)c.pContainer).Text = output;
                            }
                        }
                        catch
                        {
                            ((ucNavRichText)c.pContainer).Text = output;
                        }

                        ((ucNavRichText)c.pContainer).Refresh();
                    }
                }
            }
        }

        /// <summary>
        /// Cancels all of the controls on the OMS type display.
        /// </summary>
        public void CancelItem()
        {
            foreach (TabPage tab in tcEnquiryPages.TabPages)
            {
                if (tab.Tag != null)
                {
                    if (tab.Tag is IOMSItem)
                    {
                        IOMSItem itm = (IOMSItem)tab.Tag;
                        if (itm.IsDirty)
                        {
                            itm.CancelItem();
                            itm.RefreshItem();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method that is called when the owning tab has been selected.
        /// </summary>
        public void SelectItem()
        {
        }

        public string SearchText
        {
            get;
            set;
        }

        /// <summary>
        /// Indicates whether there are any dirty controls on the OMS type display.
        /// </summary>
        public bool IsDirty
        {
            get
            {
                foreach (TabPage tab in tcEnquiryPages.TabPages)
                {
                    if (tab.Tag != null)
                    {
                        if (tab.Tag is IOMSItem)
                        {
                            IOMSItem itm = (IOMSItem)tab.Tag;
                            if (itm.IsDirty) return true;
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// To Be Refreshed when Active
        /// </summary>
        [Browsable(false)]
        public bool ToBeRefreshed
        {
            get
            {
                foreach (TabPage tab in tcEnquiryPages.TabPages)
                {
                    if (tab.Tag != null)
                    {
                        if (tab.Tag is IOMSItem)
                        {
                            IOMSItem itm = (IOMSItem)tab.Tag;
                            if (itm.ToBeRefreshed) return true;
                        }
                    }
                }
                return false;
            }
            set
            {
                foreach (TabPage tab in tcEnquiryPages.TabPages)
                {
                    if (tab.Tag != null)
                    {
                        if (tab.Tag is IOMSItem)
                        {
                            IOMSItem itm = (IOMSItem)tab.Tag;
                            itm.ToBeRefreshed = value;
                        }
                    }
                }
            }
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            if (InfoPanelClose != null)
                InfoPanelClose(this, EventArgs.Empty);
        }


        #endregion

        #region IOMSTypeDisplay

        /// <summary>
        /// Shows and or Creates the Search Manager
        /// </summary>
        public void ShowSearchManager(SearchManager style = SearchManager.ContactManager)
        {
            if (this.ucSearchManager1 == null)
            {
                this.ucSearchManager1 = new FWBS.OMS.UI.Windows.ucSearchManager();
                // 
                // ucSearchManager1
                // 
                this.ucSearchManager1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.ucSearchManager1.Heading = new FWBS.OMS.UI.Windows.ResourceLookupItem("HdSearchMan", "Search Manager", "").ToString();
                this.ucSearchManager1.Location = new System.Drawing.Point(157, 0);
                this.ucSearchManager1.Name = "ucSearchManager1";
                this.ucSearchManager1.SearchForIndex = -1;
                this.ucSearchManager1.SearchForText = "";
                this.ucSearchManager1.Visible = false;
                this.ucSearchManager1.TabIndex = 16;
                this.ucSearchManager1.Closed += new System.EventHandler(this.ucSearchManager1_Closed);
                this.ucSearchManager1.CloseVisible = _searchmanagerclosevisible;
                this.ucSearchManager1.NewOMSTypeWindow += new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
                this.Controls.Add(this.ucSearchManager1, true);
                this.ucSearchManager1.BringToFront();
                Global.RightToLeftControlConverter(this.ucSearchManager1, this.ParentForm);
            }

            try
            {
                ucSearchManager1.SearchForIndex = Convert.ToInt32(style);
            }
            catch (Exception ex)
            {
                this.Controls.Remove(ucSearchManager1);
                ucSearchManager1 = null;
                throw ex;
            }

            if (_parentform != null) _parentform.Text = this.ucSearchManager1.SearchForText;

            this.ucSearchManager1.Visible = true;
            this.ucSearchManager1.EnquiryForm.Focus();

            pnlPanelPanel.Visible = false;
            tcEnquiryPages.Visible = false;

            DisplayRequested?.Invoke(this, ViewEnum.SearchManager);
            SearchManagerVisibleChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Sets a specificly named tab page.
        /// </summary>
        /// <param name="name">The code name of the page to set.</param>
        public void SetTabPage(string name)
        {
            _defaultPage = name;
        }

        public void GotoDefaultPage()
        {
            if (_pageKey != null && !string.IsNullOrEmpty(_defaultPage))
            {
                if (_pageKey.Contains(_defaultPage))
                {
                    tcEnquiryPages.SelectedTab = (TabPage)_pageKey[_defaultPage].Value;

                    _defaultPage = null;
                }
            }
        }

        public void GotoNextTab()
        {
            try
            {
                tcEnquiryPages.SelectedIndex++;
            }
            catch { }
        }

        public void GotoPreviousTab()
        {
            try
            {
                tcEnquiryPages.SelectedIndex--;
            }
            catch { }
        }

        public IOMSItem GetTabsOMSItem(string Code)
        {
            TabPage tp = tcEnquiryPages.TabPages[Code];
            if (tp != null && tp.Controls.Count > 0)
                return tp.Controls[0] as IOMSItem;
            else
                return null;
        }

        public void GotoTab(string Code)
        {
            TabPage tp = tcEnquiryPages.TabPages[Code];
            if (tp != null) tcEnquiryPages.SelectedTab = tp;
            else
                ErrorBox.Show(ParentForm, new OMSException2("TABNOTFOUND", "Tab Page '%1%' cannot be found", null, true, Code));
        }

        /// <summary>
        /// Sets the Visibility Info Panel Close Button
        /// </summary>
        public bool InfoPanelCloseVisible
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public System.Windows.Forms.TabControl TabControl
        {
            get
            {
                return tcEnquiryPages;
            }
        }

        private void ucSearchManager1_Closed(object sender, System.EventArgs e)
        {
            HideSearchManager();
        }

        /// <summary>
        /// Displays an instance of the OMS configurable type object display.
        /// </summary>
        public void Open(FWBS.OMS.Interfaces.IOMSType obj)
        {
            Open(obj, null);
        }

        public Panel Panels
        {
            get
            {
                return pnlInfoBack;
            }
        }


        #endregion

        #region Private

        private void tcEnquiryPages_DoubleClick(object sender, EventArgs e)
        {
            FWBS.Common.ApplicationSetting debugset = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Debug", "Enabled", "false");
            bool debug = debugset.ToBoolean();

            if (Session.CurrentSession.CurrentUser.IsInRoles("ADMIN") || debug)
                MessageBox.Show(tcEnquiryPages.SelectedTab.Name);
        }

        #endregion
        
        #region SubscribeNavigationEvents

        private void SubscribeToHorizontalNavigationEvents()
        {
            horizontalNavigationPanel.SelectedItemChanged -= this.SelectedHorizontalItemChanged;
            horizontalNavigationPanel.SelectedItemChanged += this.SelectedHorizontalItemChanged;
        }
        #endregion SubscribeNavigationEvents

        #region AlignSelectedNavigationItemAsSelectedTab
        /// <summary>
        /// Any interaction with the treeViewNavigation nodes needs to align
        /// the selected tree node with that of the tabs (no longer visible).
        /// </summary>
        /// <param name="_selectedTab"></param>
        /// <returns></returns>
        private TabPage AlignSelectedNavigationItemAsSelectedTab(TabPage _selectedTab)
        {
            if (SelectedNodeIsAdvancedSecurityNode())
            {
                _selectedTab = tcEnquiryPages.TabPages[advancedSecurityTabIndex];
                tcEnquiryPages.SelectedTab = _selectedTab;
            }
            else
            {
                foreach (TabPage tab in tcEnquiryPages.TabPages)
                {
                    if (_navigationPanelSelectedItem == tab.Name)
                    {
                        _selectedTab = tab;
                        tcEnquiryPages.SelectedTab = _selectedTab;
                        break;
                    }
                }
            }

            return _selectedTab;
        }

        private TabPage AlignSelectedHorizontalItemAsSelectedTab(TabPage _selectedTab)
        {
            if (SelectedHorizontalNodeIsAdvancedSecurityNode())
            {
                _selectedTab = tcEnquiryPages.TabPages[advancedSecurityTabIndex];
                tcEnquiryPages.SelectedTab = _selectedTab;
            }
            else
            {
                foreach (TabPage tab in tcEnquiryPages.TabPages)
                {
                    if (horizontalNavigationPanel.SelectedItem == tab.Name)
                    {
                        _selectedTab = tab;
                        tcEnquiryPages.SelectedTab = _selectedTab;
                        break;
                    }
                }
            }

            return _selectedTab;
        }
        #endregion AlignSelectedNavigationItemAsSelectedTab

        #region SelectedNodeIsAdvancedSecurityNode
        private bool SelectedNodeIsAdvancedSecurityNode()
        {
            return _navigationPanelSelectedItem == ADVANCED_SECURITY_TREE_NODE_CODE;
        }

        private bool SelectedHorizontalNodeIsAdvancedSecurityNode()
        {
            return horizontalNavigationPanel.SelectedItem == ADVANCED_SECURITY_TREE_NODE_CODE;
        }
        #endregion SelectedNodeIsAdvancedSecurityNode

        #region IDisplay

        /// <summary>
        /// Gets or Sets the Visibility of the Show Search Manager
        /// </summary>
        [Category("Search")]
        public bool SearchManagerVisible
        {
            get
            {
                return (ucSearchManager1 != null);
            }
            set
            {
                if (value)
                    ShowSearchManager();
                else
                    HideSearchManager();

            }
        }

        public string SearchManagerHeading
        {
            get { return ucSearchManager1?.Heading; }
        }

        /// <summary>
        /// Hide the Search Manager
        /// </summary>
        public void HideSearchManager()
        {
            if (this.ucSearchManager1 != null)
            {
                ucSearchManager1.Dispose();
                ucSearchManager1 = null;
            }

            pnlPanelPanel.Visible = _pnlpanelpanel;
            tcEnquiryPages.Visible = true;

            SearchManagerVisibleChanged?.Invoke(this, EventArgs.Empty);

            if (_parentform != null)
            {
                _parentform.Text = this.ObjectTypeCaption;
            }
        }

        public void SetElasticsearch()
        {
            InsertUserControl(ucElasticsearch);
            ucElasticsearch.Visible = true;
        }

        public void ResetElasticsearchResults()
        {
            ucElasticsearch.ResetResults();
        }

        public void RemoveElasticSearch()
        {
            tcMainContainer.Controls.Remove(ucElasticsearch);
        }

        public void RemoveDefaultControls()
        {
            tcMainContainer.Controls.Remove(horizontalNavigationPanel);
            tcMainContainer.Controls.Remove(tcFlowContainer);
            tcMainContainer.Controls.Remove(tcEnquiryPages);
            tcMainContainer.Controls.Remove(panelBottom);
        }

        public void ShowDefaultControls()
        {
            tcMainContainer.Controls.Add(horizontalNavigationPanel, 0, 0);
            tcMainContainer.Controls.Add(tcFlowContainer, 0, 1);
            tcMainContainer.Controls.Add(tcEnquiryPages, 0, 2);
            tcMainContainer.Controls.Add(panelBottom, 0, 3);
        }

        private void InsertUserControl(UserControl control)
        {
            tcMainContainer.Controls.Add(control, 0, 0);
            tcMainContainer.SetRowSpan(control, 3);
        }

        #endregion

        internal class OMSTypeDisplayInfo
        {
            public OMSTypeDisplayInfo(Dictionary<TabPage, OMSType.Tab> tabContents, TabPage currentTab,
                TabControl enquiryPages, int advancedSecurityTabIndex)
            {
                TabContents = tabContents;
                CurrentTab = currentTab;
                EnquiryPages = enquiryPages;
                AdvancedSecurityTabIndex = advancedSecurityTabIndex;
            }
            
            public Dictionary<TabPage, OMSType.Tab> TabContents { get; }
            public TabPage CurrentTab { get; }
            public TabControl EnquiryPages { get; }
            public int AdvancedSecurityTabIndex { get; }
        }

    }
    #endregion ucOMSTypeDisplayV2
}
