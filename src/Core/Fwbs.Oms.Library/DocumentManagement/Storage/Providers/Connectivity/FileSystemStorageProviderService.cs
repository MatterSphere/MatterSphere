namespace FWBS.OMS.DocumentManagement.Storage.Providers
{
    [System.Runtime.InteropServices.Guid("A2A368AA-8071-4926-A51B-18A6AEF28169")]
    public sealed class FileSystemStorageProviderService : StorageProviderService
    {
 

        private FileSystemStorageProviderService(string name)
            : base(name, true)
        {
            StartPolling();
        }

        public FileSystemStorageProviderService()
            : base()
        {
            this.ServiceName = "File System Storage";
        }

        protected override StorageProviderService CreateDocumentService()
        {
            return new FileSystemStorageProviderService("Document File System Storage");
        }

        protected override StorageProviderService CreatePrecedentService()
        {
            return new FileSystemStorageProviderService("Precedent File System Storage");
        }

        protected override void DocumentTest(Connectivity.IConnectableService service)
        {
            System.IO.DirectoryInfo dir = Session.CurrentSession.GetDirectory("OMDOCUMENTS");
            if (dir == null || dir.Exists == false)
                throw new Connectivity.ConnectivityException(Session.CurrentSession.Resources.GetMessage("DCMDRCNTEXST", "The document directory on the file system does not exist or is disconnected.", "").Text);
        }

        protected override void PrecedentTest(Connectivity.IConnectableService service)
        {
            System.IO.DirectoryInfo dir = Session.CurrentSession.GetDirectory("OMPRECEDENTS");
            if (dir == null || dir.Exists == false)
                throw new Connectivity.ConnectivityException(Session.CurrentSession.Resources.GetMessage("PRCDRCNTEXST", "The precedent directory on the file system does not exist or is disconnected.", "").Text);
        }

       
    }
}
