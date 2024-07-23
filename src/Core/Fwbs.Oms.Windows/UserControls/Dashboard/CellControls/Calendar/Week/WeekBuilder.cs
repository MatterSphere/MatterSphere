using System;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    internal class WeekBuilder : BaseCalendarBuilder
    {
        #region BaseCalendarBuilder

        public override void Build(ICalendarView view, DateTime date)
        {
            SetHeaders(view);
            var weekView = view as ucWeekView;
            if (weekView == null)
            {
                throw new ArgumentException();
            }

            SetWeekDays(weekView, date, true);
            SetHourItems(weekView);
        }

        public override void Rebuild(ICalendarView view, DateTime date)
        {
            var weekView = view as ucWeekView;
            if (weekView == null)
            {
                throw new ArgumentException();
            }

            SetWeekDays(weekView, date);
            weekView.UpdateHours();
        }

        #endregion

        private void SetWeekDays(ucWeekView weekView, DateTime date, bool autoWeek = false)
        {
            var firstWeekDate = date == new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1) && autoWeek
                ? _dateHelper.GetWeekDays(DateTime.Now)[0]
                : _dateHelper.GetWeekDays(date)[0];
            weekView.SetWeekDays(firstWeekDate, date);
        }

        private void SetHourItems(ucWeekView weekView)
        {
            for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    var day = weekView.StartDate.AddDays(dayOfWeek);
                    var hourCell = new ucHourItem(day, hour)
                    {
                        Dock = DockStyle.Fill
                    };

                    weekView.InsertHour(hourCell, dayOfWeek, hour);
                }
            }
        }
    }
}
