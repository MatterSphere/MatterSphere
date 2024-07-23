using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common;
using FWBS.OMS.UI.Windows;
using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Month
{
    internal partial class ucMonthView : UserControl, ICalendarView
    {
        private List<ucCalendarDay> _days;
        private UltraPeekPopup _popup;
        private ucCalendarDay _selectedDay;

        public ucMonthView()
        {
            InitializeComponent();

            _days = new List<ucCalendarDay>();
        }
        
        #region ICalendarView

        public event EventHandler<DateTime> FilterChanged;

        public DateTime SelectedMonth { get; set; }

        public void InsertHeader(ucCalendarDayHeader header, int position)
        {
            header.Dock = DockStyle.Fill;
            tlpContainer.Controls.Add(header, position, 0);
        }

        public void ResetContainer()
        {
            tlpContainer.SuspendLayout();

            foreach (var day in _days)
            {
                day.DayClicked -= OnDayClick;
                tlpContainer.Controls.Remove(day);
                day.Dispose();
            }

            var count = tlpContainer.RowStyles.Count - 1;
            for (int i = 0; i < count; i++)
            {
                tlpContainer.RowStyles.RemoveAt(tlpContainer.RowStyles.Count - 1);
            }

            tlpContainer.RowCount = 1;
            tlpContainer.ResumeLayout();
        }
        
        public void SetAppointments(List<AppointmentRow> appointments)
        {
            var days = _days.Where(day => day.Date.Month == SelectedMonth.Month && day.Date.Year == SelectedMonth.Date.Year);
            foreach (var day in days)
            {
                day.SetAppointments(appointments.Where(app =>
                        (app.Start.Date <= day.Date && app.End > day.Date) ||
                        (app.AllDay && app.Start.Date == day.Date))
                    .ToList());
            }
        }

        public DateTime[] GetInterval()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Methods

        public void InsertDay(ucCalendarDay day, int column, int row)
        {
            day.Dock = DockStyle.Fill;
            day.DayClicked += OnDayClick;
            tlpContainer.Controls.Add(day, column, row + 1);
            _days.Add(day);
        }

        public void AddRows(int number)
        {
            tlpContainer.SuspendLayout();
            for (int i = 0; i < number; i++)
            {
                tlpContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / number));
            }

            tlpContainer.RowCount = tlpContainer.RowStyles.Count;
            tlpContainer.ResumeLayout();
        }

        #endregion

        #region Handlers

        private void OnDayClick(object sender, List<AppointmentRow> e)
        {
            var calendarDay = sender as ucCalendarDay;
            ShowAppointmentsDetailsPopup(calendarDay, e);
        }

        private void OnPopupClose(object sender, EventArgs e)
        {
            UnselectDay();
        }

        #endregion

        #region Private methods

        private void ShowAppointmentsDetailsPopup(ucCalendarDay calendarDay, List<AppointmentRow> appointments)
        {
            _popup = new UltraPeekPopup
            {
                ContentMargin = new Padding(1),
                Appearance = new Infragistics.Win.Appearance
                {
                    BorderColor = Color.FromArgb(234, 234, 234)
                }
            };

            _popup.Closed += OnPopupClose;

            var container = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };

            foreach (var app in appointments)
            {
                var item = new ucAppointmentDetailsItem
                {
                    Dock = DockStyle.Top
                };
                item.SetAppointment(app, calendarDay);
                container.Controls.Add(item, true);
            }

            _popup.Content = container;

            var location = calendarDay.PointToScreen(Point.Empty);
            _selectedDay = calendarDay;
            _popup.Show(new Point(location.X + calendarDay.Size.Width / 2, location.Y + calendarDay.Size.Height / 2), Infragistics.Win.Peek.PeekLocation.RightOfItem);
        }

        private void UnselectDay()
        {
            if (_selectedDay != null)
            {
                _selectedDay.Unselect();
                _selectedDay = null;
            }
        }

        #endregion
    }
}
