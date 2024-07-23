namespace Models.DbModels
{
    public class IndexField
    {
        public IndexField(short indexId, string name, string type)
        {
            IndexId = indexId;
            FieldName = name;
            FieldType = type;
            IsDefault = true;
            IndexingEnabled = true;
        }

        public short IndexId { get; set; }
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public bool Searchable { get; set; }
        public bool Facetable { get; set; }
        public byte? FacetOrder { get; set; }
        public bool Suggestable { get; set; }
        public string Analyzer { get; set; }
        public bool IsDefault { get; set; }
        public bool IndexingEnabled { get; set; }
    }
}
