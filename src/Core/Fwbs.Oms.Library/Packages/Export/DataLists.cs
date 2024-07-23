using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class DataLists : ExportBase, IDisposable
    {
        #region Fields
        private DataSet _dslists = new DataSet("DATALISTS");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        #endregion

        #region Contructors
        public DataLists(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _code = Code;
            _name = CodeLookup.GetLookup("ENQDATALIST", Code);
            _treeview = TreeView;
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dslists.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbEnquiryDataList where enqTable = @Code", "DATALISTS", false, paramlist));
            _desc = "Data List [" + _code + "] " + Environment.NewLine + _name + Environment.NewLine + "Version : " + Convert.ToString(_dslists.Tables[0].Rows[0]["enqDLVersion"]);

            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dslists.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where (cdType = 'ENQDATALIST' AND cdCode = @Code)", "LOOKUPS", false, paramlist));

            string call = Convert.ToString(_dslists.Tables["DATALISTS"].Rows[0]["enqCall"]);
            string omstype = Convert.ToString(_dslists.Tables["DATALISTS"].Rows[0]["enqSourceType"]);

            if (call.ToUpper().StartsWith("SELECT ") == false && omstype != "INSTANCE" && omstype != "DYNAMIC")
            {
                paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("name", call);
                _dslists.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteProcedureTable("sprExportProcedure", "STOREPROC", false, paramlist));
                _desc += Environment.NewLine + Environment.NewLine + "Stored Procedure : " + call;
                if (_desc.Length >= 490) _desc = _desc.Substring(0, 490) + "...";
            }

        }
        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("DataLists");
            _directory = _directory.CreateSubdirectory(_code);

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dslists.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

            treeviewParentID = TreeView.Add(18, _code, _code, this.Active, treeviewParentID, PackageTypes.DataLists, _desc, this.RootImportable, this.RunOnce);
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
