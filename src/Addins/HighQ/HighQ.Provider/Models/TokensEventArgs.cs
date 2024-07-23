using System;

namespace FWBS.OMS.HighQ.Models
{
    internal class TokensEventArgs
    {
        public TokensEventArgs(string accessToken, string refreshToken, DateTime accessTokenExpiresAt,
            DateTime? refreshTokenExpiresAt)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpiresAt = accessTokenExpiresAt;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;
        }

        public string AccessToken { get; }
        public string RefreshToken { get; }
        public DateTime AccessTokenExpiresAt { get; }
        public DateTime? RefreshTokenExpiresAt { get; }
    }
}
