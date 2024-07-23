namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookSession
    {
        #region _NameSpace Members


        public Microsoft.Office.Core.ContactCard CreateContactCard(MSOutlook.AddressEntry AddressEntry)
        {
            return ns.CreateContactCard(AddressEntry);
        }

        #endregion
    }
}
