using System;
using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class TimeParameter : Parameter
    {
        public TimeParameter(string name, TimeSpan value) : base(name, SqlDbType.Time)
        {
            Value = value;
        }

        public TimeSpan Value { get; set; }

        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            paramaters.Add(new SqlParameter(Name, Type));
            paramaters[Name].Value = Value;
        }
    }
}
