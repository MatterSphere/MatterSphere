using System;

namespace Horizon.Common.Models.Repositories.IndexStructure
{
    public class TableField
    {
        public TableField(string field, string type)
        {
            Field = field;
            Type = type;
        }

        public string Field { get; private set; }
        public string Type { get; private set; }

        public string Label
        {
            get { return $"{Field} ({Type})"; }
        }

        public bool IsCodeLookup
        {
            get { return Type.Equals("uCodeLookup", StringComparison.OrdinalIgnoreCase); }
        }

        public string ToElasticType()
        {
            switch (Type.ToLower())
            {
                case "bit":
                    return "boolean";
                case "tinyint":
                    return "byte";
                case "smallint":
                    return "short";
                case "int":
                case "ucountry":
                case "ucreatedby":
                    return "integer";
                case "bigint":
                    return "long";
                case "datetime":
                case "datetime2":
                case "smalldatetime":
                case "date":
                case "time":
                case "datetimeoffset":
                case "timestamp":
                case "ucreated":
                    return "date";
                case "real":
                case "smallmoney":
                    return "float";
                case "float":
                case "decimal":
                case "money":
                    return "double";
                case "uniqueidentifier":
                case "uuicultureinfo":
                case "upostcode":
                case "utelephone":
                case "uemail":
                    return "keyword";
                default:
                    return "text";
            }
        }
    }
}
