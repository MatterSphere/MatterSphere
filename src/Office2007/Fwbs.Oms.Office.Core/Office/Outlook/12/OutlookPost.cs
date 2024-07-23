using System;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookPost
    {
        public bool IsMarkedAsTask
        {
            get { return InternalItem.IsMarkedAsTask; }
        }

        public void MarkAsTask(MSOutlook.OlMarkInterval MarkInterval)
        {
            InternalItem.MarkAsTask(MarkInterval);
        }

        public DateTime ReminderTime
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.ReminderTime);
            }
            set
            {
                InternalItem.ReminderTime = Helpers.LocalToLocal(value);
            }
        }

        public void ClearTaskFlag()
        {
            InternalItem.ClearTaskFlag();
        }

        public string TaskSubject
        {
            get
            {
                return InternalItem.TaskSubject;
            }
            set
            {
                InternalItem.TaskSubject = value;
            }
        }

        public DateTime ToDoTaskOrdinal
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.ToDoTaskOrdinal);
            }
            set
            {
                InternalItem.ToDoTaskOrdinal = Helpers.LocalToLocal(value);
            }
        }

        public DateTime TaskCompletedDate
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.TaskCompletedDate);
            }
            set
            {
                InternalItem.TaskCompletedDate = Helpers.LocalToLocal(value);
            }
        }

        public DateTime TaskDueDate
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.TaskDueDate);
            }
            set
            {
                InternalItem.TaskDueDate = Helpers.LocalToLocal(value);
            }
        }

        public DateTime TaskStartDate
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.TaskStartDate);
            }
            set
            {
                InternalItem.TaskStartDate = Helpers.LocalToLocal(value);
            }
        }

     
    }
}
