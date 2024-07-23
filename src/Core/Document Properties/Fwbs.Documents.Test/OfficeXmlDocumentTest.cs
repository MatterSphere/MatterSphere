using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fwbs.Documents.Test
{
    [TestClass]
    public class OfficeXmlDocumentTest : BaseDocumentTest
    {
        protected override void Init(out ICustomPropertiesDocument doc)
        {
            doc = new OfficeXmlDocument();
        }
    }
}
