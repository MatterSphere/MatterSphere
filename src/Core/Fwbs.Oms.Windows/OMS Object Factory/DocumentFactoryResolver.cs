using System;
using System.Linq;

namespace FWBS.OMS.UI.Factory
{
    class DocumentFactoryResolver : FactoryResolver
    {
        public override object Get(OMSObjectFactoryItem item)
        {
            if (item.Type == typeof(OMSDocument).ToString())
            {
                Int64 id = item.IDs.First();
                OMSDocument obj = OMSDocument.GetDocument(id);
                return obj;
            }
            else
                return null;
        }

        public override OMSObjectFactoryItem CreateFactoryItem(object fromobject)
        {
            var doc = fromobject as OMSDocument;
            if (doc != null)
            {
                var item = new OMSObjectFactoryItem();
                item.Type = doc.GetType().ToString();
                item.IDs = new long[1] { doc.ID };
                return item;
            }
            else
                return null;
        }
    }
}
