using System;
using System.Collections.Generic;

namespace Fwbs.Documents
{
    public delegate object ConvertValueDelegate(object value, IFormatProvider provider);

    [Serializable]
    public class ConversionMethods : Dictionary<Type, ConvertValueDelegate>
    {
        private ConvertValueDelegate convstring = new ConvertValueDelegate(ConvertToString);
        private ConvertValueDelegate convint = new ConvertValueDelegate(ConvertToInt);
        private ConvertValueDelegate convdouble = new ConvertValueDelegate(ConvertToDouble);
        private ConvertValueDelegate convdate = new ConvertValueDelegate(ConvertToDateTime);
        private ConvertValueDelegate convbool = new ConvertValueDelegate(ConvertToBoolean);

        public ConversionMethods()
        {
            Add(typeof(String), convstring);
            Add(typeof(char), convstring);

            Add(typeof(long), convint);
            Add(typeof(int), convint);
            Add(typeof(short), convint);
            Add(typeof(byte), convint);
            
            Add(typeof(uint), convint);
            Add(typeof(ulong), convint);
            Add(typeof(ushort), convint);
            Add(typeof(sbyte), convint);

            Add(typeof(double), convdouble);
            Add(typeof(float), convdouble);
            Add(typeof(decimal), convdouble);

            Add(typeof(DateTime), convdate);

            Add(typeof(bool), convbool);
        }
        

        public static object ConvertToBoolean(object value, IFormatProvider provider)
        {
            IConvertible conv = value as IConvertible;

            if (conv == null)
            {
                bool val;
                if (bool.TryParse(Convert.ToString(value, provider), out val))
                    return val;
                else
                    return Convert.ToString(val, provider);
            }
            else
            {
                return conv.ToBoolean(System.Globalization.CultureInfo.InvariantCulture);
            }
        }

        public static object ConvertToDateTime(object value, IFormatProvider provider)
        {
            IConvertible conv = value as IConvertible;
            DateTime val;

            if (conv == null)
            {
                if (!DateTime.TryParse(Convert.ToString(value, provider), out val))
                    return Convert.ToString(val, provider);
            }
            else
            {
                val = conv.ToDateTime(provider);
            }

            if (val.Kind == DateTimeKind.Unspecified)
                return DateTime.SpecifyKind(val, DateTimeKind.Utc).ToLocalTime();
            else
                return val.ToLocalTime();
        }

        public static object ConvertToString(object value, IFormatProvider provider)
        {
            return Convert.ToString(value, provider);
        }

        public static object ConvertToInt(object value, IFormatProvider provider)
        {
            IConvertible conv = value as IConvertible;

            if (conv == null)
            {
                int val;
                if (int.TryParse(Convert.ToString(value, provider), out val))
                    return val;
                else
                    return ConvertToDouble(value, provider);
            }
            else
            {
                try
                {
                    return conv.ToInt32(provider);
                }
                catch (OverflowException)
                {
                    return ConvertToDouble(value, provider);
                }
            }
        }

        public static object ConvertToDouble(object value, IFormatProvider provider)
        {
            IConvertible conv = value as IConvertible;

            if (conv == null)
            {
                double val;
                if (double.TryParse(Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture), out val))
                    return val;
                else
                    return ConvertToDecimal(value, provider);
            }
            else
            {
                try
                {
                    return conv.ToDouble(provider);
                }
                catch (OverflowException)
                {
                    return ConvertToDecimal(value, provider);
                }
            }
        }

        public static object ConvertToDecimal(object value, IFormatProvider provider)
        {
            IConvertible conv = value as IConvertible;

            if (conv == null)
            {
                decimal val;
                if (decimal.TryParse(Convert.ToString(value, provider), out val))
                    return val;
                else
                    return ConvertToString(value, provider);
            }
            else
            {
                try
                {
                    return conv.ToDouble(provider);
                }
                catch (OverflowException)
                {
                    return ConvertToString(value, provider);
                }
            }
        }
    }
}
