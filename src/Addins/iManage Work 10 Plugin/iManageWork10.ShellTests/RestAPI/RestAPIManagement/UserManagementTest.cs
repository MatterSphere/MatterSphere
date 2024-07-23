using System.Net.Http;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;
using NUnit.Framework;
using Rhino.Mocks;

namespace iManageWork10.ShellTests.RestAPI.RestAPIManagement
{
    class UserManagementTest
    {
        private const string LIBRARY = "preflibrary";

        private IRestApiClient _restApiClient;

        private UserManagement _userManagement;

        [SetUp]
        public void SetUp()
        {
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();
            _userManagement = new UserManagement(_restApiClient);
        }

        #region GetCurrentUserProfile

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("not_default_library")]
        public void GetCurrentUserProfile_CorrectUrlBuilt(string library)
        {

            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<CurrentUserProfile>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/me"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get),
                    Arg<object>.Is.Null,
                    Arg<string>.Is.Null))
                .Return(new DataResponse<CurrentUserProfile>());

            _userManagement.GetCurrentUserProfile(library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        private string GetExpectedRootUrl(string library)
        {
            return $"libraries/{(string.IsNullOrEmpty(library) ? LIBRARY : library)}/users";
        }
    }
}
