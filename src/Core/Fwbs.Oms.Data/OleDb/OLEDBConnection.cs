using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace FWBS.OMS.Data
{
    /// <summary>
    /// Connection class that creates a sql server specific connection.
    /// </summary>
    [Serializable()]
    public class OLEDBConnection : Connection
    {
        #region Constructors

        private OLEDBConnection() { }

        /// <summary>
        /// Creates a oledb based connection by giving it a connection string
        /// to use.
        /// </summary>
        /// <param name="connectionString">The sql specific connection string.</param>
        internal OLEDBConnection(string connectionString)
        {
            _cnn = new OleDbConnection(connectionString);
        }

        #endregion

        #region Connection Methods

        /// <summary>
        /// Connects to the a oledb connection object.
        /// </summary>
        /// <param name="keepOpen">Keeps the database open until a forced disconnect is used.</param>
        public override void Connect(bool keepOpen)
        {
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
                    OleDbConnection cnn = (OleDbConnection)_cnn;
                    cnn.InfoMessage -= new OleDbInfoMessageEventHandler(this.InfoMessage);
                    cnn.InfoMessage += new OleDbInfoMessageEventHandler(this.InfoMessage);
                    cnn.StateChange -= new StateChangeEventHandler(this.cnn_StateChanged);
                    cnn.StateChange += new StateChangeEventHandler(this.cnn_StateChanged);

                    cnn.Open();
                   

                    Trace.WriteLineIf(Global.LogSwitch.TraceVerbose, "OLEDB Database Connected: " + _cnn.ConnectionString, Global.LogSwitch.DisplayName);
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

 
        #endregion

        #region Data Retrieval Methods


        protected override DataTable InternalExecuteSQLTable(IDbCommand cmd, bool schemaOnly)
        {
            try
            {
                LockExecution();

                OleDbDataAdapter adpt = new OleDbDataAdapter();
                adpt.SelectCommand = (OleDbCommand)cmd;

                DataTable dt = new DataTable();
                if (schemaOnly)
                    adpt.FillSchema(dt, SchemaType.Source);
                else
                    adpt.Fill(dt);

                return dt;
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

                //Execute the Sql Satement.
                //Fill the data type.
                OleDbDataAdapter adpt = new OleDbDataAdapter();
                adpt.SelectCommand = (OleDbCommand)cmd;

                DataSet ds = new DataSet();
                if (schemaOnly)
                    adpt.FillSchema(ds, SchemaType.Source);
                else
                    adpt.Fill(ds);

                return ds;
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

                return command.ExecuteScalar();
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

                return command.ExecuteReader((KeepOpen == true ? System.Data.CommandBehavior.Default : System.Data.CommandBehavior.CloseConnection));
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

                return command.ExecuteNonQuery();
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
        /// <param name="sender">The OLEDB connection.</param>
        /// <param name="e">Infomrational event arguments.</param>
        private void InfoMessage(object sender, OleDbInfoMessageEventArgs e)
        {
            Trace.WriteLineIf(Global.LogSwitch.TraceWarning, "Info Message: " + e.Message, Global.LogSwitch.DisplayName);

            foreach (OleDbError err in e.Errors)
                if (err.Message != e.Message)
                    Trace.WriteLineIf(Global.LogSwitch.TraceError, "Info Message Error: " + err.Message + " - Native Error: " + err.NativeError.ToString() + " - SQL State: " + err.SQLState + " - Source: " + err.Source, Global.LogSwitch.DisplayName);

        }


        private void cnn_StateChanged(object sender, StateChangeEventArgs e)
        {
            OnStateChanged(e);
        }

        #endregion

        #region Parameter Methods

        protected override void InternalDeriveParameters(IDbCommand command)
        {
            OleDbCommandBuilder.DeriveParameters((OleDbCommand)command);
        }

        public override IDataParameter AddParameter(string paramName, SqlDbType dataType, ParameterDirection direction, int size, object val)
        {
            OleDbType dbType = OleDbType.Integer;

            dbType = SqlTypeToProviderType(dataType);

            if (paramName.Substring(0, 1) != "?") paramName = "?" + paramName;
            OleDbParameter par = new OleDbParameter(paramName, dbType, size);
            par.Direction = direction;
            par.Value = val;
            return par;
        }

        #endregion

        #region Methods

        public override object Clone()
        {
            OLEDBConnection cnn = (OLEDBConnection)this.MemberwiseClone();
            cnn._cnn = new OleDbConnection(this._cnn.ConnectionString);
            return cnn;
        }

        /// <summary>
        /// Converts the providers data type to the eqivalent sql data type.
        /// </summary>
        /// <param name="dataType">Provider datatype.</param>
        /// <returns>Converted SQL data type.</returns>
        private SqlDbType ProviderTypeToSqlType(object dataType)
        {
            SqlDbType dbType = SqlDbType.Int;
            OleDbType providerType = OleDbType.Integer;

            if (!(dataType is OleDbType))
                providerType = (OleDbType)Enum.ToObject(typeof(OleDbType), dataType);


            switch (providerType)
            {
                case OleDbType.BigInt: dbType = SqlDbType.BigInt; break;
                case OleDbType.Binary: dbType = SqlDbType.Binary; break;
                case OleDbType.Boolean: dbType = SqlDbType.Bit; break;
                case OleDbType.Char: dbType = SqlDbType.Char; break;
                case OleDbType.DBDate: dbType = SqlDbType.DateTime; break;
                case OleDbType.Decimal: dbType = SqlDbType.Decimal; break;
                case OleDbType.Double: dbType = SqlDbType.Float; break;
                case OleDbType.LongVarBinary: dbType = SqlDbType.Image; break;
                case OleDbType.Integer: dbType = SqlDbType.Int; break;
                case OleDbType.Currency: dbType = SqlDbType.Money; break;
                case OleDbType.WChar: dbType = SqlDbType.NChar; break;
                case OleDbType.LongVarWChar: dbType = SqlDbType.NText; break;
                case OleDbType.VarWChar: dbType = SqlDbType.NVarChar; break;
                case OleDbType.Single: dbType = SqlDbType.Real; break;
                case OleDbType.SmallInt: dbType = SqlDbType.SmallInt; break;
                case OleDbType.LongVarChar: dbType = SqlDbType.Text; break;
                case OleDbType.DBTimeStamp: dbType = SqlDbType.Timestamp; break;
                case OleDbType.TinyInt: dbType = SqlDbType.TinyInt; break;
                case OleDbType.Guid: dbType = SqlDbType.UniqueIdentifier; break;
                case OleDbType.VarBinary: dbType = SqlDbType.VarBinary; break;
                case OleDbType.VarChar: dbType = SqlDbType.VarChar; break;
                case OleDbType.Variant: dbType = SqlDbType.Variant; break;
                default:
                    dbType = SqlDbType.Int;
                    break;

            }

            return dbType;
        }

        /// <summary>
        /// Converts the SQL data type providers data type to the eqivalent provider data type.
        /// </summary>
        /// <param name="dataType">SQL data type.</param>
        /// <returns>Converted Provider datatype</returns>
        private OleDbType SqlTypeToProviderType(SqlDbType dataType)
        {
            OleDbType dbType = OleDbType.Integer;

            switch (dataType)
            {
                case SqlDbType.BigInt: dbType = OleDbType.BigInt; break;
                case SqlDbType.Binary: dbType = OleDbType.Binary; break;
                case SqlDbType.Bit: dbType = OleDbType.Boolean; break;
                case SqlDbType.Char: dbType = OleDbType.Char; break;
                case SqlDbType.DateTime: dbType = OleDbType.DBDate; break;
                case SqlDbType.Decimal: dbType = OleDbType.Decimal; break;
                case SqlDbType.Float: dbType = OleDbType.Double; break;
                case SqlDbType.Image: dbType = OleDbType.LongVarBinary; break;
                case SqlDbType.Int: dbType = OleDbType.Integer; break;
                case SqlDbType.Money: dbType = OleDbType.Currency; break;
                case SqlDbType.NChar: dbType = OleDbType.WChar; break;
                case SqlDbType.NText: dbType = OleDbType.LongVarWChar; break;
                case SqlDbType.NVarChar: dbType = OleDbType.VarWChar; break;
                case SqlDbType.Real: dbType = OleDbType.Single; break;
                case SqlDbType.SmallDateTime: dbType = OleDbType.DBDate; break;
                case SqlDbType.SmallInt: dbType = OleDbType.SmallInt; break;
                case SqlDbType.SmallMoney: dbType = OleDbType.Currency; break;
                case SqlDbType.Text: dbType = OleDbType.LongVarChar; break;
                case SqlDbType.Timestamp: dbType = OleDbType.DBTimeStamp; break;
                case SqlDbType.TinyInt: dbType = OleDbType.TinyInt; break;
                case SqlDbType.UniqueIdentifier: dbType = OleDbType.Guid; break;
                case SqlDbType.VarBinary: dbType = OleDbType.VarBinary; break;
                case SqlDbType.VarChar: dbType = OleDbType.VarChar; break;
                case SqlDbType.Variant: dbType = OleDbType.Variant; break;
                default:
                    dbType = OleDbType.Integer;
                    break;

            }

            return dbType;
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

                OleDbDataAdapter adpt = new OleDbDataAdapter();

                try
                {
                    if (insert != "" && insert != null)
                    {
                        OleDbCommand cmd = new OleDbCommand(insert, (OleDbConnection)_cnn);
                        if (insert.Trim().ToUpper().StartsWith("INSERT"))
                            cmd.CommandType = CommandType.Text;
                        else
                            cmd.CommandType = CommandType.StoredProcedure;

                        //Build the parameter list up if there are any.
                        base.ApplyParameters(cmd, parameters);
                        adpt.InsertCommand = cmd;
                        Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable INSERT: " + cmd.CommandText, Global.LogSwitch.DisplayName);
                    }

                    if (update != "" && update != null)
                    {
                        OleDbCommand cmd = new OleDbCommand(update, (OleDbConnection)_cnn);
                        if (update.Trim().ToUpper().StartsWith("UPDATE"))
                            cmd.CommandType = CommandType.Text;
                        else
                            cmd.CommandType = CommandType.StoredProcedure;

                        //Build the parameter list up if there are any.
                        base.ApplyParameters(cmd, parameters);
                        adpt.UpdateCommand = cmd;
                        Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable UPDATE: " + cmd.CommandText, Global.LogSwitch.DisplayName);
                    }

                    if (delete != "" && delete != null)
                    {
                        OleDbCommand cmd = new OleDbCommand(delete, (OleDbConnection)_cnn);
                        if (delete.Trim().ToUpper().StartsWith("DELETE"))
                            cmd.CommandType = CommandType.Text;
                        else
                            cmd.CommandType = CommandType.StoredProcedure;

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

                OleDbConnection cnn = null;
                OleDbTransaction tran;

                cnn = (OleDbConnection)_cnn;
                tran = cnn.BeginTransaction();


                try
                {
                    int ctr = 0;

                    foreach (string tbl in tableNames)
                    {
                        if (ds.Tables.Contains(tbl))
                        {
                            DataTable dt = ds.Tables[tbl];
                            OleDbDataAdapter adpt = new OleDbDataAdapter(selectStatements[ctr], cnn);
                            OleDbCommandBuilder cmdb = null;

                            cmdb = new OleDbCommandBuilder(adpt);

                            adpt.SelectCommand.Transaction = tran;
                            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataSet SELECT: " + selectStatements[ctr], Global.LogSwitch.DisplayName);
                            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataSet INSERT: " + cmdb.GetInsertCommand().CommandText, Global.LogSwitch.DisplayName);
                            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataSet UPDATE: " + cmdb.GetUpdateCommand().CommandText, Global.LogSwitch.DisplayName);
                            Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataSet DELETE: " + cmdb.GetDeleteCommand().CommandText, Global.LogSwitch.DisplayName);

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
                                        dt.DataSet.Merge(refresh, true, MissingSchemaAction.Ignore);
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
                
                OleDbDataAdapter adpt = new OleDbDataAdapter(selectStatement, (OleDbConnection)_cnn);

                try
                {
                    OleDbCommandBuilder cmdb = null;
                    cmdb = new OleDbCommandBuilder(adpt);

                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable SELECT: " + selectStatement, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable INSERT: " + cmdb.GetInsertCommand().CommandText, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable UPDATE: " + cmdb.GetUpdateCommand().CommandText, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable DELETE: " + cmdb.GetDeleteCommand().CommandText, Global.LogSwitch.DisplayName);

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
            }
            finally
            {
                UnLockExecution();
            }
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

                OleDbDataAdapter adpt = new OleDbDataAdapter(selectStatement + " " + whereStatement, (OleDbConnection)_cnn);

                try
                {

                    OleDbCommandBuilder cmdb = new OleDbCommandBuilder(adpt);

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
            }
            finally
            {
                UnLockExecution();
            }
        }

        #endregion

        #region New Update Methods

        /// <summary>
        /// Updates a singular row, this method should deal with getting back a new identity column
        /// value if the row was added.
        /// </summary>
        protected override void InternalUpdate(DataRow row, string tableName, bool refreshbool, params string[] fields)
        {
            CheckForRowGuid(row, "rowguid");
            string columns = "";

            foreach (DataColumn col in row.Table.Columns)
            {
                if (!string.IsNullOrEmpty(columns))
                    columns += " , ";
                columns += "[" + col.ColumnName + "]";
            }

            string selectStatement = string.Format("SELECT {0} FROM {1}", columns, tableName);

            DataTable dt = row.Table;
            try
            {
                LockExecution();

                OleDbDataAdapter adpt = new OleDbDataAdapter(selectStatement, (OleDbConnection)_cnn);

                try
                {
                    OleDbCommandBuilder cmdb = null;
                    cmdb = new OleDbCommandBuilder(adpt);

                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable SELECT: " + selectStatement, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable INSERT: " + cmdb.GetInsertCommand().CommandText, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable UPDATE: " + cmdb.GetUpdateCommand().CommandText, Global.LogSwitch.DisplayName);
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Update DataTable DELETE: " + cmdb.GetDeleteCommand().CommandText, Global.LogSwitch.DisplayName);

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
            }
            finally
            {
                UnLockExecution();
            }
        }

        /// <summary>
        /// Updates a singular row, this method should deal with getting back a new identity column
        /// value if the row was added.
        /// </summary>
        protected override void InternalUpdate(DataTable dt, string tableName, bool refresh, params string[] fields)
        {
            throw new Exception("Custom OLEDB Command Builder not yet supported.");
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
                return "?";
            }
        }

        #endregion

        #region Schema Methods

        protected override void PopulateTables(DataTable data)
        {
            OleDbConnection cnn = null;

            Connect();

            cnn = (OleDbConnection)_cnn;

            try
            {
                DataTable dt = cnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                foreach (DataRow row in dt.Rows)
                {
                    data.ImportRow(row);
                }
            }
            finally
            {
                Disconnect();
            }
        }


        protected override void PopulateViews(DataTable data)
        {
            OleDbConnection cnn = null;

            Connect();

            cnn = (OleDbConnection)_cnn;

            try
            {
                DataTable dt = cnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Views, new object[0]);
                
                foreach (DataRow row in dt.Rows)
                {
                    data.ImportRow(row);
                }

            }
            catch (ArgumentException ex)
            {
                Trace.WriteLineIf(Global.LogSwitch.TraceError, ex.Message, Global.LogSwitch.DisplayName);
                DataTable dt = cnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "VIEW" });

                foreach (DataRow row in dt.Rows)
                {
                    data.ImportRow(row);
                }

            }
            finally
            {
                Disconnect();
            }
        }


        protected override void PopulateTableColumns(string objectName, DataTable data)
        {
            OleDbConnection cnn = null;

            Connect();

            cnn = (OleDbConnection)_cnn;

            try
            {
                DataTable dt = cnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, new object[] { null, null, objectName });
                int ctr = 0;
                foreach (DataRow row in dt.Rows)
                {
                    data.ImportRow(row);
                    data.Rows[ctr]["DATA_TYPE"] = ProviderTypeToSqlType(row["DATA_TYPE"]).ToString();
                    ctr++;
                }
            }
            finally
            {
                Disconnect();
            }
        }
        protected override void PopulateProcedures(DataTable data)
        {
            OleDbConnection cnn = null;

            Connect();

            cnn = (OleDbConnection)_cnn;

            try
            {
                DataTable dt = cnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Procedures, new object[0]);
                foreach (DataRow row in dt.Rows)
                {
                    data.ImportRow(row);
                }
            }
            finally
            {
                Disconnect();
            }
        }

        protected override void PopulateProcedureParameters(string procedureName, DataTable data)
        {
            OleDbConnection cnn = null;

            Connect();

            cnn = (OleDbConnection)_cnn;

            try
            {
                DataTable dt = cnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Procedure_Parameters, new object[0]);

                int ctr = 0;
                foreach (DataRow row in dt.Rows)
                {
                    data.ImportRow(row);
                    data.Rows[ctr]["DATA_TYPE"] = ProviderTypeToSqlType(row["DATA_TYPE"]).ToString();
                    ctr++;
                }
            }
            finally
            {
                Disconnect();
            }
        }

        protected override void PopulatePrimaryKey(string tableName, DataTable data)
        {
            OleDbConnection cnn = null;

            Connect();

            cnn = (OleDbConnection)_cnn;

            try
            {
                DataTable dt = cnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Primary_Keys, new object[] { null, null, tableName });
  
                foreach (DataRow row in dt.Rows)
                {
                    data.ImportRow(row);
                }

            }
            finally
            {
                Disconnect();
            }
        }

        protected override void PopulateProcedureColumns(string procedureName, DataTable data)
        {
            OleDbConnection cnn = null;

            Connect();

            cnn = (OleDbConnection)_cnn;

            try
            {
                DataTable dt = cnn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Procedure_Columns, new object[0]);
         
                foreach (DataRow row in dt.Rows)
                {
                    data.ImportRow(row);
                }
                
            }
            finally
            {
                Disconnect();
            }
        }

        #endregion




    }
}
