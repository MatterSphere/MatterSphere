using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement
{
    class CustomsManagement : RestApiManagement
    {
        public CustomsManagement(IRestApiClient restApiClient) : base(restApiClient)
        { }

        public List<Custom> GetCustom(string customName, string customValue, string library = null)
        {
            ValidateUriParameter(nameof(customName), customName);
            return _restApiClient.ExecuteRequest<DataResponse<List<Custom>>>($"{GetRootUrl(library)}/{customName}/search?alias={customValue}", HttpMethod.Get).Data;
        }

        public List<Custom> GetChildCustom(string customName, string customValue, string parentCustomId, string library = null)
        {
            ValidateUriParameter(nameof(customName), customName);
            ValidateUriParameter(nameof(parentCustomId), parentCustomId);
            return _restApiClient.ExecuteRequest<DataResponse<List<Custom>>>($"{GetRootUrl(library)}/{customName}/search?alias={customValue}&parent_alias={parentCustomId}", HttpMethod.Get).Data;
        }

        public void CreateCustom(string customName, Custom custom, string library = null)
        {
            ValidateUriParameter(nameof(customName), customName);
            _restApiClient.ExecuteRequest<object>($"{GetRootUrl(library)}/{customName}", HttpMethod.Post, custom);
        }
        
        protected override string GetRootUrl(string library = null)
        {
            return $"{base.GetRootUrl(library)}/customs";
        }
    }
}
