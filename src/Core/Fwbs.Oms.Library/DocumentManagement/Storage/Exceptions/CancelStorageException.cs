using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public class CancelStorageException : StorageException
    {
        public CancelStorageException(string errCode, string errDescription)
            : base(errCode, errDescription)
        {
        }
        public CancelStorageException(string errCode, string errDescription, Exception innerException)
            : base(errCode, errDescription, innerException)
        {
        }
        public CancelStorageException(string errCode, string errDescription, Exception innerException, params string[] param)
            : base(errCode, errDescription, innerException, param)
        {
        }
    }
}
