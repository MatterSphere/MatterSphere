using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class FloatParameter : Parameter
    {
        public FloatParameter(string name, double? value) : base(name, SqlDbType.Float)
        {
            Value = value;
        }

        public double? Value { get; set; }

        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            paramaters.Add(new SqlParameter(Name, Type));
            paramaters[Name].Value = Value;
        }
    }
}
