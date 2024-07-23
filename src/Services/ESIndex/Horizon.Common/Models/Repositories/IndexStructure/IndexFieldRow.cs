namespace Horizon.Common.Models.Repositories.IndexStructure
{
    public class IndexFieldRow
    {
        public IndexFieldRow(short index, short entity, string name, string fieldType)
        {
            IndexId = index;
            EntityId = entity;
            Name = name;
            FieldType = fieldType;
            Searchable = true;
            IsDefault = true;
        }

        public short IndexId { get; set; }
        public short EntityId { get; set; }
        public string EntityName { get; set; }
        public string Name { get; set; }
        public string FieldType { get; set; }
        public bool Searchable { get; set; }
        public bool Facetable { get; set; }
        public byte? FacetOrder { get; set; }
        public bool Suggestable { get; set; }
        public string Analyzer { get; set; }
        public bool IsDefault { get; set; }
        public string ExtendedTable { get; set; }
        public string FieldCode { get; set; }
        public string FieldCodeLookupGroup { get; set; }
    }
}
