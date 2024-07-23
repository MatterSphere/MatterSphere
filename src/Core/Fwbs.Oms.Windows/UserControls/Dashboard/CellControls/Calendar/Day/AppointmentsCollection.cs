using System.Collections.Generic;
using System.Linq;
using FWBS.OMS.Dashboard;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    public class AppointmentsCollection
    {
        public AppointmentsCollection(List<AppointmentRow> appointments)
        {
            var apps = appointments.Where(app => (app.End - app.Start).TotalMinutes >= 15).OrderBy(app => app.Start).ToList();
            var columns = new List<Column>
            {
                new Column(0)
            };

            var appointmentsList = new List<AppointmentRow>();
            foreach (var appItem in apps)
            {
                var appointment = appItem;
                appointmentsList.Add(appointment);
                var column = columns.FirstOrDefault(col =>
                    !col.Appointments.Any() || col.Appointments.All(app => app.End <= appItem.Start));
                if (column != null)
                {
                    column.AddAppointment(appointment);
                }
                else
                {
                    var newColumn = new Column(columns.Count);
                    newColumn.AddAppointment(appointment);
                    columns.Add(newColumn);
                }
            }

            Columns = columns.ToArray();
            Appointments = appointmentsList.ToArray();
        }

        public Column[] Columns { get; private set; }
        public AppointmentRow[] Appointments { get; private set; }

        #region Classes

        public class Column
        {
            public Column(int number)
            {
                Number = number;
                Appointments = new List<AppointmentRow>();
            }

            public int Number { get; private set; }
            public List<AppointmentRow> Appointments { get; private set; }

            public void AddAppointment(AppointmentRow appointment)
            {
                Appointments.Add(appointment);
            }
        }

        #endregion
    }
}
