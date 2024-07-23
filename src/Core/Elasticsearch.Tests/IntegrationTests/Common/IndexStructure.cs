using Newtonsoft.Json;

namespace Elasticsearch.Tests.IntegrationTests.Common
{
    public class IndexStructure
    {
        public class IndexRequest
        {
            public IndexRequest()
            {
                Settings = new Settings();
                Mapping = new Mapping();
            }

            [JsonProperty("settings")]
            public Settings Settings { get; set; }

            [JsonProperty("mappings")]
            public Mapping Mapping { get; set; }
        }

        public class Settings
        {
            public Settings()
            {
                Shards = 1;
                Replicas = 0;
            }

            [JsonProperty("number_of_shards")]
            public int Shards { get; set; }

            [JsonProperty("number_of_replicas")]
            public int Replicas { get; set; }
        }

        public class Mapping
        {
            [JsonProperty("properties")]
            public dynamic Properties { get; set; }
        }
    }
}
