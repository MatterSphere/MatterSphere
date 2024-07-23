using System;
using FWBS.OMS.HighQ.Models;

namespace FWBS.OMS.HighQ.Interfaces
{
    internal interface IDbProvider
    {
        TokenDetails GetUserTokens(int userId);
        void SetUserTokens(int userId, string accessToken, string refreshToken, DateTime accessTokenExpiresAt, DateTime? refreshTokenExpiresAt);
        int GetOMSFileEntityId(long clientId);
        DocumentInfo GetDocumentInfo(long docId);
        void LogDocumentUpload(long docId, int usrId, int hqFileId);
    }
}
