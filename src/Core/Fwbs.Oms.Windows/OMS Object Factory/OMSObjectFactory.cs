using System;
using System.Collections.Generic;

namespace FWBS.OMS.UI.Factory
{
    public static class OMSObjectFactory
    {

        private static List<FactoryResolver> factoryresolvers;

        private static void BuildFactoryResolvers()
        {
            if (factoryresolvers == null)
            {
                factoryresolvers = new List<FactoryResolver>();
                factoryresolvers.Add(new OMSFileFactoryResolver());
                factoryresolvers.Add(new ClientFactoryResolver());
                factoryresolvers.Add(new DocumentFactoryResolver());
                factoryresolvers.Add(new AssociateFactoryResolver());
                factoryresolvers.Add(new ContactFactoryResolver());
                factoryresolvers.Add(new UserFactoryResolver());
                factoryresolvers.Add(new FeeEarnerFactoryResolver());
            }
        }
        public static object Get(OMSObjectFactoryItem item)
        {
            if (item == null)
                return null;
            BuildFactoryResolvers();
            foreach (var fr in factoryresolvers)
            {
                var result = fr.Get(item);
                if (result != null)
                    return result;
            }
            return null;
        }

        public static OMSObjectFactoryItem CreateFactoryItem(object fromobject)
        {
            if (fromobject == null)
                return null;
            BuildFactoryResolvers();
            foreach (var fr in factoryresolvers)
            {
                var result = fr.CreateFactoryItem(fromobject);
                if (result != null)
                {
                    return result;
                }
            }            
            return null;
        }
    }

    [Serializable]
    public class OMSObjectFactoryItem
    {
        public string Type { get; set; }
        public Int64[] IDs { get; set; }
    }

    public abstract class FactoryResolver
    {
        public abstract object Get(OMSObjectFactoryItem item);
        public abstract OMSObjectFactoryItem CreateFactoryItem(object fromobject);
    }


    public static class frmOMSItemFactory
    {
        public static FWBS.OMS.UI.Windows.Interfaces.IShowOMSItem GetFrmOMSItem(string code, object parent, FWBS.OMS.EnquiryEngine.EnquiryMode mode, bool offline, Common.KeyValueCollection param)
        {
            return new FWBS.OMS.UI.Windows.frmOMSItemV2(code, parent, mode, offline, param);
        }


        public static FWBS.OMS.UI.Windows.Interfaces.IShowOMSItem GetFrmOMSItem(string code, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, Common.KeyValueCollection param)
        {
            return new FWBS.OMS.UI.Windows.frmOMSItemV2(code, parent, businessObject, param);
        }
    }
}
