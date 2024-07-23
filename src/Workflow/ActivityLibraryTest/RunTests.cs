using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class RunTests
    {
        /// <summary>
        /// Run Enquiry Form Test
        /// </summary>
        [TestMethod]
        public void RunEnquiryFormTest()
        {
            InitializeTest.OMSLogin();

            string enquiryCommand = "";
            Dictionary<string, object> replacementParameters = new Dictionary<string, object>();

            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("EnquiryCommand", enquiryCommand);
            parameters.Add("ReplacementParameters", replacementParameters);

            Activity runEnquiryForm = new RunEnquiryCommand();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(runEnquiryForm, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "RunEnquiryForm returned object is Null");
        }

        /// <summary>
        /// Run Menu Script Test
        /// </summary>
        [TestMethod]
        public void RunMenuScriptTest()
        {
            InitializeTest.OMSLogin();

            string scriptCommand = "";

            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("ScriptCommand", scriptCommand);

            Activity runMenuScript = new RunMenuScript();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(runMenuScript, parameters);
            Assert.AreNotEqual(false, outputs["Result"], "Run Menu Script was Unsuccessful");
        }

        /// <summary>
        /// Run Wizard Test
        /// </summary>
        [TestMethod]
        public void RunWizardTest()
        {
            InitializeTest.OMSLogin();

            string wizardCode = "";
            object parentObject = new object();
            Dictionary<string, object> replacementParameters = new Dictionary<string, object>();

            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("WizardCode", wizardCode);
            parameters.Add("ParentObject", parentObject);
            parameters.Add("ReplacementParameters", replacementParameters);

            Activity runWizard = new RunWizard();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(runWizard, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "Run Wizard returned object is Null");
        }
    }
}
