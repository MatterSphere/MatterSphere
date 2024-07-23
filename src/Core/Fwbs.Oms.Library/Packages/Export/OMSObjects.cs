using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class OMSObjects : ExportBase, IDisposable
    {
        #region Fields
        private DataSet _dsscripts = new DataSet("OMSOBJECTS");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        #endregion

        #region Contructors
        public OMSObjects(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _code = Code;
            _treeview = TreeView;

            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsscripts.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbOMSObjects where objCode = @Code", "OMSOBJECTS", false, paramlist));
            _name = _code;
            if (_dsscripts.Tables["OMSOBJECTS"].Rows.Count > 0)
                _desc = "OMS Object [" + _code + "]" + Environment.NewLine + "Type Compatible : " + Convert.ToString(_dsscripts.Tables["OMSOBJECTS"].Rows[0]["ObjTypeCompatible"]);
            else
                throw new Exception(Session.CurrentSession.Resources.GetMessage("ERRNOOMSOBJFRCD", "Error no OMS Object for Code : %1%", "", _code).Text);

            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsscripts.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where cdCode = @Code and cdType = 'OMSOBJECT'", "CODELOOKUP", false, paramlist));

        }
        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("OMSObjects");
            _directory = _directory.CreateSubdirectory(_code);

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dsscripts.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

            treeviewParentID = TreeView.Add(29, _code, _name, this.Active, treeviewParentID, PackageTypes.OMSObjects, _desc, this.RootImportable, this.RunOnce);
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
