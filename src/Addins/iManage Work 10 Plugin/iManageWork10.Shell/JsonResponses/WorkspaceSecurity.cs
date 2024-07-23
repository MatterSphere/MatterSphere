using iManageWork10.Shell.JsonResponses.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace iManageWork10.Shell.JsonResponses
{
    public class WorkspaceSecurity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("access")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AccessLevel Access { get; set; }

        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public AccessType Type { get; set; }

        [JsonProperty("sid")]
        public string Sid { get; set; }

        public override string ToString()
        {
            return $"{GetType()}: [Id: {Id}, Name: {Name}, Access: {Access:G}, Type: {Type:G}, Sid: {Sid}]";
        }
    }
}