namespace FWBS.OMS.Caching.Queries
{
    internal sealed class AssemblyQueryCache : DataTableLocalQueryCache
    {
        #region IQueryCache Members

        public override bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            switch (args.SQL.ToUpperInvariant())
            {
                case "SELECT ASSEMBLY, MODIFIED, OMSVERSION, VERSION, PACKAGETYPE FROM DBASSEMBLY WHERE (OMSVERSION = @VERSION OR OMSVERSION = '')":
                case "SELECT ASSEMBLY, MODIFIED, OMSVERSION, VERSION, '' AS PACKAGETYPE FROM DBASSEMBLY WHERE (OMSVERSION = @VERSION OR OMSVERSION = '')":
                      return true;
            }

            return false;
        }

        protected override string Type
        {
            get { return "dbAssembly"; }
        }

        #endregion
    }
}
