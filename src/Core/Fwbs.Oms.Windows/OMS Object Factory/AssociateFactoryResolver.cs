using System;
using System.Linq;

namespace FWBS.OMS.UI.Factory
{
    class AssociateFactoryResolver : FactoryResolver
    {
        public override object Get(OMSObjectFactoryItem item)
        {
            if (item.Type == typeof(Associate).ToString())
            {
                Int64 id = item.IDs.First();
                Associate obj = Associate.GetAssociate(id);
                return obj;
            }
            else
                return null;
        }

        public override OMSObjectFactoryItem CreateFactoryItem(object fromobject)
        {
            var ass = fromobject as Associate;
            if (ass != null)
            {
                var item = new OMSObjectFactoryItem();
                item.Type = ass.GetType().ToString();
                item.IDs = new long[1] { ass.ID };
                return item;
            }
            else
                return null;
        }
    }
}
