using System;
using System.Data;


namespace FWBS.OMS.Design.Import
{
    public class Global
    {
        #region Fields
        public event System.ComponentModel.CancelEventHandler ShowDialog;
        public event ProgressEventHandler Progress;
        private FWBS.OMS.Design.Export.TreeView _treeview;
        private int failed = 0;
        private bool _cancel = false;
        private int _packagecount = 0;
        public System.Windows.Forms.IWin32Window Owner { get; private set; }
        #endregion

        #region Constructors
        public Global(System.Windows.Forms.IWin32Window owner)
        {
            this.Owner = owner;
        }
        #endregion

        #region Factory Code
        private FWBS.OMS.Design.Import.ImportBase CreateImport(FWBS.OMS.Design.Export.PackageTypes type)
        {
            ImportLockStateChecker checker = new ImportLockStateChecker();
            FWBS.OMS.Design.Import.ImportBase _pkg = null;
            switch (this.TreeView.Type)
            {
                case FWBS.OMS.Design.Export.PackageTypes.None:
                    _pkg = null;
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.DataLists:
                    if (!checker.CheckObjectLockState(_treeview.Code, "DataList"))
                        _pkg = new FWBS.OMS.Design.Import.DataLists(@"DataLists\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.EnquiryForms:
                case FWBS.OMS.Design.Export.PackageTypes.Screens:
                    if (!checker.CheckObjectLockState(_treeview.Code, "EnquiryForm"))
                        _pkg = new FWBS.OMS.Design.Import.EnquiryForm(@"EnquiryForms\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.Reports:
                    _pkg = new FWBS.OMS.Design.Import.Report(@"Reports\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.SearchLists:
                    if (!checker.CheckObjectLockState(_treeview.Code, "SearchList"))
                        _pkg = new FWBS.OMS.Design.Import.SearchList(@"SearchList\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.Scripts:
                    if (!checker.CheckObjectLockState(_treeview.Code, "Script"))
                        _pkg = new FWBS.OMS.Design.Import.Scripts(@"Scripts\" + _treeview.Code + @"\manifest.xml", true);
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.CodeLookups:
                    _pkg = new FWBS.OMS.Design.Import.CodeLookupType(@"CodeLookups\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.Precedents:
                    if (!checker.CheckObjectLockState(_treeview.Code, "Precedent"))
                        _pkg = new FWBS.OMS.Design.Import.Precedent(@"Precedents\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.SQLScripts:
                    _pkg = new FWBS.OMS.Design.Import.SQLScripts(@"SQLScripts\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.ExtendedData:
                    _pkg = new FWBS.OMS.Design.Import.ExtendedData(@"ExtendedData\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.OMSObjects:
                    _pkg = new FWBS.OMS.Design.Import.OMSObjects(@"OMSObjects\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.DataPackages:
                    _pkg = new FWBS.OMS.Design.Import.DataPackage(@"DataPackage\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.FileManagement:
                    _pkg = new FWBS.OMS.Design.Import.FileManagement(@"FileManagement\" + _treeview.Code + @"\manifest.xml");
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.Milestones:
                    _pkg = new FWBS.OMS.Design.Import.Milestones(@"Milestones\" + _treeview.Code + @"\manifest.xml");
                    break;
                default:
                    _pkg = null;
                    break;
            }
            return _pkg;
        }

        private FWBS.OMS.Design.Export.ExportBase CreateExport(FWBS.OMS.Design.Export.PackageTypes type, FWBS.OMS.Design.Package.Packages backuppkg)
        {
            FWBS.OMS.Design.Export.ExportBase _enq = null;
            switch (this.TreeView.Type)
            {
                case FWBS.OMS.Design.Export.PackageTypes.None:
                    _enq = null;
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.DataLists:
                    if (EnquiryEngine.DataLists.Exists(_treeview.Code))
                        _enq = new FWBS.OMS.Design.Export.DataLists(_treeview.Code, backuppkg.TreeView);
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.Screens:
                case FWBS.OMS.Design.Export.PackageTypes.EnquiryForms:
                    if (EnquiryEngine.Enquiry.Exists(_treeview.Code))
                        _enq = new FWBS.OMS.Design.Export.EnquiryForm(_treeview.Code, backuppkg.TreeView);
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.Reports:
                    if (FWBS.OMS.SearchEngine.SearchList.Exists(_treeview.Code))
                        _enq = new FWBS.OMS.Design.Export.Report(_treeview.Code, backuppkg.TreeView);
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.SearchLists:
                    if (FWBS.OMS.SearchEngine.SearchList.Exists(_treeview.Code))
                        _enq = new FWBS.OMS.Design.Export.SearchList(_treeview.Code, backuppkg.TreeView);
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.Scripts:
                    if (FWBS.OMS.Script.ScriptGen.Exists(_treeview.Code))
                        _enq = new FWBS.OMS.Design.Export.Scripts(_treeview.Code, backuppkg.TreeView);
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.CodeLookups:
                    _enq = null;
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.Precedents:
                    _enq = null;
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.SQLScripts:
                    _enq = null;
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.ExtendedData:
                    _enq = null;
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.OMSObjects:
                    if (FWBS.OMS.OmsObject.Exists(_treeview.Code))
                        _enq = new FWBS.OMS.Design.Export.OMSObjects(_treeview.Code, backuppkg.TreeView);
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.DataPackages:
                    _enq = null;
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.FileManagement:
                    if (FWBS.OMS.Design.Export.FileManagement.Exists(_treeview.Code))
                        _enq = new FWBS.OMS.Design.Export.FileManagement(_treeview.Code, backuppkg.TreeView);
                    break;
                case FWBS.OMS.Design.Export.PackageTypes.Milestones:
                    if (FWBS.OMS.Design.Export.Milestones.Exists(_treeview.Code))
                        _enq = new FWBS.OMS.Design.Export.Milestones(_treeview.Code, backuppkg.TreeView);
                    break;
                default:
                    _enq = null;
                    break;
            }
            return _enq;
        }
        #endregion

        #region Public Method
        public int Import(string filename, bool backup, bool Copy, bool allowVersionWarning)
        {
            _treeview = new FWBS.OMS.Design.Export.TreeView();
            _treeview.Load(new System.IO.FileInfo(filename), allowVersionWarning);
            System.IO.FileInfo filei = new System.IO.FileInfo(filename.Replace("Manifest.xml", "Replacement"));
            if (filei.Exists)
                _treeview.FieldReplacer.Load(filei);

            System.ComponentModel.CancelEventArgs cancel = new System.ComponentModel.CancelEventArgs();
            if (ShowDialog != null) ShowDialog(this, cancel);
            if (cancel.Cancel) return -1;

            DataView nodes = new DataView(_treeview.Source);
            nodes.RowStateFilter = DataViewRowState.OriginalRows;
            nodes.RowStateFilter = DataViewRowState.CurrentRows;
            nodes.Sort = "ID";
            nodes.RowFilter = "RootImportable = true and Active = true";

            FWBS.OMS.Design.Package.Packages backuppkg = new FWBS.OMS.Design.Package.Packages(false);

            if (backup)
            {
                backuppkg.Code = this.TreeView.PackageCode;
                backuppkg.Help = this.TreeView.PackageHelp;
                backuppkg.Description = this.TreeView.PackageDescription;
                //UTCFIX: DM - 30/11/06 - Just incase, you never know.
                backuppkg.ExportPath = System.IO.Path.Combine(FWBS.OMS.Global.GetDBPersonalDataPath("Package Backups").FullName, DateTime.UtcNow.ToString("yyyyMMdd_HHmmss"));
            }

            foreach (System.Data.DataRowView dr in nodes)
            {
                try
                {
                    _treeview.Goto(dr);

                    try
                    {
                        var enq = CreateExport(this.TreeView.Type, backuppkg);
                        if (enq != null)
                        {
                            if (backup)
                            {
                                enq.TreeViewParentID = 0;
                                enq.ExportTo(backuppkg.ExportPath + "\\" + this.TreeView.PackageCode);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ImportFailed(ex, filename) != 0)
                            return failed;
                    }

                    var pkg = CreateImport(this.TreeView.Type);
                    if (pkg != null)
                    {
                        pkg.Fieldreplacer = _treeview.FieldReplacer;
                        pkg.CurrentRow = _treeview.RowView;
                        pkg.Progress += new ProgressEventHandler(OnProgress);
                        var BackupLocation = Environment.CurrentDirectory;
                        if (pkg.Execute(Copy) == false)
                            failed++;
                        else
                            FWBS.OMS.Logging.CaptainsLog.CreateEntry(40, this.TreeView.PackageCode + ", Version : " + this.TreeView.PackageVersion.ToString() + ", Item Code : " + this.TreeView.Code + " was successfully installed", Convert.ToInt64(this.TreeView.Type), this.TreeView.Code, false);
                        Environment.CurrentDirectory = BackupLocation;
                        ((IDisposable)pkg).Dispose();
                    }
                    else
                        failed++;

                    if (_cancel) return -1;
                }
                catch (Exception ex)
                {
                    failed++;
                    string errmessage = "Error Importing Item from Package %1%" + Environment.NewLine + Environment.NewLine + "%3%" + Environment.NewLine + Environment.NewLine + "Error Message : " + Environment.NewLine + Environment.NewLine + "      %4%" + Environment.NewLine + Environment.NewLine + "Do you want to Continue ?";
                    FWBS.OMS.Logging.CaptainsLog.CreateEntry(42, this.TreeView.PackageCode + ", Version : " + this.TreeView.PackageVersion.ToString() + ", Item Code : " + this.TreeView.Code + " backup errored", Convert.ToInt64(this.TreeView.Type), ex.Message, false);

                    AskEventArgs askargs = new AskEventArgs("ERRIMP", errmessage, "", FWBS.OMS.AskResult.No, new System.IO.FileInfo(filename).Name.Replace(".Manifest.xml", ""), _treeview.Code,
                    _treeview.Description, ex.Message);
                    FWBS.OMS.Session.CurrentSession.OnAsk(this, askargs);
                    if (askargs.Result == AskResult.No)
                        return failed;
                }
            }

            nodes.RowFilter = "RootImportable = true and Active = false";
            foreach (System.Data.DataRowView dr2 in nodes)
            {
                _treeview.Goto(dr2);
                FWBS.OMS.Logging.CaptainsLog.CreateEntry(41, this.TreeView.PackageCode + ", Version : " + this.TreeView.PackageVersion.ToString() + ", Item Code : " + this.TreeView.Code + " was excluded from the Installation", Convert.ToInt64(this.TreeView.Type), "", false);
            }

            if (backup)
            {
                backuppkg.CreateTreeViewXml(this.TreeView.PackageDescription, this.TreeView.PackageHelp);
            }

            if (this.TreeView.PackageCode != "")
            {
                if (FWBS.OMS.Design.Package.Packages.PackageExists(this.TreeView.PackageCode) == false)
                {
                    FWBS.OMS.Design.Package.Packages pk = new FWBS.OMS.Design.Package.Packages(!Copy);
                    pk.Code = this.TreeView.PackageCode;
                    pk.Help = this.TreeView.PackageHelp;
                    pk.Description = this.TreeView.PackageDescription;
                    pk.SetExtraInfo("pkgExternal", true);
                    pk.TreeView = this.TreeView;
                    pk.Version = Convert.ToInt32(this.TreeView.PackageVersion);
                    pk.Update();
                }
                else
                {
                    FWBS.OMS.Design.Package.Packages pk = FWBS.OMS.Design.Package.Packages.GetPackage(this.TreeView.PackageCode, !Copy);
                    pk.Code = this.TreeView.PackageCode;
                    pk.Help = this.TreeView.PackageHelp;
                    pk.Description = this.TreeView.PackageDescription;
                    pk.SetExtraInfo("pkgExternal", true);
                    pk.TreeView = this.TreeView;
                    pk.Version = Convert.ToInt32(this.TreeView.PackageVersion);
                    pk.SetExtraInfo("Created", System.DateTime.Now);
                    pk.Update();
                }
            }
            if (Copy)
            {
                Session.CurrentSession.OnWarning(this, new MessageEventArgs(Session.CurrentSession.Resources.GetMessage("CPPKGGTPKCNDDP", "You have copied a Package. Please go to Package and Deploy and Update this Package", "").Text));
            }

            return failed;
        }

        private int ImportFailed(Exception ex, string filename)
        {
            failed++;
            string errmessage = "Error in Backup for Package %1%" + Environment.NewLine + Environment.NewLine + "%3%" + Environment.NewLine + Environment.NewLine + "Error Message : " + Environment.NewLine + Environment.NewLine + "      %4%" + Environment.NewLine + Environment.NewLine + "Do you want to Continue ?";
            FWBS.OMS.Logging.CaptainsLog.CreateEntry(42, this.TreeView.PackageCode + ", Version : " + this.TreeView.PackageVersion.ToString() + ", Item Code : " + this.TreeView.Code + " backup errored", Convert.ToInt64(this.TreeView.Type), ex.Message, false);

            AskEventArgs askargs = new AskEventArgs("ERRBAK", errmessage, "", FWBS.OMS.AskResult.No, new System.IO.FileInfo(filename).Name.Replace(".Manifest.xml", ""), _treeview.Code,
            _treeview.Description, ex.Message);
            FWBS.OMS.Session.CurrentSession.OnAsk(this, askargs);
            if (askargs.Result == AskResult.No)
                return failed;
            else
                return 0;
        }
        #endregion

        #region Properties
        public FWBS.OMS.Design.Export.TreeView TreeView
        {
            get
            {
                return _treeview;
            }
        }

        public int PackageCount
        {
            get
            {
                if (_packagecount == 0)
                {
                    DataView dvc = new DataView(_treeview.Source);
                    dvc.RowFilter = "RootImportable = true";
                    _packagecount = dvc.Count;
                }
                return _packagecount;
            }
        }
        #endregion

        #region Protected Private
        protected bool OnProgress(string label)
        {
            if (Progress != null)
            {
                ProgressEventArgs e = new ProgressEventArgs(label);
                Progress(this, e);
                return e.Cancel;
            }
            else
                return false;
        }

        private void OnProgress(object sender, ProgressEventArgs e)
        {
            _cancel = OnProgress(e.Label);
        }
        #endregion

        #region Static
        /// <summary>
        /// Has the Package Item been installed before not depended on the Package
        /// </summary>
        /// <param name="code">The Package Code Type</param>
        /// <param name="type">The Package Item Tyoe</param>
        /// <returns>Returns True if previously installed</returns>
        public static bool IsPackageItemInstalled(string code, FWBS.OMS.Design.Export.PackageTypes type)
        {
            string sql = "SELECT * FROM dbCaptainsLog WHERE logTypeID = 40 AND logDesc Like '%Item Code : {0} was%' AND logDataN = {1}";
            sql = String.Format(sql, code.Replace("'", "''"), Convert.ToInt64(type));
            DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(sql, "PACKAGE", new IDataParameter[0]);
            return (data.Rows.Count == 0);
        }
        #endregion
    }
}
