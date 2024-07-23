using Newtonsoft.Json;

namespace FWBS.OMS.HighQ.Models.Response
{
    internal class FolderInfoResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("permission")]
        public PermissionModel Permission { get; set; }

        public class PermissionModel
        {
            [JsonProperty("addEditAllFiles")]
            public bool AddEditAllFiles { get; set; }
        }
    }
}
