namespace Models.ElasticsearchModels
{
    public class Field
    {
        public Field(string name, string type, bool searchable = true)
        {
            Name = name;
            Type = type;
            Searchable = searchable;
        }

        public Field(string name, string type, bool searchable, bool facetable, bool suggestable, string analyzer = null) : this(name, type, searchable)
        {
            Facetable = facetable;
            Suggestable = suggestable;
            Analyzer = analyzer;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public bool Searchable { get; set; }
        public bool Facetable { get; set; }
        public bool Suggestable { get; set; }
        public string Analyzer { get; set; }
    }
}
