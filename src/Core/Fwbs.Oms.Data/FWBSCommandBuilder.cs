using System;
using System.Data;

namespace FWBS.OMS.Data
{

    /// <summary>
    /// FWBS version of a custom command builder.
    /// </summary>
    public abstract class FWBSCommandBuilder : IDisposable
	{
		#region Fields

		private System.Data.IDbDataAdapter _adp = null;
		private Connection _cnn = null;
		private string _table = "";
		private DataTable _schema = null;
		private bool _hasschema = false;
		private bool _refresh = true;

		#endregion

		#region Constructors

		private FWBSCommandBuilder()
		{
		}

		public FWBSCommandBuilder(Connection cnn, string tableName, bool refresh, params string[] fields)
		{
			_cnn = cnn;
		    _table = tableName;
		    _refresh = refresh;
			_adp = CreateDataAdapter(tableName, fields);			
			((System.Data.IDbDataAdapter)_adp).SelectCommand.Transaction = Cnn.CurrentTransaction;
		}

		

		#endregion

		#region Methods

		protected abstract IDbDataAdapter CreateDataAdapter(string tableName, string[] fields);

		protected virtual System.Data.IDbCommand GetDeleteCommand(DataRow row)
		{
			if (row.RowState == DataRowState.Deleted)
			{
				if (_hasschema == false) RefreshSchema();

				string sql = "DELETE FROM {0} WHERE {1}";
				string where = "";
				System.Data.IDbCommand cmd = Cnn.CreateCommand();
				cmd.Connection = Cnn.InternalConnection;

				int pars = 0;
				foreach (DataColumn col in row.Table.Columns)
				{
					if (_schema.Columns.Contains(col.ColumnName))
					{
						if (col.Unique || (Array.IndexOf(row.Table.PrimaryKey, col) > -1))
						{
							if (where != String.Empty)
								where += " and ";

							where = where + col.ColumnName + " = " + Cnn.BuildParameterName("P" + pars.ToString());
							IDataParameter p = Cnn.CreateParameter("P" + pars.ToString(), row[col, DataRowVersion.Original]);
							p.SourceColumn = col.ColumnName;
							cmd.Parameters.Add(p);
							pars++;
						}

					}
				}

				sql = String.Format(sql, _table, where);
				cmd.CommandText = sql;
				return cmd;
			}

			return null;

		}

		protected virtual System.Data.IDbCommand GetInsertCommand(DataRow row)
		{
			if (row.RowState == DataRowState.Added)
			{
				if (_hasschema == false) RefreshSchema();

				string sql = "INSERT INTO {0} ({1}) VALUES ({2});";
				if (_refresh) sql = sql + _adp.SelectCommand.CommandText + " WHERE {3}";
				string fields = "";
				string values = "";
				string where = "";

				System.Data.IDbCommand cmd = Cnn.CreateCommand();
				cmd.Connection = Cnn.InternalConnection;

				int pars = 0;
				//Parameters for main content.
				foreach (DataColumn col in row.Table.Columns)
				{
					if (_schema.Columns.Contains(col.ColumnName))
					{
						if (col.ReadOnly == false && col.AutoIncrement == false)
						{
							if (row[col, DataRowVersion.Current] != DBNull.Value)
							{
								if (fields != String.Empty)
									fields += ", ";

								if (values != String.Empty)
									values += ", ";

								fields = fields + col.ColumnName; 
								values = values + Cnn.BuildParameterName("P" + pars.ToString());
								IDataParameter p = Cnn.CreateParameter("P" + pars.ToString(), row[col, DataRowVersion.Current]);
								p.SourceColumn = col.ColumnName;
								cmd.Parameters.Add(p);
								pars++;
							}
						}
					}
						
				}


                if (row.Table.Columns.IndexOf(Global.RowGuidCol) > -1) 
                {
                    if (where != String.Empty)
                        where += " and ";
                    where += string.Format("{0} = '{1}'", Global.RowGuidCol, row[Global.RowGuidCol]);
                }
                else
                {
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        if (_schema.Columns.Contains(col.ColumnName))
                        {
                            if (col.Unique || (Array.IndexOf(row.Table.PrimaryKey, col) > -1))
                            {
                                if (where != String.Empty)
                                    where += " and "; 
                                where = where + col.ColumnName + " = " + Cnn.BuildParameterName("P" + pars.ToString());
                                IDataParameter p = Cnn.CreateParameter("P" + pars.ToString(), row[col, DataRowVersion.Current]);
                                p.SourceColumn = col.ColumnName;
                                cmd.Parameters.Add(p);
                                pars++;
                            }
                        }
                    }
                }
				
				sql = String.Format(sql, _table, fields, values, where);
				cmd.CommandText = sql;
				return cmd;
			}
			return null;

		}

		protected virtual System.Data.IDbCommand GetUpdateCommand(DataRow row)
		{
			if (row.RowState == DataRowState.Modified)
			{
				if (_hasschema == false) RefreshSchema();

				string sql = "UPDATE {0} SET {1} WHERE {2};";
				if (_refresh) sql = sql + _adp.SelectCommand.CommandText + " WHERE {3}";
				string fields = "";
				string where = "";

				System.Data.IDbCommand cmd = Cnn.CreateCommand();
				cmd.Connection = Cnn.InternalConnection;

				int pars = 0;

				//Parameters for the content.
				foreach (DataColumn col in row.Table.Columns)
				{

					if (_schema.Columns.Contains(col.ColumnName))
					{

						if (col.ReadOnly == false && col.AutoIncrement == false)
						{
							if (row[col, DataRowVersion.Original].Equals(row[col, DataRowVersion.Current]) == false)
							{
								if (fields != String.Empty)
									fields += ", ";
								fields = fields + col.ColumnName + " = " + Cnn.BuildParameterName("P" + pars.ToString());
								System.Data.IDataParameter p = Cnn.CreateParameter("P" + pars.ToString(), col.DataType, row[col, DataRowVersion.Current]);
								p.SourceColumn = col.ColumnName;
								cmd.Parameters.Add(p);
								pars++;
							}
						}

					}
				}

				//Parameters for the where clause.
				foreach (DataColumn col in row.Table.Columns)
				{
                    if (_schema.Columns.Contains(col.ColumnName))
					{
                        var scol = _schema.Columns[col.ColumnName];

                        if (row.Table.PrimaryKey.Length > 0)
                        {

                            if (col.Unique || (Array.IndexOf(row.Table.PrimaryKey, col) > -1))
                            {
                                if (where != String.Empty)
                                    where += " and ";
                                where = where + col.ColumnName + " = " + Cnn.BuildParameterName("P" + pars.ToString());
                                System.Data.IDataParameter p = Cnn.CreateParameter("P" + pars.ToString(), row[col, DataRowVersion.Original]);
                                p.SourceColumn = col.ColumnName;
                                cmd.Parameters.Insert(cmd.Parameters.Count, p);
                                pars++;

                            }                       

                        }

                        else
                        {
                            if (scol.Unique || (Array.IndexOf(_schema.PrimaryKey, scol) > -1))
                            {
                                if (where != String.Empty)
                                    where += " and ";
                                where = where + col.ColumnName + " = " + Cnn.BuildParameterName("P" + pars.ToString());
                                System.Data.IDataParameter p = Cnn.CreateParameter("P" + pars.ToString(), row[col, DataRowVersion.Original]);
                                p.SourceColumn = col.ColumnName;
                                cmd.Parameters.Insert(cmd.Parameters.Count, p);
                                pars++;
                            }
                        }
					}
				}
				

				//Parameters for the refreshed select statement.
				if (_refresh)
				{
					foreach (DataColumn col in row.Table.Columns)
					{
						if (_schema.Columns.Contains(col.ColumnName))
						{
				
							if (col.Unique || (Array.IndexOf(row.Table.PrimaryKey, col) > -1))
							{
								System.Data.IDataParameter p = Cnn.CreateParameter("P" + pars.ToString(), row[col, DataRowVersion.Original]);
								p.SourceColumn = col.ColumnName;
								cmd.Parameters.Add(p);
								pars++;
							}
						}
					}
				}
				//If there are no fields to update from the schema then return null;
				if (fields == String.Empty)
					return null;

				sql = String.Format(sql, _table, fields, where, where);
				cmd.CommandText = sql;
				return cmd;
			}

			return null;

		}

		public void RefreshSchema()
		{
			if (_adp != null)
			{
                ExecuteTableEventArgs e = new ExecuteTableEventArgs(Cnn, _adp.SelectCommand.CommandText,_adp.SelectCommand.CommandType, true, this._table, null, false, false);
                Cnn.OnBeforeExecuteTable(e);

                if (e.Data != null)
                {
                    _schema = e.Data;
                }
                else
                {
                    if (_schema == null)
                        _schema = new DataTable();
                    ((System.Data.Common.DbDataAdapter)_adp).FillSchema(_schema, SchemaType.Source);
                }
				
				_hasschema = true;

                var e2 = new ExecuteTableEventArgs(e.Connection, e.SQL, e.CommandType, e.SchemaOnly, e.TableName, e.Parameters, e.Data != null, e.Refresh);
                e2.AdditionalParameters.AddRange(e.AdditionalParameters);
                e2.Data = _schema;
                Cnn.OnAfterExecuteTable(e2);
			}
		}

        protected virtual string GetIdentity(string table)
        {
            return "@@IDENTITY";
        }

		#endregion

		#region Properties

		public Connection Cnn
		{
			get
			{
				return _cnn;
			}
		}

		public System.Data.Common.DbDataAdapter DataAdapter
		{
			get
			{
				return ((System.Data.Common.DbDataAdapter)_adp);
			}
		}

		#endregion

		#region Events

		protected void OnRowUpdating(object sender, System.Data.Common.RowUpdatingEventArgs e)
		{
			System.Data.IDbCommand cmd = null;
			switch (e.StatementType) 
			{
				case StatementType.Delete:
					cmd = GetDeleteCommand(e.Row);
					break;
				case StatementType.Insert:
					cmd = GetInsertCommand(e.Row);
					break;
				case StatementType.Update:
					cmd = GetUpdateCommand(e.Row);
					break;
				default:
					e.Errors = new Exception(String.Format("Feature not supported: {0}", e.StatementType.ToString()));
					break;
			}

			if (cmd == null)
			{
				e.Row.AcceptChanges();
				e.Status = UpdateStatus.SkipCurrentRow;
			}
			else
			{
				e.Command = cmd;
				e.Command.Transaction = Cnn.CurrentTransaction;
			}
		}

		protected void OnRowUpdated(object sender, System.Data.Common.RowUpdatedEventArgs e)
		{
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			_adp = null;
		}

		#endregion
	}

}