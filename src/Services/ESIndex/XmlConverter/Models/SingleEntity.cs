using System.Dynamic;
using Newtonsoft.Json;

namespace XmlConverter.Models
{
    public class SingleEntity
    {
        [JsonProperty("root")]
        public Message Bucket { get; set; }

        public class Message : BaseMessage
        {
            [JsonProperty("row")]
            public ExpandoObject Element { get; set; }
        }
    }
}
