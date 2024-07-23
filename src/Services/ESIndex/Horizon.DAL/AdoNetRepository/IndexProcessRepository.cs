using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.AdoNet;
using Horizon.Common.Interfaces;
using Horizon.Common.Models.Repositories.Blacklist;
using Horizon.Common.Models.Repositories.IndexProcess;

namespace Horizon.DAL.AdoNetRepository
{
    public class IndexProcessRepository : IIndexProcessRepository
    {
        private readonly string _connection;

        public IndexProcessRepository(string connection)
        {
            _connection = connection;
        }

        public IEnumerable<BlacklistItem> GetBlacklist()
        {
            var result = new List<BlacklistItem>();

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

                        var bucket = new BlacklistItem(
                            row["Extension"].ToString(),
                            row["Contains"].ToString(),
                            row["EncodingType"].ToString(),
                            convertResult ? size : (long?)null);
                        result.Add(bucket);
                    }
                }
            }

            return result;
        }

        public void AddBlacklistItem(BlacklistItem item)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[AddExtensionToBlackList]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var extension = new StringParameter("Extension ", item.Extension, 15);
                    extension.AddSqlParameter(cmd.Parameters);

                    if (!string.IsNullOrWhiteSpace(item.Metadata))
                    {
                        var metadata = new StringParameter("Contains ", item.Metadata, 1000);
                        metadata.AddSqlParameter(cmd.Parameters);
                    }

                    if (!string.IsNullOrWhiteSpace(item.Encoding))
                    {
                        var encoding = new VarcharParameter("EncodingType ", item.Encoding, 30);
                        encoding.AddSqlParameter(cmd.Parameters);
                    }

                    if (item.MaxSize.HasValue)
                    {
                        var maxSize = new LongParameter("MaxSize ", item.MaxSize);
                        maxSize.AddSqlParameter(cmd.Parameters);
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveBlacklistGroup(string extension)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[DeleteExtensionFromBlackList]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var extensionParameter = new StringParameter("Extension ", extension, 15);
                    extensionParameter.AddSqlParameter(cmd.Parameters);

                    var parameter = new BoolParameter("FullExtension ", true);
                    parameter.AddSqlParameter(cmd.Parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void RemoveBlacklistItem(string extension, string metadata = null, string encoding = null)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[DeleteExtensionFromBlackList]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var extensionParameter = new StringParameter("Extension ", extension, 15);
                    extensionParameter.AddSqlParameter(cmd.Parameters);

                    if (!string.IsNullOrWhiteSpace(metadata))
                    {
                        var metadataParameter = new StringParameter("Contains ", metadata, 1000);
                        metadataParameter.AddSqlParameter(cmd.Parameters);
                    }

                    if (!string.IsNullOrWhiteSpace(encoding))
                    {
                        var encodingParameter = new VarcharParameter("EncodingType ", encoding, 30);
                        encodingParameter.AddSqlParameter(cmd.Parameters);
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<string> GetExtensionsForReindexing()
        {
            List<string> result;

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [search].[ExtensionToReindex]", conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                result = ds.Tables[0].AsEnumerable().ToList()
                    .Select(s => s[0].ToString()).ToList();
            }

            return result;
        }

        public void AddExtensionForReindexing(string extension)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[AddExtensionToReindex]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var extensionParameter = new StringParameter("ExtensionsList ", extension);
                    extensionParameter.AddSqlParameter(cmd.Parameters);

                    var delimiterParameter = new StringParameter("Delimiter ", ",", 1);
                    delimiterParameter.AddSqlParameter(cmd.Parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ReindexAllFailedDocuments()
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[AddFailedToReindex]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public IndexSettings GetIndexSettings()
        {
            IndexSettings settings;

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                conn.Open();
                var query = "SELECT TOP(1) ProcessOrder, BatchSize, DocumentDateLimit, PreviousDocumentDateLimit, SummaryFieldEnabled FROM [search].[ChangeVersionControl]";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                var dataSet = ds.Tables[0].AsEnumerable().ToList();
                if (!dataSet.Any())
                {
                    settings = new IndexSettings();
                }
                else
                {
                    var row = dataSet[0];
                    long batchSize;
                    long.TryParse(row["BatchSize"].ToString(), out batchSize);
                    bool processOrder;
                    bool.TryParse(row["ProcessOrder"].ToString(), out processOrder);
                    DateTime? documentDateLimit = row["DocumentDateLimit"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(row["DocumentDateLimit"]);
                    DateTime? previousDocumentDateLimit = row["PreviousDocumentDateLimit"] == DBNull.Value ? null : (DateTime?)Convert.ToDateTime(row["PreviousDocumentDateLimit"]);
                    bool summaryFieldEnabled;
                    bool.TryParse(row["SummaryFieldEnabled"].ToString(), out summaryFieldEnabled);

                    settings = new IndexSettings(batchSize, processOrder, documentDateLimit, previousDocumentDateLimit, summaryFieldEnabled);
                }
            }

            return settings;
        }

        public void SaveIndexSettings(IndexSettings settings)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[UpdateSettings]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var batchSizeParameter = new LongParameter("BatchSize", settings.BatchSize);
                    batchSizeParameter.AddSqlParameter(cmd.Parameters);

                    var processOrderParameter = new BoolParameter("ProcessOrder", settings.ProcessOrderFromOldItems);
                    processOrderParameter.AddSqlParameter(cmd.Parameters);

                    var documentDateLimitParameter = new DateParameter("DocumentDateLimit", settings.DocumentDateLimit);
                    documentDateLimitParameter.AddSqlParameter(cmd.Parameters);

                    var previousDocumentDateLimitParameter = new DateParameter("PreviousDocumentDateLimit", settings.PreviousDocumentDateLimit);
                    previousDocumentDateLimitParameter.AddSqlParameter(cmd.Parameters);

                    var summaryFieldEnabledParameter = new BoolParameter("SummaryFieldEnabled", settings.SummaryFieldEnabled);
                    summaryFieldEnabledParameter.AddSqlParameter(cmd.Parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
