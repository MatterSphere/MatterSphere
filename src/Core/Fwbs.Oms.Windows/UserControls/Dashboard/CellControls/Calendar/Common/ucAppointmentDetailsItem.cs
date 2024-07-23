using System;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Month;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common
{
    internal partial class ucAppointmentDetailsItem : UserControl
    {
        private string _link;
        private string _marker = "";

        public ucAppointmentDetailsItem()
        {
            InitializeComponent();
            lblMarker.Text = _marker;
        }

        #region Methods

        public void SetAppointment(AppointmentRow appointment, ucCalendarDay calendarDay)
        {
            SetAppointment(appointment, calendarDay.Date);
        }

        public void SetAppointment(AppointmentRow appointment, ucHourItem hourItem)
        {
            SetAppointment(appointment, hourItem.Day);
        }

        public void SetAppointment(AppointmentRow appointment, ucTimeHeader timeHeader)
        {
            SetAppointment(appointment, timeHeader.Hour);
        }

        public void SetAppointment(AppointmentRow appointment, ucDayAppointmentDetails appointmentDetails)
        {
            SetAppointment(appointment, appointmentDetails.Hour);
        }

        #endregion

        #region Private methods

        private void SetAppointment(AppointmentRow appointment, DateTime day)
        {
            lblTitle.Text = appointment.AppointmentType;
            lblTime.Text = GetInterval(appointment, day);

            Uri uri;
            bool uriValidationResult = Uri.TryCreate(appointment.Location, UriKind.Absolute, out uri)
                                       && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            if (uriValidationResult)
            {
                lblJoin.Visible = true;
                lblLocation.Text = uri.Host;
                _link = uri.OriginalString;
            }
            else
            {
                lblJoin.Visible = false;
                lblLocation.Text = appointment.Location;
            }
        }

        private string GetInterval(AppointmentRow appointment, DateTime day)
        {
            return appointment.AllDay || appointment.IsAllDay(day)
                ? CodeLookup.GetLookup("DASHBOARD", "ALLDAY", "All day")
                : $"{GetTime(appointment.Start, day)} - {GetTime(appointment.End, day)}";
        }

        private string GetTime(DateTime appDate, DateTime calDate)
        {
            if (appDate.Date < calDate.Date)
            {
                return calDate.Date.ToString("hh:mm tt");
            }

            if (appDate.Date > calDate.Date)
            {
                return calDate.Date.ToString("hh:mm tt");
            }

            return appDate.ToString("hh:mm tt");
        }

        private void lblJoin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(_link);
        }

        #endregion
    }
}
