using System;
using System.Windows;
using System.Windows.Data;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class FilterConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {


            var searchText = values[0] as string;
            FilterStatus filterStatus = FilterStatus.All;
            if (values[3] != DependencyProperty.UnsetValue)
                filterStatus = (FilterStatus)values[3];
            if (string.IsNullOrWhiteSpace(searchText) && filterStatus == FilterStatus.All)
                return Visibility.Visible;



            var stage = values[1] as Milestones.MilestoneStage;
            var task = values[1] as Milestones.Task;
            bool display = false;
            if (task != null)
                display = ShowTask(task, searchText, filterStatus);
            else if (stage != null)
                display = ShowStage(stage, searchText, filterStatus);

            if (display)
                return Visibility.Visible;

            return Visibility.Collapsed;

        }

        private bool ShowStage(Milestones.MilestoneStage stage, string searchText, FilterStatus status)
        {
            
            if (!string.IsNullOrWhiteSpace(searchText) && stage.Description.IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >=0)
                return true;
            switch (status)
            {
                case FilterStatus.Completed:
                    if (stage.Status == Milestones.StageStatus.Completed)
                        return true;
                    break;
                case FilterStatus.Due:
                    switch (stage.Status)
                    {
                        case Milestones.StageStatus.Due:
                        case Milestones.StageStatus.NextDue:
                        case Milestones.StageStatus.Overdue:
                            return true;
                    }
                    break;
            }

            foreach (Milestones.Task task in stage.Tasks)
            {
                if (ShowTask(task, searchText, status))
                    return true;

            }

            return false;
        }

        private bool ShowTask(Milestones.Task task, string searchText, FilterStatus status)
        {
            switch (status)
            {
                case FilterStatus.Completed:
                    if (task.Status == Milestones.TaskStatus.Completed)
                        return true;
                    break;
                case FilterStatus.Due:
                    switch(task.Status)
                    {
                        case Milestones.TaskStatus.Due:
                        case Milestones.TaskStatus.Overdue:
                            return true;
                    }
                    break;
            }

            return !string.IsNullOrWhiteSpace(searchText) && task.Description.IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) >= 0;

            
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
