using System;

namespace FWBS.OMS.HighQ.Models
{
    internal class TokenDetails
    {
        public TokenDetails()
        {

        }

        public TokenDetails(string accessToken, string refreshToken, DateTime accessTokenExpiresAt, DateTime? refreshTokenExpiresAt = null)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            AccessTokenExpiresAt = accessTokenExpiresAt;
            RefreshTokenExpiresAt = refreshTokenExpiresAt;
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }

        internal bool IsEmpty
        {
            get { return AccessToken == null; }
        }

        internal TokenStatus GetTokensStatus()
        {
            if (!RefreshTokenExpiresAt.HasValue || RefreshTokenExpiresAt < DateTime.UtcNow)
            {
                return TokenStatus.NeedFullUpdate;
            }

            if (AccessTokenExpiresAt < DateTime.UtcNow)
            {
                return TokenStatus.NeedAccessTokenUpdate;
            }

            return TokenStatus.Actual;
        }
    }
}
