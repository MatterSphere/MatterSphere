using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RestSharp;

namespace Elasticsearch.Tests.IntegrationTests.Common
{
    public class IndexBuilder
    {
        private RestClient _client;
        private string _dataIndex;
        private string _userIndex;

        public IndexBuilder(RestClient restClient, string dataIndex, string userIndex)
        {
            _client = restClient;
            _dataIndex = dataIndex;
            _userIndex = userIndex;
        }

        #region Public methods

        public void CreateDataIndex()
        {
            DeleteIndex(_dataIndex);

            var request = new RestRequest($"/{_dataIndex}", Method.PUT);
            var body = new IndexStructure.IndexRequest();
            
            var defaultFields = IndexStructureHelper.GetDefaultDataFields();
            var properties = CreateMappingProperties(defaultFields);
            body.Mapping.Properties = properties;
            request.AddJsonBody(JsonConvert.SerializeObject(body));
            var response = _client.Execute(request);
        }

        public void AddClient(ClientData client)
        {
            var request = new RestRequest($"/{_dataIndex}/_doc/{client.Key}", Method.PUT);
            var body = new
            {
                id = client.Key,
                mattersphereid = client.Id,
                objecttype = "CLIENT",
                name = client.Name,
                clientNum = client.Number,
                clientType = client.ClientType,
                clientNotes = client.ClientNotes,
                ugdp = client.UGDP,
                clientNumSuggest = new
                {
                    input = GetElements(client.Number)
                },
                NameSuggest = new
                {
                    input = GetElements(client.Name)
                }
            };

            request.AddJsonBody(JsonConvert.SerializeObject(body));
            var response = _client.Execute(request);
        }

        public void AddFile(FileData file, ClientData client)
        {
            var request = new RestRequest($"/{_dataIndex}/_doc/{file.Key}", Method.PUT);
            var body = new
            {
                id = file.Key,
                mattersphereid = file.Id,
                objecttype = "FILE",
                clientId = client.Id,
                fileStatus = file.FileStatus,
                fileType = file.FileType,
                fileDesc = file.Description,
                Name = client.Name,
                clientNum = client.Number,
                clientType = client.ClientType,
                clientNotes = client.ClientNotes,
                ugdp = file.UGDP,
                clientNumSuggest = new
                {
                    input = GetElements(client.Number)
                },
                NameSuggest = new
                {
                    input = GetElements(client.Name)
                },
                fileDescSuggest = new
                {
                    input = GetElements(file.Description)
                }
            };

            request.AddJsonBody(JsonConvert.SerializeObject(body));
            var response = _client.Execute(request);
        }

        public void CreateUserIndex()
        {
            DeleteIndex(_userIndex);

            var request = new RestRequest($"/{_userIndex}", Method.PUT);
            var body = new IndexStructure.IndexRequest();

            var defaultFields = IndexStructureHelper.GetDefaultUserFields();
            var properties = CreateMappingProperties(defaultFields);
            body.Mapping.Properties = properties;
            request.AddJsonBody(JsonConvert.SerializeObject(body));
            var response = _client.Execute(request);
        }

        public void AddUser(UserData user)
        {
            var request = new RestRequest($"/{_userIndex}/_doc/{user.Key}", Method.PUT);
            var body = new
            {
                id = user.Key,
                mattersphereid = user.Id,
                objecttype = "users",
                usrad = user.AdName,
                usrsql = user.SqlName,
                usrAccessList = user.AccessList
            };

            request.AddJsonBody(JsonConvert.SerializeObject(body));
            var response = _client.Execute(request);
        }

        #endregion

        #region Private methods
        private void DeleteIndex(string name)
        {
            var request = new RestRequest($"/{name}", Method.DELETE);
            var response = _client.Execute(request);
        }

        private dynamic CreateMappingProperties(List<IndexField> fields)
        {
            dynamic properties = new ExpandoObject();
            var mappingProperties = properties as IDictionary<string, object>;
            foreach (var field in fields)
            {
                var mappingProperty = CreateIndexField(field);
                if (mappingProperties.ContainsKey(field.Name))
                {
                    mappingProperties[field.Name] = mappingProperty;
                }
                else
                {
                    mappingProperties.Add(field.Name, mappingProperty);
                }
            }

            var suggestNames = fields.Where(field => field.Suggestable)
                .Select(field => $"{field.Name}Suggest").ToList();
            foreach (var suggestName in suggestNames)
            {
                var suggest = CreateSuggest();
                if (mappingProperties.ContainsKey(suggestName))
                {
                    mappingProperties[suggestName] = suggest;
                }
                else
                {
                    mappingProperties.Add(suggestName, suggest);
                }
            }

            return properties;
        }

        private dynamic CreateIndexField(IndexField fld)
        {
            dynamic field = new ExpandoObject();
            field.type = fld.Type;

            if (fld.Facetable && fld.Type == "text")
            {
                field.fielddata = true;
            }

            if (!string.IsNullOrWhiteSpace(fld.Analyzer))
            {
                field.analyzer = fld.Analyzer;
            }

            if (fld.Facetable)
            {
                field.fields = new
                {
                    keyword = new
                    {
                        type = "keyword"
                    }
                };
            }

            return field;
        }

        private dynamic CreateSuggest()
        {
            dynamic suggest = new ExpandoObject();
            suggest.type = "completion";
            suggest.analyzer = "simple";
            suggest.search_analyzer = "simple";
            suggest.preserve_separators = true;
            suggest.preserve_position_increments = true;

            return suggest;
        }

        private string[] GetElements(string value)
        {
            var regex = new Regex("[ ]{2,}", RegexOptions.None);
            value = regex.Replace(value, " ");

            return value.Split(' ');
        }
        #endregion

        #region Classes

        public class ClientData
        {
            public ClientData(string key, long id, string name, string number, string clientType, string ugdp)
            {
                Key = key;
                Id = id;
                Name = name;
                Number = number;
                ClientType = clientType;
                UGDP = ugdp;
            }

            public string Key { get; set; }
            public long Id { get; set; }
            public string Name { get; set; }
            public string Number { get; set; }
            public string ClientType { get; set; }
            public string ClientNotes { get; set; }
            public string UGDP { get; set; }
        }

        public class FileData
        {
            public FileData(string key, long id, string description, string fileStatus, string fileType, string ugdp)
            {
                Key = key;
                Id = id;
                Description = description;
                FileStatus = fileStatus;
                FileType = fileType;
                UGDP = ugdp;
            }

            public string Key { get; set; }
            public long Id { get; set; }
            public string Description { get; set; }
            public string FileStatus { get; set; }
            public string FileType { get; set; }
            public string UGDP { get; set; }
        }

        public class UserData
        {
            public UserData(string key, long id, string adName, string sqlName, string accessList)
            {
                Key = key;
                Id = id;
                AdName = adName;
                SqlName = sqlName;
                AccessList = accessList;
            }

            public string Key { get; set; }
            public long Id { get; set; }
            public string AdName { get; set; }
            public string SqlName { get; set; }
            public string AccessList { get; set; }
        }

        #endregion
    }
}