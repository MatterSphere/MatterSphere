using System;
using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class StringParameter : Parameter
    {
        public StringParameter(string name, string value, int? size = null) : base(name, SqlDbType.NVarChar)
        {
            Value = value;
            Size = size;
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

            paramaters[Name].Value = (object)Value ?? DBNull.Value;
        }
    }
}
