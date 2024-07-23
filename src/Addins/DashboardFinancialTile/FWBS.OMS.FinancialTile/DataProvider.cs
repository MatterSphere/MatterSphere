using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FWBS.OMS.Data;
using FWBS.OMS.FinancialTile.Models;
using Newtonsoft.Json;
using RestSharp;

namespace FWBS.OMS.FinancialTile
{
    class DataProvider
    {
        private readonly RestClient _client;
        private readonly ICredentials _credentials;
        private readonly string _endpoint;
        private readonly string _session;
        private readonly string _authority;
        private DateTime _tokenExpireTime;
        private const string SITE_CODE = "3E";

        public DataProvider(string baseUrl, string endpoint, ICredentials credentials, string env = null)
        {
            _endpoint = endpoint;
            _session = Guid.NewGuid().ToString();
            _credentials = credentials ?? CredentialCache.DefaultCredentials;
            
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            if (!string.IsNullOrEmpty(env))
            {
                _client = new RestClient(baseUrl) { Timeout = Timeout.Infinite };

                var instanceId = GetInstanceId((NetworkCredential)_credentials);
                // Authority - customer AAD specific URL
                _authority = "https://login.microsoftonline.com/" + instanceId + "/";
                string accessToken = LoadToken(out _tokenExpireTime);
                if (!string.IsNullOrEmpty(accessToken))
                    _client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(accessToken);
            }
            else
            {
                _client = new RestClient(baseUrl) { Timeout = Timeout.Infinite };
                _client.Authenticator = new RestSharp.Authenticators.NtlmAuthenticator(_credentials);
            }
        }

        private string LoadToken(out DateTime accessTokenExpiresAt)
        {
            string accessToken = null;
            accessTokenExpiresAt = DateTime.MinValue;
            var connection = Session.CurrentSession.CurrentConnection;
            var ep = new DataTableExecuteParameters
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                Sql = "[dbo].[GetTokens]",
                Table = "Tokens",
                SchemaOnly = false
            };
            ep.Parameters.Add(connection.CreateParameter("@siteCode", SITE_CODE));
            ep.Parameters.Add(connection.CreateParameter("@userId", DBNull.Value));
            var dataTable = connection.Execute(ep);
            if (dataTable.Rows.Count > 0)
            {
                try
                {
                    accessToken = Encoding.UTF8.GetString(
                        EncryptionV2.Decrypt(Convert.FromBase64String(dataTable.Rows[0]["accessToken"].ToString()), string.Format("{0}@{1}", typeof(DBNull).Name, SITE_CODE)));
                    accessTokenExpiresAt = DateTime.SpecifyKind(Convert.ToDateTime(dataTable.Rows[0]["accessTokenExpiresAt"]), DateTimeKind.Utc);
                }
                catch { }
            }
            return accessToken;
        }

        private void StoreToken(string accessToken, DateTime accessTokenExpiresAt)
        {
            var connection = Session.CurrentSession.CurrentConnection;
            var ep = new DataTableExecuteParameters
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                Sql = "[dbo].[SetTokens]",
                Table = "Tokens",
                SchemaOnly = false
            };
            ep.Parameters.Add(connection.CreateParameter("@siteCode", SITE_CODE));
            ep.Parameters.Add(connection.CreateParameter("@userId", DBNull.Value));
            ep.Parameters.Add(connection.CreateParameter("@accessToken",
                Convert.ToBase64String(EncryptionV2.Encrypt(Encoding.UTF8.GetBytes(accessToken), string.Format("{0}@{1}", typeof(DBNull).Name, SITE_CODE)))));
            ep.Parameters.Add(connection.CreateParameter("@accessTokenExpiresAt", accessTokenExpiresAt));
            connection.Execute(ep);
        }

        private void InitAAD()
        {
            DateTime now = DateTime.UtcNow;
            if (_tokenExpireTime < now)
            {
                TokenResponse t = Authorize();
                _client.Authenticator = new RestSharp.Authenticators.JwtAuthenticator(t.AccessToken);
                _tokenExpireTime = now.AddSeconds(t.ExpiresIn - 120);
                StoreToken(t.AccessToken, _tokenExpireTime);
            }
        }

        private TokenResponse Authorize()
        {
            IRestRequest request = new RestRequest("oauth2/v2.0/token", Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Accept", "application/json");
            request.AddParameter("scope", GetAudience((NetworkCredential)_credentials));
            request.AddParameter("client_id", ((NetworkCredential)_credentials).UserName);
            request.AddParameter("client_secret", ((NetworkCredential)_credentials).Password);
            request.AddParameter("grant_type", "client_credentials");

            IRestClient client = new RestClient(_authority) { Timeout = Timeout.Infinite };
            IRestResponse response = client.Execute(request);

            if (response.IsSuccessful)
                return JsonConvert.DeserializeObject<TokenResponse>(response.Content);
            else if (response.ErrorException != null)
                throw response.ErrorException;
            else if (response.ContentType.Contains("json"))
                throw new Exception("AAD: " + JsonConvert.DeserializeObject<TokenResponse>(response.Content).ErrorDescription);
            else
                throw new WebException(Session.CurrentSession.Resources.GetMessage("ERRREQUEST", "An error occurred while processing the request: %1%.", "", response.StatusDescription).Text);
        }

        public async void GetClientFinancialItemsAsync(string sortedColumn, SortOrder order, int page, int size, Action<List<FinancialRow>, int> callback, Action<Exception> errorCallback, CancellationToken token)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    var request = CreateClientRequest(sortedColumn, order, page, size);
                    SendRequest(request, callback, token);
                }, token);
            }
            catch (OperationCanceledException)
            {
                
            }
            catch(Exception e)
            {
                errorCallback(e);
            }
        }
        
        public async void GetMatterFinancialItemsAsync(string sortedColumn, SortOrder order, int page, int size, Action<List<FinancialRow>, int> callback, Action<Exception> errorCallback, CancellationToken token)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    var request = CreateMatterRequest(sortedColumn, order, page, size);
                    SendRequest(request, callback, token);
                }, token);
            }
            catch (OperationCanceledException)
            {
                
            }
            catch (Exception e)
            {
                errorCallback(e);
            }
        }

        private RestRequest CreateClientRequest(string sortedColumn, SortOrder order, int page, int size)
        {
            var body = new ClientManagementRequest
            {
                RowsRequest = new ClientManagementRequest.RowsRequestInfo
                {
                    UseCache = true,
                    StartRow = (page-1) * size,
                    RowCount = size,
                    ColumnCount = 100
                },
                Presentation = new ClientManagementRequest.PresentationInfo
                {
                    AcrossDimensions = new ClientManagementRequest.PresentationInfo.AcrossDimension[]
                    {
                        new ClientManagementRequest.PresentationInfo.AcrossDimension
                        {
                            Type = 2,
                            MetricAttributes = new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute[]
                            {
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.FormatableMetricAttribute("WIPHours")
                                {
                                    Format = new ClientManagementRequest.PresentationInfo.AcrossDimension.FormatableMetricAttribute.FormatInfo
                                    {
                                        CurrencyDisplaySymbol = 3,
                                        CurrencyPositiveFormat = 5,
                                        CurrencyNegativeFormat = 17,
                                        CurrencyNegativeColor = string.Empty,
                                        CurrencyDisplayGroupSeperator = 3,
                                        CurrencyZeroFormat = "-",
                                        NumberNegativeFormat = 6,
                                        NumberNegativeColor = string.Empty,
                                        NumberDisplayGroupSeperator = 3,
                                        NumberZeroFormat = "-"
                                    }
                                },
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute("WIPTime"),
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute("WIPCosts"),
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute("WIPCharges"),
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute("WIPTotal")
                            }
                        }
                    },
                    DownDimensions = new ClientManagementRequest.PresentationInfo.DownDimension[]
                    {
                        new ClientManagementRequest.PresentationInfo.DownDimension
                        {
                            Id = "ClientRel",
                            Type = 1,
                            ShowSubTotalsWithSingleRows = true,
                            ShowGroupWithRowsAllZeros = true,
                            ShowRowsWithAllZeros = true,
                            Indent = true,
                            DisplayAttributes = new string[]
                            {
                                "Client|Number",
                                "Client|DisplayName"
                            },
                            SortAttributes = new ClientManagementRequest.PresentationInfo.DownDimension.SortAttribute[]
                            {
                                new ClientManagementRequest.PresentationInfo.DownDimension.SortAttribute(
                                    "Client|Number",
                                    sortedColumn == null && order == SortOrder.Descending ? -1 : 1),
                            }
                        }
                    },
                    PageDimensions = new ClientManagementRequest.PresentationInfo.PageDimension[]
                    {
                        new ClientManagementRequest.PresentationInfo.PageDimension
                        {
                            Type = 3,
                            Runs = new string[]
                            {
                                "rel-0"
                            },
                            ShowTotals = true
                        }
                    },
                    BoundId = "MxClientManagement",
                    BoundType = 13
                },
                UseExistingRun = true
            };

            if(sortedColumn != null)
            {
                body.Presentation.AcrossDimensions[0].SortAttributes =
                    new ClientManagementRequest.PresentationInfo.AcrossDimension.SortAttribute[]
                    {
                        new ClientManagementRequest.PresentationInfo.AcrossDimension.SortAttribute(sortedColumn,
                            order == SortOrder.Descending),
                    };
            }

            var json = JsonConvert.SerializeObject(body);
            return CreateRequest(json);
        }

        private RestRequest CreateMatterRequest(string sortedColumn, SortOrder order, int page, int size)
        {
            var body = new ClientManagementRequest
            {
                RowsRequest = new ClientManagementRequest.RowsRequestInfo
                {
                    UseCache = true,
                    StartRow = (page - 1) * size,
                    RowCount = size,
                    ColumnCount = 100
                },
                Presentation = new ClientManagementRequest.PresentationInfo
                {
                    AcrossDimensions = new ClientManagementRequest.PresentationInfo.AcrossDimension[]
                    {
                        new ClientManagementRequest.PresentationInfo.AcrossDimension
                        {
                            Type = 2,
                            MetricAttributes = new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute[]
                            {
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.FormatableMetricAttribute("WIPHours")
                                {
                                    Format = new ClientManagementRequest.PresentationInfo.AcrossDimension.FormatableMetricAttribute.FormatInfo
                                    {
                                        CurrencyDisplaySymbol = 3,
                                        CurrencyPositiveFormat = 5,
                                        CurrencyNegativeFormat = 17,
                                        CurrencyNegativeColor = string.Empty,
                                        CurrencyDisplayGroupSeperator = 3,
                                        CurrencyZeroFormat = "-",
                                        NumberNegativeFormat = 6,
                                        NumberNegativeColor = string.Empty,
                                        NumberDisplayGroupSeperator = 3,
                                        NumberZeroFormat = "-"
                                    }
                                },
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute("WIPTime"),
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute("WIPCosts"),
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute("WIPCharges"),
                                new ClientManagementRequest.PresentationInfo.AcrossDimension.MetricAttribute("WIPTotal")
                            }
                        }
                    },
                    DownDimensions = new ClientManagementRequest.PresentationInfo.DownDimension[]
                    {
                        new ClientManagementRequest.PresentationInfo.DownDimension
                        {
                            Id = "MatterRel",
                            Type = 1,
                            ShowSubTotalsWithSingleRows = true,
                            ShowGroupWithRowsAllZeros = true,
                            ShowRowsWithAllZeros = true,
                            Indent = true,
                            DisplayAttributes = new string[]
                            {
                                "MatterLkUp1|Number",
                                "MatterLkUp1|DisplayName"
                            },
                            SortAttributes = new ClientManagementRequest.PresentationInfo.DownDimension.SortAttribute[]
                            {
                                new ClientManagementRequest.PresentationInfo.DownDimension.SortAttribute(
                                    "MatterLkUp1|Number",
                                    sortedColumn == null && order == SortOrder.Descending ? -1 : 1),
                            }
                        }
                    },
                    PageDimensions = new ClientManagementRequest.PresentationInfo.PageDimension[]
                    {
                        new ClientManagementRequest.PresentationInfo.PageDimension
                        {
                            Type = 3,
                            Runs = new string[]
                            {
                                "rel-0"
                            },
                            ShowTotals = true
                        }
                    },
                    BoundId = "MxClientManagement",
                    BoundType = 13
                },
                UseExistingRun = true
            };

            if (sortedColumn != null)
            {
                body.Presentation.AcrossDimensions[0].SortAttributes =
                    new ClientManagementRequest.PresentationInfo.AcrossDimension.SortAttribute[]
                    {
                        new ClientManagementRequest.PresentationInfo.AcrossDimension.SortAttribute(sortedColumn,
                            order == SortOrder.Descending),
                    };
            }

            var json = JsonConvert.SerializeObject(body);
            return CreateRequest(json);
        }

        private RestRequest CreateRequest(string json)
        {
            var request = new RestRequest(_endpoint, Method.POST);
            request.AddHeader("Accept-Language", "DEFAULT");
            request.AddHeader("accept", "application/json");
            request.AddHeader("Content-Type", "application/json-patch+json");
            request.AddHeader("X-3E-SessionId", _session);
            request.AddHeader("X-3E-InstanceId", GetTenantId((NetworkCredential)_credentials));
            request.AddHeader("Cache-Control", "no-cache");
            request.AddJsonBody(json);
            return request;
        }

        private void SendRequest(RestRequest request, Action<List<FinancialRow>, int> callback, CancellationToken token)
        {
            if (_authority != null)
                InitAAD();

            IRestResponse response = _client.Execute(request);
            token.ThrowIfCancellationRequested();

            if (!response.IsSuccessful)
            {
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (_authority == null)
                        throw new AuthenticationException(Session.CurrentSession.Resources.GetMessage("ERR3EAUTHCRED", "Authentication exception: Your domain credentials do not have an access to 3E API", "").Text);

                    if (request.Attempts == 1)
                    {
                        _tokenExpireTime = DateTime.MinValue;
                        request.Parameters.RemoveAll(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase));
                        SendRequest(request, callback, token);
                        return;
                    }
                }

                throw new WebException(Session.CurrentSession.Resources.GetMessage("ERRREQUEST", "An error occurred while processing the request: %1%.", "", response.ErrorMessage ?? response.StatusDescription).Text);
            }

            int total = 0;
            var result = new List<FinancialRow>();
            var content = JsonConvert.DeserializeObject<ClientManagementResponse>(response.Content);
            if (content != null)
            {
                total = content.Data.DataRows.TotalRows;
                foreach (var row in content.Data.DataRows.Rows)
                {
                    var clientData = row.DownValues[0].Value.Split(' ');
                    var financialRow = new FinancialRow(
                        number: clientData[0],
                        name: row.DownValues[0].Value.Substring(clientData[0].Length + 1),
                        hours: row.AcrossValues.First(r => r.AttributeId == "WIPHours").Value,
                        time: row.AcrossValues.First(r => r.AttributeId == "WIPTime").Value,
                        costs: row.AcrossValues.First(r => r.AttributeId == "WIPCosts").Value,
                        charges: row.AcrossValues.First(r => r.AttributeId == "WIPCharges").Value,
                        total: row.AcrossValues.First(r => r.AttributeId == "WIPTotal").Value);
                    result.Add(financialRow);
                }
            }

            token.ThrowIfCancellationRequested();
            callback(result, total);
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
