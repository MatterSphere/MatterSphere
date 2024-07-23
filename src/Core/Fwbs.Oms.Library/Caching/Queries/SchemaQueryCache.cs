namespace FWBS.OMS.Caching.Queries
{


    internal sealed class SchemaQueryLocalCache : DataTableLocalQueryCache
    {

        #region IQueryCache Members

        public override bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            return args.SchemaOnly;
        }

        public override string Key
        {
            get
            {
                return "OMS:SCHEMAS";
            }
        }

        protected override string Type
        {
            get { return "INFORMATION_SCHEMA.TABLES"; }
        }

        #endregion


    }
}
