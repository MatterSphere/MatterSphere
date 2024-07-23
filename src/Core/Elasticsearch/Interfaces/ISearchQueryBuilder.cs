using FWBS.Common.Elasticsearch;

namespace Elasticsearch.Interfaces
{
    public interface ISearchQueryBuilder
    {
        dynamic CreateSearchRequestBody(SearchFilter filter, string[] searchableFields, string[] facetableFields,
            int facetSize, string[] accessList = null);
        dynamic CreateSuggestRequestBody(string queryString, string[] suggests, int size);
        dynamic CreateUserRequestBody(string adName, string sqlName);
    }
}
