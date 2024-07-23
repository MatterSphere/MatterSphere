using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class ContactActivityTests
    {
        [TestMethod]
        public void CreateContactTest()
        {
            InitializeTest.OMSLogin();

            Activity createContact = new CreateContact();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(createContact);
            Assert.AreNotEqual(null, outputs["Result"], "Contact is Null");
        }

        [TestMethod]
        public void FindContactTest()
        {
            InitializeTest.OMSLogin();

            Activity findContact = new FindContact();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findContact);
            Assert.AreNotEqual(null, outputs["Result"], "Contact is Null");

        }

        [TestMethod]
        public void SelectContactTest()
        {
            InitializeTest.OMSLogin();

            Activity selectContact = new SelectContact();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(selectContact);
            Assert.AreNotEqual(null, outputs["Result"], "Contact is Null");
        }

        [TestMethod]
        public void ShowContactTest()
        {
            InitializeTest.OMSLogin();

            //  Select Contact
            Activity selectContact = new SelectContact();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(selectContact);
            FWBS.OMS.Contact contact = (FWBS.OMS.Contact)outputs["Result"];
            Assert.AreNotEqual(null, contact, "Contact is Null");

            //  Show Contact            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("Contact", contact);

            Activity showContact = new ShowContact();
            outputs = WorkflowInvoker.Invoke(showContact, parameters);
        }
    }
}
