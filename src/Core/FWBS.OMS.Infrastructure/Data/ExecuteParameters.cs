using System;
using System.Collections.Generic;

namespace FWBS.OMS.Data
{
    using System.Data;


    public class ExecuteParameters
    {
        public ExecuteParameters()
        {
            DefaultDateTimeKind = DateTimeKind.Utc;
        }

        public string Sql { get; set; }

        public CommandType CommandType { get; set; }

        private readonly List<IDataParameter> parameters = new List<IDataParameter>();
        public List<IDataParameter> Parameters 
        {
            get
            {
                return parameters;
            }
        }

        public bool SchemaOnly { get; set; }

        public bool Refresh { get; set; }

        public DateTimeKind DefaultDateTimeKind { get; set; }

        private readonly IDictionary<string, DateTimeItem> _DateTimeColumns = new Dictionary<string, DateTimeItem>(StringComparer.OrdinalIgnoreCase);
        public IDictionary<string, DateTimeItem> DateTimeColumns
        {
            get
            {
                return _DateTimeColumns;
            }
        }

        private readonly IDictionary<string, DateTimeItem> _DateTimeParameters = new Dictionary<string, DateTimeItem>(StringComparer.OrdinalIgnoreCase);
        public IDictionary<string, DateTimeItem> DateTimeParameters
        {
            get
            {
                return _DateTimeParameters;
            }
        }
    }

    public class DataTableExecuteParameters : ExecuteParameters
    {
        public string Table { get; set; }
    }

    public class DataSetExecuteParameters : ExecuteParameters
    {
        public string[] Tables { get; set; }
    }

    public class DateTimeItem
    {
        public DateTimeKind Kind { get; set; }
    }
}