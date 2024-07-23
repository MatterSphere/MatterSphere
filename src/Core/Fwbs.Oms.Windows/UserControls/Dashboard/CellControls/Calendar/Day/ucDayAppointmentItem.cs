using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    internal partial class ucDayAppointmentItem : UserControl
    {
        public ucDayAppointmentItem()
        {
            InitializeComponent();
        }

        public ucDayAppointmentItem(DateTime hour) : this()
        {
            Hour = hour;
        }

        public EventHandler<AppointmentRow> Clicked;

        public DateTime Hour { get; set; }

        public void SetAppointments(List<AppointmentRow> appointments)
        {
            var time = Hour;
            var controls = new List<Control>();

            foreach (var appointment in appointments)
            {
                if (appointment.Start > time)
                {
                    var spaceTimeFrame = (appointment.Start - time).TotalMinutes;
                    var panel = new Panel
                    {
                        Dock = DockStyle.Top,
                        Height = Convert.ToInt32(this.Height / 60.0 * spaceTimeFrame)
                    };

                    controls.Add(panel);
                    time = time.AddMinutes(spaceTimeFrame);
                }

                var appointmentControl = new ucDayAppointmentDetails(Hour)
                {
                    Dock = DockStyle.Top
                };

                appointmentControl.Clicked += ItemClicked;

                if (appointment.Start < Hour)
                {
                    appointmentControl.SetFromStart();
                }

                appointmentControl.SetDetails(
                    appointment,
                    Hour,
                    Hour.Hour == 0 || appointment.Start >= Hour);

                var startPoint = Hour < appointment.Start
                    ? appointment.Start
                    : Hour;
                var endPoint = Hour.AddHours(1) < appointment.End
                    ? Hour.AddHours(1)
                    : appointment.End;

                var timeFrame = (endPoint - startPoint).TotalMinutes;
                var height = Convert.ToInt32(this.Height / 60.0 * timeFrame);
                appointmentControl.Height = height;
                controls.Add(appointmentControl);
                time = time.AddMinutes(timeFrame);
            }

            for (int i = controls.Count - 1; i >= 0; i--)
            {
                this.Controls.Add(controls[i], true);
            }
        }

        private void ItemClicked(object sender, AppointmentRow e)
        {
            Clicked?.Invoke(sender, e);
        }

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
                e.Graphics.DrawLine(pen, 0, 0, Width, 0);
            }
        }

        #endregion
    }
}
