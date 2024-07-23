using System.Net.Http;
using iManageWork10.Shell.JsonResponses;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement
{
    public class UserManagement : RestApiManagement
    {
        public UserManagement(IRestApiClient restApiClient) :base(restApiClient)
        {}

        public CurrentUserProfile GetCurrentUserProfile(string library = null)
        {
            return _restApiClient.ExecuteRequest<DataResponse<CurrentUserProfile>>($"{GetRootUrl(library)}/me", HttpMethod.Get).Data;
        }

        protected override string GetRootUrl(string library = null)
        {
            return $"{base.GetRootUrl(library)}/users";
        }
    }
}