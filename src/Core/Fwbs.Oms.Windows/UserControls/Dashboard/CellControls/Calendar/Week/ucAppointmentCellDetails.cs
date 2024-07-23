using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    internal partial class ucAppointmentCellDetails : UserControl
    {
        public ucAppointmentCellDetails()
        {
            InitializeComponent();
        }

        public void SetHeader(string title, bool allDay, DateTime start, DateTime end)
        {
            lblTitle.Text = title;
            lblTime.Text = allDay
                ? CodeLookup.GetLookup("DASHBOARD", "ALLDAY", "All day")
                : $"{GetStartTimeFrame(start)} - {GetEndTimeFrame(end)}";
        }

        public void RemoveHeader()
        {
            this.Controls.Remove(lblTitle);
            this.Controls.Remove(lblTime);
        }
        
        public void SetAppointments(int count)
        {
            this.Controls.Remove(lblTitle);
            this.Controls.Remove(lblTime);

            var label = new ucCalendarMultiItem(count)
            {
                Dock = DockStyle.Fill
            };

            this.Controls.Add(label);
            leftMarker.SendToBack();
        }

        private string GetStartTimeFrame(DateTime time)
        {
            return time.Minute == 0
                ? time.ToString("%h")
                : time.ToString("h:mm");
        }

        private string GetEndTimeFrame(DateTime time)
        {
            return time.Minute == 0
                ? time.ToString("%h tt")
                : time.ToString("h:mm tt");
        }
    }
}
