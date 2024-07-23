using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// An exception that will be raised when critical information is tried to be accessed.
    /// </summary>
    public sealed class AccessCiriticalDataException : SecurityException
    {
        public AccessCiriticalDataException(string dataName)
            : base(HelpIndexes.AccessCiriticalData, dataName) { }

        public AccessCiriticalDataException(Exception innerException, string dataName)
            : base(innerException, HelpIndexes.AccessCiriticalData, dataName) { }
    }

}
