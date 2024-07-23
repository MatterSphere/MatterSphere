using System.Collections.Generic;
using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses.AdvancedSearch
{
    public class AdvancedSearchResult<T>
    {
        [JsonProperty("results")]
        public List<T> Results { get; set; }
    }
}
