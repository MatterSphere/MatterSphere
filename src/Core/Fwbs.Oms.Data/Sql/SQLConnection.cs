using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using FWBS.Common.Security.Cryptography;
using FWBS.OMS.Data.Exceptions;


namespace FWBS.OMS.Data
{

    /// <summary>
    /// Connection class that creates a sql server specific connection.
    /// </summary>
    [Serializable()]
    public class SQLConnection : Connection
    {
        #region Constructors

        private SQLConnection() { }

        /// <summary>
        /// Creates a sql server based connection by giving it a connection string
        /// to use.
        /// </summary>
        /// <param name="connectionString">The sql specific connection string.</param>
        /// <param name="userName">User name.</param>
        internal SQLConnection(string connectionString, string userName)
        {
            _userName = userName;
            _cnn = new SqlConnection(connectionString);
        }
        /// <summary>
        /// Creates a sql server based connection by giving it a connection string
        /// to use.
        /// </summary>
        /// <param name="connectionString">The sql specific connection string.</param>
        /// <param name="userName">User name.</param>
        /// <param name="appRoleName">The application role name to use for connection</param>
        /// <param name="appRolePassword">The application role passwords to use for the connection</param>
        internal SQLConnection(string connectionString, string userName, string appRoleName, string appRolePassword)
        {
            _userName = userName;
            _cnn = new SqlConnection(connectionString);
            _appRoleName = appRoleName;
            _appRolePassword = appRolePassword;

        }
        #endregion

        #region Connection Methods

        /// <summary>
        /// Connects to the a SqlClient connection object.
        /// </summary>
        /// <param name="keepOpen">Keeps the database open until a forced disconnect is used.</param>
        public override void Connect(bool keepOpen)
        {
        Retry:
            //If application roles in place, then keep the connection open
            if (_appRoleName != "")
                keepOpen = true;

            try
            {
                if (keepOpen == true) KeepOpen = true;

                ConnectionState state = _cnn.State;

                int tries = 0;
                while (state == ConnectionState.Connecting || state == ConnectionState.Executing || state == ConnectionState.Fetching || tries >= 20)
                {
                    tries++;
                    state = _cnn.State;
                    System.Threading.Thread.Sleep(100);
                }

                if (_cnn.State == ConnectionState.Closed || _cnn.State == ConnectionState.Broken)
                {
                    SqlConnection cnn = (SqlConnection)_cnn;
                    cnn.InfoMessage -= new SqlInfoMessageEventHandler(this.InfoMessage);
                    cnn.InfoMessage += new SqlInfoMessageEventHandler(this.InfoMessage);
                    cnn.StateChange -= new StateChangeEventHandler(this.cnn_StateChanged);
                    cnn.StateChange += new StateChangeEventHandler(this.cnn_StateChanged);
                    cnn.Open();

                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "SQL Database Connected: " + _cnn.ConnectionString, Global.LogSwitch.DisplayName);
                }
                else
                    KeepOpen = true;
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                bool? ProcessCancelled;

                Exception exc = ValidateException(ex, null, out ProcessCancelled);
                if (ProcessCancelled == null)
                    throw exc;
                else if (ProcessCancelled.Value)
                {
                    OnShutdownRequest();
                    throw exc;
                }
                else
                    goto Retry;
            }

        }

        private void ApplyApplicationRole()
        {
            if (_appRoleName != "")
            {
                try
                {
                    //Declare application role command
                    System.Data.SqlClient.SqlCommand _approle = new System.Data.SqlClient.SqlCommand();
                    _approle.CommandType = CommandType.StoredProcedure;
                    _approle.CommandText = "sp_setapprole";
                    _approle.Parameters.AddWithValue("@ROLENAME", Encryption.NewKeyDecrypt(_appRoleName));
                    _approle.Parameters.AddWithValue("@PASSWORD", Encryption.NewKeyDecrypt(_appRolePassword));
                    _approle.Connection = (System.Data.SqlClient.SqlConnection)_cnn;
                    _approle.ExecuteNonQuery();
                }
                catch
                {
                    KeepOpen = false;
                    throw;
                }
            }
        }

        #endregion

        #region Data Retrieval Methods



        protected override DataTable InternalExecuteSQLTable(IDbCommand cmd, bool schemaOnly)
        {
            try
            {
                LockExecution();

            Retry:


                //Execute the Sql Satement.
                //Fill the data type.
                try
                {
                    SqlDataAdapter adpt = new SqlDataAdapter();
                    adpt.SelectCommand = (SqlCommand)cmd;

                    DataTable dt = new DataTable();
                    if (schemaOnly)
                        adpt.FillSchema(dt, SchemaType.Source);
                    else
                        adpt.Fill(dt);

                    return dt;

                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    bool? ProcessCancelled;

                    Exception exc = ValidateException(ex, null, out ProcessCancelled);
                    if (ProcessCancelled == null)
                        throw exc;
                    else if (ProcessCancelled.Value)
                    {
                        OnShutdownRequest();
                        throw exc;
                    }
                    else
                        goto Retry;
                }
            }
            finally
            {
                UnLockExecution();
            }

        }


        protected override DataSet InternalExecuteSQLDataSet(IDbCommand cmd, bool schemaOnly)
        {
            try
            {
                LockExecution();

            Retry:

                //Execute the Sql Satement.
                //Fill the data type.
                try
                {
                    SqlDataAdapter adpt = new SqlDataAdapter();
                    adpt.SelectCommand = (SqlCommand)cmd;

                    DataSet ds = new DataSet();
                    if (schemaOnly)
                        adpt.FillSchema(ds, SchemaType.Source);
                    else
                        adpt.Fill(ds);

                    return ds;

                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    bool? ProcessCancelled;

                    Exception exc = ValidateException(ex, null, out ProcessCancelled);
                    if (ProcessCancelled == null)
                        throw exc;
                    else if (ProcessCancelled.Value)
                    {
                        OnShutdownRequest();
                        throw exc;
                    }
                    else
                        goto Retry;
                }
            }
            finally
            {
                UnLockExecution();
            }
        }


        protected override object InternalExecuteSQLScalar(IDbCommand command)
        {
            try
            {
                LockExecution();

            Retry:


                try
                {
                    return command.ExecuteScalar();

                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    bool? ProcessCancelled;

                    Exception exc = ValidateException(ex, null, out ProcessCancelled);
                    if (ProcessCancelled == null)
                        throw exc;
                    else if (ProcessCancelled.Value)
                    {
                        OnShutdownRequest();
                        throw exc;
                    }
                    else
                        goto Retry;
                }
            }
            finally
            {
                UnLockExecution();
            }
        }


        protected override IDataReader InternalExecuteSQLReader(IDbCommand command)
        {
            try
            {
                LockExecution();

            Retry:


                try
                {
                    return command.ExecuteReader((KeepOpen == true ? System.Data.CommandBehavior.Default : System.Data.CommandBehavior.CloseConnection));

                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    bool? ProcessCancelled;

                    Exception exc = ValidateException(ex, null, out ProcessCancelled);
                    if (ProcessCancelled == null)
                        throw exc;
                    else if (ProcessCancelled.Value)
                    {
                        OnShutdownRequest();
                        throw exc;
                    }
                    else
                        goto Retry;
                }
            }
            finally
            {
                UnLockExecution();
            }
          
        }


        #endregion

        #region Data Manipulation Methods

  
        protected override int InternalExecuteSQL(IDbCommand command)
        {
            try
            {
                LockExecution();

            Retry:

                try
                {
                    return command.ExecuteNonQuery();

                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    bool? ProcessCancelled;

                    Exception exc = ValidateException(ex, null, out ProcessCancelled);
                    if (ProcessCancelled == null)
                        throw exc;
                    else if (ProcessCancelled.Value)
                    {
                        OnShutdownRequest();
                        throw exc;
                    }
                    else
                        goto Retry;
                }
            }
            finally
            {
                UnLockExecution();
            }

        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Captures any errors or print messages within transact SQL.
        /// </summary>
        /// <param name="sender">The SQL connection.</param>
        /// <param name="e">Infomrational event arguments.</param>
        private void InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            Trace.WriteLineIf(Global.LogSwitch.TraceWarning, "Info Message: " + e.Message, Global.LogSwitch.DisplayName);

            foreach (SqlError err in e.Errors)
                if (err.Message != e.Message)
                    Trace.WriteLineIf(Global.LogSwitch.TraceError, "Info Message Error: " + err.Message + " - Severity: " + err.Class.ToString() + " - Procedure: " + err.Procedure + " - Server: " + err.Server + "Source: " + err.Source, Global.LogSwitch.DisplayName);

        }

        private void cnn_StateChanged(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Open)
                ApplyApplicationRole();

            OnStateChanged(e);
        }

        #endregion

        #region Parameter Methods

        protected override void InternalDeriveParameters(IDbCommand command)
        {
            SqlCommandBuilder.DeriveParameters((SqlCommand)command);
        }

        public override IDataParameter AddParameter(string paramName, SqlDbType dataType, ParameterDirection direction, int size, object val)
        {
            if (paramName.Substring(0, 1) != "@") paramName = "@" + paramName;
            SqlParameter par = new SqlParameter(paramName, dataType, size);
            par.Direction = direction;
            par.Value = val;
            return par;
        }

        #endregion

        #region Update Methods
        /// <summary>
        /// Updates the specified table using source column mapping and specific DML statements.
        /// </summary>
        /// <param name="dt">The data table to update.</param>
        /// <param name="insert">The insert sql statement or stored procedure.</param>
        /// <param name="update">The update sql statement or stored procedure.</param>
        /// <param name="delete">The delete sql statement or stored procedure.</param>
        /// <param name="parameters">The parameters to use.</param>
        protected override void InternalUpdate(DataTable dt, string insert, string update, string delete, IDataParameter[] parameters)
        {
            try
            {
                LockExecution();

                SqlDataAdapter adpt = new SqlDataAdapter();

                try
                {
                    if (insert != "" && insert != null)
                    {
                        SqlCommand cmd = new SqlCommand(insert, (SqlConnection)_cnn);
                        if (insert.Trim().ToUpper().StartsWith("INSERT"))
                            cmd.CommandType = CommandType.Text;
                        else
                            cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Transaction = CurrentTransaction as SqlTransaction;
                        //Build the parameter list up if there are any.
                        base.ApplyParameters(cmd, parameters);
                        adpt.InsertCommand = cmd;
                        Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable INSERT: " + cmd.CommandText, Global.LogSwitch.DisplayName);
                    }

                    if (update != "" && update != null)
                    {
                        SqlCommand cmd = new SqlCommand(update, (SqlConnection)_cnn);
                        if (update.Trim().ToUpper().StartsWith("UPDATE"))
                            cmd.CommandType = CommandType.Text;
                        else
                            cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Transaction = CurrentTransaction as SqlTransaction;
                        //Build the parameter list up if there are any.
                        base.ApplyParameters(cmd, parameters);
                        adpt.UpdateCommand = cmd;
                        Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable UPDATE: " + cmd.CommandText, Global.LogSwitch.DisplayName);
                    }

                    if (delete != "" && delete != null)
                    {
                        SqlCommand cmd = new SqlCommand(delete, (SqlConnection)_cnn);
                        if (delete.Trim().ToUpper().StartsWith("DELETE"))
                            cmd.CommandType = CommandType.Text;
                        else
                            cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Transaction = CurrentTransaction as SqlTransaction;
                        //Build the parameter list up if there are any.
                        base.ApplyParameters(cmd, parameters);
                        adpt.DeleteCommand = cmd;
                        Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable DELETE: " + cmd.CommandText, Global.LogSwitch.DisplayName);
                    }

                    DataTable tdt = dt.GetChanges();
                    if (tdt != null)
                    {
                        adpt.Update(tdt);
                        dt.AcceptChanges();
                        Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated Dataset");
                    }

                }
                catch (DBConcurrencyException dbcex)
                {
                    Debug.Assert(!Global.AllowAssertions, String.Format("Concurrency error occurs using sql '{0}' {1} {2}", adpt.SelectCommand.CommandText, Environment.NewLine, dbcex));


                    DataTable refresh = new DataTable(dt.TableName);
                    adpt.Fill(refresh);
                    if (dt.DataSet == null)
                    {
                        DataSet olddata = new DataSet();
                        olddata.Tables.Add(dt);
                        olddata.Merge(refresh, true, MissingSchemaAction.Ignore);
                        olddata.Tables.Remove(dt);
                    }
                    else
                        dt.DataSet.Merge(refresh, true, MissingSchemaAction.Ignore);
                    adpt.Update(dt);
                    dt.AcceptChanges();
                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated Dataset after concurrency issues.");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            finally
            {
                UnLockExecution();
            }
        }

        /// <summary>
        /// Updates all tables within the dataset by passing all the select statements to each 
        /// corresponding table.
        /// </summary>
        /// <param name="ds">Dataset object.</param>
        /// <param name="tableNames">A param array of tables names matching with the sql statement given.</param>
        /// <param name="selectStatements">Param array of select statements.</param>
        protected override void InternalUpdate(DataSet ds, string[] tableNames, string[] selectStatements)
        {
            try
            {
                LockExecution();
                SqlConnection cnn = null;
                SqlTransaction tran;



                cnn = (SqlConnection)_cnn;
                tran = cnn.BeginTransaction();


                try
                {
                    int ctr = 0;

                    foreach (string tbl in tableNames)
                    {
                        if (ds.Tables.Contains(tbl))
                        {
                            DataTable dt = ds.Tables[tbl];
                            SqlDataAdapter adpt = new SqlDataAdapter(selectStatements[ctr], cnn);
                            SqlCommandBuilder cmdb = null;

                            cmdb = new SqlCommandBuilder(adpt);

                            adpt.SelectCommand.Transaction = tran;
                            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataSet SELECT: " + selectStatements[ctr], Global.LogSwitch.DisplayName);
                            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataSet INSERT: " + cmdb.GetInsertCommand().CommandText, Global.LogSwitch.DisplayName);
                            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataSet UPDATE: " + cmdb.GetUpdateCommand().CommandText, Global.LogSwitch.DisplayName);
                            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataSet DELETE: " + cmdb.GetDeleteCommand().CommandText, Global.LogSwitch.DisplayName);

                            CheckForRowGuid(dt, "");
                            DataTable tdt = dt.GetChanges();
                            if (tdt != null)
                            {
                                try
                                {
                                    adpt.Update(tdt);
                                    dt.AcceptChanges();
                                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated Dataset");

                                }
                                catch (DBConcurrencyException dbcex)
                                {
                                    Debug.Assert(!Global.AllowAssertions, String.Format("Concurrency error occurs using sql '{0}' {1} {2}", adpt.SelectCommand.CommandText, Environment.NewLine, dbcex));


                                    DataTable refresh = new DataTable(dt.TableName);
                                    adpt.Fill(refresh);
                                    if (dt.DataSet == null)
                                    {
                                        DataSet olddata = new DataSet();
                                        olddata.Tables.Add(dt);
                                        olddata.Merge(refresh, true, MissingSchemaAction.Ignore);
                                        olddata.Tables.Remove(dt);
                                    }
                                    else
                                    {
                                        dt.DataSet.Merge(refresh, true, MissingSchemaAction.AddWithKey);
                                    }
                                    adpt.Update(dt);
                                    dt.AcceptChanges();
                                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated Dataset after concurrency issues.");

                                }
                            }

                        }
                        ctr++;
                    }
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            finally
            {
                UnLockExecution();
            }
        }

        /// <summary>
        /// Updates a data table.
        /// </summary>
        /// <param name="dt">Data table to update</param>
        /// <param name="selectStatement">Select statement.</param>
        protected override void InternalUpdate(DataTable dt, string selectStatement)
        {
            try
            {
                LockExecution();

                SqlDataAdapter adpt = new SqlDataAdapter(selectStatement, (SqlConnection)_cnn);

                try
                {
                    SqlCommandBuilder cmdb = null;
                    cmdb = new SqlCommandBuilder(adpt);

                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable SELECT: " + selectStatement, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable INSERT: " + cmdb.GetInsertCommand().CommandText, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable UPDATE: " + cmdb.GetUpdateCommand().CommandText, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable DELETE: " + cmdb.GetDeleteCommand().CommandText, Global.LogSwitch.DisplayName);
                    CheckForRowGuid(dt, "");
                    DataTable tdt = dt.GetChanges();
                    if (tdt != null)
                    {
                        adpt.Update(tdt);
                        dt.AcceptChanges();
                        Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated Dataset");
                    }

                }
                catch (DBConcurrencyException dbcex)
                {
                    Debug.Assert(!Global.AllowAssertions, String.Format("Concurrency error occurs using sql '{0}' {1}{1} {2}", adpt.SelectCommand.CommandText, Environment.NewLine, dbcex));
                   
                    try
                    {
                        if (!dt.PrimaryKey.Any())
                        {
                            adpt.FillSchema(dt, SchemaType.Mapped);
                        }
                        DataTable refresh = dt.Clone();
                        adpt.Fill(refresh);                                         
                        for (int i = 0; i < dt.Rows.Count; i++)        
                        {
                            var SoureRow = dt.Rows[i];
                            var DestRow = refresh.Rows.Find(GetPrimaryKeyValues(SoureRow));
                            var Datadiff = CreateDiffRow(SoureRow);
                            if (DestRow != null)
                            {
                                foreach (var item in Datadiff)
                                {
                                    DestRow[item.Key] = item.Value;
                                }
                            }
                        }                                                                     
                        adpt.Update(refresh);
                        dt.AcceptChanges();
                        Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated Dataset after concurrency issues.");

                    }
                    catch 
                    {
                        try
                        {
                            DataTable refresh = new DataTable(dt.TableName);                           
                            adpt.Fill(refresh);                          
                            DataSet olddata = new DataSet();
                            olddata.Tables.Add(dt);
                            olddata.Merge(refresh, true, MissingSchemaAction.Ignore);
                            olddata.Tables.Remove(dt);                          
                            adpt.Update(dt);

                        }

                        catch 
                        {
                            throw dbcex;
                        }
                    }
                }
            }
            finally
            {
                UnLockExecution();
            }
        }  


        private static object[] GetPrimaryKeyValues(DataRow row)
        {
            List<object>  PrimaryKeyVals = new List<object>();
            foreach (DataColumn col in row.Table.PrimaryKey) 
            {
                PrimaryKeyVals.Add(row[col.ColumnName]);
            }
            return PrimaryKeyVals.ToArray(); 
        }



        private static Dictionary<string, object> CreateDiffRow(DataRow row )
        {

            Dictionary<string, object> changedvals = new Dictionary<string, object>();        
       

                foreach (DataColumn col in row.Table.Columns ) 
                {
                    if (Convert.ToString(row[col.ColumnName, DataRowVersion.Original]) != Convert.ToString(row[col.ColumnName, DataRowVersion.Current]))
                    {
                        changedvals.Add(col.ColumnName, row[col.ColumnName, DataRowVersion.Current]);
                    }

                }
           
            return changedvals;
        }

        private static Dictionary<string, object> CreateDiff(DataTable dt)
        {
       
            Dictionary<string, object> changedvals = new Dictionary<string, object>(); 

            foreach (DataRow row in dt.GetChanges().Rows)
            {             
           
                foreach (DataColumn col in dt.Columns)                 
                {
                    if (Convert.ToString( row[col.ColumnName , DataRowVersion.Original])!= Convert.ToString(row[col.ColumnName , DataRowVersion.Current ]))
                    {
                         changedvals.Add(col.ColumnName, row[col.ColumnName, DataRowVersion.Current]);
                    }               
                                    
                }
               
            }
            return changedvals;
        }

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
        protected override DataTable InternalUpdate(DataRow row, string selectStatement, string whereStatement, string identityColumn, string tableName)
        {
            try
            {
                LockExecution();

            Retry:


                SqlDataAdapter adpt = new SqlDataAdapter(selectStatement + " " + whereStatement, (SqlConnection)_cnn);

                try
                {

                    SqlCommandBuilder cmdb = new SqlCommandBuilder(adpt);

                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update Row SELECT: " + selectStatement, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update Row INSERT: " + cmdb.GetInsertCommand().CommandText, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update Row UPDATE: " + cmdb.GetUpdateCommand().CommandText, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update Row DELETE: " + cmdb.GetDeleteCommand().CommandText, Global.LogSwitch.DisplayName);


                    int ret = 0;

                    string selectIdentity = "";

                    if (row.RowState == DataRowState.Added)
                    {

                        if ((identityColumn != "") && row.Table.Columns.Contains(identityColumn))
                            selectIdentity = selectStatement + " where " + identityColumn + " = @@IDENTITY";

                    }

                    CheckForRowGuid(row, "");
                    ret = adpt.Update(new DataRow[1] { row });
                    try
                    {
                        row.AcceptChanges();
                    }
                    catch { }
                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated Dataset");

                    if (ret > 0 && selectIdentity != "")
                    {
                        DataTable dt = (DataTable)ExecuteSQLTable(selectIdentity, tableName, new IDataParameter[0]);
                        return dt;
                    }
                    else
                        return null;

                }
                catch (DBConcurrencyException dbcex)
                {
                    Debug.Assert(!Global.AllowAssertions, String.Format("Concurrency error occurs using sql '{0}' {1} {2}", adpt.SelectCommand.CommandText, Environment.NewLine, dbcex));


                    DataTable refresh = new DataTable(row.Table.TableName);
                    adpt.Fill(refresh);

                    if (row.Table.DataSet == null)
                    {
                        DataSet olddata = new DataSet();
                        olddata.Tables.Add(row.Table);
                        olddata.Merge(refresh, true, MissingSchemaAction.Ignore);
                        olddata.Tables.Remove(row.Table);
                    }
                    else
                        row.Table.DataSet.Merge(refresh, true, MissingSchemaAction.Ignore);

                    adpt.Update(new DataRow[1] { row });
                    row.AcceptChanges();
                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated Dataset after concurrency issues.");
                    return null;
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    bool? ProcessCancelled;

                    Exception exc = ValidateException(ex, null, out ProcessCancelled);
                    if (ProcessCancelled == null)
                        throw exc;
                    else if (ProcessCancelled.Value)
                    {
                        OnShutdownRequest();
                        throw exc;
                    }
                    else
                        goto Retry;
                }
            }
            finally
            {
                UnLockExecution();
            }
        }


        #endregion

        #region New Style Update Methods

        /// <summary>
        /// Updates a singular row, this method should deal with getting back a new identity column
        /// value if the row was added.
        /// </summary>
        protected override void InternalUpdate(DataRow row, string tableName, bool refresh, params string[] fields)
        {
            try
            {
                LockExecution();

            Retry:


                try
                {
                    SQLCommandBuilder cmdb = new SQLCommandBuilder(this, tableName, refresh, fields);
                    CheckForRowGuid(row, "");
                    cmdb.DataAdapter.Update(new DataRow[1] { row });
                    try
                    {
                        row.AcceptChanges();
                    }
                    catch { }
                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated DataRow");

                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    bool? ProcessCancelled;

                    Exception exc = ValidateException(ex, null, out ProcessCancelled);
                    if (ProcessCancelled == null)
                        throw exc;
                    else if (ProcessCancelled.Value)
                    {
                        OnShutdownRequest();
                        throw exc;
                    }
                    else
                        goto Retry;
                }
            }
            finally
            {
                UnLockExecution();
            }
        }

        protected override void InternalUpdate(DataTable dt, string tableName, bool refresh, params string[] fields)
        {
            try
            {
                LockExecution();

            Retry:


                try
                {
                    SQLCommandBuilder cmdb = new SQLCommandBuilder(this, tableName, refresh, fields);
                    CheckForRowGuid(dt, "");
                    cmdb.DataAdapter.Update(dt);
                    try
                    {
                        dt.AcceptChanges();
                    }
                    catch { }
                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "Successfully Updated DataTable");

                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    bool? ProcessCancelled;

                    Exception exc = ValidateException(ex, null, out ProcessCancelled);
                    if (ProcessCancelled == null)
                        throw exc;
                    else if (ProcessCancelled.Value)
                    {
                        OnShutdownRequest();
                        throw exc;
                    }
                    else
                        goto Retry;
                }
            }
            finally
            {
                UnLockExecution();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parameter prefix of this type of provider.
        /// </summary>
        public override string ParameterPrefix
        {
            get
            {
                return "@";
            }
        }

        #endregion

        #region Schema Methods

        protected override void PopulateTables(DataTable data)
        {
            string sql = "select TABLE_SCHEMA as [SCHEMA], TABLE_NAME as [TABLE_NAME] from INFORMATION_SCHEMA.TABLES  where TABLE_TYPE = 'BASE TABLE' order by TABLE_NAME";
            DataTable dt = ExecuteSQLTable(sql, "TABLES", new IDataParameter[0]);
            
            foreach (DataRow row in dt.Rows)
            {
                data.ImportRow(row);
            }

        }


        protected override void PopulateViews(DataTable data)
        {
            string sql = "select TABLE_SCHEMA as [SCHEMA], TABLE_NAME as [TABLE_NAME] from INFORMATION_SCHEMA.TABLES  where TABLE_TYPE = 'VIEW' order by TABLE_NAME";
            DataTable dt = ExecuteSQLTable(sql, "VIEWS", new IDataParameter[0]);
 
            foreach (DataRow row in dt.Rows)
            {
                data.ImportRow(row);
            }
            
        }

        protected override void PopulateTableColumns(string objectName, DataTable data)
        {
            string sql = "select COLUMN_NAME, DATA_TYPE from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA + '.' + TABLE_NAME = @tblName or TABLE_NAME = @tblName";
            DataTable dt = ExecuteSQLTable(sql, "COLUMNS", new IDataParameter[1] { AddParameter("tblName", SqlDbType.NVarChar, 255, objectName) });
             
            foreach (DataRow row in dt.Rows)
            {
                data.ImportRow(row);
            }
            
        }

        protected override void PopulateProcedures(DataTable data)
        {
            string sql = @"select SCHEMA_NAME([schema_id]) as [SCHEMA],  name as [PROCEDURE_NAME] 
from sys.procedures where is_ms_shipped = 0
order by [PROCEDURE_NAME]";
            DataTable dt = ExecuteSQLTable(sql, "PROCS", new IDataParameter[0]);

            foreach (DataRow row in dt.Rows)
            {
                data.ImportRow(row);
            }

        }

 
        protected override void PopulateProcedureParameters(string procedureName, DataTable data)
        {
            string sql = 
                @"select P.name as PARAMETER_NAME, T.name as [DATA_TYPE],  schema_name(PR.[schema_id]) as [SCHEMA], PR.name as [PROCEDURE_NAME]
from sys.parameters  P
join (select xtype, name from sys.systypes where xusertype = xtype) T on T.xtype = P.system_type_id
join sys.procedures PR on P.[object_id] = PR.[object_id]
where PR.is_ms_shipped = 0";
            DataTable dt = ExecuteSQLTable(sql, "PARAMETERS", null);

            foreach (DataRow row in dt.Rows)
            {
                data.ImportRow(row);
            }
            
        }


        protected override void PopulatePrimaryKey(string tableName, DataTable data)
        {
            if (string.IsNullOrWhiteSpace(tableName))
                return;

            string sql = "select K.column_name from INFORMATION_SCHEMA.TABLE_CONSTRAINTS C, INFORMATION_SCHEMA.KEY_COLUMN_USAGE K where K.constraint_name = C.constraint_name and constraint_type = 'PRIMARY KEY' and (K.TABLE_SCHEMA + '.' + K.TABLE_NAME = @tblName or K.TABLE_NAME = @tblName)";
            DataTable dt = ExecuteSQLTable(sql, "PK_COLUMNS", new IDataParameter[1] { AddParameter("tblName", SqlDbType.NVarChar, 255, tableName) });
            
            foreach (DataRow row in dt.Rows)
            {
                data.ImportRow(row);
            }
        }


        #endregion

        #region Methods

        public override object Clone()
        {
            return new SQLConnection(_cnn.ConnectionString, _userName, _appRoleName, _appRolePassword);
        }

        /// <summary>
        /// Fetches a list of linked servers associated to the server.
        /// </summary>
        /// <returns>A data table of linked servers.</returns>
        public override DataTable GetLinkedServers()
        {
            DataTable dt = ExecuteProcedureTable("sp_linkedservers", "LINKED_SERVERS", new IDataParameter[0]);
            DataTable baseTable = base.GetLinkedServers();
            foreach (DataRow row in dt.Rows)
            {
                baseTable.ImportRow(row);
            }
            return baseTable;
        }

        private Exception ValidateException(SqlException ex, Exception custom, out bool? ProcessCancelled)
        {

            ProcessCancelled = null;
            switch (ex.Number)
            {
                case 18456:		//Invalid user login or mistyped password or database does not exist.
                    {
                        return new InvalidLoginException(_userName, ((SqlConnection)_cnn).Database, ex);
                    }
                case 4060:		//Missing database error.
                    {
                        return new MissingDatabaseException(((SqlConnection)_cnn).Database, _userName, ex);
                    }
                case 17:		//Server cannot be found or access denied.
                    {
                        return new InvalidServerException(((SqlConnection)_cnn).DataSource, ex);
                    }
                case 18452:		//NT user is null / not valid.
                    {
                        return new InvalidLoginException(_userName, ((SqlConnection)_cnn).Database, ex);
                    }
                case 229: // Does not have select permissions, on most likely the dbUser table.
                    {
                        return new DeniedPermissionsException(_userName, ex);
                    }
                case 11:
                case 53:
                case 121:
                case 1231: // "An error has occurred while establishing a connection to the server.  When connecting to SQL Server 2005, this failure may be caused by the fact that under the default settings SQL Server does not allow remote connections. (provider: Named Pipes Provider, error: 40 - Could not open a connection to SQL Server)"
                    {
                        ConnectionErrorEventArgs Args = new ConnectionErrorEventArgs(true, ex);
                        OnConnectionError(Args);
                        ProcessCancelled = Args.Cancel;
                        return ex;
                    }
                default:
                    {
                        Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
                        if (custom != null)
                            return custom;
                        else
                            return ex;
                    }
            }
        }

        #endregion



    }

}

