using System.Collections.Generic;
using System.Dynamic;

namespace Models.ElasticsearchModels
{
    public class IndexDocument
    {
        public IndexDocument(ExpandoObject document, Dictionary<string, string> suggests)
        {
            Document = document;
            Suggests = suggests;
        }
        public ExpandoObject Document { get; set; }
        public Dictionary<string, string> Suggests { get; set; }
    }
}