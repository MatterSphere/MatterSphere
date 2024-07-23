namespace FWBS.OMS.Caching.Queries
{
    internal sealed class ParametersQueryCache : DataTableLocalQueryCache
    {
  
        #region IQueryCache Members

        public override bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            if (args.SQL.ToUpperInvariant().IndexOf("FROM SYS.PARAMETERS") > -1)
                return true;

            return false;
        }

        public override string Key
        {
            get
            {
                return "OMS:PARAMETERS";
            }
        }

        protected override string Type
        {
            get { return "SYS.PARAMETERS"; }
        }

        #endregion
    }
}
