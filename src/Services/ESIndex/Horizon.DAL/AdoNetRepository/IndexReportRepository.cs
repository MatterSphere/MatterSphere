using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.AdoNet;
using Horizon.Common.Interfaces;
using Horizon.Common.Models.Common;
using Horizon.Common.Models.Repositories.IndexReport;

namespace Horizon.DAL.AdoNetRepository
{
    public class IndexReportRepository : IIndexReportRepository
    {
        private readonly string _connection;

        public IndexReportRepository(string connection)
        {
            _connection = connection;
        }

        public IEnumerable<IReportItem> GetDocumentBuckets(ContentableEntityTypeEnum entityType)
        {
            var result = new List<IReportItem>();
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand($"[search].[{GetReportStoreProcedureName(entityType)}]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var dataSet = dt.AsEnumerable().ToList();
                    foreach (var row in dataSet)
                    {
                        long success;
                        Int64.TryParse(row["SuccessNumber"].ToString(), out success);
                        long failed;
                        Int64.TryParse(row["FailedNumber"].ToString(), out failed);
                        long emptyContent;
                        Int64.TryParse(row["EmptyContentNumber"].ToString(), out emptyContent);
                        var bucket = new DocumentBucket(
                            row["Type"].ToString(),
                            success,
                            failed,
                            emptyContent);
                        result.Add(bucket);
                    }
                }
            }

            return result;
        }

        public IEnumerable<DocumentErrorBucket> GetDocumentErrorBuckets(string extension, ContentableEntityTypeEnum entityType)
        {
            var result = new List<DocumentErrorBucket>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand($"[search].[{GetReportStoreProcedureName(entityType)}]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new StringParameter("documentExtension", extension)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var dataSet = dt.AsEnumerable().ToList();
                    foreach (var row in dataSet)
                    {
                        long number;
                        Int64.TryParse(row["Number"].ToString(), out number);
                        var bucket = new DocumentErrorBucket(
                            row["ErrorType"].ToString(), number);
                        result.Add(bucket);
                    }
                }
            }

            return result;
        }

        public IEnumerable<DocumentError> GetDocumentErrors(string extension, string errorCode, int page, int pageSize, ContentableEntityTypeEnum entityType)
        {
            var result = new List<DocumentError>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand($"[search].[{GetReportStoreProcedureName(entityType)}]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new StringParameter("documentExtension", extension, 15),
                        new StringParameter("errorCode", errorCode, 50),
                        new IntParameter("page", page),
                        new IntParameter("pageSize", pageSize)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var dataSet = dt.AsEnumerable().ToList();
                    foreach (var row in dataSet)
                    {
                        long id;
                        Int64.TryParse(row["Id"].ToString(), out id);

                        var bucket = new DocumentError(
                            id,
                            row["Name"].ToString(),
                            row["Path"].ToString(),
                            row["ErrorDetails"].ToString());
                        result.Add(bucket);
                    }
                }
            }

            return result;
        }

        public IEnumerable<EntityProcessItem> GetActualProcessDetail(int seconds)
        {
            var result = new List<EntityProcessItem>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                var query = "SELECT D.MessageType, D.StartDate, D.SuccessNumber, D.FailedNumber, D.Size, D.NumOfProcessedMessages, D.ESIndexProcessId" +
                            " FROM [search].[ESIndexProcessDetail] AS D" +
                            " INNER JOIN(SELECT TOP 1 Id, FinishDate FROM [search].[ESIndexProcess] ORDER BY Id DESC) AS P ON D.ESIndexProcessId = P.Id" +
                            $" WHERE DATEDIFF(ss, ISNULL(P.FinishDate, GETDATE()), GETDATE()) < {seconds}";
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                var dataSet = dt.AsEnumerable().ToList();
                foreach (var row in dataSet)
                {
                    DateTime startDate;
                    DateTime.TryParse(row["StartDate"].ToString(), out startDate);
                    int success;
                    Int32.TryParse(row["SuccessNumber"].ToString(), out success);
                    int failed;
                    Int32.TryParse(row["FailedNumber"].ToString(), out failed);
                    long size;
                    Int64.TryParse(row["Size"].ToString(), out size);
                    long messages;
                    Int64.TryParse(row["NumOfProcessedMessages"].ToString(), out messages);

                    var entity = new EntityProcessItem(
                        row["MessageType"].ToString(),
                        startDate,
                        success,
                        failed,
                        messages,
                        size);

                    result.Add(entity);
                }
            }

            return result;
        }

        public long GetQueueLength(string queue)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                var query = $"SELECT COUNT(1) FROM {queue} WITH(NOLOCK)";
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                var row = dt.AsEnumerable().FirstOrDefault();
                if (row != null)
                {
                    long count;
                    Int64.TryParse(row[0].ToString(), out count);
                    return count;
                }

                return 0;
            }
        }

        private string GetReportStoreProcedureName(ContentableEntityTypeEnum entityType)
        {
            switch (entityType)
            {
                case ContentableEntityTypeEnum.Document:
                    return "GetDocumentTypesReport";
                case ContentableEntityTypeEnum.Precedent:
                    return "GetPrecedentTypesReport";
                case ContentableEntityTypeEnum.Email:
                default:
                    return "GetDocumentTypesReport";
            }
        }
    }
}
