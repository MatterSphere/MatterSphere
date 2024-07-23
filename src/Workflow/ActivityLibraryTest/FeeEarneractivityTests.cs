using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class FeeEarneractivityTests
    {
        /// <summary>
        /// Create FeeEarner Test
        /// </summary>
        [TestMethod]
        public void CreateFeeEarnerTest()
        {
            InitializeTest.OMSLogin();

            //  Create FeeEarner
            Activity createFeeEarner = new CreateFeeEarner();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(createFeeEarner);
            Assert.AreNotEqual(null, outputs["Result"], "FeeEarner is Null");
        }

        /// <summary>
        /// Create FeeEarner with a User Test
        /// </summary>
        [TestMethod]
        public void CreateFeeEarnerWithUserTest()
        {
            InitializeTest.OMSLogin();

            //  Find User
            Activity findUser = new FindUser();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findUser);
            FWBS.OMS.User user = (FWBS.OMS.User)outputs["Result"];
            Assert.AreNotEqual(null, outputs["Result"], "User is Null");

            //  Create FeeEarner
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("User", user);

            Activity createFeeEarner = new CreateFeeEarner();
            outputs = WorkflowInvoker.Invoke(createFeeEarner, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "FeeEarner is Null");
        }
    }
}
