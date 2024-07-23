namespace FWBS.Common.Elasticsearch
{
    public class FieldTitle
    {
        public FieldTitle(string name, string title)
        {
            Name = name;
            Title = title;
        }

        public string Name { get; set; }
        public string Title { get; set; }
        public byte FacetOrder { get; set; }
    }
}
