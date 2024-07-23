using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class CodeLookupType : ImportBase, IDisposable
    {
        #region Fields
        DataSet _dscodelk = new DataSet("CODELOOKUPS");
        System.IO.FileInfo _filename;
        #endregion

        #region Contructors
        public CodeLookupType(string FileName)
        {
            _dscodelk.ReadXml(FileName);
            _filename = new System.IO.FileInfo(FileName);
        }

        public CodeLookupType(DataTable FromTable)
        {
            _dscodelk.Tables.Add(FromTable);
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            ImportTable _import = new ImportTable(this.Fieldreplacer);
            IDataParameter[] paramlist = new IDataParameter[1];
            string typeslist = "";
            foreach (DataRow types in _dscodelk.Tables["CODELOOKUPS"].Rows)
            {
                string trytype = Convert.ToString(types["cdType"]);
                if (typeslist.IndexOf("'" + trytype + "'") == -1)
                    typeslist += Convert.ToString(trytype) + "','";
            }
            typeslist = typeslist.TrimEnd(",'".ToCharArray());
            OnProgress("Importing CodeLookup Group : " + typeslist);
            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where cdType in('" + typeslist + "') OR (cdCode in('" + typeslist + "') AND cdGroup = 1)", "CODELOOKUPS", false, paramlist);
            _import.NewOnlyImport(_dscodelk.Tables["CODELOOKUPS"], _table, "cdType,cdCode,cdUICultureInfo", true);
            Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbCodeLookup");
            return true;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dscodelk.Dispose();
        }
        #endregion
    }
}
