using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DocumentArchivingClass
{
    class MSDatabase : IDisposable
    {
        private SqlConnection MSDBConnection;
        
        public MSDatabase()
        {
            GenerateDatabaseConnection();
            ReOpenConnectionIfClosed();
        }
        
        #region "IDisposable Members"

        public void Dispose()
        {
            if (MSDBConnection.State == System.Data.ConnectionState.Open)
            {
                MSDBConnection.Close();
            }
            MSDBConnection.Dispose();
        }

        #endregion

        #region "Database Connection"
        private void ReOpenConnectionIfClosed()
        {
            if (MSDBConnection.State == System.Data.ConnectionState.Closed)
            {
                MSDBConnection.Open();
            }
        }

        private void GenerateDatabaseConnection()
        {
            if (MSDBConnection == null)
            {
                SqlConnectionStringBuilder MSEWSDBString = new SqlConnectionStringBuilder();
                MSEWSDBString.DataSource = DocumentArchivingConfiguration.GetConfigurationItem("MatterSphereDatabaseServer");
                MSEWSDBString.InitialCatalog = DocumentArchivingConfiguration.GetConfigurationItem("MatterSphereDatabaseName");
                if (DocumentArchivingConfiguration.GetConfigurationItem("MatterSphereLoginType") == "AAD")
                    MSEWSDBString.Authentication = SqlAuthenticationMethod.ActiveDirectoryIntegrated;
                else
                    MSEWSDBString.IntegratedSecurity = true;
                MSDBConnection = new SqlConnection(MSEWSDBString.ToString());
            }
        }

        private void CloseDatabaseConnection()
        {
            if (MSDBConnection.State == System.Data.ConnectionState.Open)
            {
                MSDBConnection.Close();
            }
        }
        #endregion
        
        #region "Updating and Getting Items"
        internal void UpdateItemRow(DocumentRecord docRow)
        {
            if (docRow.DocumentRecordRowUpdated == false)
            {
                return;
            }
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            StringBuilder sqlCommandString = new StringBuilder();
            sqlCommandString.AppendLine("Update dbo.dbDocumentArchiveInfo");
            sqlCommandString.AppendLine("Set");
            foreach (string item in docRow.updatedColumnList)
            {
                if (item == docRow.updatedColumnList.First())
                {
                    sqlCommandString.AppendLine(item + " = @" + item);
                }
                else
                {
                    sqlCommandString.AppendLine(", " + item + " = @" + item);
                }
                sqlParameters.Add(new SqlParameter(item, docRow.DocumentRecordDataRow[item]));
            }
            sqlCommandString.Append("Where [ArchID] = @ArchID");
            sqlParameters.Add(new SqlParameter("ArchID", docRow.ArchiveID));
            ReOpenConnectionIfClosed();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSDBConnection;
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

        private DataTable GetItems(string spName)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSDBConnection;
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
        #endregion

        #region "Document Lists and Information"

        internal DataTable GetDocumentList()
        {
            return GetItems("sprDocArchiveGetList");
        }

        internal DataTable GetDocumentVersionList(Int64 docID)
        {            
            return GetDocumentInformationDatabase("sprDocumentArchiveDocVersionInfo", docID);
        }

        private DataTable GetDocumentInformationDatabase(string spName, Int64 DocID)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = spName;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@docid";
                parameter.SqlDbType = SqlDbType.BigInt;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = DocID;
                sqlCommand.Parameters.Add(parameter);
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(itemRows);
                }
            }
            CloseDatabaseConnection();
            return itemRows;
        }

        #endregion

        #region "Directory"
        internal string GetDirectoryLocation(Int16 DirectoryID)
        {
            return GetDirectoryLocationDB(DirectoryID);
        }

        internal bool UpdateDocumentDirectoryID(Int64 docID, Int16 DirectoryID)
        {
            return UpdateDocumentDirectoryIDDB(docID, DirectoryID);
        }
             
        private bool UpdateDocumentDirectoryIDDB(long docID, short DirectoryID)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            bool successful = false;
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSDBConnection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "Update DBDocument Set docdirID = @directoryID where docID = @docid";
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@directoryID";
                parameter.SqlDbType = SqlDbType.SmallInt;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = DirectoryID;
                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@docid";
                parameter2.SqlDbType = SqlDbType.BigInt;
                parameter2.Direction = ParameterDirection.Input;
                parameter2.Value = docID;
                sqlCommand.Parameters.Add(parameter);
                sqlCommand.Parameters.Add(parameter2);
                int rowcount = sqlCommand.ExecuteNonQuery();
            }
            CloseDatabaseConnection();
            return successful;
        }

        private string GetDirectoryLocationDB(Int32 DirectoryID)
        {
            ReOpenConnectionIfClosed();
            DataTable itemRows = new DataTable();
            string DirectoryPath = null;
            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSDBConnection;
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "Select DirPath from DBDirectory where dirID = @directoryID";
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@directoryID";
                parameter.SqlDbType = SqlDbType.Int;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = DirectoryID;
                sqlCommand.Parameters.Add(parameter);
                DirectoryPath = (String) sqlCommand.ExecuteScalar();
            }
            CloseDatabaseConnection();
            return DirectoryPath;
        }
        #endregion
        
        #region "Archive"
        internal void ArchiveDocumentDataInDatabase(long docID, string logMessage, int usrID)
        {
            ReOpenConnectionIfClosed();

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "sprArchiveDocumentData";

                sqlCommand.Parameters.Add(CreateSqlParameter("@docID", SqlDbType.BigInt, ParameterDirection.Input, docID));
                sqlCommand.Parameters.Add(CreateSqlParameter("@logMessage", SqlDbType.NVarChar, ParameterDirection.Input, logMessage));
                sqlCommand.Parameters.Add(CreateSqlParameter("@usrID", SqlDbType.Int, ParameterDirection.Input, usrID));

                sqlCommand.ExecuteScalar();
            }

            CloseDatabaseConnection();
        }
        #endregion

        #region "Delete"
        internal void DeleteDocumentDataInDatabase(long docID, string logMessage, int usrID)
        {
            ReOpenConnectionIfClosed();

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "sprDeleteDocumentData";
                                
                sqlCommand.Parameters.Add(CreateSqlParameter("@docID", SqlDbType.BigInt, ParameterDirection.Input, docID));
                sqlCommand.Parameters.Add(CreateSqlParameter("@logMessage", SqlDbType.NVarChar, ParameterDirection.Input, logMessage));
                sqlCommand.Parameters.Add(CreateSqlParameter("@usrID", SqlDbType.Int, ParameterDirection.Input, usrID));

                sqlCommand.ExecuteScalar();
            }

            CloseDatabaseConnection();
        }
        #endregion

        #region "Run History"
        internal Int64 CreateNewArchiveRunInDB(Guid runGuid)
        {
            ReOpenConnectionIfClosed();
            Int64 _runID;

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "sprInserttoDocumentArchiveRun";
                
                SqlParameter paramRunID = CreateSqlParameter("@runid", SqlDbType.BigInt, ParameterDirection.Output, null);
                sqlCommand.Parameters.Add(paramRunID);
                sqlCommand.Parameters.Add(CreateSqlParameter("@runStartTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow));
                sqlCommand.Parameters.Add(CreateSqlParameter("@runGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, runGuid));
           
                sqlCommand.ExecuteNonQuery();
                _runID = Convert.ToInt64(paramRunID.Value);  
            }

            CloseDatabaseConnection();
            return _runID;
        }
        
        internal void UpdateArchiveRunInDB(Guid runGuid, DateTime runEndTime, int runTotalDocs, int runProcessedDocs, int runProcessedFiles, bool runCompleted, string runMessage, string runProcessedArchiveInfoIDs)
        {
            ReOpenConnectionIfClosed();

            using (SqlCommand sqlCommand = new SqlCommand())
            {
                sqlCommand.Connection = MSDBConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandText = "sprAppendDocumentArchiveRun";

                sqlCommand.Parameters.Add(CreateSqlParameter("@runGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, runGuid));
                sqlCommand.Parameters.Add(CreateSqlParameter("@runEndTime", SqlDbType.DateTime, ParameterDirection.Input, runEndTime));
                sqlCommand.Parameters.Add(CreateSqlParameter("@runTotalDocs", SqlDbType.Int, ParameterDirection.Input, runTotalDocs));
                sqlCommand.Parameters.Add(CreateSqlParameter("@runProcessedDocs", SqlDbType.Int, ParameterDirection.Input, runProcessedDocs));
                sqlCommand.Parameters.Add(CreateSqlParameter("@runProcessedFiles", SqlDbType.Int, ParameterDirection.Input, runProcessedFiles));
                sqlCommand.Parameters.Add(CreateSqlParameter("@runCompleted", SqlDbType.Bit, ParameterDirection.Input, runCompleted));
                sqlCommand.Parameters.Add(CreateSqlParameter("@runMessage", SqlDbType.NVarChar, ParameterDirection.Input, runMessage));
                sqlCommand.Parameters.Add(CreateSqlParameter("@runProcessedArchiveInfoIDs", SqlDbType.NVarChar, ParameterDirection.Input, runProcessedArchiveInfoIDs));

                sqlCommand.ExecuteScalar();
            }

            CloseDatabaseConnection();
        }
        #endregion

        #region "Helper Methods/Functions"
        private static SqlParameter CreateSqlParameter(string paramName, SqlDbType DbType, ParameterDirection paramDirection, object paramValue)
        {
            SqlParameter _param = new SqlParameter();
            _param.ParameterName = paramName;
            _param.SqlDbType = DbType;
            _param.Direction = paramDirection;
            if (paramValue != null)
                _param.Value = paramValue;
            return _param;
        }
        #endregion

    }
}
