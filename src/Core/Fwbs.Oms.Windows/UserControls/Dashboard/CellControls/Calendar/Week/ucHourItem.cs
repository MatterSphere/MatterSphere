using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    internal partial class ucHourItem : UserControl
    {
        private readonly Color _selectColor = Color.FromArgb(234, 234, 234);

        public ucHourItem()
        {
            InitializeComponent();
        }

        public ucHourItem(DateTime day, int hour) : this()
        {
            Day = day;
            Hour = day.AddHours(hour);
            Appointments = new List<AppointmentRow>();
            this.Click += Item_Click;
        }

        public EventHandler<List<AppointmentRow>> Clicked;

        public bool IsUsed { get; private set; }
        public DateTime Hour { get; set; }
        public DateTime Day { get; set; }
        public AppointmentStatus Status { get; set; }
        public int AppointmentsCount
        {
            get { return Appointments.Count; }
        }

        public List<AppointmentRow> Appointments { get; set; }

        public void SetAppointments(List<AppointmentRow> appointments, ucHourItem prevCell)
        {
            if (appointments.Count == 1)
            {
                AddAppointment(appointments[0], prevCell);
            }
            else
            {
                AddAppointments(appointments);
            }
        }

        public void ResetContent()
        {
            this.Controls.Clear();
            Appointments.Clear();
        }

        public void Unselect()
        {
            this.BackColor = Color.White;
        }

        #region Private methods

        private void AddAppointments(List<AppointmentRow> appointments)
        {
            Status = AppointmentStatus.Multi;
            Appointments.AddRange(appointments);

            var details = new ucAppointmentCellDetails
            {
                Dock = DockStyle.Fill
            };

            details.SetAppointments(appointments.Count);
            Subscribe(details, Item_Click);
            this.Controls.Add(details);
        }

        private void Subscribe(Control control, EventHandler handler)
        {
            var controls = GetAll(control);
            controls.Add(control);
            foreach (var cntrl in controls)
            {
                cntrl.Click += handler;
            }
        }

        private List<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>().ToList();
            return controls.SelectMany(GetAll).Concat(controls).ToList();
        }

        private void AddAppointment(AppointmentRow appointment, ucHourItem prevCell)
        {
            AppointmentStatus status;

            if (appointment.Start < Hour)
            {
                status = appointment.End <= Hour.AddHours(1)
                    ? Hour.Hour == 0 || prevCell.Status == AppointmentStatus.Multi
                      ? AppointmentStatus.Full
                      : AppointmentStatus.End
                    : Hour.Hour == 0 || prevCell.Status == AppointmentStatus.Multi
                      ? AppointmentStatus.Start
                      : AppointmentStatus.Flow;
            }
            else
            {
                status = appointment.End <= Hour.AddHours(1)
                    ? AppointmentStatus.Full
                    : AppointmentStatus.Start;
            }

            SetAppointment(appointment, status);
        }

        private void SetAppointment(AppointmentRow appointment, AppointmentStatus status)
        {
            Status = status;
            Appointments.Add(appointment);

            var details = new ucAppointmentCellDetails
            {
                Dock = DockStyle.Fill
            };

            switch (status)
            {
                case AppointmentStatus.Start:
                    details.SetHeader(appointment.AppointmentType, appointment.AllDay || appointment.IsAllDay(Day), appointment.GetStartTime(Day), appointment.GetEndTime(Day));
                    this.Padding = new Padding(1,2,1,0);
                    break;
                case AppointmentStatus.Flow:
                    details.RemoveHeader();
                    this.Padding = new Padding(1, 0, 1, 0);
                    break;
                case AppointmentStatus.End:
                    details.RemoveHeader();
                    this.Padding = new Padding(1, 0, 1, 2);
                    break;
                case AppointmentStatus.Full:
                    details.SetHeader(appointment.AppointmentType, appointment.AllDay || appointment.IsAllDay(Day), appointment.GetStartTime(Day), appointment.GetEndTime(Day));
                    break;
            }

            Subscribe(details, Item_Click);
            this.Controls.Add(details);
        }

        private void SelectCell()
        {
            this.BackColor = _selectColor;
        }

        #endregion

        #region UI events

        private void Item_Click(object sender, EventArgs e)
        {
            if (AppointmentsCount > 0)
            {
                this.Focus();
                SelectCell();
                Clicked?.Invoke(this, Appointments);
            }
        }

        #endregion

        #region UI methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            DrawBorder(e);
        }

        private void DrawBorder(PaintEventArgs e)
        {
            var lineThickness = LogicalToDeviceUnits(1);
            using (var pen = new Pen(Color.FromArgb(244, 244, 244), lineThickness))
            {
                e.Graphics.DrawLine(pen, Width, Height - lineThickness, 0, Height - lineThickness);
                e.Graphics.DrawLine(pen, 0, Height - lineThickness, 0, 0);
            }
        }

        #endregion

        public enum AppointmentStatus
        {
            Start,
            Flow,
            End,
            Full,
            Multi
        }
    }
}
