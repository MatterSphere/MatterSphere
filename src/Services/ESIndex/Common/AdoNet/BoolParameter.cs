using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class BoolParameter : Parameter
    {
        public BoolParameter(string name, bool value) : base(name, SqlDbType.Bit)
        {
            Value = value;
        }

        public bool Value { get; set; }
        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            paramaters.Add(new SqlParameter(Name, Type));
            paramaters[Name].Value = Value;
        }
    }
}
