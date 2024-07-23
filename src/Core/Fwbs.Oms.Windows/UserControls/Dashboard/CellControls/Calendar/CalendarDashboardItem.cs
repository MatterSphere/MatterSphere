using System;
using System.Collections.Generic;
using System.Linq;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    internal partial class CalendarDashboardItem : BaseContainerPage
    {
        private CalendarView _calendarView;
        private List<AppointmentRow> _appointments;

        public CalendarDashboardItem()
        {
            InitializeComponent();

            HideBottomPanel = true;
            HideSearchButton = true;
            _calendarView = CalendarView.Month;
            ucCalendarPage.ViewChanged += OnCalendarViewChange;
            ucCalendarPage.MonthChanged += OnSelectedMonthChange;
            ucCalendarPage.WeekChanged += OnWeekChanged;
            ucCalendarPage.DayChanged += OnDayChanged;
        }

        #region BaseContainerPage

        public override void UpdateData(bool withScale = false)
        {
            LoadAppointments();
        }

        #endregion

        #region Handlers

        private void OnCalendarViewChange(object sender, CalendarView e)
        {
            OpenCalendarView(e);
        }

        private void OnSelectedMonthChange(object sender, EventArgs e)
        {
            LoadAppointments();
        }

        private void OnWeekChanged(object sender, DateTime e)
        {
            LoadAppointments();
        }

        private void OnDayChanged(object sender, DateTime e)
        {
            LoadAppointments();
        }

        #endregion

        #region Private methods

        private void OpenCalendarView(CalendarView view)
        {
            _calendarView = view;

            switch (_calendarView)
            {
                case CalendarView.Month:
                    ucCalendarPage.OpenMonthView();
                    break;
                case CalendarView.Week:
                    ucCalendarPage.OpenWeekView();
                    break;
                case CalendarView.Day:
                    ucCalendarPage.OpenDayView();
                    break;
            }

            LoadAppointments();
        }

        private void LoadAppointments()
        {
            var interval = ucCalendarPage.GetDateInterval();
            _appointments = DashboardTileDataProvider.GetAppointments(interval[0], interval[1]).OrderBy(app => app.Start).ToList();
            ucCalendarPage.SetAppointments(_appointments);
        }

        #endregion
    }
}
