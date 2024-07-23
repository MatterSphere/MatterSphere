using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using Dynamsoft.Core;
using Dynamsoft.Forms.Enums;
using Dynamsoft.OCR;
using Dynamsoft.OCR.Enums;

using FWBS.OMS;

namespace FWBS.Scanning
{
    /// <summary>
    /// Summary description for ucSubReports.
    /// </summary>
    public class ucScanning : FWBS.OMS.UI.Windows.ucBaseAddin, FWBS.OMS.UI.Windows.Interfaces.IOMSTypeAddin
    {
        #region API

        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
            IntPtr hdcDest, // handle to destination DC
            int nXDest,  // x-coord of destination upper-left corner
            int nYDest,  // y-coord of destination upper-left corner
            int nWidth,  // width of destination rectangle
            int nHeight, // height of destination rectangle
            IntPtr hdcSrc,  // handle to source DC
            int nXSrc,   // x-coordinate of source upper-left corner
            int nYSrc,   // y-coordinate of source upper-left corner
            System.Int32 dwRop  // raster operation code
            );

        #endregion

        #region Fields
        public event EventHandler ChangeImaged;


        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlRight;
        private FWBS.OMS.UI.Windows.eToolbars eToolbars1;
        private FWBS.OMS.UI.Windows.ucPanelNavTop ucPanelNavTop1;
        private FWBS.OMS.UI.Windows.ucNavPanel ucNavPanel1;
        private System.Windows.Forms.Label labClientFileRef;
        private System.Windows.Forms.Label labFile;
        private FWBS.OMS.UI.Windows.ucNavRichText ucNavRichText1;
        private System.Windows.Forms.Panel pnlRef;
        private Panel pnlTagged;
        private FWBS.OMS.UI.Windows.OMSToolBarButton btnPrev;
        private FWBS.OMS.UI.Windows.OMSToolBarButton btnNext;
        private FWBS.OMS.UI.Windows.OMSToolBarButton btnSp1;
        private FWBS.OMS.UI.Windows.OMSToolBarButton btnRotateLeft;
        private FWBS.OMS.UI.Windows.OMSToolBarButton btnRotateRigth;
        private System.Windows.Forms.Panel pnlCenter;
        private System.Windows.Forms.Panel panel1;
        private FWBS.OMS.UI.Windows.eToolbars eToolbars2;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtClientNo;
        private System.Windows.Forms.ComboBox cboFileList;
        private System.Windows.Forms.CheckBox chkAllFiles;
        private FWBS.OMS.UI.Windows.ucNavCommands ucNavActions;
        private FWBS.OMS.UI.Windows.ucPanelNav ucPanNavInfo;
        private FWBS.OMS.UI.Windows.ucNavCmdButtons btnAction1;
        private FWBS.OMS.UI.Windows.ucPanelNav ucNavPanelActions;
        private FWBS.Common.UI.Windows.eLabel2 eScans;
        private FWBS.Common.UI.Windows.eLabel2 ePages;
        private System.Windows.Forms.ContextMenu ctmTagged;
        private System.Windows.Forms.Panel pnlMiddle;
        private System.Windows.Forms.PictureBox picTagged;
        private System.Windows.Forms.ImageList imgBig;
        private System.Windows.Forms.MenuItem mnuRedFlag;
        private System.Windows.Forms.MenuItem mnuGreenFlag;
        private System.Windows.Forms.MenuItem mnuYellowFlag;
        private System.Windows.Forms.MenuItem mnuGreyFlag;
        private System.Windows.Forms.MenuItem mnuPurpleFlag;
        private System.Windows.Forms.MenuItem mnuBlueFlag;
        private System.Windows.Forms.MenuItem mnuSSp1;
        private System.Windows.Forms.MenuItem mnuOffFlag;
        private System.Windows.Forms.MenuItem mnuFltRed;
        private System.Windows.Forms.MenuItem mnuFltBlue;
        private System.Windows.Forms.MenuItem mnuFltYellow;
        private System.Windows.Forms.MenuItem mnuFltGreen;
        private System.Windows.Forms.MenuItem mnuFltGrey;
        private System.Windows.Forms.MenuItem mnuFltPurple;
        private Dynamsoft.Forms.DSViewer dsViewer;
        private VScrollBar scrollBar;
        private PictureBox ghost;
        private System.ComponentModel.IContainer components;
        private FWBS.OMS.UI.Windows.eToolbars eToolbars3;
        private System.Windows.Forms.Panel pnlSp3;
        private FWBS.Common.UI.Windows.eLabel2 eProc;
        private System.Windows.Forms.Panel pnlSp4;
        private System.Windows.Forms.Timer timReturnImage;
        private System.Windows.Forms.FolderBrowserDialog fldMoveTo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnFind;
        private FWBS.OMS.UI.Windows.ucPanelNav ucPanelNav1;
        private FWBS.OMS.UI.Windows.ucNavPanel ucNavPanel2;
        private System.Windows.Forms.CheckBox chkApplyFilter;
        private System.Windows.Forms.MonthCalendar Calendar;
        private System.Windows.Forms.Label labDate;
        private System.Windows.Forms.Label labCreated;
        private System.Windows.Forms.MenuItem mnuFltOff;


        private System.IO.DirectoryInfo _scanlocation = null;
        private int _scanposition = 0;
        private System.IO.FileInfo[] _files = null;

        private FWBS.OMS.User _user;
        private FWBS.OMS.Client _client;
        private DataView _fileview = null;
        private FWBS.OMS.OMSFile _omsfile = null;
        public event FWBS.OMS.AlertEventHandler Alert = null;
        private string _movepath = "";
        private bool _tagged = false;
        private SaveSettingsResults _results = SaveSettingsResults.ssrNothing;
        private string _tifFilter = "*.tif";
        private MenuItem _lastfilter = null;
        private ImageCore m_ImageCore = null;
        private Tesseract m_Tesseract = null;
        private string _username = "";
        private string _documentcontent = String.Empty;
        private byte[] selectedOcr = null;
        private const string dsProductKey = "<The license for OCR Module>";
        private string _fallback = new System.IO.DirectoryInfo(System.Environment.GetFolderPath(Environment.SpecialFolder.Personal)).FullName;
        private ContextMenu ctmFilter;


        /// <summary>
        /// Key - Temporary Tif file, Value - Source image file
        /// </summary>
        private Dictionary<string, string> _filesConvertedToTif = new Dictionary<string, string>();

        #endregion

        #region IOMSTypeAddin Implementation

        /// <summary>
        /// Allows the calling OMS dialog to connect to the addin for the configurable type object.
        /// </summary>
        /// <param name="obj">OMS Configurable type object to use.</param>
        /// <returns>A flag that tells the dialogthat the connection has been successfull.</returns>
        public override bool Connect(FWBS.OMS.Interfaces.IOMSType obj)
        {

            _user = obj as FWBS.OMS.User;

            if (obj == null)
                return false;
            else
            {
                string val = Convert.ToString(FWBS.Common.RegistryAccess.GetSetting("", Microsoft.Win32.RegistryHive.CurrentUser, @"\Software\FWBS\OMS\2.0\OMSDocumentImporter", "AfterSave"));
                _results = (SaveSettingsResults)FWBS.Common.ConvertDef.ToEnum(val, SaveSettingsResults.ssrNothing);
                _movepath = Convert.ToString(FWBS.Common.RegistryAccess.GetSetting("", Microsoft.Win32.RegistryHive.CurrentUser, @"\Software\FWBS\OMS\2.0\OMSDocumentImporter", "MovePath"));
                eToolbars1.GetButton("btnTagImage").DropDownMenu = ctmTagged;
                eToolbars1.GetButton("btnFilterBy").DropDownMenu = ctmFilter;
                _username = _user.FullName;
                _lastfilter = mnuFltOff;
                CurrentUserID = _user.ID;
                string path = "";
                try
                {
                    path = Convert.ToString(FWBS.OMS.Session.CurrentSession.CurrentUser.GetExtraInfo("usrImageFolder")).TrimEnd('\\');
                    _movepath = Convert.ToString(FWBS.OMS.Session.CurrentSession.CurrentUser.GetExtraInfo("UsrImageFolderDone")).TrimEnd('\\');
                    if (_movepath == "") _movepath = _fallback;
                    if (path == "") path = _fallback;
                }
                catch
                {
                    if (path == "") path = _fallback;
                }
                _scanlocation = new System.IO.DirectoryInfo(path);
                if (_scanlocation.Exists == false)
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("ERRNOSCANFOLDER", "The User's Scanning Folder \"%1%\" cannot be found. Please create it.", "", path).Text);
                if (ParentForm != null)
                {
                    ParentForm.KeyPreview = true;
                    ParentForm.KeyDown += new KeyEventHandler(ParentForm_KeyDown);
                }
                ToBeRefreshed = true;
                return true;
            }

        }

        /// <summary>
        /// Refreshes the addin visual look and data contents.
        /// </summary>
        public override void RefreshItem()
        {
            if (_user != null && ToBeRefreshed)
            {
                ToBeRefreshed = false;
            }
        }
        #endregion

        #region Constructors
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        public ucScanning()
        {
            // This call is required by the Windows.Forms Form Designer.
            this.m_Tesseract = new Tesseract(dsProductKey);
            string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            this.m_Tesseract.TessDataPath = Path.Combine(currentFolder, "languages");
            this.m_Tesseract.Language = "eng";
            this.m_Tesseract.ResultFormat = ResultFormat.Text;
            this.m_ImageCore = new ImageCore();

            InitializeComponent();

            if (Session.CurrentSession.IsConnected)
            {
                Res resources = Session.CurrentSession.Resources;
                mnuRedFlag.Text = mnuFltRed.Text = resources.GetResource("RedFlag", "&Red Flag", "").Text;
                mnuBlueFlag.Text = mnuFltBlue.Text = resources.GetResource("BlueFlag", "&Blue Flag", "").Text;
                mnuYellowFlag.Text = mnuFltYellow.Text = resources.GetResource("YellowFlag", "&Yellow Flag", "").Text;
                mnuGreenFlag.Text = mnuFltGreen.Text = resources.GetResource("GreenFlag", "&Green Flag", "").Text;
                mnuGreyFlag.Text = mnuFltGrey.Text = resources.GetResource("GreyFlag", "Gr&ey Flag", "").Text;
                mnuPurpleFlag.Text = mnuFltPurple.Text = resources.GetResource("PurpleFlag", "P&urple Flag", "").Text;
                mnuOffFlag.Text = mnuFltOff.Text = resources.GetResource("OFF", "Off", "").Text;
            }

            dsViewer.Bind(m_ImageCore);
            this.dsViewer.SetViewMode(-1, -1);
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucScanning));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlCenter = new System.Windows.Forms.Panel();
            this.pnlMiddle = new System.Windows.Forms.Panel();
            this.pnlTagged = new System.Windows.Forms.Panel();
            this.picTagged = new System.Windows.Forms.PictureBox();
            this.dsViewer = new Dynamsoft.Forms.DSViewer();
            this.scrollBar = new System.Windows.Forms.VScrollBar();
            this.pnlSp3 = new System.Windows.Forms.Panel();
            this.eToolbars3 = new FWBS.OMS.UI.Windows.eToolbars();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labDate = new System.Windows.Forms.Label();
            this.labCreated = new System.Windows.Forms.Label();
            this.eToolbars2 = new FWBS.OMS.UI.Windows.eToolbars();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.ucPanelNav1 = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavPanel2 = new FWBS.OMS.UI.Windows.ucNavPanel();
            this.Calendar = new System.Windows.Forms.MonthCalendar();
            this.chkApplyFilter = new System.Windows.Forms.CheckBox();
            this.eProc = new FWBS.Common.UI.Windows.eLabel2();
            this.pnlSp4 = new System.Windows.Forms.Panel();
            this.ePages = new FWBS.Common.UI.Windows.eLabel2();
            this.eScans = new FWBS.Common.UI.Windows.eLabel2();
            this.ucNavPanelActions = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavActions = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.btnAction1 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.ucPanNavInfo = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavRichText1 = new FWBS.OMS.UI.Windows.ucNavRichText();
            this.ucPanelNavTop1 = new FWBS.OMS.UI.Windows.ucPanelNavTop();
            this.ucNavPanel1 = new FWBS.OMS.UI.Windows.ucNavPanel();
            this.labClientFileRef = new System.Windows.Forms.Label();
            this.pnlRef = new System.Windows.Forms.Panel();
            this.txtClientNo = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.labFile = new System.Windows.Forms.Label();
            this.cboFileList = new System.Windows.Forms.ComboBox();
            this.chkAllFiles = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnPrev = new FWBS.OMS.UI.Windows.OMSToolBarButton();
            this.btnNext = new FWBS.OMS.UI.Windows.OMSToolBarButton();
            this.btnSp1 = new FWBS.OMS.UI.Windows.OMSToolBarButton();
            this.btnRotateLeft = new FWBS.OMS.UI.Windows.OMSToolBarButton();
            this.btnRotateRigth = new FWBS.OMS.UI.Windows.OMSToolBarButton();
            this.eToolbars1 = new FWBS.OMS.UI.Windows.eToolbars();
            this.ctmTagged = new System.Windows.Forms.ContextMenu();
            this.mnuRedFlag = new System.Windows.Forms.MenuItem();
            this.mnuBlueFlag = new System.Windows.Forms.MenuItem();
            this.mnuYellowFlag = new System.Windows.Forms.MenuItem();
            this.mnuGreenFlag = new System.Windows.Forms.MenuItem();
            this.mnuGreyFlag = new System.Windows.Forms.MenuItem();
            this.mnuPurpleFlag = new System.Windows.Forms.MenuItem();
            this.mnuOffFlag = new System.Windows.Forms.MenuItem();
            this.mnuSSp1 = new System.Windows.Forms.MenuItem();
            this.mnuFltRed = new System.Windows.Forms.MenuItem();
            this.mnuFltBlue = new System.Windows.Forms.MenuItem();
            this.mnuFltYellow = new System.Windows.Forms.MenuItem();
            this.mnuFltGreen = new System.Windows.Forms.MenuItem();
            this.mnuFltGrey = new System.Windows.Forms.MenuItem();
            this.mnuFltPurple = new System.Windows.Forms.MenuItem();
            this.mnuFltOff = new System.Windows.Forms.MenuItem();
            this.imgBig = new System.Windows.Forms.ImageList(this.components);
            this.timReturnImage = new System.Windows.Forms.Timer(this.components);
            this.fldMoveTo = new System.Windows.Forms.FolderBrowserDialog();
            this.ghost = new System.Windows.Forms.PictureBox();
            this.ctmFilter = new System.Windows.Forms.ContextMenu();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlCenter.SuspendLayout();
            this.pnlMiddle.SuspendLayout();
            this.pnlTagged.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTagged)).BeginInit();
            this.panel1.SuspendLayout();
            this.pnlRight.SuspendLayout();
            this.ucPanelNav1.SuspendLayout();
            this.ucNavPanel2.SuspendLayout();
            this.ucNavPanelActions.SuspendLayout();
            this.ucNavActions.SuspendLayout();
            this.ucPanNavInfo.SuspendLayout();
            this.ucPanelNavTop1.SuspendLayout();
            this.ucNavPanel1.SuspendLayout();
            this.pnlRef.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ghost)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Location = new System.Drawing.Point(0, 34);
            this.pnlDesign.Size = new System.Drawing.Size(168, 639);
            // 
            // pnlActions
            // 
            this.pnlActions.ExpandedHeight = 85;
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Size = new System.Drawing.Size(152, 85);
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // navCommands
            // 
            this.navCommands.Size = new System.Drawing.Size(152, 54);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.pnlCenter);
            this.pnlMain.Controls.Add(this.pnlRight);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(168, 34);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(809, 639);
            this.pnlMain.TabIndex = 9;
            // 
            // pnlCenter
            // 
            this.pnlCenter.AutoScroll = true;
            this.pnlCenter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCenter.Controls.Add(this.pnlMiddle);
            this.pnlCenter.Controls.Add(this.panel1);
            this.pnlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCenter.Location = new System.Drawing.Point(0, 0);
            this.pnlCenter.Name = "pnlCenter";
            this.pnlCenter.Size = new System.Drawing.Size(534, 639);
            this.pnlCenter.TabIndex = 4;
            // 
            // pnlMiddle
            // 
            this.pnlMiddle.BackColor = System.Drawing.SystemColors.Control;
            this.pnlMiddle.Controls.Add(this.pnlTagged);
            this.pnlMiddle.Controls.Add(this.dsViewer);
            this.pnlMiddle.Controls.Add(this.scrollBar);
            this.pnlMiddle.Controls.Add(this.pnlSp3);
            this.pnlMiddle.Controls.Add(this.eToolbars3);
            this.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddle.Location = new System.Drawing.Point(0, 0);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Size = new System.Drawing.Size(532, 607);
            this.pnlMiddle.TabIndex = 8;
            // 
            // pnlTagged
            // 
            this.pnlTagged.BackColor = System.Drawing.Color.White;
            this.pnlTagged.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTagged.Controls.Add(this.picTagged);
            this.pnlTagged.Location = new System.Drawing.Point(40, 12);
            this.pnlTagged.Name = "pnlTagged";
            this.pnlTagged.Size = new System.Drawing.Size(56, 56);
            this.pnlTagged.TabIndex = 19;
            // 
            // picTagged
            // 
            this.picTagged.Image = ((System.Drawing.Image)(resources.GetObject("picTagged.Image")));
            this.picTagged.Location = new System.Drawing.Point(3, 3);
            this.picTagged.Name = "picTagged";
            this.picTagged.Size = new System.Drawing.Size(48, 48);
            this.picTagged.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picTagged.TabIndex = 11;
            this.picTagged.TabStop = false;
            this.picTagged.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picTagged_MouseMove_1);
            // 
            // dsViewer
            // 
            this.dsViewer.BackgroundColor = -5526613;
            this.dsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dsViewer.EnableInteractiveZoom = false;
            this.dsViewer.Location = new System.Drawing.Point(26, 0);
            this.dsViewer.MouseShape = true;
            this.dsViewer.Name = "dsViewer";
            this.dsViewer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.dsViewer.SelectionRectAspectRatio = 0D;
            this.dsViewer.Size = new System.Drawing.Size(489, 607);
            this.dsViewer.TabIndex = 12;
            this.dsViewer.OnImageAreaDeselected += new Dynamsoft.Forms.Delegate.OnImageAreaDeselectedHandler(this.DsViewer_OnImageAreaDeselected);
            this.dsViewer.OnImageAreaSelected += new Dynamsoft.Forms.Delegate.OnImageAreaSelectedHandler(this.DsViewer_OnImageAreaSelected);
            this.dsViewer.SizeChanged += new System.EventHandler(this.DsViewer_OnResize);
            this.dsViewer.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.DsViewer_OnMouseWheel);
            // 
            // scrollBar
            // 
            this.scrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrollBar.LargeChange = 1;
            this.scrollBar.Location = new System.Drawing.Point(515, 0);
            this.scrollBar.Name = "scrollBar";
            this.scrollBar.Size = new System.Drawing.Size(17, 607);
            this.scrollBar.TabIndex = 13;
            this.scrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollbar_OnScroll);
            // 
            // pnlSp3
            // 
            this.pnlSp3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.pnlSp3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSp3.Location = new System.Drawing.Point(23, 0);
            this.pnlSp3.Name = "pnlSp3";
            this.pnlSp3.Size = new System.Drawing.Size(3, 607);
            this.pnlSp3.TabIndex = 18;
            // 
            // eToolbars3
            // 
            this.eToolbars3.AutoSize = false;
            this.eToolbars3.BottomDivider = false;
            this.eToolbars3.ButtonsXML = resources.GetString("eToolbars3.ButtonsXML");
            this.eToolbars3.Divider = false;
            this.eToolbars3.Dock = System.Windows.Forms.DockStyle.Left;
            this.eToolbars3.DropDownArrows = true;
            this.eToolbars3.ImageLists = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.eToolbars3.Location = new System.Drawing.Point(0, 0);
            this.eToolbars3.Name = "eToolbars3";
            this.eToolbars3.NavCommandPanel = null;
            this.eToolbars3.ShowToolTips = true;
            this.eToolbars3.Size = new System.Drawing.Size(23, 607);
            this.eToolbars3.TabIndex = 17;
            this.eToolbars3.TopDivider = false;
            this.eToolbars3.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.eToolbars3_ButtonClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.labDate);
            this.panel1.Controls.Add(this.labCreated);
            this.panel1.Controls.Add(this.eToolbars2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 607);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.panel1.Size = new System.Drawing.Size(532, 30);
            this.panel1.TabIndex = 1;
            // 
            // labDate
            // 
            this.labDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labDate.Location = new System.Drawing.Point(450, 5);
            this.labDate.Name = "labDate";
            this.labDate.Size = new System.Drawing.Size(78, 20);
            this.labDate.TabIndex = 19;
            this.labDate.Text = "07/09/2005";
            this.labDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labCreated
            // 
            this.labCreated.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labCreated.Location = new System.Drawing.Point(394, 5);
            this.resourceLookup1.SetLookup(this.labCreated, new FWBS.OMS.UI.Windows.ResourceLookupItem("labOcrCreated", "Created : ", ""));
            this.labCreated.Name = "labCreated";
            this.labCreated.Size = new System.Drawing.Size(56, 20);
            this.labCreated.TabIndex = 20;
            this.labCreated.Text = "Created : ";
            this.labCreated.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // eToolbars2
            // 
            this.eToolbars2.BottomDivider = false;
            this.eToolbars2.ButtonsXML = resources.GetString("eToolbars2.ButtonsXML");
            this.eToolbars2.Divider = false;
            this.eToolbars2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.eToolbars2.DropDownArrows = true;
            this.eToolbars2.ImageLists = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.eToolbars2.Location = new System.Drawing.Point(0, 3);
            this.eToolbars2.Name = "eToolbars2";
            this.eToolbars2.NavCommandPanel = null;
            this.eToolbars2.PanelImageLists = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.eToolbars2.ShowToolTips = true;
            this.eToolbars2.Size = new System.Drawing.Size(532, 26);
            this.eToolbars2.TabIndex = 15;
            this.eToolbars2.TopDivider = false;
            this.eToolbars2.Wrappable = false;
            this.eToolbars2.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.eToolbars1_ButtonClick);
            // 
            // pnlRight
            // 
            this.pnlRight.AutoScroll = true;
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlRight.Controls.Add(this.ucPanelNav1);
            this.pnlRight.Controls.Add(this.eProc);
            this.pnlRight.Controls.Add(this.pnlSp4);
            this.pnlRight.Controls.Add(this.ePages);
            this.pnlRight.Controls.Add(this.eScans);
            this.pnlRight.Controls.Add(this.ucNavPanelActions);
            this.pnlRight.Controls.Add(this.ucPanNavInfo);
            this.pnlRight.Controls.Add(this.ucPanelNavTop1);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlRight.Location = new System.Drawing.Point(534, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(12);
            this.pnlRight.Size = new System.Drawing.Size(275, 639);
            this.pnlRight.TabIndex = 3;
            // 
            // ucPanelNav1
            // 
            this.ucPanelNav1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNav1.Controls.Add(this.ucNavPanel2);
            this.ucPanelNav1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNav1.ExpandedHeight = 228;
            this.ucPanelNav1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNav1.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanelNav1.Location = new System.Drawing.Point(12, 334);
            this.resourceLookup1.SetLookup(this.ucPanelNav1, new FWBS.OMS.UI.Windows.ResourceLookupItem("pnlDateFilterBy", "Date Filter By", ""));
            this.ucPanelNav1.Name = "ucPanelNav1";
            this.ucPanelNav1.Size = new System.Drawing.Size(251, 228);
            this.ucPanelNav1.TabIndex = 6;
            this.ucPanelNav1.TabStop = false;
            this.ucPanelNav1.Text = "Date Filter By";
            this.ucPanelNav1.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ucNavPanel2
            // 
            this.ucNavPanel2.Controls.Add(this.Calendar);
            this.ucNavPanel2.Controls.Add(this.chkApplyFilter);
            this.ucNavPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavPanel2.Location = new System.Drawing.Point(0, 24);
            this.ucNavPanel2.Name = "ucNavPanel2";
            this.ucNavPanel2.Padding = new System.Windows.Forms.Padding(5);
            this.ucNavPanel2.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavPanel2.Size = new System.Drawing.Size(251, 197);
            this.ucNavPanel2.TabIndex = 15;
            // 
            // Calendar
            // 
            this.Calendar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Calendar.Location = new System.Drawing.Point(5, 8);
            this.Calendar.Name = "Calendar";
            this.Calendar.TabIndex = 0;
            this.Calendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.Calendar_DateChanged);
            // 
            // chkApplyFilter
            // 
            this.chkApplyFilter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkApplyFilter.Location = new System.Drawing.Point(5, 170);
            this.resourceLookup1.SetLookup(this.chkApplyFilter, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkApplyFilter", "Apply Filter", ""));
            this.chkApplyFilter.Name = "chkApplyFilter";
            this.chkApplyFilter.Size = new System.Drawing.Size(241, 22);
            this.chkApplyFilter.TabIndex = 1;
            this.chkApplyFilter.Text = "Apply Filter";
            this.chkApplyFilter.CheckedChanged += new System.EventHandler(this.chkApplyFilter_CheckedChanged);
            // 
            // eProc
            // 
            this.eProc.CaptionWidth = 150;
            this.eProc.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.eProc.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.eProc.ForeColor = System.Drawing.SystemColors.Window;
            this.eProc.Format = "";
            this.eProc.IsDirty = false;
            this.eProc.Location = new System.Drawing.Point(12, 556);
            this.resourceLookup1.SetLookup(this.eProc, new FWBS.OMS.UI.Windows.ResourceLookupItem("lbOCRProcessing", "OCR Processing : ", ""));
            this.eProc.Name = "eProc";
            this.eProc.ReadOnly = true;
            this.eProc.Size = new System.Drawing.Size(251, 23);
            this.eProc.TabIndex = 4;
            this.eProc.Text = "OCR Processing : ";
            this.eProc.Value = null;
            // 
            // pnlSp4
            // 
            this.pnlSp4.BackColor = System.Drawing.Color.White;
            this.pnlSp4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlSp4.Location = new System.Drawing.Point(12, 579);
            this.pnlSp4.Name = "pnlSp4";
            this.pnlSp4.Size = new System.Drawing.Size(251, 2);
            this.pnlSp4.TabIndex = 5;
            // 
            // ePages
            // 
            this.ePages.CaptionWidth = 150;
            this.ePages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ePages.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.ePages.Format = "";
            this.ePages.IsDirty = false;
            this.ePages.Location = new System.Drawing.Point(12, 581);
            this.resourceLookup1.SetLookup(this.ePages, new FWBS.OMS.UI.Windows.ResourceLookupItem("lbTotalNumPages", "Total No. Pages : ", ""));
            this.ePages.Name = "ePages";
            this.ePages.ReadOnly = true;
            this.ePages.Size = new System.Drawing.Size(251, 23);
            this.ePages.TabIndex = 3;
            this.ePages.Text = "Total No. Pages : ";
            this.ePages.Value = null;
            // 
            // eScans
            // 
            this.eScans.CaptionWidth = 150;
            this.eScans.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.eScans.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.eScans.Format = "";
            this.eScans.IsDirty = false;
            this.eScans.Location = new System.Drawing.Point(12, 604);
            this.resourceLookup1.SetLookup(this.eScans, new FWBS.OMS.UI.Windows.ResourceLookupItem("lbTotalNumScans", "Total No. Scans : ", ""));
            this.eScans.Name = "eScans";
            this.eScans.ReadOnly = true;
            this.eScans.Size = new System.Drawing.Size(251, 23);
            this.eScans.TabIndex = 0;
            this.eScans.Text = "Total No. Scans : ";
            this.eScans.Value = null;
            // 
            // ucNavPanelActions
            // 
            this.ucNavPanelActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucNavPanelActions.Controls.Add(this.ucNavActions);
            this.ucNavPanelActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucNavPanelActions.ExpandedHeight = 73;
            this.ucNavPanelActions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucNavPanelActions.HeaderColor = System.Drawing.Color.Empty;
            this.ucNavPanelActions.Location = new System.Drawing.Point(12, 261);
            this.ucNavPanelActions.LockOpenClose = true;
            this.resourceLookup1.SetLookup(this.ucNavPanelActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("PANELACTIONS", "Actions", ""));
            this.ucNavPanelActions.Name = "ucNavPanelActions";
            this.ucNavPanelActions.Size = new System.Drawing.Size(251, 73);
            this.ucNavPanelActions.TabIndex = 2;
            this.ucNavPanelActions.TabStop = false;
            this.ucNavPanelActions.Text = "Actions";
            this.ucNavPanelActions.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ucNavActions
            // 
            this.ucNavActions.Controls.Add(this.btnAction1);
            this.ucNavActions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavActions.Location = new System.Drawing.Point(0, 24);
            this.ucNavActions.Name = "ucNavActions";
            this.ucNavActions.Padding = new System.Windows.Forms.Padding(10);
            this.ucNavActions.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavActions.Resources = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.ucNavActions.Size = new System.Drawing.Size(251, 42);
            this.ucNavActions.TabIndex = 15;
            this.ucNavActions.TabStop = false;
            // 
            // btnAction1
            // 
            this.btnAction1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnAction1.ImageIndex = 2;
            this.btnAction1.Location = new System.Drawing.Point(10, 10);
            this.resourceLookup1.SetLookup(this.btnAction1, new FWBS.OMS.UI.Windows.ResourceLookupItem("SAVETOOMS", "Save To 3E MatterSphere", ""));
            this.btnAction1.Name = "btnAction1";
            this.btnAction1.Size = new System.Drawing.Size(231, 22);
            this.btnAction1.TabIndex = 5;
            this.btnAction1.Text = "Save To 3E MatterSphere";
            this.btnAction1.Click += new System.EventHandler(this.Actions_Click);
            // 
            // ucPanNavInfo
            // 
            this.ucPanNavInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanNavInfo.Controls.Add(this.ucNavRichText1);
            this.ucPanNavInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanNavInfo.ExpandedHeight = 58;
            this.ucPanNavInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanNavInfo.HeaderColor = System.Drawing.Color.Empty;
            this.ucPanNavInfo.Location = new System.Drawing.Point(12, 203);
            this.resourceLookup1.SetLookup(this.ucPanNavInfo, new FWBS.OMS.UI.Windows.ResourceLookupItem("ClientMatterInf", "Client & Matter Info", ""));
            this.ucPanNavInfo.Name = "ucPanNavInfo";
            this.ucPanNavInfo.Size = new System.Drawing.Size(251, 58);
            this.ucPanNavInfo.TabIndex = 1;
            this.ucPanNavInfo.TabStop = false;
            this.ucPanNavInfo.Text = "Client & Matter Info";
            this.ucPanNavInfo.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ucNavRichText1
            // 
            this.ucNavRichText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavRichText1.Location = new System.Drawing.Point(0, 24);
            this.ucNavRichText1.Name = "ucNavRichText1";
            this.ucNavRichText1.Padding = new System.Windows.Forms.Padding(3);
            this.ucNavRichText1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavRichText1.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Segoe UI;" +
    "}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs18\\par\r\n}\r\n";
            this.ucNavRichText1.Size = new System.Drawing.Size(251, 27);
            this.ucNavRichText1.TabIndex = 15;
            // 
            // ucPanelNavTop1
            // 
            this.ucPanelNavTop1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ucPanelNavTop1.Controls.Add(this.ucNavPanel1);
            this.ucPanelNavTop1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucPanelNavTop1.ExpandedHeight = 191;
            this.ucPanelNavTop1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNavTop1.HeaderBrightness = -10;
            this.ucPanelNavTop1.HeaderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ucPanelNavTop1.Image = ((System.Drawing.Image)(resources.GetObject("ucPanelNavTop1.Image")));
            this.ucPanelNavTop1.Location = new System.Drawing.Point(12, 12);
            this.resourceLookup1.SetLookup(this.ucPanelNavTop1, new FWBS.OMS.UI.Windows.ResourceLookupItem("FindClientFile", "Find Client & Matter", ""));
            this.ucPanelNavTop1.Name = "ucPanelNavTop1";
            this.ucPanelNavTop1.Size = new System.Drawing.Size(251, 191);
            this.ucPanelNavTop1.TabIndex = 0;
            this.ucPanelNavTop1.Text = "Find Client & Matter";
            this.ucPanelNavTop1.Theme = FWBS.Common.UI.Windows.ExtColorTheme.Auto;
            // 
            // ucNavPanel1
            // 
            this.ucNavPanel1.Controls.Add(this.labClientFileRef);
            this.ucNavPanel1.Controls.Add(this.pnlRef);
            this.ucNavPanel1.Controls.Add(this.labFile);
            this.ucNavPanel1.Controls.Add(this.cboFileList);
            this.ucNavPanel1.Controls.Add(this.chkAllFiles);
            this.ucNavPanel1.Controls.Add(this.panel3);
            this.ucNavPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavPanel1.Location = new System.Drawing.Point(0, 32);
            this.ucNavPanel1.Name = "ucNavPanel1";
            this.ucNavPanel1.Padding = new System.Windows.Forms.Padding(10);
            this.ucNavPanel1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavPanel1.Size = new System.Drawing.Size(251, 152);
            this.ucNavPanel1.TabIndex = 15;
            // 
            // labClientFileRef
            // 
            this.labClientFileRef.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labClientFileRef.Location = new System.Drawing.Point(10, 7);
            this.resourceLookup1.SetLookup(this.labClientFileRef, new FWBS.OMS.UI.Windows.ResourceLookupItem("ClientMatterRef", "Client && Matter Ref:", ""));
            this.labClientFileRef.Name = "labClientFileRef";
            this.labClientFileRef.Size = new System.Drawing.Size(231, 20);
            this.labClientFileRef.TabIndex = 0;
            this.labClientFileRef.Text = "Client && Matter Ref:";
            this.labClientFileRef.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlRef
            // 
            this.pnlRef.Controls.Add(this.txtClientNo);
            this.pnlRef.Controls.Add(this.btnGo);
            this.pnlRef.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlRef.Location = new System.Drawing.Point(10, 27);
            this.pnlRef.Name = "pnlRef";
            this.pnlRef.Size = new System.Drawing.Size(231, 24);
            this.pnlRef.TabIndex = 0;
            // 
            // txtClientNo
            // 
            this.txtClientNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtClientNo.Location = new System.Drawing.Point(0, 1);
            this.txtClientNo.Name = "txtClientNo";
            this.txtClientNo.Size = new System.Drawing.Size(199, 23);
            this.txtClientNo.TabIndex = 0;
            this.txtClientNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtClientNo_KeyPress);
            this.txtClientNo.Validated += new System.EventHandler(this.btnGo_Click);
            // 
            // btnGo
            // 
            this.btnGo.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGo.Location = new System.Drawing.Point(199, 0);
            this.resourceLookup1.SetLookup(this.btnGo, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnGo", "Go", ""));
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(32, 24);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Go";
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // labFile
            // 
            this.labFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labFile.Location = new System.Drawing.Point(10, 51);
            this.resourceLookup1.SetLookup(this.labFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("MatterList", "Matter List", ""));
            this.labFile.Name = "labFile";
            this.labFile.Size = new System.Drawing.Size(231, 20);
            this.labFile.TabIndex = 2;
            this.labFile.Text = "Matter List";
            this.labFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboFileList
            // 
            this.cboFileList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cboFileList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFileList.Location = new System.Drawing.Point(10, 71);
            this.cboFileList.Name = "cboFileList";
            this.cboFileList.Size = new System.Drawing.Size(231, 23);
            this.cboFileList.TabIndex = 2;
            this.cboFileList.SelectionChangeCommitted += new System.EventHandler(this.cboFileList_SelectionChangeCommitted);
            // 
            // chkAllFiles
            // 
            this.chkAllFiles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkAllFiles.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.chkAllFiles.Location = new System.Drawing.Point(10, 94);
            this.resourceLookup1.SetLookup(this.chkAllFiles, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkAllFiles", "Show all %FILES%", ""));
            this.chkAllFiles.Name = "chkAllFiles";
            this.chkAllFiles.Size = new System.Drawing.Size(231, 24);
            this.chkAllFiles.TabIndex = 3;
            this.chkAllFiles.Text = "Show all Matters";
            this.chkAllFiles.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnFind);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 118);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(231, 24);
            this.panel3.TabIndex = 4;
            // 
            // btnFind
            // 
            this.btnFind.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnFind.Location = new System.Drawing.Point(156, 0);
            this.resourceLookup1.SetLookup(this.btnFind, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnFind", "&Find", ""));
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 24);
            this.btnFind.TabIndex = 5;
            this.btnFind.Text = "&Find";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Group = "Main";
            this.btnPrev.ImageIndex = 17;
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.PanelButtonCaption = "";
            this.btnPrev.PanelButtonImageIndex = -1;
            this.btnPrev.PanelButtonVisible = false;
            this.btnPrev.ParentCode = null;
            // 
            // btnNext
            // 
            this.btnNext.Group = "Main";
            this.btnNext.ImageIndex = 18;
            this.btnNext.Name = "btnNext";
            this.btnNext.PanelButtonCaption = "";
            this.btnNext.PanelButtonImageIndex = -1;
            this.btnNext.PanelButtonVisible = false;
            this.btnNext.ParentCode = null;
            // 
            // btnSp1
            // 
            this.btnSp1.Group = "Main";
            this.btnSp1.Name = "btnSp1";
            this.btnSp1.PanelButtonCaption = "";
            this.btnSp1.PanelButtonImageIndex = -1;
            this.btnSp1.PanelButtonVisible = false;
            this.btnSp1.ParentCode = null;
            // 
            // btnRotateLeft
            // 
            this.btnRotateLeft.Group = "Main";
            this.btnRotateLeft.ImageIndex = 8;
            this.btnRotateLeft.Name = "btnRotateLeft";
            this.btnRotateLeft.PanelButtonCaption = "";
            this.btnRotateLeft.PanelButtonImageIndex = -1;
            this.btnRotateLeft.PanelButtonVisible = false;
            this.btnRotateLeft.ParentCode = null;
            // 
            // btnRotateRigth
            // 
            this.btnRotateRigth.Group = "Main";
            this.btnRotateRigth.ImageIndex = 9;
            this.btnRotateRigth.Name = "btnRotateRigth";
            this.btnRotateRigth.PanelButtonCaption = "";
            this.btnRotateRigth.PanelButtonImageIndex = -1;
            this.btnRotateRigth.PanelButtonVisible = false;
            this.btnRotateRigth.ParentCode = null;
            // 
            // eToolbars1
            // 
            this.eToolbars1.BottomDivider = false;
            this.eToolbars1.ButtonsXML = resources.GetString("eToolbars1.ButtonsXML");
            this.eToolbars1.Divider = false;
            this.eToolbars1.DropDownArrows = true;
            this.eToolbars1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.eToolbars1.ImageLists = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.eToolbars1.Location = new System.Drawing.Point(0, 0);
            this.eToolbars1.Name = "eToolbars1";
            this.eToolbars1.NavCommandPanel = this.navCommands;
            this.eToolbars1.PanelImageLists = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.eToolbars1.ShowToolTips = true;
            this.eToolbars1.Size = new System.Drawing.Size(977, 34);
            this.eToolbars1.TabIndex = 10;
            this.eToolbars1.TopDivider = false;
            this.eToolbars1.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.eToolbars1_ButtonClick);
            // 
            // ctmTagged
            // 
            this.ctmTagged.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuRedFlag,
            this.mnuBlueFlag,
            this.mnuYellowFlag,
            this.mnuGreenFlag,
            this.mnuGreyFlag,
            this.mnuPurpleFlag,
            this.mnuOffFlag});
            // 
            // mnuRedFlag
            // 
            this.mnuRedFlag.DefaultItem = true;
            this.mnuRedFlag.Index = 0;
            this.mnuRedFlag.MergeOrder = 66;
            this.mnuRedFlag.Text = "&Red Flag";
            this.mnuRedFlag.Click += new System.EventHandler(this.Flag_Clicked);
            // 
            // mnuBlueFlag
            // 
            this.mnuBlueFlag.Index = 1;
            this.mnuBlueFlag.MergeOrder = 67;
            this.mnuBlueFlag.Text = "&Blue Flag";
            this.mnuBlueFlag.Click += new System.EventHandler(this.Flag_Clicked);
            // 
            // mnuYellowFlag
            // 
            this.mnuYellowFlag.Index = 2;
            this.mnuYellowFlag.MergeOrder = 68;
            this.mnuYellowFlag.Text = "&Yellow Flag";
            this.mnuYellowFlag.Click += new System.EventHandler(this.Flag_Clicked);
            // 
            // mnuGreenFlag
            // 
            this.mnuGreenFlag.Index = 3;
            this.mnuGreenFlag.MergeOrder = 69;
            this.mnuGreenFlag.Text = "&Green Flag";
            this.mnuGreenFlag.Click += new System.EventHandler(this.Flag_Clicked);
            // 
            // mnuGreyFlag
            // 
            this.mnuGreyFlag.Index = 4;
            this.mnuGreyFlag.MergeOrder = 70;
            this.mnuGreyFlag.Text = "Gr&ey Flag";
            this.mnuGreyFlag.Click += new System.EventHandler(this.Flag_Clicked);
            // 
            // mnuPurpleFlag
            // 
            this.mnuPurpleFlag.Index = 5;
            this.mnuPurpleFlag.MergeOrder = 71;
            this.mnuPurpleFlag.Text = "P&urple Flag";
            this.mnuPurpleFlag.Click += new System.EventHandler(this.Flag_Clicked);
            // 
            // mnuOffFlag
            // 
            this.mnuOffFlag.Index = 6;
            this.mnuOffFlag.Text = "Off";
            this.mnuOffFlag.Click += new System.EventHandler(this.Flag_Clicked);
            // 
            // mnuSSp1
            // 
            this.mnuSSp1.Index = -1;
            this.mnuSSp1.Text = "-";
            // 
            // mnuFltRed
            // 
            this.mnuFltRed.Index = 0;
            this.mnuFltRed.RadioCheck = true;
            this.mnuFltRed.Text = "&Red Flag";
            this.mnuFltRed.Click += new System.EventHandler(this.FilterFlag_Clicked);
            // 
            // mnuFltBlue
            // 
            this.mnuFltBlue.Index = 1;
            this.mnuFltBlue.RadioCheck = true;
            this.mnuFltBlue.Text = "&Blue Flag";
            this.mnuFltBlue.Click += new System.EventHandler(this.FilterFlag_Clicked);
            // 
            // mnuFltYellow
            // 
            this.mnuFltYellow.Index = 2;
            this.mnuFltYellow.RadioCheck = true;
            this.mnuFltYellow.Text = "&Yellow Flag";
            this.mnuFltYellow.Click += new System.EventHandler(this.FilterFlag_Clicked);
            // 
            // mnuFltGreen
            // 
            this.mnuFltGreen.Index = 3;
            this.mnuFltGreen.RadioCheck = true;
            this.mnuFltGreen.Text = "&Green Flag";
            this.mnuFltGreen.Click += new System.EventHandler(this.FilterFlag_Clicked);
            // 
            // mnuFltGrey
            // 
            this.mnuFltGrey.Index = 4;
            this.mnuFltGrey.RadioCheck = true;
            this.mnuFltGrey.Text = "Gr&ey Flag";
            this.mnuFltGrey.Click += new System.EventHandler(this.FilterFlag_Clicked);
            // 
            // mnuFltPurple
            // 
            this.mnuFltPurple.Index = 5;
            this.mnuFltPurple.RadioCheck = true;
            this.mnuFltPurple.Text = "P&urple Flag";
            this.mnuFltPurple.Click += new System.EventHandler(this.FilterFlag_Clicked);
            // 
            // mnuFltOff
            // 
            this.mnuFltOff.Checked = true;
            this.mnuFltOff.Index = 6;
            this.mnuFltOff.RadioCheck = true;
            this.mnuFltOff.Text = "Off";
            this.mnuFltOff.Click += new System.EventHandler(this.FilterFlag_Clicked);
            // 
            // imgBig
            // 
            this.imgBig.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgBig.ImageStream")));
            this.imgBig.TransparentColor = System.Drawing.Color.Transparent;
            this.imgBig.Images.SetKeyName(0, "");
            this.imgBig.Images.SetKeyName(1, "");
            this.imgBig.Images.SetKeyName(2, "");
            this.imgBig.Images.SetKeyName(3, "");
            this.imgBig.Images.SetKeyName(4, "");
            this.imgBig.Images.SetKeyName(5, "");
            // 
            // timReturnImage
            // 
            this.timReturnImage.Interval = 1000;
            this.timReturnImage.Tick += new System.EventHandler(this.timReturnImage_Tick);
            // 
            // ghost
            // 
            this.ghost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ghost.Location = new System.Drawing.Point(168, 34);
            this.ghost.Name = "ghost";
            this.ghost.Size = new System.Drawing.Size(809, 639);
            this.ghost.TabIndex = 9;
            this.ghost.TabStop = false;
            this.ghost.Visible = false;
            // 
            // ctmFilter
            // 
            this.ctmFilter.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFltRed,
            this.mnuFltBlue,
            this.mnuFltYellow,
            this.mnuFltGreen,
            this.mnuFltGrey,
            this.mnuFltPurple,
            this.mnuFltOff});
            // 
            // ucScanning
            // 
            this.Controls.Add(this.ghost);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.eToolbars1);
            this.Name = "ucScanning";
            this.Size = new System.Drawing.Size(977, 673);
            this.Load += new System.EventHandler(this.ucScanning_Load);
            this.Controls.SetChildIndex(this.eToolbars1, 0);
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.Controls.SetChildIndex(this.ghost, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlCenter.ResumeLayout(false);
            this.pnlMiddle.ResumeLayout(false);
            this.pnlTagged.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picTagged)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlRight.ResumeLayout(false);
            this.ucPanelNav1.ResumeLayout(false);
            this.ucNavPanel2.ResumeLayout(false);
            this.ucNavPanelActions.ResumeLayout(false);
            this.ucNavActions.ResumeLayout(false);
            this.ucPanNavInfo.ResumeLayout(false);
            this.ucPanelNavTop1.ResumeLayout(false);
            this.ucNavPanel1.ResumeLayout(false);
            this.pnlRef.ResumeLayout(false);
            this.pnlRef.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ghost)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Properties
        public System.IO.DirectoryInfo ScanLocation
        {
            get
            {
                return _scanlocation;
            }
            set
            {
                _scanlocation = value;
            }
        }

        public System.IO.DirectoryInfo MoveLocation
        {
            get
            {
                return new System.IO.DirectoryInfo(_movepath);
            }
        }
        #endregion

        #region Public Methods

        #region "Refresh Scan Images"
        
        public void RefreshScanImages()
        {
            RefreshScanImages(true);
        }

        private string FormatAmount(int number, int total)
        {
            return string.Format("{0} {1} {2}", number, Session.CurrentSession.Resources.GetResource("OF", "of", "").Text, total);
        }

        public void RefreshScanImages(bool ResetPosition)
        {
            try
            {                
                _files = _scanlocation.GetFiles(_tifFilter);
                
                // CM - *** - Scan for Tif files as normal
                ArrayList foundFiles = new ArrayList();
                for (int i = 0; i < _files.Length; i++)
                {
                    System.IO.FileInfo f = _files[i];
                    foundFiles.Add(f);
                }

                // CM - *** - Additionally scan the converted folder for temporary files and add these as well
                DirectoryInfo tempLocation = new DirectoryInfo(Path.Combine(_scanlocation.FullName, "converted"));
                if (tempLocation.Exists)
                { 
                    FileInfo[] tempFiles = tempLocation.GetFiles(_tifFilter);
                    for (int i = 0; i < tempFiles.Length; i++)
                    {
                        System.IO.FileInfo f = tempFiles[i];

                        //Only add file if it is a temporary Tif file ( OR a flagged temporary Tif )
                        if (_filesConvertedToTif.ContainsKey(f.FullName.ToLower()))
                            foundFiles.Add(f);
                        else if (_tifFilter != "*.tif")
                        {
                            if (_tifFilter.IndexOf("(R)") > 0)  foundFiles.Add(f);
                            else if (_tifFilter.IndexOf("(B)") > 0)  foundFiles.Add(f);
                            else if (_tifFilter.IndexOf("(Y)") > 0)  foundFiles.Add(f);
                            else if (_tifFilter.IndexOf("(G)") > 0)  foundFiles.Add(f);
                            else if (_tifFilter.IndexOf("(A)") > 0)  foundFiles.Add(f);
                            else if (_tifFilter.IndexOf("(P)") > 0)  foundFiles.Add(f);
                        }
                                                
                    }
                }

                _files = (System.IO.FileInfo[])foundFiles.ToArray(typeof(System.IO.FileInfo));
           

                SortedList sorted = new SortedList();
                if (_tifFilter != "*.tif")
                {
                    string n = "";
                    for (int i = 0; i < _files.Length; i++)
                    {
                        System.IO.FileInfo f = _files[i];
                        if (_tifFilter.IndexOf("(R)") > 0) n = "(R)";
                        else if (_tifFilter.IndexOf("(B)") > 0) n = "(B)";
                        else if (_tifFilter.IndexOf("(Y)") > 0) n = "(Y)";
                        else if (_tifFilter.IndexOf("(G)") > 0) n = "(G)";
                        else if (_tifFilter.IndexOf("(A)") > 0) n = "(A)";
                        else if (_tifFilter.IndexOf("(P)") > 0) n = "(P)";
                        System.IO.FileInfo nf = new System.IO.FileInfo(f.FullName.Replace(n + f.Extension, ".tif"));
                        _files[i] = nf;
                    }
                }

                ArrayList d = new ArrayList();
                for (int i = 0; i < _files.Length; i++)
                {
                    System.IO.FileInfo f = _files[i];
                    DateTime creationTime = GetSourceImageCreationTime(f);
                    d.Add(creationTime);
                    sorted.Add(creationTime.ToOADate().ToString() + ":" + f.Name, f);
                }
                Calendar.BoldedDates = (DateTime[])d.ToArray(typeof(DateTime));

                int ii = 0;
                foreach (System.IO.FileInfo ff in sorted.Values)
                {
                    _files[ii] = new System.IO.FileInfo(ff.FullName.ToLower());
                    ii++;
                }

                d.Clear();
                if (chkApplyFilter.Checked)
                {
                    DateTime ds = new DateTime(Calendar.SelectionStart.Year, Calendar.SelectionStart.Month, Calendar.SelectionStart.Day, 0, 0, 0, 0);
                    DateTime de = new DateTime(Calendar.SelectionEnd.Year, Calendar.SelectionEnd.Month, Calendar.SelectionEnd.Day, 23, 59, 59, 0);
                    for (int i = 0; i < _files.Length; i++)
                    {
                        System.IO.FileInfo f = _files[i];
                        DateTime creationTime = GetSourceImageCreationTime(f);
                        if (creationTime >= ds && creationTime <= de)
                            d.Add(f);
                    }
                    _files = (System.IO.FileInfo[])d.ToArray(typeof(System.IO.FileInfo));
                }
                
                txtClientNo.Text = "";
                cboFileList.DataSource = null;
                ucNavRichText1.Text = "";
                ucNavRichText1.Refresh();
                ucNavPanelActions.ShrinkFast();
                ucPanNavInfo.ShrinkFast();
                ucNavPanelActions.LockOpenClose = true;

                pnlTagged.Visible = false;
                eScans.Value = FormatAmount(0, 0);
                ePages.Value = FormatAmount(0, 0);
                if (ResetPosition)
                    _scanposition = 0;

                OnImagesChanged();
                eToolbars1.GetButton("btnTagImage").Enabled = (this.ImageCount > 0);
                eToolbars1.GetButton("btnMoveTo").Enabled = (this.ImageCount > 0);
                eToolbars1.GetButton("btnAutoRead").Enabled = (this.ImageCount > 0);
                eToolbars1.GetButton("btnZoomIn").Enabled = (this.ImageCount > 0);
                eToolbars1.GetButton("btnZoomOut").Enabled = (this.ImageCount > 0);
                eToolbars2.GetButton("btnRotateLeft").Enabled = (this.ImageCount > 0);
                eToolbars2.GetButton("btnRotateRight").Enabled = (this.ImageCount > 0);

            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }
        
        #endregion

        #region "Loading of images"

        public void LoadImage(int Position)
        {
            this.m_ImageCore.ImageBuffer.RemoveAllImages();

            eToolbars1.GetButton("btnPrev").Enabled = !IsFirstImage;
            eToolbars1.GetButton("btnNext").Enabled = !IsLastImage;
            eToolbars2.GetButton("btnNextPg").Enabled = false;
            eToolbars2.GetButton("btnPrevPg").Enabled = false;
            this.scrollBar.Visible = false;
            labDate.Text = string.Empty;

            if (_files == null)
            {
                RefreshScanImages();
                if (_files == null) return;
            }

            if (_files.Length == 0)
            {
                return;
            }

            FWBS.OMS.AlertEventArgs args = new FWBS.OMS.AlertEventArgs(new FWBS.OMS.Alert[] { });
            OnAlert(args);

            txtClientNo.Text = "";
            cboFileList.DataSource = null;
            ucNavRichText1.Text = "";
            ucNavRichText1.Refresh();
            ucNavPanelActions.ShrinkFast();
            ucPanNavInfo.ShrinkFast();
            ucNavPanelActions.LockOpenClose = true;

            pnlTagged.Visible = _tagged;

            if (Position < 0) Position = 0;
            if (Position >= _files.Length) Position = _files.Length - 1;

            if (_files.Length > 0)
            {
                _tagged = false;
                this.Cursor = Cursors.WaitCursor;

                System.IO.FileInfo f = new System.IO.FileInfo(
                    _files[Position].FullName.Replace(
                        ".tif",
                        "(R)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials));
                if (f.Exists)
                {
                    _tagged = true;
                    picTagged.Image = imgBig.Images[0];
                }

                f = new System.IO.FileInfo(
                    _files[Position].FullName.Replace(
                        ".tif",
                        "(B)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials));
                if (f.Exists)
                {
                    _tagged = true;
                    picTagged.Image = imgBig.Images[1];
                }

                f = new System.IO.FileInfo(
                    _files[Position].FullName.Replace(
                        ".tif",
                        "(Y)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials));
                if (f.Exists)
                {
                    _tagged = true;
                    picTagged.Image = imgBig.Images[2];
                }

                f = new System.IO.FileInfo(
                    _files[Position].FullName.Replace(
                        ".tif",
                        "(G)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials));
                if (f.Exists)
                {
                    _tagged = true;
                    picTagged.Image = imgBig.Images[3];
                }

                f = new System.IO.FileInfo(
                    _files[Position].FullName.Replace(
                        ".tif",
                        "(A)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials));
                if (f.Exists)
                {
                    _tagged = true;
                    picTagged.Image = imgBig.Images[4];
                }

                f = new System.IO.FileInfo(
                    _files[Position].FullName.Replace(
                        ".tif",
                        "(P)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials));
                if (f.Exists)
                {
                    _tagged = true;
                    picTagged.Image = imgBig.Images[5];
                }

                pnlTagged.Visible = _tagged;

                this.dsViewer.Visible = false;
                m_ImageCore.IO.LoadImage(_files[Position].FullName);
                this.m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer = 0;
                Bitmap bmp = this.m_ImageCore.ImageBuffer.GetBitmap(this.m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer);
                if (this.m_ImageCore.ImageBuffer.HowManyImagesInBuffer > 1)
                {
                    this.EnablePages();
                    if (this.dsViewer.Height >= bmp.Height * this.dsViewer.Zoom - 1)
                    {
                        this.scrollBar.Visible = true;
                    }

                    this.scrollBar.Maximum = this.m_ImageCore.ImageBuffer.HowManyImagesInBuffer - 1;
                }

                if (eToolbars1.GetButton("btnFitToWidth").Pushed)
                {
                    dsViewer.IfFitWindow = true;
                    this.dsViewer.FitWindowType = EnumFitWindowType.enumFitWindowWidth;
                    if (bmp.Width * this.dsViewer.Zoom < this.dsViewer.Width)
                    {
                        this.dsViewer.Zoom = (float)this.dsViewer.Width / bmp.Width;
                    }

                    this.dsViewer.Visible = true;
                    this.CheckZoom();
                }
                else
                {
                    dsViewer.IfFitWindow = true;
                    this.dsViewer.FitWindowType = EnumFitWindowType.enumFitWindowHeight;
                    if (bmp.Height * this.dsViewer.Zoom < this.dsViewer.Height)
                    {
                        this.dsViewer.Zoom = (float)this.dsViewer.Height / bmp.Height;
                    }
                    this.dsViewer.Visible = true;
                    this.CheckZoom();
                }

                _scanposition = Position;
                labDate.Text = ImageSourceFile().CreationTime.ToShortDateString();
                eScans.Value = FormatAmount(_scanposition + 1, _files.Length);
                ieMulti1_SelectImage();
                this.Cursor = Cursors.Default;
                OCRDocumentNow();
            }
        }

        public void NextImage()
        {
            _scanposition++;
            LoadImage(_scanposition);
        }

        public void PreviousImage()
        {
            _scanposition--;
            LoadImage(_scanposition);
        }

        #endregion

        #region "Copy To Clipboard"
        public void CopyTextToClipboard()
        {
            try
            {
                string text = (this.selectedOcr != null) ? System.Text.Encoding.UTF8.GetString(this.selectedOcr) : null;
                if (!string.IsNullOrEmpty(text))
                    Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        public void CopyImageToClipboard()
        {
            try
            {
                short index = m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer;
                if (index >= 0 && index < m_ImageCore.ImageBuffer.HowManyImagesInBuffer)
                {
                    Bitmap bitmap = m_ImageCore.ImageBuffer.GetBitmap(index);
                    Clipboard.SetImage(bitmap);
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        public void CopyFileToClipboard()
        {
            try
            {
                var paths = new System.Collections.Specialized.StringCollection();
                paths.Add(ImageSourceFile().FullName);
                Clipboard.SetFileDropList(paths);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }
        #endregion

        #region "Public properties"
        public int ImageCount
        {
            get
            {
                return _files.Length;
            }
        }

        public bool IsFirstImage
        {
            get
            {
                return _scanposition <= 0;
            }
        }

        public bool IsLastImage
        {
            get
            {
                return _files.Length == 0 ? true : (_scanposition == _files.Length - 1);
            }
        }

        public string CurrentUserFolderName
        {
            get
            {
                return _username;
            }
        }

        private int CurrentUserID = 0;
        #endregion

        #region "Change Folder"
        public void ChangeFolder()
        {
            bool doneavailable = false;
            try
            {
                try
                {
                    object on = FWBS.OMS.Session.CurrentSession.CurrentUser.GetExtraInfo("usrImageFolderDone");
                    doneavailable = true;
                }
                catch
                {
                }
                object n = FWBS.OMS.Session.CurrentSession.CurrentUser.GetExtraInfo("usrImageFolder");
                try
                {
                    FWBS.OMS.ReportingServer obj = new FWBS.OMS.ReportingServer("FWBS Limited 2005");
                    DataTable tbl = null;
                    tbl = obj.Connection.ExecuteSQLTable("SELECT usrID, usrFullName FROM DBUser WHERE NOT usrImageFolder is null AND usrActive = 1 ORDER BY usrFullName", "USERS", false, null);
                    FWBS.OMS.UI.Windows.Design.frmListSelector frm = new FWBS.OMS.UI.Windows.Design.frmListSelector();
                    frm.Text = Session.CurrentSession.Resources.GetResource("SELUSERFOLDER", "Select User Folder", "").Text;
                    frm.List.DataSource = tbl;
                    frm.List.DisplayMember = "usrFullName";
                    frm.List.ValueMember = "usrID";
                    frm.List.SelectedValue = CurrentUserID;
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        _files = new System.IO.FileInfo[0];
                        User u = User.GetUser(Convert.ToInt32(frm.List.SelectedValue));
                        this.ScanLocation = new System.IO.DirectoryInfo(Convert.ToString(u.GetExtraInfo("usrImageFolder")).TrimEnd('\\'));
                        _movepath = "";
                        if (doneavailable)
                            _movepath = Convert.ToString(u.GetExtraInfo("usrImageFolderDone")).TrimEnd('\\');
                        if (_movepath == "") _movepath = this.ScanLocation.FullName + "\\done";
                        _username = frm.List.Text;
                        CurrentUserID = u.ID;

                        this.Hide();
                        ConvertSupportedFilesToTif();
                        this.RefreshScanImages();
                        this.Show();

                        this.LoadImage(0);
                    }
                }
                catch (Exception ex)
                {
                    FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                }
            }
            catch
            {
                fldMoveTo.Description = "Please select the Home Folder for the Images to Process.";
                fldMoveTo.SelectedPath = this.ScanLocation.FullName;
                if (fldMoveTo.ShowDialog() == DialogResult.OK)
                {
                    _files = new System.IO.FileInfo[0];
                    FWBS.Common.RegistryAccess.SetSetting("", Microsoft.Win32.RegistryHive.CurrentUser, @"\Software\FWBS\OMS\2.0\OMSDocumentImporter", "Location", fldMoveTo.SelectedPath);
                    this.ScanLocation = new System.IO.DirectoryInfo(fldMoveTo.SelectedPath);
                    this.Connect(FWBS.OMS.Session.CurrentSession.CurrentUser);
                    this.RefreshScanImages();
                    this.LoadImage(0);
                }
            }
        }
        #endregion
        
        #region "Move Image To Other User"
        public void MoveImageTo()
        {
            try
            {
                object n = FWBS.OMS.Session.CurrentSession.CurrentUser.GetExtraInfo("usrImageFolder");
  
                try
                {
                    FWBS.OMS.ReportingServer obj = new FWBS.OMS.ReportingServer("FWBS Limited 2005");
                    DataTable tbl = obj.Connection.ExecuteSQLTable("SELECT usrImageFolder, usrFullName FROM DBUser WHERE NOT usrImageFolder is null AND usrActive=1 ORDER BY usrFullName", "USERS", false, null);
                    n = tbl.Rows[0]["usrImageFolder"];
                    FWBS.OMS.UI.Windows.Design.frmListSelector frm = new FWBS.OMS.UI.Windows.Design.frmListSelector();
                    frm.Text = Session.CurrentSession.Resources.GetResource("SELUSERFOLDER", "Select User Folder", "").Text;
                    frm.List.Sorted = true;
                    frm.List.DataSource = tbl;
                    frm.List.DisplayMember = "usrFullName";
                    frm.List.ValueMember = "usrImageFolder";
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        FileInfo fileToMove = ImageSourceFile();
                        
                        System.IO.DirectoryInfo dn = new System.IO.DirectoryInfo(Convert.ToString(frm.List.SelectedValue));
                        dn.Create();
                        fileToMove.MoveTo(Path.Combine(dn.FullName, fileToMove.Name));
                        
                        DeleteTemporaryTifIfExists();

                        RefreshScanImages(false);
                        LoadImage(_scanposition);
                    }
                    frm.Dispose();
                }
                catch (Exception ex)
                {
                    FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                }
            }
            catch
            {
                try
                {
                    FileInfo fileToMove = ImageSourceFile();

                    fldMoveTo.Description = "Please select the Destination Folder to move the Image Too.";
                    fldMoveTo.SelectedPath = fileToMove.DirectoryName;
                    if (fldMoveTo.ShowDialog() == DialogResult.OK)
                    {
                        fileToMove.MoveTo(Path.Combine(fldMoveTo.SelectedPath, fileToMove.Name));

                        DeleteTemporaryTifIfExists();

                        RefreshScanImages(false);
                        LoadImage(_scanposition);
                    }
                }
                catch (Exception ex)
                {
                    FWBS.OMS.UI.Windows.ErrorBox.Show(ex);

                }
            }
        }
        #endregion

        #region "Conversions To Tif"
        
        public void ConvertSupportedFilesToTif()
        {
            //CM - Step 1. Get supported files that can be converted
            string[] _supportedFileTypes = { "*.gif", "*.png", "*.bmp", "*.jpg", "*.jpeg", "*.pdf" };

            ArrayList foundFiles = new ArrayList();
            foreach (string fileType in _supportedFileTypes)
            {
                System.IO.FileInfo[] fi = _scanlocation.GetFiles(fileType);

                for (int i = 0; i < fi.Length; i++)
                {
                    System.IO.FileInfo f = fi[i];
                    foundFiles.Add(f);
                }
            }

            //CM - Step 2. Generate new file names and convert to tif ... IF we have found some supported files
            if (foundFiles.Count > 0)
            {
                frmConvertProgress frm = new frmConvertProgress();

                Dictionary<ArrayList, DirectoryInfo> dic = new Dictionary<ArrayList, DirectoryInfo>();
                dic.Add(foundFiles, _scanlocation);
                frm.Tag = dic;
                frm.ShowDialog();

                _filesConvertedToTif = (Dictionary<string, string>)frm.Tag;
                frm.Dispose();
            }
        }

        #endregion
        
        #region "Delete Temporary Tifs"

        public void DeleteTemporaryTifs()
        {
            if (hasTemporaryTifs())
            {
                System.Diagnostics.Debug.WriteLine("Temporary Tifs found", Application.SafeTopLevelCaptionFormat);
                try
                {
                    foreach (string key in _filesConvertedToTif.Keys)
                    {
                        new FileInfo(key).Delete();
                    }
                    System.Diagnostics.Debug.WriteLine("Temporary Tifs deleted", Application.SafeTopLevelCaptionFormat);
                }
                catch (Exception ex)
                {
                    FWBS.OMS.UI.Windows.MessageBox.Show(ex);
                }
            }
        }

        #endregion

        #region "Has Temporary Tifs"
        public bool hasTemporaryTifs()
        {
            return (_filesConvertedToTif != null && _filesConvertedToTif.Count > 0);
        }
        #endregion

        #endregion

        #region "Non-Public Methods"

        #region "Disposal Methods"

        internal void CleanUpRoutineOnExitOfScanningApplication()
        {
            DeleteTemporaryTifs();
        }

        #endregion

        #region OCR Methods
        private void OCRDocumentNow()
        {
            _documentcontent = string.Empty;
            if (eToolbars1.GetButton("btnAutoRead").Pushed)
            {
                try
                {
                    this.ParentForm.Enabled = false;
                    this.Cursor = Cursors.AppStarting;

                    if (m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer < 0)
                    {
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation(
                            "IMGNOTLOADED",
                            "Index out of bounds: please load an image before doing OCR!");
                        return;
                    }

                    List<short> tempListSelectedIndex = dsViewer.CurrentSelectedImageIndicesInBuffer;
                    List<Bitmap> tempListSelectedBitmap = null;
                    foreach (short index in tempListSelectedIndex)
                    {
                        if (index >= 0 && index < m_ImageCore.ImageBuffer.HowManyImagesInBuffer)
                        {
                            if (tempListSelectedBitmap == null)
                            {
                                tempListSelectedBitmap = new List<Bitmap>();
                            }
                            Bitmap temp = m_ImageCore.ImageBuffer.GetBitmap(index);
                            tempListSelectedBitmap.Add(temp);
                        }
                    }

                    if (tempListSelectedBitmap != null)
                    {
                        byte[] sbytes = m_Tesseract.Recognize(tempListSelectedBitmap);
                        _documentcontent = System.Text.Encoding.UTF8.GetString(sbytes);
                    }

                    eProc.Value = "100 %";
                    
                    string delimiter = FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting(
                        "/config/clientSearch/clientDelimiter",
                        "-./:");
                    string pattern = Convert
                        .ToString(
                            FWBS.OMS.Session.CurrentSession.GetXmlProperty(
                                "ocrregex",
                                @"([a-z]\d{1,15}[%delimiter%]\d{1,15})")).Replace("%delimiter%", delimiter);
                    Regex search = new Regex(pattern, RegexOptions.IgnoreCase);
                    Match results = search.Match(_documentcontent);
                    if (results.Success)
                        txtClientNo.Text = results.Value;
                    else
                    {
                        pattern = @"(\d{1,15}[" + delimiter + @"]\d{1,15})";
                        search = new Regex(pattern, RegexOptions.IgnoreCase);
                        results = search.Match(_documentcontent);
                        if (results.Success)
                            txtClientNo.Text = results.Value;
                    }
                }
                catch (Exception ex)
                {
                    FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                    this.ParentForm.Enabled = true;
                }
            }
        }

        private void OCRSelectedArea(int left, int top, int right, int bottom)
        {
            try
            {
                this.selectedOcr = m_Tesseract.Recognize(m_ImageCore.ImageBuffer.GetBitmap(m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer),
                    left, top, right, bottom);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        private void RefreshActions()
        {
            OMS.Extensibility.OMSAddin addin = Session.CurrentSession.Addins[new Guid("8d175057-2e8a-452b-ac24-8905c2086269")];
            if (addin != null)
            {
                if (addin.Status == FWBS.OMS.Extensibility.AddinStatus.Loaded)
                {

                    Assembly ass = addin.Instance.GetType().Assembly;
                    Type t = ass.GetType("FWBS.OMS.FileManagement.Addins.Tasks");
                    if (t != null)
                    {
                        MethodInfo m = t.GetMethod("RefreshActions", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(OMSFile), typeof(FWBS.OMS.UI.Windows.ucPanelNav), typeof(FWBS.OMS.UI.Windows.ucNavPanel) }, null);
                        if (m != null)
                            m.Invoke(null, new object[] { _omsfile, ucNavPanelActions, ucNavActions });
                    }
                }
            }
        }

        private void eToolbars1_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            try
            {
                if (e.Button.Name == "btnRefresh")
                {
                    RefreshScanImages();
                    LoadImage(_scanposition);
                }
                else if (e.Button.Name == "btnFitToWidth")
                {
                    this.dsViewer.Visible = false;
                    dsViewer.IfFitWindow = true;
                    this.dsViewer.FitWindowType = EnumFitWindowType.enumFitWindowWidth;
                    if (this.m_ImageCore.ImageBuffer.GetBitmap(this.m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer).Width * this.dsViewer.Zoom < this.dsViewer.Width)
                    {
                        this.dsViewer.Zoom = (float)this.dsViewer.Width / this.m_ImageCore.ImageBuffer
                                                 .GetBitmap(this.m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer)
                                                 .Width;
                    }
                    this.dsViewer.Visible = true;
                    this.CheckZoom();
                    eToolbars1.GetButton("btnFitToWidth").Pushed = true;
                    eToolbars1.GetButton("btnFitToPage").Pushed = false;
                }
                else if (e.Button.Name == "btnFitToPage")
                {
                    this.dsViewer.Visible = false;
                    dsViewer.IfFitWindow = true;
                    this.dsViewer.FitWindowType = EnumFitWindowType.enumFitWindowHeight;
                    if (this.m_ImageCore.ImageBuffer.GetBitmap(this.m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer).Height * this.dsViewer.Zoom < this.dsViewer.Height)
                    {
                        this.dsViewer.Zoom = (float)this.dsViewer.Height / this.m_ImageCore.ImageBuffer
                                                 .GetBitmap(this.m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer)
                                                 .Height;
                    }
                    this.dsViewer.Visible = true;
                    this.CheckZoom();
                    eToolbars1.GetButton("btnFitToWidth").Pushed = false;
                    eToolbars1.GetButton("btnFitToPage").Pushed = true;
                }
                else if (e.Button.Name == "btnNextPg")
                {
                    if (m_ImageCore.ImageBuffer.HowManyImagesInBuffer > 0
                        && m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer
                        < m_ImageCore.ImageBuffer.HowManyImagesInBuffer - 1)
                    {
                        ++m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer;
                        ++this.scrollBar.Value;
                    }

                    ieMulti1_SelectImage();
                }
                else if (e.Button.Name == "btnPrevPg")
                {
                    if (m_ImageCore.ImageBuffer.HowManyImagesInBuffer > 0
                        && m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer > 0)
                    {
                        --m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer;
                        --this.scrollBar.Value;
                    }

                    ieMulti1_SelectImage();
                }
                else if (e.Button.Name == "btnRotateLeft")
                {
                    m_ImageCore.ImageProcesser.RotateLeft(m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer);
                }
                else if (e.Button.Name == "btnRotateRight")
                {
                    m_ImageCore.ImageProcesser.RotateRight(m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer);
                }
                else if (e.Button.Name == "btnZoomIn")
                {
                    float zoom = dsViewer.Zoom + 0.1F;
                    dsViewer.Zoom = zoom;
                    this.CheckZoom();
                }
                else if (e.Button.Name == "btnZoomOut")
                {
                    float zoom = dsViewer.Zoom - 0.1F;
                    dsViewer.Zoom = zoom;
                    this.CheckZoom();
                }
                else if (e.Button.Name == "btnNext")
                {
                    NextImage();
                }
                else if (e.Button.Name == "btnPrev")
                {
                    PreviousImage();
                }
                else if (e.Button.Name == "btnTagImage")
                {
                    Flag_Clicked(null, EventArgs.Empty);
                }
                else if (e.Button.Name == "btnAutoRead")
                {
                    OCRDocumentNow();
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void ieMulti1_SelectImage()
        {
            ePages.Value = FormatAmount(m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer + 1, m_ImageCore.ImageBuffer.HowManyImagesInBuffer);
            this.EnablePages();
        }

        private void btnGo_Click(object sender, System.EventArgs e)
        {
            if (txtClientNo.Text == "" || !Session.CurrentSession.IsConnected) return;
            // Parse the clientno and split
            string[] splitstr = new string[2];
            char[] delimiter;
            //TODO: ConfigSetting-/ClientSearch/ClientDelimiter
            delimiter = FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("/config/clientSearch/clientDelimiter", " ./:-").ToCharArray();
            try
            {
                txtClientNo.Text = txtClientNo.Text.Trim();
                splitstr = txtClientNo.Text.Split(delimiter, 2);
                txtClientNo.Text = splitstr[0];
                cboFileList.Tag = splitstr[1];
            }
            catch
            {
                txtClientNo.Text = splitstr[0];
                cboFileList.Tag = null;
            }
            try
            {
                _client = FWBS.OMS.Client.GetClient(txtClientNo.Text);
                if (FWBS.OMS.Session.CurrentSession.CurrentFileGatheredFromAlternative)
                {	// File information gathered from alternative FILE Need to populate
                    // the combo box to the 
                    cboFileList.Tag = FWBS.OMS.Session.CurrentSession.CurrentFile.FileNo;
                    FWBS.OMS.Session.CurrentSession.CurrentFileGatheredFromAlternative = false;
                }

                if (chkAllFiles.Checked)
                    _fileview = _client.GetDataView("FILES", "", "created");
                else
                    _fileview = _client.GetDataView("FILES", "filestatus = 'LIVE'", "created");


                if (cboFileList.Items.Count == 0)
                {
                    _fileview = _client.GetDataView("FILES", "", "created");
                    chkAllFiles.Checked = true;
                }

                cboFileList.BeginUpdate();
                cboFileList.DataSource = _fileview;
                cboFileList.DisplayMember = "fileJointDesc";
                cboFileList.ValueMember = "fileNo";
                if (cboFileList.Items.Count > 0)
                    cboFileList.SelectedIndex = 0;
                cboFileList.EndUpdate();
                if (cboFileList.Tag == null)
                    cboFileList.Tag = cboFileList.SelectedValue;
                if (cboFileList.Items.Count > 0)
                    cboFileList_SelectionChangeCommitted(sender, e);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                ucNavRichText1.Text = "";
                ucNavRichText1.Refresh();
                ucPanNavInfo.Expanded = false;
                txtClientNo.Text = "";
                cboFileList.DataSource = null;
                txtClientNo.Focus();
                ucNavPanelActions.LockOpenClose = true;
                ucNavPanelActions.Expanded = false;
            }
        }

        private void cboFileList_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            try
            {
                FWBS.OMS.ResourceItem res_fnf = FWBS.OMS.Session.CurrentSession.Resources.GetResource("OMSNFSF", "There are no %FILES% available for %CLIENT% No : %1%", "", txtClientNo.Text);

                // File No is specified within the clientno and stored in the tag so try
                // and select the additional File No from the Combo
                if (cboFileList.Tag != null)
                {
                    cboFileList.SelectedValue = cboFileList.Tag;
                    if (cboFileList.SelectedIndex == -1)
                    {
                        if (cboFileList.Items.Count > 0)
                        {
                            cboFileList.SelectedIndex = 0;
                            FWBS.OMS.UI.Windows.MessageBox.Show(res_fnf, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            txtClientNo.Focus();
                        }
                    }
                    else
                    {
                        _omsfile = _client.FindFile(Convert.ToString(cboFileList.Tag), chkAllFiles.Checked);
                        ucPanNavInfo.Expanded = true;
                        ucNavRichText1.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(_omsfile.FileClientDescription);
                        ucNavRichText1.Refresh();
                        ucNavPanelActions.Expanded = true;
                        ucNavActions.Refresh();
                        ucNavPanelActions.LockOpenClose = false;
                    }
                    cboFileList.Tag = null;
                }
                else
                {
                    _omsfile = _client.FindFile(Convert.ToString(cboFileList.SelectedValue), chkAllFiles.Checked);
                    ucPanNavInfo.Expanded = true;
                    ucNavRichText1.Rtf = FWBS.Common.Utils.GetRtfUnicodeEscapedString(_omsfile.FileClientDescription);
                    ucNavRichText1.Refresh();
                    ucNavPanelActions.Expanded = true;
                    ucNavActions.Refresh();
                    ucNavPanelActions.LockOpenClose = false;
                }
                if (cboFileList.DataSource != null)
                {
                    FWBS.OMS.AlertEventArgs args = new FWBS.OMS.AlertEventArgs(_client.FileAlerts(_fileview[cboFileList.SelectedIndex]));
                    OnAlert(args);
                }
                ucPanelNav1.ShrinkFast();
                RefreshActions();
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void txtClientNo_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
                btnGo_Click(sender, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the alert event.
        /// </summary>
        protected void OnAlert(FWBS.OMS.AlertEventArgs args)
        {
            if (Alert != null)
                Alert(this, args);
        }

        private void ucScanning_Load(object sender, System.EventArgs e)
        {
            ucPanNavInfo.ShrinkFast();
        }

        private Image CaptureScreen()
        {
            Image MyImage = null;
            using (Graphics g1 = CreateGraphics())
            {
                MyImage = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height, g1);
                using (Graphics g2 = Graphics.FromImage(MyImage))
                {
                    IntPtr dc1 = g1.GetHdc();
                    IntPtr dc2 = g2.GetHdc();
                    BitBlt(dc2, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, dc1, 0, 5 + this.eToolbars1.Height, 13369376);
                    g1.ReleaseHdc(dc1);
                    g2.ReleaseHdc(dc2);
                }
            }
            return MyImage;
        }

        private bool SaveToOMS()
        {
            bool result = false;
            if (_files.Length > 0)
            {
                ghost.Image = CaptureScreen();
                ghost.Visible = true;
                try
                {
                    OMSScan _appcontroller = new OMSScan(_documentcontent);
                    FWBS.OMS.Apps.ApplicationManager.CurrentManager.InitialiseInstance("SHELL", _appcontroller);
                    _appcontroller.SetDocVariable(_appcontroller, FWBS.OMS.UI.Windows.OMSApp.CLIENT, _client.ClientID);
                    _appcontroller.SetDocVariable(_appcontroller, FWBS.OMS.UI.Windows.OMSApp.FILE, _omsfile.ID);

                    FWBS.OMS.UI.Windows.ShellFile n = new FWBS.OMS.UI.Windows.ShellFile(ImageSourceFile());
                    result = _appcontroller.SaveAs(n, false);
                    _appcontroller.Close(n);

                    if (result)
                    {
                        AddScannedActivityToDocumentLog(n);
                    }
                }
                finally
                {
                    ghost.Visible = false;
                    ghost.Image.Dispose();
                    ghost.Image = null;
                }
                return result;
            }
            else
            {
                throw new OMSException2("ERRNOFILE", "Error No Image to Save ...");
            }
        }

        private void AddScannedActivityToDocumentLog(OMS.UI.Windows.ShellFile n)
        {
            FWBS.OMS.OMSDocument scannedDocument = FWBS.OMS.UI.Windows.OMSApp.GetDocument(n.File);
            if (scannedDocument != null)
            {
                scannedDocument.GetLatestVersion().AddActivity("SCANNED", null);
                scannedDocument.Update();
            }
            else
            {
                throw new OMSException2("ERRSCANDOCLOG", "Error adding scanning activity to the document history");
            }
        }

        private FileInfo ImageSourceFile()
        {
            FileInfo n = null;

            //If we find that this is a 'temporary' tif file - we want the original source file instead
            if (CurrentImageIsTemporaryTif())
            {
                string sourceFile;
                _filesConvertedToTif.TryGetValue(_files[_scanposition].FullName.ToLower(), out sourceFile);
                n = new FileInfo(sourceFile);
            }
            else
            {
                n = _files[_scanposition];
            }
            return n;
        }

        private bool CurrentImageIsTemporaryTif()
        {
            return _filesConvertedToTif.ContainsKey(_files[_scanposition].FullName.ToLower());
        }

        DateTime GetSourceImageCreationTime(FileInfo f)
        {
            string sourceFile;
            if (_filesConvertedToTif.TryGetValue(f.FullName.ToLower(), out sourceFile))
                f = new FileInfo(sourceFile);

            return f.CreationTime;
        }

        private void DeleteFlaggedFiles(string mainFileName, string userInitials)
        {
            foreach (char flag in new char[] { 'R', 'B', 'Y', 'G', 'A', 'P' })
            {
                File.Delete(mainFileName.Replace(".tif", $"({flag}).{userInitials}"));
            }
        }

        private void Actions_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (SaveToOMS())
                {
                    //Remove any flags against this file
                    DeleteFlaggedFiles(_files[_scanposition].FullName, Session.CurrentSession.CurrentUser.Initials);

                    FileInfo fileSaved = ImageSourceFile();

                    switch (_results)
                    {
                        case SaveSettingsResults.ssrMove:
                            {
                                bool success = false;
                                do
                                {
                                    try
                                    {
                                        System.IO.DirectoryInfo n = new System.IO.DirectoryInfo(_movepath);
                                        n.Create();
                                        fileSaved.CopyTo(Path.Combine(n.FullName, fileSaved.Name), true);
                                        fileSaved.Delete();

                                        DeleteTemporaryTifIfExists();
                                        success = true;
                                    }
                                    catch
                                    {
                                        if (FWBS.OMS.UI.Windows.MessageBox.ShowInformation("FILELOCKED", "Error moving file locked. Please close any programs that may have the ''%1%'' open", fileSaved.FullName) == DialogResult.Cancel)
                                            success = true;
                                    }
                                }
                                while (success == false);

                                break;
                            }
                        case SaveSettingsResults.ssrDelete:
                            {
                                fileSaved.Delete();
                                DeleteTemporaryTifIfExists();
                                break;
                            }
                    }
                }
                RefreshScanImages(false);
                LoadImage(_scanposition);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void DeleteTemporaryTifIfExists()
        {
            if (CurrentImageIsTemporaryTif())
            {
                _files[_scanposition].Delete();
                _filesConvertedToTif.Remove(_files[_scanposition].FullName.ToLower());
            }
        }
        
        private void ParentForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Right && e.Control)
                    NextImage();
                else if (e.KeyCode == Keys.Left && e.Control)
                    PreviousImage();
                else if (e.KeyCode == Keys.Down && e.Control)
                {
                    if (m_ImageCore.ImageBuffer.HowManyImagesInBuffer > 0 &&
                        m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer < m_ImageCore.ImageBuffer.HowManyImagesInBuffer - 1)
                        ++m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer;

                    ieMulti1_SelectImage();
                }
                else if (e.KeyCode == Keys.Up && e.Control)
                {
                    if (m_ImageCore.ImageBuffer.HowManyImagesInBuffer > 0 && m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer > 0)
                        --m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer;

                    ieMulti1_SelectImage();
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        protected virtual void OnImagesChanged()
        {
            if (ChangeImaged != null)
                ChangeImaged(this, System.EventArgs.Empty);
        }

        private void picTagged_MouseMove_1(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            pnlTagged.Visible = false;
            timReturnImage.Enabled = true;
        }

        private void Flag_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                if (_files.Length > 0)
                {
                    var btnTagImage = eToolbars1.GetButton("btnTagImage");
                    string mainFileName = _files[_scanposition].FullName;
                    string userInitials = Session.CurrentSession.CurrentUser.Initials;
                    DeleteFlaggedFiles(mainFileName, userInitials);

                    if (sender != null && ((MenuItem)sender).MergeOrder != 0)
                    {
                        ((MenuItem)sender).DefaultItem = true;
                        btnTagImage.ImageIndex = ((MenuItem)sender).MergeOrder;
                    }

                    if (sender == mnuOffFlag)
                    {
                        _tagged = false;
                        pnlTagged.Visible = _tagged;
                    }
                    else if (sender == mnuRedFlag || btnTagImage.ImageIndex == 66)
                    {
                        using (FileStream f = File.Create(mainFileName.Replace(".tif", "(R)." + userInitials)))
                        {
                            _tagged = true;
                            picTagged.Image = imgBig.Images[0];
                            pnlTagged.Visible = _tagged;
                        }
                    }
                    else if (sender == mnuBlueFlag || btnTagImage.ImageIndex == 67)
                    {
                        using (FileStream f = File.Create(mainFileName.Replace(".tif", "(B)." + userInitials)))
                        {
                            _tagged = true;
                            picTagged.Image = imgBig.Images[1];
                            pnlTagged.Visible = _tagged;
                        }
                    }
                    else if (sender == mnuYellowFlag || btnTagImage.ImageIndex == 68)
                    {
                        using (FileStream f = File.Create(mainFileName.Replace(".tif", "(Y)." + userInitials)))
                        {
                            _tagged = true;
                            picTagged.Image = imgBig.Images[2];
                            pnlTagged.Visible = _tagged;
                        }
                    }
                    else if (sender == mnuGreenFlag || btnTagImage.ImageIndex == 69)
                    {
                        using (FileStream f = File.Create(mainFileName.Replace(".tif", "(G)." + userInitials)))
                        {
                            _tagged = true;
                            picTagged.Image = imgBig.Images[3];
                            pnlTagged.Visible = _tagged;
                        }
                    }
                    else if (sender == mnuGreyFlag || btnTagImage.ImageIndex == 70)
                    {
                        using (FileStream f = File.Create(mainFileName.Replace(".tif", "(A)." + userInitials)))
                        {
                            _tagged = true;
                            picTagged.Image = imgBig.Images[4];
                            pnlTagged.Visible = _tagged;
                        }
                    }
                    else if (sender == mnuPurpleFlag || btnTagImage.ImageIndex == 71)
                    {
                        using (FileStream f = File.Create(mainFileName.Replace(".tif", "(P)." + userInitials)))
                        {
                            _tagged = true;
                            picTagged.Image = imgBig.Images[5];
                            pnlTagged.Visible = _tagged;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void FilterFlag_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                _lastfilter.Checked = false;
                ((MenuItem)sender).Checked = true;
                if (sender == mnuFltRed)
                    _tifFilter = "*(R)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials;
                else if (sender == mnuFltBlue)
                    _tifFilter = "*(B)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials;
                else if (sender == mnuFltYellow)
                    _tifFilter = "*(Y)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials;
                else if (sender == mnuFltGreen)
                    _tifFilter = "*(G)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials;
                else if (sender == mnuFltGrey)
                    _tifFilter = "*(A)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials;
                else if (sender == mnuFltPurple)
                    _tifFilter = "*(P)." + FWBS.OMS.Session.CurrentSession.CurrentUser.Initials;
                else if (sender == mnuFltOff)
                    _tifFilter = "*.tif";
                RefreshScanImages();
                LoadImage(0);
                _lastfilter = sender as MenuItem;
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void eToolbars3_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            try
            {
                eToolbars3.GetButton("btnPointer").Pushed = false;
                eToolbars3.GetButton("btnSelect").Pushed = false;
                eToolbars3.GetButton("btnAnotate").Pushed = false;

                if (e.Button == eToolbars3.GetButton("btnPointer"))
                {
                    dsViewer.MouseShape = true;
                    dsViewer.Annotation.Type = Dynamsoft.Forms.Enums.EnumAnnotationType.enumNone;

                    eToolbars3.GetButton("btnPointer").Pushed = true;
                }
                else if (e.Button == eToolbars3.GetButton("btnSelect"))
                {
                    dsViewer.MouseShape = false;
                    dsViewer.Annotation.Type = Dynamsoft.Forms.Enums.EnumAnnotationType.enumNone;

                    eToolbars3.GetButton("btnSelect").Pushed = true;
                }
                else if (e.Button == eToolbars3.GetButton("btnAnotate"))
                {
                    eToolbars3.GetButton("btnAnotate").Pushed = true;
                    throw new NotImplementedException();
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void timReturnImage_Tick(object sender, System.EventArgs e)
        {
            pnlTagged.Visible = true;
            timReturnImage.Enabled = false;
        }

        private void btnFind_Click(object sender, System.EventArgs e)
        {
            FWBS.OMS.OMSFile file = FWBS.OMS.UI.Windows.Services.Searches.FindFile();
            if (file != null)
            {
                char[] delimiter;
                delimiter = FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("/config/clientSearch/clientDelimiter", " ./:").ToCharArray();
                txtClientNo.Text = file.Client.ClientNo + delimiter[0].ToString() + file.FileNo;
                btnGo_Click(sender, e);
            }
        }

        private void chkApplyFilter_CheckedChanged(object sender, System.EventArgs e)
        {
            try
            {
                RefreshScanImages(true);
                LoadImage(_scanposition);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void Calendar_DateChanged(object sender, System.Windows.Forms.DateRangeEventArgs e)
        {
            try
            {
                if (chkApplyFilter.Checked)
                {
                    RefreshScanImages(true);
                    LoadImage(_scanposition);
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void CheckZoom()
        {
            if (m_ImageCore.ImageBuffer.HowManyImagesInBuffer == 0)
            {
                eToolbars1.GetButton("btnZoomIn").Enabled = false;
                eToolbars1.GetButton("btnZoomOut").Enabled = false;
                eToolbars1.GetButton("btnFitToWidth").Pushed = false;
                eToolbars1.GetButton("btnFitToPage").Pushed = false;
                return;
            }

            //  the valid range of zoom is between 0.02 to 65.0,

            if (dsViewer.Zoom <= 0.02F)
            {
                eToolbars1.GetButton("btnZoomOut").Enabled = false;
            }
            else
            {
                eToolbars1.GetButton("btnZoomOut").Enabled = true;
            }

            if (dsViewer.Zoom >= 65F)
            {
                eToolbars1.GetButton("btnZoomIn").Enabled = false;
            }
            else
            {
                eToolbars1.GetButton("btnZoomIn").Enabled = true;
            }

            if (this.m_ImageCore.ImageBuffer.HowManyImagesInBuffer > 1)
            {
                if (this.dsViewer.Height
                    >= this.m_ImageCore.ImageBuffer.GetBitmap(this.m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer)
                        .Height * this.dsViewer.Zoom - 1)
                {
                    this.scrollBar.Visible = true;
                }
                else
                {
                    this.scrollBar.Visible = false;
                }
            }
        }

        private void EnablePages()
        {
            int currentImageIndex = m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer;
            int imageCount = m_ImageCore.ImageBuffer.HowManyImagesInBuffer;

            eToolbars2.GetButton("btnPrevPg").Enabled = currentImageIndex > 0;
            eToolbars2.GetButton("btnNextPg").Enabled = currentImageIndex != imageCount - 1;
        }

        private void DsViewer_OnImageAreaDeselected(short simageindex)
        {
            this.selectedOcr = null;
        }

        private void DsViewer_OnImageAreaSelected(short simageindex, int left, int top, int right, int bottom)
        {
            this.Cursor = Cursors.WaitCursor;
            OCRSelectedArea(left, top, right, bottom);
            this.Cursor = Cursors.Default;
        }

        private void DsViewer_OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (m_ImageCore.ImageBuffer.HowManyImagesInBuffer > 0)
            {
                if (e.Delta > 0)
                {
                    if (m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer > 0)
                    {
                        --m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer;
                        if (this.scrollBar.Value > this.scrollBar.Minimum)
                        {
                            --this.scrollBar.Value;
                        }
                    }
                }
                else
                {
                    if (m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer
                        < m_ImageCore.ImageBuffer.HowManyImagesInBuffer - 1)
                    {
                        ++m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer;
                        if (this.scrollBar.Value < this.scrollBar.Maximum)
                        {
                            ++this.scrollBar.Value;
                        }
                    }
                }
                
                ieMulti1_SelectImage();
            }
        }

        private void scrollbar_OnScroll(object sender, ScrollEventArgs e)
        {
            this.m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer = (short)e.NewValue;
            ieMulti1_SelectImage();
        }

        private void DsViewer_OnResize(object sender, EventArgs e)
        {
            if (this.m_ImageCore.ImageBuffer.HowManyImagesInBuffer > 1)
            {
                if (this.dsViewer.Height
                    >= this.m_ImageCore.ImageBuffer.GetBitmap(this.m_ImageCore.ImageBuffer.CurrentImageIndexInBuffer)
                        .Height * this.dsViewer.Zoom - 1)
                {
                    this.scrollBar.Visible = true;
                }
                else
                {
                    this.scrollBar.Visible = false;
                }
            }
        }
        #endregion
    }
}
