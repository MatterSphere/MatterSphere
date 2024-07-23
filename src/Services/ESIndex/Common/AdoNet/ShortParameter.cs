using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class ShortParameter : Parameter
    {
        public ShortParameter(string name, short value) : base(name, SqlDbType.SmallInt)
        {
            Value = value;
        }

        public short Value { get; set; }

        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            paramaters.Add(new SqlParameter(Name, Type));
            paramaters[Name].Value = Value;
        }
    }
}
