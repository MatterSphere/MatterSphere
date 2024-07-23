using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.AdoNet;
using Horizon.Common.Interfaces;
using Horizon.Common.Models.Repositories.ProcessingStatus;

namespace Horizon.DAL.AdoNetRepository
{
    public class ProcessingStatusRepository : IProcessingStatusRepository
    {
        private readonly string _connection;

        public ProcessingStatusRepository(string connection)
        {
            _connection = connection;
        }

        public IEnumerable<ProcessHistoryItem> GetProcessHistory(DateTime dateFrom, DateTime dateTo)
        {
            var result = new List<ProcessHistoryItem>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetIndexProcessList]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new DateParameter("DateFrom", dateFrom),
                        new DateParameter("DateTo", dateTo)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var dataSet = dt.AsEnumerable().ToList();
                    foreach (var row in dataSet)
                    {
                        int id;
                        Int32.TryParse(row["Id"].ToString(), out id);
                        DateTime startDate;
                        DateTime.TryParse(row["StartDate"].ToString(), out startDate);
                        DateTime finishDate;
                        var convertResult = DateTime.TryParse(row["FinishDate"].ToString(), out finishDate);
                        int successful;
                        Int32.TryParse(row["SuccessNumber"].ToString(), out successful);
                        int failed;
                        Int32.TryParse(row["FailedNumber"].ToString(), out failed);
                        int contentErrors;
                        Int32.TryParse(row["ContentReadingFailedNumber"].ToString(), out contentErrors);

                        var entity = new ProcessHistoryItem(id, startDate)
                        {
                            FinishDate = convertResult ? finishDate : (DateTime?)null,
                            Successful = successful,
                            Failed = failed,
                            ContentErrors = contentErrors
                        };

                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public IEnumerable<ProcessHistoryItemDetail> GetProcessHistoryDetail(long processId)
        {
            var result = new List<ProcessHistoryItemDetail>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetIndexProcessDetail]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new LongParameter("ProcessId", processId)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var dataSet = dt.AsEnumerable().ToList();
                    foreach (var row in dataSet)
                    {
                        DateTime startDate;
                        DateTime.TryParse(row["StartDate"].ToString(), out startDate);
                        int successful;
                        Int32.TryParse(row["SuccessNumber"].ToString(), out successful);
                        int failed;
                        Int32.TryParse(row["FailedNumber"].ToString(), out failed);
                        int contentErrors;
                        Int32.TryParse(row["ContentReadingFailedNumber"].ToString(), out contentErrors);
                        long size;
                        Int64.TryParse(row["Size"].ToString(), out size);
                        DateTime finishDate;
                        var convertResult = DateTime.TryParse(row["FinishDate"].ToString(), out finishDate);

                        var entity = new ProcessHistoryItemDetail(row["MessageType"].ToString(), startDate)
                        {
                            FinishDate = convertResult ? finishDate : (DateTime?)null,
                            Successful = successful,
                            Failed = failed,
                            ContentErrors = contentErrors,
                            Size = size
                        };

                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public IEnumerable<ErrorCodeItem> GetErrorCodes(int processId)
        {
            var result = new List<ErrorCodeItem>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetDocProcessDetailBYProcessID]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new IntParameter("ProcessId", processId)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var dataSet = dt.AsEnumerable().ToList();
                    foreach (var row in dataSet)
                    {
                        int count;
                        Int32.TryParse(row["Count"].ToString(), out count);
                        var entity = new ErrorCodeItem(row["Sys_ErrorCode"].ToString(), count);
                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public IEnumerable<DocumentTypeItem> GetDocumentTypes(int processId, string errorCode)
        {
            var result = new List<DocumentTypeItem>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetDocProcessDetailBYExtention]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new IntParameter("ProcessId", processId),
                        new NVarcharParameter("Sys_ErrorCode", errorCode)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var dataSet = dt.AsEnumerable().ToList();
                    foreach (var row in dataSet)
                    {
                        int count;
                        Int32.TryParse(row["Count"].ToString(), out count);
                        var entity = new DocumentTypeItem(row["docExtension"].ToString(), count);
                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public IEnumerable<DocumentItem> GetDocuments(int processId, string errorCode, string extension, int page, int pageSize)
        {
            var result = new List<DocumentItem>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetDocProcessDetail]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new IntParameter("ProcessId", processId),
                        new NVarcharParameter("Sys_ErrorCode", errorCode),
                        new NVarcharParameter("docExtension", extension),
                        new IntParameter("PageNo", page),
                        new IntParameter("MAX_RECORDS", pageSize)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var dataSet = dt.AsEnumerable().ToList();
                    foreach (var row in dataSet)
                    {
                        int id;
                        Int32.TryParse(row["docID"].ToString(), out id);
                        long size;
                        Int64.TryParse(row["Sys_FileSize"].ToString(), out size);
                        var entity = new DocumentItem(
                            id,
                            row["Name"].ToString(),
                            row["Sys_FileName"].ToString(),
                            size,
                            row["Sys_DocIndexingError"].ToString());
                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public IEnumerable<DocumentErrorInfo> GetDocumentErrorsReport(int processId)
        {
            var result = new List<DocumentErrorInfo>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetDocProcessReport]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new IntParameter("ProcessId", processId)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    var dataSet = dt.AsEnumerable().ToList();
                    foreach (var row in dataSet)
                    {
                        int id;
                        Int32.TryParse(row["docID"].ToString(), out id);
                        long size;
                        Int64.TryParse(row["Sys_FileSize"].ToString(), out size);
                        var entity = new DocumentErrorInfo(
                            id,
                            row["Name"].ToString(),
                            row["docExtension"].ToString(),
                            size,
                            row["Sys_ErrorCode"].ToString(),
                            row["Sys_DocIndexingError"].ToString(),
                            row["Sys_FileName"].ToString());
                        result.Add(entity);
                    }
                }
            }

            return result;
        }
    }
}
