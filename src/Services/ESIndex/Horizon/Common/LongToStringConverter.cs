using System;
using System.Windows.Data;

namespace Horizon.Common
{
    public class LongToStringConverter : IValueConverter
    {
        public long EmptyStringValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is string)
            {
                return value;
            }

            if (value is long && (long) value == EmptyStringValue)
            {
                return string.Empty;
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                long intValue;
                var result = Int64.TryParse(value.ToString(), out intValue);

                return result
                    ? intValue
                    : EmptyStringValue;
            }

            return value;
        }
    }
}
