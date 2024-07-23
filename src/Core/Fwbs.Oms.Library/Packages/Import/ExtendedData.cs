using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class ExtendedData : ImportBase, IDisposable
    {
        #region Fields
        DataSet _dsextend = new DataSet("EXTENDEDDATA");
        System.IO.FileInfo _filename;
        private CodeLookups _lookups;

        #endregion

        #region Contructors
        public ExtendedData(string FileName)
        {
            _dsextend.ReadXml(FileName);
            _filename = new System.IO.FileInfo(FileName);
            _source = _dsextend;
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            ImportTable _import = new ImportTable(this.Fieldreplacer);
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Convert.ToString(_dsextend.Tables["EXTENDEDDATA"].Rows[0]["extCode"]));

            string code = Convert.ToString(_dsextend.Tables["EXTENDEDDATA"].Rows[0]["extCode"]);
            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbExtendedData where extCode = @Code", "EXTENDEDDATA", false, paramlist);
            OnProgress("Importing Extended Data : " + code);

            bool create = true;
            long destinuser = (long)FWBS.OMS.SystemUsers.Admin;
            long sourceuser = destinuser;
            if (_table.Rows.Count > 0) sourceuser = FWBS.Common.ConvertDef.ToInt64(_table.Rows[0]["UpdatedBy"], Session.CurrentSession.Administrator.ID);

            if (sourceuser != destinuser)
            {
                string sourceusername = "{Unknown}";
                try { sourceusername = new User(Convert.ToInt32(sourceuser)).FullName; }
                catch { }
                AskEventArgs askargs = new AskEventArgs("ALLRDYEXISTV3", "The %1% [%2%] already exists. Do you want to overwrite ?" + Environment.NewLine + Environment.NewLine + "Last Modified By : %3%", "", FWBS.OMS.AskResult.No, "Extended Data", code, sourceusername);
                Session.CurrentSession.OnAsk(this, askargs);
                if (askargs.Result == FWBS.OMS.AskResult.Yes)
                    create = true;
                else
                    create = false;
            }

            if (create)
            {
                if (_table.Rows.Count > 0)
                    _import.ImportRowOver(_dsextend.Tables["EXTENDEDDATA"], _table, 0);
                else
                    _import.Import(_dsextend.Tables["EXTENDEDDATA"], _table);
                Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbExtendedData");
                string fromvalue = "";
                if (_dsextend.Tables.Contains("OBJECTS") && _dsextend.Tables["OBJECTS"].Rows.Count > 0)
                {
                    paramlist = new IDataParameter[1];
                    paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Convert.ToString(_dsextend.Tables["OBJECTS"].Rows[0]["objCode"]));
                    _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbOMSObjects where objCode = @Code", "OBJECTS", false, paramlist);
                    if (_table.Rows.Count > 0)
                        _import.ImportRowOver(_dsextend.Tables["OBJECTS"], _table, 0);
                    else
                        _import.Import(_dsextend.Tables["OBJECTS"], _table);
                    Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbOMSObjects");

                    _lookups = new CodeLookups(_dsextend.Tables["LOOKUPS"], "OMSOBJECT");
                    DataView msg = new DataView(_lookups.DataTable);
                    fromvalue = Convert.ToString(_dsextend.Tables["OBJECTS"].Rows[0]["objCode"]);
                    msg.RowFilter = "cdCode = '" + fromvalue + "'";
                    foreach (DataRowView drv in msg)
                    {
                        if (_lookups.CheckCodeExists(Convert.ToString(fromvalue)) == false)
                            _lookups.Create("OMSOBJECT", Convert.ToString(fromvalue), Convert.ToString(drv["cdDesc"]), Convert.ToString(drv["cdHelp"]), Convert.ToString(drv["cdUICultureInfo"]));
                    }
                }

                _lookups = new CodeLookups(_dsextend.Tables["LOOKUPS"], "EXTENDEDDATA");
                DataView msg1 = new DataView(_lookups.DataTable);
                fromvalue = Convert.ToString(_dsextend.Tables["EXTENDEDDATA"].Rows[0]["extCode"]);
                msg1.RowFilter = "cdCode = '" + fromvalue + "'";
                foreach (DataRowView drv in msg1)
                {
                    if (_lookups.CheckCodeExists(Convert.ToString(fromvalue)) == false)
                        _lookups.Create("EXTENDEDDATA", Convert.ToString(fromvalue), Convert.ToString(drv["cdDesc"]), Convert.ToString(drv["cdHelp"]), Convert.ToString(drv["cdUICultureInfo"]));
                }
            }
            return true;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dsextend.Dispose();
        }
        #endregion
    }
}
