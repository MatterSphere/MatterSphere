namespace FWBS.OMS.Caching.Queries
{
    internal sealed class ApplicationQueryCache : DataTableLocalQueryCache
    {
 
        #region IQueryCache Members

        protected override string Type
        {
            get { return "dbApplication"; }
        }

        #endregion
    }
}
