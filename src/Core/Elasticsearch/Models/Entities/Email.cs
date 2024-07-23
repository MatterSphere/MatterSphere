using System;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class Email : BaseItem
    {
        [JsonProperty("file-id")]
        public long FileId { get; set; }

        [JsonProperty("docDesc")]
        public string EmailDescription { get; set; }

        [JsonProperty("docContents")]
        public string EmailContent { get; set; }

        [JsonProperty("documentExtension")]
        public string EmailExtension { get; set; }

        [JsonProperty("documentType")]
        public string EmailType { get; set; }

        [JsonProperty("docDeleted")]
        public string EmailDeleted { get; set; }

        [JsonProperty("authorType")]
        public string Author { get; set; }

        #region File
        [JsonProperty("client-id")]
        public long ClientId { get; set; }

        [JsonProperty("fileStatus")]
        public string FileStatus { get; set; }

        [JsonProperty("fileType")]
        public string FileType { get; set; }

        [JsonProperty("fileDesc")]
        public string FileDescription { get; set; }
        #endregion

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
            EmailDescription = GetHighlight(EmailDescription, "docDesc");
            ClientNumber = GetHighlight(ClientNumber, "clientNum");
            ClientName = GetHighlight(ClientName, "clientName");
            // this is workaround for renaming ClientName source field
            if (string.IsNullOrEmpty(ClientName))
            {
                ClientName = GetHighlight(Name, "name");
            }
            FileDescription = GetHighlight(FileDescription, "fileDesc");
            Title = GetHighlight(Title, "title");
            Summary = GetHighlight(Summary, "summary");

            long emailId;
            Int64.TryParse(MatterSphereId, out emailId);

            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = EmailDescription,
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? Summary : $"{ClientNumber}:{ClientName}/{FileDescription}: {EmailDescription}",
                Author = Author,
                ModifiedDate = ModifiedDate,
                DocumentType = EmailType,
                ContactId = null,
                ClientId = ClientId,
                FileId = FileId,
                DocumentId = emailId != 0 ? emailId : (long?)null,
                ObjectType = EntityType.Convert(EntityTypeEnum.Email),
                Extension = EmailExtension
            };
        }
    }
}
