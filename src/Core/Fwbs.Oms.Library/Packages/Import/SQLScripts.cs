using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class SQLScripts : ImportBase, IDisposable
    {
        #region Fields
        private DataSet _dsscripts = new DataSet("SQLSCRIPTS");
        private System.IO.FileInfo _filename;
        #endregion

        #region Contructors
        public SQLScripts(string FileName)
        {
            _dsscripts.ReadXml(FileName);
            _source = _dsscripts;
            _filename = new System.IO.FileInfo(FileName);
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            return Execute(false);
        }

        public override bool Execute(bool Copy)
        {
            ImportTable _import = new ImportTable(this.Fieldreplacer);
            string code = Convert.ToString(_dsscripts.Tables["SQLSCRIPTS"].Rows[0]["sqlName"]);
            string proccode = Convert.ToString(_dsscripts.Tables["SQLSCRIPTS"].Rows[0]["sqlScript"]);

            OnProgress("Applying SQL Script : " + code);
            OMS.Session.CurrentSession.Connection.ExecuteSQL(proccode, new IDataParameter[0]);
            if (Copy)
            {
                IDataParameter[] param = new IDataParameter[1];
                param[0] = Session.CurrentSession.Connection.CreateParameter("@Code",SqlDbType.NText,Int32.MaxValue, proccode);
                DataTable dt = OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbPackageSqlScripts WHERE sqlScript Like @Code", "dbPackageSqlScripts", false, param);
                if (dt.Rows.Count == 0)
                {
                    dt = OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT " + GetColumnsString(dt,true) + " FROM dbPackageSqlScripts", "dbPackageSqlScripts", true, new IDataParameter[0]);
                    DataRow dr = dt.NewRow();
                    dr["sqlName"] = code;
                    dr["sqlScript"] = proccode;
                    dr["sqlDescription"] = _dsscripts.Tables["SQLSCRIPTS"].Rows[0]["sqlDescription"];
                    dt.Rows.Add(dr);
                    Session.CurrentSession.Connection.Update(dr, "dbPackageSqlScripts", true, GetColumns(dt,true));
                    CurrentRow["Code"] = Convert.ToString(dr["sqlID"]);
                }
                else
                    CurrentRow["Code"] = Convert.ToString(dt.Rows[0]["sqlID"]);
            }
            return true;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _dsscripts.Dispose();
        }
        #endregion
    }
}
