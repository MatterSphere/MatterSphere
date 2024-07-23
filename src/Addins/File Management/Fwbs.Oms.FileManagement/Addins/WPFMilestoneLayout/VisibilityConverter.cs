using System;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class VisibilityConverter : System.Windows.Data.IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var val = (bool?)value;

            if (val.HasValue && val.Value)
                return System.Windows.Visibility.Visible;

            return System.Windows.Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
