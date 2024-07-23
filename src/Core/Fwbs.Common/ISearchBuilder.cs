using FWBS.Common.Elasticsearch;

namespace FWBS.Common
{
    public interface ISearchBuilder
    {
        SearchResult Search(SearchFilter searchFilter);
        string[] GetSuggests(string query, int size = 1);
    }
}
