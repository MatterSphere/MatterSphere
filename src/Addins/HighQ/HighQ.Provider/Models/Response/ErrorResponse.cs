using Newtonsoft.Json;

namespace FWBS.OMS.HighQ.Models.Response
{
    internal class ErrorResponse
    {
        [JsonProperty("summary")]
        public string ErrorMessage { get; set; }
    }
}
