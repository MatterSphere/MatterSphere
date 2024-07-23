using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Data;
using FWBS.OMS.Security.Permissions;
using swf = System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for frmMain.
    /// </summary>
    public class frmMain : FWBS.OMS.UI.Windows.frmMain, ISystemUpdate, IMainParent
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        frmAdminDesktop frmAdminDesktop1;
        ucSearchControl search;
        private System.ComponentModel.Container components = null;
        public System.Windows.Forms.Panel pnlHeight;
        private System.Windows.Forms.Button btnGetFocus;
        private System.Windows.Forms.OpenFileDialog ImportDlg;
        private SortedList Windows = new SortedList();
        internal static bool EnabledAutoStart = false;

        // Windows API Messages
        private const int WM_QUERYENDSESSION = 0x11;
        private const int ENDSESSION_CLOSEAPP = 0x1;
        private const int ENDSESSION_LOGOFF = 0x8;
        private const int WM_CREATE = 0x1;
        private FWBS.OMS.Power _power;
        private FWBS.OMS.Script.MenuScriptAggregator menuScriptAgg;

        public SortedList OpenWindows { get; set; }

        private PackageDeploy _packageDeploy = null;
        public PackageDeploy PackageDeployment
        {
            get
            {
                if (_packageDeploy == null)
                {
                    _packageDeploy = PackageDeployFactory.Create(_import_Progress, _import_ShowDialog);

                    _packageDeploy.ActionAfterInstall = AfterInstall;
                    _packageDeploy.Complete = Complete;
                }

                return _packageDeploy;
            }
        }

        private void AfterInstall(IWin32Window owner, OMS.Design.Import.Global _import)
        {
            try
            {
                FWBS.OMS.Session.CurrentSession.Resources.Refresh();
                if (FWBS.OMS.EnquiryEngine.Enquiry.Exists(_import.TreeView.AfterInstall))
                    FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(owner, _import.TreeView.AfterInstall, this,
                        FWBS.OMS.EnquiryEngine.EnquiryMode.Add, false, new FWBS.Common.KeyValueCollection());
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        public void OutcomeMessage(int failed, bool backup)
        {
            SystemUpdateDefaultActions.OutcomeMessage(failed, backup);

            this.Focus();
        }

        private void Complete()
        {
            this.MenuControl.InitalizeSetup(this.MenuCode, this);

            if (_importprogress != null)
            {
                _importprogress.Close();
                _importprogress = null;
            }
        }

        public frmMain()
            : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            Session.CurrentSession.Connected += new EventHandler(OMS_Connected);
            AttachDisconnectingEvent();
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            OpenWindows = new SortedList();
        }

        void OMS_Connected(object sender, EventArgs e)
        {
            mnuConnect.Enabled = false;
            mnuLogOff.Enabled = true;

            AttachDisconnectingEvent();
        }

        private void AttachDisconnectingEvent()
        {
            Session.CurrentSession.Disconnecting -= new CancelEventHandler(CurrentSession_Disconnecting);
            Session.CurrentSession.Disconnecting += new CancelEventHandler(CurrentSession_Disconnecting);
        }


        void CurrentSession_Disconnecting(object sender, CancelEventArgs e)
        {
            if (!RunLive)
                return;

            foreach (Form f in this.MdiChildren)
            {
                f.Close();
            }

            // if all windows closed OK then there will be no children
            if (this.MdiChildren.Length > 0)
            {
                e.Cancel = true;
                return;
            }

            if (OnBeforeDisconnect() == false)
            {
                OMSToolbars.GroupVisible("Logon", true);
                OMSToolbars.GroupVisible("Main", false);

                Session.CurrentSession.Disconnecting -= new CancelEventHandler(CurrentSession_Disconnecting);

                System.GC.Collect(2);
                System.GC.WaitForPendingFinalizers();
                System.GC.Collect(2);
            }
        }

        void OMS_Disconnected(object sender, EventArgs e)
        {
            if (menuScriptAgg != null)
            {
                menuScriptAgg.Dispose();
                menuScriptAgg = null;
            }

            try
            {
                mnuConnect.Enabled = true;
            }
            catch { }
            try
            {
                mnuLogOff.Enabled = false;
            }
            catch { }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                this.OnfrmMainClosing();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.pnlHeight = new System.Windows.Forms.Panel();
            this.btnGetFocus = new System.Windows.Forms.Button();
            this.ImportDlg = new System.Windows.Forms.OpenFileDialog();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // OMSToolbars
            // 
            this.OMSToolbars.BottomDivider = true;
            this.OMSToolbars.ButtonsXML = resources.GetString("OMSToolbars.ButtonsXML");
            this.OMSToolbars.Location = new System.Drawing.Point(6, 1);
            this.OMSToolbars.Size = new System.Drawing.Size(649, 26);
            this.OMSToolbars.OMSButtonClick += new FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventHandler(this.OMSToolbars_OMSButtonClick);
            this.OMSToolbars.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.OMSToolbars_ButtonClick);
            // 
            // pnlMain
            // 
            this.pnlMain.Padding = new System.Windows.Forms.Padding(6, 0, 6, 1);
            this.pnlMain.Size = new System.Drawing.Size(661, 28);
            // 
            // pnlHeight
            // 
            this.pnlHeight.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlHeight.Location = new System.Drawing.Point(660, 52);
            this.pnlHeight.Name = "pnlHeight";
            this.pnlHeight.Size = new System.Drawing.Size(1, 400);
            this.pnlHeight.TabIndex = 2;
            // 
            // btnGetFocus
            // 
            this.btnGetFocus.Location = new System.Drawing.Point(219, -16);
            this.btnGetFocus.Name = "btnGetFocus";
            this.btnGetFocus.Size = new System.Drawing.Size(5, 16);
            this.btnGetFocus.TabIndex = 3;
            // 
            // ImportDlg
            // 
            this.ImportDlg.DefaultExt = "xml";
            this.ImportDlg.Filter = "XML Files (*.xml)|*.xml|Batch Package Files (*.manifest)|*.manifest|All Files (*." +
    "*)|*.*";
            this.ImportDlg.Title = "Import OMS XML Package";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(661, 452);
            this.Controls.Add(this.btnGetFocus);
            this.Controls.Add(this.pnlHeight);
            this.MenuCode = "ADMIN";
            this.Name = "frmMain";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Text = FWBS.OMS.Branding.APPLICATION_NAME + " Administration";
            this.TransparencyKey = System.Drawing.Color.White;
            this.FolderOpening += new FWBS.OMS.UI.Windows.OpenFolderEventHandler(this.frmMain_FolderOpening);
            this.SystemUpdateClick += new System.EventHandler(this.MenuControl_SystemUpdateClick);
            this.ApplicationPanelClear += new System.EventHandler(this.frmMain_ApplicationPanelClear);
            this.ApplicationLinkClicked += new FWBS.OMS.UI.Windows.LinkEventHandler(this.frmMain_ApplicationLinkClicked);
            this.MenuLoaded += new System.EventHandler(this.frmMain_MenuLoaded);
            this.MenuActioned += new FWBS.OMS.UI.Windows.MenuEventHandler(this.frmMain_MenuActioned);
            this.BeforeDisconnect += new System.ComponentModel.CancelEventHandler(this.frmMain_BeforeDisconnect);
            this.AfterConnect += new System.ComponentModel.CancelEventHandler(this.frmMain_AfterConnect);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.MdiChildActivate += new System.EventHandler(this.frmMain_MdiChildActivate);
            this.Controls.SetChildIndex(this.pnlMain, 0);
            this.Controls.SetChildIndex(this.pnlHeight, 0);
            this.Controls.SetChildIndex(this.btnGetFocus, 0);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion


        #region Windows Hooks

        protected override void WndProc(ref Message m)
        {

            if (m.Msg == WM_QUERYENDSESSION)
            {
                switch ((int)m.LParam)
                {
                    case ENDSESSION_CLOSEAPP:
                        int retval;
                        retval = InteropDefs.RegisterApplicationRestart("ADMINRESET", 0);
                        if (retval != 0)
                        {
                            // Error Registering application
                        }
                        else
                            m.Result = (System.IntPtr)1;
                        return;
                    default:
                        return;
                }
            }

            base.WndProc(ref m);
        }

        #endregion


        public void frmMain_MenuActioned(object sender, FWBS.OMS.UI.Windows.MenuEventArgs e)
        {
            int nodeID = 0;
            int consoleID = 0;
            try
            {
                if (!string.IsNullOrEmpty(e.itemIDs))
                {
                    if(e.itemIDs.IndexOf(";") > -1)
                    {
                        nodeID = Convert.ToInt32(e.itemIDs.Substring(0, e.itemIDs.IndexOf(";")));
                        consoleID = Convert.ToInt32(e.itemIDs.Substring(e.itemIDs.IndexOf(";") + 1));
                    }
                    LoadAdminKitConsole(consoleID, nodeID);
                }
                else
                {
                    Action(e.ReturnKey.ToString(), e.ButtonCode);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void frmAdminDesktop1_Closing(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
                return;

            try
            {
                Windows.Remove(Convert.ToString(((Form)sender).Tag));
                ((Form)sender).Dispose();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void frmMain_MdiChildActivate(object sender, System.EventArgs e)
        {
            try
            {
                OMSToolbars.GroupVisible("Designer", this.ActiveMdiChild is FWBS.OMS.Design.frmDesigner);
                OMSToolbars.GroupVisible("EditBase", this.ActiveMdiChild is frmAdminDesktop || this.ActiveMdiChild is frmGenEngine);
                if (this.ActiveMdiChild is frmAdminDesktop)
                    OMSToolbars.GroupVisible("CODELOOKUPS", ((frmAdminDesktop)this.ActiveMdiChild).editbase is ucCodeLookups);
                else
                    OMSToolbars.GroupVisible("CODELOOKUPS", false);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void frmMain_BeforeDisconnect(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _power = null;
        }

        private void OMSToolbars_OMSButtonClick(object sender, FWBS.OMS.UI.Windows.OMSToolBarButtonClickEventArgs e)
        {
            btnGetFocus.Focus();
            Application.DoEvents();
            if (e.Button.Name == "btnClose")
            {
                frmAdminDesktop frmAD = this.ActiveMdiChild as frmAdminDesktop;
                try
                {
                    if (frmAD == null)
                    {
                        if (this.ActiveMdiChild != null)
                            this.ActiveMdiChild.Close();
                    }
                    else if (frmAD.editbase2 != null && frmAD.editbase2.ListMode == true)
                        frmAD.Close();
                    else if (frmAD.editbase2 != null && frmAD.editbase2.ListMode == false)
                        frmAD.editbase2.SaveAndClose();
                    else if (frmAD.editbase != null && frmAD.editbase.ListMode == true)
                        frmAD.Close();
                    else if (frmAD.editbase != null && frmAD.editbase.ListMode == false)
                        frmAD.editbase.SaveAndClose();

                    else
                    {

                        if (frmAD != null)
                            frmAD.Close();
                    }
                    OnfrmMainClosing();
                }
                catch
                {
                    if (this.ActiveMdiChild != null)
                        this.ActiveMdiChild.Close();
                }
            }
        }


        public override object Action(string ActionCmd, string ActionLabel)
        {
            string filter = "";
            string ecmd = ActionCmd;
            object returnval = null;
            if (ecmd.IndexOf("|") > -1)
            {
                filter = ecmd.Substring(ecmd.IndexOf("|") + 1);
                ecmd = ecmd.Substring(0, ecmd.IndexOf("|"));
            }

            if (_power != null)
            {
                if (_power.CanRunAction(ActionCmd) == false)
                {
                    throw new FWBS.OMS.Security.PermissionsException("EXPERMDENIED2", "Permission Denied", true);
                }
            }

            try
            {
                var result = false;
                returnval = MacroCommands(ecmd, filter, out result);
                if (result)
                {
                    return returnval;
                }
                else if ("SCHSYSMSTCONFIG|SCR|EF|RP|SC|EX|CL|OT|DL|SL|OO|FNDT|PE|CU|UR|PT|PD|FI|MB|FEE|SYS|PDL|DS|LI|PDFI".IndexOf(ecmd) > -1)
                {
                    frmAdminDesktop1 = Windows[filter] as frmAdminDesktop;
                    if (frmAdminDesktop1 == null)
                    {
                        frmAdminDesktop1 = new frmAdminDesktop();
                        frmAdminDesktop1.Tag = filter;
                        frmAdminDesktop1.Closing += new CancelEventHandler(frmAdminDesktop1_Closing);
                        frmAdminDesktop1.MdiParent = this;
                        Windows.Add(filter, frmAdminDesktop1);
                    }
                    ucEditBase2 ucEditBase2 = null;
                    var newreturnval = ConstructAdminElement(filter, frmAdminDesktop1, ecmd);
                    var console = newreturnval as ucOMSAdminKitConsole;
                    if (console != null)
                        console.LoadTreeView(filter);
                    if (newreturnval == null)
                    {
                        Type edit = Session.CurrentSession.TypeManager.TryLoad(ecmd);
                        if (edit != null)
                        {
                            ucEditBase2 = (ucEditBase2)edit.InvokeMember(String.Empty, System.Reflection.BindingFlags.CreateInstance, null, null, null);
                            ucEditBase2.Initialise(this, frmAdminDesktop1, new FWBS.Common.KeyValueCollection());
                            ucEditBase2.Load();
                        }
                    }
                    Global.RemoveAndDisposeControls(frmAdminDesktop1);
                    ucEditBase2 = newreturnval as ucEditBase2;
                    var ucEditBase = newreturnval as ucEditBase;
                    if (ucEditBase2 != null)
                    {
                        frmAdminDesktop1.Controls.Add(ucEditBase2.tpList);
                        frmAdminDesktop1.editbase2 = ucEditBase2;
                        ucEditBase2.tpList.Text = frmAdminDesktop1.Text;
                    }
                    else if (ucEditBase != null)
                    {
                        frmAdminDesktop1.Controls.Add(ucEditBase.tpList);
                        frmAdminDesktop1.editbase = ucEditBase;
                    }
                    else
                    {
                        newreturnval.Dock = DockStyle.Fill;
                        frmAdminDesktop1.Controls.Add(newreturnval);
                    }

                    returnval = newreturnval;

                    frmAdminDesktop1.Show();
                }
                else if ("AKC".IndexOf(ecmd) > -1)
                {
                    frmAdminDesktop1 = null;
                    try
                    {
                        LoadAdminKitConsole(0, 0, filter);
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(this, ex);
                        frmAdminDesktop1.Close();
                        Windows.Remove(ecmd + filter);
                    }
                }
                else if (Windows[ecmd + filter] == null)
                {
                    frmGenEngine frmGenEngine1 = new frmGenEngine(ecmd);
                    Windows.Add(ecmd + filter, frmGenEngine1);
                    frmGenEngine1.NavActions.BringToFront();
                    frmGenEngine1.Closing += new CancelEventHandler(frmAdminDesktop1_Closing);
                    frmGenEngine1.Tag = ecmd + filter;
                    frmGenEngine1.MdiParent = this;
                    frmGenEngine1.Show();
                }
                else
                {
                    frmGenEngine frm = (frmGenEngine)Windows[ecmd + filter];
                    frm.BringToFront();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
            finally
            {
            }
            return returnval;
        }

        public override Control MacroCommands(string ecmd, string filter, out bool result)
        {
            result = false;
            if (ecmd == "TESTBROWSER")
            {
                frmTestOMSTypeBrowser test = new frmTestOMSTypeBrowser();
                test.Show();
                result = true;
            }
            else if (ecmd == "MENUSCRIPT")
            {

                if (menuScriptAgg == null)
                    menuScriptAgg = new FWBS.OMS.Script.MenuScriptAggregator();

                menuScriptAgg.Invoke(filter);
                result = true;
            }
            else if (ecmd == "OPENLOCALDOCUMENT")
            {
                FWBS.OMS.UI.Windows.DocumentManagement.DocumentPicker picker = new FWBS.OMS.UI.Windows.DocumentManagement.DocumentPicker();
                picker.ShowLocal(null);
                result = true;
            }
            else if (ecmd == "OPENDOCUMENT")
            {
                FWBS.OMS.UI.Windows.Services.ShowOpenDocument(null);
                result = true;
            }
            else if (ecmd == "DOCUMENTPICKER")
            {
                FWBS.OMS.UI.Windows.DocumentManagement.DocumentPicker picker = new FWBS.OMS.UI.Windows.DocumentManagement.DocumentPicker();
                picker.Show(null);
                result = true;
            }
            else if (ecmd == "CLIENTMANAGER")
            {
                FWBS.OMS.UI.Windows.Services.ShowSearchManager(FWBS.OMS.UI.Windows.SearchManager.ClientManager);
                result = true;
            }
            else if (ecmd == "FILEMANAGER")
            {
                FWBS.OMS.UI.Windows.Services.ShowSearchManager(FWBS.OMS.UI.Windows.SearchManager.FileManager);
                result = true;
            }
            else if (ecmd == "CONTACTMANAGER")
            {
                FWBS.OMS.UI.Windows.Services.ShowSearchManager(FWBS.OMS.UI.Windows.SearchManager.ContactManager);
                result = true;
            }
            else if (ecmd == "NC")
            {
                FWBS.OMS.UI.Windows.Services.Wizards.CreateClient(true);
                result = true;
            }
            else if (ecmd == "NF")
            {
                FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(true);
                result = true;
            }
            else if (ecmd == "C2V2")
            {
                FWBS.OMS.UI.Windows.FormRendererBase.ConvertQuestionsToV2();
                result = true;
            }
            else if (ecmd == "SVD")
            {
                FWBS.OMS.Associate ass = FWBS.OMS.UI.Windows.Services.SelectAssociate();
                if (ass != null)
                {
                    int id = 2143593470;
                    FWBS.OMS.OMSDocument doc = new FWBS.OMS.OMSDocument(ass, "This is a Test Description", FWBS.OMS.Precedent.GetPrecedent(id), FWBS.OMS.Precedent.GetPrecedent(id), 2, FWBS.OMS.DocumentDirection.Out);
                    FWBS.OMS.UI.Windows.Services.Wizards.SaveDocument(ref doc);
                    result = true;
                }
            }
            else if (ecmd == "PM")
            {
                FWBS.OMS.UI.Windows.Services.ShowPrecedentLibrary(this, null, FWBS.OMS.UI.Windows.Services.SelectAssociate(), "LETTERHEAD", "");
                result = true;
            }
            else if (ecmd == "ASS")
            {
                FWBS.OMS.UI.Windows.Services.SelectAssociate();
                result = true;
            }
            else if (ecmd == "CLMT")
            {
                FWBS.OMS.UI.Windows.Services.ShowFile(this, true, "");
                result = true;
            }
            else if (ecmd == "CLT")
            {
                FWBS.OMS.UI.Windows.Services.ShowClient(true);
                result = true;
            }
            else if (ecmd == "MT")
            {
                OMSFile f = FWBS.OMS.UI.Windows.Services.SelectFile();
                if (f != null)
                {
                    FWBS.OMS.UI.Windows.Services.Wizards.CreateManualTime(f.ID);
                    result = true;
                }
            }
            else if (ecmd == "RT")
            {
                using (frmRichText frmRichText1 = new frmRichText())
                {
                    frmRichText1.ShowDialog(this);
                    result = true;
                }
            }
            else if (ecmd == "ED")
            {
                FWBS.OMS.Design.frmDesigner frmDesigner1;
                frmDesigner1 = Windows[ecmd] as FWBS.OMS.Design.frmDesigner;
                if (frmDesigner1 == null)
                {
                    frmDesigner1 = new FWBS.OMS.Design.frmDesigner();
                    Windows.Add(ecmd, frmDesigner1);
                    frmDesigner1.Tag = ecmd;
                    frmDesigner1.Closing += new CancelEventHandler(frmAdminDesktop1_Closing);
                    frmDesigner1.MdiParent = this;
                    try
                    {
                        frmDesigner1.Show();
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(this, ex);
                        frmDesigner1.Close();
                    }
                }
                else
                {
                    frmDesigner1.Show();
                    frmDesigner1.BringToFront();
                }
                frmDesigner1.WindowState = FormWindowState.Maximized;
                result = true;
                return frmDesigner1;
            }
            else if (ecmd == "POWER")
            {
                result = true;
            }
            else if (ecmd == "SYS")
            {
                Services.Wizards.SaveSystemSettings(this);
                result = true;
            }
            else if (ecmd == "WZ")
            {
                string code = filter;
                EnquiryEngine.EnquiryMode mode = FWBS.OMS.EnquiryEngine.EnquiryMode.Add;
                if (code.EndsWith("|A"))
                {
                    code = code.Substring(0, code.Length - 2);
                }
                else if (code.EndsWith("|E"))
                {
                    code = code.Substring(0, code.Length - 2);
                    mode = FWBS.OMS.EnquiryEngine.EnquiryMode.Edit;
                }
                Services.Wizards.GetWizard(this, code, null, mode, false, new Common.KeyValueCollection());
                result = true;
            }
            return null;
        }

        public override Control ConstructAdminElement(string filter, Control parent, string ecmd)
        {
            Control returnval = null;
            if (ecmd == "SCHSYSMSTCONFIG")
            {
                ucMileStoneConfig ucMileStoneConfig1 = new ucMileStoneConfig(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucMileStoneConfig1;
            }
            else if (ecmd == "MB")
            {
                ucCommandBarBuilder ucCommandBarBuilder1 = new ucCommandBarBuilder(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucCommandBarBuilder1;
            }
            else if (ecmd == "PD")
            {
                ucPackages ucPackages1 = new ucPackages(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucPackages1;
            }
            else if (ecmd == "RP")
            {
                FWBS.Common.KeyValueCollection gfilter = new FWBS.Common.KeyValueCollection();
                gfilter.Add("Group", filter);
                gfilter.Add("Type", DBNull.Value);
                gfilter.Add("Active", 1);
                ucReports ucReports1 = new ucReports(this, parent, gfilter);
                returnval = ucReports1;
            }
            else if (ecmd == "SC")
            {
                FWBS.Common.KeyValueCollection Params = new FWBS.Common.KeyValueCollection();
                Params.Add("ScriptType", filter);
                ucScripts ucScripts = new ucScripts(this, parent, Params);
                ucScripts.Type = filter;
                returnval = ucScripts;
            }
            else if (ecmd == "EX")
            {
                ucExtendedData ucExtendedData1 = new ucExtendedData(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucExtendedData1;
            }
            else if (ecmd == "CU")
            {
                ucCurrency ucCurrency = new ucCurrency(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucCurrency;
            }
            else if (ecmd == "FNDT")
            {
                ucFundType ucFundType = new ucFundType(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucFundType;
            }
            else if (ecmd == "DS")
            {
                ucDistributed ucDistributed = new ucDistributed(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucDistributed;
            }
            else if (ecmd == "PE")
            {
                ucPrecedent ucPrec = new ucPrecedent(this, parent);
                returnval = ucPrec;
            }
            else if (ecmd == "CL")
            {
                FWBS.Common.KeyValueCollection Params = new FWBS.Common.KeyValueCollection();
                Params.Add("TYPE", filter);
                ucCodeLookups ucCodeLookups1 = new ucCodeLookups(this, parent, Params);
                returnval = ucCodeLookups1;
            }
            else if (ecmd == "OT")
            {
                FWBS.Common.KeyValueCollection Params = new FWBS.Common.KeyValueCollection();
                Params.Add("Type", filter);
                ucEntitiesV2 ucEntitiesNT = new ucEntitiesV2(this, parent, Params);
                returnval = ucEntitiesNT;
            }
            else if (ecmd == "DL")
            {
                ucDataLists ucDataLists1 = new ucDataLists(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucDataLists1;
            }
            else if (ecmd == "PDL")
            {
                ucPackageData ucPackageData1 = new ucPackageData(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucPackageData1;
            }
            else if (ecmd == "SL")
            {
                FWBS.Common.KeyValueCollection pfilter = new FWBS.Common.KeyValueCollection();
                pfilter.Add("group", filter);
                pfilter.Add("type", DBNull.Value);
                ucSearchList ucSearchList1 = new ucSearchList(this, parent, pfilter);
                returnval = ucSearchList1;
            }
            else if (ecmd == "OO")
            {
                ucOMSObjexts ucOMSObjexts1 = new ucOMSObjexts(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucOMSObjexts1;
            }
            else if (ecmd == "UR")
            {
                ucUsers ucUsers1 = new ucUsers(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucUsers1;
            }
            else if (ecmd == "FEE")
            {
                ucFeeEarners ucFee1 = new ucFeeEarners(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucFee1;
            }
            else if (ecmd == "PT")
            {
                ucPrinters ucPrinters1 = new ucPrinters(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucPrinters1;
            }
            else if (ecmd == "FI")
            {
                ucFormImporter ucFormImporter1 = new ucFormImporter();
                returnval = ucFormImporter1;
            }
            else if (ecmd == "SCR")
            {
                string code = filter.Substring(0, filter.Length - 2);
                string mode = filter.Substring(filter.Length - 1);
                FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                param.Add("Code", code);
                param.Add("Mode", mode);
                ucScreen ucScreen1 = new ucScreen(this, parent, param);
                returnval = ucScreen1;
            }
            else if (ecmd == "EF")
            {
                string code = filter.Substring(0, filter.Length - 2);
                string mode = filter.Substring(filter.Length - 1);
                if(string.IsNullOrWhiteSpace(mode) || mode == "A")
                    FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(code, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, null);
                else
                    FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(code, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, null);
                returnval = new EnquiryForm();
            }
            else if (ecmd == "PDFI")
            {
                ucPDFImporter ucPDFImporter1 = new ucPDFImporter();
                returnval = ucPDFImporter1;
            }
            else if (ecmd == "AKC")
            {
                ucOMSAdminKitConsole console = new ucOMSAdminKitConsole(this, (frmAdminDesktop)parent);
                returnval = console;
            }
            else if (ecmd == "DSH")
            {
                ucDashboards ucDashboards1 = new ucDashboards(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucDashboards1;
            }

            return returnval;
        }

        private void frmMain_ApplicationLinkClicked(FWBS.OMS.UI.Windows.ucNavCmdButtons LinkButton)
        {
            if (LinkButton.Name == "ucApp1")
            {
                try
                {
                    System.Diagnostics.Process.Start("omsreports.exe");
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(this, ex);
                }
            }
        }

        private void MenuControl_SystemUpdateClick(object sender, EventArgs e)
        {
            FWBS.OMS.ReportingServer connection = new ReportingServer("FWBS Limited 2005");
            try { connection.Connection.ExecuteSQL("INSERT INTO dbCaptainsLogType (typeID, typeCode, typeSeverity, typeGroup, typeSystem) VALUES (40,'PACKAGE',255, 'PACKAGE',1)", new IDataParameter[0]); }
            catch { }
            try { connection.Connection.ExecuteSQL("INSERT INTO dbCaptainsLogType (typeID, typeCode, typeSeverity, typeGroup, typeSystem) VALUES (41,'PACKEXC',255, 'PACKEXC',1)", new IDataParameter[0]); }
            catch { }
            try { connection.Connection.ExecuteSQL("INSERT INTO dbCaptainsLogType (typeID, typeCode, typeSeverity, typeGroup, typeSystem) VALUES (42,'PACKERR',255, 'PACKERR',1)", new IDataParameter[0]); }
            catch { }

            using (frmFullSystemUpdate frmFullSystemUpdate = new frmFullSystemUpdate(this))
            {
                if (frmFullSystemUpdate.ShowDialog(this) == DialogResult.OK)
                {
                    object obj;

                    this.PackageDeployment.ProcessManifest(this, frmFullSystemUpdate.Filename, null, null, null, true, out obj, true);
                }
            }
        }

        private void frmMain_ApplicationPanelClear(object sender, System.EventArgs e)
        {
            FWBS.OMS.UI.Windows.ucNavCmdButtons ucApp1 = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            // 
            // ucApp1
            // 
            ucApp1.Dock = System.Windows.Forms.DockStyle.Bottom;
            ucApp1.ImageIndex = 21;
            ucApp1.Location = new System.Drawing.Point(5, 5);
            ucApp1.Name = "ucApp1";
            ucApp1.Size = new System.Drawing.Size(163, 22);
            ucApp1.TabIndex = 0;
            ucApp1.Text = Session.CurrentSession.Resources.GetResource("OMSREPORTS", FWBS.OMS.Branding.APPLICATION_NAME + " Reports", "").Text;
            MenuControl.ApplicationCommands.Controls.Add(ucApp1);
            MenuControl.ApplicationCommands.Refresh(true);
        }

        /// <summary>
        /// Show the Import Dialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _import_ShowDialog(object sender, CancelEventArgs e)
        {
            FWBS.OMS.Design.Import.Global _import = sender as FWBS.OMS.Design.Import.Global;
            frmImportDlg dlg = new frmImportDlg(_import.TreeView);
            SortedList _parents = new SortedList();

            // Builds the Tree
            dlg.TreeView.BeginUpdate();
            FWBS.OMS.Design.Export.TreeView _treeview = _import.TreeView;
            _treeview.Source.DefaultView.RowStateFilter = System.Data.DataViewRowState.OriginalRows;
            _treeview.Source.DefaultView.RowStateFilter = System.Data.DataViewRowState.CurrentRows;

            foreach (System.Data.DataRowView dr in _treeview.Source.DefaultView)
            {
                _treeview.Goto(dr);
                TreeNode tn = new TreeNode(_treeview.Name, _treeview.ImageIndex, _treeview.ImageIndex);
                tn.Tag = _treeview.RowView;
                if (_treeview.Active == false)
                    tn.Checked = false;
                else if (_treeview.InstallOnce && _treeview.RootImportable)
                {
                    tn.Checked = FWBS.OMS.Design.Import.Global.IsPackageItemInstalled(_treeview.Code, _treeview.Type);
                    dr["Active"] = tn.Checked;
                }
                else
                    tn.Checked = true;


                if (_parents[_treeview.ParentID] == null)
                    dlg.TreeView.Nodes.Add(tn);
                else
                {
                    ((TreeNode)_parents[_treeview.ParentID]).Nodes.Add(tn);
                    if (_treeview.RootImportable == false)
                        tn.Checked = ((TreeNode)_parents[_treeview.ParentID]).Checked;
                }

                _parents.Add(_treeview.ID, tn);
            }
            dlg.Version = _treeview.PackageVersion.ToString();
            dlg.TreeView.EndUpdate();
            dlg.TreeView.ExpandAll();
            dlg.btnFieldReplace.Enabled = (_import.TreeView.FieldReplacer.Count > 0);

            dlg.ShowDialog(_import.Owner);
            if (dlg.DialogResult == DialogResult.Cancel) e.Cancel = true;
        }


        private frmImportProgress _importprogress = null;
        private bool _importprogresscancel = false;

        /// <summary>
        /// Process the Progress Bar Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _import_Progress(object sender, FWBS.OMS.Design.Import.ProgressEventArgs e)
        {
            FWBS.OMS.Design.Import.Global _import = sender as FWBS.OMS.Design.Import.Global;
            if (_importprogress == null)
            {
                _importprogress = new frmImportProgress();
                _importprogress.Owner = (Form)_import.Owner;
                _importprogress.Text = Session.CurrentSession.Resources.GetResource("frmImprtProg", "Import Progress", "").Text + " - [" + _import.TreeView.PackageCode + "]";
                _importprogress.Show();
                _importprogress.Cancelled += new EventHandler(_importprogress_Cancelled);
                _importprogress.ProgressBar.Maximum = ((FWBS.OMS.Design.Import.Global)sender).PackageCount;
            }

            _importprogress.Label = e.Label;
            _importprogress.ProgressBar.Increment(1);
            e.Cancel = _importprogresscancel; // This will alow the Business Layer to Cancel
            Application.DoEvents();
        }

        /// <summary>
        /// If the Cancel Button is Clicked on the Import Progress Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _importprogress_Cancelled(object sender, EventArgs e)
        {
            _importprogresscancel = true;
        }

        private void frmMain_FolderOpening(object sender, OpenFolderEventArgs e)
        {
            try
            {
                if (e.Name == "AMUSDK" || e.Name == "1329463636")
                {
                    if (Session.CurrentSession.IsLicensedFor("SDKALL") == false)
                    {
                        if (PartnerAccess == false)
                        {
                            using (frmPasswordRequest pas = new frmPasswordRequest())
                            {
                                pas.ShowDialog(this);
                                if (pas.DialogResult == DialogResult.Cancel)
                                    e.Cancel = true;
                                else
                                {
                                    FWBS.OMS.Logging.CaptainsLog.CreateEntry(8, "SDK Access Granted", null, "", false);
                                    PartnerAccess = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void frmMain_AfterConnect(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                new SystemPermission(Permission.StandardTypeToString(StandardPermissionType.AdminKit)).Check();
                CultureMenus();
                frmWarningV5.ShowWarning();
            }
            catch (FWBS.OMS.Security.PermissionsException)
            {
                if (Session.CurrentSession.CurrentUser.IsInRoles("POWER") == false)
                {
                    if (Session.CurrentSession.AdvancedSecurity == false)
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("APROLADKIT", "You must have the Administrator or Power User Role to use the Admin Kit");
                    else
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("APROLADKIT2", "You must have the Administrator Role in Advanced Security or Power User Role to use the Admin Kit");

                    e.Cancel = true;
                    Disconnect();
                }
                else
                {
                    _power = Session.CurrentSession.CurrentPowerUserSettings;
                    if (_power.IsConfigured == false)
                    {
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("APROLADKIT2", "The Power User settings have not been configured");
                        e.Cancel = true;
                        Disconnect();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
                e.Cancel = true;
                Disconnect();
            }
        }

        private void CultureMenus()
        {

            this.mnuFile.Text = Session.CurrentSession.Resources.GetResource("mnuFile", this.mnuFile.Text, "").Text;
            this.mnuLogOff.Text = Session.CurrentSession.RegistryRes("Disconnect", this.mnuLogOff.Text);
            this.mnuConnect.Text = Session.CurrentSession.RegistryRes("Connect", this.mnuConnect.Text);

            this.mnuWindow.Text = Session.CurrentSession.Resources.GetResource("mnuWindow", this.mnuWindow.Text, "").Text;

            this.mnuHelp.Text = Session.CurrentSession.Resources.GetResource("mnuHelp", this.mnuHelp.Text, "").Text;
            this.mnuAbout.Text = Session.CurrentSession.Resources.GetResource("ABOUT", this.mnuAbout.Text, "").Text;
            this.mnuGC.Text = Session.CurrentSession.Resources.GetResource("ForceGC", this.mnuGC.Text, "").Text;



        }

        private void frmMain_MenuLoaded(object sender, EventArgs e)
        {
            this.MenuControl.AddRemoveButton.Visible = false;
            this.MenuControl.CommandBarPanel.Refresh();
            this.MenuControl.LoadBrowser += new CancelEventHandler(MenuControl_LoadBrowser);
            this.MenuControl.GoClick += new HandledEventHandler(MenuControl_GoClick);
        }

        void MenuControl_GoClick(object sender, HandledEventArgs e)
        {
            PerformAdminKitSearch();
            this.OMSToolbars.GetButton("tbParent").Enabled = false;
        }


        private void PerformAdminKitSearch()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(this.SearchText))
                {
                    this.MenuControl.SearchPanel.Visible = true;
                    FWBS.Common.KeyValueCollection searchkeys = new FWBS.Common.KeyValueCollection();
                    searchkeys.Add("DESC", this.SearchText);
                    searchkeys.Add("TYPE", "ADMIN");
                    search.SearchList.ChangeParameters(searchkeys);
                    search.Search();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        private void SetupAdminKitSearch()
        {
            try
            {
                if (CheckForAKSearchList())
                {
                    search = new ucSearchControl();
                    search.Dock = DockStyle.Fill;
                    search.SetSearchList("LADMKITSEARCH", null, new FWBS.Common.KeyValueCollection());
                    search.SearchButtonCommands -= new SearchButtonEventHandler(search_SearchButtonCommands);
                    search.SearchButtonCommands += new SearchButtonEventHandler(search_SearchButtonCommands);
                    this.MenuControl.SearchPanel.Controls.Add(search);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
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


        private void search_SearchButtonCommands(object sender, SearchButtonEventArgs e)
        {
            try
            {
                if (e.ButtonName.ToUpper() == "CMDOPEN")
                {
                    try
                    {
                        int consoleID = Convert.ToInt32(search.CurrentItem()["ContainerID"].Value);
                        int nodeID = Convert.ToInt32(search.CurrentItem()["NodeID"].Value);
                        if(CheckUserHasAccessToFunction(Convert.ToString(search.CurrentItem()["Roles"].Value)))
                        {
                            string nodeText = Convert.ToString(search.CurrentItem()["NodeDescription"].Value);
                            if (!CheckForLicenseManager(nodeID))
                            {
                                LoadAdminKitConsole(consoleID, nodeID);
                                base.frmMenu1.ucHome.SearchPanel.Visible = false;
                            }
                            else
                            {
                                Action("LI", "");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(this, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
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
                if (Convert.ToString(dt.Rows[0]["admnuSearchListCode"]) == "LI" && ConvertDef.ToInt32(dt.Rows[0]["admnuParent"],0) == 1)
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


        private void LoadAdminKitConsole(int consoleID, int nodeID, string filter = null)
        {
            try
            {
                frmAdminDesktop1 = Windows["CONSOLE"] as frmAdminDesktop;
                if (frmAdminDesktop1 == null)
                {
                    frmAdminDesktop1 = new frmAdminDesktop();
                    frmAdminDesktop1.Tag = "CONSOLE";
                    frmAdminDesktop1.Closing += new CancelEventHandler(frmAdminDesktop1_Closing);
                    frmAdminDesktop1.MdiParent = this;
                    Windows.Add("CONSOLE", frmAdminDesktop1);
                    ucOMSAdminKitConsole console = new ucOMSAdminKitConsole(this, frmAdminDesktop1);
                    frmAdminDesktop1.Controls.Add(console);
                    console.Dock = DockStyle.Fill;
                    CreateConsoleTreeView(consoleID, nodeID, filter, console);
                    if (GetSearchListCode(nodeID) != "ED")
                    {
                        frmAdminDesktop1.Show();
                    }
                }
                else
                {
                    var console = (ucOMSAdminKitConsole)frmAdminDesktop1.Controls[0];
                    CreateConsoleTreeView(consoleID, nodeID, filter, console);
                    frmAdminDesktop1.Hide();
                    frmAdminDesktop1.Show();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        private string GetSearchListCode(long nodeID)
        {
            string ret = "";
            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection(){{"nodeID", nodeID}} ;
            DataTable dt = FWBS.OMS.UI.Windows.Services.RunDataList("DMENUACTION", kvc);
            if (dt != null & dt.Rows.Count > 0)
            {
                ret = Convert.ToString(dt.Rows[0]["admnuSearchListCode"]);
            }
            return ret;
        }


        private static void CreateConsoleTreeView(int consoleID, int nodeID, string filter, ucOMSAdminKitConsole console)
        {
            if (!string.IsNullOrEmpty(filter))
                console.LoadTreeView(filter);
            else
                console.LoadTreeView(consoleID, nodeID);
        }


        void MenuControl_LoadBrowser(object sender, CancelEventArgs e)
        {
            SetupAdminKitSearch();
            e.Cancel = true;
        }


        public void SetPartnerAcessValue(bool value)
        {
            PartnerAccess = value;
        }


        private void OMSToolbars_ButtonClick(object sender, ToolBarButtonClickEventArgs e)
        {

        }

        private void mainMenu1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }


}
