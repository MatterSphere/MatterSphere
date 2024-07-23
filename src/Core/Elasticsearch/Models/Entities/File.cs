using System;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class File : BaseItem
    {
        [JsonProperty("client-id")]
        public long ClientId { get; set; }

        [JsonProperty("fileNum")]
        public string FileNumber { get; set; }

        [JsonProperty("fileStatus")]
        public string FileStatus { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("fileDesc")]
        public string FileDescription { get; set; }

        #region Client
        [JsonProperty("clientNum")]
        public string ClientNumber { get; set; }

        [JsonProperty("name"), Obsolete("This will be renamed to clientName.")]
        private string Name { get; set; }

        [JsonProperty("clientName")]
        public string ClientName { get; set; }

        [JsonProperty("clientType")]
        public string ClientType { get; set; }
        #endregion

        public ResponseItem GetResponseItem()
        {
            long fileId;
            Int64.TryParse(MatterSphereId, out fileId);
            FileNumber = GetHighlight(FileNumber, "fileNum");
            FileDescription = GetHighlight(FileDescription, "fileDesc");
            ClientNumber = GetHighlight(ClientNumber, "clientNum");
            ClientName = GetHighlight(ClientName, "clientName");
            // this is workaround for renaming ClientName source field
            if (string.IsNullOrEmpty(ClientName))
            {
                ClientName = GetHighlight(Name, "name");
            }
            Title = GetHighlight(Title, "title");
            Summary = GetHighlight(Summary, "summary");

            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = FileDescription,
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? Summary : $"{ClientNumber}:{ClientName} - {FileDescription}",
                Author = null,
                ModifiedDate = ModifiedDate,
                ContactId = null,
                ClientId = ClientId,
                FileId = fileId != 0 ? fileId : (long?)null,
                DocumentId = null,
                ObjectType = EntityType.Convert(EntityTypeEnum.File)
            };
        }
    }
}
