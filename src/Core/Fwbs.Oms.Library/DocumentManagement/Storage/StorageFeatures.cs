using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    [Flags]
    public enum StorageFeature
    {
        Retrieving = 1,
        Storing = 2,
        Purging = 4,
        xReserved1 = 8,
        xReserved2 = 16,
        AllowOverwrite = 32,
        Versioning = 64,
        CreateVersion= 128,
        CreateSubVersion = 256,
        Locking = 512,
        xReserved6 = 1024,
        xReserved7 = 2048,
        DuplicateChecking = 4096,
        Register = 8192,
      

        
    }
}
