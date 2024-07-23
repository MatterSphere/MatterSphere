using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{
    public class Library
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public override string ToString()
        {
            return $"{GetType()}: [Id: {Id}, Type: {Type}]";
        }
    }
}
