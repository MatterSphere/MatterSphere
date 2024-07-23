using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class TasksOrderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var tasks = value as Milestones.TaskCollection;

            return tasks.Where<Milestones.Task>(t => t.Visible).OrderBy<Milestones.Task, DateTime?>(t => t.Due, new TasksDateComparer());
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class TasksDateComparer : IComparer<DateTime?>
    {
        public int Compare(DateTime? x, DateTime? y)
        {
            if (x.HasValue && y.HasValue)
                return x.Value.CompareTo(y.Value);
            else if (x.HasValue)
                return -1;
            else if (y.HasValue)
                return 1;
            else
                return 0;
        }
    }
}
