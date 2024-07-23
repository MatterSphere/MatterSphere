using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Elasticsearch.Interfaces;
using Elasticsearch.Models;
using FWBS.Common.Elasticsearch;
using Newtonsoft.Json;

namespace Elasticsearch.Provider
{
    public class ResponseParser : IResponseParser
    {
        private readonly bool _isSummaryFieldEnabled;

        public ResponseParser() : this(false) { }
        
        public ResponseParser(bool isSummaryFieldEnabled)
        {
            this._isSummaryFieldEnabled = isSummaryFieldEnabled;
        }

        public SearchResult ParseResponse(string responseContent)
        {
            var content = JsonConvert.DeserializeObject<ResponseModel>(responseContent);

            var converter = new EntityConverter(_isSummaryFieldEnabled);
            var result = new SearchResult();
            foreach (var document in content.Documents.Documents)
            {
                result.Documents.Add(converter.Convert(document));
            }

            result.Aggregations = ConvertAggregations(content.Aggregations);

            result.TotalDocuments = content.Documents.Total;

            return result;
        }

        public string[] ParseSuggestResponse(string responseContent)
        {
            var contentModel = JsonConvert.DeserializeObject<ResponseModel>(responseContent);
            var json = JsonConvert.SerializeObject(contentModel.Suggest);
            var suggests = JsonConvert.DeserializeObject<IDictionary<string, dynamic>>(json);
           
            if (suggests == null)
            {
                return new string[0];
            }

            var suggestItems = new List<SuggestItem>();
            foreach (var suggest in suggests)
            {
                var optionsJson = JsonConvert.SerializeObject(suggest.Value[0].options);
                var optionsModel = JsonConvert.DeserializeObject<List<ResponseModel.OptionItem>>(optionsJson);
                var options = optionsModel as List<ResponseModel.OptionItem>;
                if (options != null)
                {
                    suggestItems.AddRange(options.Select(option => new SuggestItem(option.Text, option.Score)));
                }
            }

            return suggestItems.GroupBy(item => item.Suggest)
                .Select(group => new SuggestItem(group.Key, group.Max(item => item.Score)))
                .OrderByDescending(item => item.Score)
                .Select(item => item.Suggest).ToArray();
        }

        public string ParseUserResponse(string userContent)
        {
            var content = JsonConvert.DeserializeObject<ResponseModel>(userContent);
            var source = content.Documents.Documents[0].Source;

            return source.usrAccessList;
        }

        private List<AggregationBucket> ConvertAggregations(dynamic aggregations)
        {
            var list = new List<AggregationBucket>();
            if (aggregations == null)
            {
                return list;
            }

            var aggregationsModel = new RouteValueDictionary(aggregations);
            foreach (var item in aggregationsModel)
            {
                dynamic model = item.Value;
                var json = JsonConvert.SerializeObject(model.buckets);
                List<AggregationItem> items = JsonConvert.DeserializeObject<List<AggregationItem>>(json);
                var aggregationItem = new AggregationBucket(item.Key, items.Select(aggItem => new AggregationBucket.AggregationItem(aggItem.Key, aggItem.Number)));
                list.Add(aggregationItem);
            }

            return list.Where(item => item.Aggregations.Any()).ToList();
        }

        protected class SuggestItem
        {
            public SuggestItem(string suggest, double score)
            {
                Suggest = suggest;
                Score = score;
            }

            public string Suggest { get; }
            public double Score { get; }
        }
    }
}
