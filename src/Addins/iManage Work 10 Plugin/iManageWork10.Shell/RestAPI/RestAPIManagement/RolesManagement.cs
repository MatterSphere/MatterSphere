using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement
{
    public class RolesManagement : RestApiManagement
    {
        public RolesManagement(IRestApiClient restApiClient) : base(restApiClient)
        {}

        public List<Role> GetRoles(string library = null)
        {
            return _restApiClient.ExecuteRequest<DataResponse<List<Role>>>($"{GetRootUrl(library)}", HttpMethod.Get).Data;
        }

        protected override string GetRootUrl(string library = null)
        {
            return $"{base.GetRootUrl(library)}/roles";
        }
    }
}