using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class PrecedentActivityTests
    {
        /// <summary>
        /// Find Precedent Test
        /// </summary>
        [TestMethod]
        public void FindPrecedentTest()
        {
            InitializeTest.OMSLogin();

            string precedentType = "";

            Activity selectAssociate = new SelectAssociate();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(selectAssociate);
            FWBS.OMS.Associate associate = (FWBS.OMS.Associate)outputs["Result"];
            Assert.AreNotEqual(null, outputs["Result"], "Associate is Null");

            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("PrecedentType", precedentType);
            parameters.Add("SelectedAssociate", associate);

            Activity findPrecedent = new FindPrecedent();
            outputs = WorkflowInvoker.Invoke(findPrecedent, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "Precedent is Null");
        }
      
    }
}
