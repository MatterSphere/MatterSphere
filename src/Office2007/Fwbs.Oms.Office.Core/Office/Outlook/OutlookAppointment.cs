using System;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public partial class OutlookAppointment : OutlookItem, MSOutlook.AppointmentItem
    {
        private MSOutlook.AppointmentItem appnt;

        public OutlookAppointment(MSOutlook.AppointmentItem appointment)
            : base(appointment)
        {
        }

        new private MSOutlook.AppointmentItem InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return appnt;
            }
        }

        protected override void OnAttach(object obj)
        {
            base.OnAttach(obj);
            appnt = (MSOutlook.AppointmentItem)obj;
        }

        protected override void OnDetach()
        {
            appnt = null;
            base.OnDetach();
        }

        #region _AppointmentItem Members

        MSOutlook.Application MSOutlook._AppointmentItem.Application
        {
            get
            {
                return base.Application;
            }
        }

        public bool AllDayEvent
        {
            get
            {
                return InternalItem.AllDayEvent;
            }
            set
            {
                InternalItem.AllDayEvent = value;
            }
        }

        public MSOutlook.OlBusyStatus BusyStatus
        {
            get
            {
                return InternalItem.BusyStatus;
            }
            set
            {
                InternalItem.BusyStatus = value;
            }
        }

        public void ClearRecurrencePattern()
        {
            InternalItem.ClearRecurrencePattern();
        }

        public bool ConferenceServerAllowExternal
        {
            get
            {
                return InternalItem.ConferenceServerAllowExternal;
            }
            set
            {
                InternalItem.ConferenceServerAllowExternal = value;
            }
        }

        public string ConferenceServerPassword
        {
            get
            {
                return InternalItem.ConferenceServerPassword;
            }
            set
            {
                InternalItem.ConferenceServerPassword = value;
            }
        }

        public int Duration
        {
            get
            {
                return InternalItem.Duration;
            }
            set
            {
                InternalItem.Duration = value;
            }
        }

        public DateTime End
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.End);
            }
            set
            {
                InternalItem.End = Helpers.LocalToLocal(value);
            }
        }

     

        public MSOutlook.MailItem ForwardAsVcal()
        {
            return InternalItem.ForwardAsVcal();
        }

        public MSOutlook.RecurrencePattern GetRecurrencePattern()
        {
            return InternalItem.GetRecurrencePattern();
        }



        public bool IsOnlineMeeting
        {
            get
            {
                return InternalItem.IsOnlineMeeting;
            }
            set
            {
                InternalItem.IsOnlineMeeting = value;
            }
        }

        public bool IsRecurring
        {
            get { return InternalItem.IsRecurring; }
        }

        public string Location
        {
            get
            {
                return InternalItem.Location;
            }
            set
            {
                InternalItem.Location = value;
            }
        }

        public MSOutlook.OlMeetingStatus MeetingStatus
        {
            get
            {
                return InternalItem.MeetingStatus;
            }
            set
            {
                InternalItem.MeetingStatus = value;
            }
        }

        public string MeetingWorkspaceURL
        {
            get { return InternalItem.MeetingWorkspaceURL; }
        }

        public bool NetMeetingAutoStart
        {
            get
            {
                return InternalItem.NetMeetingAutoStart;
            }
            set
            {
                InternalItem.NetMeetingAutoStart = value;
            }
        }

        public string NetMeetingDocPathName
        {
            get
            {
                return InternalItem.NetMeetingDocPathName;
            }
            set
            {
                InternalItem.NetMeetingDocPathName = value;
            }
        }

        public string NetMeetingOrganizerAlias
        {
            get
            {
                return InternalItem.NetMeetingOrganizerAlias;
            }
            set
            {
                InternalItem.NetMeetingOrganizerAlias = value;
            }
        }

        public string NetMeetingServer
        {
            get
            {
                return InternalItem.NetMeetingServer;
            }
            set
            {
                InternalItem.NetMeetingServer = value;
            }
        }

        public MSOutlook.OlNetMeetingType NetMeetingType
        {
            get
            {
                return InternalItem.NetMeetingType;
            }
            set
            {
                InternalItem.NetMeetingType = value;
            }
        }

        public string NetShowURL
        {
            get
            {
                return InternalItem.NetShowURL;
            }
            set
            {
                InternalItem.NetShowURL = value;
            }
        }


        public string OptionalAttendees
        {
            get
            {
                return InternalItem.OptionalAttendees;
            }
            set
            {
                InternalItem.OptionalAttendees = value;
            }
        }

        public string Organizer
        {
            get { return InternalItem.Organizer; }
        }

        public MSOutlook.OlRecurrenceState RecurrenceState
        {
            get { return InternalItem.RecurrenceState; }
        }

        public int ReminderMinutesBeforeStart
        {
            get
            {
                return InternalItem.ReminderMinutesBeforeStart;
            }
            set
            {
                InternalItem.ReminderMinutesBeforeStart = value;
            }
        }


        public DateTime ReplyTime
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.ReplyTime);
            }
            set
            {
                InternalItem.ReplyTime = Helpers.LocalToLocal(value);
            }
        }

        public string RequiredAttendees
        {
            get
            {
                return InternalItem.RequiredAttendees;
            }
            set
            {
                InternalItem.RequiredAttendees = value;
            }
        }

        public string Resources
        {
            get
            {
                return InternalItem.Resources;
            }
            set
            {
                InternalItem.Resources = value;
            }
        }

        public MSOutlook.MeetingItem Respond(MSOutlook.OlMeetingResponse Response, object fNoUI, object fAdditionalTextDialog)
        {
            return InternalItem.Respond(Response, fNoUI, fAdditionalTextDialog);
        }

        public bool ResponseRequested
        {
            get
            {
                return InternalItem.ResponseRequested;
            }
            set
            {
                InternalItem.ResponseRequested = value;
            }
        }

        public MSOutlook.OlResponseStatus ResponseStatus
        {
            get { return InternalItem.ResponseStatus; }
        }

        public DateTime Start
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.Start);
            }
            set
            {
                InternalItem.Start = Helpers.LocalToLocal(value);
            }
        }


        public override OutlookFolder Folder
        {
            get
            {
                if (IsRecurring)
                {
                    return Application.GetFolderFromId(FolderID, StoreID);
                }
                else
                    return base.Folder;
                
            }
        }

        #endregion

        #region Redemption

        new protected internal Redemption.SafeAppointmentItem SafeItem
        {
            get
            {
                return (Redemption.SafeAppointmentItem)base.SafeItem;
            }
        }

        new protected internal Redemption.RDOAppointmentItem RDOItem
        {
            get
            {
                return (Redemption.RDOAppointmentItem)base.RDOItem;
            }
        }

        #endregion

        #region Getters & Setters

        protected override string GetFolderID(MSOutlook.ItemEvents_10_Event item, Redemption._ISafeItem safeItem)
        {
            var apnt = (MSOutlook.AppointmentItem)item;
            var p = apnt.Parent;

            if (apnt.IsRecurring)
            {
                while (p != null)
                {
                    var folder = p as MSOutlook.MAPIFolder;
                    if (folder != null)
                        return folder.EntryID;

                    p = GetPropertyEx<object>(p, "Parent");
                }
            }

            return base.GetFolderID(item, safeItem);
        }

        #endregion
    }
}
