using System;
using System.Windows.Data;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    class TasksCountConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var coll = value as Milestones.TaskCollection;

            if (coll == null || coll.Count == 0)
                return System.Windows.Visibility.Hidden;

            return System.Windows.Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
