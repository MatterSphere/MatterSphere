using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using FWBS.OMS.HighQ.Converters;
using FWBS.OMS.HighQ.Interfaces;
using FWBS.OMS.HighQ.Models;
using FWBS.OMS.HighQ.Models.Request;
using FWBS.OMS.HighQ.Models.Response;
using Newtonsoft.Json;
using RestSharp;

namespace FWBS.OMS.HighQ.Providers
{
    internal class HighQProvider : IHighQProvider, IDisposable
    {
        private const int DOCUMENT_UPLOAD_TIMEOUT = 360000;
        private const string MATTER_NOT_FOUND = "HQMTTRNTFND";
        private readonly ITokenProvider _tokenProvider;
        private readonly IRestClient _restClient;

        public HighQProvider()
        {
            _tokenProvider = new TokenProvider();
            _restClient = new RestClient();
        }

        internal HighQProvider(ITokenProvider tokenProvider, IRestClient restClient)
        {
            _tokenProvider = tokenProvider;
            _restClient = restClient;
        }

        #region Private methods

        private void OnTokensUpdated(object sender, TokensEventArgs e)
        {
            AccessToken = e.AccessToken;
            RefreshToken = e.RefreshToken;
            TokensUpdated?.Invoke(this, e);
        }

        private IRestResponse Execute(RestRequest request)
        {
            ApplyAuthorization(request, AccessToken);
            var response = _restClient.Execute(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                _tokenProvider.UpdateTokens(RefreshToken);

                ApplyAuthorization(request, AccessToken);
                response = _restClient.Execute(request);
            }

            return response;
        }

        private void ApplyAuthorization(RestRequest request, string accessToken)
        {
            request.AddHeader("Auth-Type", "OAUTH2");

            request.Parameters.RemoveAll(p => p.Name.Equals("Authorization", StringComparison.OrdinalIgnoreCase));

            request.AddHeader("Authorization", $"Bearer {accessToken}");
        }

        private ColumnInfo[] GetColumnsInfo(int iSheetId)
        {
            var request = new RestRequest($"/{1}/isheets/{iSheetId}/columns", Method.GET);
            request.AddHeader("Accept", "application/json");
            var response = Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw ErrorHandler.GetError(response);
            }

            var model = JsonConvert.DeserializeObject<ColumnsResponse>(response.Content);

            return model.Columns.Select(column => new ColumnInfo(column.Id, column.Title, column.Type)).ToArray();
        }

        private SearchISheetItemResponse SearchOmsFile(int iSheetId, int clientColumnId, string clientNo, int omsFileColumnId, string omsFileNo)
        {
            var request = new RestRequest($"/{3}/isheet/{iSheetId}/search?limit=100&offset=0", Method.POST);
            request.AddHeader("Accept", "application/json");
            var parameters = new Dictionary<int, string>()
            {
                {clientColumnId, clientNo},
                {omsFileColumnId, omsFileNo}
            };
            var body = new SearchFilter(parameters);
            request.AddJsonBody(JsonConvert.SerializeObject(body));

            var response = Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw ErrorHandler.GetError(response);
            }

            return JsonConvert.DeserializeObject<SearchISheetItemResponse>(response.Content);
        }

        private int GetFolderId(int iSheetId, string clientNo, string omsFileNo, int clientColumnId, int omsFileColumnId, int folderColumnId)
        {
            var model = SearchOmsFile(iSheetId, clientColumnId, clientNo, omsFileColumnId, omsFileNo);
            ResponseModels.ItemModel itemModel;
            switch (model.Content.TotalRecordCount)
            {
                case 0:
                    var message = Session.CurrentSession.Resources.GetResource(MATTER_NOT_FOUND,
                        "The Matter was not found", "").Text;
                    throw new Exception(message);
                case 1:
                    itemModel = model.Items[0];
                    break;
                default:
                    itemModel = model.Items
                        .First(i =>
                            i.Columns.First(c => c.ColumnId == clientColumnId).DisplayData.DisplayValue.Equals(clientNo) 
                            && i.Columns.First(c => c.ColumnId == omsFileColumnId).DisplayData.DisplayValue.Equals(omsFileNo));
                    break;
            }

            int id;
            int.TryParse(itemModel.Columns.First(column => column.ColumnId == folderColumnId)
                .DisplayData
                .DisplayValue, out id);

            return id;
        }

        private string GetISheetsContent(int iSheetId)
        {
            var request = new RestRequest($"/{3}/isheet/{iSheetId}/items", Method.GET);
            request.AddHeader("Accept", "application/json");
            var response = Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpException((int)response.StatusCode, response.ErrorMessage);
            }

            return response.Content;
        }

        private FoldersResponse GetFolderList(int rootFolderId)
        {
            var request = new RestRequest($"/{1}/folders/{rootFolderId}/items?statusinfo=false&includesmartfolder=false", Method.GET);
            request.AddHeader("Accept", "application/json");
            var response = Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpException((int)response.StatusCode, response.ErrorMessage);
            }

            return JsonConvert.DeserializeObject<FoldersResponse>(response.Content);
        }

        #endregion

        #region IHighQProvider

        public event EventHandler<TokensEventArgs> TokensUpdated;

        public string AccessToken { get; private set; }
        public string RefreshToken { get; private set; }

        public void Build(int clientId, string secret, string site, string redirectUri, TokenDetails tokenDetails)
        {
            AccessToken = tokenDetails.AccessToken;
            RefreshToken = tokenDetails.RefreshToken;
            _tokenProvider.Build(clientId, secret, site, redirectUri);
            _tokenProvider.TokensUpdated += OnTokensUpdated;

            if (tokenDetails.GetTokensStatus() != TokenStatus.Actual)
            {
                _tokenProvider.UpdateTokens(tokenDetails.RefreshToken, tokenDetails.GetTokensStatus() == TokenStatus.NeedFullUpdate);
            }

            _restClient.BaseUrl = new Uri($"{site}/api");
        }
        
        public SheetItemsResponse GetISheetsCollection(int iSheetId)
        {
            return JsonConvert.DeserializeObject<SheetItemsResponse>(GetISheetsContent(iSheetId));
        }

        public DataTable GetISheetsDataTable(int iSheetId)
        {
            return JsonConvert.DeserializeObject<DataTable>(GetISheetsContent(iSheetId), new SheetDataTableJsonConverter());
        }

        public int GetFolderId(FolderDetails details)
        {
            var columns = GetColumnsInfo(details.iSheetId);
            var clientColumn = columns.First(column => column.Title == details.ClientColumnTitle);
            var omsFileColumn = columns.First(column => column.Title == details.OmsFileColumnTitle);
            var folderColumn = columns.First(column => column.Title == details.FolderColumnTitle);

            return GetFolderId(details.iSheetId, details.ClientNo, details.OmsFileNo, clientColumn.Id, omsFileColumn.Id, folderColumn.Id);
        }

        public FolderInfoResponse GetFolderInfo(int folderId)
        {
            var request = new RestRequest($"/{1}/folders/{folderId}/?statusinfo=false", Method.GET);
            request.AddHeader("Accept", "application/json");
            var response = Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw ErrorHandler.GetError(response);
            }

            return JsonConvert.DeserializeObject<FolderInfoResponse>(response.Content);
        }

        public int UploadDocument(DocumentDetails details)
        {
            var request = new RestRequest($"/{1}/files/content?parentfolderid={details.HQFolderId}", Method.POST);
            request.Timeout = DOCUMENT_UPLOAD_TIMEOUT;
            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddParameter("dmsdocid", details.DocumentId, "multipart/form-data", ParameterType.GetOrPost);
            request.AddParameter("filename", details.Description, "multipart/form-data", ParameterType.GetOrPost);
            request.AddFile("file", details.Path, MimeMapping.GetMimeMapping(details.Path));

            var response = Execute(request);

            if (response.StatusCode != HttpStatusCode.Created)
            {
                throw ErrorHandler.GetDocumentError(response, details.Description);
            }

            return JsonConvert.DeserializeObject<AddDocumentResponse>(response.Content).DocumentId;
        }

        public FolderItem[] GetFolders(int rootFolder)
        {
            return GetFolderList(rootFolder).Folders.Select(folder => new FolderItem(folder.Id, folder.Name)).ToArray();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (_tokenProvider != null)
            {
                _tokenProvider.TokensUpdated -= OnTokensUpdated;
            }
        }

        #endregion
    }
}
