using System;
using System.Windows.Data;
using System.Windows.Media;
using FWBS.OMS.FileManagement.Milestones;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class StageStatusConverter : IValueConverter, IMultiValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is StageStatus))
                return value;

            var status = (StageStatus)value;

            if (targetType == typeof(bool?))
            {
                switch(status)
                {
                    case StageStatus.Completed:
                        return true;
                    default:
                        return false;
                }
            }
           
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(values[0] is StageStatus))
                return values;

            var status = (StageStatus)values[0];

            var stage = values[1] as Milestones.MilestoneStage;
            if (stage == null)
                return values;

            if (targetType == typeof(Brush))
            {
                System.Drawing.Color oldColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Control);
                switch (status)
                {
                    case StageStatus.Completed:
                        {
                            oldColor = stage.Application.Parent.TaskCompletedColour;
                            break;
                        }
                    case StageStatus.Overdue:
                        {
                            oldColor = stage.Application.Parent.TaskOverdueColour;
                            break;
                        }
                    default:
                        {
                            oldColor = stage.Application.Parent.TaskDueColour;
                            break;
                        }

                }

                if (stage.IsNextDue)
                    oldColor = FWBS.OMS.UI.Windows.ucPanelNav.ChangeBrightness(oldColor, -50);

                Color col = Color.FromArgb(oldColor.A, oldColor.R, oldColor.G, oldColor.B);
              
                return new SolidColorBrush(col);
            }

            return values;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
