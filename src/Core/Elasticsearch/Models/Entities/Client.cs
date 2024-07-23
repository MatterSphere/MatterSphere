using System;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class Client : BaseItem
    {
        [JsonProperty("clientNum")]
        public string ClientNumber { get; set; }

        [JsonProperty("name")]
        public string ClientName { get; set; }

        [JsonProperty("clientType")]
        public string ClientType { get; set; }

        public ResponseItem GetResponseItem()
        {
            long clientId;
            Int64.TryParse(MatterSphereId, out clientId);
            ClientNumber = GetHighlight(ClientNumber, "clientNum");
            ClientName = GetHighlight(ClientName, "name");
            ClientType = GetHighlight(ClientType, "clientType");
            Title = GetHighlight(Title, "title");
            Summary = GetHighlight(Summary, "summary");

            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = $"{ClientNumber}: {ClientName}",
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? Summary : ClientType,
                Author = null,
                ModifiedDate = ModifiedDate,
                ContactId = null,
                ClientId = clientId != 0 ? clientId : (long?)null,
                FileId = null,
                DocumentId = null,
                ObjectType = EntityType.Convert(EntityTypeEnum.Client)
            };
        }
    }
}
