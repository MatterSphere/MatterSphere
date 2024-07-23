using System;
using System.Linq;

namespace FWBS.OMS.UI.Factory
{
    class FeeEarnerFactoryResolver : FactoryResolver
    {
        public override object Get(OMSObjectFactoryItem item)
        {
            if (item.Type == typeof(FeeEarner).ToString())
            {
                Int64 id = item.IDs.First();
                User obj = FeeEarner.GetFeeEarner(Convert.ToInt32(id));
                return obj;
            }
            else
                return null;
        }

        public override OMSObjectFactoryItem CreateFactoryItem(object fromobject)
        {
            var feeearner = fromobject as FeeEarner;
            if (feeearner != null)
            {
                var item = new OMSObjectFactoryItem();
                item.Type = feeearner.GetType().ToString();
                item.IDs = new long[1] { feeearner.ID };
                return item;
            }
            else
                return null;
        }
    }
}
