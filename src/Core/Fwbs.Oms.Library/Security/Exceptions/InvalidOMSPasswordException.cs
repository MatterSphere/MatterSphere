using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// User does exist but the password was incorrect.
    /// </summary>
    public sealed class InvalidOMSPasswordException : LoginException
    {
        public InvalidOMSPasswordException()
            : base(HelpIndexes.InvalidOMSPassword) { }
        public InvalidOMSPasswordException(Exception innerException)
            : base(innerException, HelpIndexes.InvalidOMSPassword) { }
    }
}
