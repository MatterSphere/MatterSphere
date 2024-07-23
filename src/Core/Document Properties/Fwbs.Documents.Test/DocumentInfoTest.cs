using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fwbs.Documents.Test
{

    [TestClass]
    public class DocumentInfoTest
    {
        // If you get 0x800401FA (CO_E_WRONGOSFORAPP) error on attempt to create an instance of a Redemption object:
        // The bitness of the running process, Redemption, and Outlook/MAPI must all match, see
        // http://www.dimastr.com/redemption/faq.htm#ErrorCreatingRedemptionObject

        public static Dictionary<string, FileInfo> TestDocuments = new Dictionary<string, FileInfo>();
        public static DirectoryInfo TestDocumentsDirectory;

        public DocumentInfoTest()
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
        [AssemblyInitialize()]
        public static void MyClassInitialize(TestContext testContext) 
        {
            try
            {
                TestDocumentsDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), "Test Documents for DocumentInfo"));
                if (!System.IO.Directory.Exists(TestDocumentsDirectory.FullName))
                    TestDocumentsDirectory.Create();

                string[] resources = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();

                foreach (string res in resources)
                {
                    using (Stream strm = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(res))
                    {
                        FileInfo file = new FileInfo(Path.Combine(TestDocumentsDirectory.FullName, res));
                        file.Refresh();
                        if (file.Exists)
                            file.Delete();
                        using (FileStream fs = file.OpenWrite())
                        {
                            byte[] buffer = new byte[strm.Length];
                            strm.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, buffer.Length);
                        }

                        TestDocuments.Add(res, file);
                    }
                }

            }
            catch
            {
                throw;
            }
        }
        
        // Use ClassCleanup to run code after all tests in a class have run
         [AssemblyCleanup()]
         public static void MyClassCleanup() 
         {
             TestDocumentsDirectory.Refresh();
             if (System.IO.Directory.Exists(TestDocumentsDirectory.FullName))
                 TestDocumentsDirectory.Delete(true);
         }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        public void FileMissingTest()
        {
            using (DocumentInfo doc = new DocumentInfo(@"C:\poweirtpoweirtpowier.msg"))
            {
            }
        }

        [TestMethod]
        public void CloseFileTest()
        {
            using (DocumentInfo doc = new DocumentInfo(TestDocuments.Values.FirstOrDefault()))
            {
                doc.Close();
            }
        }


        [TestMethod]
        public void TestDocumentsInitTest()
        {
            foreach (FileInfo file in TestDocuments.Values)
            {
                using (DocumentInfo doc = new DocumentInfo(file))
                {
                    if (file.Name.IndexOf("noprops") > -1)
                    {
                        Assert.IsTrue(doc.CustomProperties.Count == 0, "Number of properties should be zero for file '{0}'.", file.Name);
                    }
                    else if (file.Name.IndexOf("props") > -1)
                    {
                        Assert.IsTrue(doc.CustomProperties.Count == 1, "Number of properties should be one for file '{0}'.", file.Name);
                        CustomProperty prop = doc.CustomProperties[0];
                        Assert.AreEqual<string>("TEST", prop.Name, "Property should be named 'TEST' for file '{0}'.", file.Name);
                        Assert.AreEqual("Testing 123", prop.Value, "Test property should by a 'Testing 123' string for file '{0}'.", file.Name);

                    }

                    Assert.IsFalse(doc.CustomProperties.HasChanged, "Should not be flagged as changed.");

                }
            }

        }


        [TestMethod]
        public void DeletePropertiesTest()
        {
            foreach (FileInfo file in TestDocuments.Values)
            {
                using (DocumentInfo doc = new DocumentInfo(file))
                {
                    foreach (CustomProperty prop in doc.CustomProperties)
                    {
                        prop.Delete();
                    }

                    doc.Save();
                }

                using (DocumentInfo doc = new DocumentInfo(file))
                {
                    Assert.IsTrue(doc.CustomProperties.Count == 0, "All Properties should now be deleted.");
                }

            }

        }


    }
}
