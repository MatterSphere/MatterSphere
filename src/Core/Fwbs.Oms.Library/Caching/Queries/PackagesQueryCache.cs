namespace FWBS.OMS.Caching.Queries
{
    internal sealed class PackagesQueryCache : DataTableLocalQueryCache
    {
 
        #region IQueryCache Members

        protected override string Type
        {
            get { return "dbPackages"; }
        }

        #endregion
    }
}
