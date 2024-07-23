using System;

namespace FWBS.OMS.DocumentManagement.Storage
{

    /// <summary>
    /// An exception that can is raised when a document is already checked out.
    /// </summary>
    public class StorageItemCheckedOutException : StorageException
    {
        private const string MESSAGE = "This document is already checked out by '%1%' at '%2%'.";
        private const string CODE = "SPCHECKEDOUT";

        public StorageItemCheckedOutException(string user, DateTime time)
            : base(CODE, MESSAGE, null, user, time.ToString()) { }

        public StorageItemCheckedOutException(string user, DateTime time, Exception innerException)
            : base(CODE, MESSAGE, innerException, user, time.ToString()) { }
    }
}
