using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class ClientActivityTests
    {
        /// <summary>
        /// Create Client Test
        /// </summary>
        [TestMethod]
        public void CreateClientTest()
        {
            InitializeTest.OMSLogin();

            Activity createClient = new CreateClient();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(createClient);
            Assert.AreNotEqual(null, outputs["Result"], "Client is Null");
        }

        /// <summary>
        /// Create Pre-Client Test
        /// </summary>
        [TestMethod]
        public void CreatePreClientTest()
        {
            InitializeTest.OMSLogin();

            Activity createPreClient = new CreatePreClient();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(createPreClient);
            Assert.AreNotEqual(null, outputs["Result"], "Pre Client is Null");
        }

        /// <summary>
        /// Find Client Test
        /// </summary>
        [TestMethod]
        public void FindClientTest()
        {
            InitializeTest.OMSLogin();

            Activity findClient = new FindClient();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findClient);
            Assert.AreNotEqual(null, outputs["Result"], "Client is Null");
        }

        /// <summary>
        /// Select Client Test
        /// </summary>
        [TestMethod]
        public void SelectClientTest()
        {
            InitializeTest.OMSLogin();

            Activity selectClient = new SelectClient();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(selectClient);
            Assert.AreNotEqual(null, outputs["Result"], "Client is Null");
        }

        /// <summary>
        /// Show Client Test
        /// </summary>
        [TestMethod]
        public void ShowClientTest()
        {
            InitializeTest.OMSLogin();

            //  Find Client
            Activity findClient = new FindClient();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findClient);
            FWBS.OMS.Client client = (FWBS.OMS.Client)outputs["Result"];
            Assert.AreNotEqual(null, client, "Client is Null");

            //  Show Client            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("Client", client);

            Activity showClient = new ShowClient();
            outputs = WorkflowInvoker.Invoke(showClient, parameters);
        }

        /// <summary>
        /// Select Client Return Client No Test
        /// </summary>
        [TestMethod]
        public void SelectClientReturnClientNoTest()
        {
            InitializeTest.OMSLogin();

            Activity selectClientReturnClientNo = new SelectClientReturnClientNo();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(selectClientReturnClientNo);
            Assert.AreNotEqual(string.Empty, outputs["Result"], "Select Client Return No is Empty");
        }
    }
}
