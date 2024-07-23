using System;
using System.Data;
using FWBS.OMS.Extensibility;

namespace FWBS.OMS.HighQ
{
    public static class HighQAdapter
    {
        public static DataTable GetMatters(long clId)
        {
            return GetMattersDataTable(clId);
        }

        private static DataTable GetMattersDataTable(long clientId)
        {
            return GetHighQ().GetMatters(clientId);
        }

        public static IHighQ GetHighQ()
        {
            foreach (AddinConfiguration addin in Session.CurrentSession.Addins)
            {
                if (addin.Code == "HIGHQ")
                {
                    return addin.Instance as IHighQ;
                }
            }

            var message = Session.CurrentSession.Resources.GetMessage("HQADDNTFND",
                "The HighQ addin was not found", "").Text;
            throw new Exception(message);
        }
    }    
}
