using System;
using System.Windows.Data;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class DateAsLocalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
			if (value == null)
				return value;

            var dte = (DateTime)value;
            if (dte != null)
                return dte.ToLocalTime();

            var ndte = value as DateTime?;
            if (ndte != null && ndte.HasValue)
                return ndte.Value.ToLocalTime();

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var date = value as DateTime?;
            if (date == null || !date.HasValue)
                return value;

            var dte = date.Value;
            return new DateTime(dte.Year, dte.Month, dte.Day, dte.Hour, dte.Minute, dte.Second, DateTimeKind.Local);
        }
    }
}
