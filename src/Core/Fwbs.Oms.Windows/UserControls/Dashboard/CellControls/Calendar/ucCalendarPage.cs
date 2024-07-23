using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Month;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    internal partial class ucCalendarPage : UserControl
    {
        private ucCalendarPickerPopup _popup;
        private ucCalendarPopupContainer _container;

        private CalendarView _calendarView;
        private ICalendarView _view;
        private BaseCalendarBuilder _builder;

        public ucCalendarPage()
        {
            InitializeComponent();

            _calendarView = CalendarView.Month;
            SelectedMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            monthPicker.SetTitle(SelectedMonth);

            _builder = new MonthBuilder();
            _builder.Build(ucCalendar, SelectedMonth);
            _view = ucCalendar;

            SetRadioButtonsWidth();
        }

        public EventHandler<CalendarView> ViewChanged;
        public EventHandler MonthChanged;
        public EventHandler<DateTime> WeekChanged;
        public EventHandler<DateTime> DayChanged;

        public DateTime SelectedMonth { get; private set; }

        #region Methods

        public void SetAppointments(List<AppointmentRow> appointments)
        {
            _view.SetAppointments(appointments);
        }

        public DateTime[] GetDateInterval()
        {
            switch (_calendarView)
            {
                case CalendarView.Month:
                    return new DateTime[]
                    {
                        SelectedMonth.Date,
                        SelectedMonth.Date.AddMonths(1)
                    };
                case CalendarView.Week:
                case CalendarView.Day:
                    return _view.GetInterval();
            }

            throw new InvalidOperationException();
        }

        public void OpenMonthView()
        {
            _view = new ucMonthView
            {
                Dock = DockStyle.Fill
            };

            _builder = new MonthBuilder();
            _builder.Build(_view, SelectedMonth);
            SetControls(_view);
            _calendarView = CalendarView.Month;
        }

        public void OpenWeekView()
        {
            _view = new ucWeekView
            {
                Dock = DockStyle.Fill
            };

            _view.FilterChanged += OnWeekChanged;

            _builder = new WeekBuilder();
            _builder.Build(_view, SelectedMonth);
            SetControls(_view);
            _calendarView = CalendarView.Week;
        }

        public void OpenDayView()
        {
            var date = SelectedMonth == new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                ? DateTime.Today
                : SelectedMonth;

            _view = new ucDayView
            {
                Dock = DockStyle.Fill,
                SelectedMonth = SelectedMonth
            };

            _view.FilterChanged += OnDayChanged;

            _builder = new DayBuilder();
            _builder.Build(_view, date);
            SetControls(_view);
            _calendarView = CalendarView.Day;
        }

        #endregion

        #region Private methods

        private void OnWeekChanged(object sender, DateTime e)
        {
            _builder.Rebuild(_view, e);
            WeekChanged?.Invoke(sender, e);
        }

        private void OnDayChanged(object sender, DateTime e)
        {
            _builder.Rebuild(_view, e);
            DayChanged?.Invoke(sender, e);
        }

        private void SetControls(ICalendarView view)
        {
            var control = view as Control;
            if (control == null)
            {
                throw new ArgumentException();
            }

            foreach (Control cntrl in this.Controls)
            {
                if (Convert.ToString(cntrl.Tag) == "View")
                {
                    if (cntrl is ucDayView)
                    {
                        ((ICalendarView)cntrl).FilterChanged -= OnDayChanged;
                    }
                    else if (cntrl is ucWeekView)
                    {
                        ((ICalendarView)cntrl).FilterChanged -= OnWeekChanged;
                    }

                    ((ICalendarView)cntrl).ResetContainer();
                    cntrl.Dispose();

                    break;
                }
            }
            this.SuspendLayout();
            this.Controls.Clear();
            this.Controls.Add(control, true);
            this.Controls.Add(topLine);
            this.Controls.Add(pnlFilter);
            this.ResumeLayout();
        }

        private void ShowMonthPickerPopup()
        {
            _popup = new ucMonthPickerPopup();
            _popup.DayClicked += (sender, e) =>
            {
                SelectedMonth = e;
                monthPicker.SetTitle(e);
                _view.SelectedMonth = e;
                Rebuild();
                _container.Close();
                MonthChanged?.Invoke(this, EventArgs.Empty);
            };

            _popup.ChangeDpi(this.DeviceDpi);

            _container = new ucCalendarPopupContainer(_popup)
            {
                Height = LogicalToDeviceUnits(155)
            };

            _container.Show(this.monthPicker);
        }

        private void Rebuild()
        {
            switch (_calendarView)
            {
                case CalendarView.Month:
                    _builder.Rebuild(_view, SelectedMonth);
                    break;
                case CalendarView.Week:
                    _builder.Rebuild(_view, SelectedMonth);
                    break;
                case CalendarView.Day:
                    var selectedDay = SelectedMonth == new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                        ? DateTime.Today
                        : SelectedMonth;
                    _builder.Rebuild(_view, selectedDay);
                    break;
            }
        }

        private void SetRadioButtonsWidth()
        {
            var maxWidth = Math.Max(rbDay.PreferredWidth, rbWeek.PreferredWidth);
            maxWidth = 96 * Math.Max(maxWidth, rbMonth.PreferredWidth) / DeviceDpi;
            rbDay.Width = maxWidth;
            rbWeek.Width = maxWidth;
            rbMonth.Width = maxWidth;
        }

        #endregion

        #region UI events

        private void rbMonth_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as ucViewSelector).Checked)
            {
                ViewChanged?.Invoke(this, CalendarView.Month);
            }
        }

        private void rbWeek_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as ucViewSelector).Checked)
            {
                ViewChanged?.Invoke(this, CalendarView.Week);
            }
        }

        private void rbDay_CheckedChanged(object sender, EventArgs e)
        {
            if ((sender as ucViewSelector).Checked)
            {
                ViewChanged?.Invoke(this, CalendarView.Day);
            }
        }

        private void monthPicker_Click(object sender, System.EventArgs e)
        {
            ShowMonthPickerPopup();
        }

        #endregion
    }
}
