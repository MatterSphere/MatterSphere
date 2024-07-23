using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class Scripts : ImportBase, IDisposable
    {
        #region Fields
        private DataSet _dsscripts = new DataSet("SCRIPTS");
        private System.IO.FileInfo _filename;
        private bool _confirmversion = false;
        #endregion

        #region Contructors
        public Scripts(string FileName, bool ConfirmVersion)
        {
            _dsscripts.ReadXml(FileName);
            _source = _dsscripts;
            _filename = new System.IO.FileInfo(FileName);
            _confirmversion = ConfirmVersion;
        }

        public Scripts(string FileName)
        {
            _dsscripts.ReadXml(FileName);
            _source = _dsscripts;
            _filename = new System.IO.FileInfo(FileName);
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            ImportTable _import = new ImportTable(this.Fieldreplacer);
            IDataParameter[] paramlist = new IDataParameter[1];
            string code = Convert.ToString(_dsscripts.Tables["SCRIPTS"].Rows[0]["scrCode"]);

            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);


            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbScript where scrCode = @Code", "SCRIPTS", false, paramlist);

            bool create = true;
            long sourceversion = 0;
            long destinversion = Convert.ToInt64(_dsscripts.Tables["SCRIPTS"].Rows[0]["scrVersion"]);
            if (_table.Rows.Count > 0) sourceversion = Convert.ToInt64(_table.Rows[0]["scrVersion"]);

            long destinuser = (long)FWBS.OMS.SystemUsers.Admin;
            long sourceuser = destinuser;
            if (_table.Rows.Count > 0) sourceuser = FWBS.Common.ConvertDef.ToInt64(_table.Rows[0]["UpdatedBy"], Session.CurrentSession.Administrator.ID);

            if (sourceuser != destinuser || (sourceuser == destinuser && destinversion < sourceversion))
            {
                string sourceusername = "{Unknown}";
                try { sourceusername = new User(Convert.ToInt32(sourceuser)).FullName; }
                catch { }
                AskEventArgs askargs = null;
                if (sourceuser != destinuser)
                    askargs = new AskEventArgs("ALLRDYEXISTV2", "The %1% [%2%] already exists. Do you want to overwrite %1% version %3% with version %4% ?" + Environment.NewLine + Environment.NewLine + "Last Modified By : %5%", "", FWBS.OMS.AskResult.No, "Script", code, sourceversion.ToString(), destinversion.ToString(), sourceusername);
                else
                    askargs = new AskEventArgs("ALLRDYEXISTV1", "The %1% [%2%] already exists. Do you want to overwrite %1% version %3% with version %4% ?", "", FWBS.OMS.AskResult.No, "Script", code, sourceversion.ToString(), destinversion.ToString());
                Session.CurrentSession.OnAsk(this, askargs);
                if (askargs.Result == FWBS.OMS.AskResult.Yes)
                    create = true;
                else
                    create = false;
            }

            if (create)
            {
                OnProgress("Importing Script : " + code);

                if (_table.Rows.Count > 0)
                    _import.ImportRowOver(_dsscripts.Tables["SCRIPTS"], _table, 0);
                else
                    _import.Import(_dsscripts.Tables["SCRIPTS"], _table);
                Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbScript");
            }
            return true;
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
