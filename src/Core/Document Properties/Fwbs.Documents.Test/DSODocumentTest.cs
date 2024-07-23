using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fwbs.Documents.Test
{
    [TestClass]
    public class DSODocumentTest : BaseDocumentTest
    {
        protected override void Init(out ICustomPropertiesDocument doc)
        {
            doc = new DSODocument();
        }
    }
}
