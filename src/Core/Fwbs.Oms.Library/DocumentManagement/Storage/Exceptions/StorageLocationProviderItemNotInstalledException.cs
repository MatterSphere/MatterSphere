using System;

namespace FWBS.OMS.DocumentManagement.Storage
{

    /// <summary>
    /// An exception that can be raised when a Store item location provider is not registered.
    /// </summary>
    public class StorageLocationProviderItemNotInstalledException : StorageException
    {
        private const string MESSAGE = "Storage item location provider '%1%' is not installed.";
        private const string CODE = "STOREPROVINST";

        public StorageLocationProviderItemNotInstalledException(string type)
            : base(CODE, MESSAGE, null, type) { }

        public StorageLocationProviderItemNotInstalledException(Exception innerException, string type)
            : base(CODE, MESSAGE, innerException, type) { }
    }

   
  

}
