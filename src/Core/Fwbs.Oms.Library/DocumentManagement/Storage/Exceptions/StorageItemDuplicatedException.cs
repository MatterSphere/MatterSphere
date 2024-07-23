using System;

namespace FWBS.OMS.DocumentManagement.Storage
{

    /// <summary>
    /// An exception that can be raised when a documents checksum matches another.
    /// </summary>
    public class StorageItemDuplicatedException : StorageException
    {
        private const string MESSAGE = "This document may already exist. %1%";
        private const string CODE = "SPDUPLICATEITEM";

        public StorageItemDuplicatedException(IStorageItemDuplication item, string message)
            : base(CODE, MESSAGE, null, message) { }

        public StorageItemDuplicatedException(IStorageItemDuplication item, Exception innerException, string message)
            : base(CODE, MESSAGE, innerException, message) { }
    }
}
