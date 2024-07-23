using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Fwbs.Documents.Test
{

    [TestClass]
    public class MsgDocumentTest : BaseDocumentTest
    {
        protected override void Init(out ICustomPropertiesDocument doc)
        {
            doc = new MsgDocument();
        }
    }
}
