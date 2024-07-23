using System;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookAppointment
    {
        public DateTime EndInEndTimeZone
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.EndInEndTimeZone);
            }
            set
            {
                InternalItem.EndInEndTimeZone = Helpers.LocalToLocal(value);
            }
        }

        public MSOutlook.TimeZone EndTimeZone
        {
            get
            {
                return InternalItem.EndTimeZone;
            }
            set
            {
                InternalItem.EndTimeZone = value;
            }
        }

        public DateTime EndUTC
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.EndUTC);
            }
            set
            {
                InternalItem.EndUTC = Helpers.LocalToLocal(value);
            }
        }

        public bool ForceUpdateToAllAttendees
        {
            get
            {
                return InternalItem.ForceUpdateToAllAttendees;
            }
            set
            {
                InternalItem.ForceUpdateToAllAttendees = value;
            }
        }

        public DateTime StartInStartTimeZone
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.StartInStartTimeZone);
            }
            set
            {
                InternalItem.StartInStartTimeZone = Helpers.LocalToLocal(value);
            }
        }

        public MSOutlook.TimeZone StartTimeZone
        {
            get
            {
                return InternalItem.StartTimeZone;
            }
            set
            {
                InternalItem.StartTimeZone = value;
            }
        }

        public DateTime StartUTC
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.StartUTC);
            }
            set
            {
                InternalItem.StartUTC = Helpers.LocalToLocal(value);
            }
        }


        public string GlobalAppointmentID
        {
            get { return InternalItem.GlobalAppointmentID; }
        }
    }
}
