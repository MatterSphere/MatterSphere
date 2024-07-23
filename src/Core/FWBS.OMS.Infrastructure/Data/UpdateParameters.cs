using System;
using System.Data;

namespace FWBS.OMS.Data
{
    public abstract class UpdateParameters
    {
        public bool Refresh { get; set; }

        public string Table { get; set; }

        public string[] Fields{get;set;}
    }

    public class DataTableUpdateParameters : UpdateParameters
    {
        public DataTableUpdateParameters(DataTable data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            this.Data = data;
        }
        public DataTable Data { get; private set; }
    }

    public class DataRowUpdateParameters : UpdateParameters
    {
        public DataRowUpdateParameters(DataRow data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            this.Data = data;
        }

        public DataRow Data { get; private set; }
    }
}
