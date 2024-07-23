using System;
using System.ComponentModel;

namespace FWBS.OMS.UI.Windows.Admin
{
    internal class ImageTypeConvertor : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            else
                return false;
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string val = Convert.ToString(value);
            if (string.IsNullOrEmpty(val))
                return null;
            else
                return value;
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            else
                return false;

        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
                return "(None)";
            else
                return "(Image)";
        }
    }
}
