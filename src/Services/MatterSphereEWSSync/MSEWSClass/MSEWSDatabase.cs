using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MSEWSClass
{
    class MSEWSDatabase : IDisposable
    {
        public MSEWSDatabase()
        {
            GenerateDatabaseConnection();
            ReOpenConnectionIfClosed();
        }
        private SqlConnection MSEWSDBConnection;
        #region IDisposable Members

        public void Dispose()
        {
            if (MSEWSDBConnection.State == System.Data.ConnectionState.Open)
            {
                MSEWSDBConnection.Close();
            }
            MSEWSDBConnection.Dispose();
        }

        #endregion

        private void ReOpenConnectionIfClosed()
        {
            if (MSEWSDBConnection.State == System.Data.ConnectionState.Closed)
            {
                MSEWSDBConnection.Open();
            }
        }

        private void GenerateDatabaseConnection()
        {
            if (MSEWSDBConnection == null)
            {
                SqlConnectionStringBuilder MSEWSDBString = new SqlConnectionStringBuilder();
                MSEWSDBString.DataSource = MSEWSConfiguration.GetConfigurationItem("MSEWSDatabaseServer");
                MSEWSDBString.InitialCatalog = MSEWSConfiguration.GetConfigurationItem("MSEWSDatabaseName");
                if (MSEWSConfiguration.GetConfigurationItem("MSEWSDatabaseLoginType") == "AAD")
                    MSEWSDBString.Authentication = SqlAuthenticationMethod.ActiveDirectoryIntegrated;
                else
                    MSEWSDBString.IntegratedSecurity = true;
                MSEWSDBConnection = new SqlConnection(MSEWSDBString.ToString());
            }
        }

        private void CloseDatabaseConnection()
        {
            if (MSEWSDBConnection.State == System.Data.ConnectionState.Open)
            {
                MSEWSDBConnection.Close();
            }
        }

        internal void UpdateItemRow(MSEWSItem itemRow)
        {
            if (itemRow.MSEWSItemRowUpdated == false)
            {
                return;
            }
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            StringBuilder sqlCommandString = new StringBuilder();
            sqlCommandString.AppendLine("Update dbo.ESItemMaster");
            sqlCommandString.AppendLine("Set");
            foreach (string item in itemRow.updatedColumnList)
            {
                if (item == itemRow.updatedColumnList.First())
                {
                    sqlCommandString.AppendLine(item + " = @" + item);
                }
                else
                {
                    sqlCommandString.AppendLine(", " + item + " = @" + item);
                }
                sqlParameters.Add(new SqlParameter(item, itemRow.MSEWSItemDataRow[item]));
            }
            sqlCommandString.Append("Where [ItemID] = @ItemID");
            sqlParameters.Add(new SqlParameter("ItemID", itemRow.ItemID));
            ReOpenConnectionIfClosed();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sqlCommandString.ToString();
                foreach (SqlParameter sqlPara in sqlParameters)
                {
                    sqlCommand.Parameters.Add(sqlPara);
                }
                sqlCommand.ExecuteNonQuery();
                CloseDatabaseConnection();
            }
        }



        internal DataTable GetItemsToDelete()
        {
            return GetItems("ESGetItemsDelete");
        }

        internal DataTable GetItemsToUpdate()
        {
            return GetItems("ESGetItemsUpdate");
        }
        internal DataTable GetItemsToAdd()
        {
            return GetItems("ESGetItemsNew");
        }

        private DataTable GetItems(string spName)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = spName;
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(itemRows);
                }
            }
            CloseDatabaseConnection();
            return itemRows;
        }
    }
}
