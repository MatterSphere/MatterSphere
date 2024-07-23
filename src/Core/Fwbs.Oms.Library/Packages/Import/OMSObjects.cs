using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class OMSObjects : ImportBase, IDisposable
    {
        #region Fields
        private DataSet _dsomsobjects = new DataSet("OMSOBJECTS");
        private System.IO.FileInfo _filename;
        private bool _confirmversion = false;
        #endregion

        #region Contructors
        public OMSObjects(string FileName, bool ConfirmVersion)
        {
            _dsomsobjects.ReadXml(FileName);
            _source = _dsomsobjects;
            _filename = new System.IO.FileInfo(FileName);
            _confirmversion = ConfirmVersion;
        }

        public OMSObjects(string FileName)
        {
            _dsomsobjects.ReadXml(FileName);
            _source = _dsomsobjects;
            _filename = new System.IO.FileInfo(FileName);
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            ImportTable _import = new ImportTable(this.Fieldreplacer);
            IDataParameter[] paramlist = new IDataParameter[2];
            string code = Convert.ToString(_dsomsobjects.Tables["OMSOBJECTS"].Rows[0]["objCode"]);

            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);
            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbOMSObjects where objCode = @Code", "OMSOBJECTS", false, paramlist);
            OnProgress("Importing OMS Object : " + code);
            if (_table.Rows.Count > 0)
                _import.ImportRowOver(_dsomsobjects.Tables["OMSOBJECTS"], _table, 0);
            else
                _import.Import(_dsomsobjects.Tables["OMSOBJECTS"], _table);
            Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbOMSObjects");

            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);
            _table = Session.CurrentSession.Connection.ExecuteSQLTable("DELETE FROM dbCodeLookup WHERE cdCode = @Code AND cdType = 'OMSOBJECT' SELECT * FROM dbCodeLookup where cdCode = @Code AND cdType = 'OMSOBJECT'", "OMSOBJECTS", false, paramlist);
            OnProgress("Importing OMS Object Code Lookups");
            _import.Import(_dsomsobjects.Tables["CODELOOKUP"], _table);
            Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbCodeLookup");
            return true;

        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dsomsobjects.Dispose();
        }
        #endregion
    }
}
