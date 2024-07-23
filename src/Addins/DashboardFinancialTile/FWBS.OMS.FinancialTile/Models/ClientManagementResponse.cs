using Newtonsoft.Json;

namespace FWBS.OMS.FinancialTile.Models
{
    class ClientManagementResponse
    {
        [JsonProperty("data")]
        public DataInfo Data { get; set; }

        public class DataInfo
        {
            [JsonProperty("data")]
            public DataRowsBucket DataRows { get; set; }

            public class DataRowsBucket
            {
                [JsonProperty("rows")]
                public DataRow[] Rows { get; set; }

                [JsonProperty("totalRows")]
                public int TotalRows { get; set; }
                
                public class DataRow
                {
                    [JsonProperty("acrossValues")]
                    public AcrossValue[] AcrossValues { get; set; }

                    [JsonProperty("downValues")]
                    public DownValue[] DownValues { get; set; }

                    public class AcrossValue
                    {
                        [JsonProperty("attributeId")]
                        public string AttributeId { get; set; }

                        [JsonProperty("value")]
                        public string Value { get; set; }
                    }

                    public class DownValue
                    {
                        [JsonProperty("value")]
                        public string Value { get; set; }
                    }
                }
            }
        }
    }
}
