namespace Horizon.Common.Models.Repositories.IndexStructure
{
    public class IndexField
    {
        public IndexField(short entityId, string tableField, string indexField, string fieldType)
        {
            EntityId = entityId;
            TableFieldName = tableField;
            IndexFieldName = indexField;
            IndexFieldType = fieldType;
            Searchable = true;
        }

        public short EntityId { get; set; }
        public string TableFieldName { get; set; }
        public string IndexFieldName { get; set; }
        public string IndexFieldType { get; set; }
        public bool Searchable { get; set; }
        public bool Facetable { get; set; }
        public byte? FacetOrder { get; set; }
        public string FieldCode { get; set; }
        public bool Suggestable { get; set; }
        public string Analyzer { get; set; }
        public string ExtendedData { get; set; }
        public string FieldCodeLookupGroup { get; set; }
    }
}
