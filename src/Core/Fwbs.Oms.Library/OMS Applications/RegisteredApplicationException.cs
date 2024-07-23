using System;

namespace FWBS.OMS.Apps
{
    /// <summary>
    /// Generic Registered Application exception.
    /// </summary>
    public class RegisteredApplicationException : OMSException2
    {
        public RegisteredApplicationException(string errCode, string errDescription)
            : base(errCode, errDescription, "")
        {
        }
        public RegisteredApplicationException(string errCode, string errDescription, Exception innerException)
            : base(errCode, errDescription, "", innerException)
        {
        }
        public RegisteredApplicationException(string errCode, string errDescription, Exception innerException, params string[] param)
            : base(errCode, errDescription, "", innerException, false, param)
        {
        }
    }
}
