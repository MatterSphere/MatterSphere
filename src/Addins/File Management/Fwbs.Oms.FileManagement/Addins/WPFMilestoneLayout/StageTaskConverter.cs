using System;
using System.Windows.Data;
using System.Windows.Media;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class StageTaskConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var task = value as FWBS.OMS.FileManagement.Milestones.Task;

            if (task == null)
                return value;

            if (targetType == typeof(Brush))
            {
                var col = task.Application.Parent.TaskDueColour;

                if (task.IsCompleted)
                    col = task.Application.Parent.TaskCompletedColour;
                if (task.IsOverdue)
                    col = task.Application.Parent.TaskOverdueColour;
               
                var displayColor = Color.FromArgb(col.A, col.R, col.G, col.B);

                return new SolidColorBrush(displayColor);
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Convert(values[1], targetType, parameter, culture);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
