using System;
using System.Collections.Generic;
using System.Linq;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day;
using NUnit.Framework;

namespace Windows.Tests.Dashboard
{
    [TestFixture]
    public class AppointmentsCollectionTest
    {
        [Test]
        public void GetClientByName()
        {
            var appointments = new List<AppointmentRow>
            {
                new AppointmentRow(
                    1,
                    "First Column",
                    new DateTime(2020, 6, 22, 8, 0, 0),
                    new DateTime(2020, 6, 22, 9, 0, 0),
                    "Appointment 8:00-9:00",
                    "London"),
                new AppointmentRow(
                    2,
                    "First Column",
                    new DateTime(2020, 6, 22, 9, 0, 0),
                    new DateTime(2020, 6, 22, 12, 0, 0),
                    "Appointment 9:00-12:00",
                    "London"),
                new AppointmentRow(
                    3,
                    "Second Column",
                    new DateTime(2020, 6, 22, 10, 0, 0),
                    new DateTime(2020, 6, 22, 11, 0, 0),
                    "Appointment 10:00-11:00",
                    "London"),
                new AppointmentRow(
                    4,
                    "Third Column",
                    new DateTime(2020, 6, 22, 10, 30, 0),
                    new DateTime(2020, 6, 22, 13, 0, 0),
                    "Appointment 10:30-13:00",
                    "London"),
                new AppointmentRow(
                    5,
                    "Second Column",
                    new DateTime(2020, 6, 22, 11, 30, 0),
                    new DateTime(2020, 6, 22, 12, 0, 0),
                    "Appointment 11:30-12:00",
                    "London")
            };
           
            var collection = new AppointmentsCollection(appointments);

            Assert.AreEqual(3, collection.Columns.Length);
            Assert.IsTrue(collection.Columns[0].Appointments.Any(app => app.Id == 1));
            Assert.IsTrue(collection.Columns[0].Appointments.Any(app => app.Id == 2));
            Assert.IsTrue(collection.Columns[1].Appointments.Any(app => app.Id == 3));
            Assert.IsTrue(collection.Columns[2].Appointments.Any(app => app.Id == 4));
            Assert.IsTrue(collection.Columns[1].Appointments.Any(app => app.Id == 5));
        }
    }
}
