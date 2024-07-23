using System.Configuration;

namespace Elasticsearch.Tests.IntegrationTests
{
    public class ElasticConfiguration
    {
        public static readonly string Url = ConfigurationManager.AppSettings["Elasticsearch_url"];
        public static readonly string SearchApiKey = ConfigurationManager.AppSettings["Elasticsearch_search_apiKey"];
        public static readonly string IndexApiKey = ConfigurationManager.AppSettings["Elasticsearch_index_apiKey"];
        public static readonly string DataIndex = "testdataindex";
        public static readonly string UserIndex = "testuserindex";
    }
}