using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class Milestones : ImportBase, IDisposable
    {
        #region Fields
        DataSet _dslists = new DataSet("MILESTONE");
        System.IO.FileInfo _filename;
        #endregion

        #region Contructors
        public Milestones(string FileName)
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
            string code = Convert.ToString(_dslists.Tables["MILESTONE"].Rows[0]["MSCode"]);
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);

            OnProgress("Importing Milestone : " + code);

            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbMSConfig_OMS2K where MSCode = @Code", "MILESTONES", false, paramlist);

            bool create = true;

            if (_table.Rows.Count > 0)
            {
                AskEventArgs askargs = new AskEventArgs("ALLRDYEXISTV4", "The %1% [%2%] already exists. Do you want to overwrite ?", "", FWBS.OMS.AskResult.No, "Milestone Plan", code);
                Session.CurrentSession.OnAsk(this, askargs);
                if (askargs.Result == FWBS.OMS.AskResult.Yes)
                    create = true;
                else
                    create = false;
            }


            if (create)
            {
                if (_table.Rows.Count > 0)
                    _import.ImportRowOver(_dslists.Tables["MILESTONE"], _table, 0);
                else
                    _import.Import(_dslists.Tables["MILESTONE"], _table);
                Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbMSConfig_OMS2K");
                return true;
            }
            else
                return false;
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
