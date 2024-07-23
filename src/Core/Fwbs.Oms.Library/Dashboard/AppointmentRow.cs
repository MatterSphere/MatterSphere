using System;

namespace FWBS.OMS.Dashboard
{
    public class AppointmentRow
    {
        public AppointmentRow(long id, string type, DateTime start, DateTime end, string description, string location, bool allDay = false)
        {
            Id = id;
            AppointmentType = type;
            Start = start;
            End = allDay ? start.AddDays(1) : end;
            Description = description;
            Location = location;
            AllDay = allDay;
        }

        public long Id { get; }
        public string AppointmentType { get; }
        public DateTime Start { get; }
        public DateTime End { get; }
        public string Description { get; }
        public string Location { get; }
        public bool AllDay { get; }

        public bool IsAllDay(DateTime day)
        {
            return Start <= day.Date && End >= day.Date.AddDays(1);
        }

        public DateTime GetStartTime(DateTime day)
        {
            return Start < day.Date
                ? day.Date
                : Start;
        }

        public DateTime GetEndTime(DateTime day)
        {
            var date = day.AddDays(1).Date;
            return End >= date
                ? date
                : End;
        }
    }
}
