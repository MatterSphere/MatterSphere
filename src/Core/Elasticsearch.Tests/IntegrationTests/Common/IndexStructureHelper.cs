using System.Collections.Generic;

namespace Elasticsearch.Tests.IntegrationTests.Common
{
    public static class IndexStructureHelper
    {
        public static List<IndexField> GetDefaultDataFields()
        {
            return new List<IndexField>
            {
                new IndexField("id", "text")
                {
                    Searchable = false,
                    Analyzer = "whitespace"
                },
                new IndexField("mattersphereid", "text")
                {
                    Analyzer = "standard"
                },
                new IndexField("objecttype", "text")
                {
                    Facetable = true,
                    Analyzer = "keyword"
                },
                new IndexField("addressId", "long")
                {
                    Searchable = false
                },
                new IndexField("clientNotes", "text")
                {
                    Analyzer = "english"
                },
                new IndexField("clientNum", "text")
                {
                    Suggestable = true
                },
                new IndexField("clientType", "text")
                {
                    Facetable = true,
                    Analyzer = "keyword"
                },
                new IndexField("Name", "text")
                {
                    Suggestable = true
                },
                new IndexField("clientId", "long")
                {
                    Searchable = false,
                },
                new IndexField("fileDesc", "text")
                {
                    Suggestable = true,
                    Analyzer = "english"
                },
                new IndexField("fileStatus", "text")
                {
                    Facetable = true,
                    Analyzer = "keyword"
                },
                new IndexField("fileType", "text")
                {
                    Facetable = true,
                    Analyzer = "keyword"
                },
                new IndexField("ugdp", "text")
                {
                    Analyzer = "whitespace"
                }
            };
        }

        public static List<IndexField> GetDefaultUserFields()
        {
            return new List<IndexField>
            {
                new IndexField("id", "text")
                {
                    Searchable = false,
                    Analyzer = "whitespace"
                },
                new IndexField("mattersphereid", "text")
                {
                    Analyzer = "standard"
                },
                new IndexField("objecttype", "text")
                {
                    Facetable = true,
                    Analyzer = "keyword"
                },
                new IndexField("usrad", "text")
                {
                    Analyzer = "keyword"
                },
                new IndexField("usrsql", "text")
                {
                    Analyzer = "keyword"
                },
                new IndexField("usrAccessList", "text")
            };
        }
    }
}
