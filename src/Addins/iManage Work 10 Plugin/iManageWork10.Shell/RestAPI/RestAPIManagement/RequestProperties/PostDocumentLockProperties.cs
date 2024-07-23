using Newtonsoft.Json;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties
{
    public class PostDocumentLockProperties
    {
        [JsonProperty("due_date")]
        public string DueDate { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    }
}
