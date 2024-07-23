using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fwbs.Documents.Test
{
    [TestClass]
    public class JZipDocumentTest
    {
        [TestMethod]
        public void JZip_DocxFilesTest()
        {
            foreach (var file in DocumentInfoTest.TestDocuments.Values.Where(f => f.Extension == ".docx"))
            {
                var testFile = file.CopyTo(file.FullName + ".zip", true);
                try
                {
                    using (var parser = new JZipOfficeXmlParser())
                    {
                        parser.Open(testFile);
                        parser.Save();
                    }
                }
                catch (InvalidDataException)
                {
                    Assert.AreEqual(file, DocumentInfoTest.TestDocuments["System.Documents.Test.Resources.test-type-docasdocx.docx"]);
                }
                testFile.Delete();
            }
        }
    }
}
