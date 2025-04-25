using System;
using System.Collections.Generic;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class DateHelper
    {
        #region Methods
        public DateTime GetFirstMonthDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        public DateTime GetLastMonthDate(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
        }

        public int GetWeeksCount(DateTime date)
        {
            var firstDateOfMonth = GetFirstMonthDate(date);
            var daysInMonth = DateTime.DaysInMonth(firstDateOfMonth.Year, firstDateOfMonth.Month);
            var firstDayOfWeek = (int)firstDateOfMonth.DayOfWeek;
            var totalDaysWithOffset = firstDayOfWeek + daysInMonth;
            var numberOfWeeks = totalDaysWithOffset / 7;

            // Check if there are extra days that do not fit into a complete row
            if (totalDaysWithOffset % 7 != 0)
            {
                numberOfWeeks++;  // Add an extra week for those remaining days
            }

            return numberOfWeeks;
        }

        public List<WeekInfo> GetWeeks(DateTime date)
        {
            var weeksCount = GetWeeksCount(date);
            var firstMontDate = GetFirstMonthDate(date);
            var weeks = new List<WeekInfo>();
            for (int i = 0; i < weeksCount; i++)
            {
                var week = new WeekInfo(
                    firstMontDate.Date.AddDays(-(int)firstMontDate.DayOfWeek + 7 * i),
                    firstMontDate.Date.AddDays((7 - (int)firstMontDate.DayOfWeek) + 7 * i),
                    i + 1,
                    firstMontDate);
                weeks.Add(week);
            }

            return weeks;
        }

        public List<DateTime> GetWeekDays(DateTime date)
        {
            var days = new List<DateTime>();
            for (int i = 0; i < 7; i++)
            {
                days.Add(date.AddDays(i-(int)date.DayOfWeek));
            }

            return days;
        }

        public int GetWeekNumberOfMonth(DateTime date)
        {
            var beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
            }

            return (int)Math.Truncate(date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        #endregion

        #region Private methods

        private int GetLastMonthDay(DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1).Day;
        }

        #endregion

        #region Classes

        public class WeekInfo
        {
            public WeekInfo(DateTime start, DateTime end, int number, DateTime firstMonthDay)
            {
                Start = start;
                End = end;
                WeekNumber = number;
                FirstMonthDay = firstMonthDay;
            }

            public DateTime Start { get; private set; }
            public DateTime End { get; private set; }
            public int WeekNumber { get; private set; }
            public DateTime FirstMonthDay { get; private set; }
            
            public string Interval
            {
                get { return $"{Start.Day} - {End.AddDays(-1).Day}"; }
            }
        }

        #endregion
    }
}
