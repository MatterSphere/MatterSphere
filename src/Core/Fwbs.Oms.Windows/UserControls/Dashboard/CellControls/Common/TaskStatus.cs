using System;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public class TaskStatus
    {
        public static string GetLabel(TaskStatusEnum status)
        {
            switch (status)
            {
                case TaskStatusEnum.PastDue:
                    return CodeLookup.GetLookup("DASHBOARD", "PASTDUE", "Past Due");
                case TaskStatusEnum.DueSoon:
                    return CodeLookup.GetLookup("DASHBOARD", "DUESOON", "Due Soon");
                case TaskStatusEnum.OnTime:
                    return CodeLookup.GetLookup("DASHBOARD", "ONTIME", "On Time");
                case TaskStatusEnum.Completed:
                    return CodeLookup.GetLookup("DASHBOARD", "CMPLT", "Complete");
                default:
                    throw new ArgumentException($"The value {status} is not expected");
            }
        }
    }
}
