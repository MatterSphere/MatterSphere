using System.Collections.Generic;
using FWBS.Common;
using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.Search
{
    public class MSSearchBuilder : ISearchBuilder
    {
        private readonly string _cdsConnection;
        private ISearchProvider _provider;

        public MSSearchBuilder(string cdsConnection)
        {
            _cdsConnection = cdsConnection;
        }

        public void Build(int facetSize)
        {
            if (string.IsNullOrWhiteSpace(_cdsConnection))
                _provider = new StubSearchProvider();
            else
                _provider = new MSSearchProvider(_cdsConnection, Session.CurrentSession.CurrentUser.ID);
        }

        public SearchResult Search(SearchFilter searchFilter)
        {
            if (!searchFilter.HasTypesFilter)
            {
                searchFilter.TypesFilter.Add(EntityTypeEnum.Unknown);
            }

            return _provider.Search(searchFilter);
        }

        public string[] GetSuggests(string query, int size = 1)
        {
            return new string[0];
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
