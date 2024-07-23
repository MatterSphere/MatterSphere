using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Elasticsearch.Interfaces;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;
using RestSharp;

namespace Elasticsearch.Provider
{
    public class SearchProvider : ISearchProvider
    {
        private readonly RestClient _client;
        private string _userIndex;
        private string _dataIndex;
        private string[] _facetableFields;
        private string[] _searchableFields;
        private string[] _suggestableFields;
        private int _facetSize;
        private string[] _accessList;
        private string _adUserName;
        private string _sqlUserName;

        public SearchProvider(string url, string apiKey, string dataIndex, string userIndex)
        {
            _client = new RestClient(url);
            _userIndex = userIndex;
            _dataIndex = dataIndex;

            _suggestableFields = new string[0];

            if (!string.IsNullOrEmpty(apiKey))
                _client.AddDefaultHeader("Authorization", $"ApiKey {apiKey}");
        }

        public ISearchQueryBuilder SearchBuilder { get; set; }
        public IResponseParser ResponseParser { get; set; }

        private string[] AccessList
        {
            get
            {
                if (_accessList == null && _userIndex != null)
                {
                    _accessList = GetUserInfo(_adUserName, _sqlUserName);
                }

                return _accessList;
            }
        }

        public SearchResult Search(SearchFilter searchFilter)
        {
            var request = CreateSearchRequest(searchFilter);
            var response = _client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                HandleFailedResponse(response);
            }

            return ResponseParser.ParseResponse(response.Content);
        }

        public string[] Suggest(string query, int size)
        {
            var request = CreateSuggestRequest(query, size);
            var response = _client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                HandleFailedResponse(response);
            }

            return ResponseParser.ParseSuggestResponse(response.Content);
        }

        public void SetFacetableFields(string[] fields, int facetSize)
        {
            _facetableFields = fields;
            _facetSize = facetSize;
        }

        public void SetSearchableFields(string[] fields)
        {
            _searchableFields = fields;
        }

        public void SetSuggestableFields(string[] fields)
        {
            _suggestableFields = fields;
        }

        public void SetUserInfo(string adName, string sqlName)
        {
            _adUserName = adName;
            _sqlUserName = sqlName;
        }

        #region private methods

        private string[] GetUserInfo(string adName, string sqlName)
        {
            var request = CreateUserRequest(adName, sqlName);
            var response = _client.Execute(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                HandleFailedResponse(response);
            }

            var usrAccessList = ResponseParser.ParseUserResponse(response.Content);
            return usrAccessList.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private RestRequest CreateSearchRequest(SearchFilter filter)
        {
            var body = SearchBuilder.CreateSearchRequestBody(
                filter,
                _searchableFields,
                _facetableFields,
                _facetSize,
                AccessList);
            var request = new RestRequest($"/{_dataIndex}/_search", Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(body));

            return request;
        }
        
        private RestRequest CreateSuggestRequest(string search, int size)
        {
            var body = SearchBuilder.CreateSuggestRequestBody(search, _suggestableFields, size);
            var request = new RestRequest($"/{_dataIndex}/_search", Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(body));

            return request;
        }

        private RestRequest CreateUserRequest(string adName, string sqlName)
        {
            var body = SearchBuilder.CreateUserRequestBody(adName, sqlName);
            var request = new RestRequest($"/{_userIndex}/_search", Method.POST);
            request.AddJsonBody(JsonConvert.SerializeObject(body));

            return request;
        }

        private string GetErrorDetails(string errorResponseContent)
        {
            var errors = new StringBuilder();
            var info = JsonConvert.DeserializeObject<Error>(errorResponseContent);
            if (info != null)
            {
                foreach (var error in info.Info.Errors)
                {
                    errors.AppendLine(error.Reason);
                }
            }
            return errors.ToString();
        }

        private void HandleFailedResponse(IRestResponse response)
        {
            var errors = GetErrorDetails(response.Content);
            if (!string.IsNullOrWhiteSpace(errors))
            {
                throw new ElasticsearchException($"Elasticsearch server: {errors}");
            }

            if (response.ErrorException != null)
            {
                throw response.ErrorException;
            }

            throw new ElasticsearchException("Elasticsearch server error");
        }

        #endregion

        #region clasees

        private class Error
        {
            [JsonProperty("error")]
            public GeneralInfo Info { get; set; }


            public class GeneralInfo
            {
                public GeneralInfo()
                {
                    Errors = new List<ErrorDetails>();
                }

                [JsonProperty("root_cause")]
                public List<ErrorDetails> Errors { get; set; }
            }

            public class ErrorDetails
            {
                [JsonProperty("reason")]
                public string Reason { get; set; }
            }
        }

        public class ElasticsearchException : Exception
        {
            public ElasticsearchException(string message) : base(message)
            {

            }
        }

        #endregion
    }
}
