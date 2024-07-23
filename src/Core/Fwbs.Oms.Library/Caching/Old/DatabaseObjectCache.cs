using System;

namespace FWBS.OMS.Caching
{
    [Obsolete("Use the one in Queries")]
    internal sealed class DatabaseObjectCache : BaseQueryCache
    {
        #region IQueryCache Members

        public override bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            if (args.SQL.ToUpperInvariant().IndexOf("FROM INFORMATION_SCHEMA.ROUTINES") > -1)
                return true;

            return false;
        }

        protected override DataTableCache Cache
        {
            get { return (DataTableCache)Session.CurrentSession.CachedItems["OMS:DATABASEOBJECTS"]; }
        }

        protected override string GetName(FWBS.OMS.Data.ExecuteEventArgs args)
        {
            return "Procedures";
        }

        #endregion


    }

   
}
