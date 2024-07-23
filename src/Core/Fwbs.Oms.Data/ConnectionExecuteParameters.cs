namespace FWBS.OMS.Data
{
    using System.Data;

    public sealed class ConnectionExecuteParameters
    {
        private string sql;
        private string[] tablenames;
        private CommandType type;
        private bool schemaOnly;
        private IDataParameter[] parameters;
        private bool utcenabled = true;
        private string[] localdatecolumns;
        private string[] localdateparams;
        private bool forcerefresh;

        public string Sql
        {
            get
            {
                return sql;
            }
            set
            {
                sql = value;
            }
        }

        public string[] TableNames
        {
            get
            {
                if (tablenames == null)
                    tablenames = new string[1];

                return tablenames;
            }
            set
            {
                tablenames = value;
            }
        }

        public CommandType CommandType
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public bool ShemaOnly
        {
            get
            {
                return schemaOnly;
            }
            set
            {
                schemaOnly = value;
            }
        }

        public IDataParameter[] Parameters
        {
            get
            {
                return parameters;
            }
            set
            {
                parameters = value;
            }
        }

        public bool UTCDatesEnabled
        {
            get
            {
                return utcenabled;
            }
            set
            {
                utcenabled = value;
            }
        }


        public string[] LocalDateColumns
        {
            get
            {
                if (localdatecolumns == null)
                    localdatecolumns = new string[0];
                return localdatecolumns;
            }
            set
            {
                localdatecolumns = value;
            }
        }

        public string[] LocalDateParameters
        {
            get
            {
                if (localdateparams == null)
                    localdateparams = new string[0];
                return localdateparams;
            }
            set
            {
                localdateparams = value;
            }
        }

        public bool ForceRefresh
        {
            get
            {
                return forcerefresh;
            }
            set
            {
                forcerefresh = value;
            }
        }
    }
}
