using System;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public partial class OutlookTask : OutlookItem, MSOutlook.TaskItem
    {
        private MSOutlook._TaskItem task;

        public OutlookTask(MSOutlook.TaskItem task)
            : base(task)
        {
        }

        new private MSOutlook._TaskItem InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return task;
            }
        }

        protected override void OnAttach(object obj)
        {
            base.OnAttach(obj);
            task = (MSOutlook._TaskItem)obj;
        }

        protected override void OnDetach()
        {
            task = null;
            base.OnDetach();
        }

        #region _TaskItem Members

        MSOutlook.Application MSOutlook._TaskItem.Application
        {
            get
            {
                return base.Application;
            }
        }

        public int ActualWork
        {
            get
            {
                return InternalItem.ActualWork;
            }
            set
            {
                InternalItem.ActualWork = value;
            }
        }

        public MSOutlook.TaskItem Assign()
        {
            return InternalItem.Assign();
        }

        public void CancelResponseState()
        {
            InternalItem.CancelResponseState();
        }

        public string CardData
        {
            get
            {
                return InternalItem.CardData;
            }
            set
            {
                InternalItem.CardData = value;
            }
        }

        public void ClearRecurrencePattern()
        {
            InternalItem.ClearRecurrencePattern();
        }

        public bool Complete
        {
            get
            {
                return InternalItem.Complete;
            }
            set
            {
                InternalItem.Complete = value;
            }
        }

        public string ContactNames
        {
            get
            {
                return InternalItem.ContactNames;
            }
            set
            {
                InternalItem.ContactNames = value;
            }
        }

        public string Contacts
        {
            get
            {
                return InternalItem.Contacts;
            }
            set
            {
                InternalItem.Contacts = value;
            }
        }

        public DateTime DateCompleted
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.DateCompleted);
            }
            set
            {
                InternalItem.DateCompleted = Helpers.LocalToLocal(value);
            }
        }

        public MSOutlook.OlTaskDelegationState DelegationState
        {
            get { return InternalItem.DelegationState; }
        }

        public string Delegator
        {
            get { return InternalItem.Delegator; }
        }

        public DateTime DueDate
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.DueDate);
            }
            set
            {
                InternalItem.DueDate = Helpers.LocalToLocal(value);
            }
        }

        public MSOutlook.RecurrencePattern GetRecurrencePattern()
        {
            return InternalItem.GetRecurrencePattern();
        }

        public bool IsRecurring
        {
            get { return InternalItem.IsRecurring; }
        }

        public void MarkComplete()
        {
            InternalItem.MarkComplete();
        }



        public int Ordinal
        {
            get
            {
                return InternalItem.Ordinal;
            }
            set
            {
                InternalItem.Ordinal = value;
            }
        }

        public string Owner
        {
            get
            {
                return InternalItem.Owner;
            }
            set
            {
                InternalItem.Owner = value;
            }
        }

        public MSOutlook.OlTaskOwnership Ownership
        {
            get { return InternalItem.Ownership; }
        }

        public int PercentComplete
        {
            get
            {
                return InternalItem.PercentComplete;
            }
            set
            {
                InternalItem.PercentComplete = value;
            }
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

        public MSOutlook.TaskItem Respond(MSOutlook.OlTaskResponse Response, object fNoUI, object fAdditionalTextDialog)
        {
            return InternalItem.Respond(Response, fNoUI, fAdditionalTextDialog);
        }

        public MSOutlook.OlTaskResponse ResponseState
        {
            get { return InternalItem.ResponseState; }
        }

        public string Role
        {
            get
            {
                return InternalItem.Role;
            }
            set
            {
                InternalItem.Role = value;
            }
        }

        public string SchedulePlusPriority
        {
            get
            {
                return InternalItem.SchedulePlusPriority;
            }
            set
            {
                InternalItem.SchedulePlusPriority = value;
            }
        }

        public bool SkipRecurrence()
        {
            return InternalItem.SkipRecurrence();
        }

        public DateTime StartDate
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.StartDate);
            }
            set
            {
                InternalItem.StartDate = Helpers.LocalToLocal(value);
            }
        }

        public MSOutlook.OlTaskStatus Status
        {
            get
            {
                return InternalItem.Status;
            }
            set
            {
                InternalItem.Status = value;
            }
        }

        public string StatusOnCompletionRecipients
        {
            get
            {
                return InternalItem.StatusOnCompletionRecipients;
            }
            set
            {
                InternalItem.StatusOnCompletionRecipients = value;
            }
        }

        public object StatusReport()
        {
            return InternalItem.StatusReport();
        }

        public string StatusUpdateRecipients
        {
            get
            {
                return InternalItem.StatusUpdateRecipients;
            }
            set
            {
                InternalItem.StatusUpdateRecipients = value;
            }
        }

        public bool TeamTask
        {
            get
            {
                return InternalItem.TeamTask;
            }
            set
            {
                InternalItem.TeamTask = value;
            }
        }



        public int TotalWork
        {
            get
            {
                return InternalItem.TotalWork;
            }
            set
            {
                InternalItem.TotalWork = value;
            }
        }

        #endregion

        #region Redemption

        new protected internal Redemption.SafeTaskItem SafeItem
        {
            get
            {
                return (Redemption.SafeTaskItem)base.SafeItem;
            }
        }

        new protected internal Redemption.RDOTaskItem RDOItem
        {
            get
            {
                return (Redemption.RDOTaskItem)base.RDOItem;
            }
        }

        #endregion
    }
}
