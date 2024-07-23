using Newtonsoft.Json;

namespace XmlConverter.Models
{
    public abstract class BaseMessage
    {
        [JsonProperty("objecttype")]
        public string Entity { get; set; }

        [JsonProperty("relatedtype")]
        public string Target { get; set; }
    }
}
