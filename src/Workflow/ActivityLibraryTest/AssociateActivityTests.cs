using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class AssociateActivityTests
    {
        /// <summary>
        /// Create Associate Test
        /// </summary>
        [TestMethod]
        public void CreateAssociateTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Create Associate            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity createAssociate = new CreateAssociate();
            outputs = WorkflowInvoker.Invoke(createAssociate, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "Associate is Null");
        }

        /// <summary>
        /// Select Associate Test
        /// </summary>
        [TestMethod]
        public void SelectAssociateTest()
        {
            InitializeTest.OMSLogin();

            Activity selectAssociate = new SelectAssociate();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(selectAssociate);
            Assert.AreNotEqual(null, outputs["Result"], "Associate is Null");
        }

        /// <summary>
        /// Select Default Associate Test
        /// </summary>
        [TestMethod]
        public void SelectDefaultAssociateTest()
        {
            InitializeTest.OMSLogin();

            Activity selectDefaultAssociate = new SelectDefaultAssociate();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(selectDefaultAssociate);
            Assert.AreNotEqual(null, outputs["Result"], "Associate is Null");
        }

        /// <summary>
        /// Show Associate Test
        /// </summary>
        [TestMethod]
        public void ShowAssociateTest()
        {
            InitializeTest.OMSLogin();

            //  Find Associate
            Activity selectAssociate = new SelectAssociate();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(selectAssociate);
            FWBS.OMS.Associate associate = (FWBS.OMS.Associate)outputs["Result"];
            Assert.AreNotEqual(null, outputs["Result"], "Associate is Null");

            //  Show Associate            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("Associate", associate);

            Activity showAssociate = new ShowAssociate();
            outputs = WorkflowInvoker.Invoke(showAssociate, parameters);
        }

        /// <summary>
        /// Pick Associate Test
        /// </summary>
        [TestMethod]
        public void PickAssociateTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Create Associate            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity pickAssociate = new PickAssociate();
            outputs = WorkflowInvoker.Invoke(pickAssociate, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "Associate is Null");
        }
    }
}
