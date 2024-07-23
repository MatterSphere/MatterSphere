using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindGenericResponse<T> : BaseResponse where T : class
    {
        public FindGenericResponse()
        {
            Rows = new List<FindResponseRow>();
        }

        [JsonProperty("rowCount")]
        public int RowCount { get; set; }

        [JsonProperty("rows")]
        public List<FindResponseRow> Rows { get; set; }

        [JsonProperty("fields")]
        public List<FindResponseField> Fields { get; set; }

        [JsonProperty("errors")]
        public Dictionary<string, string[]> Error { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("traceId")]
        public string TraceId { get; set; }

        public override string ErrorMessage
        {
            get
            {
                return string.Concat(base.ErrorMessage, GetErrorMessage());
            }
            set
            {
                base.ErrorMessage = value;
            }
        }

        private string GetErrorMessage()
        {
            StringBuilder errorMessage = new StringBuilder();
            string separator = "\r\n";

            if (Error != null)
            {
                foreach (var item in Error)
                {
                    errorMessage.Append(separator).Append(string.Join(separator, item.Value));
                }
            }

            return errorMessage.ToString();
        }

        public class FindResponseRow
        {
            [JsonProperty("attributes")]
            public T Attributes { get; set; }
        }

        public class FindResponseField
        {
            [JsonProperty("caption")]
            public string Caption { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("type")]
            public int Type { get; set; }
        }
    }
}
