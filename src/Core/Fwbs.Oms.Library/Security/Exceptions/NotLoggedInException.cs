using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// Warns the user that they are not logged in if items within session are trying
    /// to be used when the user is not logged in.
    /// </summary>
    public sealed class NotLoggedInException : LoginException
    {
        public NotLoggedInException()
            : base(HelpIndexes.NotLoggedIn) { }

        public NotLoggedInException(Exception innerException)
            : base(innerException, HelpIndexes.NotLoggedIn) { }
    }
}
