namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookFolder 
    {

        public MSOutlook.CalendarSharing GetCalendarExporter()
        {
            return InternalItem.GetCalendarExporter();
        }


        public MSOutlook.StorageItem GetStorage(string StorageIdentifier, MSOutlook.OlStorageIdentifierType StorageIdentifierType)
        {
            return InternalItem.GetStorage(StorageIdentifier, StorageIdentifierType);
        }

        public MSOutlook.Table GetTable(object Filter, object TableContents)
        {
            return InternalItem.GetTable(Filter, TableContents);

        }
        public MSOutlook.PropertyAccessor PropertyAccessor
        {
            get { return InternalItem.PropertyAccessor; }

        }
        public MSOutlook.UserDefinedProperties UserDefinedProperties
        {
            get { return InternalItem.UserDefinedProperties; }
        }


        public MSOutlook.Store Store
        {
            get { return InternalItem.Store; }
        }
    }
}
