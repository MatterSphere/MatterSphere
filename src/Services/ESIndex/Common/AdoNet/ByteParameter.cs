using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class ByteParameter : Parameter
    {
        public ByteParameter(string name, byte value) : base(name, SqlDbType.TinyInt)
        {
            Value = value;
        }

        public byte Value { get; set; }

        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            paramaters.Add(new SqlParameter(Name, Type));
            paramaters[Name].Value = Value;
        }
    }
}
