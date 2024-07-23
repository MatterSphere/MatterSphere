using System.Collections.Generic;
using Newtonsoft.Json;

namespace Elasticsearch.Models
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            Documents = new DocumentBucket();
        }

        [JsonProperty("hits")]
        public DocumentBucket Documents { get; set; }

        [JsonProperty("aggregations")]
        public dynamic Aggregations { get; set; }

        [JsonProperty("suggest")]
        public dynamic Suggest { get; set; }

        #region classes
        public class DocumentBucket
        {
            public DocumentBucket()
            {
                Documents = new List<DocumentInfo>();
            }

            [JsonProperty("hits")]
            public List<DocumentInfo> Documents { get; set; }

            [JsonProperty("total")]
            public dynamic TotalValue { get; set; }

            public int Total
            {
                get { return TotalValue.value; }
            }
        }

        public class DocumentInfo
        {
            [JsonProperty("_index")]
            public string Index { get; set; }

            [JsonProperty("_id")]
            public string Key { get; set; }

            [JsonProperty("_source")]
            public dynamic Source { get; set; }

            [JsonProperty("highlight")]
            public dynamic Highlight { get; set; }
        }

        public class OptionItem
        {
            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("_score")]
            public double Score { get; set; }
        }
        #endregion
    }
}
