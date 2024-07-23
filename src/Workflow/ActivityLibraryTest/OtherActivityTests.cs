using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class OtherActivityTests
    {
        /// <summary>
        /// Create Key Date Test
        /// </summary>
        [TestMethod]
        public void CreateKeyDateTest()
        {
            InitializeTest.OMSLogin();

            Activity createKeyDate = new CreateKeyDate();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(createKeyDate);
            Assert.AreNotEqual(false, outputs["Result"], "Create Key Date was Unsuccessful");
        }

        /// <summary>
        /// Create Key Date Test
        /// </summary>
        [TestMethod]
        public void CreateKeyDateWithFileTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Create Key Date            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity createKeyDate = new CreateKeyDate();
            outputs = WorkflowInvoker.Invoke(createKeyDate, parameters);
            Assert.AreNotEqual(false, outputs["Result"], "Create Key Date was Unsuccessful");
        }

        /// <summary>
        /// Message Box Test
        /// </summary>
        [TestMethod]
        public void MessageBoxTest()
        {
            InitializeTest.OMSLogin();

            string message = "Hello Test";
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("Message", message);

            Activity messageBox = new MessageBox();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(messageBox, parameters);
        }

        /// <summary>
        /// Open Report Test
        /// </summary>
        [TestMethod]
        public void OpenReportTest()
        {
            InitializeTest.OMSLogin();

            //  Parent (File)
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Create Associate            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("ReportCode", file.ID.ToString());
            parameters.Add("Parent", file);
            parameters.Add("Parameters", null);
            parameters.Add("RunNow", false);

            Activity openReport = new OpenReport();
            outputs = WorkflowInvoker.Invoke(openReport, parameters);
        }

        /// <summary>
        /// Send Email Test
        /// </summary>
        [TestMethod]
        public void SendEmailTest()
        {
            InitializeTest.OMSLogin();

            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("From", "test1@test.fwbs.net");
            parameters.Add("To", "test3@test.fwbs.net");
            parameters.Add("Subject", "Hello");
            parameters.Add("Body", "Hello Test3");

            Activity sendEmail = new SendEMail();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(sendEmail, parameters);
        }
    }
}