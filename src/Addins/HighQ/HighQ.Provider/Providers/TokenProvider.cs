using System;
using System.Net;
using System.Threading;
using FWBS.OMS.HighQ.Interfaces;
using FWBS.OMS.HighQ.Models;
using FWBS.OMS.HighQ.Models.Response;
using Newtonsoft.Json;
using RestSharp;

namespace FWBS.OMS.HighQ.Providers
{
    internal class TokenProvider : ITokenProvider
    {
        private int _clientId;
        private string _clientSecret;
        private string _site;
        private string _redirectUri;
        private readonly IRestClient _restClient;
        private string _authorizationCode;

        public TokenProvider()
        {
            _restClient = new RestClient();
        }

        public TokenProvider(IRestClient restClient)
        {
            _restClient = restClient;
        }

        #region ITokenProvider

        public event EventHandler<TokensEventArgs> TokensUpdated;

        public void Build(int clientId, string clientSecret, string site, string redirectUri)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _site = site;
            _redirectUri = redirectUri;
        }

        public void UpdateTokens(string refreshToken, bool fullUpdate = false)
        {
            CheckHighQApi();

            if (fullUpdate)
            {
                Thread oauthThread = new Thread(new ThreadStart(GetAuthorizationCode));
                oauthThread.SetApartmentState(ApartmentState.STA);
                oauthThread.Start();
                oauthThread.Join();

                var tokens = GetTokenDetails(_authorizationCode);
                TokensUpdated?.Invoke(this, new TokensEventArgs(tokens.AccessToken, tokens.RefreshToken, tokens.AccessTokenExpiresAt, tokens.RefreshTokenExpiresAt));
                return;
            }

            var updatedTokens = UpdateToken(refreshToken);
            TokensUpdated?.Invoke(this, new TokensEventArgs(updatedTokens.AccessToken, updatedTokens.RefreshToken, updatedTokens.AccessTokenExpiresAt, updatedTokens.RefreshTokenExpiresAt));
        }

        #endregion

        #region Private methods

        private void GetAuthorizationCode()
        {
            using (var auth = new CredentialsForm(_clientId, _site, _redirectUri))
            {
                auth.GetTokens();
                _authorizationCode = auth.AuthorizationCode;
            }
        }

        private TokenDetails GetTokenDetails(string authorizationCode)
        {
            return GetTokens(
                $"grant_type=authorization_code&client_id={_clientId}&client_secret={_clientSecret}&code={authorizationCode}");
        }

        private TokenDetails UpdateToken(string refreshToken)
        {
            return GetTokens(
                $"client_id={_clientId}&client_secret={_clientSecret}&grant_type=refresh_token&refresh_token={refreshToken}");
        }

        private TokenDetails GetTokens(string body)
        {
            var date = DateTime.UtcNow;
            var request = new RestRequest(Method.POST);
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Content-Length", body.Length.ToString());
            _restClient.BaseUrl = new Uri($"{_site}/api/oauth2/token");
            var response = _restClient.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw ErrorHandler.GetError(response);
            }

            var model = JsonConvert.DeserializeObject<TokensMapper>(response.Content);
            return new TokenDetails(
                model.AccessToken,
                model.RefreshToken,
                date.AddSeconds(model.AccessTokenExpiresIn),
                model.RefreshTokenExpiresIn.HasValue
                    ? date.AddSeconds(model.RefreshTokenExpiresIn.Value)
                    : (DateTime?)null);
        }

        private void CheckHighQApi()
        {
            _restClient.BaseUrl = new Uri($"{_site}/authorize.action");
            var response = _restClient.Get(new RestRequest());
            if (!response.IsSuccessful)
            {
                throw ErrorHandler.GetError(response);
            }
        }

        #endregion
    }
}
