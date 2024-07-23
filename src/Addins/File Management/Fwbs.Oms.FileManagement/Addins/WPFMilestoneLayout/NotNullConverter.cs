using System;
using System.Windows.Data;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class NotNullConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isNull = false;

            if (value is string)
            {
                var str = value as string;

                isNull = string.IsNullOrWhiteSpace(str);
            }
            else if (value is bool)
            {
                isNull = (bool)value;
            }
            else if (value is bool?)
            {
                var val = value as bool?;
                if (val.HasValue)
                    isNull = val.Value;
                else
                    isNull = true;
            }
            else
                isNull = value == null;

            if (targetType == typeof(System.Windows.Visibility))
                if (isNull)
                    return System.Windows.Visibility.Collapsed;
                else
                    return System.Windows.Visibility.Visible;

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
