using System;
using System.Data;
using System.Data.SqlClient;


namespace FWBS.OMS.Data
{
	/// <summary>
	/// SQL version of a custom command builder.
	/// </summary>
	internal class SQLCommandBuilder : FWBSCommandBuilder, IDisposable
	{
		#region Constructors


		public SQLCommandBuilder(Connection cnn, string tableName, bool refresh, params string[] fields) : base(cnn, tableName, refresh, fields)
		{
		}

		#endregion

		#region FWBSCommandBuilder

		protected override IDbDataAdapter CreateDataAdapter(string tableName, string[] fields)
		{
		    string flds = string.Empty;

		    if (fields != null)
		    {
		        foreach (string f in fields)
		        {
		            if (!string.IsNullOrEmpty(flds))
		            {
		                flds += ", ";
		            }
		            flds += f;
		        }
            }

            if (string.IsNullOrEmpty(flds)) flds = "*";

		    string sql = string.Format("SELECT {0} FROM {1}", flds, tableName);
            
            var adapter = new SqlDataAdapter(sql, (SqlConnection)Cnn.InternalConnection);
            adapter.SelectCommand.Parameters.Clear();

            adapter.RowUpdating +=new SqlRowUpdatingEventHandler(adpt_RowUpdating);
			adapter.RowUpdated +=new SqlRowUpdatedEventHandler(adpt_RowUpdated);
			return adapter;
		}

        protected override string GetIdentity(string table)
        {
            return "SCOPE_IDENTITY()";
        }

		#endregion

		#region Event Methods

		private void adpt_RowUpdating(object sender, SqlRowUpdatingEventArgs e)
		{
			this.OnRowUpdating(sender, e);
		}

		private void adpt_RowUpdated(object sender, SqlRowUpdatedEventArgs e)
		{
			this.OnRowUpdated(sender, e);
		}

		#endregion
	}
}
