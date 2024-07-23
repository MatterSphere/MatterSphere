using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class NVarcharParameter : Parameter
    {
        public NVarcharParameter(string name, string value, int size) : base(name, SqlDbType.NVarChar)
        {
            Value = value;
            Size = size;
        }

        public NVarcharParameter(string name, string value) : base(name, SqlDbType.NVarChar)
        {
            Value = value;
        }

        public string Value { get; set; }
        public int? Size { get; set; }
        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            if (Size.HasValue)
            {
                paramaters.Add(new SqlParameter(Name, Type, Size.Value));
            }
            else
            {
                paramaters.Add(new SqlParameter(Name, Type));
            }

            paramaters[Name].Value = Value;
        }
    }
}
