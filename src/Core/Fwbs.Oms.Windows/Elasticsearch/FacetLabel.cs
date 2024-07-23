using System;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class FacetLabel
    {
        public FacetLabel(Guid key, string field, string value)
        {
            Key = key;
            Field = field;
            Value = value;
        }

        public Guid Key { get; set; }
        public string Field { get; set; }
        public string Value { get; set; }
    }
}
