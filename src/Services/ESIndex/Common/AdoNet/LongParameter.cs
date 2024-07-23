using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class LongParameter : Parameter
    {
        public LongParameter(string name, long? value) : base(name, SqlDbType.BigInt)
        {
            Value = value;
        }

        public long? Value { get; set; }

        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            paramaters.Add(new SqlParameter(Name, Type));
            paramaters[Name].Value = Value;
        }
    }
}
