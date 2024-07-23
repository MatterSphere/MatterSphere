using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.Search
{
    public class MSSearchProvider : ISearchProvider
    {
        private readonly string _dbConnection;
        private const string SP_SEARCH = "[dbo].[Search]";
        private readonly int? _userId;
        private readonly MSQueryBuilder _msQueryBuilder;
        private readonly List<FieldTitle> _fieldTitles;

        public MSSearchProvider(string connection, int? userId = null)
        {
            _dbConnection = connection;
            _userId = userId;
            _msQueryBuilder = new MSQueryBuilder();
            
            _fieldTitles = new List<FieldTitle>();
            var facetsDataTable = FWBS.OMS.CodeLookup.GetLookups("SEARCH");
            foreach (DataRow row in facetsDataTable.Rows)
            {
                _fieldTitles.Add(new FieldTitle(row["cdcode"].ToString(), Session.CurrentSession.Terminology.Parse(row["cddesc"].ToString(), false)));
            }
        }

        public SearchResult Search(SearchFilter filter)
        {
            if (filter.Query == null)
                return new SearchResult();

            var query = _msQueryBuilder.Build(filter.Query);

            using (SqlConnection conn = new SqlConnection(_dbConnection))
            {
                using (SqlCommand cmd = new SqlCommand(SP_SEARCH, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = Session.CurrentSession.Connection.CommandTimeout;

                    if (_userId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@UserId", _userId);
                    }
                    
                    cmd.Parameters.AddWithValue("@SearchKey", query);
                    cmd.Parameters.AddWithValue("@MAX_RECORDS", filter.PageInfo.Size);
                    cmd.Parameters.AddWithValue("@PageNo", filter.PageInfo.Page);
                    cmd.Parameters.AddWithValue("@IsSecurityAdmin", Session.CurrentSession.CurrentUser.IsInRoles("SECADMIN"));

                    if (filter.HasEntityFilter)
                    {
                        var entityTable = new DataTable("EntityFilter");
                        entityTable.Columns.Add(new DataColumn("EntityField", typeof(string)));
                        entityTable.Columns.Add(new DataColumn("EntityValue", typeof(long)));
                        entityTable.Columns.Add(new DataColumn("EntityPrimary", typeof(bool)));
                        entityTable.Rows.Add(filter.EntityFilter.Fields[0], filter.EntityFilter.Value, true);

                        foreach (var linkedEntity in filter.LinkedEntityFilter)
                        {
                            entityTable.Rows.Add(linkedEntity.Fields[0], linkedEntity.Value, false);
                        }

                        cmd.Parameters.AddWithValue("@EntityFilter", entityTable);
                    }

                    if (filter.HasFieldsFilter)
                    {
                        var facetsTable = new DataTable("FacetFilter");
                        facetsTable.Columns.Add(new DataColumn("FacetField", typeof(string)));
                        facetsTable.Columns.Add(new DataColumn("FacetValue", typeof(string)));

                        foreach (var field in filter.FieldsFilter)
                        {
                            facetsTable.Rows.Add(field.Field, field.Value);
                        }

                        cmd.Parameters.AddWithValue("@FacetFilter", facetsTable);
                    }

                    if (filter.HasTypesFilter)
                    {
                        var typeFilter = new StringBuilder();
                        for (int i = 0; i < filter.TypesFilter.Count; i++)
                        {
                            typeFilter.Append(filter.TypesFilter[i].ToString().ToUpper());
                            if (i < filter.TypesFilter.Count - 1)
                            {
                                typeFilter.Append(',');
                            }
                        }

                        cmd.Parameters.AddWithValue("@SearchIn", typeFilter.ToString());
                    }

                    conn.Open();
                    var adapter = new SqlDataAdapter(cmd);
                    var dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    conn.Close();

                    var total = 0;
                    if (dataSet.Tables[0].Rows.Count > 0)
                    {
                        Int32.TryParse(dataSet.Tables[0].Rows[0]["Total"].ToString(), out total);
                    }

                    return new SearchResult
                    {
                        Documents = GetDocuments(dataSet.Tables[0], query),
                        Aggregations = GetFacets(dataSet.Tables[1]),
                        TotalDocuments = total
                    };
                }
            }
        }

        public string[] Suggest(string query, int size)
        {
            return new string[0];
        }

        private List<ResponseItem> GetDocuments(DataTable table, string query)
        {
            var documents = new List<ResponseItem>();
            var converter = new EntityConverter();
            var highlighter = new Highlighter(_msQueryBuilder.GetQueryWords(query));

            foreach (DataRow row in table.Rows)
            {
                DateTime modifiedDate;
                DateTime.TryParse(row["modifieddate"].ToString(), out modifiedDate);

                var item = new CommonRow(row["EntityName"].ToString(), row["Id"].ToString())
                {
                    AssociateInfo = new CommonRow.Associate(highlighter.SetHighlights(row["Salutation"]),
                        highlighter.SetHighlights(row["AssociateType"])),
                    AppointmentInfo = new CommonRow.Appointment(highlighter.SetHighlights(row["appDesc"]),
                        highlighter.SetHighlights(row["AppointmentType"]), highlighter.SetHighlights(row["AppointmentLocation"])),
                    ClientInfo = new CommonRow.Client(highlighter.SetHighlights(row["ClientNumber"]),
                        highlighter.SetHighlights(row["ClientName"]), highlighter.SetHighlights(row["ClientType"])),
                    DocumentInfo = new CommonRow.Document(row["Author"].ToString(),
                        highlighter.SetHighlights(row["DocumentDescription"]), highlighter.SetHighlights(row["DocumentExtension"])),
                    PrecedentInfo = new CommonRow.Precedent(highlighter.SetHighlights(row["PrecedentTitle"]),
                        highlighter.SetHighlights(row["PrecedentCategory"]), highlighter.SetHighlights(row["PrecedentDescription"]),
                        highlighter.SetHighlights(row["PrecedentExtension"]))
                    {
                        Subcategory = highlighter.SetHighlights(row["PrecedentSubcategory"])
                    },
                    TaskInfo = new CommonRow.Task(highlighter.SetHighlights(row["TaskDescription"]), highlighter.SetHighlights(row["taskType"])),
                    Address = highlighter.SetHighlights(row["Address"]),
                    ContactName = highlighter.SetHighlights(row["ContactName"]),
                    FileDescription = highlighter.SetHighlights(row["FileDescription"]),
                    ModifiedDate = modifiedDate
                };

                long clientId;
                if (Int64.TryParse(row["ClientId"].ToString(), out clientId))
                {
                    item.ClientId = clientId;
                }

                long fileId;
                if (Int64.TryParse(row["FileId"].ToString(), out fileId))
                {
                    item.FileId = fileId;
                }

                long contactId;
                if (Int64.TryParse(row["ContactId"].ToString(), out contactId))
                {
                    item.ContactId = contactId;
                }

                var document = converter.Convert(item);
                documents.Add(document);
            }

            return documents;
        }

        private List<AggregationBucket> GetFacets(DataTable table)
        {
            var facets = new List<AggregationBucket>();
            List<Tuple<string, string, string, int>> rows = new List<Tuple<string, string, string, int>>();
            foreach (DataRow row in table.Rows)
            {
                int count;
                Int32.TryParse(row["Cnt"].ToString(), out count);
                rows.Add(new Tuple<string, string, string, int>(
                    row["FacetField"].ToString(),
                    row["cdCode"].ToString(),
                    row["FacetValue"].ToString(),
                    count));
            }

            facets = rows.GroupBy(row => row.Item1).Select(group =>
                new AggregationBucket(group.Key,
                    group.Select(facet => new AggregationBucket.AggregationItem(facet.Item3, facet.Item4)).ToList())
                {
                    Title = group.First().Item2
                }).ToList();

            foreach (var facet in facets)
            {
                var title = _fieldTitles.FirstOrDefault(field => field.Name == facet.Title)?.Title;
                if (!string.IsNullOrEmpty(title))
                {
                    facet.Title = title;
                }
            }

            return facets;
        }
    }
}
