using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class Scripts : ExportBase, IDisposable
    {
        #region Fields
        private DataSet _dsscripts = new DataSet("SCRIPTS");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        #endregion

        #region Contructors
        public Scripts(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _code = Code;
            _treeview = TreeView;
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsscripts.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbScript where scrCode = @Code", "SCRIPTS", false, paramlist));
            _name = _code;
            if (_dsscripts.Tables["SCRIPTS"].Rows.Count > 0)
                _desc = "Script [" + _code + "]" + Environment.NewLine + "Version : " + Convert.ToString(_dsscripts.Tables["SCRIPTS"].Rows[0]["scrVersion"]);
            else
                throw new Exception(Session.CurrentSession.Resources.GetMessage("ERRNOSCRFRCD", "Error no Script for Code : %1%", "", _code).Text);
        }
        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("Scripts");
            _directory = _directory.CreateSubdirectory(_code);

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dsscripts.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

            treeviewParentID = TreeView.Add(9, _code, _name, this.Active, treeviewParentID, PackageTypes.Scripts, _desc, this.RootImportable, this.RunOnce);
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
