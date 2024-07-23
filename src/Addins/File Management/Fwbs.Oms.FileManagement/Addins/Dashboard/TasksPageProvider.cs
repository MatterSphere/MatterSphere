using System;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.FileManagement.Addins.Dashboard
{
    public class TasksPageProvider : IPageProvider
    {
        private const string MY_TASKS_OMS_CODE = "ADDFMTASKSUSR";
        private const string MY_TASKS_TITLE_CODE = "MYTSKS";
        private const string MY_TEAMS_TASKS_OMS_CODE = "ADDFMTASKSUSR";
        private const string MY_TEAMS_TASKS_TITLE_CODE = "TEAMTSKS";
        private const string ALL_TASKS_OMS_CODE = "ADDFMTASKSUSR";
        private const string ALL_TASKS_TITLE_CODE = "ALLTSKS";

        public TasksPageProvider()
        {
            Headers = new[]
            {
                TasksDashboardItem.TasksPageEnum.AllTasks.ToString(),
                TasksDashboardItem.TasksPageEnum.MyTeamsTasks.ToString(),
                TasksDashboardItem.TasksPageEnum.MyTasks.ToString()
            };
        }

        public string[] Headers { get; }

        public BaseContainerPage GetPage(string header)
        {
            if (header == TasksDashboardItem.TasksPageEnum.MyTasks.ToString())
            {
                return CreateMyTasksPage();
            }

            if (header == TasksDashboardItem.TasksPageEnum.MyTeamsTasks.ToString())
            {
                return CreateMyTeamsTasksPage();
            }

            if (header == TasksDashboardItem.TasksPageEnum.AllTasks.ToString())
            {
                return CreateAllTasksPage();
            }

            throw new ArgumentOutOfRangeException();
        }

        public PageDetails GetDetails(string header)
        {
            if (header == TasksDashboardItem.TasksPageEnum.MyTasks.ToString())
            {
                return new PageDetails(MY_TASKS_OMS_CODE, CodeLookup.GetLookup("DASHBOARD", MY_TASKS_TITLE_CODE, "My Tasks"));
            }

            if (header == TasksDashboardItem.TasksPageEnum.MyTeamsTasks.ToString())
            {
                return new PageDetails(MY_TEAMS_TASKS_OMS_CODE, CodeLookup.GetLookup("DASHBOARD", MY_TEAMS_TASKS_TITLE_CODE, "Team Tasks"));
            }

            if (header == TasksDashboardItem.TasksPageEnum.AllTasks.ToString())
            {
                return new PageDetails(ALL_TASKS_OMS_CODE, CodeLookup.GetLookup("DASHBOARD", ALL_TASKS_TITLE_CODE, "All Tasks"));
            }

            throw new ArgumentOutOfRangeException();
        }

        private BaseContainerPage CreateMyTasksPage()
        {
            return new TasksDashboardItem
            {
                Code = TasksDashboardItem.TasksPageEnum.MyTasks.ToString(),
                Title = CodeLookup.GetLookup("DASHBOARD", MY_TASKS_TITLE_CODE, "My Tasks"),
                TasksPage = TasksDashboardItem.TasksPageEnum.MyTasks,
                Dock = DockStyle.Fill,
                AllowColumnChange = true
            };
        }

        private BaseContainerPage CreateMyTeamsTasksPage()
        {
            return new TasksDashboardItem
            {
                Code = TasksDashboardItem.TasksPageEnum.MyTeamsTasks.ToString(),
                Title = CodeLookup.GetLookup("DASHBOARD", MY_TEAMS_TASKS_TITLE_CODE, "Team Tasks"),
                TasksPage = TasksDashboardItem.TasksPageEnum.MyTeamsTasks,
                Dock = DockStyle.Fill,
                AllowColumnChange = true
            };
        }

        private BaseContainerPage CreateAllTasksPage()
        {
            return new TasksDashboardItem
            {
                Code = TasksDashboardItem.TasksPageEnum.AllTasks.ToString(),
                Title = CodeLookup.GetLookup("DASHBOARD", ALL_TASKS_TITLE_CODE, "All Tasks"),
                TasksPage = TasksDashboardItem.TasksPageEnum.AllTasks,
                Dock = DockStyle.Fill,
                AllowColumnChange = true
            };
        }
    }
}
