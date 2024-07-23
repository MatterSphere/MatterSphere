using System.Collections.Generic;
using System.Data;

namespace FWBS.OMS.Data
{
    public static class ConnectionExtensions
    {

        public static DataTable ExecuteSQL(this IConnection connection, string sql, IEnumerable<IDataParameter> parameters)
        {
            return ExecuteSQL(connection, sql, null, false, parameters);
        }

        public static DataTable ExecuteSQL(this IConnection connection, string sql, string tableName, bool schemaOnly, IEnumerable<IDataParameter> parameters)
        {
            var cp = new DataTableExecuteParameters();
            cp.CommandType = CommandType.Text;
            cp.Sql = sql;
            cp.Table = tableName;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = schemaOnly;

            return connection.Execute(cp);
        }

        public static DataTable ExecuteProcedure(this IConnection connection, string procedure, IEnumerable<IDataParameter> parameters)
        {
            return ExecuteProcedure(connection, procedure, null, false, parameters);
        }

        public static DataTable ExecuteProcedure(IConnection connection, string procedure, string tableName, bool schemaOnly, IEnumerable<IDataParameter> parameters)
        {
            var cp = new DataTableExecuteParameters();
            cp.CommandType = CommandType.StoredProcedure;
            cp.Sql = procedure;
            cp.Table = tableName;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = schemaOnly;

            return connection.Execute(cp);
        }
    }
}
