using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;
using NUnit.Framework;
using Rhino.Mocks;

namespace iManageWork10.ShellTests.RestAPI.RestAPIManagement
{
    [TestFixture]
    class RolesManagementTest
    {
        private const string LIBRARY = "preflibrary";

        private IRestApiClient _restApiClient;

        private RolesManagement _rolesManagement;

        [SetUp]
        public void SetUp()
        {
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();
            _rolesManagement = new RolesManagement(_restApiClient);
        }

        #region GetRoles

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("not_default_library")]
        public void GetRoles_CorrectUrlBuilt(string library)
        {

            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<List<Role>>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get),
                    Arg<object>.Is.Null,
                    Arg<string>.Is.Null))
                .Return(new DataResponse<List<Role>>());

            _rolesManagement.GetRoles(library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        private string GetExpectedRootUrl(string library)
        {
            return $"libraries/{(string.IsNullOrEmpty(library) ? LIBRARY : library)}/roles";
        }
    }
}
