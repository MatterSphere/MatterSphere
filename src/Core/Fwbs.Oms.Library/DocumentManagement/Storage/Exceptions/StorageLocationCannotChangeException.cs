using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    /// <summary>
    /// An exception that can be raised when a Storage provider is trying to be changed for a store item.
    /// </summary>
    public class StorageLocationCannotChangeException : StorageException
    {
        private const string MESSAGE = "The storage location cannot be changed.";
        private const string CODE = "STOREPROVCHANGE";

        public StorageLocationCannotChangeException()
            : base(CODE, MESSAGE) { }

        public StorageLocationCannotChangeException(Exception innerException)
            : base(CODE, MESSAGE, innerException) { }
    }

   
  

}
