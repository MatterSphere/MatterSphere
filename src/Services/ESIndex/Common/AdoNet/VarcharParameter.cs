using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class VarcharParameter : Parameter
    {
        public VarcharParameter(string name, string value, int size) : base(name, SqlDbType.VarChar)
        {
            Value = value;
            Size = size;
        }

        public string Value { get; set; }
        public int Size { get; set; }
        public override void AddSqlParameter(SqlParameterCollection paramaters)
        {
            paramaters.Add(new SqlParameter(Name, Type, Size));
            paramaters[Name].Value = Value;
        }
    }
}
