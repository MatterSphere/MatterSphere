using System;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    internal class DayBuilder : BaseCalendarBuilder
    {
        #region BaseCalendarBuilder

        public override void Build(ICalendarView view, DateTime date)
        {
            var dayView = view as ucDayView;
            if (dayView == null)
            {
                throw new ArgumentException();
            }

            dayView.SetDay(date);
        }

        public override void Rebuild(ICalendarView view, DateTime date)
        {
            var dayView = view as ucDayView;
            if (dayView == null)
            {
                throw new ArgumentException();
            }

            dayView.SetDay(date);
            dayView.UpdateHeaders(date);
        }

        #endregion
    }
}
