using System.Collections.Generic;

namespace FWBS.Common.Elasticsearch
{
    public class AggregationBucket
    {
        public AggregationBucket(string field, IEnumerable<AggregationItem> aggregations)
        {
            Field = field;
            Aggregations = new List<AggregationItem>(aggregations);
        }

        public string Title { get; set; }
        public string Field { get; set; }
        public byte Order { get; set; }
        public List<AggregationItem> Aggregations { get; set; }

        public class AggregationItem
        {
            public AggregationItem(string value, int number)
            {
                Value = value;
                Number = number;
            }

            public string Value { get; set; }
            public int Number { get; set; }
        }
    }
}
