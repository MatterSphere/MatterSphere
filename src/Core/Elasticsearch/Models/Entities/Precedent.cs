using System.Text;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public class Precedent : BaseItem
    {
        [JsonProperty("precTitle")]
        public string PrecedentTitle { get; set; }

        [JsonProperty("precDesc")]
        public string PrecedentDescription { get; set; }

        [JsonProperty("precContents")]
        public string PrecedentContent { get; set; }

        [JsonProperty("precCategory")]
        public string PrecedentCategory { get; set; }

        [JsonProperty("precSubCategory")]
        public string PrecedentSubcategory { get; set; }

        [JsonProperty("precMinorCategory")]
        public string PrecedentMinorCategory { get; set; }

        [JsonProperty("precDeleted")]
        public string PrecedentDeleted { get; set; }

        [JsonProperty("precedentExtension")]
        public string PrecedentExtension { get; set; }

        [JsonProperty("precedentType")]
        public string PrecedentType { get; set; }

        [JsonProperty("authorType")]
        public string Author { get; set; }

        public string FormattedSummary
        {
            get
            {
                var catBuilder = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(PrecedentCategory))
                {
                    catBuilder.Append("[").Append(PrecedentCategory);
                    if (!string.IsNullOrWhiteSpace(PrecedentSubcategory))
                    {
                        catBuilder.Append(" / ").Append(PrecedentSubcategory);
                        if (!string.IsNullOrWhiteSpace(PrecedentMinorCategory))
                        {
                            catBuilder.Append(" / ").Append(PrecedentMinorCategory);
                        }
                    }
                    catBuilder.Append("] : ");
                }

                catBuilder.Append(PrecedentDescription);
                return catBuilder.ToString();
            }
        }

        public ResponseItem GetResponseItem()
        {
            PrecedentTitle = GetHighlight(PrecedentTitle, "precTitle");
            PrecedentMinorCategory = GetHighlight(PrecedentMinorCategory, "precMinorCategory");
            PrecedentSubcategory = GetHighlight(PrecedentSubcategory, "precSubCategory");
            PrecedentCategory = GetHighlight(PrecedentCategory, "precCategory");
            PrecedentDescription = GetHighlight(PrecedentDescription, "precDesc");
            Title = GetHighlight(Title, "title");
            Summary = GetHighlight(Summary, "summary");
            
            return new ResponseItem
            {
                ElasticsearchId = ElasticsearchId,
                MatterSphereId = MatterSphereId,
                Name = PrecedentTitle,
                Title = Title,
                Summary = IsSummaryFieldEnabled && !string.IsNullOrWhiteSpace(Summary) ? 
                    Summary : FormattedSummary,
                Author = Author,
                ModifiedDate = ModifiedDate,
                ContactId = null,
                ClientId = null,
                FileId = null,
                DocumentId = null,
                ObjectType = EntityType.Convert(EntityTypeEnum.Precedent),
                Extension = PrecedentExtension
            };
        }
    }
}
