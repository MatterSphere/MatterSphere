using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class SQLScripts : ExportBase, IDisposable
    {
        #region Fields
        private DataSet _dsscripts = new DataSet("SQLSCRIPTS");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        #endregion

        #region Contructors
        public SQLScripts(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _code = Code;
            _treeview = TreeView;
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsscripts.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbPackageSqlScripts where sqlID = @Code", "SQLSCRIPTS", false, paramlist));
            if (_dsscripts.Tables["SQLSCRIPTS"].Rows.Count == 0)
                throw new Exception("Sql Script does not exist.");
            _name = Convert.ToString(_dsscripts.Tables["SQLSCRIPTS"].Rows[0]["sqlName"]);
            _desc = Convert.ToString(_dsscripts.Tables["SQLSCRIPTS"].Rows[0]["sqlDescription"]);
        }
        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("SQLScripts");
            _directory = _directory.CreateSubdirectory(_code);

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dsscripts.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

            treeviewParentID = TreeView.Add(26, _code, _name, this.Active, treeviewParentID, PackageTypes.SQLScripts, _desc, this.RootImportable, false);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dsscripts.Dispose();
        }
        #endregion
    }
}
