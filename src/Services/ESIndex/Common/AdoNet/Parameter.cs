using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public abstract class Parameter
    {
        public Parameter(string name, SqlDbType type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; set; }
        public SqlDbType Type { get; set; }

        public abstract void AddSqlParameter(SqlParameterCollection paramaters);
    }
}
