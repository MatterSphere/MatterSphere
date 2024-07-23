using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;
using NUnit.Framework;
using Rhino.Mocks;

namespace iManageWork10.ShellTests.RestAPI.RestAPIManagement
{
    class LibrariesManagementTest
    {
        private IRestApiClient _restApiClient;

        private LibrariesManagement _librariesManagement;

        [SetUp]
        public void SetUp()
        {
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();
            _librariesManagement = new LibrariesManagement(_restApiClient);
        }

        [Test]
        public void GetLibraries_CorrectUrlBuilt()
        {
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<List<Library>>>(
                    Arg<string>.Is.Equal("libraries"),
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get),
                    Arg<object>.Is.Null,
                    Arg<string>.Is.Null))
                .Return(new DataResponse<List<Library>>());

            _librariesManagement.GetLibraries();

            _restApiClient.VerifyAllExpectations();
        }
    }
}
