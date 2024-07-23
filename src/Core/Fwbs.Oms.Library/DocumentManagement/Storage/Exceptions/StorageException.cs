using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    /// <summary>
    /// Generic Storage exception.
    /// </summary>
    public class StorageException : OMSException2
    {
        public StorageException(string errCode, string errDescription)
            : base(errCode, errDescription, "")
        {
        }
        public StorageException(string errCode, string errDescription, Exception innerException)
            : base(errCode, errDescription, "", innerException)
        {
        }
        public StorageException(string errCode, string errDescription, Exception innerException, params string[] param)
            : base(errCode, errDescription, "", innerException, false, param)
        {
        }
    }

 

}
