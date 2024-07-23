using System.Collections.Generic;
using System.Data.SqlClient;

namespace FWBS.OMS.OMSEXPORT
{
    public interface IDatabaseProvider
    {
        int ExecuteSQL(string SQL, IEnumerable<SqlParameter> parameters);
        SqlCommand CreateSqlCommand(string SQL, IEnumerable<SqlParameter> parameters);
    }
}
