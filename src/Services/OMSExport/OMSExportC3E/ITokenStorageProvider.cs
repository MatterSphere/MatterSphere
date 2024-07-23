using System;

namespace FWBS.OMS.OMSEXPORT
{
    interface ITokenStorageProvider
    {
        string LoadToken(out DateTime accessTokenExpiresAt);
        void StoreToken(string accessToken, DateTime accessTokenExpiresAt);
    }
}
