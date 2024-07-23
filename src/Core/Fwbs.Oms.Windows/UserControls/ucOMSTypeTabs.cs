using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using FWBS.OMS.UI.Windows.Interfaces;


namespace FWBS.OMS.UI.Windows
{
    public class ucOMSTypeTabs : System.Windows.Forms.UserControl, IOpenOMSType, IOMSItem, IOMSTypeDisplay, FWBS.Common.UI.IBasicEnquiryControl2
    {
        #region Events
        /// <summary>
        /// An event that gets raised when a new OMS type object needs to be opened in
        /// a navigational format on the dialog form.
        /// </summary>
        public event NewOMSTypeWindowEventHandler NewOMSTypeWindow = null;
        /// <summary>
        /// When the Control is reported Dirty
        /// </summary>
        public event EventHandler Dirty = null;

        #endregion

        #region Controls
        private TabControl tcEnquiryPages;
        #endregion

        #region Fields
        private bool overridetheme = false;
        /// <summary>
        /// A Dictionary for when the Tab is Selected for the First Time
        /// </summary>
        private Dictionary<TabPage, Tab> tabcontents;
        /// <summary>
        /// The default tab page to display on load up.
        /// </summary>
        private string _defaultPage = null;
        /// <summary>
        /// First tab recognition.
        /// </summary>
        private bool _first = false;
        /// <summary>
        /// Stores the current tab.
        /// </summary>
        private TabPage _currentTab = null;
        /// <summary>
        /// Temporary panels to add and remove on each item select / deselect.
        /// </summary>
        private ucPanelNav[] _tempPanels = null;
        /// <summary>
        /// Current Brightness of the panels collection..
        /// </summary>
        private int _increasebrightness = 105;
        /// <summary>
        /// A key paor collection that will holds a reference to a specific
        /// tab page using a unique code.  This will be used so that a specific
        /// page can be jumped to by a key name.
        /// </summary>
        private Common.KeyValueCollection _pageKey = null;
        /// <summary>
        /// The Base IOMSType used to pass to all Tab Pages
        /// </summary>
        private FWBS.OMS.Interfaces.IOMSType iobject;
        /// <summary>
        /// The Parent EnquiryForm
        /// </summary>
        private EnquiryForm parentform;

        private TabsCollection _tabs;

        private string xml;

        private int _itemHeight = 20;

        private bool _tabImagesVisible = true;

        private Point _tabPadding = new Point(6, 3);

        #endregion

        #region External Properties that will need to be assigned before Execution
        private Panel pnlInfoBack;

        private ucPanelNav PanelActions
        {
            get;
            set;
        }

        private ucNavCommands ucNavCommands1
        {
            get;
            set;
        }
        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Create a new instance of this control.
        /// </summary>
        public ucOMSTypeTabs()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            iomstypename = "FWBS.OMS.Client";
            _tabs = new TabsCollection();
        }

        [Browsable(true)]
        [DefaultValue(20)]
        public int ItemHeight
        {
            get { return _itemHeight; }
            set
            {
                if (_itemHeight != value)
                {
                    _itemHeight = value;
                    TabControl.ItemSize = new Size(TabControl.ItemSize.Width, LogicalToDeviceUnits(_itemHeight));
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        public bool TabImagesVisible
        {
            get { return _tabImagesVisible; }
            set
            {
                if (_tabImagesVisible != value)
                {
                    _tabImagesVisible = value;

                    if (_tabImagesVisible && !DesignMode)
                    {
                        tcEnquiryPages.ImageList = Images.GetCoolButtonsList((Images.IconSize)LogicalToDeviceUnits(16));
                    }
                    else
                    {
                        if (tcEnquiryPages.ImageList != null)
                        {
                            tcEnquiryPages.ImageList.Dispose();
                            tcEnquiryPages.ImageList = null;
                        }
                    }
                }
            }
        }

        [Browsable(true)]
        public Point TabPadding
        {
            get { return _tabPadding; }
            set
            {
                if (_tabPadding != value)
                {
                    _tabPadding = value;
                    TabControl.Padding = new Point(LogicalToDeviceUnits(_tabPadding.X), LogicalToDeviceUnits(_tabPadding.Y));
                }
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (tcEnquiryPages.ImageList != null)
                {
                    tcEnquiryPages.ImageList.Dispose();
                    tcEnquiryPages.ImageList = null;
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tcEnquiryPages = new FWBS.OMS.UI.TabControl();
            this.SuspendLayout();
            // 
            // tcEnquiryPages
            // 
            this.tcEnquiryPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcEnquiryPages.Location = new System.Drawing.Point(0, 0);
            this.tcEnquiryPages.Multiline = true;
            this.tcEnquiryPages.Name = "tcEnquiryPages";
            this.tcEnquiryPages.SelectedIndex = 0;
            this.tcEnquiryPages.Size = new System.Drawing.Size(741, 488);
            this.tcEnquiryPages.TabIndex = 0;
            this.tcEnquiryPages.Selected += new System.Windows.Forms.TabControlEventHandler(this.tcEnquiryPages_Selected);
            this.tcEnquiryPages.Click += new System.EventHandler(this.tcEnquiryPages_Click);
            this.tcEnquiryPages.DoubleClick += new System.EventHandler(this.tcEnquiryPages_DoubleClick);
            // 
            // ucOMSTypeTabs
            // 
            this.Controls.Add(this.tcEnquiryPages);
            this.DoubleBuffered = true;
            this.Name = "ucOMSTypeTabs";
            this.Size = new System.Drawing.Size(741, 488);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        #region Methods
        public void ActivateCurrentTab()
        {
            TabPage _selectedTab = tcEnquiryPages.SelectedTab;
            IOpenOMSType Iomstype = null;
            if (_selectedTab == null)
                return;

            try
            {
                if (_currentTab != _selectedTab)
                {
                    RemoveTempPanels();
                }

                //Loop through each of the tabs and add them to the tab page collection and also initiate a control type
                //to be put on them.

                if (_selectedTab.ToolTipText == " ")
                {
                    InitialiseTabContent(_selectedTab, ref Iomstype);
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
                                etoolbar.ShowPanelButtons();
                                PanelActions.Visible = (ucNavCommands1.Controls.Count > 0);
                                PanelActions.BringToFront();
                                break;
                            }
                        }
                    }
                    else if (_selectedTab.Tag is ucSearchControl)
                    {
                        ucSearchControl sl = _selectedTab.Tag as ucSearchControl;
                        sl.ShowPanelButtons();
                        PanelActions.Visible = (ucNavCommands1.Controls.Count > 0);
                        PanelActions.BringToFront();
                    }
                    else if (_selectedTab.Tag is IOMSTypeAddin)
                    {
                        IOMSTypeAddin addin = _selectedTab.Tag as IOMSTypeAddin;
                        _tempPanels = addin.Panels;
                        if (_tempPanels != null)
                        {
                            foreach (ucPanelNav panel in _tempPanels)
                            {
                                AddPanel(panel);
                            }
                        }
                    }
                    //TabControlItemFocus();
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
            }
        }

        private void TabControlItemFocus()
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

        /// <summary>
        /// Displays an instance of the OMS configurable type object display.
        /// </summary>
        /// <param name="obj">OMS Configurable type type object.</param>
        private void Connect()
        {
            tabcontents = new Dictionary<TabPage, Tab>();
            Parent.Cursor = Cursors.WaitCursor;
            _tempPanels = null;
            _first = false;
            tcEnquiryPages.TabPages.Clear();
            try
            {
                //Create a new key pair tab collection.
                _pageKey = new Common.KeyValueCollection();

                Debug.WriteLine("ucOMSTypeTabs Begin Tab Construction");

                //Loop through each of the tabs and add them to the tab page collection and also initiate a control type
                //to be put on them.
                var font = new Font("Segoe UI", 9);
                foreach (Tab tab in _tabs)
                {
                    if (omsDesignMode)
                    {
                        TabPage tp = new TabPage(Session.CurrentSession.Terminology.Parse(tab.LocalizedCode.Description, true))
                            {
                                Font = font
                            };
                        tp.ControlAdded += new ControlEventHandler(tp_ControlAdded);
                        tp.BackColor = System.Drawing.SystemColors.Window;
                        tp.Name = tab.Source;
                        //Get the icon to display on the tab.
                        tp.ImageIndex = tab.Glyph;
                        tcEnquiryPages.TabPages.Add(tp);
                    }
                    else
                    {
                        tab.OMSObjectType = iobject.GetType();
                        if (tab.UserRoles == "" || (tab.UserRoles != "" && Session.CurrentSession.CurrentUser.IsInRoles(tab.UserRoles)))
                            if (Session.CurrentSession.ValidateConditional(iobject, tab.Conditional) && String.IsNullOrEmpty(tab.Source) == false)
                            {
                                Debug.WriteLine("ucOMSTypeTabs Construct Tab :" + tab.LocalizedCode.Description + " (" + tab.Code + ")");
                                //Create a tab page based on the description returned.
                                TabPage tp = new TabPage(Session.CurrentSession.Terminology.Parse(tab.LocalizedCode.Description, true))
                                    {
                                        Font = font
                                    };
                                tabcontents[tp] = tab;
                                tp.ControlAdded += new ControlEventHandler(tp_ControlAdded);
                                tp.BackColor = System.Drawing.SystemColors.Window;
                                tp.Name = tab.Source;
                                tp.ToolTipText = " ";
                                bool construct = true;

                                if (tab.SourceParam == "")
                                {
                                    if (OmsObject.Exists(tab.Source) == false)
                                    {
                                        ucErrorBox err = new ucErrorBox();
                                        err.SetErrorBox(new OMSException2("38004", "OMS Object '%1%' does not exist.", new Exception(), false, tab.Source));
                                        err.Location = new Point(10, 20);
                                        tp.Controls.Add(err);
                                        construct = false;
                                    }
                                    else
                                    {
                                    }
                                }
                                else
                                {
                                }

                                //DM - 18/08/05 - Construct and Initialise all of the addins.
                                if (tab.SourceType == OMSObjectTypes.Addin && construct)
                                {
                                    Debug.WriteLine("ucOMSTypeTabs If Addin :" + tab.LocalizedCode.Description + " (" + tab.Code + ")");
                                    try
                                    {
                                        OmsObject omsobj = new OmsObject(tab.Source);
                                        Type type = Global.CreateType(omsobj.Windows, omsobj.Assembly);

                                        IOMSTypeAddin addin = (IOMSTypeAddin)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
                                        Control addinctrl = addin.UIElement ?? (Control)addin;

                                        tp.Tag = addin;
                                        addinctrl.Visible = false;
                                        tp.Controls.Add(addinctrl);

                                        Debug.WriteLine("ucOMSTypeTabs Addin Initialise :" + tab.LocalizedCode.Description + " (" + tab.Code + ")");
                                        addin.Initialise(iobject);
                                        Debug.WriteLine("ucOMSTypeTabs Addin Initialise Complete :" + tab.LocalizedCode.Description + " (" + tab.Code + ")");

                                        //Ad global panels, these should rarely be used.
                                        ucPanelNav[] global_panels = addin.GlobalPanels;

                                        if (global_panels != null)
                                        {
                                            foreach (ucPanelNav panel in global_panels)
                                            {
                                                if (String.IsNullOrEmpty(panel.Name) == false)
                                                {
                                                    if (pnlInfoBack.Controls.ContainsKey(panel.Name))
                                                        continue;
                                                }
                                                AddPanel(panel);
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
                                tcEnquiryPages.TabPages.Add(tp);

                                //Add the source code and the tab reference to the page collection so that
                                //the page can be later set by a key name.
                                string src = tab.Source;
                                if (_pageKey.Contains(src) == false)
                                    _pageKey.Add(src, tp);
                                //if no default is set, set the default - this should fefault to the first page
                                if (_defaultPage == null)
                                    _defaultPage = src;

                                //Set the first tab.
                                if (_first == false)
                                {
                                    Debug.WriteLine("ucOMSTypeTabs Before DoEvents:" + tab.LocalizedCode.Description + " (" + tab.Code + ")");
                                    Application.DoEvents();
                                    Debug.WriteLine("ucOMSTypeTabs After DoEvents:" + tab.LocalizedCode.Description + " (" + tab.Code + ")");
                                    _first = true;
                                }
                            }
                    }
                }
                if (omsDesignMode) return;
                Debug.WriteLine("ucOMSTypeTabs End Tab Construction");
                PanelActions.BringToFront();
                RefreshItem(true);
            }
            finally
            {
                Parent.Cursor = Cursors.Default;
            }
        }

        private void RemoveTempPanelsOnSelectedTabChanged(object sender, EventArgs args)
        {
            RemoveTempPanels();
        }

        private void RemoveTempPanels()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                ucNavCommands1.Controls.Clear();
                PanelActions.Visible = false;

                if (_tempPanels != null)
                {
                    foreach (ucPanelNav panel in _tempPanels)
                    {
                        pnlInfoBack.Controls.Remove(panel);
                    }
                }

                if (_currentTab != null && _currentTab.Tag is ucSearchControl)
                {
                    ucSearchControl ctrl = (ucSearchControl)_currentTab.Tag;
                    ctrl.HaltSearch();
                }

                _currentTab = tcEnquiryPages.SelectedTab;
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
        }


        private Interfaces.IOMSTypeDisplay currentdisplay;

        /// <summary>
        /// Gets the Current Display windows.
        /// </summary>
        private Interfaces.IOMSTypeDisplay CurrentDisplay
        {
            get
            {
                if (currentdisplay == null)
                {
                    if (parentform == null) return null;
                    Control parent = parentform.Parent;
                    while (parent != null)
                    {
                        if (parent is Interfaces.IOMSTypeDisplay)
                        {
                            currentdisplay = (Interfaces.IOMSTypeDisplay)parent;
                            return currentdisplay;
                        }
                        else
                            parent = parent.Parent;
                    }

                    currentdisplay = parent as Interfaces.IOMSTypeDisplay;
                    return currentdisplay;
                }
                else
                    return currentdisplay;
            }
        }

        private void InitialiseTabContent(TabPage _selectedTab, ref IOpenOMSType Iomstype)
        {
            tcEnquiryPages.Enabled = false;
            Tab tab = tabcontents[_selectedTab];
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
                        ctrl.Enquiry = iobject.Edit(tab.Source, null);
                        ctrl.Dirty += new EventHandler(OnDirty);
                        ctrl.Dock = DockStyle.Fill;

                        foreach (Control c in ctrl.Controls)
                        {
                            eToolbars etoolbar = c as eToolbars;
                            if (etoolbar != null)
                            {
                                etoolbar.NavCommandPanel = ucNavCommands1;
                                etoolbar.ShowPanelButtons();
                                PanelActions.Visible = (ucNavCommands1.Controls.Count > 0);
                                break;
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
                            if (addin.Connect(iobject))
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
                            _tempPanels = addin.Panels;

                            if (_tempPanels != null)
                            {
                                foreach (ucPanelNav panel in _tempPanels)
                                {
                                    AddPanel(panel);
                                }
                            }

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
                            param.Add("ID", iobject.LinkValue);
                            sl.NavCommandPanel = ucNavCommands1;
                            sl.Dock = DockStyle.Fill;
                            _selectedTab.Controls.Add(sl, true);

                            sl.SetSearchList(tab.Source, true, iobject, param);
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
                            if (iobject is FWBS.OMS.Interfaces.IExtendedDataCompatible)
                            {
                                FWBS.OMS.Interfaces.IExtendedDataCompatible ext = (FWBS.OMS.Interfaces.IExtendedDataCompatible)iobject;
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

        private void tcEnquiryPages_DoubleClick(object sender, EventArgs e)
        {
            FWBS.Common.ApplicationSetting debugset = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Debug", "Enabled", "false");
            bool debug = debugset.ToBoolean();

            if (Session.CurrentSession.CurrentUser.IsInRoles("ADMIN") || debug)
                MessageBox.Show(tcEnquiryPages.SelectedTab.Name);
        }

        private void SetTabsOMSObjectType()
        {
            string typename = this.IOMSTypeName;
            if (typename.ToLowerInvariant() == "fwbs.oms.omsfile")
                typename = "FWBS.OMS.OMSFile";

            if (!typename.Contains(",")) typename += ", OMS.Library";
            Type tt = Session.CurrentSession.TypeManager.TryLoad(typename);

            foreach (Tab t in _tabs)
                t.OMSObjectType = tt;
        }
        #endregion

        #region Event Methods
        private void tcEnquiryPages_Selected(object sender, TabControlEventArgs e)
        {
            ActivateCurrentTab();
        }

        /// <summary>
        /// Add an item the panels collection.
        /// </summary>
        /// <param name="pnl">Panel</param>
        private void AddPanel(ucPanelNav pnl)
        {
            pnl.ModernStyle = ucPanelNav.NavStyle.ModernHeader;
            pnl.SetBackColor(Color.FromArgb(244, 244, 244));
            pnlInfoBack.Controls.Add(pnl);
            pnl.BringToFront();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Debug.WriteLine("ucOMSTypeTabs Load...");
            if (!DesignMode)
            {
                if (_tabImagesVisible)
                {
                    tcEnquiryPages.ImageList = Images.GetCoolButtonsList((Images.IconSize)LogicalToDeviceUnits(16));
                }
                if (omsDesignMode)
                    Connect();
            }
        }

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            base.OnDpiChangedBeforeParent(e);
            if (tcEnquiryPages.ImageList != null)
            {
                tcEnquiryPages.ImageList = Images.GetCoolButtonsList((Images.IconSize)LogicalToDeviceUnits(16));
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible)
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

        private void tp_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.BackColor = System.Drawing.SystemColors.Window;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                parentform = Parent as EnquiryForm;
                if (parentform != null)
                {
                    parentform.Updated += new EventHandler(parentform_Updated);
                    if (parentform.Enquiry != null)
                        parentform.Enquiry.Refreshed += new EventHandler(Enquiry_Refreshed);
                    if (CurrentDisplay != null)
                    {
                        this.pnlInfoBack = CurrentDisplay.Panels;
                        CurrentDisplay.TabControl.SelectedIndexChanged += new EventHandler(RemoveTempPanelsOnSelectedTabChanged);
                        this.PanelActions = GetPanelActions();
                        this.ucNavCommands1 = PanelActions.Controls[0] as ucNavCommands;
                        iobject = GetOMSType();
                        Connect();
                    }
                }
            }
            else
            {
                if (parentform != null)
                {
                    if (parentform.Enquiry != null)
                        parentform.Enquiry.Refreshed -= new EventHandler(Enquiry_Refreshed);
                    parentform.Updated -= new EventHandler(parentform_Updated);
                    parentform = null;
                    if (CurrentDisplay != null)
                    {
                        CurrentDisplay.TabControl.SelectedIndexChanged -= new EventHandler(RemoveTempPanelsOnSelectedTabChanged);
                    }
                }
            }
            tcEnquiryPages.Alignment = tappositions;
        }


        private ucPanelNav GetPanelActions()
        {
            return ((ucOMSTypeDisplayV2)CurrentDisplay).PanelActions;
        }


        private FWBS.OMS.Interfaces.IOMSType GetOMSType()
        {
            return ((ucOMSTypeDisplayV2)CurrentDisplay).Object as FWBS.OMS.Interfaces.IOMSType;
        }


        void Enquiry_Refreshed(object sender, EventArgs e)
        {
            this.RefreshItem();
        }


        private void parentform_Updated(object sender, EventArgs e)
        {
            this.UpdateItem();
        }

        private void tcEnquiryPages_Click(object sender, EventArgs e)
        {
            TabControlItemFocus();
        }

        /// <summary>
        /// Raises the OnNewOMSTypeWindow event with the specified event arguments.
        /// </summary>
        /// <param name="sender">Add in cotnrol reference.</param>
        /// <param name="e">NewOMSTypeWindowEventArgs event arguments.</param>
        private void OnNewOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
        {
            if (NewOMSTypeWindow != null)
                NewOMSTypeWindow(this, e);
            else if (parentform != null)
                parentform.OnNewOMSTypeWindow(e);
        }

        private void OnDirty(object sender, EventArgs e)
        {
            if (Dirty != null)
                Dirty(this, EventArgs.Empty);
            OnActiveChanged();
            OnChanged();
        }
        #endregion

        #region Properties

        private string iomstypename;
        [Editor("FWBS.OMS.Design.DataListEditor,OMS.UI", typeof(System.Drawing.Design.UITypeEditor))]
        [FWBS.OMS.Design.DataList("DSCONFIGTYPES", NullValue = "", UseNull = false, DisplayMember = "EnumItem1", ValueMember = "EnumItem")]
        public string IOMSTypeName
        {
            get
            {
                return iomstypename;
            }
            set
            {
                if (iomstypename != value)
                {
                    if (omsDesignMode)
                    {
                        if (_tabs.Count > 0)
                        {
                            if (MessageBox.ShowYesNoQuestion("Are you sure you wish to Change the IOMSTypeName this will clear your current Tabs") == DialogResult.Yes)
                            {
                                _tabs.Clear();
                                TabsXML = "";
                                tcEnquiryPages.TabPages.Clear();
                            }
                            else
                                return;
                        }
                    }
                    iomstypename = value;

                    SetTabsOMSObjectType();
                }
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

        [System.ComponentModel.Editor(typeof(TabEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string TabsXML
        {
            get
            {
                return xml;
            }
            set
            {
                if (xml != value)
                {
                    xml = value;
                    if (String.IsNullOrEmpty(xml) == false)
                    {
                        try
                        {
                            Desearlise();
                        }
                        catch (Exception ex)
                        {
                            Tab.LogEvent(ex);
                            xml = "";
                        }
                    }
                }
            }
        }


        private void Desearlise()
        {
            XmlSerializer s = new XmlSerializer(typeof(TabsCollection));
            StringReader w = new StringReader(xml);
            _tabs = s.Deserialize(w) as TabsCollection;

            SetTabsOMSObjectType();
        }

        #endregion

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
                        frmOMSType.savetrace.Add(String.Format("    {0} - {1}", tab.Text, itm.ToString()));
                        if (itm.IsDirty)
                        {
                            itm.UpdateItem();
                            frmOMSType.savetrace[frmOMSType.savetrace.Count - 1] += " : Updated Successful";
                        }
                        else
                        {
                            frmOMSType.savetrace[frmOMSType.savetrace.Count - 1] += " : Not Dirty";
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
            // ADDED THIS TO REFRESH THE OBJECT
            // Just in case the tab you refresh is
            // a search list which when refreshed 
            // just gets its data.
            if (iobject != null) // Note it may be null if SearchManager is used
                iobject.Refresh();

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
                        itm.CancelItem();
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

        private bool isdirty = false;

        /// <summary>
        /// Indicates whether there are any dirty controls on the OMS type display.
        /// </summary>
        [Browsable(false)]
        public bool IsDirty
        {
            get
            {
                if (isdirty) return true;
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
            set
            {
                isdirty = value;
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

        #endregion

        #region IOMSTypeDisplay

        /// <summary>
        /// Sets a specificly named tab page.
        /// </summary>
        /// <param name="name">The code name of the page to set.</param>
        public void SetTabPage(string name)
        {
            _defaultPage = name;
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


        [Browsable(false)]
        public System.Windows.Forms.TabControl TabControl
        {
            get
            {
                return tcEnquiryPages;
            }
        }

        /// <summary>
        /// Displays an instance of the OMS configurable type object display.
        /// </summary>
        public void Open(FWBS.OMS.Interfaces.IOMSType obj)
        {
            this.iobject = obj;
            Connect();
        }

        [Browsable(false)]
        public Panel Panels
        {
            get
            {
                return pnlInfoBack;
            }
        }


        #endregion

        #region IOMSTypeDisplay Members

        public event EventHandler SearchManagerVisibleChanged;

        public event EventHandler InfoPanelClose;

        public void ShowSearchManager(SearchManager Style)
        {
            throw new NotImplementedException();
        }

        public void HideSearchManager()
        {
            throw new NotImplementedException();
        }

        #endregion
        
        #region Tab Class
        public class Tab : LookupTypeDescriptor
        {
            private CodeLookupDisplay _code = null;
            private CodeLookupDisplayReadOnly _source = null;
            private CodeLookupDisplayMulti _usrroles = null;

            public override string ToString()
            {
                return LocalizedCode.Description;
            }

            public Tab()
            {
                Code = "";
                Source = "";
                this.UserRoles = "";
                this.Conditional = new string[0];
            }
            
            public Tab(Type omstype) : this()
            {
                this.OMSObjectType = omstype;
            }

            [System.ComponentModel.Browsable(false)]
            public string Code
            {
                get;
                set;
            }


            [System.ComponentModel.Browsable(false)]
            public string Default
            {
                get;
                set;
            }

            private OMSObjectTypes tabtype;
            [XmlIgnore()]
            public OMSObjectTypes SourceType
            {
                get
                {
                    return tabtype;
                }
            }

            [Lookup("Description")]
            [LocCategory("DATA")]
            [CodeLookupSelectorTitle("TABCAP", "Tab Captions")]
            [XmlIgnore()]
            public CodeLookupDisplay LocalizedCode
            {
                get
                {
                    if (_code == null)
                    {
                        _code = new CodeLookupDisplay("DLGTABCAPTION");
                        _code.Code = this.Code;
                        if (String.IsNullOrEmpty(_code.Description) && !String.IsNullOrEmpty(this.Default))
                        {
                            _code = new CodeLookupDisplay(this.Code, "DLGTABCAPTION", this.Default, CodeLookup.DefaultCulture, "");
                            CodeLookup.Create("DLGTABCAPTION", this.Code, this.Default, "", CodeLookup.DefaultCulture, true,true,true);
                        }
                        if (String.IsNullOrEmpty(this.Default) && _code != null && !String.IsNullOrEmpty(_code.Description))
                            this.Default = _code.Description;
                    }
                    return _code;
                }
                set
                {
                    if (_code != value)
                    {
                        _code = value;
                        this.Default = _code.Description;
                        this.Code = value.Code;
                    }
                }
            }

            [Parameter(CodeLookupDisplaySettings.omsObjects)]
            [Lookup("OMSObject")]
            [LocCategory("DESIGN")]
            [System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
            [CodeLookupSelectorTitle("OMSOBJECTS", "OMS Objects")]
            [XmlIgnore()]
            public CodeLookupDisplayReadOnly LocalizedSource
            {
                get
                {
                    if (_source == null)
                    {
                        _source = new CodeLookupDisplayReadOnly("OMSOBJECT");
                        _source.Code = this.Source;
                    }
                    return _source;
                }
                set
                {
                    _source = value;
                    this.Source = value.Code;
                }
            }

            [Lookup("OMSOBJCODE")]
            [LocCategory("DESIGN")]
            [XmlIgnore()]
            public string OMSObjectCode
            {
                get
                {
                    return this.source;
                }
            }

            [LocCategory("TABVISIBLE")]
            public string[] Conditional
            {
                get;
                set;
            }

            [Lookup("USERROLES")]
            [LocCategory("TABVISIBLE")]
            [System.ComponentModel.RefreshProperties(System.ComponentModel.RefreshProperties.All)]
            [CodeLookupSelectorTitle("USERROLES", "User Roles")]
            [XmlIgnore()]
            public CodeLookupDisplayMulti UserRolesDisplay
            {
                get
                {
                    if (_usrroles == null)
                    {
                        _usrroles = new CodeLookupDisplayMulti("USRROLES");
                        _usrroles.Codes = this.UserRoles;
                    }
                    return _usrroles;
                }
                set
                {
                    _usrroles = value;
                    this.UserRoles = value.Codes;
                }
            }

            [Browsable(false)]
            public string UserRoles
            {
                get;
                set;
            }

            private string source;
            [System.ComponentModel.Browsable(false)]
            public string Source
            {
                get
                {
                    return source;
                }
                set
                {
                    source = value;
                    if (String.IsNullOrEmpty(source) == false)
                    {
                        try
                        {
                            OmsObject obj = new OmsObject(value);
                            tabtype = obj.ObjectType;
                        }
                        catch (Exception ex)
                        {
                            LogEvent(ex);
                        }
                    }
                }
            }

            internal static void LogEvent(Exception ex)
            {
                try
                {
                    EventLog evt = new System.Diagnostics.EventLog();
                    // Create the source, if it does not already exist.
                    if (!EventLog.SourceExists("OMSDOTNET"))
                        EventLog.CreateEventSource("OMSDOTNET", "Application");

                    // Create an EventLog instance and assign its source.
                    EventLog myLog = new EventLog();
                    myLog.Source = "OMSDOTNET";
                    myLog.WriteEntry("TabsInTabs Error: " + ex.Message + Environment.NewLine + "-----------------------------------------------------------------------------------------------" + ex.StackTrace, EventLogEntryType.Error);
                }
                catch { }
            }

            [System.ComponentModel.Browsable(false)]
            public string SourceParam
            {
                get;
                set;
            }

            [System.ComponentModel.Editor("FWBS.OMS.UI.Windows.Design.TabGlyphDisplayEditor,omsadmin", typeof(System.Drawing.Design.UITypeEditor))]
            [System.ComponentModel.TypeConverter("FWBS.OMS.UI.Windows.Design.CoolButtonLister,omsadmin")]
            [LocCategory("DESIGN")]
            public int Glyph
            {
                get;
                set;
            }

            [System.ComponentModel.Browsable(false)]
            [XmlIgnore()]
            public Type OMSObjectType
            {
                get;
                set;
            }
        }
       
        public class TabEditor : System.ComponentModel.Design.CollectionEditor
        {
            public TabEditor()
                : base(typeof(ArrayList))
            {
            }

            protected override System.Type CreateCollectionItemType()
            {
                return typeof(Tab);
            }

            protected override object CreateInstance(System.Type t)
            {
                ucOMSTypeTabs ct = (ucOMSTypeTabs)this.Context.Instance;
                string typename = ct.IOMSTypeName;
                if (typename.ToLowerInvariant() == "fwbs.oms.omsfile")
                    typename = "FWBS.OMS.OMSFile";

                if (!typename.Contains(",")) typename += ", OMS.Library";
                Type tt = Session.CurrentSession.TypeManager.TryLoad(typename);
                if (tt == null)
                {
                    MessageBox.ShowInformation(String.Format("The Type '{0}' cannot be found. ", ct.IOMSTypeName));
                    return null;
                }

                return new Tab(tt);
            }

            protected override Type[] CreateNewItemTypes()
            {
                return new Type[] { typeof(Tab) };
            }

            protected override object SetItems(object editValue, object[] value)
            {
                ucOMSTypeTabs ct = (ucOMSTypeTabs)this.Context.Instance;
                ct._tabs.Clear();
                foreach (Tab t in value)
                    ct._tabs.Add(t);
                ct.Connect();
                return editValue;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                ucOMSTypeTabs ct = (ucOMSTypeTabs)context.Instance;
                if (ct.IOMSTypeName == null)
                {
                    MessageBox.ShowInformation("IOMSTypeName has not been set ...");
                    return null;
                } 
                
                base.EditValue(context, provider, ct._tabs);
                XmlSerializer s = new XmlSerializer(typeof(TabsCollection));
                StringWriter w = new StringWriter();
                s.Serialize(w, ct._tabs);
                return w.ToString().Replace(Environment.NewLine, "").Replace(">    <", "><").Replace(">  <", "><");
            }

        }

        [Serializable()]
        public class TabsCollection : Crownwood.Magic.Collections.CollectionWithEvents
        {
            public TabsCollection() { }


            public Tab Add(Tab value)
            {
                // Use base class to process actual collection operation
                base.List.Add(value as object);
                return value;
            }

            public void AddRange(Tab[] values)
            {
                // Use existing method to add each array entry
                foreach (Tab page in values)
                    Add(page);
            }

            public void Remove(Tab value)
            {
                // Use base class to process actual collection operation
                base.List.Remove(value as object);
            }

            public void Insert(int index, Tab value)
            {
                // Use base class to process actual collection operation
                base.List.Insert(index, value as object);
            }

            public bool Contains(Tab value)
            {
                // Use base class to process actual collection operation
                return base.List.Contains(value as object);
            }

            public Tab this[int index]
            {
                // Use base class to process actual collection operation
                get { return (base.List[index] as Tab); }
            }

            public int IndexOf(Tab value)
            {
                // Find the 0 based index of the requested entry
                return base.List.IndexOf(value);
            }
        }
        #endregion


        #region IBasicEnquiryControl2 Members

        public event EventHandler ActiveChanged;

        public event EventHandler Changed;

        [Browsable(false)]
        public object Control
        {
            get { return this; }
        }

        [Browsable(false)]
        public int CaptionWidth
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        [Browsable(false)]
        public bool CaptionTop
        {
            get
            {
                return false;
            }
            set { }
        }

        [Browsable(false)]
        public bool LockHeight
        {
            get { return false; }
        }

        [Browsable(false)]
        public bool Required
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [Browsable(false)]
        public bool ReadOnly
        {
            get
            {
                return !this.Enabled;
            }
            set
            {
                this.Enabled = !value;
            }
        }

        [Browsable(false)]
        public bool omsDesignMode
        {
            get;
            set;
        }

        [Browsable(false)]
        public object Value
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        public void OnChanged()
        {
            if (Changed != null)
                Changed(this, EventArgs.Empty);
        }

        public void OnActiveChanged()
        {
            if (ActiveChanged != null)
                ActiveChanged(this, EventArgs.Empty);
        }

        #endregion
    }
}
