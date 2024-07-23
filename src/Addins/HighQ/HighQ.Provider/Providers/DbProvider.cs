using System;
using System.Data;
using System.Text;
using FWBS.OMS.Data;
using FWBS.OMS.HighQ.Interfaces;
using FWBS.OMS.HighQ.Models;

namespace FWBS.OMS.HighQ.Providers
{
    internal class DbProvider : IDbProvider
    {
        private const string SITE_CODE = "HQ";
        private const string CLIENT_NOT_FOUND = "HQCLNTNTFND";
        private const string DOCUMENT_NOT_FOUND = "HQDOCNTFND";

        private readonly IConnection _connection;

        public DbProvider()
        {
            _connection = Session.CurrentSession.CurrentConnection;
        }

        public TokenDetails GetUserTokens(int userId)
        {
            var ep = new DataTableExecuteParameters
            {
                CommandType = CommandType.StoredProcedure,
                Sql = "[dbo].[GetTokens]",
                Table = "Tokens",
                SchemaOnly = false
            };
            ep.Parameters.Add(_connection.CreateParameter("@siteCode", SITE_CODE));
            ep.Parameters.Add(_connection.CreateParameter("@userId", userId));
            var dataTable = _connection.Execute(ep);
            if (dataTable.Rows.Count > 0)
            {
                try
                {
                    string password = string.Format("{0}@{1}", userId, SITE_CODE);
                    string accessToken = Encoding.UTF8.GetString(EncryptionV2.Decrypt(Convert.FromBase64String(dataTable.Rows[0]["accessToken"].ToString()), password));
                    string refreshToken = Encoding.UTF8.GetString(EncryptionV2.Decrypt(Convert.FromBase64String(dataTable.Rows[0]["refreshToken"].ToString()), password));
                    DateTime accessTokenExpiresAt = Convert.ToDateTime(dataTable.Rows[0]["accessTokenExpiresAt"]);
                    DateTime? refreshTokenExpiresAt = Convert.IsDBNull(dataTable.Rows[0]["refreshTokenExpiresAt"]) ? (DateTime?)null : Convert.ToDateTime(dataTable.Rows[0]["refreshTokenExpiresAt"]);

                    return new TokenDetails(accessToken, refreshToken, accessTokenExpiresAt, refreshTokenExpiresAt);
                }
                catch { }
            }
            return new TokenDetails();
        }

        public void SetUserTokens(int userId, string accessToken, string refreshToken, DateTime accessTokenExpiresAt, DateTime? refreshTokenExpiresAt)
        {
            var ep = new DataTableExecuteParameters
            {
                CommandType = CommandType.StoredProcedure,
                Sql = "[dbo].[SetTokens]",
                Table = "Tokens",
                SchemaOnly = false
            };
            string password = string.Format("{0}@{1}", userId, SITE_CODE);
            ep.Parameters.Add(_connection.CreateParameter("@siteCode", SITE_CODE));
            ep.Parameters.Add(_connection.CreateParameter("@userId", userId));
            ep.Parameters.Add(_connection.CreateParameter("@accessToken", Convert.ToBase64String(EncryptionV2.Encrypt(Encoding.UTF8.GetBytes(accessToken), password))));
            ep.Parameters.Add(_connection.CreateParameter("@accessTokenExpiresAt", accessTokenExpiresAt));
            ep.Parameters.Add(_connection.CreateParameter("@refreshToken", Convert.ToBase64String(EncryptionV2.Encrypt(Encoding.UTF8.GetBytes(refreshToken), password))));
            ep.Parameters.Add(_connection.CreateParameter("@refreshTokenExpiresAt", refreshTokenExpiresAt));

            _connection.Execute(ep);
        }

        public int GetOMSFileEntityId(long clientId)
        {
            var ep = new DataTableExecuteParameters
            {
                CommandType = CommandType.StoredProcedure,
                Sql = "[highq].[GetOmsFileEntityKey]",
                Table = "OmsFile",
                SchemaOnly = false
            };
            ep.Parameters.Add(_connection.CreateParameter("@clientId", clientId));
            var dataTable = _connection.Execute(ep);

            if (dataTable.Rows.Count == 0)
            {
                var message = Session.CurrentSession.Resources.GetResource(CLIENT_NOT_FOUND,
                    "The Client with ID=%1% was not found", "", clientId.ToString()).Text;
                throw new DataException(message);
            }

            int id;
            Int32.TryParse(dataTable.Rows[0]["omsFileEntityId"].ToString(), out id);

            return id;
        }

        public DocumentInfo GetDocumentInfo(long docId)
        {
            var ep = new DataTableExecuteParameters
            {
                CommandType = CommandType.StoredProcedure,
                Sql = "[highq].[GetDocumentInfo]",
                Table = "Document",
                SchemaOnly = false
            };
            ep.Parameters.Add(_connection.CreateParameter("@docId", docId));
            var dataTable = _connection.Execute(ep);

            if (dataTable.Rows.Count == 0)
            {
                var message = Session.CurrentSession.Resources.GetResource(DOCUMENT_NOT_FOUND,
                    "The Document with ID=%1% was not found", "", docId.ToString()).Text;
                throw new DataException(message);
            }

            long clientId;
            Int64.TryParse(dataTable.Rows[0]["clID"].ToString(), out clientId);
            
            return new DocumentInfo(
                clientId,
                dataTable.Rows[0]["clNo"].ToString(),
                dataTable.Rows[0]["fileNo"].ToString(),
                $"{dataTable.Rows[0]["dirPath"]}\\{dataTable.Rows[0]["docFileName"]}",
                dataTable.Rows[0]["docDesc"].ToString());
        }

        public void LogDocumentUpload(long docId, int usrId, int hqFileId)
        {
            var ep = new DataTableExecuteParameters
            {
                CommandType = CommandType.StoredProcedure,
                Sql = "[highq].[LogDocumentUpload]",
                SchemaOnly = false
            };

            ep.Parameters.Add(_connection.CreateParameter("@docID", docId));
            ep.Parameters.Add(_connection.CreateParameter("@usrID", usrId));
            ep.Parameters.Add(_connection.CreateParameter("@hqFileID", hqFileId));

            _connection.Execute(ep);
        }
    }
}
