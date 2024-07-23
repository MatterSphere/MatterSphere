using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    /// <summary>
    /// An exception that can be raised when a Store item is not found in the location that it is supposed to be.
    /// </summary>
    [Obsolete("Does not seem to be used.")]
    public class StoreItemNotInStorageLocationException : StorageException
    {
        private const string MESSAGE = "The specified item is not in the specified storage.";
        private const string CODE = "STOREITEMMISS";

        public StoreItemNotInStorageLocationException()
            : base(CODE, MESSAGE) { }

        public StoreItemNotInStorageLocationException(Exception innerException)
            : base(CODE, MESSAGE, innerException) { }
    }

   
  

}
