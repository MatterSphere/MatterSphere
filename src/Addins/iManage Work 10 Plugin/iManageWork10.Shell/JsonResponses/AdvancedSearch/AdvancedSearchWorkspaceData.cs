using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses.AdvancedSearch
{
    public class AdvancedSearchWorkspaceData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("workspace_id")]
        public string WorkspaceId { get; set; }

        [JsonProperty("wstype")]
        public string WsType { get; set; }
    }
}
