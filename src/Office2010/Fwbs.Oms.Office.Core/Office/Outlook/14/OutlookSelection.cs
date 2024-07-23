using System;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookSelection
    {

        #region Selection Members


        public MSOutlook.Selection GetSelection(MSOutlook.OlSelectionContents SelectionContents)
        {
            throw new NotImplementedException();
        }

        public MSOutlook.OlSelectionLocation Location
        {
            get {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
