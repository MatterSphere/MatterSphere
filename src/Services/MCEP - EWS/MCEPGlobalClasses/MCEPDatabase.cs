using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MCEPGlobalClasses
{
    class MCEPDatabase : IDisposable
    {
        public MCEPDatabase()
        {
            GenerateDatabaseConnection();
            ReOpenConnectionIfClosed();
        }
        private SqlConnection MCEPDatabaseConnection;

        #region IDisposable Members

        public void Dispose()
        {
            if (MCEPDatabaseConnection.State == System.Data.ConnectionState.Open)
            {
                MCEPDatabaseConnection.Close();
            }
            MCEPDatabaseConnection.Dispose();
        }

        #endregion

        private void ReOpenConnectionIfClosed()
        {
            if (MCEPDatabaseConnection.State == System.Data.ConnectionState.Closed)
            {
                MCEPDatabaseConnection.Open();
            }
        }

        private void GenerateDatabaseConnection()
        {
            if (MCEPDatabaseConnection == null)
            {
                SqlConnectionStringBuilder MCEPDatabaseString = new SqlConnectionStringBuilder();
                MCEPDatabaseString.DataSource = MCEPConfiguration.GetConfigurationItem("MCEPDatabaseServer");
                MCEPDatabaseString.InitialCatalog = MCEPConfiguration.GetConfigurationItem("MCEPDatabaseName");
                if (MCEPConfiguration.GetConfigurationItem("MCEPDatabaseLoginType") == "AAD")
                    MCEPDatabaseString.Authentication = SqlAuthenticationMethod.ActiveDirectoryIntegrated;
                else
                    MCEPDatabaseString.IntegratedSecurity = true;
                MCEPDatabaseConnection = new SqlConnection(MCEPDatabaseString.ToString());
            }
        }

        public DataTable UserToSyncFromDatabase()
        {
            ReOpenConnectionIfClosed();
            DataTable userRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MCEPDatabaseConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "MCEPGetUsers";
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(userRows);
                    }
            }
            CloseDatabaseConnection();
            return userRows;
        }

        private void CloseDatabaseConnection()
        {
            if (MCEPDatabaseConnection.State == System.Data.ConnectionState.Open)
            {
                MCEPDatabaseConnection.Close();
            }
        }


        internal void UpdateUserRow(MCEPUser userRow)
        {
            if (userRow.UserRowUpdated == false)
            {
                return;
            }
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            StringBuilder sqlCommandString = new StringBuilder();
            sqlCommandString.AppendLine("Update MCEP.[User]");
            sqlCommandString.AppendLine("Set");
            foreach (string item in userRow.updatedColumnList)
            {
                if (item == userRow.updatedColumnList.First())
                {
                    sqlCommandString.AppendLine(item + " = @" + item);
                }
                else
                {
                    sqlCommandString.AppendLine(", " + item + " = @" + item);
                }
                sqlParameters.Add(new SqlParameter(item,userRow.UserDataRow[item]));
            }
            sqlCommandString.Append("Where UserID = @UserID");
            sqlParameters.Add(new SqlParameter("UserID", userRow.UserID));
            ReOpenConnectionIfClosed();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MCEPDatabaseConnection;
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

        internal bool CreateQueueItem(MCEPProfilerItem mailItem)
        {
            bool returnvalue = false;
            ReOpenConnectionIfClosed();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MCEPDatabaseConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "AddQueueItem";
                sqlCommand.Parameters.Add(new SqlParameter("UserEmail", mailItem.UserEmail));
                sqlCommand.Parameters.Add(new SqlParameter("FolderID", mailItem.FolderID));
                sqlCommand.Parameters.Add(new SqlParameter("MessageID", mailItem.MessageID));
                sqlCommand.Parameters.Add(new SqlParameter("UserID", mailItem.UserID));
                sqlCommand.Parameters.Add(new SqlParameter("FileID", mailItem.FileID));
                sqlCommand.Parameters.Add(new SqlParameter("Created", mailItem.ItemCreated));
                sqlCommand.Parameters.Add(new SqlParameter("Processed", mailItem.Processed));
                sqlCommand.Parameters.Add(new SqlParameter("Updated", mailItem.ItemUpdated));
                sqlCommand.ExecuteNonQuery();
                returnvalue = true;
            }
            CloseDatabaseConnection();
            return returnvalue;
        }

        internal DataTable ItemsToSyncFromDatabase()
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MCEPDatabaseConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "MCEPGetItems";
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(itemRows);
                }
            }
            CloseDatabaseConnection();
            return itemRows;
        }

        internal void UpdateItemRow(MCEPMailItem itemRow)
        {
            if (itemRow.MailItemRowUpdated == false)
            {
                return;
            }
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            StringBuilder sqlCommandString = new StringBuilder();
            sqlCommandString.AppendLine("Update MCEP.[Queue]");
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
                sqlParameters.Add(new SqlParameter(item, itemRow.MailItemDataRow[item]));
            }
            sqlCommandString.Append("Where [ID] = @ItemID");
            sqlParameters.Add(new SqlParameter("ItemID", itemRow.ItemID));
            ReOpenConnectionIfClosed();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MCEPDatabaseConnection;
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

        internal bool AddUserRow(DataRow userRow, string rootFolerName)
        {
            bool returnvalue = false;
            ReOpenConnectionIfClosed();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MCEPDatabaseConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "AddUser";
                sqlCommand.Parameters.Add(new SqlParameter("UserID",userRow["UsrID"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("Email", userRow["UsrEmail"].ToString()));
                sqlCommand.Parameters.Add(new SqlParameter("RootFolderName", rootFolerName));
                sqlCommand.Parameters.Add(new SqlParameter("Created", DateTime.UtcNow));
                sqlCommand.Parameters.Add(new SqlParameter("Active", false));
                sqlCommand.ExecuteNonQuery();
                returnvalue = true;
            }
            CloseDatabaseConnection();
            return returnvalue;
        }

        internal DataTable UsersFromDatabaseADMIN()
        {
            ReOpenConnectionIfClosed();
            DataTable userRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MCEPDatabaseConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "MCEPGetUsersADMIN";
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(userRows);
                }
            }
            CloseDatabaseConnection();
            return userRows;
        }
    }
}
