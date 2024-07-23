using Newtonsoft.Json;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties
{
    public class SearchFoldersProperties
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// iManage REST API v2
    /// Payload for Scoped search for folders
    /// </summary>
    public class SearchFoldersProperties2
    {
        public SearchFoldersProperties2(string name, string workspaceId)
        {
            ProfileFields = new FolderFields();
            Filters = new SearchFolderFilters
            {
                Name = name,
                WorkspaceId = workspaceId
            };
        }

        [JsonProperty("profile_fields")]
        public FolderFields ProfileFields { get; set; }

        [JsonProperty("filters")]
        public SearchFolderFilters Filters { get; set; }

        public class FolderFields
        {
            [JsonProperty("folder")]
            public string[] Fields
            {
                get
                {
                    return new string[]
                    {
                    "id",
                    "name"
                    };
                }
            }
        }

        public class SearchFolderFilters
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("container_id")]
            public string WorkspaceId { get; set; }
        }
    }
}
