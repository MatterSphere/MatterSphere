using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class Milestones : ExportBase, IDisposable
    {
        #region Fields
        private DataSet _mslists = new DataSet("MILESTONE");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        #endregion

        #region Contructors
        public Milestones(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _code = Code;
            _treeview = TreeView;
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _mslists.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbMSConfig_OMS2K where MSCode = @Code", "MILESTONE", false, paramlist));
            _name = Convert.ToString(_mslists.Tables[0].Rows[0]["MSDescription"]);
            _desc = "Milestone [" + _code + "] " + Environment.NewLine + _name;
        }
        #endregion

        public static bool Exists(string Code)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            if (OMS.Session.CurrentSession.Connection.ExecuteSQLScalar("SELECT MSCode FROM dbMSConfig_OMS2K where MSCode = @Code", paramlist) != null)
                return true;
            else
                return false;
        }

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("Milestones");
            _directory = _directory.CreateSubdirectory(_code);

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _mslists.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

            treeviewParentID = TreeView.Add(30, _code, _code, this.Active, treeviewParentID, PackageTypes.Milestones, _desc, this.RootImportable, this.RunOnce);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _mslists.Dispose();
        }
        #endregion
    }
}
