using System;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookMail
    {
        public void MarkAsTask(MSOutlook.OlMarkInterval MarkInterval)
        {
            InternalItem.MarkAsTask(MarkInterval);
        }


        public void AddBusinessCard(MSOutlook.ContactItem contact)
        {
            InternalItem.AddBusinessCard(contact);
        }


        public void ClearTaskFlag()
        {
            InternalItem.ClearTaskFlag();
        }

        public bool IsMarkedAsTask
        {
            get { return InternalItem.IsMarkedAsTask; }
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

    }
}
