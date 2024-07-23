using System;
using System.Collections.Generic;
using FWBS.OMS.Dashboard;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common
{
    internal interface ICalendarView
    {
        event EventHandler<DateTime> FilterChanged;
        DateTime SelectedMonth { get; set; }
        void InsertHeader(ucCalendarDayHeader header, int position);
        void ResetContainer();
        void SetAppointments(List<AppointmentRow> appointments);
        DateTime[] GetInterval();
    }
}
