using Newtonsoft.Json;

namespace ElasticsearchProvider.Models
{
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

        // this settings was added accordingly to the documentation
        // https://www.elastic.co/guide/en/elasticsearch/reference/7.7/normalizer.html
        public dynamic analysis = new
        {
            normalizer = new
            {
                title_normalizer = new
                {
                    type = "custom",
                    filter = new[]
                    {
                        "lowercase"
                    }
                }
            }
        };
    }
}
