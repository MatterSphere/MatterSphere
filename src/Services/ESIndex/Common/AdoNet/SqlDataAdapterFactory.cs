using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class SqlDataAdapterFactory
    {
        public SqlDataAdapter GetSqlDataAdapter(SqlCommand command, List<Parameter> parameters = null)
        {
            var adapt = new SqlDataAdapter(command);
            adapt.SelectCommand.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    parameter.AddSqlParameter(adapt.SelectCommand.Parameters);
                }
            }

            return adapt;
        }
    }
}
