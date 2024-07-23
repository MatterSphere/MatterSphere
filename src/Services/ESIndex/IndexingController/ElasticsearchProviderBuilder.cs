using System;
using ElasticsearchProvider.Models;
using Models.ElasticsearchModels;
using Models.Interfaces;
using RestSharp;

namespace IndexingController
{
    public class ElasticsearchProviderBuilder
    {
        private IDbProvider _dbProvider;
        private string _url;
        private readonly Settings _dataIndexSettings;
        private readonly Settings _userIndexSettings;
        private RestClient _restClient;

        public ElasticsearchProviderBuilder(IDbProvider dbProvider, string url, Settings dataIndexSettings, Settings userIndexSettings)
        {
            _dbProvider = dbProvider;
            _url = url;
            _dataIndexSettings = dataIndexSettings;
            _userIndexSettings = userIndexSettings;
        }

        public IElasticsearchProvider CreateProvider()
        {
            var userIndex = _dbProvider.GetUserIndexName();
            var dataIndex = _dbProvider.GetDataIndexName();
            if (_restClient == null)
            {
                throw new InvalidOperationException("Can't create Provider without Client. Please call 'InitClient' method first.");
            }

            return new ElasticsearchProvider.Provider(_restClient, dataIndex, _dataIndexSettings, userIndex, _userIndexSettings);
        }

        public ElasticsearchProviderBuilder InitClient(ElasticsearchClientParameters clientParams)
        {
            _restClient = new RestClient(clientParams.Url);
            if (!string.IsNullOrEmpty(clientParams.ApiKey))
                _restClient.AddDefaultHeader("Authorization", $"ApiKey {clientParams.ApiKey}");

            return this;
        }
    }
}
