using System;
using System.Globalization;
using System.Windows.Data;

namespace Horizon.Common
{
    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = value as bool?;

            if (val.GetValueOrDefault())
                return System.Windows.Visibility.Visible;

            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
