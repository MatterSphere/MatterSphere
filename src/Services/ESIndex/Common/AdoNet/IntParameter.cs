using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class IntParameter : Parameter
    {
        public IntParameter(string name, int value) : base(name, SqlDbType.Int)
        {
            Value = value;
        }

        public int Value { get; set; }

        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            paramaters.Add(new SqlParameter(Name, Type));
            paramaters[Name].Value = Value;
        }
    }
}
