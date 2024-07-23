using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using FWBS.OMS.Data;


namespace FWBS.OMS.UI.Windows
{
    public interface IVersionDataSaveMethod
    {
        void Save(VersionDataSaveArgs versionDataSaveArgs);
    }


    public class VersionDataSaveArgs
    { 
        public DataSet DataSetToSave { get; set; }
        public string Destination { get; set; }
        public string Code { get; set; }
        public long CurrentVersion { get; set; }
        public string Comments { get; set; }
    }


    public class VersionDataSaver
    {
        IVersionDataSaveMethod saveMethod;


        public VersionDataSaver(IVersionDataSaveMethod saveMethod)
        {
            this.saveMethod = saveMethod;
        }

        
        public void SaveVersionData(VersionDataSaveArgs versionData)
        {
            ValidationResult validationResult = ValidateInputArguments(versionData);

            if (validationResult.Success)
            {
                saveMethod.Save(versionData);         
            }
            else
            {
                OuputErrorMessages(validationResult);
            }
        }


        private static void OuputErrorMessages(ValidationResult validationResult)
        {
            var errorMessages = new StringBuilder("Unable to save version data to the archives because:\n");

            foreach (var problem in validationResult.ProblemsFound)
            {
                errorMessages.AppendLine(problem);
            }

            throw new Exception("Unable to Save Version Data:\n" + errorMessages.ToString());
        }


        private ValidationResult ValidateInputArguments(VersionDataSaveArgs versionData)
        {
            var validationResult = new ValidationResult();

            if (string.IsNullOrWhiteSpace(versionData.Code))
            {
                validationResult.ProblemsFound.Add("No ID/Code supplied.");
                validationResult.Success = false;
            }

            if (versionData.DataSetToSave == null || versionData.DataSetToSave.Tables.Count < 1)
            {
                validationResult.ProblemsFound.Add("No DataTables found in supplied DataSet.");
                validationResult.Success = false;
            }

            if (string.IsNullOrWhiteSpace(versionData.Destination))
            {
                validationResult.ProblemsFound.Add("No destination value supplied.");
                validationResult.Success = false;
            }

            if (string.IsNullOrWhiteSpace(versionData.CurrentVersion.ToString()) || versionData.CurrentVersion == 0)
            {
                validationResult.ProblemsFound.Add("No version number supplied.");
                validationResult.Success = false;
            }

            return validationResult;
        }


        internal static bool VersionDataExistsInDB(string tableName, string code, long version, out DataTable results)
        {
            var sql = string.Format(@"select * from {0} where Code = @code and Version = @version", tableName);

            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = Session.CurrentSession.CurrentConnection.CreateParameter("code", code);
            paramlist[1] = Session.CurrentSession.CurrentConnection.CreateParameter("version", version);
            results = connection.ExecuteSQL(sql, tableName, false, paramlist);

            if (results.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }
    }


    public class VersionDataToXML : IVersionDataSaveMethod
    {
        IConnection connection;


        public VersionDataToXML (IConnection connection)
	    {
            this.connection = connection;
	    }


        public void Save(VersionDataSaveArgs versionData)
        {
            IDataParameter[] pars = new IDataParameter[4 + versionData.DataSetToSave.Tables.Count];
            var sqlToExecute = BuildInsertSQL(versionData, pars);
            connection.ExecuteSQL(sqlToExecute, pars);
        }


        private string BuildInsertSQL(VersionDataSaveArgs versionData, IDataParameter[] pars)
        {
            var sqlInsertHeader = new StringBuilder(string.Format(@"insert into {0} (Code, Version", versionData.Destination));
            var sqlInsertValues = new StringBuilder(@"values (@code, @version");

            PopulateCommonParameters(versionData, pars);

            var parameterIndex = 4;
            var tableNumber = 1;

            foreach (DataTable table in versionData.DataSetToSave.Tables)
            {
                var tableData = "tableData" + tableNumber.ToString();
                
                var tableDataAsXML = new StringWriter();
                table.WriteXml(tableDataAsXML, XmlWriteMode.WriteSchema);
                
                pars[parameterIndex] = Session.CurrentSession.CurrentConnection.CreateParameter(tableData, tableDataAsXML.ToString());

                sqlInsertHeader.Append(string.Format(", {0}", table.TableName));
                sqlInsertValues.Append(string.Format(", @{0}", tableData));

                tableNumber++;
                parameterIndex++;
            }

            sqlInsertHeader.Append(@", Created, CreatedBy, Comments) ");
            sqlInsertValues.Append(@", GetDate(), @createdBy, @comments)");

            var sqlToExecute = string.Concat(sqlInsertHeader.ToString(), Environment.NewLine, sqlInsertValues.ToString());
            return sqlToExecute;
        }


        private void PopulateCommonParameters(VersionDataSaveArgs versionData, IDataParameter[] pars)
        {
            pars[0] = Session.CurrentSession.CurrentConnection.CreateParameter("code", versionData.Code);
            pars[1] = Session.CurrentSession.CurrentConnection.CreateParameter("version", versionData.CurrentVersion);
            pars[2] = Session.CurrentSession.CurrentConnection.CreateParameter("createdBy", Session.CurrentSession.CurrentUser.ID);
            pars[3] = Session.CurrentSession.CurrentConnection.CreateParameter("comments", versionData.Comments);
        }
    }


    public class ValidationResult
    {
        private bool sucess;
        private List<string> problemsFound;


        public ValidationResult()
        {
            this.Success = true;
            problemsFound = new List<string>();
        }

        
        public bool Success
        {
            get { return sucess; }
            set { sucess = value; }
        }


        public List<string> ProblemsFound
        {
            get { return problemsFound; }
            set { problemsFound = value; }
        }
    }
}
