using System.Data;
using System.Data.SqlClient;

namespace Common.AdoNet
{
    public class CharParameter : Parameter
    {
        public CharParameter(string name, string value, int size) : base(name, SqlDbType.Char)
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
