using System;

namespace FWBS.OMS.Caching.Queries
{
    sealed class FieldsQueryCache : DataSetLocalQueryCache
    {
        public FieldsQueryCache()
            :base("dbFields")
        {
        }

        public override bool Handles(Data.ExecuteDataSetEventArgs args)
        {
            if (args.SQL.Equals("sprFieldTypes", StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }

    }
}
