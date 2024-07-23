using System;
using System.Data.SqlClient;

namespace FWBS.OMS.OMSEXPORT
{
    public static class InvoiceSyncTimeStamp
    {
        private const int DefaultBackwardDaysPeriod = 31;
        private const string InvoiceSyncStateCode = "3E_INVOICE_SYNC";

        private const string LoadTimeStampSQL = @"SELECT stateData FROM dbState WHERE stateCode = @Code AND brID IS NULL AND usrID IS NULL";

        private const string SaveTimeStampSQL = @"UPDATE dbState SET stateData = @Data WHERE stateCode = @Code AND brID IS NULL AND usrID IS NULL
IF @@ROWCOUNT = 0 INSERT INTO dbState (stateCode, stateData) VALUES (@Code, @Data)";

        public static DateTime Load(SqlConnection connection)
        {
            DateTime timeStamp;
            SqlCommand cmd = new SqlCommand(LoadTimeStampSQL, connection);
            cmd.Parameters.AddWithValue("@Code", InvoiceSyncStateCode);
            object retVal = cmd.ExecuteScalar();
            if (retVal == null || Convert.IsDBNull(retVal))
                timeStamp = DateTime.UtcNow.AddDays(-DefaultBackwardDaysPeriod);
            else
                timeStamp = DateTime.SpecifyKind(Convert.ToDateTime(retVal), DateTimeKind.Utc);
            return timeStamp;
        }

        public static void Save(SqlConnection connection, DateTime timeStamp)
        {
            SqlCommand cmd = new SqlCommand(SaveTimeStampSQL, connection);
            cmd.Parameters.AddWithValue("@Code", InvoiceSyncStateCode);
            cmd.Parameters.AddWithValue("@Data", timeStamp);
            cmd.ExecuteNonQuery();
        }
    }
}
