namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookItem
    {

        #region ItemEvents_10_Event Members

        event MSOutlook.ItemEvents_10_ReadCompleteEventHandler MSOutlook.ItemEvents_10_Event.ReadComplete
        {
            add
            {
                obj.ReadComplete += value;
            }
            remove
            {
                obj.ReadComplete -= value;
            }
        }

        #endregion



    }
}
