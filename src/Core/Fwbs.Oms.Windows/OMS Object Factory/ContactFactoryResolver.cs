using System;
using System.Linq;

namespace FWBS.OMS.UI.Factory
{
    class ContactFactoryResolver : FactoryResolver
    {
        public override object Get(OMSObjectFactoryItem item)
        {
            if (item.Type == typeof(Contact).ToString())
            {
                Int64 id = item.IDs.First();
                Contact obj = Contact.GetContact(id);
                return obj;
            }
            else
                return null;
        }

        public override OMSObjectFactoryItem CreateFactoryItem(object fromobject)
        {
            var contact = fromobject as Contact;
            if (contact != null)
            {
                var item = new OMSObjectFactoryItem();
                item.Type = contact.GetType().ToString();
                item.IDs = new long[1] { contact.ID };
                return item;
            }
            else
                return null;
        }
    }
}
