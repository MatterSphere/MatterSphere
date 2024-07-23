using iManageWork10.Shell.JsonResponses;
using Newtonsoft.Json;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties
{
    public class PostDocumentProfileProperties
    {
        [JsonProperty("warnings_for_required_and_disabled_fields")]
        public bool Warnings { get; set; }

        [JsonProperty("doc_profile")]
        public DocumentProfile DocProfile { get; set; }
    }
}
