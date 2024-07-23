using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class FileManagement : ExportBase, IDisposable
    {
        #region Fields
        private DataSet _dslists = new DataSet("FILEMANAGEMENT");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        private string _xml = "";
        #endregion

        #region Contructors
        public FileManagement(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _code = Code;
            _name = CodeLookup.GetLookup("FMAPPLICATION", Code);
            _treeview = TreeView;
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dslists.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbFileManagementApplication where appCode = @Code", "FILEMANAGEMENT", false, paramlist));

            _xml = Convert.ToString(_dslists.Tables["FILEMANAGEMENT"].Rows[0]["appXML"]);

            _desc = "File Management [" + _code + "] " + Environment.NewLine + _name;

            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dslists.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where (cdType = 'FMAPPLICATION' AND cdCode = @Code)", "LOOKUPS", false, paramlist));
        }
        #endregion

        public static bool Exists(string Code)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            if (OMS.Session.CurrentSession.Connection.ExecuteSQLScalar("SELECT appcode FROM dbFileManagementApplication where appCode = @Code", paramlist) != null)
                return true;
            else
                return false;
        }

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("FileManagement");
            _directory = _directory.CreateSubdirectory(_code);

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dslists.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

            treeviewParentID = TreeView.Add(6, _code, _code, this.Active, treeviewParentID, PackageTypes.FileManagement, _desc, this.RootImportable, this.RunOnce);

            CodeLookupType codes = new CodeLookupType("FMACTIONS", TreeView, _xml, "lookup=\"{0}\"");
            codes.TreeViewParentID = treeviewParentID;
            codes.RootImportable = false;
            codes.ExportTo(Directory);
            codes.Dispose();

            string script = Convert.ToString(_dslists.Tables["FILEMANAGEMENT"].Rows[0]["appScript"]);
            if (script != "")
            {
                Scripts _script = new Scripts(script, TreeView);
                _script.TreeViewParentID = treeviewParentID;
                _script.RootImportable = false;
                _script.ExportTo(Directory);
                _script.Dispose();
            }
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dslists.Dispose();
        }
        #endregion
    }
}
