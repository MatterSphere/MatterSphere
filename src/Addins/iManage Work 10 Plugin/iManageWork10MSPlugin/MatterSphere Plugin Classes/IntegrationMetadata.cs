using System.Text;
using iManageWork10.Shell.JsonResponses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MatterSphereIntercept
{
    public class IntegrationMetadata
    {
        private readonly string[] _mergedData;

        public IntegrationMetadata(string[] mergedData)
        {
            _mergedData = mergedData;
        }

        private string _clientNumber;

        public string ClientNumber
        {
            get
            {
                if (string.IsNullOrEmpty(_clientNumber))
                {
                    _clientNumber = GetFieldValue("CUSTOM1");
                }
                return _clientNumber;
            }
        }

        private string _matterNumber;

        public string MatterNumber
        {
            get
            {
                if (string.IsNullOrEmpty(_matterNumber))
                {
                    _matterNumber = GetFieldValue("CUSTOM2");
                }
                return _matterNumber;
            }
        }

        private string _documentFolder;

        public string DocumentFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_documentFolder))
                {
                    _documentFolder = GetFieldValue("DOCUMENTFOLDER");
                }
                return string.IsNullOrEmpty(_documentFolder) ? "Documents" : _documentFolder;
            }
        }

        private string _documentDescription;

        public string DocumentDescription
        {
            get
            {
                if (string.IsNullOrEmpty(_documentDescription))
                {
                    _documentDescription = GetFieldValue("DOCUMENTDESCRIPTION");
                }
                return _documentDescription;
            }
        }
        
        public string GetFormattedProfile()
        {
            return JsonConvert.SerializeObject(GetDocumentProfile(), new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }

        public DocumentProfile GetDocumentProfile()
        {
            return JsonConvert.DeserializeObject<DocumentProfile>(GetFormattedMetadata(), new JsonSerializerSettings()
            {
                ContractResolver = new DocumentMetadataContractResolver()
            });
        }

        private string GetFormattedMetadata()
        {
            StringBuilder stringBuilder = new StringBuilder("{");
            foreach (var value in _mergedData)
            {
                if (value.Contains(":"))
                {
                    int colonIndex = value.IndexOf(':');
                    if (stringBuilder.Length > 1)
                    {
                        stringBuilder.Append(",");
                    }

                    var fieldName = value.Substring(0, colonIndex).ToLower();
                    stringBuilder.Append($"\"{fieldName}\":\"{value.Substring(colonIndex + 1)}\"");
                }
            }

            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }

        private string GetFieldValue(string fieldName)
        {
            foreach (var value in _mergedData)
            {
                if (value.ToUpper().StartsWith($"{fieldName.ToUpper()}:"))
                {
                    int colonIndex = value.IndexOf(':');
                    return value.Substring(colonIndex + 1);
                }
            }
            return string.Empty;
        }
    }

    class DocumentMetadataContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string name)
        {
            switch (name)
            {
                case "extension":
                    return "fileextension";
                case "name":
                    return "documentdescription";
                case "class":
                case "comment":
                case "type":
                case "author":
                    return $"document{name}";
                default:
                    return base.ResolvePropertyName(name);
            }
        }
    }
}
