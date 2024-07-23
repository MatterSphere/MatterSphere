using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Elasticsearch.Models.Entities
{
    public abstract class BaseItem
    {
        [JsonProperty("id")]
        public string ElasticsearchId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("mattersphereid")]
        public string MatterSphereId { get; set; }

        [JsonProperty("modifieddate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("highlights")]
        public dynamic Highlights { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        public bool IsSummaryFieldEnabled { get; set; }


        private IDictionary<string, dynamic> _highlightList;
        public IDictionary<string, dynamic> HighlightList
        {
            get
            {
                if (_highlightList == null)
                {
                    _highlightList = Highlights != null
                        ? JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(Highlights.ToString())
                        : new Dictionary<string, dynamic>();
                }

                return _highlightList;
            }
        }

        protected string GetHighlight(string fieldValue, string fieldName)
        {
            return HighlightList.ContainsKey(fieldName)
                ? (string) HighlightList[fieldName][0].ToString()
                : fieldValue;
        }
    }
}
