using System;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.KeyDates;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    internal class CalendarPageProvider : IPageProvider
    {
        private const string KEY_DATES_TITLE_CODE = "KDTS";
        private const string CALENDAR_TITLE_CODE = "CLNDR";

        public CalendarPageProvider()
        {
            if (KeyDatesDashboardCell.CanShowKeyDatesTile)
            {
                Headers = new[]
                {
                    CalendarPage.Calendar.ToString(),
                    CalendarPage.KeyDates.ToString()
                };
            }
            else
            {
                Headers = new[]
                {
                    CalendarPage.Calendar.ToString()
                };
            }
        }

        public string[] Headers { get; }

        public BaseContainerPage GetPage(string header)
        {
            if (header == CalendarPage.Calendar.ToString())
            {
                return CreateCalendarPage();
            }

            if (header == CalendarPage.KeyDates.ToString())
            {
                return CreateKeyDatesPage();
            }

            throw new ArgumentOutOfRangeException();
        }

        public PageDetails GetDetails(string header)
        {
            if (header == CalendarPage.Calendar.ToString())
            {
                return new PageDetails(null, CodeLookup.GetLookup("DASHBOARD", CALENDAR_TITLE_CODE, "Calendar"));
            }

            if (header == CalendarPage.KeyDates.ToString())
            {
                return new PageDetails(null, CodeLookup.GetLookup("DASHBOARD", KEY_DATES_TITLE_CODE, "Key Dates"));
            }

            throw new ArgumentOutOfRangeException();
        }

        private BaseContainerPage CreateKeyDatesPage()
        {
            var item = new KeyDatesDashboardItem
            {
                Code = CalendarPage.KeyDates.ToString(),
                Title = CodeLookup.GetLookup("DASHBOARD", KEY_DATES_TITLE_CODE, "Key Dates"),
                Dock = DockStyle.Fill
            };

            return item;
        }

        private BaseContainerPage CreateCalendarPage()
        {
            var item = new CalendarDashboardItem
            {
                Code = CalendarPage.Calendar.ToString(),
                Title = CodeLookup.GetLookup("DASHBOARD", CALENDAR_TITLE_CODE, "Calendar"),
                Dock = DockStyle.Fill
            };

            return item;
        }
    }

    public enum CalendarPage
    {
        KeyDates,
        Calendar
    }
}
