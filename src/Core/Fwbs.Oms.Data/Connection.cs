using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using FWBS.Common.Security.Cryptography;
using FWBS.OMS.Data.Exceptions;


namespace FWBS.OMS.Data
{

    /// <summary>
    /// A base connection object that makes any type of connection object transparent
    /// too the sata source it connects to.
    /// </summary>
    [Serializable()]
    public abstract class Connection : IDatabaseSchema, ICloneable, IDisposable, IConnection
    {
        #region Static Methods

        /// <summary>
        /// Gets a reference to a OMS connection object based on the multi database indesx passed.
        /// Certain types of connections like a SQL login connection also requires
        /// a user name and password to be passed.
        /// </summary>
        /// <param name="db">Database settings.</param>
        /// <param name="userName">User name to login to the server.</param>
        /// <param name="password">Password to authenticate on the server.</param>
        /// <returns>A connection reference.</returns>
        static public Connection GetOMSConnection(DatabaseSettings db, string userName, string password)
        {
            Connection cnn = null;

            string provider = db.Provider;
            string connectionString = db.ConnectionString;


            switch (db.LoginType)
            {
                case "SQL":
                    break;
                default:
                    userName = FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(db.UserName);
                    password = FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(db.Password);
                    break;

            }

            connectionString = connectionString.Replace("%SERVER%", db.Server);
            connectionString = connectionString.Replace("%DATABASE%", db.DatabaseName);
            connectionString = connectionString.Replace("%USER%", userName);
            connectionString = connectionString.Replace("%PASSWORD%", password);

            //If application roles in use then set the ;Pooling=false; option and set the username and password
            if (Encryption.NewKeyDecrypt(db.ApplicationRoleName) != "")
            {
                connectionString += ";Pooling=false";
            }
            switch (provider)
            {
                case "SQL":
                    if (Encryption.NewKeyDecrypt(db.ApplicationRoleName) == "")
                        cnn = new SQLConnection(connectionString, userName);
                    else
                        cnn = new SQLConnection(connectionString, userName, db.ApplicationRoleName, db.ApplicationRolePassword);
                    break;
                case "OLEDB":
                    break;
                case "ORACLE":
                    break;
                case "":
                    goto case "SQL";
                default:
                    throw new UnSupportedDataProviderException(provider);
            }

            cnn.alwaysconnected = db.ConnectionAlwaysOpen;
            return cnn;

        }


        /// <summary>
        /// Gets a connection object based on the connection string passed.
        /// </summary>
        /// <param name="connectionString">Connection string.</param>
        /// <returns>Connection object.</returns>
        public static Connection GetConnection(string connectionString)
        {
            return new OLEDBConnection(connectionString);
        }

        internal static void CheckForRowGuid(System.Data.DataTable dt, string rep_column)
        {
            if (rep_column == String.Empty) rep_column = Global.RowGuidCol;
            if (dt.Columns.Contains(Global.RowGuidCol))
            {
                if (dt.Columns[Global.RowGuidCol].DataType == typeof(System.Guid))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (CheckRowGuid(row))
                        {
                            row[Global.RowGuidCol] = System.Guid.NewGuid();
                        }
                    }
                }
            }
        }

        internal static void CheckForRowGuid(System.Data.DataRow row, string rep_column)
        {
            if (rep_column == String.Empty) rep_column = Global.RowGuidCol;
            if (row.Table.Columns.Contains(Global.RowGuidCol))
            {
                if (row.Table.Columns[Global.RowGuidCol].DataType == typeof(System.Guid))
                {
                    if (CheckRowGuid(row))
                    {
                        row[Global.RowGuidCol] = System.Guid.NewGuid();
                    }
                }
            }
        }

        private static bool CheckRowGuid(System.Data.DataRow row)
        {
            if (row.Table.ExtendedProperties.ContainsKey("KeyInitialized") &&
                (bool) row.Table.ExtendedProperties["KeyInitialized"])
            {
                return row.RowState == DataRowState.Added &&
                       (row[Global.RowGuidCol].ToString() == Guid.Empty.ToString() ||
                        string.IsNullOrEmpty(row[Global.RowGuidCol].ToString()));
            }

            return row.RowState == DataRowState.Added;
        }

        #endregion

        #region Fields

        /// <summary>
        /// Connection object reference.
        /// </summary>
        protected IDbConnection _cnn = null;

        /// <summary>
        /// Name of the server / database login.
        /// </summary>
        protected string _userName = "";

        /// <summary>
        /// A flag that tells the connection that the connection is going to stay open
        /// after an operation has completed.
        /// </summary>
        private bool _keepOpen = false;

        /// <summary>
        /// A current transaction object.
        /// </summary>
        private IDbTransaction _trans = null;

        /// <summary>
        /// Application Role Name.
        /// </summary>
        protected string _appRoleName = "";

        /// <summary>
        /// Application Role Password.
        /// </summary>
        protected string _appRolePassword = "";

        private DataSet schema = new DataSet();

        private bool alwaysconnected;

        private Dictionary<string, IDataParameter[]> procparamcache = new Dictionary<string, IDataParameter[]>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Events

        public event ConnectionErrorEventHandler ConnectionError = null;
        public event ExecuteTableEventHandler BeforeExecuteTable = null;
        public event ExecuteDataSetEventHandler BeforeExecuteDataSet = null;
        public event ExecuteTableEventHandler AfterExecuteTable = null;
        public event ExecuteDataSetEventHandler AfterExecuteDataSet = null;
        public event EventHandler ShutdownRequest = null;
        public event EventHandler<StateChangeEventArgs> StateChanged = null;
        public event EventHandler<ExecuteEventArgs> BeforeExecute = null;
        public event EventHandler<ExecuteEventArgs> AfterExecute = null;

        protected void OnConnectionError(ConnectionErrorEventArgs Args)
        {
            ConnectionErrorEventHandler ev = ConnectionError;
            if (ev != null)
                ev(this, Args);
        }

        protected void OnShutdownRequest()
        {
            EventHandler ev = ShutdownRequest;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }

        protected internal void OnBeforeExecuteTable(ExecuteTableEventArgs e)
        {
            ExecuteTableEventHandler ev = BeforeExecuteTable;
            if (ev != null)
                ev(this, e);
        }
        protected void OnBeforeExecuteDataSet(ExecuteDataSetEventArgs e)
        {
            ExecuteDataSetEventHandler ev = BeforeExecuteDataSet;
            if (ev != null)
                ev(this, e);
        }
        protected internal void OnAfterExecuteTable(ExecuteTableEventArgs e)
        {
            ExecuteTableEventHandler ev = AfterExecuteTable;
            if (ev != null)
                ev(this, e);
        }
        protected void OnAfterExecuteDataSet(ExecuteDataSetEventArgs e)
        {
            ExecuteDataSetEventHandler ev = AfterExecuteDataSet;
            if (ev != null)
                ev(this, e);
        }

        protected void OnStateChanged(StateChangeEventArgs e)
        {
            var ev = StateChanged;
            if (ev != null)
            {
                ev(this, e);
            }
        }

        protected void OnBeforeExecute(ExecuteEventArgs e)
        {
            var ev = BeforeExecute;
            if (ev != null)
            {
                ev(this, e);
            }
        }

        protected void OnAfterExecute(ExecuteEventArgs e)
        {
            var ev = AfterExecute;
            if (ev != null)
            {
                ev(this, e);
            }
        }

        #endregion

        #region Connection Methods

        /// <summary>
        /// Connects to the database using the previously built connection string.
        /// </summary>
        /// <param name="keepOpen">Keeps the database open until a forced disconnect is used.</param>
        public virtual void Connect(bool keepOpen)
        {
            try
            {
                if (keepOpen == true) KeepOpen = true;
                if (_cnn.State == ConnectionState.Closed)
                {
                    _cnn.Open();
                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Database Connected: " + _cnn.ConnectionString, Global.LogSwitch.DisplayName);
                }
                else
                    KeepOpen = true;
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
                throw ex;
            }
        }
        /// <summary>
        /// Connects to the database using the previously built connection string.
        /// </summary>
        public void Connect()
        {
            Connect(false);
        }

        /// <summary>
        /// Disconnects the internal connection object.
        /// </summary>
        public void Disconnect()
        {
            Disconnect(false);
        }

        /// <summary>
        /// Disconnects the internal connection object.
        /// </summary>
        /// <param name="force">Forces the connection to close.</param>
        public virtual void Disconnect(bool force)
        {
            if (force) KeepOpen = false;

            //Closes the connection if the connection exists and it is not stated to stay open.
            if (KeepOpen == false)
            {
                if ((_cnn != null) && (_cnn.State != ConnectionState.Closed))
                {
                    _cnn.Close();
                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Connection Closed", Global.LogSwitch.DisplayName);
                }
            }
        }

        #endregion

        #region Transaction Methods

        /// <summary>
        /// Gets the current transaction being used, otherwise null is returned.
        /// </summary>
        public IDbTransaction CurrentTransaction
        {
            get
            {
                return _trans;
            }
        }

        /// <summary>
        /// Begins a provider specific data transaction.
        /// </summary>
        public void BeginTransaction(IsolationLevel level)
        {
            if (CurrentTransaction == null)
                _trans = InternalConnection.BeginTransaction(level);
        }

        public void BeginTransaction()
        {
            if (CurrentTransaction == null)
                _trans = InternalConnection.BeginTransaction();
        }

        /// <summary>
        /// Commits the current transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (CurrentTransaction != null)
                _trans.Commit();
            _trans = null;
        }

        /// <summary>
        /// Rollback the current transaction.
        /// </summary>
        public void RollbackTransaction()
        {
            if (CurrentTransaction != null)
                _trans.Rollback();
            _trans = null;
        }

        #endregion

        #region UTC Schema Methods


        #endregion

        #region Data Retrieval Methods


        /// <summary>
        /// Executes a SQL statement and specifies a table name for the returned data.
        /// </summary>
        /// <param name="type">Type of execution.</param>
        /// <param name="sql">SQL statement</param>
        /// <param name="tableName">Table name for the returned data.</param>
        /// <param name="parameters">Parameters list.</param>
        /// <returns>Returns a data table based on the sql statement given.</returns>
        protected DataTable ExecuteSQLTable(CommandType type, string sql, string tableName, IDataParameter[] parameters)
        {
            var cp = new DataTableExecuteParameters();
            cp.CommandType = type;
            cp.Sql = sql;
            cp.Table = tableName;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return Execute(cp);
        }

        public DataTable ExecuteSQLTable(string sql, string tableName, IDataParameter[] parameters)
        {
            var cp = new DataTableExecuteParameters();
            cp.CommandType = CommandType.Text;
            cp.Sql = sql;
            cp.Table = tableName;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return Execute(cp);
        }

        public DataTable ExecuteProcedureTable(string sql, string tableName, IDataParameter[] parameters)
        {
            var cp = new DataTableExecuteParameters();
            cp.CommandType = CommandType.StoredProcedure;
            cp.Sql = sql;
            cp.Table = tableName;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return Execute(cp);
        }

        public DataTable ExecuteSQLTable(ConnectionExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            var p = ConvertExecuteParameters<DataTableExecuteParameters>(parameters);

            return Execute(p);
        }


        protected abstract DataTable InternalExecuteSQLTable(IDbCommand cmd, bool schemaOnly);

        /// <summary>
        /// Executes a select SQL statement and builds a named data table with the 
        /// schema from the database.
        /// </summary>
        /// <param name="type">Type of execution.</param>
        /// <param name="sql">SQL Statement.</param>
        /// <param name="tableName">Table name for the returned schema table.</param>
        /// <param name="schemaOnly"></param>
        /// <param name="parameters">Parameters list.</param>
        /// <param name="refresh"></param>
        /// <returns>Returns an empty data table based on the sql statement given.</returns>
        private DataTable ExecuteSQLTable(CommandType type, string sql, string tableName, bool schemaOnly, IDataParameter[] parameters, bool refresh)
        {
            bool cached = false;

            ExecuteTableEventArgs e = new ExecuteTableEventArgs(this, sql, type, schemaOnly, tableName, parameters, cached, refresh);
            OnBeforeExecuteTable(e);

            var pars = e.Parameters.Union(e.AdditionalParameters).ToArray();

            DataTable dt = null;
            if (e.Data == null || refresh)
            {
                IDbCommand cmd = CreateCommand();

                //Set the command properties.
                cmd.Connection = InternalConnection;
                cmd.CommandText = sql;
                cmd.CommandType = type;
                cmd.Transaction = CurrentTransaction;

                Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Command Text : " + sql, Global.LogSwitch.DisplayName);

                string erroutput = "";

                try
                {
                    Connect();

                    //Build the parameter list up if there are any.
                    erroutput = ApplyParameters(cmd, pars);

                    dt = InternalExecuteSQLTable(cmd, schemaOnly);
                }
                catch (Exception ex)
                {
                    Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
                    throw new ConnectionException("SQL Code Error" + Environment.NewLine + Environment.NewLine + "Connection : " + Environment.NewLine + "     " + _cnn.ConnectionString + Environment.NewLine + "Command Text : " + Environment.NewLine + "     " + sql + Environment.NewLine + "Parameters : " + Environment.NewLine + erroutput + Environment.NewLine + ex.Message, ex);
                }
                finally
                {
                    if (cmd != null) cmd.Parameters.Clear();
                    //Disconnect from the database if it wasn't already open.
                    try
                    {
                        Disconnect();
                    }
                    catch { } // Added Try Catch due to overall error not being thrown


                }

            }
            else
            {
                dt = e.Data;
                cached = true;
            }

            if (dt != null)
            {
                dt.TableName = tableName;
            }

            Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Executed Dataset Command", Global.LogSwitch.DisplayName);

            e = new ExecuteTableEventArgs(this, sql, type, schemaOnly, tableName, pars, cached, refresh);
            e.Data = dt;
            OnAfterExecuteTable(e);

            return dt;
        }


        public DataTable ExecuteSQLTable(string sql, string tableName, bool schemaOnly, IDataParameter[] parameters)
        {
            var cp = new DataTableExecuteParameters();
            cp.CommandType = CommandType.Text;
            cp.Sql = sql;
            cp.Table = tableName;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = schemaOnly;

            return Execute(cp);
        }

        public DataTable ExecuteProcedureTable(string sql, string tableName, bool schemaOnly, IDataParameter[] parameters)
        {
            var cp = new DataTableExecuteParameters();
            cp.CommandType = CommandType.StoredProcedure;
            cp.Sql = sql;
            cp.Table = tableName;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = schemaOnly;

            return Execute(cp);
        }

        protected abstract DataSet InternalExecuteSQLDataSet(IDbCommand cmd, bool schemaOnly);

        /// <summary>
        /// Retrieves a data set based on the sql statement passed.
        /// </summary>
        /// <param name="type">Type of execution.</param>
        /// <param name="sql">SQL statement. </param>
        /// <param name="schemaOnly"></param>
        /// <param name="tableNames">An array of table names that the database returns.</param>
        /// <param name="parameters">Parameters list.</param>
        /// <param name="refresh"></param>
        /// <returns>A data set containing one or more tables.</returns>
        private DataSet ExecuteSQLDataSet(CommandType type, string sql, bool schemaOnly, string[] tableNames, IDataParameter[] parameters, bool refresh)
        {
            bool cached = false;

            ExecuteDataSetEventArgs e = new ExecuteDataSetEventArgs(this, sql, type, schemaOnly, tableNames, parameters, cached, refresh);
            OnBeforeExecuteDataSet(e);

            var pars = e.Parameters.Union(e.AdditionalParameters).ToArray();

            DataSet ds = null;
            if (e.Data == null || refresh)
            {
                IDbCommand cmd = CreateCommand();

                //Set the command properties.
                cmd.Connection = InternalConnection;
                cmd.CommandText = sql;
                cmd.CommandType = type;
                cmd.Transaction = CurrentTransaction;

                Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Command Text : " + sql, Global.LogSwitch.DisplayName);


                string erroutput = "";

                try
                {
                    Connect();

                    //Build the parameter list up if there are any.
                    erroutput = ApplyParameters(cmd, pars);

                    ds = InternalExecuteSQLDataSet(cmd, schemaOnly);
                }
                catch (Exception ex)
                {
                    Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
                    throw new ConnectionException("SQL Code Error" + Environment.NewLine + Environment.NewLine + "Connection : " + Environment.NewLine + "     " + _cnn.ConnectionString + Environment.NewLine + "Command Text : " + Environment.NewLine + "     " + sql + Environment.NewLine + "Parameters : " + Environment.NewLine + erroutput + Environment.NewLine + ex.Message, ex);
                }
                finally
                {
                    if (cmd != null) cmd.Parameters.Clear();
                    //Disconnect from the database if it wasn't already open.
                    try
                    {
                        Disconnect();
                    }
                    catch { } // Added Try Catch due to overall error not being thrown

                }
            }
            else
            {
                ds = e.Data;
                cached = true;
            }

            if (ds.Tables.Count > 0)
            {
                for (int ctr = tableNames.GetLowerBound(0); ctr <= tableNames.GetUpperBound(0); ctr++)
                {
                    if (ds.Tables[ctr].TableName != null)
                        ds.Tables[ctr].TableName = tableNames[ctr];
                }
            }

            Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Executed Dataset Command", Global.LogSwitch.DisplayName);

            e = new ExecuteDataSetEventArgs(this, sql, type, schemaOnly, tableNames, pars, cached, refresh);
            e.Data = ds;
            OnAfterExecuteDataSet(e);

            return ds;
        }

        [Obsolete("Please use Execute(DataSetExecuteParameters) instead.")]
        public DataSet ExecuteSQLDataSet(ConnectionExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            var p = ConvertExecuteParameters<DataSetExecuteParameters>(parameters);

            return Execute(p);
        }

        public DataSet ExecuteSQLDataSet(string sql, string[] tableNames, IDataParameter[] parameters)
        {
            var cp = new DataSetExecuteParameters();
            cp.CommandType = CommandType.Text;
            cp.Sql = sql;
            cp.Tables = tableNames;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return Execute(cp);
        }

        public DataSet ExecuteProcedureDataSet(string sql, string[] tableNames, IDataParameter[] parameters)
        {
            var cp = new DataSetExecuteParameters();
            cp.CommandType = CommandType.StoredProcedure;
            cp.Sql = sql;
            cp.Tables = tableNames;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return Execute(cp);
        }

        public DataSet ExecuteSQLDataSet(string sql, bool schemaOnly, string[] tableNames, IDataParameter[] parameters)
        {
            var cp = new DataSetExecuteParameters();
            cp.CommandType = CommandType.Text;
            cp.Sql = sql;
            cp.Tables = tableNames;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = schemaOnly;

            return Execute(cp);
        }

        public DataSet ExecuteProcedureDataSet(string sql, bool schemaOnly, string[] tableNames, IDataParameter[] parameters)
        {
            var cp = new DataSetExecuteParameters();
            cp.CommandType = CommandType.StoredProcedure;
            cp.Sql = sql;
            cp.Tables = tableNames;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = schemaOnly;

            return Execute(cp);
        }

        /// <summary>
        /// Executes a sql statement that only returns one single value.
        /// </summary>
        /// <param name="type">Type of execution.</param>
        /// <param name="sql">SQL statement.</param>
        /// <param name="parameters">Parameters list.</param>
        /// <returns>A single value of any type.</returns>
        private object ExecuteSQLScalar(CommandType type, string sql, IDataParameter[] parameters)
        {

            ExecuteEventArgs e = new ExecuteEventArgs(this, sql, type, parameters);
            OnBeforeExecute(e);

            var pars = e.Parameters.Union(e.AdditionalParameters).ToArray();


            IDbCommand cmd = CreateCommand();

            //Set the command properties.
            cmd.Connection = InternalConnection;
            cmd.CommandText = sql;
            cmd.CommandType = type;
            cmd.Transaction = CurrentTransaction;


            object ret = null;

            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Command Text : " + sql, Global.LogSwitch.DisplayName);

            string erroutput = "";

            try
            {
                Connect();

                //Build the parameter list up if there are any.
                erroutput = ApplyParameters(cmd, pars);

                ret = InternalExecuteSQLScalar(cmd);
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
                throw new ConnectionException("SQL Code Error" + Environment.NewLine + Environment.NewLine + "Connection : " + Environment.NewLine + "     " + _cnn.ConnectionString + Environment.NewLine + "Command Text : " + Environment.NewLine + "     " + sql + Environment.NewLine + "Parameters : " + Environment.NewLine + erroutput + Environment.NewLine + ex.Message, ex);
            }
            finally
            {
                if (cmd != null) cmd.Parameters.Clear();
                //Disconnect from the database if it wasn't already open.
                try
                {
                    Disconnect();
                }
                catch { } // Added Try Catch due to overall error not being thrown


            }


            Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Executed Scalar Command", Global.LogSwitch.DisplayName);

            e = new ExecuteEventArgs(this, sql, type, pars);

            OnAfterExecute(e);

            return ret;
        }


        protected abstract object InternalExecuteSQLScalar(IDbCommand command);

        [Obsolete("Please use ExecuteScalar(ExecuteParameters) instead.")]
        public object ExecuteSQLScalar(ConnectionExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            var p = ConvertExecuteParameters<ExecuteParameters>(parameters);

            return ExecuteScalar(p);
        }

        public object ExecuteSQLScalar(string sql, IDataParameter[] parameters)
        {
            var cp = new ExecuteParameters();
            cp.CommandType = CommandType.Text;
            cp.Sql = sql;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return ExecuteScalar(cp);
        }

        public object ExecuteProcedureScalar(string sql, IDataParameter[] parameters)
        {
            var cp = new ExecuteParameters();
            cp.CommandType = CommandType.StoredProcedure;
            cp.Sql = sql;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return ExecuteScalar(cp);
        }

        /// <summary>
        /// Executes a select query that returns back a data reader for speedy data access.
        /// </summary>
        /// <param name="type">Type of execution.</param>
        /// <param name="sql">SQL statement.</param>
        /// <param name="parameters">Parameters list.</param>
        /// <returns>A data reader object reference.</returns>
        private IDataReader ExecuteSQLReader(CommandType type, string sql, IDataParameter[] parameters)
        {
            ExecuteEventArgs e = new ExecuteEventArgs(this, sql, type, parameters);
            OnBeforeExecute(e);

            var pars = e.Parameters.Union(e.AdditionalParameters).ToArray();


            IDbCommand cmd = CreateCommand();

            //Set the command properties.
            cmd.Connection = InternalConnection;
            cmd.CommandText = sql;
            cmd.CommandType = type;
            cmd.Transaction = CurrentTransaction;

            IDataReader rdr = null;

            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Command Text : " + sql, Global.LogSwitch.DisplayName);

            string erroutput = "";

            try
            {
                Connect();

                //Build the parameter list up if there are any.
                erroutput = ApplyParameters(cmd, pars);

                rdr = InternalExecuteSQLReader(cmd);
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
                throw new ConnectionException("SQL Code Error" + Environment.NewLine + Environment.NewLine + "Connection : " + Environment.NewLine + "     " + _cnn.ConnectionString + Environment.NewLine + "Command Text : " + Environment.NewLine + "     " + sql + Environment.NewLine + "Parameters : " + Environment.NewLine + erroutput + Environment.NewLine + ex.Message, ex);
            }
            
            return rdr;
        }


        protected abstract IDataReader InternalExecuteSQLReader(IDbCommand command);

        [Obsolete("Please use ExecuteReader(ExecuteParameters) instead.")]
        public IDataReader ExecuteSQLReader(ConnectionExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            var p = ConvertExecuteParameters<ExecuteParameters>(parameters);

            return ExecuteReader(p);
        }


        public IDataReader ExecuteSQLReader(string sql, IDataParameter[] parameters)
        {
            var cp = new ExecuteParameters();
            cp.CommandType = CommandType.Text;
            cp.Sql = sql;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return ExecuteReader(cp);
        }

        public IDataReader ExecuteProcedureReader(string sql, IDataParameter[] parameters)
        {
            var cp = new ExecuteParameters();
            cp.CommandType = CommandType.StoredProcedure;
            cp.Sql = sql;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return ExecuteReader(cp);
        }

        #endregion

        #region Data Manipulation Methods

        /// <summary>
        /// Executes an INSER, UPDATE or DELETE sql query to modify data within the database.
        /// </summary>
        /// <param name="type">Type of execution.</param>
        /// <param name="sql">DML SQL statement.</param>
        /// <param name="parameters">Parameters list.</param>
        /// <returns>The number of rows affected by the modification.</returns>
        private int ExecuteSQL(CommandType type, string sql, IDataParameter[] parameters)
        {
            var e = new ExecuteEventArgs(this, sql, type, parameters);
            OnBeforeExecute(e);

            var pars = e.Parameters.Union(e.AdditionalParameters).ToArray();

            IDbCommand cmd = CreateCommand();


            //Set the command properties.
            cmd.Connection = InternalConnection;
            cmd.CommandText = sql;
            cmd.CommandType = type;
            cmd.Transaction = CurrentTransaction;

            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Command Text : " + sql, Global.LogSwitch.DisplayName);

            int ret;

            string erroutput = "";

            try
            {
                Connect();

                //Build the parameter list up if there are any.
                erroutput = ApplyParameters(cmd, pars);

                ret = InternalExecuteSQL(cmd);
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
                throw new ConnectionException("SQL Code Error" + Environment.NewLine + Environment.NewLine + "Connection : " + Environment.NewLine + "     " + _cnn.ConnectionString + Environment.NewLine + "Command Text : " + Environment.NewLine + "     " + sql + Environment.NewLine + "Parameters : " + Environment.NewLine + erroutput + Environment.NewLine + ex.Message, ex);
            }
            finally
            {
                if (cmd != null) cmd.Parameters.Clear();
                //Disconnect from the database if it wasn't already open.
                try
                {
                    Disconnect();
                }
                catch { } // Added Try Catch due to overall error not being thrown

            }


            Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Executed Command", Global.LogSwitch.DisplayName);

            e = new ExecuteEventArgs(this, sql, type, pars);
            OnAfterExecute(e);

            return ret;
        }


        protected abstract int InternalExecuteSQL(IDbCommand command);

        [Obsolete("Please use Execute(ExecuteParameters) instead.")]
        public int ExecuteSQL(ConnectionExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            var p = ConvertExecuteParameters<ExecuteParameters>(parameters);

            return Execute(p);
        }

        public int ExecuteSQL(string sql, params IDataParameter[] parameters)
        {
            var cp = new ExecuteParameters();
            cp.CommandType = CommandType.Text;
            cp.Sql = sql;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return Execute(cp);
        }

        public int ExecuteProcedure(string sql, params IDataParameter[] parameters)
        {
            var cp = new ExecuteParameters();
            cp.CommandType = CommandType.StoredProcedure;
            cp.Sql = sql;
            if (parameters != null)
                cp.Parameters.AddRange(parameters);
            cp.SchemaOnly = false;

            return Execute(cp);
        }

        #endregion

        #region Parameter Methods

        private void ValidateDateParameters(ExecuteParameters parameters)
        {

            if (parameters.Parameters == null)
                return;

            foreach (IDataParameter param in parameters.Parameters)
            {
                if (param == null)
                    continue;

                if ((param.DbType == DbType.Date || param.DbType == DbType.DateTime) && param.Value is DateTime)
                {
                    DateTime dte = (DateTime)param.Value;
                    string name = param.ParameterName.Replace(ParameterPrefix, "");

                    DateTimeItem paritem;

                    if (parameters.DateTimeParameters.TryGetValue(name, out paritem))
                    {
                        param.Value = ConvertParameterDateValue(dte, paritem.Kind);
                    }
                    else
                    {
                        param.Value = ConvertParameterDateValue(dte, parameters.DefaultDateTimeKind);
                    }

                }
            }
        }

        private static DateTime ConvertParameterDateValue(DateTime dte, DateTimeKind dateTimeKind)
        {
            switch (dateTimeKind)
            {
                case DateTimeKind.Utc:
                    return dte.ToUniversalTime();
                case DateTimeKind.Local:
                    return dte.ToLocalTime();
                default:
                    return dte;
            }
        }

        /// <summary>
        /// Adds a parameter to a stored procedure.
        /// </summary>
        public IDataParameter CreateParameter(string paramName, SqlDbType dataType, int size, object val)
        {
            return AddParameter(paramName, dataType, ParameterDirection.Input, size, val);
        }


        public IDataParameter CreateParameter(string paramName, Type type, object val)
        {
            if (val == null || val == DBNull.Value)
            {
                SqlDbType dataType;
                if (type == typeof(byte)) dataType = SqlDbType.TinyInt;
                else if (type == typeof(short)) dataType = SqlDbType.SmallInt;
                else if (type == typeof(int)) dataType = SqlDbType.Int;
                else if (type == typeof(long)) dataType = SqlDbType.BigInt;
                else if (type == typeof(float)) dataType = SqlDbType.Real;
                else if (type == typeof(double)) dataType = SqlDbType.Float;
                else if (type == typeof(decimal)) dataType = SqlDbType.Decimal;
                else if (type == typeof(bool)) dataType = SqlDbType.Bit;
                else if (type == typeof(DateTime)) dataType = SqlDbType.DateTime;
                else if (type == typeof(string)) dataType = SqlDbType.NVarChar;
                else if (type == typeof(byte[])) dataType = SqlDbType.Image;
                else if (type == typeof(System.Text.StringBuilder)) dataType = SqlDbType.NText;
                else if (type == typeof(Guid)) dataType = SqlDbType.UniqueIdentifier;
                else if (type == typeof(DataTable)) dataType = SqlDbType.Structured;
                else dataType = SqlDbType.NVarChar;

                return AddParameter(paramName, dataType, ParameterDirection.Input, 0, val);

            }
            else
                return CreateParameter(paramName, val);

        }

        public IDataParameter CreateParameter(string paramName, object val)
        {
            SqlDbType dataType;
            if (val is byte) dataType = SqlDbType.TinyInt;
            else if (val is short) dataType = SqlDbType.SmallInt;
            else if (val is int) dataType = SqlDbType.Int;
            else if (val is long) dataType = SqlDbType.BigInt;
            else if (val is float) dataType = SqlDbType.Real;
            else if (val is double) dataType = SqlDbType.Float;
            else if (val is decimal) dataType = SqlDbType.Decimal;
            else if (val is bool) dataType = SqlDbType.Bit;
            else if (val is DateTime) dataType = SqlDbType.DateTime;
            else if (val is string) dataType = SqlDbType.NVarChar;
            else if (val is byte[]) dataType = SqlDbType.Image;
            else if (val is System.Text.StringBuilder) dataType = SqlDbType.NText;
            else if (val is Guid) dataType = SqlDbType.UniqueIdentifier;
            else if (val is DataTable) dataType = SqlDbType.Structured;
            else dataType = SqlDbType.NVarChar;


            return AddParameter(paramName, dataType, ParameterDirection.Input, 0, val);
        }

        /// <summary>
        /// Adds a parameter to a stored procedure.
        /// </summary>
        /// <param name="paramName">Parameter Name.</param>
        /// <param name="dataType">Data type.</param>
        /// <param name="size">Size of parameter.</param>
        /// <param name="val">Value of parameter.</param>
        public IDataParameter AddParameter(string paramName, SqlDbType dataType, int size, object val)
        {
            return AddParameter(paramName, dataType, ParameterDirection.Input, size, val);
        }

        public abstract IDataParameter AddParameter(string paramName, SqlDbType dataType, ParameterDirection direction, int size, object val);

        public IDataParameter AddParameter(string paramName, object val)
        {

            SqlDbType dataType;
            if (val is Int64) dataType = SqlDbType.BigInt;
            else
                if (val is Int32) dataType = SqlDbType.Int;
                else
                    if (val is Int16) dataType = SqlDbType.SmallInt;
                    else
                        if (val is byte) dataType = SqlDbType.TinyInt;
                        else
                            if (val is DateTime) dataType = SqlDbType.DateTime;
                            else
                                if (val is Boolean) dataType = SqlDbType.Bit;
                                else
                                    if (val is System.Text.StringBuilder) dataType = SqlDbType.NText;
                                    else
                                        if (val is byte[]) dataType = SqlDbType.Image;
                                        else
                                            if (val is System.Guid) dataType = SqlDbType.UniqueIdentifier;
                                            else
                                                if(val is System.Data.DataTable) dataType = SqlDbType.Structured;
                                            else
                                                dataType = SqlDbType.NVarChar;

            return AddParameter(paramName, dataType, ParameterDirection.Input, 0, val);
        }

        protected abstract void InternalDeriveParameters(IDbCommand command);

        private void DeriveParameters(IDbCommand command)
        {
            if (command.CommandType == CommandType.StoredProcedure && IsProcedure(command.CommandText))
            {
                IDataParameter[] pars;
                if (procparamcache.TryGetValue(command.CommandText, out pars))
                {
                    foreach (var p in pars)
                    {
                        var par = (IDataParameter)((ICloneable)p).Clone();
                        par.Value = null;
                        command.Parameters.Add(par);
                    }

                }
                else
                {
                    InternalDeriveParameters(command);
                    pars = command.Parameters.Cast<IDataParameter>().Select(p => (IDataParameter)((ICloneable)p).Clone()).ToArray();
                    procparamcache.Add(command.CommandText, pars);
                }
            }
        }

        /// <summary>
        /// Apply parameters to a specified data command object.
        /// </summary>
        /// <param name="cmd">Command object.</param>
        /// <param name="parameters">The parameters to apply.</param>
        protected string ApplyParameters(IDbCommand cmd, IDataParameter[] parameters)
        {
            bool prepopulated = cmd.Parameters.Count > 0;

            if (parameters == null) parameters = new IDataParameter[0];
            //Build the parameter list up if there are any.
            string output = "";
            foreach (IDataParameter par in parameters)
            {
                if (par == null) continue;
                try
                {
                    output = output + "     " + par.ParameterName + " - Type: " + par.DbType.ToString() + " - Value: " + Convert.ToString(par.Value) + Environment.NewLine;
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Parameter: " + par.ParameterName + " - Type: " + par.DbType.ToString() + " - Value: " + Convert.ToString(par.Value), Global.LogSwitch.DisplayName);
                }
                catch
                {
                    output = output + "Error in Parameter: " + par.ParameterName + Environment.NewLine;
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Error in Parameter: " + par.ParameterName, Global.LogSwitch.DisplayName);
                }
                //--- Add the Parameter object to the Command object
                // Added by MNW to check Null if int
                if (prepopulated)
                {
                    if (cmd.Parameters.Contains(par.ParameterName))
                    {
                        ((IDataParameter)cmd.Parameters[par.ParameterName]).Value = par.Value;
                    }
                    else
                    {
                        output += "Specified Parameter : " + par.ParameterName + " does not exist in derived list.";
                        Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Specified Parameter : " + par.ParameterName + " does not exist in derived list.", Global.LogSwitch.DisplayName);
                    }
                }
                else
                {
                    cmd.Parameters.Add(par);
                }
            }
            return output;
        }

        #endregion

        #region Update Methods

        /// <summary>
        /// Updates the database using a specified changed one table dataset.
        /// </summary>
        /// <param name="ds">Dataset object.</param>
        /// <param name="tableName">Table name within dataset.</param>
        /// <param name="selectStatement">Select statement.</param>
        public void Update(DataSet ds, string tableName, string selectStatement)
        {
            Update(ds, new string[1] { tableName }, new string[1] { selectStatement });
        }

        /// <summary>
        /// Updates all tables within the dataset by passing all the select statements to each 
        /// corresponding table.
        /// </summary>
        /// <param name="ds">Dataset object.</param>
        /// <param name="tableNames">A param array of tables names matching with the sql statement given.</param>
        /// <param name="selectStatements">Param array of select statements.</param>
        public void Update(DataSet ds, string[] tableNames, string[] selectStatements)
        {
            Connect();

            try
            {
                InternalUpdate(ds, tableNames, selectStatements);
            }
            catch (Exception ex)
            {
				Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
				throw new ConnectionException("Update Error" + Environment.NewLine + Environment.NewLine + "Connection : "
					+ Environment.NewLine + "     " + _cnn.ConnectionString					
					+ Environment.NewLine + ex.Message, ex);      
            }
            finally
            {
                Disconnect();
            }
        }

        protected abstract void InternalUpdate(DataSet ds, string[] tableNames, string[] selectStatements);

        /// <summary>
        /// Updates a data table.
        /// </summary>
        /// <param name="dt">Data table to update</param>
        /// <param name="selectStatement">Select statement.</param>
        [Obsolete("Please use the newer Update methods", false)]
        public void Update(DataTable dt, string selectStatement)
        {
            Connect();

            try
            {
                InternalUpdate(dt, selectStatement);
            }
            catch (Exception ex)
            {
				Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
				throw new ConnectionException("Update Error" + Environment.NewLine + Environment.NewLine + "Connection : "
					+ Environment.NewLine + "     " + _cnn.ConnectionString
					+ Environment.NewLine + ex.Message, ex);      
            }
            finally
            {
                Disconnect();
            }
        }

        protected abstract void InternalUpdate(DataTable dt, string selectStatement);

        /// <summary>
        /// Updates a singular row, this method should deal with getting back a new identity column
        /// value if the row was added.
        /// </summary>
        /// <param name="row">Row object by reference.</param>
        /// <param name="selectStatement">Select statement used by a command builder.</param>
        /// <param name="whereStatement">The where clause of the select statement.</param>
        /// <param name="identityColumn">The identity column name for addin new rows.</param>
        /// <param name="tableName">The table name of the returned table if the record needs refreshed.</param>
        /// <returns>A data table of refreshed data information with a new identity column.</returns>
        [Obsolete("Please use the newer Update methods", false)]
        public DataTable Update(DataRow row, string selectStatement, string whereStatement, string identityColumn, string tableName)
        {
            Connect();

            try
            {
                return InternalUpdate(row, selectStatement, whereStatement, identityColumn, tableName);
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
				throw new ConnectionException("Update Error" + Environment.NewLine + Environment.NewLine + "Connection : " 
					+ Environment.NewLine + "     " + _cnn.ConnectionString 
					+ Environment.NewLine + "Updating : " 
					+ Environment.NewLine + "     " + string.Format("{0} {1}",selectStatement,whereStatement)
					+ Environment.NewLine + ex.Message, ex);                
            }
            finally
            {
                Disconnect();
            }
        }


        protected abstract DataTable InternalUpdate(DataRow row, string selectStatement, string whereStatement, string identityColumn, string tableName);


        /// <summary>
        /// Updates the specified table using source column mapping and specific DML statements.
        /// </summary>
        /// <param name="dt">The data table to update.</param>
        /// <param name="insert">The insert sql statement or stored procedure.</param>
        /// <param name="update">The update sql statement or stored procedure.</param>
        /// <param name="delete">The delete sql statement or stored procedure.</param>
        /// <param name="parameters">The parameters to use.</param>
        [Obsolete("Please use the newer Update methods", false)]
        public void Update(DataTable dt, string insert, string update, string delete, IDataParameter[] parameters)
        {

            Connect();

            try
            {
                ExecuteParameters cp = new ExecuteParameters();

                if (parameters != null)
                    cp.Parameters.AddRange(parameters);

                ValidateDateParameters(cp);

                InternalUpdate(dt, insert, update, delete, cp.Parameters.ToArray());
            }
            catch (Exception ex)
            {
				Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
				throw new ConnectionException("Update Error" + Environment.NewLine + Environment.NewLine + "Connection : "
					+ Environment.NewLine + "     " + _cnn.ConnectionString
					+ Environment.NewLine + "     " + string.Format("Insert SQL: {1}{0}Update SQL:{2}{0}Delete SQL:{3}", Environment.NewLine, insert, update, delete)
					+ Environment.NewLine + ex.Message, ex);       
            }
            finally
            {
                Disconnect();
            }
        }

        protected abstract void InternalUpdate(DataTable dt, string insert, string update, string delete, IDataParameter[] parameters);


        /// <summary>
        /// Updates a singular row, this method should deal with getting back a new identity column
        /// value if the row was added.
        /// </summary>
        public void Update(DataRow row, string tableName, bool refresh, params string[] fields)
        {
            var up = new DataRowUpdateParameters(row);
            up.Table = tableName;
            up.Refresh = refresh;
            up.Fields = fields;
            Update(up);
        }

        protected abstract void InternalUpdate(DataRow row, string tableName, bool refresh, params string[] fields);

        public void Update(DataRow row, string tableName)
        {
            Update(row, tableName, true, new string[0]);
        }

        public void Update(DataTable dt, string tableName, bool refresh, params string[] fields)
        {
            var up = new DataTableUpdateParameters(dt);
            up.Refresh = refresh;
            up.Table = tableName;
            up.Fields = fields;
            Update(up);
        }

        protected abstract void InternalUpdate(DataTable dt, string tableName, bool refresh, params string[] fields);


        #endregion

        #region Schema Methods

        public bool IsTable(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            var vals = FWBS.Common.SQLRoutines.RemoveRubbish(name).Split('.');

            using (DataView dv = new DataView(GetTables()))
            {
                if (vals.Length > 1)
                {
                    var schema = FWBS.Common.SQLRoutines.RemoveRubbish(vals[vals.Length - 2]);
                    var objname = FWBS.Common.SQLRoutines.RemoveRubbish(vals[vals.Length - 1]);

                    dv.RowFilter = String.Format("[SCHEMA] = '{1}' AND [TABLE_NAME] = '{0}'", objname, schema);

                    return dv.Count > 0;
                }
                else
                {
                    dv.RowFilter = String.Format("[TABLE_NAME] = '{0}'", name);
                    return dv.Count > 0;
                }


            }
        }

        public bool IsProcedure(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            using (var vw = new DataView(GetProcedures()))
            {
                return FilterByProcedure(name, vw);
            }
        }

        private bool FilterByProcedure(string name, DataView dv)
        {
            var vals = FWBS.Common.SQLRoutines.RemoveRubbish(name).Split('.');


            if (vals.Length > 1)
            {
                var schema = FWBS.Common.SQLRoutines.RemoveRubbish(vals[vals.Length - 2]);
                var objname = FWBS.Common.SQLRoutines.RemoveRubbish(vals[vals.Length - 1]);

                dv.RowFilter = String.Format("[SCHEMA] = '{1}' AND [PROCEDURE_NAME] = '{0}'", objname, schema);

                return dv.Count > 0;
            }
            else
            {
                dv.RowFilter = String.Format("[PROCEDURE_NAME] = '{0}'", name);
                return dv.Count > 0;
            }
        }

        private DataTable tables = null;
        /// <summary>
        /// Returns a list of tables within the database.
        /// </summary>
        /// <returns>A data table of tables.</returns>
        public virtual DataTable GetTables()
        {
            if (tables != null)
                return tables;
            DataTable dt = new DataTable("TABLES");
            dt.Columns.Add("TABLE_NAME", typeof(string));
            dt.Columns.Add("DESCRIPTION", typeof(string));
            dt.Columns.Add("SCHEMA", typeof(string));
            PopulateTables(dt);
            tables = dt.Copy();
            return dt;
        }

        protected virtual void PopulateTables(DataTable data)
        {
        }

        private DataTable views = null;
        /// <summary>
        /// Returns a list of views within the database.
        /// </summary>
        /// <returns>A data table of views.</returns>
        public virtual DataTable GetViews()
        {
            if (views != null)
                return views;
            DataTable dt = new DataTable("VIEWS");
            dt.Columns.Add("TABLE_NAME", typeof(string));
            dt.Columns.Add("DESCRIPTION", typeof(string));
            dt.Columns.Add("SCHEMA", typeof(string));
            PopulateViews(dt);
            views = dt.Copy();
            return dt;
        }

        protected virtual void PopulateViews(DataTable data)
        {
        }

        /// <summary>
        /// Returns a list of columns within a specified table / view.
        /// </summary>
        /// <param name="tableName">Table / view name.</param>
        /// <returns>A data table of columns.</returns>
        public virtual DataTable GetColumns(string tableName)
        {
            DataTable dt = new DataTable("COLUMNS");
            dt.Columns.Add("COLUMN_NAME", typeof(string));
            dt.Columns.Add("DATA_TYPE", typeof(string));
            dt.Columns.Add("DESCRIPTION", typeof(string));
            if (string.IsNullOrWhiteSpace(tableName))
                return dt;
            PopulateTableColumns(tableName, dt);
            return dt;
        }

        protected virtual void PopulateTableColumns(string name, DataTable data)
        {

        }


        private DataTable procs = null;
        /// <summary>
        /// Returns a list of stored procedures within the database.
        /// </summary>
        /// <returns>A data table of stored procedures.</returns>
        public DataTable GetProcedures()
        {
            if (procs != null)
                return procs;
            DataTable dt = new DataTable("PROCEDURES");
            dt.Columns.Add("PROCEDURE_NAME", typeof(string));
            dt.Columns.Add("DESCRIPTION", typeof(string));
            dt.Columns.Add("VERSION", typeof(int));
            dt.Columns.Add("SCHEMA", typeof(string));
            PopulateProcedures(dt);
            procs = dt.Copy();
            return dt;
        }

        protected virtual void PopulateProcedures(DataTable data)
        {
        }


        /// <summary>
        /// Returns a list of columns within a specified table / view.
        /// </summary>
        /// <param name="procedureName">Procedure name.</param>
        /// <returns>A data table of parameters.</returns>
        public DataTable GetParameters(string procedureName)
        {
            DataTable dt = new DataTable("PARAMETERS");
            dt.Columns.Add("PARAMETER_NAME", typeof(string));
            dt.Columns.Add("DATA_TYPE", typeof(string));
            dt.Columns.Add("DESCRIPTION", typeof(string));
            dt.Columns.Add("SCHEMA", typeof(string));
            dt.Columns.Add("PROCEDURE_NAME", typeof(string));

            PopulateProcedureParameters(procedureName, dt);

            if (String.IsNullOrWhiteSpace(procedureName))
                return dt;

            using (var vw = new DataView(dt))
            {
                FilterByProcedure(procedureName, vw);
                return vw.ToTable();
            }
        }

        protected virtual void PopulateProcedureParameters(string procedureName, DataTable data)
        {
        }

        /// <summary>
        /// Fetches the primary key field name of a particular table.
        /// </summary>
        /// <param name="tableName">Table name within the database.</param>
        /// <returns>Data table of primary key information.</returns>
        public DataTable GetPrimaryKey(string tableName)
        {
            DataTable dt = new DataTable("PK_COLUMNS");
            dt.Columns.Add("COLUMN_NAME", typeof(string));

            if (string.IsNullOrWhiteSpace(tableName))
                return dt;

            PopulatePrimaryKey(tableName, dt);

            return dt;
        }

        protected virtual void PopulatePrimaryKey(string tableName, DataTable data)
        {
        }

        /// <summary>
        /// Fetches the list of columns that the stored procedure is going to return back.
        /// </summary>
        /// <param name="procedureName">Stored procedure name.</param>
        /// <returns>A data table of columns.</returns>
        public virtual DataTable GetProcedureColumns(string procedureName)
        {
            DataTable dt = new DataTable("COLUMNS");
            dt.Columns.Add("COLUMN_NAME", typeof(string));
            dt.Columns.Add("DATA_TYPE", typeof(int));
            dt.Columns.Add("DESCRIPTION", typeof(string));

            if (string.IsNullOrWhiteSpace(procedureName))
                return dt;

            PopulateProcedureColumns(procedureName, dt);

            return dt;
        }

        protected virtual void PopulateProcedureColumns(string procedureName, DataTable table)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a provider specific command.
        /// </summary>
        /// <returns></returns>
        public IDbCommand CreateCommand()
        {
            IDbCommand cmd = InternalConnection.CreateCommand();
            cmd.Transaction = _trans;
            cmd.CommandTimeout = CommandTimeout;
            return cmd;
        }

        /// <summary>
        /// Returns a parameter name string for a where clause of a sql statement.
        /// </summary>
        public virtual string BuildParameterName(string name)
        {
            return ParameterPrefix + name;
        }

        /// <summary>
        /// Fetches a list of linked servers associated to the server.
        /// </summary>
        /// <returns>A data table of linked servers.</returns>
        public virtual DataTable GetLinkedServers()
        {
            DataTable dt = new DataTable("LINKED_SERVERS");
            dt.Columns.Add("SRV_NAME", typeof(string));
            dt.Columns.Add("SRV_PRODUCT", typeof(string));
            return dt;
        }


        #endregion

        #region Properties


        protected bool AlwaysConnected
        {
            get
            {
                return alwaysconnected;
            }
        }

        /// <summary>
        /// Name of the server / database login.
        /// </summary>
        protected string UserName
        {
            get
            {
                return _userName;
            }
        }

        /// <summary>
        /// A flag that tells the connection that the connection is going to stay open
        /// after an operation has completed.
        /// </summary>
        protected bool KeepOpen
        {
            get
            {
                if (AlwaysConnected)
                    return true;

                return _keepOpen;
            }
            set
            {
                _keepOpen = value;
            }
        }

        /// <summary>
        /// Application Role Name.
        /// </summary>
        protected string ApplicationRole
        {
            get
            {
                return _appRoleName;
            }
        }

        /// <summary>
        /// Application Role Password.
        /// </summary>
        protected string ApplicationRolePassword
        {
            get
            {
                return _appRolePassword;
            }
        }


        /// <summary>
        /// Gets the parameter prefix of this type of provider.
        /// </summary>
        public abstract string ParameterPrefix { get; }

        /// <summary>
        /// Gets the actual ADO.NET connection object.
        /// </summary>
        protected internal IDbConnection InternalConnection
        {
            get
            {
                return _cnn;
            }
        }

        /// <summary>
        /// Gets a boolean value indicating whether the connection is currently open.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return (_cnn.State == ConnectionState.Open || _cnn.State == ConnectionState.Executing || _cnn.State == ConnectionState.Fetching);
            }
        }

        /// <summary>
        /// Gets a boolean value indicating whether the connection is currently executing a prcoedure or fetching records.
        /// </summary>
        public bool IsExecuting
        {
            get
            {
                return (_cnn.State == ConnectionState.Executing || _cnn.State == ConnectionState.Fetching);
            }
        }

        #endregion

        #region Timeout

        private int? timeout;
        public int ConcurrentQueryTimeout
        {
            get
            {
                if (timeout == null)
                    timeout = FWBS.Common.ConvertDef.ToInt32(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Data", "QueryTimeout").GetSetting(60), 60);

                return timeout.Value;
            }
        }

        protected TimeoutException GetConcurrentQueryTimeoutException()
        {
            throw new TimeoutException("Connection is busy with another command.");
        }

        private int? _commandTimeout;
        /// <summary>
        /// The maximum number of seconds a query can run for before a Command Timeout exception is thrown.
        /// This will only affect ExecuteSQLDataSet and ExecuteSQLTable
        /// It is mapped to the registry key ...\Data\CommandTimeout with a default of 120 seconds
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                if (_commandTimeout == null)
                    _commandTimeout = FWBS.Common.ConvertDef.ToInt32(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Data", "CommandTimeout").GetSetting(120), 120);

                return _commandTimeout.Value;
            }
        }

        private int runningthreadid = 0;

        protected void LockExecution()
        {
            int ctr = 0;

            while (runningthreadid > 0 && runningthreadid != System.Threading.Thread.CurrentThread.ManagedThreadId)
            {
                System.Threading.Thread.Sleep(1000);
                ctr++;

                if (ctr > ConcurrentQueryTimeout)
                    throw GetConcurrentQueryTimeoutException();
            }

            runningthreadid = System.Threading.Thread.CurrentThread.ManagedThreadId;
        }

        protected void UnLockExecution()
        {
            runningthreadid = 0;
        }

        #endregion


        #region ICloneable Members

        public abstract object Clone();


        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this._trans != null)
                {
                    _trans.Dispose();
                    _trans = null;
                }

                if (_cnn != null)
                {
                    _cnn.Dispose();
                    _cnn = null;
                }
            }
        }

        #endregion

        #region IConnection

        private static T ConvertExecuteParameters<T>(ConnectionExecuteParameters parameters)
            where T : ExecuteParameters, new ()
        {
            T ret = new T();

            var dt = ret as DataTableExecuteParameters;
            var ds = ret as DataSetExecuteParameters;

            if (dt != null)
            {
                dt.Table = parameters.TableNames[0];
            }
            if (ds != null)
            {
                ds.Tables = parameters.TableNames;
            }

            ret.CommandType = parameters.CommandType;
            ret.Refresh = parameters.ForceRefresh;
            ret.SchemaOnly = parameters.ShemaOnly;
            ret.Sql = parameters.Sql;
            if (parameters.Parameters != null)
                ret.Parameters.AddRange(parameters.Parameters);

            AddLocalDateTimeItems(parameters.LocalDateColumns, ret.DateTimeColumns);
            AddLocalDateTimeItems(parameters.LocalDateParameters, ret.DateTimeParameters);
         
            ret.DefaultDateTimeKind = parameters.UTCDatesEnabled ? DateTimeKind.Utc : DateTimeKind.Local;

            return ret;
        }

        private static void AddLocalDateTimeItems(IEnumerable<string> source, IDictionary<string, DateTimeItem> destination)
        {
            if (source == null)
                return;

            foreach (var item in source)
            {
                destination[item] = new DateTimeItem() { Kind = DateTimeKind.Local };
            }
        }

        public DataTable Execute(DataTableExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateDateParameters(parameters);

            var results = ExecuteSQLTable(parameters.CommandType, parameters.Sql, parameters.Table, parameters.SchemaOnly, parameters.Parameters.ToArray(), parameters.Refresh);

            var schema = results.Clone();

            foreach (DataColumn col in schema.Columns)
            {
                if (col.DataType == typeof(DateTime))
                {
                    DateTimeItem colitem;

                    if (parameters.DateTimeColumns.TryGetValue(col.ColumnName, out colitem))
                    {
                        col.DateTimeMode =  ConvertToDataSetDateTime(colitem.Kind);
                    }
                    else
                    {
                        col.DateTimeMode = ConvertToDataSetDateTime(parameters.DefaultDateTimeKind);
                    }
                }
            }

            schema.Merge(results, false, MissingSchemaAction.Ignore);
            schema.AcceptChanges();
            return schema;

        }

        private static DataSetDateTime ConvertToDataSetDateTime(DateTimeKind dateTimeKind)
        {
            switch (dateTimeKind)
            {
                case DateTimeKind.Local:
                    return DataSetDateTime.Local;
                case DateTimeKind.Unspecified:
                    return DataSetDateTime.UnspecifiedLocal;
                case DateTimeKind.Utc:
                    return DataSetDateTime.Utc;
                default:
                    return DataSetDateTime.UnspecifiedLocal;
            }
        }

        public DataSet Execute(DataSetExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateDateParameters(parameters);

            var results = ExecuteSQLDataSet(parameters.CommandType, parameters.Sql, parameters.SchemaOnly, parameters.Tables, parameters.Parameters.ToArray(), parameters.Refresh);


            DataSet schema = results.Clone();


            foreach (DataTable dt in schema.Tables)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    if (col.DataType == typeof(DateTime))
                    {
                        DateTimeItem colitem;

                        if (parameters.DateTimeColumns.TryGetValue(col.ColumnName, out colitem))
                        {
                            col.DateTimeMode = ConvertToDataSetDateTime(colitem.Kind);
                        }
                        else
                        {
                            col.DateTimeMode = ConvertToDataSetDateTime(parameters.DefaultDateTimeKind);
                        }
                    }
                }
            }
            
            schema.Merge(results, false, MissingSchemaAction.Ignore);
            schema.AcceptChanges();
            return schema;

        }

        public int Execute(ExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateDateParameters(parameters);

            return ExecuteSQL(parameters.CommandType, parameters.Sql, parameters.Parameters.ToArray());
        }

        public IDataReader ExecuteReader(ExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateDateParameters(parameters);

            var reader = ExecuteSQLReader(parameters.CommandType, parameters.Sql, parameters.Parameters.ToArray());

            if (Global.ExecuteReaderUseExecuteParameters)
                return new DataReaderWrapper(reader, parameters);
            else
                return reader;
        }

        public object ExecuteScalar(ExecuteParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            ValidateDateParameters(parameters);

            object result = ExecuteSQLScalar(parameters.CommandType, parameters.Sql, parameters.Parameters.ToArray());

            if (result is DateTime)
            {
                DateTime date = (DateTime)result;

                var col = parameters.DateTimeColumns.Values.FirstOrDefault();

                if (col == null)
                    return DateTime.SpecifyKind(date, parameters.DefaultDateTimeKind);
                else
                    return DateTime.SpecifyKind(date, col.Kind);
            }


            return result;
        }

        public void Update(DataTableUpdateParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            Connect();

            try
            {
                InternalUpdate(parameters.Data, parameters.Table, parameters.Refresh, parameters.Fields);
            }
            catch (Exception ex)
            {
				Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
				throw new ConnectionException("Update Error" + Environment.NewLine + Environment.NewLine + "Connection : "
					+ Environment.NewLine + "     " + _cnn.ConnectionString
					+ Environment.NewLine + ex.Message, ex);      
            }
            finally
            {
                Disconnect();
            }
        }

        public void Update(DataRowUpdateParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException("parameters");

            Connect();

            try
            {
                InternalUpdate(parameters.Data, parameters.Table, parameters.Refresh, parameters.Fields);
            }
            catch (Exception ex)
            {
				Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
				throw new ConnectionException("Update Error" + Environment.NewLine + Environment.NewLine + "Connection : "
					+ Environment.NewLine + "     " + _cnn.ConnectionString			
					+ Environment.NewLine + ex.Message, ex);      
            }
            finally
            {
                Disconnect();
            }
        }

        #endregion
    }
}
