using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class AppointmentActivityTests
    {
        /// <summary>
        /// Create Appointment Test
        /// </summary>
        [TestMethod]
        public void CreateAppointmentTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Create Appointment            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity createAppointment = new CreateAppointment();
            outputs = WorkflowInvoker.Invoke(createAppointment, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "Appointment is Null");
        }

        /// <summary>
        /// Pick Appointment Test
        /// </summary>
        [TestMethod]
        public void PickAppointmentTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Pick Appointment            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity pickAppointment = new PickAppointment();
            outputs = WorkflowInvoker.Invoke(pickAppointment, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "Appointment is Null");
        }

        /// <summary>
        /// Save Appointment Test
        /// </summary>
        [TestMethod]
        public void SaveAppointmentTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Pick Appointment            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity pickAppointment = new PickAppointment();
            outputs = WorkflowInvoker.Invoke(pickAppointment, parameters);
            FWBS.OMS.Appointment appointment = (FWBS.OMS.Appointment)outputs["Result"];
            Assert.AreNotEqual(null, outputs["Result"], "Appointment is Null");

            parameters.Clear();
            parameters.Add("Appointment", appointment);

            //  Save Appointment
            Activity saveAppointment = new SaveAppointment();
            outputs = WorkflowInvoker.Invoke(pickAppointment, parameters);
            Assert.AreNotEqual(false, outputs["Result"], "Appointment has not been Saved");
        }
    }
}
