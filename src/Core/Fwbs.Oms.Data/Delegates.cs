using System;
using System.Collections.Generic;
using System.Data;

namespace FWBS.OMS.Data
{
    public delegate void ExecuteTableEventHandler(Connection cnn, ExecuteTableEventArgs e);

    public class ExecuteTableEventArgs : ExecuteEventArgs
    {
        public ExecuteTableEventArgs(Connection cnn, string sql, CommandType commandType, bool schemaOnly, string tableName, IDataParameter[] pars, bool cached, bool refresh)
            :base(cnn, sql, commandType, pars)
        {

            if (tableName == null)
                tableName = String.Empty;
            if (pars == null)
                pars = new IDbDataParameter[0];


            this.schemaonly = schemaOnly;
            this.tablename = tableName;
            this.cached = cached;
            this.refresh = refresh;
        }

        private readonly bool schemaonly;
        public bool SchemaOnly
        {
            get
            {
                return schemaonly;
            }
        }


        private readonly string tablename;
        public string TableName
        {
            get
            {
                return tablename;
            }
        }

        private DataTable dt;
        public DataTable Data
        {
            get
            {
                return dt;
            }
            set
            {
                dt = value;
            }
        }

        private readonly bool cached;
        public bool Cached
        {
            get
            {
                return cached;
            }
        }

        private readonly bool refresh;
        public bool Refresh
        {
            get
            {
                return refresh;
            }
        }

    }


    public delegate void ExecuteDataSetEventHandler(Connection cnn, ExecuteDataSetEventArgs e);

    public class ExecuteDataSetEventArgs : ExecuteEventArgs
    {
        public ExecuteDataSetEventArgs(Connection cnn, string sql, CommandType commandType, bool schemaOnly, string[] tableNames, IDataParameter[] pars, bool cached, bool refresh)
            :base(cnn, sql, commandType, pars)
        {
            if (tableNames == null)
                tableNames = new string[0];

            this.schemaonly = schemaOnly;
            this.tablenames = tableNames;
            this.cached = cached;
            this.refresh = refresh;
        }

        private readonly bool schemaonly;
        public bool SchemaOnly
        {
            get
            {
                return schemaonly;
            }
        }


        private readonly string[] tablenames;
        public string[] TableNames
        {
            get
            {
                return tablenames;
            }
        }

        private DataSet ds;
        public DataSet Data
        {
            get
            {
                return ds;
            }
            set
            {
                ds = value;
            }
        }

        private readonly bool cached;
        public bool Cached
        {
            get
            {
                return cached;
            }
        }

        private readonly bool refresh;
        public bool Refresh
        {
            get
            {
                return refresh;
            }
        }      
    }

    public class ExecuteEventArgs : EventArgs
    {
        public ExecuteEventArgs(Connection cnn, string sql, CommandType commandType, IDataParameter[] pars)
        {
            if (String.IsNullOrEmpty(sql))
                throw new ArgumentNullException("sql");
            if (cnn == null)
                throw new ArgumentNullException("cnn");

            if (pars == null)
                pars = new IDbDataParameter[0];


            this.cnn = cnn;
            this.sql = sql;
            this.commandtype = commandType;
            this.pars = pars;
            this.AdditionalParameters = new List<IDataParameter>();
        }

        private readonly Connection cnn;

        private readonly string sql;
        public string SQL
        {
            get
            {
                return sql;
            }
        }

        public Connection Connection
        {
            get
            {
                return cnn;
            }
        }

        private readonly CommandType commandtype;
        public CommandType CommandType
        {
            get
            {
                return commandtype;
            }
        }

        private readonly IDataParameter[] pars;
        public IDataParameter[] Parameters
        {
            get
            {
                return pars;
            }
        }

        public List<IDataParameter> AdditionalParameters { get; private set; }


        public IDataParameter GetParameterByName(string name)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            name = cnn.BuildParameterName(name).ToUpperInvariant();
            foreach (IDataParameter par in pars)
            {
                if (par.ParameterName.ToUpperInvariant().Equals(name))
                    return par;

            }

            return null;
        }
    }

    public class LinkItemSavedEventArgs : EventArgs
    {
        public long versionID { get; set; }
        public string tableName { get; set; }
    }
}
