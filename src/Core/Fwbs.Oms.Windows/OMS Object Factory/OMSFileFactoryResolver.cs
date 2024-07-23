using System;
using System.Linq;

namespace FWBS.OMS.UI.Factory
{
    class OMSFileFactoryResolver : FactoryResolver
    {
        public override object Get(OMSObjectFactoryItem item)
        {
            if (item.Type == typeof(OMSFile).ToString())
            {
                Int64 id = item.IDs.First();
                OMSFile obj = OMSFile.GetFile(id);
                return obj;
            }
            else
                return null;
        }

        public override OMSObjectFactoryItem CreateFactoryItem(object fromobject)
        {
            var file = fromobject as OMSFile;
            if (file != null)
            {
                var item = new OMSObjectFactoryItem();
                item.Type = file.GetType().ToString();
                item.IDs = new long[1] { file.ID };
                return item;
            }
            else
                return null;
        }
    }
}
