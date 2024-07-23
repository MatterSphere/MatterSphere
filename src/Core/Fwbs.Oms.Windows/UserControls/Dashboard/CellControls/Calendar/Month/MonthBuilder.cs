using System;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Month
{
    internal class MonthBuilder : BaseCalendarBuilder
    {
        #region BaseCalendarBuilder

        public override void Build(ICalendarView view, DateTime date)
        {
            SetHeaders(view);
            SetDays(view, date);
        }

        public override void Rebuild(ICalendarView view, DateTime date)
        {
            view.ResetContainer();
            SetDays(view, date);
        }

        #endregion

        private void SetDays(ICalendarView view, DateTime selectedMonth)
        {
            var calendar = view as ucMonthView;
            if (calendar == null)
            {
                throw new ArgumentException();
            }

            calendar.SelectedMonth = selectedMonth;
            var firstDate = _dateHelper.GetFirstMonthDate(selectedMonth);
            var weeks = _dateHelper.GetWeeksCount(firstDate);
            calendar.AddRows(weeks);
            
            CreateFirstWeek(firstDate, calendar);

            var firstMonthDay = firstDate.DayOfWeek;
            for (int row = 1; row <= weeks - 2; row++)
            {
                for (int column = 0; column < 7; column++)
                {
                    var currentMonthDay = new ucCalendarDay();
                    currentMonthDay.SetDay(
                        firstDate.AddDays(row * 7 + column - (int)firstMonthDay).Day,
                        firstDate.Month,
                        firstDate.Year);
                    calendar.InsertDay(currentMonthDay, column, row);
                }
            }

            CreateLastWeek(firstDate, calendar, weeks);
        }

        private void CreateFirstWeek(DateTime firstDate, ucMonthView calendar)
        {
            var firstMonthDay = firstDate.DayOfWeek;
            for (int column = 0; column < 7; column++)
            {
                if (column < (int)firstMonthDay)
                {

                    var prevMonthDay = new ucCalendarDay();
                    prevMonthDay.SetDay(
                        firstDate.AddDays(column - (int)firstMonthDay).Day,
                        firstDate.AddMonths(-1).Month,
                        firstDate.Month == 1
                            ? firstDate.AddYears(-1).Year
                            : firstDate.Year,
                        false);
                    calendar.InsertDay(prevMonthDay, column, 0);
                }
                else
                {
                    var currentMonthDay = new ucCalendarDay();
                    currentMonthDay.SetDay(
                        firstDate.AddDays(column - (int)firstMonthDay).Day,
                        firstDate.Month,
                        firstDate.Year);
                    calendar.InsertDay(currentMonthDay, column, 0);
                }
            }
        }

        private void CreateLastWeek(DateTime firstDate, ucMonthView calendar, int weeks)
        {
            var lastDate = _dateHelper.GetLastMonthDate(firstDate);
            var lastMonthDay = lastDate.DayOfWeek;
            for (int column = 0; column < 7; column++)
            {
                if (column > (int)lastMonthDay)
                {
                    var nextMonthDay = new ucCalendarDay();
                    nextMonthDay.SetDay(
                        lastDate.AddDays(column - (int)lastMonthDay).Day,
                        lastDate.AddMonths(1).Month,
                        lastDate.Month == 12
                            ? lastDate.AddYears(1).Year
                            : lastDate.Year,
                        false);
                    calendar.InsertDay(nextMonthDay, column, weeks - 1);
                }
                else
                {
                    var currentMonthDay = new ucCalendarDay();
                    currentMonthDay.SetDay(
                        lastDate.AddDays(column - (int)lastMonthDay).Day,
                        lastDate.Month,
                        lastDate.Year);
                    calendar.InsertDay(currentMonthDay, column, weeks - 1);
                }
            }
        }
    }
}
