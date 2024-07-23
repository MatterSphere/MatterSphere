using System.Net;
using Horizon.Common.Interfaces;
using RestSharp;

namespace Horizon.Providers
{
    public class ElasticsearchProvider : IElasticsearchProvider
    {
        private RestClient _restClient;

        public ElasticsearchProvider(string url, string apiKey)
        {
            _restClient = new RestClient(url);
            if (!string.IsNullOrEmpty(apiKey))
                _restClient.AddDefaultHeader("Authorization", $"ApiKey {apiKey}");
        }

        public bool CheckIndex(string index)
        {
            var request = new RestRequest($"/{index}", Method.GET);
            var response = _restClient.Execute(request);

            return response.StatusCode == HttpStatusCode.OK;
        }
    }
}
