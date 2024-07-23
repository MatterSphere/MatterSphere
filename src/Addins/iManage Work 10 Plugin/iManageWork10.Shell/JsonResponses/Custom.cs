using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{
    public class Custom
    {
        [JsonProperty("database")]
        public string Database { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("hipaa")]
        public bool Hipaa { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("wstype")]
        public string WsType { get; set; }

        [JsonProperty("parent")]
        public Parent Parent { get; set; }
    }

    public class Parent
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
