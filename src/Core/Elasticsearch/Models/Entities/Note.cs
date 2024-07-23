using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class Note : BaseItem
    {
        [JsonProperty("appoinmentId")]
        public long? AppoinmentId { get; set; }

        [JsonProperty("fileId")]
        public long? FileId { get; set; }

        [JsonProperty("clientId")]
        public long? ClientId { get; set; }

        [JsonProperty("contactId")]
        public long? ContactId { get; set; }

        [JsonProperty("note")]
        public string NoteDescription { get; set; }

        [JsonProperty("noteSource")]
        public string NoteSource { get; set; }
        
        public ResponseItem GetResponseItem()
        {
            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = $"Note from {NoteSource}",
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? Summary : NoteDescription,
                Author = null,
                ModifiedDate = ModifiedDate,
                ContactId = ContactId,
                ClientId = ClientId,
                FileId = FileId,
                DocumentId = null,
                ObjectType = EntityType.Convert(EntityTypeEnum.Note)
            };
        }
    }
}
