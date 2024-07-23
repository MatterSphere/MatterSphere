using System.Collections.Generic;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public abstract class StorageProviderService : Connectivity.ConnectableService
    {
        private StorageProviderService documents = null;
        private StorageProviderService precedents = null;

        protected StorageProviderService()
            : base()
        {
            documents = CreateDocumentService();
            precedents = CreatePrecedentService();

            if (documents != null)
                documents.SetTestMethod(new FWBS.OMS.Connectivity.TestConnectivityDelegate(this.DocumentTest));
            
            if (precedents != null)
                precedents.SetTestMethod(new FWBS.OMS.Connectivity.TestConnectivityDelegate(this.PrecedentTest));
        }

        protected StorageProviderService(string name, bool generateId)
            : base(name, generateId)
        {
     
        }

        protected override bool InternalDependsOn(FWBS.OMS.Connectivity.IConnectableService service)
        {
            if (service.Id == Connectivity.ConnectivityManager.OMS_SERVICE)
                return true;
            else
                return false;
        }

        protected override void InternalTest()
        {
            if (documents != null)
                documents.Test();

            if (precedents != null)
                precedents.Test();
        }

        protected abstract StorageProviderService CreateDocumentService();

        protected abstract StorageProviderService CreatePrecedentService();

        protected abstract void DocumentTest(Connectivity.IConnectableService service);

        protected abstract void PrecedentTest(Connectivity.IConnectableService service);
        
        public StorageProviderService PrecedentService
        {
            get
            {
                return precedents;
            }
        }

        public StorageProviderService DocumentService
        {
            get
            {
                return documents;
            }
        }



        public override Connectivity.IConnectableService[] GetServices()
        {
            List<Connectivity.IConnectableService> services = new List<FWBS.OMS.Connectivity.IConnectableService>();

            if (documents != null)
                services.Add(documents);

            if (precedents != null)
                services.Add(precedents);

            return services.ToArray();
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (documents != null)
                    {
                        documents.Dispose();
                        documents = null;
                    }

                    if (precedents != null)
                    {
                        precedents.Dispose();
                        precedents = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

    }
}
