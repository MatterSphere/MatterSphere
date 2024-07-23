using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FWBS.Common;
using FWBS.Common.Elasticsearch;
using FWBS.OMS.Data;

namespace FWBS.OMS.Elasticsearch
{
    public class DbProvider : IDbProvider
    {
        public string[] GetSearchableFields()
        {
            var fields = GetFields();
            return fields.Where(field => field.Searchable)
                .Select(field => field.FieldName)
                .Distinct()
                .ToArray();
        }

        public string[] GetSuggestableFields()
        {
            var fields = GetFields();
            return fields.Where(field => field.Suggestable)
                .Select(field => field.FieldName)
                .Distinct()
                .ToArray();
        }

        public string[] GetFacetableFields()
        {
            var fields = GetFields();
            return fields.Where(field => field.Facetable)
                .Select(field => field.FieldName)
                .Distinct()
                .ToArray();
        }

        public FieldTitle[] GetFieldTitles()
        {
            var fields = new List<FieldTitle>();
            var indexId = GetIndicesInfo().First(index => index.IndexType == IndexTypeEnum.Data).Id;
            if (indexId == 0)
            {
                return fields.ToArray();
            }

            try
            {
                var connection = Session.CurrentSession.CurrentConnection;
                var ep = new DataSetExecuteParameters()
                {
                    CommandType = CommandType.StoredProcedure,
                    Sql = "[search].[ESGetIndexStuctureInfo]",
                    Tables = new[] { "Fields" },
                    SchemaOnly = false
                };
                ep.Parameters.Add(connection.CreateParameter("@ESIndexId", indexId));
                var dataSet = connection.Execute(ep);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    bool facetable;
                    if (bool.TryParse(row["facetable"].ToString(), out facetable))
                    {
                        if (facetable)
                        {
                            var item = new FieldTitle(row["FieldName"].ToString(), Session.CurrentSession.Terminology.Parse(row["FieldTitle"].ToString(), false));
                            byte facetOrder;
                            if (!byte.TryParse(row["FacetOrder"].ToString(), out facetOrder))
                            {
                                facetOrder = byte.MaxValue;
                            }
                            item.FacetOrder = facetOrder;
                            fields.Add(item);
                        }
                    }
                }
            }
            catch
            {

            }

            return fields.ToArray();
        }

        public string[] GetIndexedEntities()
        {
            var entities = new List<string>();
            try
            {
                var ep = new DataSetExecuteParameters()
                {
                    CommandType = CommandType.Text,
                    Sql = "SELECT ObjectType FROM search.ESIndexTable WHERE IndexingEnabled = 1",
                    Tables = new[] { "Entities" },
                    SchemaOnly = false
                };
                var dataSet = Session.CurrentSession.CurrentConnection.Execute(ep);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    entities.Add(row[0].ToString());
                }
            }
            catch
            {

            }

            return entities.ToArray();
        }

        private List<IndexField> GetFields()
        {
            var indices = GetIndicesInfo();
            var dataIndex = indices.First(it => it.IndexType == IndexTypeEnum.Data);

            return GetIndexFields(dataIndex.Id).Where(field => field.IndexingEnabled).ToList();
        }

        private List<IndexField> GetIndexFields(short indexId)
        {
            var fields = new List<IndexField>();
            var connection = Session.CurrentSession.CurrentConnection;
            var ep = new DataTableExecuteParameters()
            {
                CommandType = CommandType.StoredProcedure,
                Sql = "[search].[ESGetIndexStuctureInfo]",
                Table = "Fields",
                SchemaOnly = false
            };
            ep.Parameters.Add(connection.CreateParameter("@ESIndexId", indexId));
            var dataTable = connection.Execute(ep);

            foreach (DataRow row in dataTable.Rows)
            {
                bool searchable;
                Boolean.TryParse(row["searchable"].ToString(), out searchable);
                bool facetable;
                Boolean.TryParse(row["facetable"].ToString(), out facetable);
                bool suggestable;
                Boolean.TryParse(row["suggestable"].ToString(), out suggestable);
                bool isDefault;
                Boolean.TryParse(row["IsDefault"].ToString(), out isDefault);
                bool indexingEnabled;
                Boolean.TryParse(row["IndexingEnabled"].ToString(), out indexingEnabled);

                var field = new IndexField(row["FieldName"].ToString())
                {
                    Searchable = searchable,
                    Facetable = facetable,
                    Suggestable = suggestable,
                    IndexingEnabled = indexingEnabled
                };

                fields.Add(field);
            }

            return fields;
        }

        private List<IndexInfo> GetIndicesInfo()
        {
            var indices = new List<IndexInfo>();
            var ep = new DataTableExecuteParameters()
            {
                CommandType = CommandType.StoredProcedure,
                Sql = "[search].[ESGetIndexInfo]",
                Table = "Indices",
                SchemaOnly = false
            };
            var dataTable = Session.CurrentSession.CurrentConnection.Execute(ep);

            foreach (DataRow row in dataTable.Rows)
            {
                var field = new IndexInfo(
                    Int16.Parse(row["ESIndexId"].ToString()),
                    row["ESIndexName"].ToString(),
                    row["ESIndexType"].ToString());

                indices.Add(field);
            }

            return indices;
        }

        public bool IsSummaryFieldEnabled()
        {
            var parameters = new ExecuteParameters()
            {
                CommandType = CommandType.Text,
                Sql = "SELECT TOP 1 SummaryFieldEnabled FROM [search].[ChangeVersionControl]"
            };

            return ConvertDef.ToBoolean(Session.CurrentSession.CurrentConnection.ExecuteScalar(parameters), false);
        }

        private class IndexField
        {
            public IndexField(string name)
            {
                FieldName = name;
                IndexingEnabled = true;
            }
            
            public string FieldName { get; set; }
            public bool Searchable { get; set; }
            public bool Facetable { get; set; }
            public bool Suggestable { get; set; }
            public bool IndexingEnabled { get; set; }
        }

        private class IndexInfo
        {
            public IndexInfo(short id, string name, string type)
            {
                Id = id;
                Name = name;

                switch (type.ToLower())
                {
                    case "user":
                        IndexType = IndexTypeEnum.User;
                        break;
                    case "data":
                        IndexType = IndexTypeEnum.Data;
                        break;
                    default:
                        IndexType = IndexTypeEnum.Unknown;
                        break;
                }
            }

            public short Id { get; set; }
            private string Name { get; set; }
            public IndexTypeEnum IndexType { get; set; }
        }

        private enum IndexTypeEnum
        {
            Unknown = 0,
            User = 1,
            Data = 2
        }
    }
}
