using FWBS.Common.Elasticsearch;

namespace Elasticsearch.Interfaces
{
    public interface IResponseParser
    {
        SearchResult ParseResponse(string responseContent);
        string[] ParseSuggestResponse(string responseContent);
        string ParseUserResponse(string userContent);
    }
}
