using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS.UI.Windows.Admin
{
    class AdminKitTreeNavigationActions : TreeNavigationActions, ISystemUpdate
    {
        frmAdminDesktop frmAdminDesktop1;
        private FWBS.OMS.Script.MenuScriptAggregator menuScriptAgg;
        private frmImportProgress _importprogress = null;
        private bool _importprogresscancel = false;

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
                ErrorBox.Show(ex);
            }
        }

        private void Complete()
        {
            if (_importprogress != null)
            {
                _importprogress.Close();
                _importprogress = null;
            }
        }

        public AdminKitTreeNavigationActions()
        {
            this.ParentKey = "ADMIN";
            this.Title = FWBS.OMS.Branding.APPLICATION_NAME + " Administration";
        }

        public override void BuildApplicationPanels(ucNavCommands ucNavApplications)
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
            ucApp1.Text = Session.CurrentSession.Resources.GetResource("OMSREPORTS", "3E MatterSphere Reports", "").Text;
            ucNavApplications.Controls.Add(ucApp1);
            ucNavApplications.Refresh(true);
        }

        public override void ApplicationLinkClicked(ucNavCmdButtons linkButton)
        {
            if (linkButton.Name == "ucApp1")
            {
                try
                {
                    System.Diagnostics.Process.Start("omsreports.exe");
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ex);
                }
            }
        }


        frmFullSystemUpdate frmFullSystemUpdate;

        void frmFullSystemUpdate_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (frmFullSystemUpdate.somethingSucceeded)
            {
                frmFullSystemUpdate.Cursor = Cursors.WaitCursor;
                OnPackageInstalled();
                frmFullSystemUpdate.Cursor = Cursors.Default;
            }
        }

        public override void SystemUpdateClick(ucDockMainView ucHome2, EventArgs e)
        {
            try
            {
                new SystemPermission(Permission.StandardTypeToString(StandardPermissionType.AdminKit)).Check();

                FWBS.OMS.ReportingServer reportingServer = new ReportingServer("FWBS Limited 2005");
                try { reportingServer.Connection.ExecuteSQL("INSERT INTO dbCaptainsLogType (typeID, typeCode, typeSeverity, typeGroup, typeSystem) VALUES (40,'PACKAGE',255, 'PACKAGE',1)", new IDataParameter[0]); }
                catch { }
                try { reportingServer.Connection.ExecuteSQL("INSERT INTO dbCaptainsLogType (typeID, typeCode, typeSeverity, typeGroup, typeSystem) VALUES (41,'PACKEXC',255, 'PACKEXC',1)", new IDataParameter[0]); }
                catch { }
                try { reportingServer.Connection.ExecuteSQL("INSERT INTO dbCaptainsLogType (typeID, typeCode, typeSeverity, typeGroup, typeSystem) VALUES (42,'PACKERR',255, 'PACKERR',1)", new IDataParameter[0]); }
                catch { }

                var owner = Application.OpenForms.FirstOrDefault<Form>();

                using (frmFullSystemUpdate = new frmFullSystemUpdate(this))
                {
                    frmFullSystemUpdate.FormClosing -= new FormClosingEventHandler(frmFullSystemUpdate_FormClosing);
                    frmFullSystemUpdate.FormClosing += new FormClosingEventHandler(frmFullSystemUpdate_FormClosing);

                    if (frmFullSystemUpdate.ShowDialog(owner) == DialogResult.OK)
                    {
                        object obj;

                        this.PackageDeployment.ProcessManifest(owner, frmFullSystemUpdate.Filename, null, null, null, true, out obj, true);
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex);
            }
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

            var _power = Session.CurrentSession.CurrentPowerUserSettings;
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
                    frmAdminDesktop1 = base.OpenWindows[filter] as frmAdminDesktop;
                    if (frmAdminDesktop1 == null)
                    {
                        frmAdminDesktop1 = new frmAdminDesktop();
                        frmAdminDesktop1.Tag = filter;
                        base.OpenWindows.Add(filter, frmAdminDesktop1);
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
                else if (base.OpenWindows[ecmd + filter] == null)
                {
                    frmGenEngine frmGenEngine1 = new frmGenEngine(ecmd);
                    base.OpenWindows.Add(ecmd + filter, frmGenEngine1);
                    frmGenEngine1.NavActions.BringToFront();
                    frmGenEngine1.Closing += new CancelEventHandler(frmAdminDesktop1_Closing);
                    frmGenEngine1.Tag = ecmd + filter;
                    frmGenEngine1.Show();
                }
                else
                {
                    frmGenEngine frm = (frmGenEngine)OpenWindows[ecmd + filter];
                    frm.BringToFront();
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
            finally
            {
            }
            return returnval;
        }

        private void frmAdminDesktop1_Closing(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
                return;

            try
            {
                OpenWindows.Remove(Convert.ToString(((Form)sender).Tag));
                ((Form)sender).Dispose();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
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
                FWBS.OMS.UI.Windows.Services.ShowPrecedentLibrary(Application.OpenForms.FirstOrDefault<Form>(), null, FWBS.OMS.UI.Windows.Services.SelectAssociate(), "LETTERHEAD", "");
                result = true;
            }
            else if (ecmd == "ASS")
            {
                FWBS.OMS.UI.Windows.Services.SelectAssociate();
                result = true;
            }
            else if (ecmd == "CLMT")
            {
                FWBS.OMS.UI.Windows.Services.ShowFile(Application.OpenForms.FirstOrDefault<Form>(), true, "");
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
                    frmRichText1.ShowDialog();
                    result = true;
                }
            }
            else if (ecmd == "ED")
            {
                FWBS.OMS.Design.frmDesigner frmDesigner1;
                frmDesigner1 = OpenWindows[ecmd] as FWBS.OMS.Design.frmDesigner;
                if (frmDesigner1 == null)
                {
                    frmDesigner1 = new FWBS.OMS.Design.frmDesigner();
                    OpenWindows.Add(ecmd, frmDesigner1);
                    frmDesigner1.Tag = ecmd;
                    frmDesigner1.Closing += new CancelEventHandler(frmAdminDesktop1_Closing);
                    try
                    {
                        frmDesigner1.Show();
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ex);
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
                DefaultPowerProfile powersettings = new DefaultPowerProfile();
                FWBS.OMS.UI.Windows.Services.ShowOMSItem(null, "SCRPWRAVLROL", null, powersettings, new FWBS.Common.KeyValueCollection());
                result = true;
            }
            else if (ecmd == "SYS")
            {
                Services.Wizards.SaveSystemSettings(Application.OpenForms.FirstOrDefault<Form>());
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
                Services.Wizards.GetWizard(Application.OpenForms.FirstOrDefault<Form>(), code, null, mode, false, new Common.KeyValueCollection());
                result = true;
            }
            return null;
        }


        public override Control ConstructAdminElement(string filter, Control parent, string ecmd)
        {
            string command = ecmd + "|" + filter;
            var _power = Session.CurrentSession.CurrentPowerUserSettings;
            if (_power != null)
            {
                if (_power.CanRunAction(command.Trim('|')) == false)
                {
                    throw new FWBS.OMS.Security.PermissionsException("EXPERMDENIED2", "Permission Denied", true);
                }
            }

            Control returnval = null;
            if (ecmd == "SCHSYSMSTCONFIG")
            {
                ucMileStoneConfig ucMileStoneConfig1 = new ucMileStoneConfig(this, parent, new FWBS.Common.KeyValueCollection());
                ucMileStoneConfig1.Dock = DockStyle.Fill;
                returnval = ucMileStoneConfig1;
            }
            else if (ecmd == "MB")
            {
                ucCommandBarBuilder ucCommandBarBuilder1 = new ucCommandBarBuilder(this, parent, new FWBS.Common.KeyValueCollection());
                ucCommandBarBuilder1.Dock = DockStyle.Fill;
                returnval = ucCommandBarBuilder1;
            }
            else if (ecmd == "PD")
            {
                ucPackages ucPackages1 = new ucPackages(this, parent, new FWBS.Common.KeyValueCollection());
                ucPackages1.Dock = DockStyle.Fill;
                returnval = ucPackages1;
            }
            else if (ecmd == "RP")
            {
                FWBS.Common.KeyValueCollection gfilter = new FWBS.Common.KeyValueCollection();
                gfilter.Add("Group", filter);
                gfilter.Add("Type", DBNull.Value);
                gfilter.Add("Active", 1);
                ucReports ucReports1 = new ucReports(this, parent, gfilter);
                ucReports1.Dock = DockStyle.Fill;
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
                ucFormImporter1.Dock = DockStyle.Fill;
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
                ucScreen1.Dock = DockStyle.Fill;
                returnval = ucScreen1;
            }
            else if (ecmd == "EF")
            {
                bool wizard = false;
                string code = filter.Substring(0, filter.Length - 2);
                string mode = filter.Substring(filter.Length - 1);
                if (string.IsNullOrWhiteSpace(mode) || mode == "A")
                {
                    FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(code, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, null);
                    wizard = true;
                }
                else
                {
                    FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(code, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, null);
                    wizard = true;
                }
                if (!wizard)
                    returnval = new EnquiryForm();
                else
                    returnval = new NoDisplayControl();
            }
            else if (ecmd == "PDFI")
            {
                ucPDFImporter ucPDFImporter1 = new ucPDFImporter();
                ucPDFImporter1.Dock = DockStyle.Fill;
                returnval = ucPDFImporter1;
            }
            else if(ecmd == "DSH")
            {
                ucDashboards ucDashboards1 = new ucDashboards(this, parent, new FWBS.Common.KeyValueCollection());
                returnval = ucDashboards1;
            }

            return returnval;
        }
    }
}
