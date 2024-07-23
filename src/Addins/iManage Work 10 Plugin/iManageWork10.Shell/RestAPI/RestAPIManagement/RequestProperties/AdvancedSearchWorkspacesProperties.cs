using Newtonsoft.Json;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties
{
    public class AdvancedSearchWorkspacesProperties
    {
        public AdvancedSearchWorkspacesProperties(SearchWorkspacesProperties searchWorkspacesProperties)
        {
            ProfileFields = new WorkspaceFields();
            Filters = searchWorkspacesProperties;
        }

        [JsonProperty("filters")]
        public SearchWorkspacesProperties Filters { get; set; }
        
        [JsonProperty("profile_fields")]
        public WorkspaceFields ProfileFields { get; set; }

        public override string ToString()
        {
            return $"[Filters: {Filters}]";
        }

        public class WorkspaceFields
        {
            [JsonProperty("workspace")]
            public string[] Fields
            {
                get
                {
                    return new string[]
                    {
                        "id",
                        "database",
                        "name",
                        "custom1",
                        "custom2",
                        "default_security",
                        "wstype"
                    };
                }
            }
        }
    }
}
