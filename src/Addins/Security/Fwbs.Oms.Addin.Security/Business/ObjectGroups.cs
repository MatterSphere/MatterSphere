using System.Data;

namespace FWBS.OMS.Addin.Security
{
    class ObjectGroups
    {
        public static bool PolicyTypeExists(string Type)
        {
            ReportingServer OMS = new ReportingServer("FWBS Limited 2005");
            IDataParameter[] param = new IDataParameter[1];
            param[0] = OMS.Connection.AddParameter("Code", Type);
            System.Data.DataTable dt = OMS.Connection.ExecuteSQLTable("SELECT PolicyTypeCode FROM config.PolicyType WHERE PolicyTypeCode=@Code", "TYPE", param);
            return (dt.Rows.Count > 0);
        }
    }
}
