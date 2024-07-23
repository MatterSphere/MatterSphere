namespace FWBS.Common.Elasticsearch
{
    public interface ISearchProvider
    {
        SearchResult Search(SearchFilter filter);
        string[] Suggest(string query, int size);
    }
}
