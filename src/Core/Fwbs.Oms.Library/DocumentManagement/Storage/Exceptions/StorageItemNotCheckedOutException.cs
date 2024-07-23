using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public class StorageItemNotCheckedOutException : StorageException
    {
        private const string MESSAGE = "Document %1% is not checked out.";
        private const string CODE = "NOTCHECKEDOUT";

         public StorageItemNotCheckedOutException(IStorageItem item)
            : base(CODE, MESSAGE, null, item.DisplayID) { }

         public StorageItemNotCheckedOutException(IStorageItem item, Exception innerException)
             : base(CODE, MESSAGE, innerException, item.DisplayID) { }


    }
}
