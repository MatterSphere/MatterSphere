using System.Collections.Generic;

namespace FWBS.OMS.Search
{
    /// <summary>
    /// Provides access to a Searching Mechanism
    /// </summary>
    public interface ISearchForEntity
    {
        IEnumerable<ISearchEntity> Search(SearchParameters searchDetails);
    }
}
