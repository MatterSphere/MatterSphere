using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace FWBS.OMS.FileManagement.Addins.WPFMilestoneLayout
{
    public class TaskActionsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length < 3)
                return null;

            var businessObject = values[0];

            if (values[1] == System.Windows.DependencyProperty.UnsetValue)
                return null;

            if (values[2] == System.Windows.DependencyProperty.UnsetValue)
                return null;

            var actionsOrderType = (ActionsOrderType)values[1];
            var actionsToDisplay = (ActionsToDisplay)values[2];

            if (businessObject == null)
                return businessObject;

            List<WPFViewAction> fileViewActions = null;
            List<WPFViewAction> taskViewActions = null;

            bool incFileActions = actionsToDisplay == ActionsToDisplay.FileAndTask || actionsToDisplay == ActionsToDisplay.FileOnly;
            bool incTaskActions = actionsToDisplay == ActionsToDisplay.FileAndTask || actionsToDisplay == ActionsToDisplay.TaskOnly;

            var stage = businessObject as Milestones.MilestoneStage;
            var task = businessObject as Milestones.Task;

            if (task != null)
            {
                fileViewActions = incFileActions ? GetFileViewActions(task.Stage) : null;
                taskViewActions = incTaskActions ? GetTaskViewActions(task) : null;
            }
            else if (stage != null)
            {
                fileViewActions = incFileActions ? GetFileViewActions(stage) : null;
            }

            List<WPFViewAction> viewActions = new List<WPFViewAction>();
            IEnumerable<WPFViewAction> firstActions;
            IEnumerable<WPFViewAction> secondActions;

            if (actionsOrderType == ActionsOrderType.FileActions1st)
            {
                firstActions = fileViewActions;
                secondActions = taskViewActions;
            }
            else
            {
                firstActions = taskViewActions;
                secondActions = fileViewActions;
            }

            if (firstActions != null)
                viewActions.AddRange(firstActions);

            if (firstActions != null && firstActions.Count() > 0 && secondActions != null && secondActions.Count() > 0)
                viewActions.Add(new WPFSplitterViewAction());

            if (secondActions != null)
                viewActions.AddRange(secondActions);

            return viewActions;
        }

        private List<WPFViewAction> GetTaskViewActions(Milestones.Task task)
        {
            var actions = task.Application.GetAvailableActions(task);

            if (actions.Length == 0)
            {
                ResourceItem res = Session.CurrentSession.Resources.GetMessage("NOTASKACTIONS", "No Task Actions", "");
                return GetEmptyViewAction(res);
            }

            List<WPFViewAction> viewActions = new List<WPFViewAction>();
            foreach (Action action in actions)
            {
                viewActions.Add(new WPFViewAction(action));
            }

            return viewActions;
        }
        
        private List<WPFViewAction> GetFileViewActions(Milestones.MilestoneStage stage)
        {
            var actions = stage.Application.GetAvailableActions(stage);

            if (actions.Length == 0)
            {
                ResourceItem res = Session.CurrentSession.Resources.GetMessage("NOFILEACTIONS", "No %FILE% Actions", "");
                return GetEmptyViewAction(res);
            }

            List<WPFViewAction> viewActions = new List<WPFViewAction>();
            foreach (Action action in actions)
            {
                viewActions.Add(new WPFViewAction(action));
            }

            return viewActions;
        }

        private static List<WPFViewAction> GetEmptyViewAction(ResourceItem res)
        {
            string description = res.Text;

            if (string.IsNullOrWhiteSpace(description))
                return null;

            return new List<WPFViewAction>(new WPFDisabledViewAction[] { new WPFDisabledViewAction(description) });
        }
       
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
