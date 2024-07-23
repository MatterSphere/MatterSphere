using System;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class Associate : BaseItem
    {
        [JsonProperty("associateType")]
        public string AssociateType { get; set; }

        [JsonProperty("assocHeading")]
        public string Heading { get; set; }

        [JsonProperty("assocSalut")]
        public string Salutation { get; set; }

        [JsonProperty("file-Id")]
        public long FileId { get; set; }

        [JsonProperty("contact-Id")]
        public long ContactId { get; set; }

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

        #region Contact
        [JsonProperty("contName")]
        public string ContactName { get; set; }

        [JsonProperty("contactType")]
        public string ContactType { get; set; }
        #endregion

        public ResponseItem GetResponseItem()
        {
            long associateId;
            Int64.TryParse(MatterSphereId, out associateId);
            Salutation = GetHighlight(Salutation, "assocSalut");
            FileDescription = GetHighlight(FileDescription, "fileDesc");
            ContactName = GetHighlight(ContactName, "contName");
            Title = GetHighlight(Title, "title");
            Summary = GetHighlight(Summary, "summary");

            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = Salutation,
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? Summary : $"{Salutation} ({AssociateType})",
                Author = null,
                ModifiedDate = ModifiedDate,
                ContactId = ContactId,
                ClientId = ClientId,
                FileId = FileId,
                DocumentId = null,
                ObjectType = EntityType.Convert(EntityTypeEnum.Associate),
                Extension = null,
                AssociateId = associateId
            };
        }
    }
}
