using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{
    public class Folder
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        public override string ToString()
        {
            return $"{GetType()}: [Id: {Id}, Name: {Name}]";
        }
    }
}