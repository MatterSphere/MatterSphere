namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookInspector
    {

        public event MSOutlook.InspectorEvents_10_PageChangeEventHandler PageChange
        {
            add
            {
                inspector.PageChange += value;
            }
            remove
            {
                inspector.PageChange -= value;
            }
        }

        public object NewFormRegion()
        {
            return inspector.NewFormRegion();
        }

        public object OpenFormRegion(string Path)
        {
            return inspector.OpenFormRegion(Path);
        }
        public void SaveFormRegion(object Page, string FileName)
        {
            inspector.SaveFormRegion(Page, FileName);
        }
    }
}
