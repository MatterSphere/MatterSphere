using System.Collections.Generic;
using System.Linq;
using Elasticsearch.Provider;
using FWBS.Common.Elasticsearch;
using NUnit.Framework;

namespace Elasticsearch.Tests.IntegrationTests
{
    [TestFixture]
    public class SearchProviderTest
    {
        private const string _startTag = "<highlight>";
        private const string _endTag = "</highlight>";

        [Test]
        public void GetClientByName()
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter("Arthur");

            var result = provider.Search(filter);

            Assert.AreEqual(1, result.Documents.Count);
            Assert.AreEqual("722F9D66-8C1C-4C30-A415-F0392990F68B", result.Documents[0].ElasticsearchId);
        }

        [Test]
        public void GetThreeAggregations()
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            factory.FacetableFields = new [] { "objecttype", "fileStatus", "clientType" };
            factory.FacetSize = 5;
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter("*");

            var result = provider.Search(filter);

            Assert.AreEqual(3, result.Aggregations.Count);
            Assert.IsTrue(result.Aggregations.Any(agg => agg.Field == "objecttype"));
            var objectTypeAggregation = result.Aggregations.First(agg => agg.Field == "objecttype");
            Assert.AreEqual(2, objectTypeAggregation.Aggregations.Count);
            Assert.AreEqual(5, objectTypeAggregation.Aggregations.First(item => item.Value == "CLIENT").Number);
            Assert.AreEqual(4, objectTypeAggregation.Aggregations.First(item => item.Value == "FILE").Number);

            Assert.IsTrue(result.Aggregations.Any(agg => agg.Field == "fileStatus"));
            var fileStatusAggregation = result.Aggregations.First(agg => agg.Field == "fileStatus");
            Assert.AreEqual(2, fileStatusAggregation.Aggregations.Count);
            Assert.AreEqual(3, fileStatusAggregation.Aggregations.First(item => item.Value == "live").Number);
            Assert.AreEqual(1, fileStatusAggregation.Aggregations.First(item => item.Value == "closed").Number);

            Assert.IsTrue(result.Aggregations.Any(agg => agg.Field == "clientType"));
            var clientTypeAggregation = result.Aggregations.First(agg => agg.Field == "clientType");
            Assert.AreEqual(3, clientTypeAggregation.Aggregations.Count);
            Assert.AreEqual(6, clientTypeAggregation.Aggregations.First(item => item.Value == "Person").Number);
            Assert.AreEqual(1, clientTypeAggregation.Aggregations.First(item => item.Value == "Pre client").Number);
            Assert.AreEqual(2, clientTypeAggregation.Aggregations.First(item => item.Value == "Company").Number);
        }

        [Test]
        public void FilterByType()
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter("*")
            {
                TypesFilter = new List<EntityTypeEnum> { EntityTypeEnum.Client }
            };

            var result = provider.Search(filter);

            Assert.AreEqual(5, result.Documents.Count);
            Assert.AreEqual(5, result.Documents.Count(doc => doc.ObjectType == "client"));
        }
        
        [Test]
        public void FilterByEntity()
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter("*")
            {
                EntityFilter = new SearchFilter.EntityFilterData(EntityTypeEnum.Client, "1")
            };

            var result = provider.Search(filter);

            Assert.AreEqual(4, result.Documents.Count);
            Assert.IsTrue(result.Documents.Any(document => document.ElasticsearchId == "37D2AAA7-D2B2-4555-8378-E155549217A0"));
            Assert.IsTrue(result.Documents.Any(document => document.ElasticsearchId == "C0143517-B5AC-4D81-99D3-275F4BC5925B"));
            Assert.IsTrue(result.Documents.Any(document => document.ElasticsearchId == "00F8EA97-C089-425B-8ACD-5C8A3458C1EF"));
        }

        [Test]
        public void FilterByField()
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter("*")
            {
                FieldsFilter = new List<SearchFilter.FieldFilterItem> { new SearchFilter.FieldFilterItem("fileType", "Default") }
            };

            var result = provider.Search(filter);

            Assert.AreEqual(3, result.Documents.Count);
            Assert.IsTrue(result.Documents.Any(document => document.ElasticsearchId == "00F8EA97-C089-425B-8ACD-5C8A3458C1EF"));
            Assert.IsTrue(result.Documents.Any(document => document.ElasticsearchId == "3547630C-12F5-444B-98CB-98F77CC7A5B4"));
        }

        [TestCase("F", 6)]
        [TestCase("Denis", 1)]
        public void GetSuggest(string query, int expectedResult)
        {
            var size = 10;
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser())
            {
                SuggestableFields = new[] { "clientNumSuggest", "NameSuggest", "fileDescSuggest" }
            };
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);

            var result = provider.Suggest(query, size);

            Assert.AreEqual(expectedResult, result.Length);
        }

        [TestCase("Denis")]
        public void GetHighlights(string query)
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter(query)
            {
                WithHighlights = true
            };

            var result = provider.Search(filter);

            Assert.IsTrue(result.Documents[0].Name.Contains($"{_startTag}{query}{_endTag}"));
            Assert.IsTrue(result.Documents[1].Name.Contains($"{_startTag}{query}{_endTag}"));
        }

        [TestCase("Denis")]
        public void SearchWithHighlightsOff(string query)
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter(query);

            var result = provider.Search(filter);

            Assert.IsFalse(result.Documents[0].Name.Contains($"{_startTag}{query}{_endTag}"));
            Assert.IsFalse(result.Documents[1].Name.Contains($"{_startTag}{query}{_endTag}"));
        }

        [Test]
        public void UserFindsDocumentWithAllowUgdp()
        {
            var user = "User1";
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser())
            {
                AdUser = user,
                SqlUser = user
            };
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex, ElasticConfiguration.UserIndex);
            var filter = new SearchFilter("Tomas");

            var result = provider.Search(filter);

            Assert.AreEqual(1, result.Documents.Count);
            Assert.IsTrue(result.Documents.Any(document => document.ElasticsearchId == "37D2AAA7-D2B2-4555-8378-E155549217A0"));
        }

        [TestCase("Arthur")]
        [TestCase("Jack")]
        public void UserDoesNotFindDocumentWithDenyUgdp(string query)
        {
            var user = "User34";
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser())
            {
                AdUser = user,
                SqlUser = user
            };
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex, ElasticConfiguration.UserIndex);
            var filter = new SearchFilter("query");

            var result = provider.Search(filter);

            Assert.AreEqual(0, result.Documents.Count);
        }

        [Test]
        public void UserDoesNotFindDocumentWithoutUsersAllowUgdp()
        {
            var user = "User2";
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser())
            {
                AdUser = user,
                SqlUser = user
            };
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex, ElasticConfiguration.UserIndex);
            var filter = new SearchFilter("Tomas");

            var result = provider.Search(filter);
            
            Assert.AreEqual(0, result.Documents.Count);
        }
    }
}