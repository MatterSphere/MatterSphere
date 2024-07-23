using System.Collections.Generic;
using Horizon.Common.Models.Repositories.Blacklist;
using Horizon.Models.IndexReports;

namespace Horizon.Providers
{
    public class BREData
    {
        public BREData()
        {
            Blacklist = new List<BlacklistItem>();
        }

        public string InfoMessage { get; set; }
        public string RecommendationMessage { get; set; }
        public string Status { get; set; }

        public List<BlacklistItem> Blacklist { get; set; }
        public IFilterInfo IFilterInfo { get; set; }
    }
}
