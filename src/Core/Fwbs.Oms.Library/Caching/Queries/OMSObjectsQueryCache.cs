namespace FWBS.OMS.Caching.Queries
{
    internal sealed class OMSObjectsQueryCache : DataTableLocalQueryCache
    {
 
        #region IQueryCache Members

        protected override string Type
        {
            get { return "dbOMSObjects"; }
        }

        #endregion
    }
}
