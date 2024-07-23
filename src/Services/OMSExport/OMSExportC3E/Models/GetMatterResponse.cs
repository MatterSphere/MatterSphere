using System.Collections.Generic;
using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class GetMatterResponse : BaseResponse
    {
        private bool _success;
        [JsonProperty("success")]
        public override bool Success
        {
            get
            {
                return _success && base.Success;
            }
            set
            {
                _success = value;
            }
        }

        public string GetMatterID()
        {
            return Rows[0].Attributes["ItemID"].Value;
        }

        public string GetMatterIndex()
        {
            return Rows[0].Attributes["MattIndex"].Value;
        }

        public string GetEffectiveMattDateID(string effectiveStart)
        {
            foreach (var childObject in Rows[0].ChildObjects)
            {
                if (childObject.ObjectId == "MattDate")
                {
                    foreach (var row in childObject.Rows)
                    {
                        AttributeItem attribute;
                        if (row.Attributes.TryGetValue("EffStart", out attribute) && attribute.Value == effectiveStart)
                            return row.Attributes["ItemID"].Value;
                    }
                }
            }
            return null;
        }

        [JsonProperty("matters")]
        public ResponseRows[] Rows { get; set; }

        public class ResponseRows
        {
            public ResponseRows()
            {
                ChildObjects = new List<BaseCollection>();
            }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("attributes")]
            public IDictionary<string, AttributeItem> Attributes { get; set; }

            [JsonProperty("childObjects")]
            public List<BaseCollection> ChildObjects { get; set; }

            [JsonProperty("index")]
            public int Index { get; set; }

            [JsonProperty("subclassId")]
            public string SubclassId { get; set; }

            public class BaseCollection
            {
                public BaseCollection()
                {
                    Rows = new List<ResponseRows>();
                }

                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("objectId")]
                public string ObjectId { get; set; }

                [JsonProperty("actualRowCount")]
                public int ActualRowCount { get; set; }

                [JsonProperty("rows")]
                public List<ResponseRows> Rows { get; set; }
            }
        }

        public class AttributeItem
        {
            [JsonProperty("aliasValue")]
            public string AliasValue { get; set; }

            [JsonProperty("displayValue")]
            public string DisplayValue { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}
