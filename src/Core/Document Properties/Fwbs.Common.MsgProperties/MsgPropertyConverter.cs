using System;
using System.Text;

namespace Fwbs.Documents
{
    public sealed class MsgPropertyConverter : IPropertyConverter<Redemption.rdoUserPropertyType, object>
    {
        private ConversionMethods methods = new ConversionMethods();

        public MsgPropertyConverter()
        {
        }

        #region IPropertyConverter<string> Members

        public object FromSource(object value, Redemption.rdoUserPropertyType type)
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


        public Redemption.rdoUserPropertyType ToSourceType(Type type)
        {
            if (type == typeof(bool))
                return Redemption.rdoUserPropertyType.olYesNo;
            else if (type == typeof(DateTime))
                return Redemption.rdoUserPropertyType.olDateTime;
            else if (type == typeof(Double))
                return Redemption.rdoUserPropertyType.olNumber;
            else if (type == typeof(Int32))
                return Redemption.rdoUserPropertyType.olInteger;
            else if (type == typeof(string))
                return Redemption.rdoUserPropertyType.olText;
            else
                return Redemption.rdoUserPropertyType.olText;
        }

        public Type FromSourceType(Redemption.rdoUserPropertyType type)
        {
            switch (type)
            {

                case Redemption.rdoUserPropertyType.olYesNo:
                    return typeof(bool);
                case Redemption.rdoUserPropertyType.olInteger:
                    return typeof(int);
                case Redemption.rdoUserPropertyType.olPercent:
                case Redemption.rdoUserPropertyType.olCurrency:
                case Redemption.rdoUserPropertyType.olNumber:
                    return typeof(double);
                case Redemption.rdoUserPropertyType.olDateTime:
                case Redemption.rdoUserPropertyType.olDuration:
                    return typeof(DateTime);
                case Redemption.rdoUserPropertyType.olCombination:
                case Redemption.rdoUserPropertyType.olFormula:
                case Redemption.rdoUserPropertyType.olKeywords:
                case Redemption.rdoUserPropertyType.olOutlookInternal:
                case Redemption.rdoUserPropertyType.olText:
                default:
                    return typeof(string);
            }
            
        }

        #endregion
    }
}
