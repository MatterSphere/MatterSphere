using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace FWBS.OMS.OMSEXPORT
{
    using Models;

    class ApiRequester
    {
        private readonly string _session = Guid.NewGuid().ToString();
        private readonly RestClient _client;
        private readonly ICredentials _credentials;
        private readonly string _authority;
        private readonly ITokenStorageProvider _tokenStorageProvider;
        private DateTime _tokenExpireTime;
        private readonly bool _debug;
        
        private const int AudienceIndex = 0;
        private const int ClientIdIndex = 1;
        private const int ClientSecretIndex = 2;
        private const int TenantIdIndex = 3;
        private const int InstanceIdIndex = 4;

        public ApiRequester(string apiUrl, KeyValuePair<string, string> parameters, ITokenStorageProvider tokenStorageProvider = null, ICredentials credentials = null, bool debug = false)
        {
            _credentials = credentials ?? CredentialCache.DefaultCredentials;
            _debug = debug;

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            if (!string.IsNullOrWhiteSpace(parameters.Key))
            {
                string[] value = System.Text.Encoding.UTF8.GetString(
                    EncryptionV2.Decrypt(
                        Convert.FromBase64String(parameters.Value), string.Concat(Environment.MachineName, ":", "AAD")))
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                
                _credentials = new NetworkCredential(value[ClientIdIndex], value[ClientSecretIndex], value[AudienceIndex] + " " + value[TenantIdIndex] + " " + value[InstanceIdIndex]);
                var instanceId = value[InstanceIdIndex];
                
                // Authority - customer AAD specific URL
                _authority = "https://login.microsoftonline.com/" + instanceId + "/";

                _client = new RestClient(apiUrl) { Timeout = System.Threading.Timeout.Infinite };
                
                _tokenStorageProvider = tokenStorageProvider;
                if (_tokenStorageProvider != null)
                {
                    string accessToken = _tokenStorageProvider.LoadToken(out _tokenExpireTime);
                    if (!string.IsNullOrEmpty(accessToken))
                        _client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(accessToken);
                }
            }
            else
            {
                _client = new RestClient(apiUrl) { Timeout = System.Threading.Timeout.Infinite };
                _client.Authenticator = new RestSharp.Authenticators.NtlmAuthenticator(_credentials);
            }
        }

        private void InitAAD()
        {
            DateTime now = DateTime.UtcNow;
            if (_tokenExpireTime < now)
            {
                TokenResponse t = Authorize();
                _client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(t.AccessToken);
                _tokenExpireTime = now.AddSeconds(t.ExpiresIn - 120);
                if (_tokenStorageProvider != null)
                    _tokenStorageProvider.StoreToken(t.AccessToken, _tokenExpireTime);
            }
        }

        public InfoResponse Info()
        {
            var request = CreateRequest("v1/info", Method.GET);
            return ExecuteRequest<InfoResponse>(request);
        }

        public GenericResponse CreatePersonContact(string contact)
        {
            var request = CreateRequest("v1/entity/person", Method.POST, contact);
            return ExecuteRequest<GenericResponse>(request);
        }

        public GenericResponse CreateOrgContact(string contact)
        {
            var request = CreateRequest("v1/entity/organization", Method.POST, contact);
            return ExecuteRequest<GenericResponse>(request);
        }

        public GenericResponse UpdateContact(string contact)
        {
            var request = CreateRequest("v1/entity", Method.PATCH, contact);
            return ExecuteRequest<GenericResponse>(request);
        }

        public FindGenericResponse<FindContactAttribute> FindContact(FindContactRequest findContact)
        {
            var request = CreateRequest("v1/find/query", Method.POST, JsonConvert.SerializeObject(findContact));
            return ExecuteRequest<FindGenericResponse<FindContactAttribute>>(request);
        }

        public GetClientResponse GetClient(string clientId)
        {
            var request = CreateRequest("v1/client?ClientId=" + clientId, Method.GET);
            return ExecuteRequest<GetClientResponse>(request);
        }

        public GetClientResponse GetClient(int clientIndex)
        {
            var request = CreateRequest("v1/client?ClientIndex=" + clientIndex.ToString(System.Globalization.CultureInfo.InvariantCulture), Method.GET);
            return ExecuteRequest<GetClientResponse>(request);
        }

        public GenericResponse CreateClient(string client)
        {
            var request = CreateRequest("v1/client", Method.POST, client);
            return ExecuteRequest<GenericResponse>(request);
        }

        public GenericResponse UpdateClient(string client)
        {
            var request = CreateRequest("v1/client", Method.PATCH, client);
            return ExecuteRequest<GenericResponse>(request);
        }

        public FindGenericResponse<FindInvoiceAttachmentAttribute> FindInvoiceAttachments(FindInvoiceAttachmentRequest findInvoiceAttachments)
        {
            var request = CreateRequest("v1/find/query", Method.POST, JsonConvert.SerializeObject(findInvoiceAttachments));
            return ExecuteRequest<FindGenericResponse<FindInvoiceAttachmentAttribute>>(request);
        }

        public BaseResponse GetAttachment(string attachmentId)
        {
            var request = CreateRequest("v1/attachment/file?AttachmentId=" + attachmentId, Method.GET);
            return ExecuteRequest<BaseResponse>(request);
        }

        public GetMatterResponse GetMatter(string matterId)
        {
            var request = CreateRequest("v1/matter?MatterId=" + matterId, Method.GET);
            return ExecuteRequest<GetMatterResponse>(request);
        }

        public GetMatterResponse GetMatter(int matterIndex)
        {
            var request = CreateRequest("v1/matter?MattIndex=" + matterIndex.ToString(System.Globalization.CultureInfo.InvariantCulture), Method.GET);
            return ExecuteRequest<GetMatterResponse>(request);
        }

        public GenericResponse CreateMatter(string matter)
        {
            var request = CreateRequest("v1/matter", Method.POST, matter);
            return ExecuteRequest<GenericResponse>(request);
        }

        public GenericResponse UpdateMatter(string matter)
        {
            var request = CreateRequest("v1/matter", Method.PATCH, matter);
            return ExecuteRequest<GenericResponse>(request);
        }

        public GenericResponse CreateTimeCard(string timeCard)
        {
            var request = CreateRequest("v1/time/pending", Method.POST, timeCard);
            return ExecuteRequest<GenericResponse>(request);
        }

        public GenericResponse CreateCostCard(string costCard)
        {
            var request = CreateRequest("v1/cost/pending", Method.POST, costCard);
            return ExecuteRequest<GenericResponse>(request);
        }

        public GenericResponse CreateNBISearch(string search)
        {
            var request = CreateRequest("v1/NBIWF", Method.POST, search);
            return ExecuteRequest<GenericResponse>(request);
        }

        public GetNBISearchResponse GetNBISearch(string searchId)
        {
            var request = CreateRequest("v1/NBIWF?CftNBISearchID=" + searchId, Method.GET);
            return ExecuteRequest<GetNBISearchResponse>(request);
        }

        public GetUserResponse GetUserInfo(string networkAlias)
        {
            var request = CreateRequest("v1/user?networkalias=" + networkAlias, Method.GET);
            return ExecuteRequest<GetUserResponse>(request);
        }

        public GetMeResponse GetMeInfo()
        {
            var request = CreateRequest("v1/me", Method.GET);
            return ExecuteRequest<GetMeResponse>(request);
        }

        private TokenResponse Authorize()
        {
            var networkCredential = (NetworkCredential)_credentials;

            IRestRequest request = new RestRequest("oauth2/v2.0/token", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");
            request.AddParameter("scope", GetAudience(networkCredential));
            request.AddParameter("client_id", networkCredential.UserName);
            request.AddParameter("client_secret", networkCredential.Password);
            request.AddParameter("grant_type", "client_credentials");

            IRestClient client = new RestClient(_authority)
            {
                Timeout = System.Threading.Timeout.Infinite
            };
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
                return JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            else if (response.ErrorException != null)
                throw response.ErrorException;
            else if (response.ContentType.Contains("json"))
                throw new Exception(JsonConvert.DeserializeObject<TokenResponse>(response.Content).ErrorDescription);
            else
                throw new WebException(response.StatusDescription);
        }

        private T ExecuteRequest<T>(IRestRequest request) where T : BaseResponse, new()
        {
            T responseObj;

            if (_authority != null)
                InitAAD();

            IRestResponse response = _client.Execute(request);

            if (response.IsSuccessful && response.StatusCode != HttpStatusCode.NoContent)
            {
                if (response.ContentType.Contains("json"))
                    responseObj = JsonConvert.DeserializeObject<T>(response.Content);
                else
                    responseObj = new T { RawBytes = response.RawBytes };
            }
            else
            {
                string errorMessage = string.Format("An error occurred while processing the request: {0}.",
                    response.ErrorMessage ?? response.StatusDescription);

                string headerMessage = response.Headers.FirstOrDefault(x => x.Name == "X-3E-Message")?.Value as string;
                if (!string.IsNullOrWhiteSpace(headerMessage))
                    errorMessage += "\r\n" + WebUtility.UrlDecode(headerMessage);

                if (response.StatusCode == HttpStatusCode.Unauthorized && _authority != null && request.Attempts == 1)
                {
                    _tokenExpireTime = DateTime.MinValue;
                    request.Parameters.RemoveAll(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase));
                    return ExecuteRequest<T>(request);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest && response.ContentType.Contains("json") && !string.IsNullOrWhiteSpace(response.Content))
                {
                    responseObj = JsonConvert.DeserializeObject<T>(response.Content);
                    responseObj.ErrorMessage = errorMessage;
                }
                else
                {
                    responseObj = new T { ErrorMessage = errorMessage };
                }
            }

            responseObj.Dump = DumpResponse(response, responseObj.RawBytes == null);
            return responseObj;
        }

        private IRestRequest CreateRequest(string endPoint, Method method, string jsonBody = null)
        {
            IRestRequest request = new RestRequest(endPoint, method);
            request.AddHeader("Accept-Language", "DEFAULT");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("accept", "application/json");
            request.AddHeader("X-3E-SessionId", _session);
            request.AddHeader("X-3E-InstanceId", GetTenantId((NetworkCredential)_credentials));
            
            if (jsonBody != null)
                request.AddJsonBody(jsonBody);

            return request;
        }

        private string DumpResponse(IRestResponse response, bool includeContent)
        {
            JObject dump = JObject.FromObject(new
            {
                code = response.StatusCode,
                status = response.ErrorMessage ?? response.StatusDescription,
                message = WebUtility.UrlDecode(response.Headers.FirstOrDefault(x => x.Name == "X-3E-Message")?.Value as string)
            });
            if (_debug && includeContent && !string.IsNullOrWhiteSpace(response.Content))
            {
                JObject body = response.ContentType.Contains("json")
                    ? JObject.Parse(response.Content)
                    : JObject.FromObject(new { content = response.Content });
                dump.Merge(body);
            }
            return dump.ToString(Formatting.None);
        }

        private const int CredentialsAudienceIdIndex = 0;
        private const int CredentialsTenantIdIndex = 1;
        private const int CredentialsInstanceIdIndex = 2;

        private string GetAudience(NetworkCredential credentials)
        {
            return GetFromCredentials(credentials, CredentialsAudienceIdIndex);
        }

        private string GetTenantId(NetworkCredential credentials)
        {
            return GetFromCredentials(credentials, CredentialsTenantIdIndex);
        }

        private string GetInstanceId(NetworkCredential credentials)
        {
            return GetFromCredentials(credentials, CredentialsInstanceIdIndex);
        }

        private string GetFromCredentials(NetworkCredential credentials, int index)
        {
            if (credentials == null || string.IsNullOrEmpty(credentials.Domain))
            {
                return string.Empty;
            }

            var result = string.Empty;
            var values = credentials.Domain.Split(' ');
            if (values.Length > index)
            {
                result = values[index];
            }

            return result;
        }
    }
}
