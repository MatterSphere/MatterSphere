namespace FWBS.OMS.Caching.Queries
{
    internal sealed class TablesQueryCache : DataTableLocalQueryCache
    {
   
        #region IQueryCache Members

        public override bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            if (args.SQL.ToUpperInvariant().IndexOf("FROM INFORMATION_SCHEMA.TABLES") > -1)
                return true;

            return false;
        }

        public override string Key
        {
            get { return "OMS:TABLES"; }
        }

        protected override string Type
        {
            get { return "INFORMATION_SCHEMA.TABLES"; }
        }

        #endregion
    }



    
}
