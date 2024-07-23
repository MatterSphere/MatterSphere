namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookExplorer
    {

        #region _Explorer Members

        dynamic MSOutlook._Explorer.ActiveInlineResponse
        {
            get { return explorer.ActiveInlineResponse; }
        }

        dynamic MSOutlook._Explorer.ActiveInlineResponseWordEditor
        {
            get { return explorer.ActiveInlineResponseWordEditor; }
        }

        #endregion

        #region ExplorerEvents_10_Event Members

        event MSOutlook.ExplorerEvents_10_InlineResponseEventHandler MSOutlook.ExplorerEvents_10_Event.InlineResponse
        {
            add
            {
                explorer.InlineResponse += value;
            }
            remove
            {
                explorer.InlineResponse -= value;
            }
        }

        event MSOutlook.ExplorerEvents_10_InlineResponseCloseEventHandler MSOutlook.ExplorerEvents_10_Event.InlineResponseClose
        {
            add
            {
                explorer.InlineResponseClose += value;
            }
            remove
            {
                explorer.InlineResponseClose -= value;
            }
        }
        
        #endregion        
      
    }
}
