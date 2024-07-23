using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ActivityLibraryTest
{
    [TestClass]
    public class FileActivityTests
    {
        /// <summary>
        /// Create File Test
        /// </summary>
        [TestMethod]
        public void CreateFileTest()
        {
            InitializeTest.OMSLogin();

            Activity createFile = new CreateFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(createFile);
            Assert.AreNotEqual(null, outputs["Result"], "File is Null");
        }

        /// <summary>
        /// Find File Test
        /// </summary>
        [TestMethod]
        public void FindFileTest()
        {
            InitializeTest.OMSLogin();

            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            Assert.AreNotEqual(null, outputs["Result"], "File is Null");
        }

        /// <summary>
        /// Select File Test
        /// </summary>
        [TestMethod]
        public void SelectFileTest()
        {
            InitializeTest.OMSLogin();

            Activity selectFile = new SelectFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(selectFile);
            Assert.AreNotEqual(null, outputs["Result"], "File is Null");
        }

        /// <summary>
        /// Show File Test
        /// </summary>
        [TestMethod]
        public void ShowFileTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Show File            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity showFile = new ShowFile();
            outputs = WorkflowInvoker.Invoke(showFile, parameters);
        }

        /// <summary>
        /// Show File Conflict Test
        /// </summary>
        [TestMethod]
        public void ShowFileConflictTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Show File            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity showFileConflicy = new ShowFileConflict();
            outputs = WorkflowInvoker.Invoke(showFileConflicy, parameters);
        }

        /// <summary>
        /// File Review Test
        /// </summary>
        [TestMethod]
        public void FileReviewTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Show File            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity showFileConflicy = new FileReview();
            outputs = WorkflowInvoker.Invoke(showFileConflicy, parameters);
        }
    }
}
