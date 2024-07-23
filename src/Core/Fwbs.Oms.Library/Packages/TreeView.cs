using System;
using System.Data;
using FWBS.Common;

namespace FWBS.OMS.Design.Export
{
    public class TreeView
    {
        #region Fields
        protected DataTable _data;
        protected DataRowView _rw = null;
        protected int _pos = -1;
        protected DataView _view = null;
        protected FieldReplacer _fieldreplacer = null;
        protected string packageCode = "";
        public Int64 PackageVersion = 0;
        public string PackageDescription = "";
        public string PackageHelp = "";
        public string PackageRequiredLicenses = "";
        protected FWBS.OMS.Design.Package.DependentCollection _dependents;
        public bool PackageIsBackup = false;
        public TreeViewItem _treeviewitem;
        private string[] _conditional;
        private string _afterinstall;
        #endregion

        #region Constructors
        public TreeView()
        {
            _dependents = new FWBS.OMS.Design.Package.DependentCollection();
            _data = new DataTable("TREEVIEW");
            _data.Columns.Add("ID", typeof(System.Int32));
            _data.Columns.Add("ImageIndex", typeof(System.Int32));
            _data.Columns.Add("Name", typeof(System.String));
            _data.Columns.Add("Code", typeof(System.String));
            _data.Columns.Add("Active", typeof(System.Boolean));
            _data.Columns.Add("Parent", typeof(System.Int32));
            _data.Columns.Add("Type", typeof(FWBS.OMS.Design.Export.PackageTypes));
            _data.Columns.Add("Description", typeof(System.String));
            _data.Columns.Add("InstallOnce", typeof(System.Boolean));
            _data.Columns.Add("RootImportable", typeof(System.Boolean));
            _data.Columns.Add("TreeViewItem", typeof(TreeViewItem));

            _data.Columns["ID"].AutoIncrement = true;
            _data.Columns["ID"].AutoIncrementSeed = 1;
            _data.Columns["ID"].AllowDBNull = false;
            _data.PrimaryKey = new DataColumn[4] { _data.Columns["Code"], _data.Columns["ImageIndex"], _data.Columns["Parent"], _data.Columns["Type"] };
            _data.Columns["Parent"].DefaultValue = 0;

            _fieldreplacer = new FieldReplacer();
        }

        public TreeView(DataTable Table)
        {
            _data = Table;
        }
        #endregion

        #region Public
        public int Add(int ImageIndex, string Code, string Name, bool Active, int Parent, string ParentPath, FWBS.OMS.Design.Export.PackageTypes Type, string Description, bool RootImportable, bool InstallOnce)
        {
            string[] paths = ParentPath.Split("/".ToCharArray());
            int ParentID = Parent;
            for (int i = 0; i < paths.Length; i++)
            {
                string _desc = "";
                string[] param = paths[i].Split(",".ToCharArray());
                if (param.Length == 3) _desc = param[2];
                if ((param.Length == 1 && paths[i] != "") || (param.Length > 1 && param[0] != ""))
                {
                    if (param.Length == 1)
                        _data.DefaultView.RowFilter = "Code = '" + paths[i].Replace("'", "''") + "' AND [Parent] = " + ParentID.ToString();
                    else
                        _data.DefaultView.RowFilter = "Code = '" + param[0].Replace("'", "''") + "' AND [Parent] = " + ParentID.ToString();
                    if (_data.DefaultView.Count == 0)
                    {
                        if (param.Length == 1)
                            ParentID = Add(ImageIndex, paths[i], paths[i], true, ParentID, PackageTypes.None, _desc, false, InstallOnce);
                        else
                            ParentID = Add(Convert.ToInt32(param[1]), param[0], param[0], true, ParentID, PackageTypes.None, _desc, false, InstallOnce);
                    }
                    else
                        ParentID = Convert.ToInt32(_data.DefaultView[0]["ID"]);
                }
            }
            return Add(-1, ImageIndex, Code, Name, Active, ParentID, Type, Description, RootImportable, InstallOnce);
        }

        public int Add(int ImageIndex, string Code, string Name, bool Active, int Parent, FWBS.OMS.Design.Export.PackageTypes Type, string Description, bool RootImportable, bool InstallOnce)
        {
            return Add(-1, ImageIndex, Code, Name, Active, Parent, Type, Description, RootImportable, InstallOnce);
        }

        public int Add(int ID, int ImageIndex, string Code, string Name, bool Active, int Parent, FWBS.OMS.Design.Export.PackageTypes Type, string Description, bool RootImportable, bool InstallOnce)
        {
            DataRow dr = _data.NewRow();
            if (ID != -1)
                dr["ID"] = ID;
            dr["ImageIndex"] = ImageIndex;
            dr["Code"] = Code;
            if (Name.Length > 0)
                dr["Name"] = Name.ToUpper().Substring(0, 1) + Name.ToLower().Substring(1);
            dr["Active"] = Active;
            dr["Parent"] = Parent;
            dr["Type"] = Type;
            dr["Description"] = Description;
            dr["RootImportable"] = RootImportable;
            dr["InstallOnce"] = InstallOnce;
            dr["TreeViewItem"] = new TreeViewItem(dr);

            _data.Rows.Add(dr);
            Goto(Convert.ToInt32(dr["ID"]));
            return this.ID;
        }

        public void RemoveChildren(DataRow Row)
        {
            int parent = Convert.ToInt32(Row["ID"]);
            DataView _childs = new DataView(_data, "[Parent] = " + parent.ToString(), "", DataViewRowState.CurrentRows);
            if (_childs.Count > 0)
                for (int i = _childs.Count - 1; i > -1; i--)
                    RemoveChildren(_childs[i].Row);
            Row.Delete();
        }

        public void RemoteAt(int Index)
        {
            try
            {
                _data.DefaultView[Index].Delete();
            }
            catch
            { }
        }

        public void Remove(DataRowView Row)
        {
            Row.Row.Delete();
        }

        public bool Goto(int ID)
        {
            _view = new DataView(_data, "ID = " + ID.ToString(), "", DataViewRowState.CurrentRows);
            _pos = -1;
            if (_view.Count > 0)
            {
                _rw = _view[0];
                return true;
            }
            else
                return false;
        }

        public bool GotoRoot()
        {
            _view = new DataView(_data, "Code = 'ROOT'", "", DataViewRowState.CurrentRows);
            _pos = -1;
            if (_view.Count > 0)
            {
                _rw = _view[0];
                return true;
            }
            else
                return false;
        }

        public void Goto(DataRowView Row)
        {
            _rw = Row;
        }

        public void Parent()
        {
            if (this.ParentID != 0)
            {
                _view = new DataView(_data, "ID = " + this.ParentID, "", DataViewRowState.CurrentRows);
                _pos = -1;
                if (_view.Count > 0)
                {
                    _rw = _view[0];
                }
            }
        }

        public void Clear()
        {
            _data.Rows.Clear();
            _data.AcceptChanges();
        }

        public int Count
        {
            get
            {
                return _data.DefaultView.Count;
            }
        }

        public DataRowView RowView
        {
            get
            {
                return _rw;
            }
        }

        public void Load(System.IO.FileInfo filename, bool allowVersionWarning)
        {
            DataSet _dataset = new DataSet();
            _dataset.ReadXml(filename.FullName);
            PackageDescription = "";
            PackageHelp = "";
            PackageRequiredLicenses = "";

            if (_dataset.DataSetName == "config" || (_dataset.Tables.Contains("config") && _dataset.Tables.Contains("Items")))
            {
                if (_dataset.Tables.Contains("config"))
                {
                    if (_dataset.Tables["config"].Columns.Contains("Code"))
                        PackageCode = Convert.ToString(_dataset.Tables["config"].Rows[0]["Code"]);
                    if (_dataset.Tables["config"].Columns.Contains("Version"))
                        PackageVersion = FWBS.Common.ConvertDef.ToInt64(_dataset.Tables["config"].Rows[0]["Version"], 0);
                    if (_dataset.Tables["config"].Columns.Contains("Licenses"))
                        PackageRequiredLicenses = Convert.ToString(_dataset.Tables["config"].Rows[0]["Licenses"]);
                    if (_dataset.Tables["config"].Columns.Contains("Backup"))
                        PackageIsBackup = Convert.ToBoolean(_dataset.Tables["config"].Rows[0]["Backup"]);
                    if (_dataset.Tables["config"].Columns.Contains("Conditional"))
                        _conditional = Convert.ToString(_dataset.Tables["config"].Rows[0]["Conditional"]).Split("|".ToCharArray());
                    if (_dataset.Tables["config"].Columns.Contains("AfterInstall"))
                        _afterinstall = Convert.ToString(_dataset.Tables["config"].Rows[0]["AfterInstall"]);

                }

                if (Package.Packages.PackageExists(PackageCode))
                {
                    Package.Packages g = FWBS.OMS.Design.Package.Packages.GetPackage(PackageCode, true);
                    if (g.Version > PackageVersion)
                    {
                        AskEventArgs askargs = new AskEventArgs("VERWARN2A", "%1% package version '%2%' is less than the installed package version '%3%' Do you wish to continue.", "", FWBS.OMS.AskResult.No, PackageCode, PackageVersion.ToString(), g.Version.ToString());
                        FWBS.OMS.Session.CurrentSession.OnAsk(this, askargs);
                        if (askargs.Result == AskResult.No)
                            throw new Exception("Update Cancelled ... ");
                    }
                }

                try
                {
                    if (_conditional != null)
                        Session.CurrentSession.ValidateConditional(null, _conditional, true);
                }
                catch (Exception ex)
                {
                    throw new Exception(String.Format("The Package '{1}' {0} before it can be installed", ex.Message, PackageCode));
                }

                string[] codes = PackageRequiredLicenses.Split(",".ToCharArray());
                foreach (string code in codes)
                {
                    if (code != "" && Session.CurrentSession.IsLicensedFor(code) == false)
                        throw new OMSException2("LICREQED", "The Package '%1%' requires a License '%2%' before it can be installed", "", new Exception(), true, PackageCode, code);
                }

                if (_dataset.Tables.Contains("Dependents"))
                {
                    DataTable installed = FWBS.OMS.Design.Package.Packages.GetImportedPackageList();
                    foreach (DataRow dr in _dataset.Tables["Dependents"].Rows)
                    {
                        _dependents.Add(new FWBS.OMS.Design.Package.Dependent(Convert.ToString(dr["Code"]), Convert.ToInt32(dr["Version"]), Convert.ToBoolean(dr["VersionWarning"]), Convert.ToBoolean(dr["VersionError"])));
                        DataView checkinstalled = new DataView(installed);
                        checkinstalled.RowFilter = "pkgCode = '" + Convert.ToString(dr["Code"]) + "'";
                        bool vw = Convert.ToBoolean(dr["VersionWarning"]);
                        bool ve = Convert.ToBoolean(dr["VersionError"]);
                        int v = Convert.ToInt32(dr["Version"]);
                        if (checkinstalled.Count > 0)
                        {
                            int vi = Convert.ToInt32(checkinstalled[0]["pkgVersion"]);
                            if (v > vi)
                            {
                                if (vw)
                                {
                                    if (allowVersionWarning)
                                        Session.CurrentSession.OnWarning(this, new MessageEventArgs(Session.CurrentSession.Resources.GetMessage("VERWARN3", "%1% package requires package '%2%' with a version '%3%' you have installed version '%4%'", "", true, "Warning", Convert.ToString(dr["Code"]), v.ToString(), vi.ToString()).Text));
                                    else
                                        throw new PackageException("VERWARN3", "%1% package requires package '%2%' with a version '%3%' you have installed version '%4%'", "", new Exception(), true, "Error", Convert.ToString(dr["Code"]), v.ToString(), vi.ToString());
                                }

                                if (ve)
                                    throw new PackageException("VERWARN3", "%1% package requires package '%2%' with a version '%3%' you have installed version '%4%'", "", new Exception(), true, "Error", Convert.ToString(dr["Code"]), v.ToString(), vi.ToString());
                            }
                        }
                        else
                        {
                            throw new PackageException("VERWARNMIS", "The Package requires package '%1%' with a version of '%2%'", "", new Exception(), true, Convert.ToString(dr["Code"]), v.ToString());
                        }
                    }
                }

                if (_dataset.Tables["Items"].Columns.Contains("InstallOnce") == false)
                    _dataset.Tables["Items"].Columns.Add("InstallOnce", typeof(Boolean));

                foreach (DataRow dr in _dataset.Tables["Items"].Rows)
                {
                    FWBS.OMS.Design.Export.PackageTypes _type = (FWBS.OMS.Design.Export.PackageTypes)FWBS.Common.ConvertDef.ToEnum(dr["Type"], FWBS.OMS.Design.Export.PackageTypes.None);
                    string _desc = "";
                    if (_dataset.Tables["Items"].Columns.Contains("Description"))
                    {
                        _desc = Convert.ToString(dr["Description"]);
                        if (PackageHelp == "") PackageHelp = _desc;
                    }
                    if (_dataset.Tables["Items"].Columns.Contains("Name") && PackageDescription == "")
                        PackageDescription = Convert.ToString(dr["Name"]);
                    Add(Convert.ToInt32(dr["ID"]), Convert.ToInt32(dr["ImageIndex"]), Convert.ToString(dr["Code"]), Convert.ToString(dr["Name"]), Convert.ToBoolean(dr["Active"]), Convert.ToInt32(dr["ParentID"]), _type, _desc, Convert.ToBoolean(dr["RootImportable"]), ConvertDef.ToBoolean(dr["InstallOnce"], false));
                }
            }
        }

        public int RootID()
        {
            _view = new DataView(_data, "Code = 'ROOT'", "", DataViewRowState.CurrentRows);
            _pos = -1;
            if (_view.Count > 0)
                return Convert.ToInt32(_view[0]["ID"]);
            else
                return -1;
        }
        #endregion

        #region Properties
        public string PackageCode
        {
            get
            {
                return packageCode;
            }
            internal protected set
            {
                packageCode = value;
            }
        }

        public FWBS.OMS.Design.Package.DependentCollection DependentPackages
        {
            get
            {
                return _dependents;
            }
        }

        public string AfterInstall
        {
            get
            {
                return _afterinstall;
            }
        }

        public FieldReplacer FieldReplacer
        {
            get
            {
                return _fieldreplacer;
            }
            set
            {
                _fieldreplacer = value;
            }
        }

        public int ID
        {
            get
            {
                if (_rw != null)
                    return Convert.ToInt32(_rw["ID"]);
                else
                    return 0;
            }
        }

        public int ImageIndex
        {
            get
            {
                if (_rw != null)
                    return Convert.ToInt32(_rw["ImageIndex"]);
                else
                    return -1;
            }
            set
            {
                if (_rw != null)
                    _rw["ImageIndex"] = value;
            }
        }

        public string Code
        {
            get
            {
                if (_rw != null)
                    return Convert.ToString(_rw["Code"]);
                else
                    return "";
            }
            set
            {
                if (_rw != null)
                    _rw["Code"] = value;
            }
        }

        public string Description
        {
            get
            {
                if (_rw != null)
                    return Convert.ToString(_rw["Description"]);
                else
                    return "";
            }
            set
            {
                if (_rw != null)
                    _rw["Description"] = value;
            }
        }

        public string Name
        {
            get
            {
                if (_rw != null)
                    return Convert.ToString(_rw["Name"]);
                else
                    return "";
            }
            set
            {
                if (_rw != null)
                    _rw["Name"] = value;
            }
        }

        public bool Active
        {
            get
            {
                if (_rw != null)
                    return Convert.ToBoolean(_rw["Active"]);
                else
                    return false;
            }
            set
            {
                if (_rw != null)
                    _rw["Active"] = value;
            }
        }

        public bool RootImportable
        {
            get
            {
                if (_rw != null)
                    return Convert.ToBoolean(_rw["RootImportable"]);
                else
                    return false;
            }
            set
            {
                if (_rw != null)
                    _rw["RootImportable"] = value;
            }
        }

        public bool InstallOnce
        {
            get
            {
                if (_rw != null)
                    return Convert.ToBoolean(_rw["InstallOnce"]);
                else
                    return false;
            }
            set
            {
                if (_rw != null)
                    _rw["InstallOnce"] = value;
            }
        }

        public int ParentID
        {
            get
            {
                if (_rw != null)
                    return Convert.ToInt32(_rw["Parent"]);
                else
                    return -1;
            }
            set
            {
                if (_rw != null)
                    _rw["Parent"] = value;
            }
        }

        public TreeViewItem TreeViewItem
        {
            get
            {
                return _treeviewitem;
            }
        }

        public PackageTypes Type
        {
            get
            {
                if (_rw != null)
                    return (PackageTypes)_rw["Type"];
                else
                    return PackageTypes.None;
            }
            set
            {
                if (_rw != null)
                    _rw["Type"] = value;
            }
        }

        public DataTable Source
        {
            get
            {
                return _data;
            }
        }

        public DataTable Copy
        {
            get
            {
                return _data.Copy();
            }
        }

        public bool EOF
        {
            get
            {
                return (_pos == _data.DefaultView.Count);
            }
        }
        #endregion
    }
}
