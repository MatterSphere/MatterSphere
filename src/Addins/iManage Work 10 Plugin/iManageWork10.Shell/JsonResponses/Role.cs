using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{

    public class Role
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{GetType()}: [Id: {Id}, Description: {Description}]";
        }
    }

}
