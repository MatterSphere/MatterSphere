namespace FWBS.OMS.Caching.Queries
{
    internal sealed class StoredProcedureQueryCache : DataTableLocalQueryCache
    {
 
        #region IQueryCache Members

        public override bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            if (args.SQL.ToUpperInvariant().IndexOf("FROM SYS.PROCEDURES") > -1)
                return true;

            return false;
        }

        public override string Key
        {
            get
            {
                return "OMS:STOREDPROCEDURES";
            }
        }

        protected override string Type
        {
            get { return "SYS.PROCEDURES"; }
        }

        #endregion
    }
}
