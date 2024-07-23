using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindClientAttribute
    {
        [JsonProperty("Client.ClientIndex")]
        public string ClientIndex { get; set; }

        [JsonProperty("Client.ClientID")]
        public string ClientID { get; set; }

        [JsonProperty("CliDate.MakeAlias<0>.CliDateID")]
        public string CliDateID { get; set; }

        [JsonProperty("CliDate.MakeAlias<0>.EffStart")]
        public string EffStart { get; set; }
    }
}
