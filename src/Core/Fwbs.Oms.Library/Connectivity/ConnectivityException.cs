using System;

namespace FWBS.OMS.Connectivity
{
    public class ConnectivityException : Exception
    {
        public ConnectivityException() : base()
        {
        }

        public ConnectivityException(string message) : base(message)
        {
        }

        public ConnectivityException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}
