using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using ElasticsearchProvider.Models;
using Models.Common;
using Models.ElasticsearchModels;
using Models.Interfaces;
using Newtonsoft.Json;
using RestSharp;

namespace ElasticsearchProvider
{
    public class Provider : IElasticsearchProvider
    {
        private RestClient _restClient;
        private string _dataIndex;
        private string _userIndex;
        private readonly Settings _dataIndexSettings;
        private readonly Settings _userIndexSettings;
        private ISuggestionsFactory _suggestionsFactory;
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public Provider(RestClient restClient, string dataIndex, Settings dataIndexSettings, string userIndex, Settings userIndexSettings)
        {
            _restClient = restClient;
            _dataIndex = dataIndex;
            _dataIndexSettings = dataIndexSettings;
            _userIndex = userIndex;
            _userIndexSettings = userIndexSettings;
            _suggestionsFactory = new SuggestionsFactory();
        }

        public void CreateUserIndex(List<Field> fields)
        {
            CreateIndex(_userIndex, _userIndexSettings, fields);
        }

        public void CreateDataIndex(List<Field> fields)
        {
            CreateIndex(_dataIndex, _dataIndexSettings, fields);
        }

        public bool CheckUserIndex()
        {
            return CheckIndex(_userIndex);
        }

        public bool CheckDataIndex()
        {
            return CheckIndex(_dataIndex);
        }

        public bool CheckService(out string error)
        {
            var request = new RestRequest("/_cluster/health", Method.GET);
            var response = _restClient.Execute(request);

            LogRequestResponse(request, response);

            error = string.Join(Environment.NewLine, new[] {
                "Server Response:", response.Content,
                "Error Message:", response.ErrorMessage });

            if (response.ErrorException != null)
            {
                _logger.Error(response.ErrorException);
                throw new InvalidOperationException("An error has occurred in Elasticsearch provider.", response.ErrorException);
            }

            return response.StatusCode == HttpStatusCode.OK;
        }

        public ElasticsearchResponse Index(IndexDocument document)
        {
            var request = new RestRequest($"/{_dataIndex}/_update/{((dynamic)document.Document).id}", Method.POST);

            var body = new
            {
                doc = GetDocumentWithSuggest(document),
                doc_as_upsert = true
            };

            request.AddJsonBody(JsonConvert.SerializeObject(body));
            var response = _restClient.Execute(request);

            LogRequestResponse(request, response);

            if (response.ErrorException != null)
            {
                _logger.Error(response.ErrorException);
                throw new InvalidOperationException("An error has occurred in Elasticsearch provider.", response.ErrorException);
            }

            return response.IsSuccessful
                ? new ElasticsearchResponse(1, 0)
                : new ElasticsearchResponse(response.ErrorMessage);
        }

        public ElasticsearchResponse BulkIndex(MessageTypeEnum messageType, List<IndexDocument> documents)
        {
            var request = new RestRequest("/_bulk", Method.POST);
            var docs = documents.Select(doc => GetDocumentWithSuggest(doc)).ToList();
            var body = CreateBulkBody(
                messageType == MessageTypeEnum.Users
                    ? _userIndex
                    : _dataIndex,
                docs);
            request.AddJsonBody(body);
            var response = _restClient.Execute(request);

            LogRequestResponse(request, response);

            if (response.ErrorException != null)
            {
                _logger.Error(response.ErrorException);
                throw new InvalidOperationException("An error has occurred in Elasticsearch provider.", response.ErrorException);
            }

            var model = JsonConvert.DeserializeObject<UpdateItemInfo>(response.Content);

            if (!response.IsSuccessful)
            {
                return new ElasticsearchResponse(response.ErrorMessage);
            }

            var result = new ElasticsearchResponse(model.SuccessNumber, model.FailedNumber)
            {
                HasErrors = model.HasErrors
            };
            
            foreach (var item in model.Items)
            {
                if (item.Info == null)
                    continue;
                var itemLog = new ProcessingItemLog()
                {
                    IsSucceeded = item.Info.IsSuccess,
                    ItemId = item.Info.Key.ToString(),
                    Result = item.Info.Result,
                    ErrorType = item.Info.Error?.Type,
                    ErrorReason = item.Info.Error?.Reason
                };
                result.Logs.Add(item.Info.Key, itemLog);
            }

            return result;
        }

        public bool Delete(Guid key)
        {
            var request = new RestRequest($"/{_dataIndex}/_doc/{key.ToString().ToUpper()}", Method.DELETE);
            var response = _restClient.Execute(request);

            LogRequestResponse(request, response);

            if (response.ErrorException != null)
            {
                _logger.Error(response.ErrorException);
                throw new InvalidOperationException("An error has occurred in Elasticsearch provider.", response.ErrorException);
            }

            return response.IsSuccessful;
        }

        private void CreateIndex(string index, Settings settings, List<Field> fields)
        {
            var request = new RestRequest($"/{index}", Method.PUT);
            var indexRequest = new CreateIndexRequest(settings);
            var builder = new MappingBuilder();
            builder.AddFields(fields);
            if (fields.Any(field => field.Suggestable))
            {
                var suggestableFields = fields.Where(field => field.Suggestable)
                    .Select(field => field.Name).Distinct().ToList();
                builder.AddSuggestFields(suggestableFields);
            }

            indexRequest.Mapping.Properties = builder.Properties;
            string body = JsonConvert.SerializeObject(indexRequest);
            request.AddJsonBody(body);
            var response = _restClient.Execute(request);

            LogRequestResponse(request, response);

            if (response.ErrorException != null)
            {
                _logger.Error(response.ErrorException);
                throw new InvalidOperationException("An error has occurred in Elasticsearch provider.", response.ErrorException);
            }
        }

        public bool CheckIndex(string index)
        {
            var request = new RestRequest($"/{index}", Method.GET);
            var response = _restClient.Execute(request);

            LogRequestResponse(request, response);

            if (response.ErrorException != null)
            {
                _logger.Error(response.ErrorException);
                throw new InvalidOperationException("An error has occurred in Elasticsearch provider.", response.ErrorException);
            }

            return response.StatusCode == HttpStatusCode.OK;
        }

        private string CreateBulkBody(string index, List<dynamic> documents)
        {
            var body = new StringBuilder();
            foreach (var document in documents)
            {
                if (document.entityDeleted)
                {
                    var delete = new
                    {
                        delete = new
                        {
                            _index = index,
                            _id = document.id
                        }
                    };
                    body.AppendLine(JsonConvert.SerializeObject(delete));
                }
                else
                {
                    var update = new
                    {
                        update = new
                        {
                            _index = index,
                            _id = document.id
                        }
                    };
                    body.AppendLine(JsonConvert.SerializeObject(update));

                    var data = new
                    {
                        doc = document,
                        doc_as_upsert = true
                    };
                    body.AppendLine(JsonConvert.SerializeObject(data));
                }
            }

            return body.ToString();
        }

        private dynamic GetDocumentWithSuggest(IndexDocument document)
        {
            var properties = document.Document as IDictionary<string, object>;
            foreach (var suggest in document.Suggests)
            {
                var field = new
                {
                    input = _suggestionsFactory.CreateSuggestions(suggest.Value)
                };
                properties.Add($"{suggest.Key}Suggest", field);
            }

            return properties;
        }

        private void LogRequestResponse(IRestRequest request, IRestResponse response)
        {
            var logMessage = new StringBuilder();
            logMessage.AppendLine($"Elasticsearch Request: [{request.Method} {request.Resource}]");

            if (request.Method == Method.POST || request.Method == Method.PUT)
            {
                logMessage.AppendLine($"{request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody)?.Value}");
            }

            logMessage.AppendLine($"Elasticsearch Response: [{response.StatusCode}]{Environment.NewLine}{response.Content}");

            _logger.Trace(logMessage);
        }
    }
}
