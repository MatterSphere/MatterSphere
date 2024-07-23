using Newtonsoft.Json;

namespace ElasticsearchProvider.Models
{
    public class Mapping
    {
        [JsonProperty("properties")]
        public dynamic Properties { get; set; }
    }
}
