using Horizon.Common.Models.Repositories.IndexStructure;

namespace Horizon.Models.Settings
{
    public class IndexField
    {
        public IndexField(IndexFieldRow field)
        {
            IndexId = field.IndexId;
            EntityId = field.EntityId;
            Name = field.Name;
            FieldType = field.FieldType;
            Searchable = field.Searchable;
            Facetable = field.Facetable;
            FacetOrder = field.FacetOrder;
            Suggestable = field.Suggestable;
            Analyzer = field.Analyzer;
            IsDefault = field.IsDefault;
            ExtendedTable = field.ExtendedTable;
            FieldCode = field.FieldCode;
            FieldCodeLookupGroup = field.FieldCodeLookupGroup;
        }

        public short IndexId { get; set; }
        public short EntityId { get; set; }
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
        public string FieldDesc { get; set; }
        public string FieldCodeLookupGroup { get; set; }
    }
}
