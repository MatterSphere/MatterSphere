using Newtonsoft.Json;

namespace FWBS.OMS.HighQ.Models.Response
{
    internal class FoldersResponse
    {
        [JsonProperty("folder")]
        public FolderModel[] Folders { get; set; }

        public class FolderModel
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}
