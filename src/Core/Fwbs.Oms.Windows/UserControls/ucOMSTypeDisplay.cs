using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Security.Permissions;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// 38000 A common user controlthat will display OMS Configurable types.  This form exposes
    /// tabs and side panels of information containing potential enquiry forms and complex
    /// user controls.
    /// </summary>
    public class ucOMSTypeDisplay : System.Windows.Forms.UserControl, Interfaces.IOMSTypeDisplay, IOpenOMSType, IOMSItem, Interfaces.IEmbeddedOMSTypeDisplay
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

        #endregion

        #region Controls

        private TabControl tcEnquiryPages;
        private System.Windows.Forms.Panel pnlInfoBack;
        private omsSplitter splitter1;
        private System.ComponentModel.IContainer components;
        private ucAlert pnlAlerts;
        private bool overridetheme = false;
        private Dictionary<TabPage, OMSType.Tab> tabcontents;

        #endregion

        #region Fields
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
        private ucPanelNav[] _tempPanels = null;
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
        private System.Windows.Forms.Panel pnlHeading;
        private System.Windows.Forms.Button btnClose;
        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;

        /// <summary>
        /// The default tab page to display on load up.
        /// </summary>
        private string _defaultPage = null;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Create a new instance of this control.
        /// </summary>
        public ucOMSTypeDisplay()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            this.SuspendLayout();
            pnlInfoBack.ControlAdded += new ControlEventHandler(this.AddPanel);
            SetOfficeStyle();
            this.ResumeLayout();
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                this.btnClose.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);

            base.OnRightToLeftChanged(e);
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
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlInfoBack = new System.Windows.Forms.Panel();
            this.PanelActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavCommands1 = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.pnlBackMain = new System.Windows.Forms.Panel();
            this.tcEnquiryPages = new FWBS.OMS.UI.TabControl();
            this.pnlPanelPanel = new System.Windows.Forms.Panel();
            this.pnlHeading = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.splitter1 = new FWBS.OMS.UI.Windows.omsSplitter();
            this.pnlAlerts = new FWBS.OMS.UI.Windows.ucAlert();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlInfoBack.SuspendLayout();
            this.PanelActions.SuspendLayout();
            this.pnlBackMain.SuspendLayout();
            this.pnlPanelPanel.SuspendLayout();
            this.pnlHeading.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlInfoBack
            // 
            this.pnlInfoBack.AutoScroll = true;
            this.pnlInfoBack.BackColor = System.Drawing.Color.White;
            this.pnlInfoBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlInfoBack.Controls.Add(this.PanelActions);
            this.pnlInfoBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlInfoBack.Location = new System.Drawing.Point(0, 23);
            this.pnlInfoBack.Name = "pnlInfoBack";
            this.pnlInfoBack.Padding = new System.Windows.Forms.Padding(8);
            this.pnlInfoBack.Size = new System.Drawing.Size(175, 455);
            this.pnlInfoBack.TabIndex = 13;
            // 
            // PanelActions
            // 
            this.PanelActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.PanelActions.Controls.Add(this.ucNavCommands1);
            this.PanelActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelActions.ExpandedHeight = 31;
            this.PanelActions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.PanelActions.HeaderColor = System.Drawing.Color.Empty;
            this.PanelActions.Location = new System.Drawing.Point(8, 8);
            this.resourceLookup1.SetLookup(this.PanelActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("PanelActions", "Actions", ""));
            this.PanelActions.Name = "PanelActions";
            this.PanelActions.Size = new System.Drawing.Size(157, 31);
            this.PanelActions.TabIndex = 0;
            this.PanelActions.TabStop = false;
            this.PanelActions.Text = "Actions";
            // 
            // ucNavCommands1
            // 
            this.ucNavCommands1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavCommands1.Location = new System.Drawing.Point(0, 24);
            this.ucNavCommands1.Name = "ucNavCommands1";
            this.ucNavCommands1.Padding = new System.Windows.Forms.Padding(5);
            this.ucNavCommands1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavCommands1.Size = new System.Drawing.Size(157, 0);
            this.ucNavCommands1.TabIndex = 15;
            this.ucNavCommands1.TabStop = false;
            // 
            // pnlBackMain
            // 
            this.pnlBackMain.BackColor = System.Drawing.Color.White;
            this.pnlBackMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBackMain.Controls.Add(this.tcEnquiryPages);
            this.pnlBackMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBackMain.Location = new System.Drawing.Point(185, 5);
            this.pnlBackMain.Name = "pnlBackMain";
            this.pnlBackMain.Padding = new System.Windows.Forms.Padding(4);
            this.pnlBackMain.Size = new System.Drawing.Size(551, 478);
            this.pnlBackMain.TabIndex = 18;
            // 
            // tcEnquiryPages
            // 
            this.tcEnquiryPages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcEnquiryPages.Location = new System.Drawing.Point(4, 4);
            this.tcEnquiryPages.Multiline = true;
            this.tcEnquiryPages.Name = "tcEnquiryPages";
            this.tcEnquiryPages.SelectedIndex = 0;
            this.tcEnquiryPages.Size = new System.Drawing.Size(541, 468);
            this.tcEnquiryPages.TabIndex = 0;
            this.tcEnquiryPages.Click += new System.EventHandler(this.tcEnquiryPages_Click);
            this.tcEnquiryPages.DoubleClick += new System.EventHandler(this.tcEnquiryPages_DoubleClick);
            // 
            // pnlPanelPanel
            // 
            this.pnlPanelPanel.Controls.Add(this.pnlInfoBack);
            this.pnlPanelPanel.Controls.Add(this.pnlHeading);
            this.pnlPanelPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlPanelPanel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlPanelPanel.Location = new System.Drawing.Point(5, 5);
            this.pnlPanelPanel.Name = "pnlPanelPanel";
            this.pnlPanelPanel.Size = new System.Drawing.Size(175, 478);
            this.pnlPanelPanel.TabIndex = 1;
            this.pnlPanelPanel.VisibleChanged += new System.EventHandler(this.pnlPanelPanel_VisibleChanged);
            // 
            // pnlHeading
            // 
            this.pnlHeading.BackColor = System.Drawing.Color.White;
            this.pnlHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlHeading.Controls.Add(this.btnClose);
            this.pnlHeading.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeading.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(66)))), ((int)(((byte)(139)))));
            this.pnlHeading.Location = new System.Drawing.Point(0, 0);
            this.pnlHeading.Name = "pnlHeading";
            this.pnlHeading.Padding = new System.Windows.Forms.Padding(3);
            this.pnlHeading.Size = new System.Drawing.Size(175, 23);
            this.pnlHeading.TabIndex = 14;
            this.pnlHeading.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHeading_Paint);
            this.pnlHeading.Resize += new System.EventHandler(this.pnlHeading_Resize);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnClose.Location = new System.Drawing.Point(152, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(18, 15);
            this.btnClose.TabIndex = 4;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(180, 5);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 478);
            this.splitter1.TabIndex = 15;
            this.splitter1.TabStop = false;
            // 
            // pnlAlerts
            // 
            this.pnlAlerts.BackColor = System.Drawing.Color.Transparent;
            this.pnlAlerts.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAlerts.Location = new System.Drawing.Point(5, 5);
            this.pnlAlerts.Name = "pnlAlerts";
            this.pnlAlerts.Size = new System.Drawing.Size(731, 49);
            this.pnlAlerts.TabIndex = 19;
            this.pnlAlerts.Visible = false;
            // 
            // ucOMSTypeDisplay
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlBackMain);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlPanelPanel);
            this.Controls.Add(this.pnlAlerts);
            this.DoubleBuffered = true;
            this.Name = "ucOMSTypeDisplay";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(741, 488);
            this.Load += new System.EventHandler(this.ucOMSTypeDisplay_Load);
            this.VisibleChanged += new System.EventHandler(this.ucOMSTypeDisplay_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.ucOMSTypeDisplay_ParentChanged);
            this.pnlInfoBack.ResumeLayout(false);
            this.PanelActions.ResumeLayout(false);
            this.pnlBackMain.ResumeLayout(false);
            this.pnlPanelPanel.ResumeLayout(false);
            this.pnlHeading.ResumeLayout(false);
            this.ResumeLayout(false);

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

        public void RefreshElasticsearch()
        {
            
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
        public void Open(FWBS.OMS.Interfaces.IOMSType obj, OMSType omst)
        {
            tabcontents = new Dictionary<TabPage, OMSType.Tab>();
            Parent.Cursor = Cursors.WaitCursor;
            _tempPanels = null;
            _first = false;
            this.tcEnquiryPages.SelectedIndexChanged -= new System.EventHandler(this.tcEnquiryPages_SelectedIndexChanged);
            tcEnquiryPages.TabPages.Clear();
            ClearInfoPanel();

            try
            {
                //Assign the global form object.
                _dialogobj = obj;

                //Makes sure that a valid object is passed.
                if (_dialogobj == null)
                {
                    throw new OMSException2("DIAGNOOBJ", "No object passed to the type window");
                }

                //Get the config type table structure.
                if (omst == null)
                    omst = _dialogobj.GetOMSType();

                view = omst;


                //Set global colour theme of the panel as a whole.
                overridetheme = omst.OverrideTheme;
                if (omst.OverrideTheme)
                {
                    this.ipc_BackColor = view.PanelBackColour;
                    this.ipc_PanelBrightness = view.PanelBrightness;
                }

                this.ipc_Width = LogicalToDeviceUnits(view.PanelWidth);

                //Loop through each panel and add them to the panels collection.
                bool _firstpanel = true;

                Debug.WriteLine("ucOMSTypeDisplay Begin Panel Construction");

                using (ImageList en = FWBS.OMS.UI.Windows.Images.Entities32())
                {
                    foreach (OMSType.Panel pnl in view.Panels)
                    {
                        if (Session.CurrentSession.ValidateConditional(obj, pnl.Conditional))
                        {
                            switch (pnl.PanelType)
                            {
                                case OMSType.PanelTypes.Property:
                                    {
                                        if (_firstpanel)
                                        {
                                            ucPanelNavTop p = new ucPanelNavTop();
                                            p.TabStop = false;
                                            p.Text = pnl.Description;
                                            try
                                            {
                                                p.Image = en.Images[view.Glyph];
                                            }
                                            catch
                                            {
                                                p.Image = en.Images[0];
                                            }
                                            p.Expanded = pnl.Expanded;
                                            ucNavRichText r = new ucNavRichText();
                                            r.Dock = DockStyle.Fill;
                                            p.Controls.Add(r);
                                            p.pContainer = r;
                                            pnlInfoBack.Controls.Add(p, true);
                                            p.Tag = pnl;
                                            _firstpanel = false;
                                        }
                                        else
                                        {
                                            ucPanelNav p = new ucPanelNav(pnl.Description, "", pnl.Height, pnl.Expanded);
                                            p.Tag = pnl;
                                            pnlInfoBack.Controls.Add(p, true);
                                        }

                                    }
                                    break;
                                case OMSType.PanelTypes.TimeStatistics:
                                    {
                                        ucPanelNav pts = new ucPanelNav();
                                        pts.Text = pnl.Description;
                                        pts.TabStop = false;
                                        pts.Expanded = pnl.Expanded;
                                        ucNavPanel np = new ucNavPanel();
                                        np.Dock = DockStyle.Fill;
                                        np.AutoSize = true;
                                        ucTimeStats ts = new ucTimeStats();
                                        ts.Connect(obj);
                                        np.Controls.Add(ts);
                                        pts.Controls.Add(np);
                                        pts.pContainer = ts;
                                        pts.Tag = pnl;
                                        pnlInfoBack.Controls.Add(pts, true);
                                    }
                                    break;
                                case OMSType.PanelTypes.Addin:
                                    {
                                        ucPanelNav pn = new ucPanelNav();
                                        pn.Text = pnl.Description;
                                        pn.Expanded = pnl.Expanded;

                                        try
                                        {
                                            OmsObject omsobj = new OmsObject(pnl.Parameter);
                                            Type type = Global.CreateType(omsobj.Windows, omsobj.Assembly);
                                            IOMSTypeAddin addin = (IOMSTypeAddin)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);

                                            addin.Initialise(_dialogobj);
                                            if (addin.Connect(_dialogobj))
                                            {
                                                if (addin.AddinText != null) pn.Text = addin.AddinText;
                                            }
                                            else
                                            {
                                                ucErrorBox err = new ucErrorBox();
                                                err.Dock = DockStyle.Fill;
                                                err.SetErrorBox(new OMSException2("38001", "Cannot Connect To Addin '%1%'.", null as Exception, false, pnl.Parameter));
                                                err.Location = new Point(10, 20);
                                                err.BackColor = pn.BackColor;
                                                err.ForeColor = pn.ForeColor;
                                                pn.Controls.Add(err);
                                            }

                                            Control addinctrl = addin.UIElement ?? (Control)addin;
                                            addinctrl.RightToLeft = RightToLeft;
                                            Global.RightToLeftControlConverter(addinctrl, ParentForm);
                                            pn.pContainer = addinctrl;
                                            addinctrl.Dock = DockStyle.Fill;
                                            pn.Tag = pnl;
                                            pn.Controls.Add(addinctrl);
                                        }
                                        catch (Exception ex)
                                        {
                                            ucErrorBox err = new ucErrorBox();
                                            err.Dock = DockStyle.Fill;
                                            err.SetErrorBox(new OMSException2("38002A", "Error Loading Addin '%1%'." + Environment.NewLine + Environment.NewLine + "%2%", ex, false, pnl.Parameter, ex.Message));
                                            err.Location = new Point(10, 20);
                                            err.BackColor = pn.BackColor;
                                            err.ForeColor = pn.ForeColor;
                                            pn.Controls.Add(err);
                                        }
                                        pnlInfoBack.Controls.Add(pn, true);

                                    }
                                    break;
                                case OMSType.PanelTypes.DataList:
                                    {
                                        ucPanelNav pts = new ucPanelNav();
                                        pts.Text = pnl.Description;
                                        pts.Expanded = pnl.Expanded;
                                        ucNavPanel np = new ucNavPanel();
                                        np.Dock = DockStyle.Fill;
                                        np.AutoSize = true;
                                        pts.Controls.Add(np);
                                        pts.pContainer = np;
                                        pts.Tag = pnl;
                                        pnlInfoBack.Controls.Add(pts, true);
                                    }
                                    break;
                                default:
                                    goto case OMSType.PanelTypes.Property;
                            }
                        }
                    }
                }
                Debug.WriteLine("ucOMSTypeDisplay End Panel Construction");

                if (_defaultPage == null && obj.DefaultTab != null)
                    _defaultPage = obj.DefaultTab;

                //Create a new key pair tab collection.
                _pageKey = new Common.KeyValueCollection();

                Debug.WriteLine("ucOMSTypeDisplay Begin Tab Construction");

                //Loop through each of the tabs and add them to the tab page collection and also initiate a control type
                //to be put on them.
                foreach (OMSType.Tab tab in view.Tabs)
                {
                    if (tab.UserRoles == "" || (tab.UserRoles != "" && Session.CurrentSession.CurrentUser.IsInRoles(tab.UserRoles)))
                        if (Session.CurrentSession.ValidateConditional(obj, tab.Conditional))
                        {
                            Debug.WriteLine("ucOMSTypeDisplay Construct Tab :" + tab.Description + " (" + tab.Code + ")");
                            //Create a tab page based on the description returned.
                            TabPage tp = new TabPage(tab.Description);
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
                                Debug.WriteLine("ucOMSTypeDisplay If Addin :" + tab.Description + " (" + tab.Code + ")");
                                try
                                {
                                    OmsObject omsobj = new OmsObject(tab.Source);
                                    Type type = Global.CreateType(omsobj.Windows, omsobj.Assembly);

                                    IOMSTypeAddin addin = (IOMSTypeAddin)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
                                    Control addinctrl = addin.UIElement ?? (Control)addin;
                                    
                                    tp.Tag = addin;
                                    addinctrl.Visible = false;
                                    addinctrl.RightToLeft = this.RightToLeft;
                                    tp.Controls.Add(addinctrl);
                                    Global.RightToLeftControlConverter(addinctrl, this.ParentForm);

                                    Debug.WriteLine("ucOMSTypeDisplay Addin Initialise :" + tab.Description + " (" + tab.Code + ")");
                                    addin.Initialise(_dialogobj);
                                    Debug.WriteLine("ucOMSTypeDisplay Addin Initialise Complete :" + tab.Description + " (" + tab.Code + ")");

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
                                _pageKey.Add(src, tp);
                            //if no default is set, set the default - this should fefault to the first page
                            if (_defaultPage == null)
                                _defaultPage = src;

                            //Set the first tab.
                            if (_first == false)
                            {
                                Debug.WriteLine("ucOMSTypeDisplay Before DoEvents:" + tab.Description + " (" + tab.Code + ")");
                                Application.DoEvents();
                                Debug.WriteLine("ucOMSTypeDisplay After DoEvents:" + tab.Description + " (" + tab.Code + ")");
                                _first = true;
                            }
                        }
                }
                Debug.WriteLine("ucOMSTypeDisplay End Tab Construction");
                Debug.WriteLine("ucOMSTypeDisplay Security Tab Construction");

                if (Session.CurrentSession.AdvancedSecurity && ((obj is OMSDocument && Session.CurrentSession.AdvancedSecurityDocumentActive) || (obj is OMSFile && Session.CurrentSession.AdvancedSecurityFileActive) || (obj is Contact && Session.CurrentSession.AdvancedSecurityContactActive) || (obj is Client && Session.CurrentSession.AdvancedSecurityClientActive)))
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
                        tp.Tag = addin;
                        tp.Controls.Add(addinctrl);
                        addinctrl.RightToLeft = RightToLeft;
                        Global.RightToLeftControlConverter(addinctrl, ParentForm);
                        addinctrl.Dock = DockStyle.Fill;
                    }
                    catch (Exception ex)
                    {
                        ucErrorBox err = new ucErrorBox();
                        err.SetErrorBox(ex);
                        err.Location = new Point(10, 20);
                        tp.Controls.Add(err);
                    }
                    tcEnquiryPages.AddTabPage(tp);
                }
                Debug.WriteLine("ucOMSTypeDisplay Security Tab Construction");

                PanelActions.BringToFront();
                RefreshItem(true);

                Debug.WriteLine("ucOMSTypeDisplay Fire First Page...");
                tcEnquiryPages_SelectedIndexChanged(this, EventArgs.Empty);
                this.tcEnquiryPages.SelectedIndexChanged += new System.EventHandler(this.tcEnquiryPages_SelectedIndexChanged);
            }
            finally
            {
                Parent.Cursor = Cursors.Default;
            }
        }

        void tp_ControlAdded(object sender, ControlEventArgs e)
        {
            e.Control.BackColor = System.Drawing.SystemColors.Window;
        }



        /// <summary>
        /// Capture the tab page click and refresh each of the controls on the tab.
        /// </summary>
        /// <param name="sender">Tabs control object.</param>
        /// <param name="e">Empty event arguments.</param>
        private void tcEnquiryPages_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            TabPage _selectedTab = tcEnquiryPages.SelectedTab;
            IOpenOMSType Iomstype = null;
            if (_selectedTab == null)
                return;

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
                    //TabControlFocus();
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
                                if (ictrl != null) ictrl .Dock = DockStyle.Fill;
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

                                    _tempPanels = addin.Panels;

                                    if (_tempPanels != null)
                                    {
                                        foreach (ucPanelNav panel in _tempPanels)
                                        {
                                            pnlInfoBack.Controls.Add(panel);
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
                                etoolbar.ShowPanelButtons();
                                PanelActions.Visible = (ucNavCommands1.Controls.Count > 0);
                                PanelActions.BringToFront();
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
            }
        }

        /// <summary>
        /// This method gets called when an item is added to the panels collection.
        /// </summary>
        /// <param name="sender">Control Collection Object.</param>
        /// <param name="e">Control event arguments.</param>
        private void AddPanel(object sender, ControlEventArgs e)
        {
            ucPanelNav pnl = (ucPanelNav)e.Control;
            pnl.Dock = DockStyle.Top;
            if (overridetheme == false)
                pnl.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            else
                pnl.Brightness = _increasebrightness;
            pnl.BringToFront();
        }

        /// <summary>
        /// Load event of the control.
        /// </summary>
        /// <param name="sender">This control.</param>
        /// <param name="e">Empty event arguments.</param>
        private void ucOMSTypeDisplay_Load(object sender, System.EventArgs e)
        {
            Debug.WriteLine("ucOMSTypeDisplay Load...");
            if (!DesignMode)
            {
                tcEnquiryPages.ImageList = Images.GetCoolButtonsList((Images.IconSize)LogicalToDeviceUnits(16));
            }
            Debug.WriteLine("ucOMSTypeDisplay set Parent Form...");
            _parentform = Global.GetParentForm(this);
            Debug.WriteLine("ucOMSTypeDisplay set Parent Form Finished...");
            
            pnlPanelPanel.Visible = _pnlpanelpanel;
            TabControlItemFocus();
        }

        protected override void OnDpiChangedBeforeParent(EventArgs e)
        {
            base.OnDpiChangedBeforeParent(e);
            tcEnquiryPages.ImageList = Images.GetCoolButtonsList((Images.IconSize)LogicalToDeviceUnits(16));
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

        #endregion
        
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

        /// <summary>
        /// Shows or Hides the Information Panel
        /// </summary>
        public bool InformationPanelVisible
        {
            get
            {
                return _pnlpanelpanel;
            }
            set
            {
                if (this.SearchManagerVisible == false || value == false)
                {
                    _pnlpanelpanel = value;
                    pnlPanelPanel.Visible = value;
                    splitter1.Visible = value;
                }
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
        /// Gets or Sets the Visiblity of the Show Search Manager
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

        /// <summary>
        /// Gets or Sets the Visiblity of the Show Search Manager
        /// </summary>
        [Category("Search")]
        public bool ElasticsearchVisible
        {
            get
            {
                return true;
            }
            set
            {
                if (value)
                    ShowElasticsearch();
                else
                    HideElasticsearch();

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
                    case Common.TriState.True: succinct = true; break;
                    case Common.TriState.False: succinct = false; break;
                    case Common.TriState.Null: succinct = Session.CurrentSession.SuccinctTypeDisplayFormCaption; break;
                }
                return succinct ? ObjectDescription : ObjectTypeDescription;
            }
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
                            IOMSItem itm = (IOMSItem)tab.Tag;
                            itm.ToBeRefreshed = true;

                            if (tab == tcEnquiryPages.SelectedTab)
                            {
                                itm.RefreshItem();
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

            foreach (Control ctrl in pnlInfoBack.Controls)
            {
                ucPanelNav c = ctrl as ucPanelNav;
                if (c != null && c.Tag is OMSType.Panel)
                {
                    string output = "";
                    OMSType.Panel pnl = c.Tag as OMSType.Panel;

                    if (pnl.PanelType == OMSType.PanelTypes.Property)
                    {
                        try
                        {
                            if (pnl.Parameter != "")
                                output = Convert.ToString(_dialogobj.GetType().InvokeMember(pnl.Parameter, System.Reflection.BindingFlags.GetProperty, null, _dialogobj, Type.EmptyTypes));
                        }
                        catch
                        {
                        }
                    }
                    if (c.pContainer is ucNavRichText)
                    {
                        try       
                        {
                              
                            if (output.ToLower().StartsWith(@"{\\rtf1", StringComparison.OrdinalIgnoreCase) || output.ToLower().StartsWith(@"{\rtf1", StringComparison.OrdinalIgnoreCase))
                            {
                                ((ucNavRichText)c.pContainer).Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(output);
                            }
                            else
                            {
                                ((ucNavRichText)c.pContainer).Text  = output;
                            }
                        }
                        catch 
                        {
                            ((ucNavRichText)c.pContainer).Text = output;
                        }
                        ((ucNavRichText)c.pContainer).Refresh();
                    }
                    else if (c.pContainer is ucTimeStats)
                    {
                        ((ucTimeStats)c.pContainer).RefreshItem();

                        if (((ucTimeStats)c.pContainer).Warning && c.Expanded == false)
                        {
                            if (c.LoadFired == false)
                                c.Expanded = true;
                            else
                                c.ToggleExpand();
                        }
                    }
                    else if (c.pContainer is ucNavPanel)
                    {
                        if (c.Expanded) c.Visible = false;
                        ucNavPanel n = c.pContainer as ucNavPanel;
                        FWBS.OMS.EnquiryEngine.DataLists dtlist = new FWBS.OMS.EnquiryEngine.DataLists(pnl.Parameter);
                        dtlist.ChangeParent(_dialogobj);
                        DataTable dt = dtlist.Run(false) as DataTable;
                        int max = 0;
                        FWBS.Common.UI.Windows.eLabel2 tlab = new FWBS.Common.UI.Windows.eLabel2();
                        using (Graphics g = Graphics.FromHwnd(Parent.Handle))
                        {
                            foreach (DataRow rw in dt.Rows)
                            {
                                tlab.Text = Convert.ToString(rw[0]);
                                max = Math.Max(max, Convert.ToInt32(g.MeasureString(tlab.Text, tlab.Font).Width));
                            }
                        }
                        tlab.Dispose();
                        n.Controls.Clear();
                        foreach (DataRow rw in dt.Rows)
                        {
                            FWBS.Common.UI.Windows.eLabel2 lab = new FWBS.Common.UI.Windows.eLabel2();
                            lab.Text = Convert.ToString(rw[0]);
                            lab.Value = rw[1];
                            lab.CaptionWidth = max + 5;
                            n.Controls.Add(lab, true);
                            lab.SendToBack();
                        }
                        n.Refresh();
                        c.Visible = true;
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

        private void pnlPanelPanel_VisibleChanged(object sender, System.EventArgs e)
        {
            splitter1.Visible = pnlPanelPanel.Visible;
        }


        #endregion

        #region IOMSTypeDisplay
        public void ShowElasticsearch()
        {
            
        }

        public void ShowSearchManager()
        {
            ShowSearchManager(SearchManager.ContactManager);
        }

        /// <summary>
        /// Shows and or Creates the Search Manager
        /// </summary>
        public void ShowSearchManager(SearchManager Style)
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
                ucSearchManager1.SearchForIndex = Convert.ToInt32(Style);
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

            if (SearchManagerVisibleChanged != null)
                SearchManagerVisibleChanged(this, EventArgs.Empty);
        }

        void CurrentView_SearchCompleted(object sender, EventArgs e)
        {
            if (this.SearchCompleted != null)
                this.SearchCompleted(this, e);
        }

        public void ClearInfoPanel()
        {
            pnlInfoBack.Controls.Clear();
        }

        /// <summary>
        /// Hides the Tickford Search
        /// </summary>
        public void HideElasticsearch()
        {
            
        }

        /// <summary>
        /// Hides the Search Manager
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

            if (SearchManagerVisibleChanged != null)
                SearchManagerVisibleChanged(this, EventArgs.Empty);
            
            if (_parentform != null) _parentform.Text = this.ObjectTypeCaption;
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
                ErrorBox.Show(ParentForm, new OMSException2("TABNOTFOUND", "Tab Page '%1%' cannot be found",null,true,Code));
        }

        /// <summary>
        /// Sets the Visibility Info Panel Close Button
        /// </summary>
        public bool InfoPanelCloseVisible
        {
            get
            {
                return btnClose.Visible;
            }
            set
            {
                btnClose.Visible = value;
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

        private void pnlHeading_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawRectangle(Pens.Black, 0, 0, pnlInfoBack.Width - 1, pnlInfoBack.Height - 1);
            if (FWBS.OMS.Session.CurrentSession.IsLoggedIn)
            {
                string heading = FWBS.OMS.Session.CurrentSession.Resources.GetResource("INFORMATION", "Information", "").Text;
                using (StringFormat sf = new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center })
                {
                    if (this.RightToLeft == RightToLeft.Yes)
                        sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;

                    Rectangle rect = pnlHeading.Bounds;
                    rect.Inflate(-LogicalToDeviceUnits(4), 0);
                    using (SolidBrush br = new SolidBrush(pnlHeading.ForeColor))
                    {
                        e.Graphics.DrawString(heading, pnlHeading.Font, br, rect, sf);
                    }
                }
            }
        }

        private void SetOfficeStyle()
        {
            this.btnClose.Image = global::FWBS.OMS.UI.Properties.Resources.BlackColapse;
            this.pnlHeading.BorderStyle = BorderStyle.None;
            this.PanelActions.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            this.pnlBackMain.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }


        private void pnlHeading_Resize(object sender, EventArgs e)
        {
            pnlHeading.Invalidate();
        }

        #endregion


        public void ApplyFilter(int state)
        {
            
        }


        private void tcEnquiryPages_Click(object sender, EventArgs e)
        {
            TabControlItemFocus();
        }
    }
}
