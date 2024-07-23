using System.Collections.Generic;

namespace FWBS.Common.Elasticsearch
{
    public class SearchResult
    {
        public SearchResult()
        {
            Documents = new List<ResponseItem>();
            Aggregations = new List<AggregationBucket>();
        }

        public List<ResponseItem> Documents { get; set; }
        public List<AggregationBucket> Aggregations { get; set; }
        public int TotalDocuments { get; set; }
    }
}
