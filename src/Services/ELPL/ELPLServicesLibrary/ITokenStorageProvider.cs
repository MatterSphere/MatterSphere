using System;

namespace ELPLServicesLibrary
{
    public interface ITokenStorageProvider
    {
        TokenDetails LoadToken(int? msUserId);
        void StoreToken(int? msUserId, TokenDetails tokenDetails);
    }

    public class TokenDetails
    {
        public string AccessToken { get; set; }
        public DateTime AccessTokenExpiresAt { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }

        public bool IsAccessTokenExpired
        {
            get { return AccessTokenExpiresAt <= DateTime.UtcNow; }
        }

        public bool IsRefreshTokenExpired
        {
            get { return RefreshTokenExpiresAt <= DateTime.UtcNow; }
        }
    }
}
