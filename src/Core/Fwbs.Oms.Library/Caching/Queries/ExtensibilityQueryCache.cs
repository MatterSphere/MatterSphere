namespace FWBS.OMS.Caching.Queries
{
    internal sealed class ExtensibilityQueryCache : DataTableLocalQueryCache
    {
 
        #region IQueryCache Members

        protected override string Type
        {
            get { return "dbExtensibility"; }
        }

        #endregion
    }
}
