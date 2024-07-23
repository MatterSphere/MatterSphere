using System;
using System.Text;

namespace Fwbs.Documents
{
    public sealed class DSOPropertyConverter : IPropertyConverter<DSOFile.dsoFilePropertyType, object>
    {
        private ConversionMethods methods = new ConversionMethods();

        public DSOPropertyConverter()
        {
        }

        #region IPropertyConverter<string> Members

        public object FromSource(object value, DSOFile.dsoFilePropertyType type)
        {
            if (value == null)
                return null;

            IFormatProvider provider = System.Globalization.CultureInfo.InvariantCulture;
            Type t = FromSourceType(type);
            return methods[t](value, provider);
        }

        public object ToSource(object value)
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

                return date.ToUniversalTime();
            }

            return value;
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


        public Type FromSourceType(DSOFile.dsoFilePropertyType type)
        {
            switch (type)
            {
                case DSOFile.dsoFilePropertyType.dsoPropertyTypeBool:
                    return typeof(bool);
                case DSOFile.dsoFilePropertyType.dsoPropertyTypeDate:
                    return typeof(DateTime);
                case DSOFile.dsoFilePropertyType.dsoPropertyTypeDouble:
                    return typeof(Double);
                case DSOFile.dsoFilePropertyType.dsoPropertyTypeLong:
                    return typeof(int);
                case DSOFile.dsoFilePropertyType.dsoPropertyTypeString:
                    return typeof(string);
                default:
                    return typeof(string);
            }

        }

        public DSOFile.dsoFilePropertyType ToSourceType(Type type)
        {
            if (type == typeof(bool))
                return DSOFile.dsoFilePropertyType.dsoPropertyTypeBool;
            else if (type == typeof(DateTime))
                return DSOFile.dsoFilePropertyType.dsoPropertyTypeDate;
            else if (type == typeof(Double))
                return DSOFile.dsoFilePropertyType.dsoPropertyTypeDouble;
            else if (type == typeof(Int32))
                return DSOFile.dsoFilePropertyType.dsoPropertyTypeLong;
            else if (type == typeof(string))
                return DSOFile.dsoFilePropertyType.dsoPropertyTypeString;
            else
                return DSOFile.dsoFilePropertyType.dsoPropertyTypeString;
        }

        #endregion
    }
}
