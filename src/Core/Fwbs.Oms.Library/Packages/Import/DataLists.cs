using System;
using System.Data;
using FWBS.Common;

namespace FWBS.OMS.Design.Import
{
    internal class DataLists : ImportBase, IDisposable
    {
        #region Fields
        DataSet _dslists = new DataSet("DATALISTS");
        System.IO.FileInfo _filename;
        #endregion

        #region Contructors
        public DataLists(string FileName)
        {
            _dslists.ReadXml(FileName);
            _filename = new System.IO.FileInfo(FileName);
            _source = _dslists;
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            ImportTable _import = new ImportTable(this.Fieldreplacer);
            IDataParameter[] paramlist = new IDataParameter[1];
            string code = Convert.ToString(_dslists.Tables["DATALISTS"].Rows[0]["enqTable"]);
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);

            OnProgress("Importing Data List : " + code);

            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbEnquiryDataList where enqTable = @Code", "DATALISTS", false, paramlist);

            long sourceversion;

            if (_dslists.Tables["DATALISTS"].Columns.Contains("enqDLVersion"))
                sourceversion = ConvertDef.ToInt64(_dslists.Tables["DATALISTS"].Rows[0]["enqDLVersion"], 1);
            else
                sourceversion = 1;

            long destinversion = (_table.Rows.Count == 0) ? 1 : ConvertDef.ToInt64(_table.Rows[0]["enqDLVersion"], 1);

            bool create = true;
            long destinuser = (long)FWBS.OMS.SystemUsers.Admin;
            long sourceuser = destinuser;
            if (_table.Rows.Count > 0) sourceuser = FWBS.Common.ConvertDef.ToInt64(_table.Rows[0]["UpdatedBy"], Session.CurrentSession.Administrator.ID);

            if (sourceuser != destinuser || (sourceuser == destinuser && sourceversion < destinversion))
            {
                string sourceusername = "{Unknown}";
                try { sourceusername = new User(Convert.ToInt32(sourceuser)).FullName; }
                catch { }
                AskEventArgs askargs = null;

                if (sourceuser != destinuser)
                    askargs = new AskEventArgs("ALLRDYEXISTV2", "The %1% [%2%] already exists. Do you want to overwrite %1% version %3% with version %4% ?" + Environment.NewLine + Environment.NewLine + "Last Modified By : %5%", "", FWBS.OMS.AskResult.No, "Data List", code, destinversion.ToString(), sourceversion.ToString(), sourceusername);
                else
                    askargs = new AskEventArgs("ALLRDYEXISTV1", "The %1% [%2%] already exists. Do you want to overwrite %1% version %3% with version %4% ?", "", FWBS.OMS.AskResult.No, "Data List", code, destinversion.ToString(), sourceversion.ToString());

                Session.CurrentSession.OnAsk(this, askargs);
                if (askargs.Result == FWBS.OMS.AskResult.Yes)
                    create = true;
                else
                    create = false;
            }


            if (create)
            {
                if (_table.Rows.Count > 0)
                {
                    _import.ImportRowOver(AddEnqDLVersionColumn(_dslists.Tables["DATALISTS"]), _table, 0);
                }
                else
                {
                    _import.Import(AddEnqDLVersionColumn(_dslists.Tables["DATALISTS"]), _table);
                }
                Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbEnquiryDataList");

                if (_dslists.Tables.Contains("LOOKUPS"))
                {
                    CodeLookups _lookups = new CodeLookups(_dslists.Tables["LOOKUPS"], "ENQDATALIST");
                    DataView msg1 = new DataView(_lookups.DataTable);
                    string fromvalue = Convert.ToString(_dslists.Tables["DATALISTS"].Rows[0]["enqTable"]);
                    msg1.RowFilter = "cdCode = '" + fromvalue + "'";
                    foreach (DataRowView drv in msg1)
                    {
                        if (_lookups.CheckCodeExists(Convert.ToString(fromvalue)) == false)
                            _lookups.Create("ENQDATALIST", Convert.ToString(fromvalue), Convert.ToString(drv["cdDesc"]), Convert.ToString(drv["cdHelp"]), Convert.ToString(drv["cdUICultureInfo"]));
                    }
                }
            }
            return true;
        }

        private DataTable AddEnqDLVersionColumn(DataTable TableFromPackage)
        {
            if (!TableFromPackage.Columns.Contains("enqDLVersion"))
            {
                TableFromPackage.Columns.Add("enqDLVersion", typeof(int));
                foreach (DataRow r in TableFromPackage.Rows)
                    r["enqDLVersion"] = 1;
            }
            return TableFromPackage;
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
