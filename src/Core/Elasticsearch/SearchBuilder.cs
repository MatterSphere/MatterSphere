using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Provider;
using FWBS.Common;
using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.Elasticsearch
{
    public class SearchBuilder : ISearchBuilder
    {
        public delegate void SearchCompleted(SearchResult result);
        private readonly string _url;
        private readonly string _apiKey;
        private readonly string _userIndex;
        private readonly string _dataIndex;
        private ISearchProvider _provider;
        private IDbProvider _dbProvider;
        private string _adUser;
        private string _sqlUser;
        private List<FieldTitle> _fieldTitles;

        public SearchBuilder(string url, string apiKey, string dataIndex, string userIndex = null)
        {
            _url = url;
            _apiKey = apiKey;
            _dataIndex = dataIndex;
            _userIndex = userIndex;

            _dbProvider = new DbProvider();
        }

        public void SetUser(string adUser, string sqlUser)
        {
            _adUser = adUser;
            _sqlUser = sqlUser;
        }

        public void Build(int facetSize)
        {
            if (string.IsNullOrWhiteSpace(_url) || string.IsNullOrWhiteSpace(_dataIndex))
            {
                _provider = new StubSearchProvider();
                return;
            }

            var searchableFields = _dbProvider.GetSearchableFields();
            var suggestableFields = _dbProvider.GetSuggestableFields()
                .Select(field => $"{field}Suggest")
                .ToArray();
            var facetableFields = facetSize > 0
                ? _dbProvider.GetFacetableFields()
                : new string[0];
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser(_dbProvider.IsSummaryFieldEnabled()), facetSize)
            {
                SearchableFields = searchableFields,
                SuggestableFields = suggestableFields,
                FacetableFields = facetableFields,
                AdUser = _adUser,
                SqlUser = _sqlUser
            };

            _provider = factory.CreateProvider(_url, _apiKey, _dataIndex, _userIndex);
            _fieldTitles = _dbProvider.GetFieldTitles().ToList();
        }

        public SearchResult Search(SearchFilter searchFilter)
        {
            if (!searchFilter.HasTypesFilter)
            {
                searchFilter.TypesFilter.Add(EntityTypeEnum.Unknown);
            }

            var result = _provider.Search(searchFilter);
            foreach (var aggregation in result.Aggregations)
            {
                var title = _fieldTitles.FirstOrDefault(field => field.Name == aggregation.Field);
                aggregation.Title = title != null
                    ? title.Title
                    : aggregation.Field;

                aggregation.Order = _fieldTitles.Find(field => field.Name == aggregation.Field).FacetOrder;
            }

            return result;
        }

        public string[] GetSuggests(string query, int size = 1)
        {
            return _provider.Suggest(query, size);
        }

        #region Classes

        private class StubSearchProvider : ISearchProvider
        {
            public SearchResult Search(SearchFilter filter)
            {
                return new SearchResult();
            }

            public string[] Suggest(string query, int size)
            {
                return new string[0];
            }
        }

        #endregion
    }
}
