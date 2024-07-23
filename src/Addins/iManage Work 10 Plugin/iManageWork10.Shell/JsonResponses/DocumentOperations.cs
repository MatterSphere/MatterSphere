using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{
    public class DocumentOperations
    {
        [JsonProperty("create_new_version")]
        public bool CreateNewVersion { get; set; }

        [JsonProperty("unlock")]
        public bool Unlock { get; set; }

        [JsonProperty("update")]
        public bool Update { get; set; }

        public override string ToString()
        {
            return $"{GetType()}: [CreateNewVersion: {CreateNewVersion}, Unlock: {Unlock}, Update: {Update}]";
        }
    }
}
