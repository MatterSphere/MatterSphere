using System;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    internal class DayAppointmentsDataSource
    {
        public DayAppointmentsDataSource(AppointmentsCollection appointments, DateTime day, EventHandler<AppointmentRow> clickHandler)
        {
            Columns = new Column[appointments.Columns.Length];
            for (int i = 0; i < appointments.Columns.Length; i++)
            {
                Columns[i] = new Column(appointments.Columns[i], day, clickHandler);
            }
        }

        public Column[] Columns { get; private set; }

        public class Column
        {
            public Column(AppointmentsCollection.Column column, DateTime day, EventHandler<AppointmentRow> clickHandler)
            {
                Number = column.Number;
                DayAppointmentItems = new ucDayAppointmentItem[24];
                for (int i = 0; i < 24; i++)
                {
                    DayAppointmentItems[i] = new ucDayAppointmentItem(day.AddHours(i))
                    {
                        Dock = DockStyle.Fill
                    };

                    var apps = column.Appointments.Where(app =>
                                        app.Start < day.AddHours(i).AddHours(1) && app.End > day.AddHours(i)).ToList();
                    if (apps.Any())
                    {
                        DayAppointmentItems[i].SetAppointments(apps);
                        DayAppointmentItems[i].Clicked += clickHandler;
                    }
                }
            }

            public int Number { get; private set; }
            public ucDayAppointmentItem[] DayAppointmentItems { get; private set; }
         }
    }
}
