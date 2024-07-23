using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindMatterAttribute
    {
        [JsonProperty("Matter.MatterID")]
        public string MatterID { get; set; }

        [JsonProperty("Matter.MattIndex")]
        public string MattIndex { get; set; }

        [JsonProperty("MattDate.MakeAlias<0>.MattDateID")]
        public string MattDateID { get; set; }

        [JsonProperty("MattDate.MakeAlias<0>.EffStart")]
        public string EffStart { get; set; }
    }
}
