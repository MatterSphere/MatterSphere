using System;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class Document : BaseItem
    {
        [JsonProperty("file-id")]
        public long FileId { get; set; }

        [JsonProperty("docDesc")]
        public string DocumentDescription { get; set; }

        [JsonProperty("docContents")]
        public string DocumentContent { get; set; }

        [JsonProperty("documentExtension")]
        public string DocumentExtension { get; set; }

        [JsonProperty("documentType")]
        public string DocumentType { get; set; }

        [JsonProperty("docDeleted")]
        public string DocumentDeleted { get; set; }

        [JsonProperty("authorType")]
        public string Author { get; set; }

        #region File
        [JsonProperty("client-Id")]
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
            Author = GetHighlight(Author, "authorType");
            DocumentDescription = GetHighlight(DocumentDescription, "docDesc");
            ClientNumber = GetHighlight(ClientNumber, "clientNum");
            ClientName = GetHighlight(ClientName, "clientName");
            // this is workaround for renaming ClientName source field
            if (string.IsNullOrEmpty(ClientName))
            {
                ClientName = GetHighlight(Name, "name");
            }
            FileDescription = GetHighlight(FileDescription, "fileDesc");
            DocumentExtension = GetHighlight(DocumentExtension, "documentExtension");
            Title = GetHighlight(Title, "title");
            Summary = GetHighlight(Summary, "summary");

            long documentId;
            Int64.TryParse(MatterSphereId, out documentId);

            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = DocumentDescription,
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? Summary : $"{ClientNumber}:{ClientName} - {FileDescription}. {DocumentDescription}.{DocumentExtension}",
                Author = Author,
                ModifiedDate = ModifiedDate,
                DocumentType = DocumentType,
                ContactId = null,
                ClientId = ClientId,
                FileId = FileId,
                DocumentId = documentId != 0 ? documentId : (long?)null,
                ObjectType = EntityType.Convert(EntityTypeEnum.Document),
                Extension = DocumentExtension
            };
        }
    }
}
