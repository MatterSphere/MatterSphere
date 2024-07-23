using System;

namespace FWBS.OMS.Caching
{
    [Obsolete("Use the one in Queries")]
    internal sealed class SchemaQueryCache : BaseQueryCache
    {
        #region IQueryCache Members

        public override bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            return args.SchemaOnly;
        }

        protected override DataTableCache Cache
        {
            get { return (DataTableCache)Session.CurrentSession.CachedItems["OMS:SCHEMAS"]; }
        }

        #endregion


    }
}
