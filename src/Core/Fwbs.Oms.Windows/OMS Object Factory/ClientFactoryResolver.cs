using System;
using System.Linq;

namespace FWBS.OMS.UI.Factory
{
    class ClientFactoryResolver : FactoryResolver
    {
        public override object Get(OMSObjectFactoryItem item)
        {
            if (item.Type == typeof(Client).ToString())
            {
                Int64 id = item.IDs.First();
                Client obj = Client.GetClient(id);
                return obj;
            }
            else
                return null;
        }

        public override OMSObjectFactoryItem CreateFactoryItem(object fromobject)
        {
            var client = fromobject as Client;
            if (client != null)
            {
                var item = new OMSObjectFactoryItem();
                item.Type = client.GetType().ToString();
                item.IDs = new long[1] { client.ID };
                return item;
            }
            else
                return null;
        }
    }
}
