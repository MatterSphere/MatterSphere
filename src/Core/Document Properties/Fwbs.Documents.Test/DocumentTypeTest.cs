using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fwbs.Documents.Test
{
    [TestClass]
    public class DocumentTypeTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

      
   

        [TestMethod]
        public void DOCTest()
        {
            using (DocumentInfo doc = new DocumentInfo(DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type.doc"]))
            {
                PrivateObject po = new PrivateObject(doc);
                IRawDocument raw = (IRawDocument)po.GetField("doc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.IsInstanceOfType(raw, typeof(DSODocument));
            }
        }

        [TestMethod]
        public void DOCXTest()
        {
            using (DocumentInfo doc = new DocumentInfo(DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type.docx"]))
            {
                PrivateObject po = new PrivateObject(doc);
                IRawDocument raw = (IRawDocument)po.GetField("doc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.IsInstanceOfType(raw, typeof(OfficeXmlDocument));
            }
        }

        [TestMethod]
        public void DOCAsDOCXTest()
        {
            using (DocumentInfo doc = new DocumentInfo(DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type-docasdocx.docx"]))
            {
                PrivateObject po = new PrivateObject(doc);
                IRawDocument raw = (IRawDocument)po.GetField("doc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.IsInstanceOfType(raw, typeof(DSODocument));
            }
        }


        [TestMethod]
        public void DOCXAsDOCTest()
        {
            using (DocumentInfo doc = new DocumentInfo(DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type-docxasdoc.doc"]))
            {
                PrivateObject po = new PrivateObject(doc);
                IRawDocument raw = (IRawDocument)po.GetField("doc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.IsInstanceOfType(raw, typeof(OfficeXmlDocument));
            }
        }


        [TestMethod]
        public void XLSTest()
        {
            using (DocumentInfo doc = new DocumentInfo(DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type.xls"]))
            {
                PrivateObject po = new PrivateObject(doc);
                IRawDocument raw = (IRawDocument)po.GetField("doc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.IsInstanceOfType(raw, typeof(DSODocument));
            }
        }

        [TestMethod]
        public void XLSXTest()
        {
            using (DocumentInfo doc = new DocumentInfo(DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type.xlsx"]))
            {
                PrivateObject po = new PrivateObject(doc);
                IRawDocument raw = (IRawDocument)po.GetField("doc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.IsInstanceOfType(raw, typeof(OfficeXmlDocument));
            }
        }

        [TestMethod]
        public void MSGTest()
        {
            using (DocumentInfo doc = new DocumentInfo(DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type.msg"]))
            {
                PrivateObject po = new PrivateObject(doc);
                IRawDocument raw = (IRawDocument)po.GetField("doc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.IsInstanceOfType(raw, typeof(MsgDocument));
            }
        }

        [TestMethod]
        public void OFTTest()
        {
            using (DocumentInfo doc = new DocumentInfo(DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type.oft"]))
            {
                PrivateObject po = new PrivateObject(doc);
                IRawDocument raw = (IRawDocument)po.GetField("doc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.IsInstanceOfType(raw, typeof(MsgDocument));
            }
        }

        [TestMethod]
        public void TXTTest()
        {
            using (DocumentInfo doc = new DocumentInfo(DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type.txt"]))
            {
                PrivateObject po = new PrivateObject(doc);
                IRawDocument raw = (IRawDocument)po.GetField("doc", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Assert.IsInstanceOfType(raw, typeof(DSODocument));
            }
        }
    }
}
