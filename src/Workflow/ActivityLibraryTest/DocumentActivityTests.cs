using System;
using System.Activities;
using System.Collections.Generic;
using FWBS.WF.OMS.ActivityLibrary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ActivityLibraryTest
{
    [TestClass]
    public class DocumentActivityTests
    {
        /// <summary>
        /// Find Document Test
        /// </summary>
        [TestMethod]
        public void FindDocumentTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Find Document            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity findDocument = new FindDocument();
            outputs = WorkflowInvoker.Invoke(findDocument, parameters);
            Assert.AreNotEqual(null, outputs["Result"], "Document is Null");
        }

        /// <summary>
        /// Open Document Test
        /// </summary>
        [TestMethod]
        public void OpenDocumentTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Find Document            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity findDocument = new FindDocument();
            outputs = WorkflowInvoker.Invoke(findDocument, parameters);
            FWBS.OMS.OMSDocument document = (FWBS.OMS.OMSDocument)outputs["Result"];
            Assert.AreNotEqual(null, outputs["Result"], "Document is Null");

            //  Open Document
            parameters.Clear();
            parameters.Add("Document", document);
            parameters.Add("Mode", FWBS.OMS.DocOpenMode.View);

            Activity openDocument = new OpenDocument();
            outputs = WorkflowInvoker.Invoke(openDocument, parameters);
        }        

        /// <summary>
        /// Show Document Test
        /// </summary>
        [TestMethod]
        public void ShowDocumentTest()
        {
            InitializeTest.OMSLogin();

            //  Find File
            Activity findFile = new FindFile();
            IDictionary<String, Object> outputs = WorkflowInvoker.Invoke(findFile);
            FWBS.OMS.OMSFile file = (FWBS.OMS.OMSFile)outputs["Result"];
            Assert.AreNotEqual(null, file, "File is Null");

            //  Find Document            
            Dictionary<String, Object> parameters = new Dictionary<string, object>();
            parameters.Add("File", file);

            Activity findDocument = new FindDocument();
            outputs = WorkflowInvoker.Invoke(findDocument, parameters);
            FWBS.OMS.OMSDocument document = (FWBS.OMS.OMSDocument)outputs["Result"];
            Assert.AreNotEqual(null, outputs["Result"], "Document is Null");

            //  Save Document
            parameters.Clear();
            parameters.Add("Document", document);

            Activity showdocument = new ShowDocument();
            outputs = WorkflowInvoker.Invoke(showdocument, parameters);
        }
    }
}
