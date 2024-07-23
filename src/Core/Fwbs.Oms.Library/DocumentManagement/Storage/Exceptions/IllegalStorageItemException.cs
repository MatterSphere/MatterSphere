using System;

namespace FWBS.OMS.DocumentManagement.Storage
{

    /// <summary>
    /// An exception that can is raised when a document is already checked out.
    /// </summary>
    public class IllegalStorageItemException : StorageException
    {
        private const string MESSAGE = "This document extension of '%1%' is registered as an illegal file extension.";
        private const string CODE = "SPILLEGALEXT";

        public IllegalStorageItemException(string extension)
            : base(CODE, MESSAGE, null, extension){ }

        public IllegalStorageItemException(string extension, Exception innerException)
            : base(CODE, MESSAGE, innerException, extension) { }
    }
}
