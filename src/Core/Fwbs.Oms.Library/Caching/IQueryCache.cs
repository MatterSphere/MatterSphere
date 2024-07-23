using System.Data;

namespace FWBS.OMS.Caching
{
    public interface IQueryCache
    {
        bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args);
        bool Handles(FWBS.OMS.Data.ExecuteDataSetEventArgs args);

        DataTable GetData(FWBS.OMS.Data.ExecuteTableEventArgs args);
        DataSet GetData(FWBS.OMS.Data.ExecuteDataSetEventArgs args);

        void SetData(FWBS.OMS.Data.ExecuteTableEventArgs args);
        void SetData(FWBS.OMS.Data.ExecuteDataSetEventArgs args);

        void Clear(string name, CacheSearch search);
    }
}
