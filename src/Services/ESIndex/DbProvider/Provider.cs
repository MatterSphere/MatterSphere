using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.AdoNet;
using Models.DbModels;
using Models.Interfaces;

namespace DbProvider
{
    public class Provider : IDbProvider
    {
        private readonly string _connection;

        public Provider(string connection)
        {
            _connection = connection;
        }

        #region Index structure
        public string GetUserIndexName()
        {
            var info = GetIndicesInfo().FirstOrDefault(index => index.IndexType == IndexTypeEnum.User);
            if (info == null)
            {
                throw new Exception("The database does not have information about user index");
            }

            return info.Name;
        }

        public string GetDataIndexName()
        {
            var info = GetIndicesInfo().FirstOrDefault(index => index.IndexType == IndexTypeEnum.Data);
            if (info == null)
            {
                throw new Exception("The database does not have information about data index");
            }

            return info.Name;
        }

        public List<IndexField> GetIndexFields(IndexTypeEnum indexType)
        {
            var info = GetIndicesInfo().FirstOrDefault(index => index.IndexType == indexType);
            if (info == null)
            {
                return new List<IndexField>();
            }

            return GetIndexFields(info.Id);
        }

        public List<string> GetSuggestableFields()
        {
            var info = GetIndicesInfo().FirstOrDefault(index => index.IndexType == IndexTypeEnum.Data);
            if (info == null)
            {
                return new List<string>();
            }

            var fields = GetIndexFields(info.Id);

            return fields.Where(field => field.Suggestable).Select(field => field.FieldName).Distinct().ToList();
        }

        private List<IndexInfo> GetIndicesInfo()
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetIndexInfo]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    return dt.AsEnumerable()
                        .Select(s => new IndexInfo(
                            Int16.Parse(s["ESIndexId"].ToString()),
                            s["ESIndexName"].ToString(),
                            s["ESIndexType"].ToString())).ToList();
                }
            }
        }

        private List<IndexField> GetIndexFields(short indexId)
        {
            var result = new List<IndexField>();
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetIndexStuctureInfo]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var extensionParameter = new ShortParameter("ESIndexId", indexId);
                    extensionParameter.AddSqlParameter(cmd.Parameters);

                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        bool searchable;
                        Boolean.TryParse(row["searchable"].ToString(), out searchable);
                        bool facetable;
                        Boolean.TryParse(row["facetable"].ToString(), out facetable);
                        bool suggestable;
                        Boolean.TryParse(row["Suggestable"].ToString(), out suggestable);
                        bool isDefault;
                        Boolean.TryParse(row["IsDefault"].ToString(), out isDefault);
                        bool indexingEnabled;
                        Boolean.TryParse(row["IndexingEnabled"].ToString(), out indexingEnabled);

                        var field = new IndexField(
                            indexId,
                            row["FieldName"].ToString(),
                            row["ESFieldType"].ToString())
                        {
                            Searchable = searchable,
                            Facetable = facetable,
                            Suggestable = suggestable,
                            Analyzer = row["Analyzer"].ToString(),
                            IsDefault = isDefault,
                            IndexingEnabled = indexingEnabled
                        };

                        result.Add(field);
                    }
                }
            }

            return result;
        }
        #endregion

        #region Logs
        public void SetDocumentLogs(string entity, DocumentLog[] messageLogs)
        {
            using (var dataTable = new DataTable("Table"))
            {
                dataTable.Columns.Add(new DataColumn("EntityID", typeof(long)));
                dataTable.Columns.Add(new DataColumn("Sys_FileName", typeof(string)));
                var column = dataTable.Columns.Add("Sys_FileSize", typeof(long));
                column.AllowDBNull = true;
                dataTable.Columns.Add(new DataColumn("Sys_ProcessTime", typeof(double)));
                dataTable.Columns.Add(new DataColumn("Sys_DocIndexingError", typeof(string)));
                dataTable.Columns.Add(new DataColumn("Sys_ErrorCode", typeof(string)));
                dataTable.Columns.Add(new DataColumn("docExtension", typeof(string)));
                dataTable.Columns.Add(new DataColumn("EmptyContent", typeof(bool)));

                foreach (var resultItem in messageLogs)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["EntityID"] = resultItem.EntityId;
                    dataRow["Sys_FileName"] = resultItem.FileName;
                    dataRow["Sys_FileSize"] = resultItem.Size;
                    dataRow["Sys_ProcessTime"] = resultItem.Ticks;
                    dataRow["Sys_DocIndexingError"] = resultItem.ErrorMessage;
                    dataRow["Sys_ErrorCode"] = resultItem.ErrorCode;
                    dataRow["docExtension"] = resultItem.Extension;
                    dataRow["EmptyContent"] = !resultItem.HasContent;

                    dataTable.Rows.Add(dataRow);
                }

                using (SqlConnection conn = new SqlConnection(_connection))
                {
                    using (SqlCommand cmd = new SqlCommand("[search].[SaveIndexingInfo]", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ii", dataTable);
                        var procNameParameter = new StringParameter("ProcName", entity, 128);
                        procNameParameter.AddSqlParameter(cmd.Parameters);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public void StartIndexingProcess()
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESIndexProcessStart]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CompleteIndexingProcess()
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESIndexProcessFinish]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SetMessageLogs(MessageLog messageLog)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[AddESIndexProcessDetail]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var messageTypeParameter = new StringParameter("MessageType", messageLog.MessageType, 100);
                    messageTypeParameter.AddSqlParameter(cmd.Parameters);

                    var successNumberParameter = new IntParameter("SuccessNumber", messageLog.SuccessNumber);
                    successNumberParameter.AddSqlParameter(cmd.Parameters);

                    var failedNumberParameter = new IntParameter("FailedNumber", messageLog.FailedNumber);
                    failedNumberParameter.AddSqlParameter(cmd.Parameters);

                    var contentFailedNumberParameter = new IntParameter("ContentReadingFailedNumber", messageLog.ContentReadingFailedNumber);
                    contentFailedNumberParameter.AddSqlParameter(cmd.Parameters);

                    var sizeParameter = new LongParameter("Size", messageLog.Size);
                    sizeParameter.AddSqlParameter(cmd.Parameters);

                    var processTimeParameter = new FloatParameter("ProcessTime", messageLog.Ticks);
                    processTimeParameter.AddSqlParameter(cmd.Parameters);

                    var messagesParameter = new LongParameter("NumOfProcessedMessages", 1);
                    messagesParameter.AddSqlParameter(cmd.Parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Horizon

        public List<BlacklistCriterion> GetBlacklist()
        {
            var result = new List<BlacklistCriterion>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[GetBlackList]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        long size;
                        var convertResult = Int64.TryParse(row["MaxSize"].ToString(), out size);

                        var criterion = new BlacklistCriterion(
                            row["Extension"].ToString(),
                            convertResult ? size : 0,
                            row["EncodingType"].ToString(),
                            row["Contains"].ToString());
                            
                        result.Add(criterion);
                    }
                }
            }

            return result;
        }

        #endregion

        public Dictionary<string, string> GetSummaryTemplates()
        {
            Dictionary<string, string> templates = new Dictionary<string, string>();
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ObjectType, summaryTemplate FROM [search].[ESIndexTable]", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    var adapter = new SqlDataAdapter(cmd);

                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow dr in dt.Rows)
                    {
                        templates[dr["ObjectType"].ToString()] = dr["summaryTemplate"].ToString();
                    }
                }
            }
            return templates;
        }

        public bool GetSummarySetting()
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 SummaryFieldEnabled FROM [search].[ChangeVersionControl]", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    bool isSummaryFieldEnabled;

                    try
                    {
                        conn.Open();
                        isSummaryFieldEnabled = (bool)cmd.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException("Cannot get the summary setting information", ex);
                    }

                    return isSummaryFieldEnabled;
                }
            }
        }
    }
}
