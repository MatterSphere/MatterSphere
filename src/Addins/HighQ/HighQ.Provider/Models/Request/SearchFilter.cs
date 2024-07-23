using System.Collections.Generic;
using Newtonsoft.Json;

namespace FWBS.OMS.HighQ.Models.Request
{
    internal class SearchFilter
    {
        public SearchFilter(Dictionary<int, string> parameters)
        {
            var columns = new List<ColumnModel>();
            foreach (var parameter in parameters)
            {
                columns.Add(new ColumnModel
                {
                    ColumnId = parameter.Key,
                    RawData =  new RawDataModel
                    {
                        Value = parameter.Value
                    }
                });
            }

            Data = new DataModel
            {
                Columns = columns.ToArray()
            };
        }

        [JsonProperty("data")]
        public DataModel Data { get; set; }

        public class DataModel
        {
            [JsonProperty("column")]
            public ColumnModel[] Columns { get; set; }
        }

        public class ColumnModel
        {
            [JsonProperty("rawdata")]
            public RawDataModel RawData { get; set; }

            [JsonProperty("attributecolumnid")]
            public int ColumnId { get; set; }
        }

        public class RawDataModel
        {
            [JsonProperty("value")]
            public string Value { get; set; }
        }
    }
}
