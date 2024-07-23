using System;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class Contact : BaseItem
    {
        [JsonProperty("contactType")]
        public string ContactType { get; set; }

        [JsonProperty("contName")]
        public string ContactName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        public ResponseItem GetResponseItem()
        {
            long contactId;
            Int64.TryParse(MatterSphereId, out contactId);
            ContactName = GetHighlight(ContactName, "contName");
            Address = GetHighlight(Address, "address");
            Title = GetHighlight(Title, "title");
            Summary = GetHighlight(Summary, "summary");

            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = ContactName,
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? Summary : $"{Address}",
                Author = null,
                ModifiedDate = ModifiedDate,
                ContactId = contactId != 0 ? contactId : (long?) null,
                ClientId = null,
                FileId = null,
                DocumentId = null,
                ObjectType = EntityType.Convert(EntityTypeEnum.Contact)
            };
        }
    }
}
