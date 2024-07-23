using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ActivityLibraryTest
{
    [TestClass]
    public class TaskTests
    {
        /// <summary>
        /// Create Task Test
        /// </summary>
        [TestMethod]
        public void CreateTaskTest()
        {
            InitializeTest.OMSLogin();

            //  Create Task
            Activity createTask = new CreateTask();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(createTask);
            Assert.AreNotEqual(null, outputs["Result"], "Task is Null");
        }

        /// <summary>
        /// Create Task with a File Test
        /// </summary>
        [TestMethod]
        public void CreateTaskWithFileTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Create Task
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity createTask = new CreateTask();
            outputs = WorkflowInvoker.Invoke(createTask, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "Task is Null");
        }
       
    }
}
