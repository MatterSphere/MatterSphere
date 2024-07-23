using System;

namespace FWBS.OMS.DocumentManagement.Storage.Providers
{
    [System.Runtime.InteropServices.Guid("559970BA-5DCA-471f-8E65-C5F3B84D669B")]
    public sealed class BlobStorageProviderService : StorageProviderService
    {
        public BlobStorageProviderService()
            : base()
        {
            this.ServiceName = "Blob Storage";
        }

        protected override void InternalTest()
        {
        }

        protected override StorageProviderService CreateDocumentService()
        {
            return null;
        }

        protected override void DocumentTest(FWBS.OMS.Connectivity.IConnectableService service)
        {
            throw new NotImplementedException();
        }

        protected override StorageProviderService CreatePrecedentService()
        {
            return null;
        }

        protected override void PrecedentTest(FWBS.OMS.Connectivity.IConnectableService service)
        {
            throw new NotImplementedException();
        }

    }
}
