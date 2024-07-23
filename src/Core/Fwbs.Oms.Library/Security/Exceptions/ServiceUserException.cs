using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// User name and password match with an OMS user but the user is marked as inactive.
    /// </summary>
    public sealed class ServiceUserException : LoginException
    {
        public ServiceUserException(string userName)
            : base(HelpIndexes.ServiceUser, userName) { }

        public ServiceUserException(string userName, Exception innerException)
            : base(innerException, HelpIndexes.ServiceUser, userName) { }
    }
}
