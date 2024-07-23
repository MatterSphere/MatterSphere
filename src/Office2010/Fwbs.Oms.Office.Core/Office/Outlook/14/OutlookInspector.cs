namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookInspector
    {
        #region _Inspector Members


        public MSOutlook.AttachmentSelection AttachmentSelection
        {
            get { return inspector.AttachmentSelection; }
        }

        public void SetSchedulingStartTime(System.DateTime Start)
        {
            inspector.SetSchedulingStartTime(Start);
        }

        #endregion

        #region InspectorEvents_10_Event Members


        public event MSOutlook.InspectorEvents_10_AttachmentSelectionChangeEventHandler AttachmentSelectionChange
        {
            add
            {
                inspector.AttachmentSelectionChange += value;
            }
            remove
            {
                inspector.AttachmentSelectionChange -= value;
            }
        }

        #endregion
    }
}
