using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Month
{
    internal partial class ucCalendarDay : UserControl
    {
        private readonly Color _defaultForeColor = Color.FromArgb(51, 51, 51);
        private readonly Color _todayColor = Color.FromArgb(0, 120, 212);
        private readonly Color _notSelectedMonthColor = Color.FromArgb(244, 244, 244);
        private readonly Color _borderColor = Color.FromArgb(244, 244, 244);
        private readonly Color _selectColor = Color.FromArgb(234, 234, 234);
        private bool _isSelectedMonth;
        private bool _isToday;
        private bool _isSelected;
        private List<AppointmentRow> _appointments;
        private bool _isHeader;

        public ucCalendarDay()
        {
            InitializeComponent();

            _appointments = new List<AppointmentRow>();
        }

        public ucCalendarDay(bool isHeader) : this()
        {
            _isHeader = isHeader;
        }

        public EventHandler<List<AppointmentRow>> DayClicked;

        public DateTime Date { get; private set; }

        public bool HasAppointments
        {
            get { return _appointments.Count > 0; }
        }

        #region Methods

        public void SetDay(DateTime date, bool selectedMonth = true)
        {
            _appointments.Clear();
            lbDay.Text = date.Day.ToString();
            Date = date.Date;
            _isToday = Date == DateTime.Today;

            this.BackColor = Color.White;
            this.lbDay.ForeColor = _defaultForeColor;

            if (selectedMonth)
            {
                _isSelectedMonth = true;

                if (_isToday)
                {
                    this.BackColor = _todayColor;
                    this.lbDay.ForeColor = Color.White;
                }
            }
            else
            {
                this.BackColor = _notSelectedMonthColor;
            }
        }

        public void SetDay(int day, int month, int year, bool selectedMonth=true)
        {
            var date = new DateTime(year, month, day);
            SetDay(date, selectedMonth);
        }

        public void SetAppointments(List<AppointmentRow> appointments)
        {
            _appointments = appointments;
        }

        public void Unselect()
        {
            _isSelected = false;
            if (!_isToday)
            {
                this.BackColor = Color.White;
            }
        }

        #endregion

        #region UI methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (HasAppointments)
            {
                DrawMarker(e);
            }

            DrawBorder(e);
        }

        private void DrawMarker(PaintEventArgs e)
        {
            var color = _isToday ? Color.White : _todayColor;

            using (var blueBrush = new SolidBrush(color))
            {
                e.Graphics.FillRectangle(blueBrush, LogicalToDeviceUnits(2), LogicalToDeviceUnits(2), LogicalToDeviceUnits(5), LogicalToDeviceUnits(5));
            }
        }

        private void DrawBorder(PaintEventArgs e)
        {
            var lineThickness = LogicalToDeviceUnits(1);
            using (var pen = new Pen(_borderColor, lineThickness))
            {
                e.Graphics.DrawLine(pen, 0, 0, Width, 0);
                e.Graphics.DrawLine(pen, Width, 0, Width, Height);
                e.Graphics.DrawLine(pen, Width, Height, 0, Height);
                e.Graphics.DrawLine(pen, 0, Height, 0, 0);
            }
        }

        #endregion

        #region UI events

        private void lbDay_MouseHover(object sender, EventArgs e)
        {
            if (!_isHeader && !_isToday && _isSelectedMonth)
            {
                this.BackColor = _selectColor;
            }
        }

        private void lbDay_MouseLeave(object sender, EventArgs e)
        {
            if (!_isHeader && _isSelectedMonth && !_isToday && !_isSelected)
            {
                this.BackColor = Color.White;
            }
        }

        private void lbDay_Click(object sender, EventArgs e)
        {
            if (!_isHeader && _isSelectedMonth && HasAppointments)
            {
                SelectDay();
                DayClicked?.Invoke(this, _appointments);
            }
        }

        #endregion

        #region Private methods

        private void SelectDay()
        {
            _isSelected = true;
            if (!_isToday)
            {
                this.BackColor = _selectColor;
            }
        }

        #endregion
    }
}
