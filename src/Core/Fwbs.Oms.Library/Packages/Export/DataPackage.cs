using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class DataPackage : ExportBase, IDisposable
    {
        #region Fields
        private DataSet _dsdatapack = new DataSet("DATAPACKAGE");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        #endregion

        #region Contructors
        protected internal DataPackage(string Code)
        {
            _code = Code;
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("UI", SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            _dsdatapack.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT *, dbo.GetCodeLookupDesc('PACKAGEDATA', pkdCode, @UI) as pkdDesc FROM dbPackageData where pkdCode = @Code", "PACKAGEHEAD", false, paramlist));
            _name = Convert.ToString(_dsdatapack.Tables["PACKAGEHEAD"].Rows[0]["pkdCode"]);
            _desc = Convert.ToString(_dsdatapack.Tables["PACKAGEHEAD"].Rows[0]["pkdDesc"]);
            FWBS.OMS.Design.Package.PackageData _datap = new FWBS.OMS.Design.Package.PackageData(_dsdatapack.Tables["PACKAGEHEAD"]);
            _dsdatapack.Tables.Add(_datap.GetTable());
            if (_dsdatapack.Tables["SOURCE"].Columns.Contains("rowguid"))
                _dsdatapack.Tables["SOURCE"].Columns.Remove("rowguid");
        }

        public DataPackage(string Code, TreeView TreeView)
            : this(Code)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _treeview = TreeView;
        }
        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("DataPackage");
            _directory = _directory.CreateSubdirectory(_code);

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dsdatapack.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

            treeviewParentID = TreeView.Add(38, _code, _name, this.Active, treeviewParentID, PackageTypes.DataPackages, _desc, this.RootImportable, this.RunOnce);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dsdatapack.Dispose();
        }
        #endregion
    }
}
