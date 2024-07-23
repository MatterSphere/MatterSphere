using System.Linq;
using Elasticsearch.Interfaces;
using FWBS.Common.Elasticsearch;

namespace Elasticsearch.Provider
{
    public class SearchFactory
    {
        private ISearchQueryBuilder _searchBuilder;
        private IResponseParser _responseParser;

        public SearchFactory(ISearchQueryBuilder searchBuilder, IResponseParser responseParser, int facetSize = 0)
        {
            _searchBuilder = searchBuilder;
            _responseParser = responseParser;
            FacetSize = facetSize;
        }

        public int FacetSize { get; set; }
        public string[] FacetableFields { get; set; }
        public string[] SearchableFields { get; set; }
        public string[] SuggestableFields { get; set; }
        public string AdUser { get; set; }
        public string SqlUser { get; set; }

        public ISearchProvider CreateProvider(string url, string apiKey, string dataIndex, string userIndex = null)
        {
            var provider = new SearchProvider(url, apiKey, dataIndex, userIndex)
            {
                SearchBuilder = _searchBuilder,
                ResponseParser = _responseParser
            };

            if (FacetableFields != null && FacetableFields.Any())
            {
                provider.SetFacetableFields(FacetableFields, FacetSize);
            }

            if (SearchableFields != null && SearchableFields.Any())
            {
                provider.SetSearchableFields(SearchableFields);
            }

            if (SuggestableFields != null && SuggestableFields.Any())
            {
                provider.SetSuggestableFields(SuggestableFields);
            }

            if (AdUser != null || SqlUser != null)
            {
                provider.SetUserInfo(AdUser, SqlUser);
            }

            return provider;
        }
    }
}
