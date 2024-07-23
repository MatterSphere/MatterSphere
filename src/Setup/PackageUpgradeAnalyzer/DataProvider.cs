using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PackageUpgradeAnalyzer
{
    class DataProvider : IDisposable
    {
        private const string TableColumnQuery = "SELECT CASE WHEN EXISTS (SELECT * FROM sys.columns c WHERE c.object_id = object_id(@tableName,'U') AND c.name = @columnName) THEN 1 ELSE 0 END";

        private readonly Dictionary<string, string> _queries = new Dictionary<string, string>
        {
            { "EnquiryForms",   "SELECT enqCode AS Code, enqVersion , Updated, UpdatedBy FROM dbEnquiry"              },
            { "Scripts",        "SELECT scrCode AS Code, scrVersion, Updated, UpdatedBy FROM dbScript"                },
            { "SearchList",     "SELECT schCode AS Code, schVersion, Updated, UpdatedBy FROM dbSearchListConfig"      },
            { "DataLists",      "SELECT enqTable AS Code, enqDLVersion, Updated, UpdatedBy FROM dbEnquiryDataList"    },
            { "Reports",        "SELECT rptCode AS Code, rptVersion, Updated, UpdatedBy FROM dbReport"                },
            { "FileManagement", "SELECT appCode AS Code, appVer, Updated, UpdatedBy FROM dbFileManagementApplication" }
        };

        private readonly Dictionary<string, DataSet> _datasets = new Dictionary<string, DataSet>();

        public DataProvider(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Initialize(connection);
                foreach (var kvp in _queries)
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(kvp.Value, connection))
                    {
                        DataSet dataset = new DataSet(kvp.Key);
                        adapter.Fill(dataset);
                        _datasets[kvp.Key] = dataset;
                    }
                }
            }
        }

        private void Initialize(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand(TableColumnQuery, connection))
            {
                cmd.Parameters.AddWithValue("@tableName", "dbFileManagementApplication");
                cmd.Parameters.AddWithValue("@columnName", "UpdatedBy");
                if (!Convert.ToBoolean(cmd.ExecuteScalar()))
                {
                    _queries["FileManagement"] = _queries["FileManagement"].Replace("UpdatedBy", "NULL").Replace("Updated", "NULL");
                }
            }
        }

        public IEnumerable<string> OmsTypes
        {
            get { return _datasets.Keys; }
        }

        public string GetVersionColumnName(string omsType)
        {
            return _datasets[omsType].Tables[0].Columns[1].ColumnName;
        }

        public OmsObjectInfo GetOmsObjectInfo(string omsType, string code)
        {
            OmsObjectInfo info = null;
            DataRow[] rows = _datasets[omsType].Tables[0].Select($"Code = '{code}'");
            if (rows.Length > 0)
            {
                info = new OmsObjectInfo(omsType, code);

                if (!Convert.IsDBNull(rows[0][1]))
                    info.Version = Convert.ToInt32(rows[0][1]);

                if (!Convert.IsDBNull(rows[0][2]))
                    info.Updated = DateTime.SpecifyKind(Convert.ToDateTime(rows[0][2]), DateTimeKind.Utc);

                if (!Convert.IsDBNull(rows[0][3]))
                    info.UpdatedBy = Convert.ToInt32(rows[0][3]);
            }
            return info;
        }

        public void Dispose()
        {
            foreach (DataSet dataset in _datasets.Values)
            {
                dataset.Dispose();
            }

            _datasets.Clear();
            _queries.Clear();
        }
    }
}
