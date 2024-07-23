using System.Collections.Generic;

namespace FWBS.OMS.Search
{
    /// <summary>
    /// Details of information to be passed into a search
    /// </summary>
    public class SearchParameters
    {
        public SearchParameters()
        {
            AdditionalParameters = new Dictionary<string, object>();
        }

        public string SearchValue { get; set; }
        public Dictionary<string, object> AdditionalParameters { get; private set; }
    }
}
