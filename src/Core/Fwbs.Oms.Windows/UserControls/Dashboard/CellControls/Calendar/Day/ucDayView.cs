using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common;
using FWBS.OMS.UI.Windows;
using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    internal partial class ucDayView : UserControl, ICalendarView
    {
        private ucCalendarPickerPopup _popup;
        private ucCalendarPopupContainer _container;
        private List<ucTimeHeader> _timeHeaders;
        private UltraPeekPopup _detailsPopup;
        private AppointmentsCollection _appointmentsCollection;
        private DayAppointmentsDataSource _appointmentsTable;

        public ucDayView()
        {
            InitializeComponent();

            _timeHeaders = new List<ucTimeHeader>();
            SetTimeHeaders();
            this.tlpDays.MouseWheel += OnMouseWheel;
        }
        
        #region ICalendarView

        public event EventHandler<DateTime> FilterChanged;

        public DateTime SelectedMonth { get; set; }

        public void InsertHeader(ucCalendarDayHeader header, int position)
        {
            throw new NotImplementedException();
        }

        public void ResetContainer()
        {
            while (tlpDays.Controls.Count > 0)
            {
                var control = tlpDays.Controls[0];
                if (control is ucDayAppointmentItem)
                {
                    ((ucDayAppointmentItem)control).Clicked -= AppointmentClicked;
                }
                else if (control is ucTimeHeader)
                {
                    ((ucTimeHeader)control).Clicked -= TimeHeaderClicked;
                }

                tlpDays.Controls.Remove(control);
                control.Dispose();
            }
        }

        public void SetAppointments(List<AppointmentRow> appointments)
        {
            SetAppointmentsInTimeHeaders(appointments);
            SetAppointmentsInMainContainer(appointments);
        }

        public DateTime[] GetInterval()
        {
            return new DateTime[]
            {
                Day.Date,
                Day.Date.AddDays(1)
            };
        }

        #endregion

        public DateTime Day { get; private set; }

        private DateTime _selectedDay;
        public DateTime SelectedDay
        {
            get { return _selectedDay; }
            private set
            {
                _selectedDay = value;
                SetDay(_selectedDay);
            }
        }

        #region Methods

        public void SetDay(DateTime day)
        {
            Day = day;
            dayPicker.SetTitle(day);
        }

        public void UpdateHeaders(DateTime day)
        {
            foreach (var timeHeader in _timeHeaders)
            {
                timeHeader.SetDay(day);
            }
        }

        #endregion

        #region UI methods

        private void MoveToCurrentTime()
        {
            tlpDays.AutoScroll = false;
            var height = LogicalToDeviceUnits(Convert.ToInt32(tlpDays.RowStyles[1].Height));
            tlpDays.VerticalScroll.Maximum = height * 24;
            tlpDays.VerticalScroll.Value = height * DateTime.Now.Hour;
            tlpDays.AutoScroll = true;
        }

        #endregion

        #region Private methods

        private void SetAppointmentsInMainContainer(List<AppointmentRow> appointments)
        {
            tlpDays.SuspendLayout();

            _appointmentsCollection = new AppointmentsCollection(appointments);

            UpdateTable();

            _appointmentsTable = new DayAppointmentsDataSource(_appointmentsCollection, Day, AppointmentClicked);
            
            for (int column = 0; column < _appointmentsTable.Columns.Length; column++)
            {
                for (int row = 0; row < 24; row++)
                {
                    tlpDays.Controls.Add(_appointmentsTable.Columns[column].DayAppointmentItems[row], column + 1, row);
                }
            }

            MoveToCurrentTime();
            tlpDays.ResumeLayout();
        }

        private void UpdateTable()
        {
            var items = tlpDays.Controls.Where<Control>(ctrl => ctrl is ucDayAppointmentItem).ToList();
            foreach (var item in items)
            {
                tlpDays.Controls.Remove(item);
            }

            for (int i = tlpDays.ColumnStyles.Count - 1; i > 0; i--)
            {
                tlpDays.ColumnStyles.Remove(tlpDays.ColumnStyles[i]);
            }

            var percent = 100F / _appointmentsCollection.Columns.Length;
            tlpDays.ColumnCount = _appointmentsCollection.Columns.Length + 1;
            for (int i = 0; i < _appointmentsCollection.Columns.Length; i++)
            {
                tlpDays.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, percent));
            }
        }

        private void SetAppointmentsInTimeHeaders(List<AppointmentRow> appointments)
        {
            for (int i = 0; i < 24; i++)
            {
                var appList = appointments.Where(app =>
                    (app.Start < _timeHeaders[i].Hour.AddHours(1) && app.End > _timeHeaders[i].Hour) ||
                    (app.AllDay && app.Start.Date == _timeHeaders[i].Hour.Date) ||
                    (app.Start == _timeHeaders[i].Hour && app.End == _timeHeaders[i].Hour)).ToList();
                _timeHeaders[i].SetAppointments(appList);
            }
        }

        private void ShowDayPickerPopup()
        {
            _popup = SelectedMonth == new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)
                ? new ucDayPickerPopup()
                : new ucDayPickerPopup(Day);
            _popup.DayClicked += (sender, e) =>
            {
                SelectedDay = e;
                dayPicker.SetTitle(e);
                _container.Close();
                FilterChanged?.Invoke(this, e);
            };

            _popup.ChangeDpi(this.DeviceDpi);
            _container = new ucCalendarPopupContainer(_popup)
            {
                Height = LogicalToDeviceUnits(155)
            };

            _container.Show(this.dayPicker);
        }

        private void SetTimeHeaders()
        {
            var time = DateTime.Today;
            for (int i = 0; i < 24; i++)
            {
                var header = new ucTimeHeader(time.AddHours(i))
                {
                    Dock = DockStyle.Fill
                };

                tlpDays.Controls.Add(header, 0, i);
                header.Clicked += TimeHeaderClicked;
                _timeHeaders.Add(header);
            }
        }

        private void ShowAppointmentsDetailsPopup(ucTimeHeader timeHeader, List<AppointmentRow> appointments)
        {
            _detailsPopup = new UltraPeekPopup
            {
                ContentMargin = new Padding(1),
                Appearance = new Infragistics.Win.Appearance
                {
                    BorderColor = Color.FromArgb(234, 234, 234)
                }
            };

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
                item.SetAppointment(app, timeHeader);
                container.Controls.Add(item, true);
            }

            _detailsPopup.Content = container;

            var location = timeHeader.PointToScreen(Point.Empty);
            _detailsPopup.Show(new Point(location.X + timeHeader.Size.Width / 2, location.Y + timeHeader.Size.Height / 2), Infragistics.Win.Peek.PeekLocation.RightOfItem);
        }

        private void ShowAppointmentPopup(ucDayAppointmentDetails appointmentDetails, AppointmentRow appointment)
        {
            _detailsPopup = new UltraPeekPopup
            {
                ContentMargin = new Padding(1),
                Appearance = new Infragistics.Win.Appearance
                {
                    BorderColor = Color.FromArgb(234, 234, 234)
                }
            };

            var container = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };

            var item = new ucAppointmentDetailsItem
            {
                Dock = DockStyle.Top
            };
            item.SetAppointment(appointment, appointmentDetails);
            container.Controls.Add(item, true);

            _detailsPopup.Content = container;

            var location = appointmentDetails.PointToScreen(Point.Empty);
            _detailsPopup.Show(new Point(location.X + 5, location.Y + appointmentDetails.Size.Height / 2), Infragistics.Win.Peek.PeekLocation.RightOfItem);
        }

        #endregion

        #region UI events

        private void dayPicker_Click(object sender, EventArgs e)
        {
            ShowDayPickerPopup();
        }

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (_detailsPopup != null && _detailsPopup.Visible)
            {
                _detailsPopup.Close();
            }
        }

        #endregion

        #region Handlers

        private void TimeHeaderClicked(object sender, List<AppointmentRow> e)
        {
            var item = sender as ucTimeHeader;
            if (item != null)
            {
                item.Focus();
                ShowAppointmentsDetailsPopup(sender as ucTimeHeader, e);
            }
        }

        private void AppointmentClicked(object sender, AppointmentRow e)
        {
            var item = sender as ucDayAppointmentDetails;
            if(item != null)
            {
                item.Focus();
                ShowAppointmentPopup(item, e);
            }
        }

        #endregion
    }
}
