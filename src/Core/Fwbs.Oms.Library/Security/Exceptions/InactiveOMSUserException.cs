using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// User name and password match with an OMS user but the user is marked as inactive.
    /// </summary>
    public sealed class InactiveOMSUserException : LoginException
    {
        public InactiveOMSUserException(string userName)
            : base(HelpIndexes.InactiveOMSUser, userName) { }

        public InactiveOMSUserException(string userName, Exception innerException)
            : base(innerException, HelpIndexes.InactiveOMSUser, userName) { }
    }
}
