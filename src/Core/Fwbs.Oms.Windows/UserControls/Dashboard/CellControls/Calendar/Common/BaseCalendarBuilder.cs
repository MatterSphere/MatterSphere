using System;
using System.Collections.Generic;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common
{
    internal abstract class BaseCalendarBuilder
    {
        protected DateHelper _dateHelper;

        protected BaseCalendarBuilder()
        {
            _dateHelper = new DateHelper();
        }

        public abstract void Build(ICalendarView view, DateTime date);
        public abstract void Rebuild(ICalendarView view, DateTime date);

        public void SetHeaders(ICalendarView view)
        {
            var headers = GetHeaders();
            for (int i = 0; i < headers.Count; i++)
            {
                view.InsertHeader(headers[i], i);
            }
        }

        protected List<ucCalendarDayHeader> GetHeaders()
        {
            var headers = new List<ucCalendarDayHeader>();

            var sunday = new ucCalendarDayHeader();
            sunday.SetTitle(DayOfWeek.Sunday);
            headers.Add(sunday);

            var monday = new ucCalendarDayHeader();
            monday.SetTitle(DayOfWeek.Monday);
            headers.Add(monday);

            var tuesday = new ucCalendarDayHeader();
            tuesday.SetTitle(DayOfWeek.Tuesday);
            headers.Add(tuesday);

            var wednesday = new ucCalendarDayHeader();
            wednesday.SetTitle(DayOfWeek.Wednesday);
            headers.Add(wednesday);

            var thursday = new ucCalendarDayHeader();
            thursday.SetTitle(DayOfWeek.Thursday);
            headers.Add(thursday);

            var friday = new ucCalendarDayHeader();
            friday.SetTitle(DayOfWeek.Friday);
            headers.Add(friday);

            var saturday = new ucCalendarDayHeader();
            saturday.SetTitle(DayOfWeek.Saturday);
            headers.Add(saturday);

            return headers;
        }
    }
}
