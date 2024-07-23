using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.UI.UserControls;
using FWBS.OMS.UI.UserControls.Breadcrumbs;
using FWBS.OMS.UI.UserControls.InfoPanel;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// This form holds the user controls that build the OMSType object display dialog.
    /// </summary>
    internal class frmOMSTypeV2 : frmDialogV2, Interfaces.IOMSTypeWindow, Interfaces.IfrmOMSType
    {
        #region Control Fields

        private System.ComponentModel.IContainer components = null;
        private ucOMSTypeDisplayV2 ucOMSTypeDefault;
        private SearchManager _style = SearchManager.None;
        protected ucSearchTextControl titleBarSearch = null;

        #endregion

        #region Fields

        /// <summary>
        /// The Display Type that Contains the Search Window
        /// </summary>
        private ucOMSTypeDisplayV2 _omstypewithsearch = null;

        /// <summary>
        /// Holds the display order of object display controls.
        /// </summary>
        private DisplayCollection<ucOMSTypeDisplayV2> _displayCollection;

        /// <summary>
        /// Returns the Last OMS Type Display 
        /// </summary>
        private ucOMSTypeDisplayV2 lastdisplay;

        /// <summary>
        /// Returns the Command Centre OMS Type Display 
        /// </summary>
        private Interfaces.IOMSTypeDisplay CmdCentre;

        private System.Windows.Forms.ContextMenu cmenu;
        private System.Windows.Forms.MenuItem menuItem1;
        private FWBS.OMS.UI.StatusBar stBar;
        private System.Windows.Forms.ToolBarButton cmdSearch;
        private System.Windows.Forms.ToolBarButton cmdCmdCentre;
        private System.Windows.Forms.ToolBarButton sp1;
        private System.Windows.Forms.ToolBarButton Sp2;
        private System.Windows.Forms.StatusBarPanel statusBarPanel1;
        public static ArrayList savetrace = new ArrayList();
        protected internal ucNavigationPanel navigationPanel;
        private Panel pnlBreadCrumbs;
        private BreadCrumbsPanel breadCrumbsContainer;
        private Button btnRefresh;
        private readonly BreadCrumbsBuilder _breadCrumbsBuilder;
        private readonly PageManager<ucOMSTypeDisplayV2> _pageManager;
        private ucActionPanel actionPanel;
        private ucToggleButton toggleButton;
        private string _rootBreadCrumb;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Default contructor.
        /// </summary>
        private frmOMSTypeV2() : base(TitleBarStyle.Large)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();
            AdjustTitleBarSearchLayout();

            _displayCollection = new DisplayCollection<ucOMSTypeDisplayV2>();
            _breadCrumbsBuilder = new BreadCrumbsBuilder(breadCrumbsContainer);
            _breadCrumbsBuilder.onLabelClicked += BreadCrumbsBuilderClick;
            _pageManager = new PageManager<ucOMSTypeDisplayV2>(_breadCrumbsBuilder, _displayCollection);
            ucOMSTypeDefault.DisplayRequested += OnDisplayPage;
            ucOMSTypeDefault.SelectedIndexChanged += OnSelectedIndexChanged;
            ucOMSTypeDefault.HideElasticsearchRequested += OnHideElasticsearch;

            this.ucOMSTypeDefault.RootDisplay = true;
            this.ucOMSTypeDefault.CmdButtonsActions(cmdSave_Click, cmdCancel_Click);

            if (Session.CurrentSession.CurrentUser.IsInRoles("SHWINFPNL"))
            {
                this.toggleButton.Checked = Session.CurrentSession.CurrentUser.DisplayInformationPanel;
                this.actionPanel.SetActions(ucOMSTypeDefault.Panels, LogicalToDeviceUnits(ucOMSTypeDefault.ipc_Width));
                this.toggleButton.Visible = true;
                this.actionPanel.Visible = Session.CurrentSession.CurrentUser.DisplayInformationPanel;
            }

            tbRight.Visible = false;
            this.Activated += new EventHandler(frmOMSTypeV2_Activated);
            FWBS.OMS.UI.Windows.Services.OMSTypeRefresh += new EventHandler(cmdRefresh_Click);
            cmdCmdCentre.Visible = Session.CurrentSession.IsPackageInstalled("COMMANDCENTRE");
            cmdSearch.Visible = (!Session.CurrentSession.CurrentBranch.DisableSearchButton);
            Sp2.Visible = cmdCmdCentre.Visible;

            try
            {
                this.ucOMSTypeDefault.CreateAdapter(this.titleBarSearch);
            }
            catch (Exception e)
            {
                ErrorBox.Show(this, e);
            }

            SetRefreshImage();
        }

        private void AdjustTitleBarSearchLayout()
        {
            SuspendLayout();
            titleBarSearch.Anchor = AnchorStyles.None;
            titleBarSearch.Left = (this.ClientSize.Width - titleBarSearch.Width) / 2;
            titleBarSearch.Anchor = AnchorStyles.Top;
            ResumeLayout();
        }

        protected override void SetResources()
        {
            cmdBack.Text = Session.CurrentSession.Resources.GetResource("CMDBACK", "&Back", "").Text;
            cmdRefresh.Text = Session.CurrentSession.Resources.GetResource("CMDREFRESH", "&Refresh", "").Text;
            cmdSave.Text = Session.CurrentSession.Resources.GetResource("CMDSAVE", "&Save", "").Text;
            cmdCancel.Text = Session.CurrentSession.Resources.GetResource("CMDCANCEL", "Cance&l", "").Text;
            cmdOK.Text = Session.CurrentSession.Resources.GetResource("OMSTOK", "&OK", "").Text;
            this.cmdSearch.Text = Session.CurrentSession.Resources.GetResource("cmdSearch", "Se&arch", "").Text;
            this.cmdCmdCentre.Text = Session.CurrentSession.Resources.GetResource("CmdCentre", "&Command Centre", "").Text;
            this.toolTip1.SetToolTip(this.toggleButton, Session.CurrentSession.Resources.GetResource("INFPANEL", "Information Panel", "").Text);
        }

        private void frmOMSTypeV2_Activated(object sender, EventArgs e)
        {
            try
            {
                if (_displayCollection.Count > 0 && _displayCollection.LastDisplay != null && _displayCollection.LastDisplay.Object != null)
                {
                    Trace.WriteLine("Before");
                    if (Session.CurrentSession.CurrentClient != null)
                        Trace.WriteLine("CC: " + Session.CurrentSession.CurrentClient.ClientNo);
                    if (Session.CurrentSession.CurrentFile != null)
                        Trace.WriteLine("CF: " + Session.CurrentSession.CurrentFile.Client.ClientNo + "/" + Session.CurrentSession.CurrentFile.FileNo);
                    if (Session.CurrentSession.CurrentAssociate != null)
                        Trace.WriteLine("CA: " + Session.CurrentSession.CurrentAssociate.Salutation + "/" + Session.CurrentSession.CurrentAssociate.Contact.Name);
                    if (Session.CurrentSession.CurrentContact != null)
                        Trace.WriteLine("CC: " + Session.CurrentSession.CurrentContact.Name);
                    _displayCollection.LastDisplay.Object.SetCurrentSessions();
                    Trace.WriteLine("After");
                    if (Session.CurrentSession.CurrentClient != null)
                        Trace.WriteLine("CC: " + Session.CurrentSession.CurrentClient.ClientNo);
                    if (Session.CurrentSession.CurrentFile != null)
                        Trace.WriteLine("CF: " + Session.CurrentSession.CurrentFile.Client.ClientNo + "/" + Session.CurrentSession.CurrentFile.FileNo);
                    if (Session.CurrentSession.CurrentAssociate != null)
                        Trace.WriteLine("CA: " + Session.CurrentSession.CurrentAssociate.Salutation + "/" + Session.CurrentSession.CurrentAssociate.Contact.Name);
                    if (Session.CurrentSession.CurrentContact != null)
                        Trace.WriteLine("CC: " + Session.CurrentSession.CurrentContact.Name);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("frmOMSTypeV2_Activated:" + ex.Message);
            }
        }

        /// <summary>
        /// Constructs an OMS form as a search manager.
        /// </summary>
        public frmOMSTypeV2(SearchManager Style) : this()
        {
            cmdSearch.Pushed=true;
            cmdSearch.Visible=false;
            ucOMSTypeDefault.ShowBottomPanel(false);
            //added DMB 2/2/2004 
            cmdRefresh.Enabled=false;
            this.FormStorageID = Style.ToString();
            _style = Style;
            _displayCollection.Add(ucOMSTypeDefault);
            ucOMSTypeDefault.SearchManagerCloseVisible = false;
            SetNavigationButtonState();
            navigationPanel.Hide();
        }

        /// <summary>
        /// Constructs an OMS forms for a users command centre configuration.
        /// </summary>
        public frmOMSTypeV2(FWBS.OMS.User user) : this(user, user.CommandCentre)
        {
        }

        /// <summary>
        /// Constructs the dialog form with a base configurable object.
        /// </summary>
        /// <param name="obj">Configurable type object.</param>
        public frmOMSTypeV2(FWBS.OMS.Interfaces.IOMSType obj) : this(obj, null)
        {
        }

        /// <summary>
        /// Constructs the dialog form with a base configurable object.
        /// </summary>
        internal frmOMSTypeV2(FWBS.OMS.Interfaces.IOMSType obj, OMSType omst, string defaultPage = null) : this()
        {
            _rootBreadCrumb = GetFormTitle(obj, defaultPage);
            navigationPanel.Show();

            var isCommandCentre = omst is CommandCentreType; //obj is User;
            FWBS.OMS.Interfaces.IOMSType startObject = isCommandCentre
                ? obj
                : Session.CurrentSession.CurrentUser;
            OMSType startOMSType = isCommandCentre
                ? omst
                : Session.CurrentSession.CurrentUser.CommandCentre;

            try
            {
                this.FormStorageID = obj.GetType().Name;
                Cursor = Cursors.WaitCursor;
                _displayCollection.Add(ucOMSTypeDefault);
                ucOMSTypeDefault.IsCommandCentre = isCommandCentre;
                ucOMSTypeDefault.Open(startObject, startOMSType);
                var info = ucOMSTypeDefault.GetInfo();
                var currentTabName = !string.IsNullOrEmpty(defaultPage) ? defaultPage : info.CurrentTab?.Name;
                BuildNavigationPanel(startOMSType, info.EnquiryPages, info.TabContents, currentTabName, info.AdvancedSecurityTabIndex);
                Icon = ucOMSTypeDefault.ObjectTypeIcon;
                Text = ucOMSTypeDefault.ObjectTypeCaption;
                SetNavigationButtonState();
                lastdisplay = ucOMSTypeDefault;
                if (omst is FWBS.OMS.CommandCentreType)
                {
                    cmdCmdCentre.Enabled = false;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            if (!isCommandCentre)
            {
                _pageManager.ShowPage(ViewEnum.Default);
                ucOMSTypeDefault_NewOMSTypeWindow(ucOMSTypeDefault, new NewOMSTypeWindowEventArgs(obj));
            }
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
                    if (_displayCollection != null)
                    {
                        foreach (ucOMSTypeDisplayV2 i in _displayCollection)
                        {
                            UnattachDisplayEvents(i);
                            i.Dispose();
                        }
                    }
                    _displayCollection = null;
                    lastdisplay = null;
                    FWBS.OMS.UI.Windows.Services.OMSTypeRefresh -= new EventHandler(cmdRefresh_Click);
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    if (_displayCollection != null)
                    {
                        _displayCollection.Clear();
                        _displayCollection = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        private void UnattachDisplayEvents(ucOMSTypeDisplayV2 display)
        {
            if (display != null)
            {
                display.NewOMSTypeWindow -= new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.ucOMSTypeDefault_NewOMSTypeWindow);
                display.SearchManagerVisibleChanged -= new EventHandler(active_SearchManagerVisibleChanged);
                display.SelectedIndexChanged -= OnSelectedIndexChanged;
                display.HideElasticsearchRequested -= OnHideElasticsearch;
            }
        }

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.titleBarSearch = new FWBS.OMS.UI.Windows.ucSearchTextControl();
            this.ucOMSTypeDefault = new FWBS.OMS.UI.Windows.ucOMSTypeDisplayV2();
            this.cmenu = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.stBar = new FWBS.OMS.UI.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.cmdSearch = new System.Windows.Forms.ToolBarButton();
            this.cmdCmdCentre = new System.Windows.Forms.ToolBarButton();
            this.sp1 = new System.Windows.Forms.ToolBarButton();
            this.Sp2 = new System.Windows.Forms.ToolBarButton();
            this.navigationPanel = new FWBS.Common.UI.Windows.ucNavigationPanel();
            this.pnlBreadCrumbs = new System.Windows.Forms.Panel();
            this.breadCrumbsContainer = new FWBS.OMS.UI.UserControls.Breadcrumbs.BreadCrumbsPanel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.actionPanel = new FWBS.OMS.UI.UserControls.InfoPanel.ucActionPanel();
            this.toggleButton = new FWBS.OMS.UI.UserControls.ucToggleButton();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            this.pnlBreadCrumbs.SuspendLayout();
            this.SuspendLayout();
            // 
            // titleBarSearch
            // 
            this.titleBarSearch.Location = new System.Drawing.Point(200, 8);
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.Position = false;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ucFormStorage1.State = false;
            this.ucFormStorage1.DefaultPercentageHeight = 90;
            this.ucFormStorage1.DefaultPercentageWidth = 90;
            this.ucFormStorage1.Version = ((long)(3));
            // 
            // tbLeft
            // 
            this.tbLeft.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.cmdCmdCentre,
            this.cmdSearch});
            this.tbLeft.Size = new System.Drawing.Size(141, 33);
            this.tbLeft.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbDialogs_ButtonClick);
            // 
            // tbRight
            // 
            this.tbRight.Size = new System.Drawing.Size(165, 33);
            this.tbRight.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbDialogs_ButtonClick);
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.Transparent;
            this.pnlTop.Location = new System.Drawing.Point(501, 48);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(0);
            this.pnlTop.Padding = new System.Windows.Forms.Padding(0);
            this.pnlTop.Size = new System.Drawing.Size(298, 36);
            this.pnlTop.Visible = false;
            // 
            // cmdBack
            // 
            this.cmdBack.DropDownMenu = this.cmenu;
            // 
            // titleBarSearch
            // 
            this.titleBarSearch.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.titleBarSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.titleBarSearch.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.titleBarSearch.Location = new System.Drawing.Point(144, 8);
            this.titleBarSearch.Name = "titleBarSearch";
            this.titleBarSearch.Size = new System.Drawing.Size(440, 32);
            this.titleBarSearch.TabIndex = 0;
            this.titleBarSearch.TabStop = false;
            // 
            // ucOMSTypeDefault
            // 
            this.ucOMSTypeDefault.AlertsVisible = true;
            this.ucOMSTypeDefault.BackColor = System.Drawing.Color.White;
            this.ucOMSTypeDefault.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ucOMSTypeDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucOMSTypeDefault.ElasticsearchVisible = false;
            this.ucOMSTypeDefault.InfoPanelCloseVisible = true;
            this.ucOMSTypeDefault.InformationPanelVisible = true;
            this.ucOMSTypeDefault.ipc_BackColor = System.Drawing.Color.White;
            this.ucOMSTypeDefault.ipc_Visible = true;
            this.ucOMSTypeDefault.ipc_Width = 157;
            this.ucOMSTypeDefault.Location = new System.Drawing.Point(501, 132);
            this.ucOMSTypeDefault.Name = "ucOMSTypeDefault";
            this.ucOMSTypeDefault.SearchManagerCloseVisible = true;
            this.ucOMSTypeDefault.SearchManagerVisible = false;
            this.ucOMSTypeDefault.SearchText = null;
            this.ucOMSTypeDefault.Size = new System.Drawing.Size(298, 486);
            this.ucOMSTypeDefault.TabIndex = 0;
            this.ucOMSTypeDefault.TabPositions = System.Windows.Forms.TabAlignment.Top;
            this.ucOMSTypeDefault.ToBeRefreshed = false;
            this.ucOMSTypeDefault.NewOMSTypeWindow += new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.ucOMSTypeDefault_NewOMSTypeWindow);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = -1;
            this.menuItem1.Text = "";
            // 
            // stBar
            // 
            this.stBar.Location = new System.Drawing.Point(501, 618);
            this.stBar.Name = "stBar";
            this.stBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1});
            this.stBar.ShowPanels = true;
            this.stBar.Size = new System.Drawing.Size(298, 22);
            this.stBar.TabIndex = 10;
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanel1.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Width = 10;
            // 
            // cmdSearch
            // 
            this.cmdSearch.ImageIndex = 10;
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            // 
            // cmdCmdCentre
            // 
            this.cmdCmdCentre.ImageIndex = 27;
            this.cmdCmdCentre.Name = "cmdCmdCentre";
            // 
            // sp1
            // 
            this.sp1.Name = "sp1";
            this.sp1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // Sp2
            // 
            this.Sp2.Name = "Sp2";
            this.Sp2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // navigationPanel
            // 
            this.navigationPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.navigationPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.navigationPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.navigationPanel.Location = new System.Drawing.Point(1, 48);
            this.navigationPanel.Name = "navigationPanel";
            this.navigationPanel.Size = new System.Drawing.Size(300, 592);
            this.navigationPanel.TabIndex = 29;
            this.navigationPanel.Visible = true;
            // 
            // pnlBreadCrumbs
            // 
            this.pnlBreadCrumbs.Controls.Add(this.breadCrumbsContainer);
            this.pnlBreadCrumbs.Controls.Add(this.toggleButton);
            this.pnlBreadCrumbs.Controls.Add(this.btnRefresh);
            this.pnlBreadCrumbs.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBreadCrumbs.Location = new System.Drawing.Point(501, 84);
            this.pnlBreadCrumbs.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBreadCrumbs.Name = "pnlBreadCrumbs";
            this.pnlBreadCrumbs.Size = new System.Drawing.Size(298, 48);
            this.pnlBreadCrumbs.TabIndex = 30;
            // 
            // breadCrumbsContainer
            // 
            this.breadCrumbsContainer.AutoScroll = true;
            this.breadCrumbsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.breadCrumbsContainer.Location = new System.Drawing.Point(0, 0);
            this.breadCrumbsContainer.Margin = new System.Windows.Forms.Padding(0);
            this.breadCrumbsContainer.Name = "breadCrumbsContainer";
            this.breadCrumbsContainer.Padding = new System.Windows.Forms.Padding(0, 12, 0, 0);
            this.breadCrumbsContainer.Size = new System.Drawing.Size(132, 48);
            this.breadCrumbsContainer.TabIndex = 0;
            this.breadCrumbsContainer.WrapContents = false;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(250, 0);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(48, 48);
            this.btnRefresh.TabIndex = 14;
            this.btnRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // actionPanel
            // 
            this.actionPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.actionPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.actionPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.actionPanel.Location = new System.Drawing.Point(301, 48);
            this.actionPanel.Margin = new System.Windows.Forms.Padding(0);
            this.actionPanel.Name = "actionPanel";
            this.actionPanel.Size = new System.Drawing.Size(200, 592);
            this.actionPanel.TabIndex = 31;
            this.actionPanel.Visible = false;
            // 
            // toggleButton
            //
            this.toggleButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.toggleButton.Location = new System.Drawing.Point(132, 0);
            this.toggleButton.Name = "toggleButton";
            this.toggleButton.Size = new System.Drawing.Size(60, 48);
            this.toggleButton.TabIndex = 15;
            this.toggleButton.Visible = false;
            this.toggleButton.CheckedChanged += ToggleButtonCheckedChanged;
            // 
            // frmOMSTypeV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(800, 641);
            this.Controls.Add(this.titleBarSearch);
            this.Controls.Add(this.ucOMSTypeDefault);
            this.Controls.Add(this.pnlBreadCrumbs);
            this.Controls.Add(this.stBar);
            this.Controls.Add(this.actionPanel);
            this.Controls.Add(this.navigationPanel);
            this.Name = "frmOMSTypeV2";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmOMSTypeV2_Closing);
            this.Load += new System.EventHandler(this.frmOMSTypeV2_Load);
            this.Controls.SetChildIndex(this.navigationPanel, 0);
            this.Controls.SetChildIndex(this.actionPanel, 0);
            this.Controls.SetChildIndex(this.stBar, 0);
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.pnlBreadCrumbs, 0);
            this.Controls.SetChildIndex(this.ucOMSTypeDefault, 0);
            this.Controls.SetChildIndex(this.titleBarSearch, 0);
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            this.pnlBreadCrumbs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        #endregion

        #region Properties
        
        [Browsable(false)]
        public TabPage CurrentTab
        {
            get { return _displayCollection.LastDisplay?.CurrentTab; }
            set
            {
                if (_displayCollection.LastDisplay != null)
                {
                    _displayCollection.LastDisplay.CurrentTab = value;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Captures any new dialog window request from the child user controls.
        /// </summary>
        /// <param name="sender">Display control instance.</param>
        /// <param name="e">NewOMSTypeWindowEventArgs event arguments.</param>
        private void ucOMSTypeDefault_NewOMSTypeWindow(object sender, FWBS.OMS.UI.Windows.NewOMSTypeWindowEventArgs e)
        {
            ucOMSTypeDisplayV2 active = _displayCollection.LastDisplay;
            active.Enabled = false;
            stBar.Panels[0].Text = "";

            MenuItem mnui = new MenuItem();
            mnui.Text = active.ObjectTypeDescription;
            this.cmenu.MenuItems.Add(_displayCollection.Count - 1, mnui);
            mnui.Click += new System.EventHandler(this.pnlSelect_Click);

            FWBS.OMS.UI.Windows.ucOMSTypeDisplayV2 display = new FWBS.OMS.UI.Windows.ucOMSTypeDisplayV2()
            {
                Location = ucOMSTypeDefault.Location,
                Size = ucOMSTypeDefault.Size,
                IsUser = e.OMSType is UserType
            };

            display.SuspendLayout();
            display.SetSelectedItem(navigationPanel.SelectedItem);
            display.DisplayRequested += OnDisplayPage;
            display.SelectedIndexChanged += OnSelectedIndexChanged;
            display.HideElasticsearchRequested += OnHideElasticsearch;
            display.CmdButtonsActions(cmdSave_Click, cmdCancel_Click);
            display.SearchManagerVisibleChanged +=new EventHandler(active_SearchManagerVisibleChanged);
            display.NewOMSTypeWindow += new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.ucOMSTypeDefault_NewOMSTypeWindow);

            //Set the default properties of the new display item and make it visible.
            display.ipc_BackColor = System.Drawing.Color.White;
            display.ipc_Width = 157;

            this.Controls.Add(display);
            display.RootDisplay = _displayCollection.Count == 0;
            display.Open(e.OMSObject, e.OMSType);
            display.BringToFront();
            display.Dock = DockStyle.Fill;
            display.ResumeLayout();
            actionPanel.SetActions(display.Panels, LogicalToDeviceUnits(display.ipc_Width));

            Icon = display.ObjectTypeIcon;
            Text = display.ObjectTypeCaption;

            _displayCollection.Add(display);

            try
            {
                display.CreateAdapter(this.titleBarSearch);
            }
            catch (Exception exception)
            {
                ErrorBox.Show(this, exception);
            }

            Global.RightToLeftControlConverter(display, this);
            display.ipc_Visible = true;
            SetNavigationButtonState();

            display.Visible=true;
            active.Visible = false;

            if (_displayCollection.LastDisplay == _omstypewithsearch && cmdSearch.Pushed)
            {
                cmdSearch.Enabled=false;
            }

            ucOMSTypeDefault.ShowBottomPanel(true);
            //added DMB 2/2/2004
            cmdRefresh.Enabled=true;
            lastdisplay = display;
            if (e.OMSType is FWBS.OMS.CommandCentreType)
            {
                CmdCentre = lastdisplay;
                cmdCmdCentre.Enabled = false;
            }
            display.GotoDefaultPage();
        }

        /// <summary>
        /// Disables the navigational buttons if there is only one object display control available.
        /// </summary>
        private void SetNavigationButtonState()
        {
            if (_displayCollection.Count > 1 || (cmdSearch.Pushed && cmdSearch.Visible))
                cmdBack.Enabled = true;
            else
                cmdBack.Enabled = false;
        }

        /// <summary>
        /// Disposes of the active display control.
        /// </summary>
        /// <param name="sender">Back button.</param>
        /// <param name="e">Empty event arguments.</param>
        protected void cmdBack_Click(object sender, System.EventArgs e)
        {
            if (cmdBack.Enabled)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;

                    Back();
                }
                catch(Exception ex)
                {
                    ErrorBox.Show(this, ex);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// Saves all of the current display controls and their content and closes the form.
        /// </summary>
        /// <param name="sender">OK Button.</param>
        /// <param name="e">Empty event arguments.</param>
        protected void cmdOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                for (int ctr = _displayCollection.Count - 1; ctr >= 0; ctr--)
                {
                    ucOMSTypeDisplayV2 display = _displayCollection[ctr];
                    display.UpdateItem();
                }
                this.DialogResult = DialogResult.OK;

                if (ucFormStorage1 != null)
                {
                    ucFormStorage1.SaveNow();
                }

                this.Close();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }


        /// <summary>
        /// Saves all the open display control and their contents and keeps the form open.
        /// This is similar to an apply button.
        /// </summary>
        /// <param name="sender">Save button.</param>
        /// <param name="e">Empty event arguments.</param>
        protected void cmdSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                Save();
            }
            catch (Exception ex)
            {
                // DMB 24/02/2004 added a check to see if this was called by pressing the button or called
                // from another function
                if(sender is frmOMSTypeV2)
                    throw ex;
                else
                    ErrorBox.Show(this, ex, frmOMSTypeV2.savetrace);

            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        /// <summary>
        /// Refreshes the whole dialog form.
        /// </summary>
        /// <param name="sender">Refresh button.</param>
        /// <param name="e">Empty event arguments.</param>
        protected void cmdRefresh_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                using (var locker = new WindowUpdateLocker(this))
                    Refresh();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }


        /// <summary>
        /// Cancels any changes for the whole of the dialog form.
        /// </summary>
        /// <param name="sender">Cancel button.</param>
        /// <param name="e">Empty event arguments.</param>
        protected void cmdCancel_Click(object sender, System.EventArgs e)
        {
            Cancel();
        }

        /// <summary>
        /// Captures all the toolbar buttons clicks.
        /// </summary>
        /// <param name="sender">Toolbar button.</param>
        /// <param name="e">Tool bar button event arguments.</param>
        private void tbDialogs_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            try
            {
                pnlTop.Focus();
                if (e.Button == cmdSearch)
                {
                    ShowSearch();
                }
                if (e.Button == cmdRefresh) cmdRefresh_Click(sender,e);
                if (e.Button == cmdBack) cmdBack_Click(sender,e);
                if (e.Button == cmdOK) cmdOK_Click(sender, e);
                if (e.Button == cmdSave) cmdSave_Click(sender, e);
                if (e.Button == cmdCancel) cmdCancel_Click(sender, e);
                if (e.Button == cmdCmdCentre)
                {
                    ShowCommandCentre();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this,ex);
            }
        }


        /// <summary>
        /// Back Button Drop Menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pnlSelect_Click(object sender, System.EventArgs e)
        {
            int b = cmenu.MenuItems.Count - (((MenuItem)sender).Index );
            for (int a = 0;a < b;a++)
            {
                this.cmdBack_Click(sender,e);
                Application.DoEvents();
            }

        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            SetRefreshImage();
        }

        private void SetRefreshImage()
        {
            this.btnRefresh.Image = Images.GetCommonIcon(DeviceDpi, "refresh");
        }

        private void BuildNavigationPanel(OMSType omsType, TabControl enquiryPages, Dictionary<TabPage, OMSType.Tab> tabContents, string currentTab, int advancedSecurityTabIndex)
        {
            if (enquiryPages.TabCount > 0)
            {
                var factory = new NavigationPanelFactory(navigationPanel);
                var builder = factory.CreateBuilder(tabContents, enquiryPages, advancedSecurityTabIndex, omsType);
                builder.Build(currentTab);

                navigationPanel.InitState(Session.CurrentSession.CurrentUser.ExpandNavigationPanel);
                navigationPanel.Visible = true;

                SubscribeToNavigationEvents();
            }
        }

        private void SubscribeToNavigationEvents()
        {
            navigationPanel.SelectedItemChanged -= OnSelectItemChanged;
            navigationPanel.SelectedItemChanged += OnSelectItemChanged;

            navigationPanel.NavigationPanelStateChanged -= OnNavigationPanelStateChanged;
            navigationPanel.NavigationPanelStateChanged += OnNavigationPanelStateChanged;
        }

        private string GetFormTitle(FWBS.OMS.Interfaces.IOMSType obj, string defaultPage)
        {
            if (string.IsNullOrWhiteSpace(defaultPage))
            {
                if (obj is FWBS.OMS.OMSFile)
                {
                    return Session.CurrentSession.Terminology.Parse(CodeLookup.GetLookup("CBCCAPTIONS", "FILEINFO", "Matter Information"), false);
                }

                if (obj is FWBS.OMS.Client)
                {
                    return Session.CurrentSession.Terminology.Parse(CodeLookup.GetLookup("CBCCAPTIONS", "CLIENTINFO", "Client Information"), false);
                }
            }
            else
            {
                if (obj is FWBS.OMS.OMSFile && defaultPage == "SCRFILMSTAD")
                {
                    return CodeLookup.GetLookup("CBCCAPTIONS", "MILESTONE", "Milestone Information");
                }

                if (obj is FWBS.OMS.User && defaultPage == "USER")
                {
                    return CodeLookup.GetLookup("OMS", "USERINFO", "User Info");
                }
            }

            return CodeLookup.GetLookup("CBCCAPTIONS", "COMMANDCENTRE", "Command Centre");
        }

        #endregion

        #region Private Events
        private void frmOMSTypeV2_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            quickCancel.Select(); // it is necessary to lose focus from the last edited control to don't lose this changes on save
            Services.BackForwardMouse.BackButtonClicked -= new EventHandler(BackForwardMouse_BackButtonClicked);
            bool isCancelling = false;
            
            if (ucFormStorage1 != null)
            {
                ucFormStorage1.SaveNow();
            }

            try
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    Cursor = Cursors.WaitCursor;

                    for (int ctr = _displayCollection.Count - 1; ctr >= 0; ctr--)
                    {
                        ucOMSTypeDisplayV2 display = _displayCollection[ctr];

                        //Check for dirty data before cancelling.
                        if (display.IsDirty)
                        {
                            DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAPRM", "Changes have been detected to %1%, would you like to save?", "", display.ObjectTypeDescription), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                            switch (res)
                            {
                                case DialogResult.Yes:
                                    // DMB 24/02/2004 
                                    // Added a try catch around this to catch save errors and stop form closing
                                    try
                                    {
                                        cmdSave_Click(this, EventArgs.Empty);
                                        cmdBack_Click(this, EventArgs.Empty);
                                    }
                                    catch (Exception exe)
                                    {
                                        ErrorBox.Show(this, exe);
                                        isCancelling = true;
                                        e.Cancel = true;
                                    }
                                    break;
                                case DialogResult.No:
                                    display.CancelItem();
                                    cmdBack_Click(this, EventArgs.Empty);
                                    break;
                                case DialogResult.Cancel:
                                    {
                                        isCancelling = true;
                                        e.Cancel = true;
                                        return;
                                    }
                            }
                        }
                    }
                    if(!isCancelling)
                        this.Hide();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
            finally
            {
                Cursor = Cursors.Default;

                if (!isCancelling)
                {

                    this.Dispose();
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                }
            }
        }

        private void BackForwardMouse_BackButtonClicked(object sender, EventArgs e)
        {
            if (cmdBack.Enabled)
                cmdBack_Click(this,e);
        }

        private void frmOMSTypeV2_Load(object sender, System.EventArgs e)
        {
            Services.BackForwardMouse.BackButtonClicked +=new EventHandler(BackForwardMouse_BackButtonClicked);
            ucOMSTypeDefault.SearchManagerVisibleChanged += new EventHandler(active_SearchManagerVisibleChanged);
            if (_style != SearchManager.None)
            {
                this.toggleButton.Visible = false;
                this.actionPanel.Visible = false;
                ucOMSTypeDefault.ShowSearchManager(_style);
            }
            ucOMSTypeDefault.TabControlFocus();
        }

        private void active_SearchManagerVisibleChanged(object sender, EventArgs e)
        {
            cmdSearch.Pushed = ((ucOMSTypeDisplayV2)sender).SearchManagerVisible || ((ucOMSTypeDisplayV2)sender).ElasticsearchVisible;
            ucOMSTypeDefault.ShowBottomPanel(!cmdSearch.Pushed);
            cmdRefresh.Enabled = !cmdSearch.Pushed;
        }

        private void OnSelectedIndexChanged(object sender, TabPage tabPage)
        {
            if (_breadCrumbsBuilder != null && (CurrentTab != tabPage ||
                                                _breadCrumbsBuilder.LastBreadCrumb.ViewType == ViewEnum.ElasticSearch ||
                                                _breadCrumbsBuilder.LastBreadCrumb.IsRootItem))
            {
                CurrentTab = tabPage;
                var breadCrumbItem = BreadCrumbItem.CreateBreadCrumbItem((ucOMSTypeDisplayV2)sender, tabPage, ViewEnum.Default);
                _breadCrumbsBuilder.Build(breadCrumbItem);
            }
        }

        private void OnHideElasticsearch(object sender, EventArgs e)
        {
            _pageManager.HideElasticSearch();
            _breadCrumbsBuilder.RemoveSearchItem();
        }

        private void OnDisplayPage(object sender, ViewEnum e)
        {
            var title = e == ViewEnum.StartPoint
                ? _rootBreadCrumb
                : null;

            actionPanel.ActionsVisible = (e == ViewEnum.StartPoint || e == ViewEnum.Default);
            _pageManager.ShowPage(e, title);
        }

        private void ToggleButtonCheckedChanged(Object sender, EventArgs e)
        {
            actionPanel.Visible = toggleButton.Checked;
            Session.CurrentSession.CurrentUser.DisplayInformationPanel = toggleButton.Checked;
        }

        #endregion

        #region IOMSTypeWindow Members

        /// <summary>
        /// Sets a specificly named tab page.
        /// </summary>
        /// <param name="name">The code name of the page to set.</param>
        public void SetTabPage(string name)
        {
            ucOMSTypeDisplayV2 active = _displayCollection.LastDisplay;
            active.SetTabPage(name);
        }

        public void GotoTab(string Code)
        {
            ucOMSTypeDisplayV2 active = _displayCollection.LastDisplay;
            active.GotoTab(Code);
        }

        public IOMSItem GetTabsOMSItem(string Code)
        {
            ucOMSTypeDisplayV2 active = _displayCollection.LastDisplay;
            return active.GetTabsOMSItem(Code);
        }

        public void Back()
        {
            if (!cmdBack.Enabled || _displayCollection == null)
            {
                return;
            }

            try
            {
                Cursor = Cursors.WaitCursor;
                using (var locker = new WindowUpdateLocker(this))
                    BackInternal(locker, true);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void OnSelectItemChanged(object sender, CancelEventArgs e)
        {
            if (_displayCollection.LastDisplay.ElasticsearchVisible)
            {
                _pageManager.HideElasticSearch();
            }

            using (var locker = new WindowUpdateLocker(this))
            {
                if (!CheckChanges(_displayCollection.LastDisplay, locker))
                {
                    return;
                }

                foreach (ucOMSTypeDisplayV2 display in _displayCollection)
                {
                    display.SetSelectedItem(navigationPanel.SelectedItem);
                }

                if (_displayCollection.LastDisplay.View is FWBS.OMS.CommandCentreType)
                {
                    _displayCollection.LastDisplay.SelectPage(sender, e);
                }
                else
                {
                    ucOMSTypeDefault_NewOMSTypeWindow(this,
                        new NewOMSTypeWindowEventArgs(_displayCollection.FirstDisplay.Object, _displayCollection.FirstDisplay.View, string.Empty));
                    _displayCollection.LastDisplay.SelectPage(sender, e);
                }
            }
        }

        private void OnNavigationPanelStateChanged(object sender, NavigationPanelStateChangedEventArgs e)
        {
            Session.CurrentSession.CurrentUser.ExpandNavigationPanel = e.Expand;
        }

        private bool CheckChanges(ucOMSTypeDisplayV2 active, WindowUpdateLocker locker)
        {
            if (active.Object != null)
            {
                if (active.IsDirty)
                {
                    locker.Unlock();
                    DialogResult res = MessageBox.Show(
                        Session.CurrentSession.Resources.GetMessage("DIRTYDATAPRM",
                            "Changes have been detected to %1%, would you like to save?", "", active.ObjectTypeDescription),
                        FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);
                    locker.Lock();
                    active.ActiveControl = null;
                    try
                    {
                        switch (res)
                        {
                            case DialogResult.Yes:
                                active.UpdateItem();
                                break;
                            case DialogResult.No:
                                active.CancelItem();
                                break;
                            case DialogResult.Cancel:
                                return false;
                        }
                    }
                    catch (Exception e)
                    {
                        ErrorBox.Show(this, e);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// <param name="locker">Window update locker.</param>
        /// <param name="resetSessions">True to reset active session objects in accordance with the new type display object, otherwise False.</param>
        /// </summary>
        /// <returns>True if back is executed</returns>
        private bool BackInternal(WindowUpdateLocker locker, bool resetSessions)
        {
            ucOMSTypeDisplayV2 active = _displayCollection.LastDisplay;
            //added DMB 10/2/2004 to allow back button to work when search screen is shown
            if (cmdSearch.Pushed && cmdSearch.Visible && (active.SearchManagerVisible || active.ElasticsearchVisible))
            {
                cmdSearch.Pushed = false;
                ShowSearch();
                return true;
            }

            cmdSearch.Pushed = false;
            if (!CheckChanges(active, locker))
            {
                return false;
            }

            if (this.cmenu.MenuItems.Count > 0)
                this.cmenu.MenuItems.RemoveAt(this.cmenu.MenuItems.Count - 1);

            _displayCollection.Remove(active);
            _displayCollection.LastDisplay.Visible = true;
            var viewType = _breadCrumbsBuilder.RemoveLastItem();
            _pageManager.SetViewType(viewType);
            cmdBack.Enabled = false;
            // If the Active Display is the Command Centre then on Back Enabled Button
            if (active == CmdCentre) cmdCmdCentre.Enabled = true;

            UnattachDisplayEvents(active);
            active.Parent.Controls.Remove(active);
            active.Dispose();

            active = _displayCollection.LastDisplay;
            active.Enabled = true;
            if (active == _omstypewithsearch)
            {
                if (ucOMSTypeDefault.Object != null) cmdSearch.Enabled = true;
                cmdSearch.Pushed = Convert.ToBoolean(cmdSearch.Tag);
            }
            actionPanel.SetActions(active.Panels, LogicalToDeviceUnits(active.ipc_Width));

            Text = active.ObjectTypeCaption;
            if (active.Object != null)
            {
                Icon = active.ObjectTypeIcon;
                if (resetSessions)
                    active.Object.SetCurrentSessions();
            }

            cmdSearch.Pushed = active.SearchManagerVisible || active.ElasticsearchVisible;
            ucOMSTypeDefault.ShowBottomPanel(!cmdSearch.Pushed);
            active.Focus();

            SetNavigationButtonState();
            stBar.Panels[0].Text = "";

            return true;
        }

        public void Save()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                savetrace.Clear();
                for (int ctr = _displayCollection.Count - 1; ctr >= 0; ctr--)
                {
                    ucOMSTypeDisplayV2 display = _displayCollection[ctr];
                    if (display.Object != null)
                    {
                        savetrace.Add(display.ObjectTypeDescription);
                        display.UpdateItem();
                        display.RefreshItem(true);
                    }
                }

                //UTCFIX: DM - 30/11/06 - No fix required local time displayed as it should.
                stBar.Panels[0].Text = Session.CurrentSession.Resources.GetResource("INFOSAVEDTIME", "Last Saved at %1% ...","", DateTime.Now.ToLongTimeString()).Text;
            }
            finally
            {
                Cursor = Cursors.Default;
            }

        }

        new public void Refresh()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                for (int ctr = _displayCollection.Count - 1; ctr >= 0; ctr--)
                {
                    ucOMSTypeDisplayV2 display = _displayCollection[ctr];

                    //Check for dirty data before refreshing the data.
                    if (display.IsDirty)
                    {
                        DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAPRM", "Changes have been detected to %1%, would you like to save?","",display.ObjectTypeDescription), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                        switch (res)
                        {
                            case DialogResult.Yes:
                                cmdSave_Click(this, EventArgs.Empty);
                                break;
                            case DialogResult.No:
                                display.CancelItem();
                                break;
                            case DialogResult.Cancel:
                                return;
                        }
                    }
                    display.RefreshItem();
                }
            }
            finally
            {
                base.Refresh();
                Cursor = Cursors.Default;
            }
        }

        public void Cancel()
        {
            this.Close();
        }

        public void ShowCommandCentre()
        {
            NewOMSTypeWindowEventArgs r = new NewOMSTypeWindowEventArgs(Session.CurrentSession.CurrentUser, Session.CurrentSession.CurrentUser.CommandCentre, "");
            ucOMSTypeDefault_NewOMSTypeWindow(null,r);
        }

        public void ShowSearch()
        {
            try
            {
                if (Session.CurrentSession.IsSearchConfigured)
                {
                    if (Session.CurrentSession.SearchButtonUseSearchManager && cmdSearch.Pushed)
                    {
                        _displayCollection.LastDisplay.SearchManagerVisible = true;
                    }
                    else if (Session.CurrentSession.SearchButtonUseSearchManager == false && cmdSearch.Pushed)
                    {
                        _displayCollection.LastDisplay.ElasticsearchVisible = true;
                    }
                    else
                    {
                        if (_displayCollection.LastDisplay.SearchManagerVisible)
                            _displayCollection.LastDisplay.SearchManagerVisible = false;
                        if (_displayCollection.LastDisplay.ElasticsearchVisible)
                            _displayCollection.LastDisplay.ElasticsearchVisible = false;
                    }
                }
                else
                {
                    _displayCollection.LastDisplay.SearchManagerVisible = cmdSearch.Pushed;
                }

                if (cmdSearch.Pushed)
                {
                    _omstypewithsearch = _displayCollection.LastDisplay;
                    if (_omstypewithsearch.ElasticsearchVisible == false)
                        _omstypewithsearch.RefreshSearchManager();
                    else
                        _omstypewithsearch.RefreshElasticsearch();
                }
                else
                {
                    _omstypewithsearch = null;
                }
                ucOMSTypeDefault.ShowBottomPanel(!cmdSearch.Pushed);
            }
            catch (Exception ex)
            {
                cmdSearch.Pushed = false;
                throw ex;
            }

            //added DMB 10/2/2004
            SetNavigationButtonState();
        }
        #endregion

        #region IBreadCrumbsOwner
        private void BreadCrumbsBuilderClick(BreadCrumbItem breadCrumb)
        {
            if (breadCrumb.IsLastItem)
            {
                return;
            }

            CollectSearchGarbage();
            using (var locker = new WindowUpdateLocker(this))
            {
                while (_displayCollection.Count > 0)
                {
                    if (_displayCollection.LastDisplay == breadCrumb.Display)
                    {
                        SetTabPage(breadCrumb.Page.Name);
                        navigationPanel.SelectItem(breadCrumb.Page.Name);
                        actionPanel.ActionsVisible = true;
                        break;
                    }

                    if (_displayCollection.Count == 1)
                    {
                        if (breadCrumb.ViewType == ViewEnum.SearchManager)
                        {
                            ucOMSTypeDefault.ShowSearchManager();
                        }
                        else
                        {
                            _pageManager.ShowPage(ViewEnum.Default);
                        }
                        break;
                    }

                    var backResult = BackInternal(locker, false);
                    if (!backResult)
                    {
                        break;
                    }
                }
            }
            frmOMSTypeV2_Activated(this, EventArgs.Empty);
        }

        private void CollectSearchGarbage()
        {
            if (_breadCrumbsBuilder.ContainsSearchBreadCrumbItem)
            {
                var lastDisplay = _displayCollection.LastDisplay;
                if (lastDisplay != null)
                {
                    _pageManager.HideElasticSearch();
                    _breadCrumbsBuilder.RemoveSearchItem();
                }
            }
        }

        #endregion IBreadCrumbsOwner
    }
}

