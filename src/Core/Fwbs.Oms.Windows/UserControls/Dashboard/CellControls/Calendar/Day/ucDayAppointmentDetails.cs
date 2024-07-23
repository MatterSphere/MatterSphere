using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    internal partial class ucDayAppointmentDetails : UserControl
    {
        private string _link;
        private const string _marker = "";

        private AppointmentRow _appointment;

        public ucDayAppointmentDetails()
        {
            InitializeComponent();
        }

        public ucDayAppointmentDetails(DateTime hour) : this()
        {
            var controls = GetAll(this);
            foreach (Control control in controls)
            {
                control.Click += ItemClicked;
            }

            lblMarker.Text = _marker;
            Hour = hour;
        }

        public EventHandler<AppointmentRow> Clicked;

        public DateTime Hour { get; set; }

        public void SetDetails(AppointmentRow appointment, DateTime date, bool showHeader)
        {
            _appointment = appointment;

            if (showHeader)
            {
                SetAppointment(appointment, date);
            }
            else
            {
                pnlContainer.Controls.Remove(tlpContainer);
            }
        }

        public void SetFromStart()
        {
            pnlMarkerContainer.Padding = new Padding(0);
        }

        #region Private methods

        private IEnumerable<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>().ToList();
            return controls.SelectMany(ctrl => GetAll(ctrl)).Where(ctrl => ctrl.Name != "lblJoin").Concat(controls);
        }

        private void SetAppointment(AppointmentRow appointment, DateTime date)
        {
            lblTitle.Text = appointment.AppointmentType;
            lblTime.Text = GetInterval(appointment.Start, appointment.End, appointment.AllDay || appointment.IsAllDay(date), date);

            Uri uri;
            bool uriValidationResult = Uri.TryCreate(appointment.Location, UriKind.Absolute, out uri)
                                       && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);

            if (uriValidationResult)
            {
                lblJoin.Visible = true;
                tlpContainer.ColumnStyles[2].Width = LogicalToDeviceUnits(60);
                lblLocation.Text = uri.Host;
                _link = uri.OriginalString;
            }
            else
            {
                lblJoin.Visible = false;
                tlpContainer.ColumnStyles[2].Width = 0;
                lblLocation.Text = appointment.Location;
            }
        }

        private string GetInterval(DateTime start, DateTime end, bool allDay, DateTime date)
        {
            return allDay
                ? CodeLookup.GetLookup("DASHBOARD", "ALLDAY", "All day")
                : $"{GetTime(start, date)} - {GetTime(end, date)}";
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

        private void ItemClicked(object sender, EventArgs e)
        {
            Clicked?.Invoke(this, _appointment);
        }

        #endregion
    }
}
