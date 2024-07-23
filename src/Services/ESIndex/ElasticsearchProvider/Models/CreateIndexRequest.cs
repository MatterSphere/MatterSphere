using Newtonsoft.Json;

namespace ElasticsearchProvider.Models
{
    public class CreateIndexRequest
    {
        public CreateIndexRequest()
        {
            Settings = new Settings();
            Mapping = new Mapping();
        }

        public CreateIndexRequest(Settings settings)
        {
            Settings = settings;
            Mapping = new Mapping();
        }

        [JsonProperty("settings")]
        public Settings Settings { get; set; }

        [JsonProperty("mappings")]
        public Mapping Mapping { get; set; }
    }
}
