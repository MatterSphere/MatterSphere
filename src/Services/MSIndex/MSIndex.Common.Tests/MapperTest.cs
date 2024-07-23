using System;
using System.Dynamic;
using MSIndex.Common.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace MSIndex.Common.Tests
{
    [TestFixture]
    public class MapperTest
    {
        [TestCase(1, 'I', 2, 3, null, null, "Default Type", "Description 1", "Location")]
        [TestCase(1, 'I', 2, 3, null, null, "Default Type", "Description 2", null)]
        public void MapToAppointment(long matterSphereId, char operation, long fileId, long clientId,
            long? associateId, long? documentId, string appointmentType, string description, string location)
        {
            var date = DateTime.Now;

            var source = new ExpandoObject() as IDictionary<string, Object>;
            source.Add("mattersphereid", matterSphereId);
            source.Add("modifieddate", date);
            source.Add("op", operation);
            source.Add("file-id", fileId);
            source.Add("client-id", clientId);
            source.Add("appointmentType", appointmentType);
            source.Add("appDesc", description);
            source.Add("appLocation", location);

            var appointment = new MSAppointment();
            var mapper = new Mapper();
            mapper.Map((dynamic)source, appointment);
            
            Assert.AreEqual(matterSphereId, appointment.MatterSphereId);
            Assert.AreEqual(date, appointment.ModifiedDate);
            Assert.AreEqual(operation, appointment.Operation);
            Assert.AreEqual(fileId, appointment.FileId);
            Assert.AreEqual(clientId, appointment.ClientId);
            Assert.AreEqual(associateId, appointment.AssociateId);
            Assert.AreEqual(documentId, appointment.DocumentId);
            Assert.AreEqual(appointmentType, appointment.AppointmentType);
            Assert.AreEqual(description, appointment.Description);
            Assert.AreEqual(location, appointment.Location);
        }
    }
}
