namespace FWBS.OMS.Caching.Queries
{
    internal sealed class StorageProviderQueryCache : DataTableLocalQueryCache
    {
 
        #region IQueryCache Members

        protected override string Type
        {
            get { return "dbStorageProvider"; }
        }

        #endregion
    }
}
