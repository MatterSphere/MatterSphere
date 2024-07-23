using Newtonsoft.Json;

namespace Elasticsearch.Models
{
    public class AggregationItem
    {
        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("doc_count")]
        public int Number { get; set; }
    }
}
