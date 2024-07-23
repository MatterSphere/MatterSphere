using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Common.AdoNet;
using Horizon.Common.Interfaces;
using Horizon.Common.Models.Repositories.IndexStructure;

namespace Horizon.DAL.AdoNetRepository
{
    public class IndexStructureRepository : IIndexStructureRepository
    {
        private readonly string _connection;

        public IndexStructureRepository(string connection)
        {
            _connection = connection;
        }

        public List<IndexInfo> GetIndices()
        {
            var result = new List<IndexInfo>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetIndexInfo]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        short id;
                        Int16.TryParse(row["ESIndexId"].ToString(), out id);
                        var info = new IndexInfo(
                            id,
                            row["ESIndexName"].ToString(),
                            IndexType.GetIndexType(row["ESIndexType"].ToString()));
                        result.Add(info);
                    }
                }
            }

            return result;
        }

        public List<IndexEntity> GetIndexEntities(short indexId)
        {
            var result = new List<IndexEntity>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetIndexTableInfo]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new ShortParameter("ESIndexId", indexId)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        short id;
                        Int16.TryParse(row["ESIndexTableId"].ToString(), out id);
                        bool indexingEnabled;
                        Boolean.TryParse(row["IndexingEnabled"].ToString(), out indexingEnabled);
                        bool isDefault;
                        Boolean.TryParse(row["IsDefault"].ToString(), out isDefault);
                        var entity = new IndexEntity(
                            id,
                            row["ObjectType"].ToString(),
                            row["tablename"].ToString(),
                            row["pkFieldName"].ToString())
                        {
                            IndexingEnabled = indexingEnabled,
                            IsDefault = isDefault,
                            SummaryTemplate = row["summaryTemplate"].ToString(),
                        };
                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public List<IndexFieldRow> GetIndexFields(short entityId)
        {
            var result = new List<IndexFieldRow>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetIndexStuctureInfoByObjectType]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new ShortParameter("ESIndexTableId", entityId)
                    };

                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        short indexId;
                        Int16.TryParse(row["ESIndexId"].ToString(), out indexId);
                        bool searchable;
                        Boolean.TryParse(row["searchable"].ToString(), out searchable);
                        bool facetable;
                        Boolean.TryParse(row["facetable"].ToString(), out facetable);
                        bool suggestable;
                        Boolean.TryParse(row["Suggestable"].ToString(), out suggestable);
                        bool isDefault;
                        Boolean.TryParse(row["IsDefault"].ToString(), out isDefault);

                        var entity = new IndexFieldRow(
                            indexId,
                            entityId,
                            row["FieldName"].ToString(),
                            row["ESFieldType"].ToString())
                        {
                            Searchable = searchable,
                            Facetable = facetable,
                            Suggestable = suggestable,
                            Analyzer = row["Analyzer"].ToString(),
                            IsDefault = isDefault,
                            ExtendedTable = row["ExtTable"].ToString(),
                            FieldCode = row["cdCode"].ToString(),
                            FieldCodeLookupGroup = row["fieldCodeLookupGroup"].ToString()
                        };

                        byte facetOrder;
                        if (byte.TryParse(row["FacetOrder"].ToString(), out facetOrder))
                        {
                            entity.FacetOrder = facetOrder;
                        }

                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public List<IndexFieldRow> GetAllIndexFields(short indexId)
        {
            var result = new List<IndexFieldRow>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESGetIndexStuctureInfo]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new ShortParameter("ESIndexId", indexId)
                    };

                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        short esIndexId;
                        Int16.TryParse(row["ESIndexId"].ToString(), out esIndexId);
                        short entityId;
                        Int16.TryParse(row["ESIndexTableId"].ToString(), out entityId);
                        bool searchable;
                        Boolean.TryParse(row["searchable"].ToString(), out searchable);
                        bool facetable;
                        Boolean.TryParse(row["facetable"].ToString(), out facetable);
                        byte facetOrder;
                        bool suggestable;
                        Boolean.TryParse(row["Suggestable"].ToString(), out suggestable);
                        bool isDefault;
                        Boolean.TryParse(row["IsDefault"].ToString(), out isDefault);

                        var index = new IndexFieldRow(
                            esIndexId,
                            entityId,
                            row["FieldName"].ToString(),
                            row["ESFieldType"].ToString())
                        {
                            Searchable = searchable,
                            Facetable = facetable,
                            Suggestable = suggestable,
                            Analyzer = row["Analyzer"].ToString(),
                            IsDefault = isDefault,
                            FieldCode = row["cdCode"].ToString()
                        };

                        if (byte.TryParse(row["FacetOrder"].ToString(), out facetOrder))
                        {
                            index.FacetOrder = facetOrder;
                        }

                        result.Add(index);
                    }
                }
            }

            return result;
        }

        public void UpdateIndexField(IndexField field)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESUpdateIndexStructure]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var entityIdParameter = new ShortParameter("ESIndexTableId ", field.EntityId);
                    entityIdParameter.AddSqlParameter(cmd.Parameters);


                    var indexFieldNameParameter = new StringParameter("FieldName ", field.IndexFieldName, 128);
                    indexFieldNameParameter.AddSqlParameter(cmd.Parameters);

                    if (field.Facetable)
                    {
                        var fieldCodeParameter = new StringParameter("cdCode ", field.FieldCode, 15);
                        fieldCodeParameter.AddSqlParameter(cmd.Parameters);
                    }

                    var indexFieldTypeParameter = new StringParameter("ESFieldType ", field.IndexFieldType, 50);
                    indexFieldTypeParameter.AddSqlParameter(cmd.Parameters);

                    var searchableParameter = new BoolParameter("searchable ", field.Searchable);
                    searchableParameter.AddSqlParameter(cmd.Parameters);

                    var facetableParameter = new BoolParameter("facetable ", field.Facetable);
                    facetableParameter.AddSqlParameter(cmd.Parameters);

                    if (field.FacetOrder.HasValue)
                    {
                        var facetOrderParameter = new ByteParameter("FacetOrder", field.FacetOrder.Value);
                        facetOrderParameter.AddSqlParameter(cmd.Parameters);
                    }

                    var suggestableParameter = new BoolParameter("Suggestable ", field.Suggestable);
                    suggestableParameter.AddSqlParameter(cmd.Parameters);

                    var analyzerParameter = new StringParameter("Analyzer ", field.Analyzer, 50);
                    analyzerParameter.AddSqlParameter(cmd.Parameters);

                    if (!string.IsNullOrEmpty(field.FieldCodeLookupGroup))
                    {
                        var fieldCodeParameter = new StringParameter("@FieldCodeLookupGroup ", field.FieldCodeLookupGroup, 15);
                        fieldCodeParameter.AddSqlParameter(cmd.Parameters);
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCommonIndexField(short indexId, IndexField field)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE [search].[ESIndexCommonStructure] SET FacetOrder=@FacetOrder WHERE ESIndexId=@IndexId AND FieldName=@FieldName", conn))
                {
                    cmd.CommandType = CommandType.Text;

                    var indexIdParameter = new ShortParameter("IndexId ", indexId);
                    indexIdParameter.AddSqlParameter(cmd.Parameters);

                    var indexFieldNameParameter = new StringParameter("FieldName ", field.IndexFieldName, 128);
                    indexFieldNameParameter.AddSqlParameter(cmd.Parameters);

                    if (field.FacetOrder.HasValue)
                    {
                        var facetOrderParameter = new ByteParameter("FacetOrder", field.FacetOrder.Value);
                        facetOrderParameter.AddSqlParameter(cmd.Parameters);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("FacetOrder", DBNull.Value);
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteIndexField(short entityId, string field)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESDeleteStructureField]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var entityIdParameter = new ShortParameter("ESIndexTableId", entityId);
                    entityIdParameter.AddSqlParameter(cmd.Parameters);

                    var fieldParameter = new StringParameter("FieldName", field, 128);
                    fieldParameter.AddSqlParameter(cmd.Parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool CheckExtendedData(string tableName, string pkFieldName)
        {
            var result = false;
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESCheckExtendedTable]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var tableNameParameter = new StringParameter("TableName ", tableName, 128);
                    tableNameParameter.AddSqlParameter(cmd.Parameters);

                    var pkFieldNameParameter = new StringParameter("pkFieldName ", pkFieldName, 128);
                    pkFieldNameParameter.AddSqlParameter(cmd.Parameters);

                    SqlParameter returnValue = new SqlParameter();
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    cmd.Parameters.Add(returnValue);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    result = (int)returnValue.Value == 0;
                }
            }

            return result;
        }

        public List<TableField> GetTableFields(string tableName)
        {
            var result = new List<TableField>();

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[fwbsXDcols]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    var parameters = new List<Parameter>
                    {
                        new VarcharParameter("table", tableName, 100)
                    };
                    var adapter = new SqlDataAdapterFactory().GetSqlDataAdapter(cmd, parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        var entity = new TableField(
                            row[2].ToString(),
                            row[3].ToString());

                        result.Add(entity);
                    }
                }
            }

            return result;
        }

        public void AddField(IndexField field)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[ESAddStructureField]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var entityIdParameter = new ShortParameter("ESIndexTableId ", field.EntityId);
                    entityIdParameter.AddSqlParameter(cmd.Parameters);

                    var tableFieldNameParameter = new StringParameter("TableFieldName ", field.TableFieldName, 128);
                    tableFieldNameParameter.AddSqlParameter(cmd.Parameters);

                    var indexFieldNameParameter = new StringParameter("FieldName ", field.IndexFieldName, 128);
                    indexFieldNameParameter.AddSqlParameter(cmd.Parameters);

                    if (field.Facetable)
                    {
                        var fieldCodeParameter = new StringParameter("cdCode ", field.FieldCode, 15);
                        fieldCodeParameter.AddSqlParameter(cmd.Parameters);
                    }

                    var indexFieldTypeParameter = new StringParameter("ESFieldType ", field.IndexFieldType, 50);
                    indexFieldTypeParameter.AddSqlParameter(cmd.Parameters);

                    var searchableParameter = new BoolParameter("searchable ", field.Searchable);
                    searchableParameter.AddSqlParameter(cmd.Parameters);

                    var facetableParameter = new BoolParameter("facetable ", field.Facetable);
                    facetableParameter.AddSqlParameter(cmd.Parameters);

                    if (field.FacetOrder.HasValue)
                    {
                        var facetOrderParameter = new ByteParameter("FacetOrder", field.FacetOrder.Value);
                        facetOrderParameter.AddSqlParameter(cmd.Parameters);
                    }

                    var suggestableParameter = new BoolParameter("Suggestable ", field.Suggestable);
                    suggestableParameter.AddSqlParameter(cmd.Parameters);

                    var analyzerParameter = new StringParameter("Analyzer ", field.Analyzer, 50);
                    analyzerParameter.AddSqlParameter(cmd.Parameters);

                    var extendedDataParameter = new StringParameter("ExtTable ", field.ExtendedData, 128);
                    extendedDataParameter.AddSqlParameter(cmd.Parameters);

                    if (!string.IsNullOrEmpty(field.FieldCodeLookupGroup))
                    {
                        var fieldCodeLookupGroupParameter = new StringParameter("FieldCodeLookupGroup ", field.FieldCodeLookupGroup, 15);
                        fieldCodeLookupGroupParameter.AddSqlParameter(cmd.Parameters);
                    }

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ChangeIndexingEnabling(short entityId, bool enable)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[EntityToFromIndexing]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var entityIdParameter = new ShortParameter("ESIndexTableId ", entityId);
                    entityIdParameter.AddSqlParameter(cmd.Parameters);

                    var enableParameter = new BoolParameter("IndexingEnabled ", enable);
                    enableParameter.AddSqlParameter(cmd.Parameters);
                    
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateSummaryTemplate(IndexEntity indexEntity)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[search].[UpdateSummaryTemplate]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var entityIdParameter = new ShortParameter("ESIndexTableId", indexEntity.Id);
                    entityIdParameter.AddSqlParameter(cmd.Parameters);

                    var summaryTemplateParameter = new StringParameter("SummaryTemplate", indexEntity.SummaryTemplate);
                    summaryTemplateParameter.AddSqlParameter(cmd.Parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetCodeLookupGroups()
        {
            var result = new SortedSet<string>(StringComparer.InvariantCultureIgnoreCase);

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[sprCodeLookupGroupList]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result.Add(reader.GetString(0));
                    }
                }
            }

            return result.ToList();
        }

        public Dictionary<string, string> GetFacetableCodeLookups()
        {
            var result = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);

            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[sprCodeLookupList]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var typeParameter = new StringParameter("Type", "SEARCH", 15);
                    typeParameter.AddSqlParameter(cmd.Parameters);

                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        result[reader.GetString(0)] = reader.GetString(1);
                    }
                }
            }

            return result;
        }

        public void CreateFacetableCodeLookup(string cdCode, string cdDesc)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[sprCreateCodeLookup]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var typeParameter = new StringParameter("Type", "SEARCH", 15);
                    typeParameter.AddSqlParameter(cmd.Parameters);

                    var codeParameter = new StringParameter("Code", cdCode, 15);
                    codeParameter.AddSqlParameter(cmd.Parameters);

                    var descParameter = new StringParameter("Description", cdDesc, 1000);
                    descParameter.AddSqlParameter(cmd.Parameters);

                    var helpParameter = new StringParameter("Help", null);
                    helpParameter.AddSqlParameter(cmd.Parameters);

                    var notesParameter = new StringParameter("Notes", null);
                    notesParameter.AddSqlParameter(cmd.Parameters);

                    var addlinkParameter = new StringParameter("AddLink", null);
                    addlinkParameter.AddSqlParameter(cmd.Parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateFacetableCodeLookup(string cdCode, string cdDesc)
        {
            using (SqlConnection conn = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[dbCodeLookup] SET cdDesc = @Description WHERE cdType = 'SEARCH' AND cdCode = @Code AND cdUICultureInfo = '{default}'", conn))
                {
                    var codeParameter = new StringParameter("Code", cdCode, 15);
                    codeParameter.AddSqlParameter(cmd.Parameters);

                    var descParameter = new StringParameter("Description", cdDesc, 1000);
                    descParameter.AddSqlParameter(cmd.Parameters);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
