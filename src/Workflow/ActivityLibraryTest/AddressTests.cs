using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ActivityLibraryTest
{
    [TestClass]
    public class AddressTests
    {
        /// <summary>

        /// Find Address Test
        /// </summary>
        [TestMethod]
        public void FindAddressTest()
        {
            InitializeTest.OMSLogin();

            Activity findAddress = new FindAddress();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findAddress);
            Assert.AreNotEqual(null, outputs["Result"], "Address is Null");
        }
    }
}
