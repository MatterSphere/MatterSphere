using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.Windows
{
    public interface IVersionDataRestorer
    {
        List<string> CreateSQLStatements();
    }

    public class VersionDataRestorationProcessor
    {
        private int same;
        private DataTable restoreversions;
        private DataTable currentversions;
        private List<string> sqlstatements;
        DataTable dtNoticeBoardData = null;
        DataTable dtNonRestoredData = null;
        DataTable dtNoticeBoardAuditData = null;

        public VersionDataRestorationProcessor(DataTable CurrentVersions, DataTable RestoreVersions)
        {
            restoreversions = RestoreVersions;
            currentversions = CurrentVersions;
            sqlstatements = new List<string>();
            dtNonRestoredData = CreateNonRestoredDataTable();
        }

        public event EventHandler <RestorationCompletedEventArgs> RestorationCompleted;
        
        public void ProcessVersionData()
        {
            try
            {
                foreach (DataRow r in restoreversions.Rows)
                {
                    if (!IsCurrentVersionSameAsRestore(r))
                    {
                        IVersionDataRestorer restore = ObjectRestorationFactory.CreateIVersionDataRestorer(r);
                        sqlstatements.AddRange(restore.CreateSQLStatements());
                    }
                }

                if (sqlstatements.Count > 0)
                {
                    ExecuteSQLStatements();
                    return;
                }
                if (same > 0)
                {
                    dtNoticeBoardData = AmalgamateNoticeBoardData(dtNonRestoredData, dtNoticeBoardAuditData);
                    OnRestorationCompleted(0, dtNoticeBoardData);
                    return;
                }
                FeedbackAboutNoRestoration();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }

        private bool IsCurrentVersionSameAsRestore(DataRow r)
        {
            DataRow[] rows = currentversions.Select("Code = '" + Convert.ToString(r["Code"]) + "' and CurrentVersionNumber = '" + Convert.ToString(r["VersionNumber"]) + "'" );
            if (rows != null && rows.Length > 0)
            {
                same++;
                AddRowToNonRestoreData(r);
                return true;
            }
            else
                return false;
        }

        private void FeedbackAboutNoRestoration()
        {
            string lookup = "";
            if(same == 1)
                lookup = "OBJRESTNOTREST";
            else
                lookup = "OBJRESTNOTREST2";

            MessageBox.Show(Convert.ToString(same) + " " + ResourceLookup.GetLookupText(lookup), ResourceLookup.GetLookupText("OVDSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void ExecuteSQLStatements()
        {
            try
            {
                DataTable dt = CreateVersionDataQueryTable();
                foreach (string strquery in sqlstatements)
                    dt.Rows.Add(strquery + "; ");

                IConnection con = Session.CurrentSession.CurrentConnection;
                List<IDataParameter> parList = new List<IDataParameter>();
                parList.Add(con.CreateParameter("versiondataquerytable", dt));

                IDataParameter returnparameter = con.CreateParameter("result", 0);
                returnparameter.Direction = ParameterDirection.InputOutput;
                parList.Add(returnparameter);

                con.ExecuteProcedure("sprObjectVersionDataRestoration", parList);

                int result = ConvertDef.ToInt32(returnparameter.Value, 0);
                if (result == 1)
                {
                    RestorationAuditCreator creator = new RestorationAuditCreator(restoreversions);
                    dtNoticeBoardAuditData = creator.CreateRestorationAuditRecords(currentversions);
                }

                dtNoticeBoardData = AmalgamateNoticeBoardData(dtNonRestoredData, dtNoticeBoardAuditData);

                OnRestorationCompleted(result, dtNoticeBoardData);
            }
            catch(Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }

        private void OnRestorationCompleted(int result, DataTable dtNoticeBoardData)
        {
            if (RestorationCompleted != null)
            {
                RestorationCompletedEventArgs args = new RestorationCompletedEventArgs();
                args.result = result;
                args.dtNoticeBoardData = dtNoticeBoardData;
                RestorationCompleted(this, args);
            }
        }

        private DataTable CreateVersionDataQueryTable()
        {
            DataTable table = new DataTable();
            table.Columns.Add("cmd", typeof(string));
            return table;
        }

        private DataTable CreateNonRestoredDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("ObjectType", typeof(string));
            dt.Columns.Add("RestoredDate", typeof(string));
            dt.Columns.Add("RestoredBy", typeof(long));
            dt.Columns.Add("RestoredFrom", typeof(string));
            dt.Columns.Add("RestoredTo", typeof(string));
            return dt;
        }

        private void AddRowToNonRestoreData(DataRow r)
        {
            DataRow newrow = dtNonRestoredData.NewRow();
            newrow["Code"] = Convert.ToString(r["Code"]);
            newrow["ObjectType"] = Convert.ToString(r["ObjectType"]);
            newrow["RestoredDate"] = System.DateTime.Now.ToLongDateString();
            newrow["RestoredBy"] = Session.CurrentSession.CurrentUser.ID;
            newrow["RestoredFrom"] = Convert.ToString(r["VersionNumber"]);
            newrow["RestoredTo"] = Convert.ToString(r["VersionNumber"]);
            dtNonRestoredData.Rows.Add(newrow);
        }

        private DataTable AmalgamateNoticeBoardData(DataTable dtNonRestoredData, DataTable dtNoticeBoardAuditData)
        {
            if (dtNoticeBoardAuditData == null || dtNoticeBoardAuditData.Rows.Count < 1)
            {
                if (dtNonRestoredData != null && dtNonRestoredData.Rows.Count > 0)
                {
                    return dtNonRestoredData;
                }
            }

            if (dtNonRestoredData == null || dtNonRestoredData.Rows.Count < 1)
            {
                if (dtNoticeBoardAuditData != null && dtNoticeBoardAuditData.Rows.Count > 0)
                {
                    return dtNoticeBoardAuditData;
                }
            }

            if ((dtNonRestoredData != null && dtNonRestoredData.Rows.Count > 0) && (dtNoticeBoardAuditData != null && dtNoticeBoardAuditData.Rows.Count > 0))
            {
                foreach (DataRow r in dtNonRestoredData.Rows)
                {
                    dtNoticeBoardAuditData.ImportRow(r);
                }
                return dtNoticeBoardAuditData;
            }
            return null;
        }

    }

    public static class ObjectRestorationFactory
    {
        public static IVersionDataRestorer CreateIVersionDataRestorer(DataRow r)
        {
            switch (r["ObjectType"].ToString())
            {
                case "Enquiry Form":
                    return new EnquiryFormDataRestorer(Convert.ToString(r["ObjectType"]), Convert.ToString(r["Code"]), Convert.ToInt32(r["VersionNumber"]));
                case "Data List":
                    return new DataListDataRestorer(Convert.ToString(r["ObjectType"]), Convert.ToString(r["Code"]), Convert.ToInt32(r["VersionNumber"]));
                case "Script":
                    return new ScriptDataRestorer(Convert.ToString(r["ObjectType"]), Convert.ToString(r["Code"]), Convert.ToInt32(r["VersionNumber"]));
                case "Search List":
                    return new SearchListDataRestorer(Convert.ToString(r["ObjectType"]), Convert.ToString(r["Code"]), Convert.ToInt32(r["VersionNumber"]));
                case "Precedent":
                    return new PrecedentRestorer(Convert.ToString(r["ObjectType"]), Convert.ToString(r["Code"]), Convert.ToInt32(r["VersionNumber"]));
                case "File Management":
                    return new FileManagementDataRestorer(Convert.ToString(r["ObjectType"]), Convert.ToString(r["Code"]), Convert.ToInt32(r["VersionNumber"]));
            }
            return null;
        }
    }

    public class EnquiryFormDataRestorer : IVersionDataRestorer
    {
        ObjectRestorationParams objparams;

        public EnquiryFormDataRestorer(string ObjectType, string Code, int Version)
        {
            objparams = new ObjectRestorationParams();
            objparams.Code = Code;
            objparams.ObjectType = ObjectType;
            objparams.Version = Convert.ToInt32(Version);
            objparams.ExlusionList = new string[] { "quID", "rowguid", "enqdesc", "enqWelcomeHeader", "enqWelcomeText", "enqScript1", "quAssembly", "qudesc", "qucontrol", "qusourcetype", "qusource", "qucall", "quparameters", "quhelp", "qucmddesc", "qucmdhelp", "qucmdmethod", "qucmdparameters", "qucmdtype", "pgeDesc", "pgeShortDesc" };
            objparams.XMLRetrievalQuery = "select dbEnquiry, dbEnquiryQuestion, dbEnquiryPage from dbEnquiryVersionData where Code = '{0}' and Version = '{1}'";
            objparams.VersionCheckQuery = "select * from dbEnquiry where EnqCode = '{0}'";
            objparams.VersionCheckColumn = "EnqVersion";
        }

        public List<string> CreateSQLStatements()
        {
            DataRestorerUtility utility = new DataRestorerUtility(objparams);
            return utility.InitiateDataProcessing();
        }
    }

    public class SearchListDataRestorer : IVersionDataRestorer
    {
        ObjectRestorationParams objparams;

        public SearchListDataRestorer(string ObjectType, string Code, int Version)
        {
            objparams = new ObjectRestorationParams();
            objparams.Code = Code;
            objparams.ObjectType = ObjectType;
            objparams.Version = Convert.ToInt32(Version);
            objparams.XMLRetrievalQuery = "select dbSearchListConfig from dbSearchListVersionData where Code = '{0}' and Version = '{1}'";
            objparams.VersionCheckQuery = "select * from dbSearchListConfig where schCode = '{0}'";
            objparams.VersionCheckColumn = "schVersion";
        }

        public List<string> CreateSQLStatements()
        {
            DataRestorerUtility utility = new DataRestorerUtility(objparams);
            return utility.InitiateDataProcessing();
        }
    }

    public class DataListDataRestorer : IVersionDataRestorer
    {
        ObjectRestorationParams objparams;

        public DataListDataRestorer(string ObjectType, string Code, int Version)
        {
            objparams = new ObjectRestorationParams();
            objparams.Code = Code;
            objparams.ObjectType = ObjectType;
            objparams.Version = Convert.ToInt32(Version);
            objparams.XMLRetrievalQuery = "select dbEnquiryDataList from dbDataListVersionData where Code = '{0}' and Version = '{1}'";
            objparams.VersionCheckQuery = "select * from dbEnquiryDataList where enqTable = '{0}'";
            objparams.VersionCheckColumn = "enqDLVersion";
        }

        public List<string> CreateSQLStatements()
        {
            DataRestorerUtility utility = new DataRestorerUtility(objparams);
            return utility.InitiateDataProcessing();
        }
    }

    public class ScriptDataRestorer : IVersionDataRestorer
    {
        ObjectRestorationParams objparams;

        public ScriptDataRestorer(string ObjectType, string Code, int Version)
        {
            objparams = new ObjectRestorationParams();
            objparams.Code = Code;
            objparams.ObjectType = ObjectType;
            objparams.Version = Convert.ToInt32(Version);
            objparams.XMLRetrievalQuery = "select dbScript from dbScriptVersionData where Code = '{0}' and Version = '{1}'";
            objparams.VersionCheckQuery = "select * from dbScript where scrCode = '{0}'";
            objparams.VersionCheckColumn = "scrVersion";
        }

        public List<string> CreateSQLStatements()
        {
            DataRestorerUtility utility = new DataRestorerUtility(objparams);
            return utility.InitiateDataProcessing();
        }
    }

    public class PrecedentRestorer : IVersionDataRestorer
    {
        ObjectRestorationParams objparams;

        public PrecedentRestorer(string ObjectType, string Code, int Version)
        {
            objparams = new ObjectRestorationParams();
            objparams.Code = Code;
            objparams.ObjectType = ObjectType;
            objparams.Version = Convert.ToInt32(Version);
            objparams.XMLRetrievalQuery = "select * from dbPrecdentVersion where Code = '{0}' and Version = '{1}'";
            objparams.VersionCheckQuery = "select * from dbPrecedents where precID = '{0}'";
            objparams.VersionCheckColumn = "";
        }

        public List<string> CreateSQLStatements()
        {
            DataRestorerUtility utility = new DataRestorerUtility(objparams);
            return utility.InitiateDataProcessing();
        }
    }

    public class FileManagementDataRestorer : IVersionDataRestorer
    {
        ObjectRestorationParams objparams;

        public FileManagementDataRestorer(string ObjectType, string Code, int Version)
        {
            objparams = new ObjectRestorationParams();
            objparams.Code = Code;
            objparams.ObjectType = ObjectType;
            objparams.Version = Convert.ToInt32(Version);
            objparams.XMLRetrievalQuery = "select dbFileManagementApplication from dbFileManagementVersionData where Code = '{0}' and Version = '{1}'";
            objparams.VersionCheckQuery = "select * from dbFileManagementApplication where appCode = '{0}'";
            objparams.VersionCheckColumn = "appVer";
        }

        public List<string> CreateSQLStatements()
        {
            DataRestorerUtility utility = new DataRestorerUtility(objparams);
            return utility.InitiateDataProcessing();
        }
    }


    internal class DataRestorerUtility
    {
        string strSQL = "";
        string columns = "";
        string values = "";
        string where = "";
        private string code;
        private int version;
        private int currentenqid;
        private string objecttype;
        private string  deletionquery;
        private string[] exclusionlist;
        private IConnection connection;
        private string versioncheckquery;
        private string xmlretrievalquery;
        private string versioncheckcolumn; 
        private List<string> sqlstatements;
        private List<IDataParameter> parList;


        internal DataRestorerUtility(ObjectRestorationParams objparams)
        {
            code = objparams.Code;
            version = objparams.Version;
            objecttype = objparams.ObjectType;
            exclusionlist = objparams.ExlusionList;
            versioncheckquery = objparams.VersionCheckQuery;
            xmlretrievalquery = objparams.XMLRetrievalQuery;
            versioncheckcolumn = objparams.VersionCheckColumn;

            sqlstatements = new List<string>();
            parList = new List<IDataParameter>();
            connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
        }

        internal List<String> InitiateDataProcessing()
        {
            DataTable dt = ExecuteDataGather(versioncheckquery);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToUInt32(dt.Rows[0][versioncheckcolumn]) != version)
                    CreateDataSet(RestorationQueryType.Update);
            }
            else
            {
                CreateDataSet(RestorationQueryType.Insert);
            }
            return sqlstatements;
        }

        internal void CreateDataSet(RestorationQueryType querytype)
        {
            try
            {
                DataTable dt = ExecuteDataGather(xmlretrievalquery);
                DataSet ds = new DataSet();
                StringReader strReader;

                for (int counter = 0; counter < dt.Columns.Count; counter++)
                {
                    strReader = new StringReader(Convert.ToString(dt.Rows[0][counter]));
                    ds.ReadXml(strReader);
                }

                if (objecttype != "Enquiry Form")
                {
                    if (querytype == RestorationQueryType.Insert)
                        BuildSQLInsertStatements(ds);
                    else
                        BuildSQLUpdateStatements(ds);
                }
                else
                    BuildEnquiryFormSQLStatements(ds, querytype);
            }
            catch (Exception ex) { ErrorBox.Show(ex); }
        }

        internal void BuildSQLInsertStatements(DataSet ds)
        {
            try
            {
                foreach (DataTable t in ds.Tables)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        foreach (DataColumn c in t.Columns)
                        {
                            if (!CheckExclusionList(c.ColumnName))
                            {
                                AddToColumnList(c);
                                AddToValueList(r, c);
                            }
                        }
                        strSQL = "insert into " + t.TableName + " (" + columns + ") values (" + values + ")";
                        sqlstatements.Add(strSQL);
                        ClearSQLQueryStrings();
                    }
                }
            }
            catch (Exception ex) { ErrorBox.Show(ex); }
        }

        internal void BuildSQLUpdateStatements(DataSet ds)
        {
            try
            {
                string primarykeys = "";

                foreach (DataTable t in ds.Tables)
                {
                    primarykeys = GetPrimaryKey(t.TableName);

                    foreach (DataRow r in t.Rows)
                    {
                        foreach (DataColumn c in t.Columns)
                        {
                            if (!CheckExclusionList(c.ColumnName))
                            {
                                if (objecttype != "Enquiry Form")
                                    ProcessColumnNonEnquiryForm(primarykeys, r, c);
                                else
                                    ProcessColumnEnquiryForm(r, c);
                            }
                        }
                        if (objecttype != "Enquiry Form")
                            strSQL = "update " + t.TableName + " set " + columns + where;
                        else
                            strSQL = "update " + t.TableName + " set " + columns + " where enqID = " + currentenqid;
                        sqlstatements.Add(strSQL);
                        ClearSQLQueryStrings();
                    }
                }
            }
            catch (Exception ex) { ErrorBox.Show(ex); }
        }

        internal void ProcessColumnNonEnquiryForm(string primarykeys, DataRow r, DataColumn c)
        {
            if (IsinPrimaryKey(primarykeys, c.ColumnName))
                BuildWhereClause(r, c);
            else
            {
                if (!string.IsNullOrWhiteSpace(columns))
                    columns += ", ";
                columns += c.ColumnName + " = " + GetColumnValue(r, c);
            }
        }

        internal void ProcessColumnEnquiryForm(DataRow r, DataColumn c)
        {
            if (!string.IsNullOrWhiteSpace(columns))
                columns += ", ";
            columns += c.ColumnName + " = " + GetColumnValue(r, c);
        }

        internal void BuildEnquiryFormSQLStatements(DataSet ds, RestorationQueryType querytype)
        {
            try
            {
                if (querytype == RestorationQueryType.Insert)
                    BuildSQLInsertStatements(ds);
                else
                {
                    currentenqid = GetEnquiryID();
                    sqlstatements.Add(deletionquery);
                    CreateEnquiryFormUpdates(ds);
                    CreateEnquiryFormInserts(ds);
                }
            }
            catch (Exception ex) { ErrorBox.Show(ex); }
        }

        private int GetEnquiryID()
        {
            DataTable dt = ReturnDataBaseData(string.Format("select enqID from dbEnquiry where enqCode = '{0}'", code), null);
            deletionquery = "delete from dbEnquiryQuestion where enqID = " + ConvertDef.ToInt32(dt.Rows[0]["enqID"],0) + "; delete from dbEnquiryPage where enqID = " + ConvertDef.ToInt32(dt.Rows[0]["enqID"],0);
            return ConvertDef.ToInt32(dt.Rows[0]["enqID"],0);
        }
        
        private void CreateEnquiryFormInserts(DataSet ds)
        {
            DataSet inserts = new DataSet();
            DataTable dt = ds.Tables[1].Copy();
            inserts.Tables.Add(dt);
            dt = ds.Tables[2].Copy();
            inserts.Tables.Add(dt);
            BuildSQLInsertStatements(inserts);
        }

        private void CreateEnquiryFormUpdates(DataSet ds)
        {
            DataSet updates = new DataSet();
            DataTable dt = ds.Tables[0].Copy();
            updates.Tables.Add(dt);
            BuildSQLUpdateStatements(updates);
        }

        private bool CheckExclusionList(string columnName)
        {
            bool result = false;
            if (exclusionlist != null && exclusionlist.Length != 0)
            {
                for (int i = 0; i < exclusionlist.Length; i++)
                {
                    if (exclusionlist[i] == columnName)
                        result = true;
                }
            }
            return result;
        }

        private bool IsinPrimaryKey(string keys, string columnname)
        {
            return Convert.ToBoolean((keys.IndexOf(columnname) != -1));
        }

        private void BuildWhereClause(DataRow r, DataColumn c)
        {
            if (string.IsNullOrWhiteSpace(where))
                where += " where " + c.ColumnName + " = " + GetColumnValue(r, c);   
            else
                where += " and " + c.ColumnName + " = " + GetColumnValue(r, c); 
        }

        private void AddToColumnList(DataColumn c)
        {
            if (string.IsNullOrWhiteSpace(columns))
                columns += c.ColumnName;
            else
                columns += ", " + c.ColumnName;
        }

        private void AddToValueList(DataRow r, DataColumn c)
        {
            if (!string.IsNullOrWhiteSpace(values))
                values += ", ";
            values += GetColumnValue(r, c);
        }

        private string GetColumnValue(DataRow r, DataColumn c)
        {
            if (c.DataType == typeof(string))
                return FormatStringValue(c, r[c].ToString());
            else if (Convert.IsDBNull(r[c]))
                return "NULL";
            else if (c.DataType == typeof(DateTime))
                return "'" + Convert.ToDateTime(r[c]).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            else if (c.DataType == typeof(Int32))
                return ConvertDef.ToInt32(r[c], 0).ToString();
            else if (c.DataType == typeof(Int64))
                return ConvertDef.ToInt64(r[c], 0).ToString();
            else if (c.DataType == typeof(byte))
                return ConvertDef.ToByte(r[c], 1).ToString();
            else if (c.DataType == typeof(Int16))
                return ConvertDef.ToInt16(r[c], 0).ToString();
            else if (c.DataType == typeof(bool))
                return ConvertDef.ToBoolean(r[c], false) ? "1" : "0";
            else if (c.DataType == typeof(byte[]))
                return "NULL";
            else
                return string.IsNullOrWhiteSpace(Convert.ToString(r[c])) ? "NULL" : "'" + Convert.ToString(r[c]) + "'";
        }
          
        private string FormatStringValue(DataColumn c, string value)
        {
            string result = "";
            if (!string.IsNullOrWhiteSpace(value))
                result = string.Format(@"'{0}'", value.Replace("'", "''"));
            else
            {
                if (c.ColumnName != "enqPath" && c.ColumnName != "quCode")
                    result = "NULL";
                else
                    result = string.Format(@"'{0}'", value);
            }
            return result;
        }

        private string GetPrimaryKey(string tablename)
        {
            switch(tablename)
            {
                case "dbEnquiry":
                    return "enqID";
                case "dbEnquiryQuestion":
                    return "quID";
                case "dbEnquiryPage":
                    return "enqID, pgeOrder";
                case "dbEnquiryDataList":
                    return "enqTable";
                case "dbScript":
                    return "scrCode";
                case "dbSearchListConfig":
                    return "schCode";
                case "dbFileManagementApplication":
                    return "appCode";
            }
            return "";
        }

        private void ClearSQLQueryStrings()
        {
            strSQL = "";
            columns = "";
            values = "";
            where = "";
        }

        internal DataTable ExecuteDataGather(string sql)
        {
            System.Data.DataTable dt = null;
            dt = ReturnDataBaseData(string.Format(sql, code, version), null);
            return dt;
        }

        private System.Data.DataTable ReturnDataBaseData(string sql, List<IDataParameter> parlist)
        {
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            connection.Disconnect();
            return dt;
        }
    }

    internal struct ObjectRestorationParams
    {
        internal string Code;
        internal int Version;
        internal string ObjectType; 
        internal string DeletionQuery; 
        internal string[] ExlusionList;
        internal string XMLRetrievalQuery;
        internal string VersionCheckQuery;
        internal string VersionCheckColumn;
    }

    internal enum RestorationQueryType
    {
        Insert,
        Update
    }

    public class RestorationCompletedEventArgs : EventArgs
    {
        public int result { get; set; }
        public DataTable dtNoticeBoardData { get; set; }
    }

    internal class RestorationAuditCreator
    {
        private DataTable Restoreddt;
        private IConnection connection;
        private List<IDataParameter> parList;

        internal RestorationAuditCreator(DataTable restores)
        {
            Restoreddt = restores.Copy();
            parList = new List<IDataParameter>();
            connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
        }

        internal DataTable CreateRestorationAuditRecords(DataTable currentversions)
        {
            DataTable dtAuditResults = CreateNoticeBoardDataTable();
            try
            {
                string restoredfromversion = "";
                foreach(DataRow r in Restoreddt.Rows)
                {
                    string code = Convert.ToString(r["Code"]);
                    string objecttype = Convert.ToString(r["ObjectType"]);
                    string restored = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss");
                    long restoredby = Session.CurrentSession.CurrentUser.ID;
                    string restoredtoversion = Convert.ToString(r["VersionNumber"]);
                    string type = Convert.ToString(r["ObjectType"]).Replace(" ", "");
                    DataRow[] rows = currentversions.Select("Code = '" + Convert.ToString(r["Code"]) + "' and Type = '" + type + "'");
                    if (rows != null && rows.Length > 0)
                    {
                        restoredfromversion = Convert.ToString(rows[0]["CurrentVersionNumber"]);
                    }
                    if(restoredfromversion != restoredtoversion)
                    {
                        CreateRestorationAuditRecord(code, objecttype, restored, restoredby, restoredfromversion, restoredtoversion);
                        DataRow AuditRow = dtAuditResults.NewRow();
                        dtAuditResults.Rows.Add(PopulateNewNoticeBoardDataRow(AuditRow, code, objecttype, restored, restoredby, restoredfromversion, restoredtoversion));
                        rows[0]["CurrentVersionNumber"] = Convert.ToString(restoredtoversion);
                    }
                }
                return dtAuditResults; 
            }
            catch (Exception ex) { ErrorBox.Show(ex); }
            return null; 
        }

        internal void CreateRestorationAuditRecord(string code, string objecttype, string restored, long restoredby, string restoredfromversion, string restoredtoversion)
        {
            string sql = "insert into dbVersionDataRestorationAudit (code, objecttype, restored, restoredby, restoredfromversion, restoredtoversion) values ('{0}','{1}','{2}',{3},'{4}','{5}')";
            ExecuteDataGather(sql, code, objecttype, restored, restoredby, restoredfromversion, restoredtoversion);
        }

        internal DataTable ExecuteDataGather(string sql, string code, string objecttype, string restored, long restoredby, string restoredfromversion, string restoredtoversion)
        {
            System.Data.DataTable dt = null;
            dt = ReturnDataBaseData(string.Format(sql, code, objecttype, restored, restoredby, restoredfromversion, restoredtoversion), null);
            return dt;
        }

        private System.Data.DataTable ReturnDataBaseData(string sql, List<IDataParameter> parlist)
        {
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            connection.Disconnect();
            return dt;
        }

        private DataTable CreateNoticeBoardDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("ObjectType", typeof(string));
            dt.Columns.Add("RestoredDate", typeof(string));
            dt.Columns.Add("RestoredBy", typeof(long));
            dt.Columns.Add("RestoredFrom", typeof(string));
            dt.Columns.Add("RestoredTo", typeof(string));
            return dt;
        }

        private DataRow PopulateNewNoticeBoardDataRow(DataRow AuditRow, string code, string objecttype, string restoreddate, long restoredby, string restoredfromversion, string restoredtoversion)
        {
            AuditRow["Code"] = code;
            AuditRow["ObjectType"] = objecttype;
            AuditRow["RestoredDate"] = restoreddate;
            AuditRow["RestoredBy"] = restoredby;
            AuditRow["RestoredFrom"] = restoredfromversion;
            AuditRow["RestoredTo"] = restoredtoversion;
            return AuditRow;
        }
    }
}
