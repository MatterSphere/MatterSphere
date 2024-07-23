using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    public partial class ucTimeHeader : UserControl
    {
        private List<AppointmentRow> _appointments;

        public ucTimeHeader()
        {
            InitializeComponent();

            _appointments = new List<AppointmentRow>();
        }

        public EventHandler<List<AppointmentRow>> Clicked;

        public DateTime Hour { get; private set; }
        public int AppointmentsCount { get; private set; }

        public ucTimeHeader(DateTime time) : this()
        {
            Hour = time;
            lblTime.Text = time.ToString("h tt");
        }

        public void SetDay(DateTime day)
        {
            Hour = new DateTime(day.Year, day.Month, day.Day, Hour.Hour, 0,0);
        }

        public void SetAppointments(List<AppointmentRow> appointments)
        {
            AppointmentsCount = appointments.Count;
            _appointments = appointments;

            lblNumber.Text = AppointmentsCount > 0
                ? AppointmentsCount.ToString()
                : null;
        }

        private void lblNumber_Click(object sender, EventArgs e)
        {
            if (AppointmentsCount > 0)
            {
                Clicked?.Invoke(this, _appointments);
            }
        }
    }
}
