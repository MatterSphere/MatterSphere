using System;
using System.Linq;

namespace FWBS.OMS.UI.Factory
{
    class UserFactoryResolver : FactoryResolver
    {
        public override object Get(OMSObjectFactoryItem item)
        {
            if (item.Type == typeof(User).ToString())
            {
                Int64 id = item.IDs.First();
                User obj = User.GetUser(Convert.ToInt32(id));
                return obj;
            }
            else
                return null;
        }

        public override OMSObjectFactoryItem CreateFactoryItem(object fromobject)
        {
            var user = fromobject as User;
            if (user != null)
            {
                var item = new OMSObjectFactoryItem();
                item.Type = user.GetType().ToString();
                item.IDs = new long[1] { user.ID };
                return item;
            }
            else
                return null;
        }
    }
}
