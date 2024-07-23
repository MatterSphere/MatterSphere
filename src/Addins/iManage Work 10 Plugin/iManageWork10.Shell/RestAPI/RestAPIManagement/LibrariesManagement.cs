using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement
{
    public class LibrariesManagement : RestApiManagement
    {
        public LibrariesManagement(IRestApiClient restApiClient) : base(restApiClient)
        {
        }

        public List<Library> GetLibraries()
        {
            return _restApiClient.ExecuteRequest<DataResponse<List<Library>>>($"libraries", HttpMethod.Get).Data;
        }
    }
}
