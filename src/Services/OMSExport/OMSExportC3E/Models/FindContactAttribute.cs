using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindContactAttribute
    {
        [JsonProperty("Entity.EntIndex")]
        public string EntIndex { get; set; }

        [JsonProperty("Entity.EntityID")]
        public string EntityID { get; set; }

        [JsonProperty("Site.SiteIndex")]
        public string SiteIndex { get; set; }

        [JsonProperty("Site.SiteID")]
        public string SiteID { get; set; }

        [JsonProperty("Relate.RelateID")]
        public string RelateID { get; set; }
    }
}
