using System;
using System.Linq;
using Elasticsearch.Provider;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Elasticsearch.Tests.IntegrationTests
{
    [TestFixture]
    public class SearchQueryBuilderTest
    {
        [TestCase("+ - = && || > < ! ( ) { } [ ] ^ \" ~ * ? : \\ / ")]
        public void ReservedSymbolSearchSuccess(string query)
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter(query);

            var result = provider.Search(filter);

            Assert.IsNotNull(result);
        }

        [TestCase("+")]
        [TestCase("-")]
        [TestCase("=")]
        [TestCase("&&")]
        [TestCase("||")]
        [TestCase(">")]
        [TestCase("<")]
        [TestCase("!")]
        [TestCase("(")]
        [TestCase(")")]
        [TestCase("{")]
        [TestCase("}")]
        [TestCase("[")]
        [TestCase("]")]
        [TestCase("^")]
        [TestCase("\"")]
        [TestCase("~")]
        [TestCase("*")]
        [TestCase("?")]
        [TestCase(":")]
        [TestCase(@"\")]
        [TestCase("/")]
        public void ReservedSymbolSingleSearchSuccess(string query)
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter(query);

            var result = provider.Search(filter);

            Assert.IsNotNull(result);
        }

        [TestCase("+Jack")]
        [TestCase("-Jack")]
        [TestCase("=Jack")]
        [TestCase("&&Jack")]
        [TestCase("||Jack")]
        [TestCase(">Jack")]
        [TestCase("<Jack")]
        [TestCase("!Jack")]
        [TestCase("(Jack")]
        [TestCase(")Jack")]
        [TestCase("{Jack")]
        [TestCase("}Jack")]
        [TestCase("[Jack")]
        [TestCase("]Jack")]
        [TestCase("^Jack")]
        [TestCase("\"Jack")]
        [TestCase("~Jack")]
        [TestCase("*Jack")]
        [TestCase("?Jack")]
        [TestCase(":Jack")]
        [TestCase(@"\Jack")]
        [TestCase("/Jack")]
        public void ReservedSymbolAsPrefixSearchSuccess(string query)
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter(query);

            var result = provider.Search(filter);

            Assert.IsNotNull(result);
        }

        [TestCase("Jack+")]
        [TestCase("Jack-")]
        [TestCase("Jack=")]
        [TestCase("Jack&&")]
        [TestCase("Jack||")]
        [TestCase("Jack>")]
        [TestCase("Jack<")]
        [TestCase("Jack!")]
        [TestCase("Jack(")]
        [TestCase("Jack)")]
        [TestCase("Jack{")]
        [TestCase("Jack}")]
        [TestCase("Jack[")]
        [TestCase("Jack]")]
        [TestCase("Jack^")]
        [TestCase("Jack\"")]
        [TestCase("Jack~")]
        [TestCase("Jack*")]
        [TestCase("Jack?")]
        [TestCase("Jack:")]
        [TestCase(@"Jack\")]
        [TestCase("Jack/")]
        public void ReservedSymbolAsSuffixSearchSuccess(string query)
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter(query);

            var result = provider.Search(filter);

            Assert.IsNotNull(result);
        }

        [TestCase("Jack +")]
        [TestCase("Jack -")]
        [TestCase("Jack =")]
        [TestCase("Jack &&")]
        [TestCase("Jack ||")]
        [TestCase("Jack >")]
        [TestCase("Jack <")]
        [TestCase("Jack !")]
        [TestCase("Jack (")]
        [TestCase("Jack )")]
        [TestCase("Jack {")]
        [TestCase("Jack }")]
        [TestCase("Jack [")]
        [TestCase("Jack ]")]
        [TestCase("Jack ^")]
        [TestCase("Jack \"")]
        [TestCase("Jack ~")]
        [TestCase("Jack *")]
        [TestCase("Jack ?")]
        [TestCase("Jack :")]
        [TestCase(@"Jack \")]
        [TestCase("Jack /")]
        public void ReservedSymbolAsPartOfQuerySearchSuccess(string query)
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter(query);

            var result = provider.Search(filter);

            Assert.IsNotNull(result);
        }

        [Test]
        public void Bug_40325_Test()
        {
            var factory = new SearchFactory(new SearchQueryBuilder(), new ResponseParser());
            var provider = factory.CreateProvider(ElasticConfiguration.Url, ElasticConfiguration.SearchApiKey, ElasticConfiguration.DataIndex);
            var filter = new SearchFilter("Matter 100/1");

            var result = provider.Search(filter);

            Assert.AreEqual(1, result.Documents.Count(d => d.ElasticsearchId == "338842C5-1A0E-47E7-A8E4-61DF47C0EE0B"));
        }

        [Test]
        public void CreateSearchRequest_With_Type_Filter_Success()
        {
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);

            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), o.SelectToken("query.bool.must[1].bool.should[0].term.objecttype.value").ToString());
        }

        // test FieldsFilterTest with 1 Range filter - begin and end
        // equivalent SQL where:
        // WHERE
        //  objecttype = 'DOCUMENT'
        //  AND (modifieddate BETWEEN '2020-11-15' AND '2021-2-15')
        [Test]
        public void CreateSearchRequest_With_1_Range_Filter_begin_end_Success()
        {
            var testBeginDate = new DateTime(2020, 11, 15);
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.GreaterOrEqual });
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.AddMonths(3).ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.LessOrEqual });

            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(2, o.SelectToken("query.bool.must[1].bool.should[0].bool.must").Count());
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.AreEqual(testBeginDate, (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
            Assert.AreEqual(testBeginDate.AddMonths(3), (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
        }

        // test FieldsFilterTest with 1 Range filter - begin only
        // equivalent SQL where:
        // WHERE
        //  objecttype = 'DOCUMENT'
        //  AND modifieddate >= '2020-11-15'
        [Test]
        public void CreateSearchRequest_With_1_Range_Filter_begin_only_Success()
        {
            var testBeginDate = new DateTime(2020, 11, 15);
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.GreaterOrEqual });

            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(2, o.SelectToken("query.bool.must[1].bool.should[0].bool.must").Count());
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.AreEqual(testBeginDate, (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.IsNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
        }

        // test FieldsFilterTest with 1 Range filter - end only
        // equivalent SQL where:
        // WHERE
        //  objecttype = 'DOCUMENT'
        //  AND modifieddate <= '2020-11-15'
        [Test]
        public void CreateSearchRequest_With_1_Range_Filter_end_only_Success()
        {
            var testBeginDate = new DateTime(2020, 11, 15);
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.LessOrEqual });

            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(2, o.SelectToken("query.bool.must[1].bool.should[0].bool.must").Count());
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
            Assert.AreEqual(testBeginDate, (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
            Assert.IsNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
        }

        // test FieldsFilterTest with 2 Range filter - (1, 2) begin and end
        // equivalent SQL where:
        // WHERE
        //  objecttype = 'DOCUMENT'
        //  AND (modifieddate BETWEEN '2020-11-15' AND '2021-5-15')
        //  AND mattersphereid <= '99'
        [Test]
        public void CreateSearchRequest_With_2_Range_Filter_beginend_end_Success()
        {
            var testBeginDate = new DateTime(2020, 11, 15);
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.AddMonths(6).ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.LessOrEqual });
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.GreaterOrEqual });
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("mattersphereid", "99") { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.LessOrEqual });

            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(3, o.SelectToken("query.bool.must[1].bool.should[0].bool.must").Count());
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
            Assert.AreEqual(testBeginDate.AddMonths(6), (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.AreEqual(testBeginDate, (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid.lte"));
            Assert.AreEqual("99", (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid.lte"));
        }

        // test FieldsFilterTest with 2 Range filter - (1) begin and (2) end
        // equivalent SQL where:
        // WHERE
        //  objecttype = 'DOCUMENT'
        //  AND modifieddate >= '2020-11-15'
        //  AND mattersphereid <= '99'
        [Test]
        public void CreateSearchRequest_With_2_Range_Filter_begin_end_Success()
        {
            var testBeginDate = new DateTime(2020, 11, 15);
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.GreaterOrEqual });
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("mattersphereid", "99") { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.LessOrEqual });

            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(3, o.SelectToken("query.bool.must[1].bool.should[0].bool.must").Count());
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.AreEqual(testBeginDate, (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid.lte"));
            Assert.AreEqual("99", (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid.lte"));
        }


        // test FieldsFilterTest with 2 Entity - 2 Range filter - (1, 2) begin and (1, 2) end
        // equivalent SQL where:
        // WHERE
        //  objecttype = 'DOCUMENT'
        //  AND (modifieddate BETWEEN '2020-11-15' AND '2021-5-15')
        //  AND (mattersphereid >= '00' AND mattersphereid <= '99')
        [Test]
        public void CreateSearchRequest_With_2_Range_Filter_beginend_beginend_Success()
        {
            var testBeginDate = new DateTime(2020, 11, 15);
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.AddMonths(6).ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.LessOrEqual });
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.GreaterOrEqual });
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("mattersphereid", "00") { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.GreaterOrEqual });
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("mattersphereid", "99") { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.LessOrEqual });

            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(3, o.SelectToken("query.bool.must[1].bool.should[0].bool.must").Count());
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.AreEqual(testBeginDate, (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
            Assert.AreEqual(testBeginDate.AddMonths(6), (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid.gte"));
            Assert.AreEqual("00", (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid.gte"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid.lte"));
            Assert.AreEqual("99", (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].range.mattersphereid.lte"));
        }

        [Test]
        public void CreateSearchRequest_Request_Should_Not_Contain_Nulls()
        {
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", DateTime.Now.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.GreaterOrEqual });

            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.That(o.SelectToken("query.bool.must").ToList(), Does.Not.Contains(JValue.CreateNull()), "query.bool.must[] array should not contain nulls.");
        }

        // test FieldsFilterTest with 1 Term filter
        // equivalent SQL where:
        // WHERE
        //  objecttype = 'DOCUMENT'
        //  AND modifieddate = '2020-11-15'
        [Test]
        public void CreateSearchRequest_With_1_Term_Filter_Success()
        {
            var testBeginDate = new DateTime(2020, 11, 15);
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.EqualTo });
            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(2, o.SelectToken("query.bool.must[1].bool.should[0].bool.must").Count());
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].term.modifieddate"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].term.modifieddate.value"));
            Assert.AreEqual(testBeginDate, (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].term.modifieddate.value"));
        }

        // test FieldsFilterTest with 1 Term filter
        // equivalent SQL where:
        // WHERE
        //  objecttype = 'DOCUMENT'
        //  AND modifieddate = '2020-11-15'
        //  AND mattersphereid = '10'
        [Test]
        public void CreateSearchRequest_With_2_Term_Filter_Success()
        {
            var testBeginDate = new DateTime(2020, 11, 15);
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.EqualTo });
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("mattersphereid", "10") { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.EqualTo });
            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(3, o.SelectToken("query.bool.must[1].bool.should[0].bool.must").Count());
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].term.modifieddate"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].term.modifieddate.value"));
            Assert.AreEqual(testBeginDate, (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].term.modifieddate.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].term.mattersphereid"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].term.mattersphereid.value"));
            Assert.AreEqual("10", (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].term.mattersphereid.value"));
        }

        // test FieldsFilterTest with 1 Term filter
        // equivalent SQL where:
        // WHERE
        //  objecttype = 'DOCUMENT'
        //  AND (modifieddate BETWEEN '2020-11-15' AND '2021-5-15')
        //  AND mattersphereid = '10'
        [Test]
        public void CreateSearchRequest_With_2_Term_and_Range_Filter_Success()
        {
            var testBeginDate = new DateTime(2020, 11, 15);
            SearchQueryBuilder searchBuilder = new SearchQueryBuilder();
            var factory = new SearchFactory(searchBuilder, new ResponseParser());
            var filter = new SearchFilter("*");
            filter.TypesFilter.Add(EntityTypeEnum.Document);
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.GreaterOrEqual});
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("modifieddate", testBeginDate.AddMonths(6).ToString("O")) { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.LessOrEqual });
            filter.FieldsFilter.Add(new SearchFilter.FieldFilterItem("mattersphereid", "10") { EntityType = EntityTypeEnum.Document, Operator = ComparisonOperator.EqualTo });
            dynamic requestBody = searchBuilder.CreateSearchRequestBody(filter,
                factory.SearchableFields,
                factory.FacetableFields,
                10);
            JObject o = JObject.Parse(JsonConvert.SerializeObject(requestBody));
            Console.WriteLine(o.ToString(Formatting.Indented));

            Assert.AreEqual(3, o.SelectToken("query.bool.must[1].bool.should[0].bool.must").Count());
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.AreEqual(EntityTypeEnum.Document.ToString().ToUpper(), (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[0].term.objecttype.value"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.AreEqual(testBeginDate, (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.gte"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
            Assert.AreEqual(testBeginDate.AddMonths(6), (DateTime)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[1].range.modifieddate.lte"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].term.mattersphereid"));
            Assert.IsNotNull(o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].term.mattersphereid.value"));
            Assert.AreEqual("10", (string)o.SelectToken("query.bool.must[1].bool.should[0].bool.must[2].term.mattersphereid.value"));
        }
    }
}