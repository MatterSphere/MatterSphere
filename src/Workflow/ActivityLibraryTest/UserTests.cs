using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class UserTests
    {
        /// <summary>
        /// Create User Test
        /// </summary>
        [TestMethod]
        public void CreateUserTest()
        {
            InitializeTest.OMSLogin();

            Activity createUser = new CreateUser();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(createUser);
            Assert.AreNotEqual(null, outputs["Result"], "User is Null");
        }

        /// <summary>
        /// Find User Test
        /// </summary>
        [TestMethod]
        public void FindUserTest()
        {
            InitializeTest.OMSLogin();

            Activity findUser = new FindUser();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findUser);
            Assert.AreNotEqual(null, outputs["Result"], "User is Null");
        }
    }
}
