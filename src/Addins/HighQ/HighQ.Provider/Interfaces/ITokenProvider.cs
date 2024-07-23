using System;
using FWBS.OMS.HighQ.Models;

namespace FWBS.OMS.HighQ.Interfaces
{
    internal interface ITokenProvider
    {
        event EventHandler<TokensEventArgs> TokensUpdated;
        void Build(int clientId, string clientSecret, string site, string redirectUri);
        void UpdateTokens(string refreshToken, bool fullUpdate = false);
    }
}
