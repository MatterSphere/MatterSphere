using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace MatterSphereEWS
{
    class Database : IDisposable
    {
        public Database()
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
                MSEWSDBString.DataSource = Config.GetConfigurationItem("MatterSphereServer");
                MSEWSDBString.InitialCatalog = Config.GetConfigurationItem("MatterSphereDatabase");
                if (Config.GetConfigurationItem("MatterSphereLoginType") == "AAD")
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

        internal void UpdateItemRow(AppointmentItem itemRow)
        {
            if (itemRow.MSEWSItemRowUpdated == false)
            {
                return;
            }
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            StringBuilder sqlCommandString = new StringBuilder();
            sqlCommandString.AppendLine("Update dbo.DBAppointments");
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
            sqlCommandString.Append("Where [appID] = @ItemID");
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

        internal MatterSphereSettings GetRegInfoSetting()
        {
            MatterSphereSettings msSettings = new MatterSphereSettings();
            DataTable dteTable = GetSettingsFromDB();
            msSettings.CompanyID = Convert.ToInt64(dteTable.Rows[0]["RegCompanyID"]);
            msSettings.Edition = Convert.ToString(dteTable.Rows[0]["RegEdition"]);
            msSettings.SerialNo = Convert.ToInt32(dteTable.Rows[0]["RegSerialNo"]);
            return msSettings;
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
        private DataTable GetItemsBranch(string spName, int BranchID)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@BranchID";
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = BranchID;
                sqlCommand.Parameters.Add(parameter);
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(itemRows);
                }
            }
            CloseDatabaseConnection();
            return itemRows;
        }

        internal string GetFeeEarnerEmailAddress(long feeEarnerID)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            string usrEmailAddress = null;
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "Select usrEmail from dbUser where usrID = @usrID";
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@usrID";
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = feeEarnerID;
                sqlCommand.Parameters.Add(parameter);
                usrEmailAddress = (String)sqlCommand.ExecuteScalar();
            }
            CloseDatabaseConnection();
            return usrEmailAddress;
        }
        private DataTable GetSettingsFromDB()
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "Select Top 1 * from DBRegInfo";
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(itemRows);
                }
            }
            CloseDatabaseConnection();
            return itemRows;
        }

        internal string GetUserTimeZone(int feeEarnerID)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            string usrTimeZone = null;
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "Select " + Config.GetConfigurationItem("TimeZoneColumnName") + " from " + Config.GetConfigurationItem("TimeZoneTableName")+ " where usrID = @usrID";
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@usrID";
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = feeEarnerID;
                sqlCommand.Parameters.Add(parameter);
                usrTimeZone = (String)sqlCommand.ExecuteScalar();
            }
            CloseDatabaseConnection();
            return usrTimeZone;
        }

        internal DataTable GetFeeEarnerList()
        {
            try
            {
                int branchCode = Convert.ToInt32(Config.GetConfigurationItem("BranchCodeToSync"));
                if (branchCode == 0)
                {
                    return GetItems("fdAppGetFeeEarnerList");
                }
                else
                {
                    return GetItemsBranch("fdAppGetFeeEarnerList_Branch", branchCode);
                }
            }
            catch
            {
                return GetItems("fdAppGetFeeEarnerList");
            }

        }

        internal List<AppItemShort> GetUpdatedAppointments(int feeUsrID, DateTime lastrun)
        {
            
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "fdAppGetUpdatedAppointmentsForFE";
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@LastUpdate";
                parameter.SqlDbType = SqlDbType.DateTime;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = lastrun;
                sqlCommand.Parameters.Add(parameter);
                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@feeusrid";
                parameter2.SqlDbType = SqlDbType.Int;
                parameter2.Direction = ParameterDirection.Input;
                parameter2.Value = feeUsrID;
                sqlCommand.Parameters.Add(parameter2);
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(itemRows);
                }
            }
            CloseDatabaseConnection();
            List<AppItemShort> dbList = new List<AppItemShort>();
            foreach(DataRow row in itemRows.Rows)
            {
                AppItemShort appItem = new AppItemShort();
                appItem.EWSID = row["appMAPIStoreID"].ToString();
                appItem.AppointmentID = Convert.ToInt64(row["appID"]);
                appItem.MSUpdated = Convert.ToDateTime(row["Updated"]);
                dbList.Add(appItem);
            }
            return dbList;
        }


        internal AppointmentItem GetAppointment(long appID)
        {
            DataTable Appointments = GetAppointmentDB("fdAppGetAppointment", appID);
            if (Appointments.Rows.Count != 1)
            {
                return null;
            }
            return new AppointmentItem(Appointments.Rows[0]);
        }

        private DataTable GetAppointmentDB(string spName, long appID)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@AppID";
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = appID;
                sqlCommand.Parameters.Add(parameter);
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(itemRows);
                }
            }
            CloseDatabaseConnection();
            return itemRows;
        }

        internal DataTable GetNewAppointments()
        {
            try
            {
                int branchCode = Convert.ToInt32(Config.GetConfigurationItem("BranchCodeToSync"));
                if (branchCode == 0)
                {
                    return GetAppointmentsDB("fdAppGetNewAppointments");
                }
                else
                {
                    return GetAppointmentsDBBranch("fdAppGetNewAppointments_BRANCH", branchCode);
                }
            }
            catch
            {
                return GetAppointmentsDB("fdAppGetNewAppointments");
            }
        }
        internal DataTable GetAllAppointments()
        {
            try
            {
                int branchCode = Convert.ToInt32(Config.GetConfigurationItem("BranchCodeToSync"));
                if (branchCode == 0)
                {
                    return GetAppointmentsDB("fdAppGetAllAppointments");
                }
                else
                {
                    return GetAppointmentsDBBranch("fdAppGetAllAppointments_BRANCH", branchCode);
                }
            }
            catch
            {
                return GetAppointmentsDB("fdAppGetAllAppointments");
            }
        }

        internal DataTable GetDeletedAppointments()
        {
            try
            {
                int branchCode = Convert.ToInt32(Config.GetConfigurationItem("BranchCodeToSync"));
                if (branchCode == 0)
                {
                    return GetAppointmentsDB("fdAppGetDeletedAppointments");
                }
                else
                {
                    return GetAppointmentsDBBranch("fdAppGetDeletedAppointments_BRANCH", branchCode);
                }
            }
            catch
            {
                return GetAppointmentsDB("fdAppGetDeletedAppointments");
            }
        }


        private DataTable GetAppointmentsDB(string spName)
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

        private DataTable GetAppointmentsDBBranch(string spName, int BranchID)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@BranchID";
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = BranchID;
                sqlCommand.Parameters.Add(parameter);
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(itemRows);
                }
            }
            CloseDatabaseConnection();
            return itemRows;
        }

        internal void UpdateLastRun()
        {
            UpdateLastRunDB();
        }

        private void UpdateLastRunDB()
        {
            ReOpenConnectionIfClosed();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSEWSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "fdAppUpdateLastRunDate";
                sqlCommand.ExecuteNonQuery();
            }
            CloseDatabaseConnection();
        }

        internal DateTime GetLastRun()
        {
            return GetLastRunDB();
        }

        private DateTime GetLastRunDB()
        {
            ReOpenConnectionIfClosed();

            DateTime lastrun = DateTime.UtcNow;
            try
            {
                using (SqlCommand sqlCommand = new SqlCommand())
                {
                    sqlCommand.Connection = MSEWSDBConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "fdAppGetLastRunDate";
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@lastdate";
                    parameter.SqlDbType = SqlDbType.DateTime;
                    parameter.Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add(parameter);
                    sqlCommand.ExecuteNonQuery();
                    lastrun = (DateTime)sqlCommand.Parameters["@lastdate"].Value;
                }
            }
            catch (Exception)
            {
                
            }
            finally
            {
                CloseDatabaseConnection();
            }
            return lastrun;
        }
    }
}
