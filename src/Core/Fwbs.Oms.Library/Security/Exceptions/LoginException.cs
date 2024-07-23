using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// Login related exceptions.
    /// </summary>
    public class LoginException : SecurityException
    {
        public LoginException(HelpIndexes helpID, params string[] param)
            : base(helpID, false, param) { }
        public LoginException(Exception innerException, HelpIndexes helpID, params string[] param)
            : base(innerException, helpID, false, param) { }
    }
}
