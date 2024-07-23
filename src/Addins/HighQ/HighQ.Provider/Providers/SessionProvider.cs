using System;
using FWBS.OMS.HighQ.Interfaces;

namespace FWBS.OMS.HighQ.Providers
{
    internal class SessionProvider : ISessionProvider
    {
        public int GetUserId()
        {
            return Session.CurrentSession.CurrentUser.ID;
        }

        public string GetSpecificData(string parameter)
        {
            return Convert.ToString(Session.CurrentSession.GetSpecificData(parameter));
        }
    }
}
