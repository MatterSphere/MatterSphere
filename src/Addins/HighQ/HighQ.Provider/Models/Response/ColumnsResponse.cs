using Newtonsoft.Json;

namespace FWBS.OMS.HighQ.Models.Response
{
    internal class ColumnsResponse
    {
        [JsonProperty("column")]
        public ColumnModel[] Columns { get; set; }

        public class ColumnModel
        {
            [JsonProperty("columnID")]
            public int Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }
    }
}
