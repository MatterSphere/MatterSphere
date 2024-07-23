namespace Elasticsearch.Tests.IntegrationTests.Common
{
    public class IndexField
    {
        public IndexField(string name, string type)
        {
            Name = name;
            Type = type;
            Searchable = true;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public bool Searchable { get; set; }
        public bool Facetable { get; set; }
        public bool Suggestable { get; set; }
        public string Analyzer { get; set; }
    }
}
