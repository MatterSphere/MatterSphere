using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// User does not exist within OMS databse.
    /// </summary>
    public  sealed class InvalidOMSUserException : LoginException
    {
        public InvalidOMSUserException(string userName)
            : base(HelpIndexes.InvalidOMSUser, userName) { }

        public InvalidOMSUserException(Exception innerException, string userName)
            : base(innerException, HelpIndexes.InvalidOMSUser, userName) { }

    }
}
