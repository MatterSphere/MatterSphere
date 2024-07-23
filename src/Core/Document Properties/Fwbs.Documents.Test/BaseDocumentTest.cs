using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fwbs.Documents.Test
{
    using System.IO;

    [TestClass]
    public abstract class BaseDocumentTest
    {
        private ICustomPropertiesDocument doc;

        public BaseDocumentTest()
        {
        }

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

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() 
        {
            Init(out doc);
        }
        //
        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
         public void MyTestCleanup() 
        {
            IDisposable disp = doc as IDisposable;

            if (disp != null)
                disp.Dispose();

            if (doc != null)
                doc.Close();
        }
        
        #endregion

        protected abstract void Init(out ICustomPropertiesDocument doc);

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void FileMissingTest()
        {
            doc.Open(new FileInfo(@"C:\poweirtpoweirtpowier.msg"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void OpenNullTest()
        {
            doc.Open(null);
        }

        [TestMethod]
        public void InitialisedStateTest()
        {
            Assert.IsFalse(doc.IsOpen, "Document should be closed");
        }

        [TestMethod]
        [ExpectedException(typeof(Fwbs.Documents.FileClosedException))]
        public void SaveUnopenFileTest()
        {
            doc.Save();
        }

        [TestMethod]
        [ExpectedException(typeof(Fwbs.Documents.FileClosedException))]
        public void ReadPropertiesUnopenFileTest()
        {
            CustomPropertyCollection props = new CustomPropertyCollection();
            doc.ReadCustomProperties(props);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadPropertiesNullTest()
        {
            doc.ReadCustomProperties(null);
        }

        [TestMethod]
        [ExpectedException(typeof(Fwbs.Documents.FileClosedException))]
        public void WritePropertiesUnopenFileTest()
        {
            CustomPropertyCollection props = new CustomPropertyCollection();
            doc.WriteCustomProperties(props);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WritePropertiesNullTest()
        {
            doc.WriteCustomProperties(null);
        }

        [TestMethod]
        public void CloseUnopenFileTest()
        {
            doc.Close();
        }

        [WorkItem(2142)]
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void WriteCaseSensitiveProperties()
        {
            CustomPropertyCollection props = new CustomPropertyCollection();
            props.Add("Test");
            props.Add("TEST");
        }
    }
}
