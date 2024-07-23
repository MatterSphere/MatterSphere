using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class GetMeResponse : BaseResponse
    {
        [JsonProperty("id")]
        public string UserId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
