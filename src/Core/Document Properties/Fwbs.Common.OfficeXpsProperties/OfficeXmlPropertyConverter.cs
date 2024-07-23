using System;
using System.Text;

namespace Fwbs.Documents
{
    public sealed class OfficeXmlPropertyConverter : IPropertyConverter<string, string>
    {
        private ConversionMethods methods = new ConversionMethods();

        #region IPropertyConverter<string> Members

        public object FromSource(string value, string type)
        {
            if (value == null)
                return null;

            IFormatProvider provider = System.Globalization.CultureInfo.InvariantCulture;

            Type t = FromSourceType(type);
            
            return methods[t](value, provider);
        }

        public string ToSource(object value)
        {
            if (value == null)
                return null;

            IFormatProvider provider = System.Globalization.CultureInfo.InvariantCulture;

            Type type = ConvertType(value.GetType());
            value = methods[type](value, provider);

            if (value is DateTime)
            {
                DateTime date = (DateTime)value;
                if (date.Kind == DateTimeKind.Unspecified)
                    date = DateTime.SpecifyKind(date, DateTimeKind.Local);

                return string.Format(provider, "{0:o}", date.ToUniversalTime());
            }
            else if (value is Double)
            {
                return string.Format(provider, "{0:R}", value);
            }
            else if (value is Boolean)
            {
                return string.Format(provider, "{0}", value).ToLowerInvariant();
            }

            return (string)methods[typeof(string)](value, provider);
        }


        public Type ConvertType(Type type)
        {
            if (type == typeof(int) ||
                type == typeof(byte) ||
                type == typeof(short) ||
                type == typeof(sbyte) ||
                type == typeof(ushort))
                return typeof(int);
            
            if (type == typeof(bool))
                return typeof(bool);
            
            if (type == typeof(string) ||
                type == typeof(char) ||
                type == typeof(StringBuilder) ||
                type == typeof(Guid))
                return typeof(string);
            
            if (type == typeof(float) ||
                type == typeof(double) ||
                type == typeof(long) ||
                type == typeof(uint) ||
                type == typeof(ulong) ||
                type == typeof(decimal))
                return typeof(double);

            if (type == typeof(DateTime))
                return typeof(DateTime);

            return typeof(string);
        }

        public Type FromSourceType(string type)
        {
            switch (type.ToLowerInvariant())
            {
                case "vt:filetime":
                    return typeof(DateTime);
                case "vt:i4":
                    return typeof(int);
                case "vt:r8":
                    return typeof(double);
                case "vt:bool":
                    return typeof(bool);
                case "vt:lpwstr":
                    return typeof(string);
                default:
                    return typeof(string);
            }
        }

        public string ToSourceType(Type type)
        {
            if (type == typeof(DateTime))
                return "vt:filetime";
            else if (type == typeof(int))
                return "vt:i4";
            else if (type == typeof(double))
                return "vt:r8";
            else if (type == typeof(bool))
                return "vt:bool";
            else
                return "vt:lpwstr";
        }

        #endregion
    }
}
