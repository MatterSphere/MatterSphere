using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Month;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;
using FWBS.OMS.UI.Windows;
using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    internal partial class ucWeekView : UserControl, ICalendarView
    {
        private DateHelper _dateHelper;
        private ucWeekPickerPopup _popup;
        private ucCalendarPopupContainer _container;
        private DateTime _firstDate;
        private ucCalendarDay[] _dates;
        private UltraPeekPopup _detailsPopup;
        private ucHourItem _selectedHour;

        public ucWeekView()
        {
            InitializeComponent();

            _dateHelper = new DateHelper();
            SetTimeLabels();
            MoveToCurrentTime();
            this.tlpHoursContainer.MouseWheel += OnMouseWheel;
        }

        #region ICalendarView

        public event EventHandler<DateTime> FilterChanged;

        public DateTime SelectedMonth { get; set; }

        public void InsertHeader(ucCalendarDayHeader header, int position)
        {
            header.Dock = DockStyle.Fill;
            tlpContainer.Controls.Add(header, position + 1, 0);
        }

        public void ResetContainer()
        {
            for (int day = 0; day < 7; day++)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    var hourItem = tlpHoursContainer.GetControlFromPosition(day + 1, hour) as ucHourItem;
                    if (hourItem != null)
                    {
                        hourItem.Clicked -= HourItemClicked;
                        hourItem.ResetContent();
                        hourItem.Dispose();
                    }
                }
            }
        }

        public void SetAppointments(List<AppointmentRow> appointments)
        {
            tlpHoursContainer.SuspendLayout();
            for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
            {
                var day = GetDayCell(dayOfWeek);
                if (day.Date.Month != SelectedMonth.Month)
                {
                    continue;
                }

                day.SetAppointments(appointments.Where(app =>
                        (app.Start < day.Date.AddDays(1) && app.End > day.Date) ||
                        (app.AllDay && app.Start.Date == day.Date) ||
                        (app.Start == day.Date && app.End == day.Date)).ToList());

                ucHourItem prevCell = null;
                for (int hour = 0; hour < 24; hour++)
                {
                    var cell = GetHourCell(dayOfWeek, hour);
                    var appList = appointments.Where(app =>
                        (app.Start < cell.Hour.AddHours(1) && app.End > cell.Hour) ||
                        (app.AllDay && app.Start.Date == cell.Hour.Date) ||
                        (app.Start == cell.Hour && app.End == cell.Hour)).ToList();
                    if (!appList.Any())
                    {
                        continue;
                    }

                    cell.SetAppointments(appList, prevCell);
                    prevCell = cell;
                }
            }

            tlpHoursContainer.ResumeLayout();
        }

        public DateTime[] GetInterval()
        {
            return new []
            {
                StartDate,
                EndDate
            };
        }

        #endregion
        
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        #region Methods

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent != null)
            {
                this.tlpContainer.ColumnStyles[8].Width = SystemInformation.GetVerticalScrollBarWidthForDpi(tlpContainer.DeviceDpi);
            }
        }

        public void SetWeekDays(DateTime firstWeekDate, DateTime month)
        {
            SelectedMonth = month;
            SetWeek(firstWeekDate);
        }

        public void InsertHour(ucHourItem cell, int dayOfWeek, int hour)
        {
            tlpHoursContainer.Controls.Add(cell, dayOfWeek+1, hour, true);
            cell.Clicked += HourItemClicked;
        }

        public void UpdateHours()
        {
            for (int day = 0; day < 7; day++)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    var hourItem = tlpHoursContainer.GetControlFromPosition(day + 1, hour) as ucHourItem;
                    hourItem.ResetContent();
                    hourItem.Day = StartDate.AddDays(day);
                    hourItem.Hour = hourItem.Day.AddHours(hour);
                }
            }
        }

        #endregion

        #region UI events

        private void weekPicker_Click(object sender, EventArgs e)
        {
            ShowWeekPickerPopup();
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (_detailsPopup != null && _detailsPopup.Visible)
            {
                _detailsPopup.Close();
            }
        }

        #endregion

        #region Private methods

        private void SetTimeLabels()
        {
            var time = DateTime.Now.Date;
            for (int i = 0; i < 24; i++)
            {
                var label = new Label
                {
                    Dock = DockStyle.Fill,
                    Font = new Font("Segoe UI", 6F),
                    Text = time.AddHours(i).ToString("h tt"),
                    TextAlign = ContentAlignment.TopRight
                };

                tlpHoursContainer.Controls.Add(label, 0, i);
            }
        }

        private void ShowWeekPickerPopup()
        {
            var weeks = _dateHelper.GetWeeks(_firstDate);
            _popup = new ucWeekPickerPopup();
            _popup.SetWeeks(weeks);
            _popup.ChangeDpi(this.DeviceDpi);
            _container = new ucCalendarPopupContainer(_popup);
            _popup.WeekClicked += OnWeekSelected;
            _container.Show(this.weekPicker);
        }

        private void SetWeek(DateTime firstWeekDate)
        {
            var days = _dateHelper.GetWeekDays(firstWeekDate);

            StartDate = days[0].Date;
            EndDate = days[6].Date;
            _firstDate = days.First(day => day.Month == SelectedMonth.Month);

            weekPicker.SetTitle(_firstDate);

            if (_dates == null)
            {
                _dates = new ucCalendarDay[7];
                for (int i = 0; i < 7; i++)
                {
                    var calendarDay = new ucCalendarDay(true)
                    {
                        Dock = DockStyle.Fill
                    };

                    calendarDay.SetDay(days[i], days[i].Month == SelectedMonth.Month);
                    tlpContainer.Controls.Add(calendarDay, i + 1, 1);
                    _dates[i] = calendarDay;
                }
            }
            else
            {
                for (int i = 0; i < 7; i++)
                {
                    _dates[i].SetDay(days[i], days[i].Month == SelectedMonth.Month);
                }
            }
        }

        private ucCalendarDay GetDayCell(int dayOfWeek)
        {
            return tlpContainer.GetControlFromPosition(dayOfWeek + 1, 1) as ucCalendarDay;
        }

        private ucHourItem GetHourCell(int dayOfWeek, int hour)
        {
            return tlpHoursContainer.GetControlFromPosition(dayOfWeek + 1, hour) as ucHourItem;
        }

        private void ShowAppointmentsDetailsPopup(ucHourItem hourItem, List<AppointmentRow> appointments)
        {
            _detailsPopup = new UltraPeekPopup
            {
                ContentMargin = new Padding(1),
                Appearance = new Infragistics.Win.Appearance
                {
                    BorderColor = Color.FromArgb(234, 234, 234)
                }
            };

            _detailsPopup.Closed += OnPopupClose;

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
                item.SetAppointment(app, hourItem);
                container.Controls.Add(item, true);
            }

            _detailsPopup.Content = container;

            var location = hourItem.PointToScreen(Point.Empty);
            _selectedHour = hourItem;
            _detailsPopup.Show(new Point(location.X + hourItem.Size.Width / 2, location.Y + hourItem.Size.Height / 2), Infragistics.Win.Peek.PeekLocation.RightOfItem);
        }
        
        private void UnselectCell()
        {
            if (_selectedHour != null)
            {
                _selectedHour.Unselect();
                _selectedHour = null;
            }
        }

        private void MoveToCurrentTime()
        {
            tlpHoursContainer.AutoScroll = false;
            var height = LogicalToDeviceUnits(Convert.ToInt32(tlpHoursContainer.RowStyles[1].Height));
            tlpHoursContainer.VerticalScroll.Maximum = height * 24;
            tlpHoursContainer.VerticalScroll.Value = height * DateTime.Now.Hour;
            tlpHoursContainer.AutoScroll = true;
        }

        #endregion

        #region Handlers

        private void OnWeekSelected(object sender, DateTime e)
        {
            _container.Close();
            FilterChanged?.Invoke(this, e);
        }

        private void HourItemClicked(object sender, List<AppointmentRow> e)
        {
            var item = sender as ucHourItem;
            if (item != null)
            {
                item.Focus();
                ShowAppointmentsDetailsPopup(item, e);
            }
        }

        private void OnPopupClose(object sender, EventArgs e)
        {
            UnselectCell();
        }

        #endregion
    }
}
