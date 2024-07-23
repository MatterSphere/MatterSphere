using Newtonsoft.Json;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties
{
    public class SearchWorkspacesProperties
    {
        [JsonProperty("custom1")]
        public string Custom1 { get; set; }

        [JsonProperty("custom2")]
        public string Custom2 { get; set; }

        [JsonProperty("libraries")]
        public string Libraries { get; set; }

        public override string ToString()
        {
            return $"[Custom1: {Custom1}, Custom2: {Custom2}, Libraries: {Libraries}]";
        }
    }
}
