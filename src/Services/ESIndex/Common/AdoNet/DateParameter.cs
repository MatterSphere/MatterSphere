using System;
using System.Data;
using System.Data.SqlClient;
namespace Common.AdoNet
{
    public class DateParameter : Parameter
    {
        public DateParameter(string name, DateTime? value) : base(name, SqlDbType.DateTime)
        {
            Value = value;
        }

        public DateTime? Value { get; set; }
        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            paramaters.Add(new SqlParameter(Name, Type));

            if (Value.HasValue)
            {
                paramaters[Name].Value = Value.Value;
            }
            else
            {
                paramaters[Name].Value = DBNull.Value;
            }
        }
    }
}
