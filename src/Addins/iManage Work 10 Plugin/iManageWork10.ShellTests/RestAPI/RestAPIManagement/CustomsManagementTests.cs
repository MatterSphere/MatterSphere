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
    public class CustomsManagementTests
    {
        private const string PREFERRED_LIBRARY = "preferredLibrary";
        private CustomsManagement _customsManagement;
        private IRestApiClient _restApiClient;

        [SetUp]
        public void SetUp()
        {
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();
            _restApiClient.Stub(c => c.PreferredLibrary).Return(PREFERRED_LIBRARY);
            _customsManagement = new CustomsManagement(_restApiClient);
        }

        [Test]
        [TestCase("F1")]
        [TestCase("F5")]
        public void GetCustom_ExecuteRestApiClientOnProperUriAndHttpMethod(string value)
        {
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<List<Custom>>>($"libraries/{PREFERRED_LIBRARY}/customs/custom1/search?alias={value}", HttpMethod.Get)).Return(new DataResponse<List<Custom>>());

            _customsManagement.GetCustom("custom1",value);

            _restApiClient.VerifyAllExpectations();
        }

        [Test]
        public void GetCustom_ReturnsResponseDataFromRestApiClient()
        {
            var customList = new List<Custom>() { new Custom() };
            var response = new DataResponse<List<Custom>>() { Data = customList };
            _restApiClient.Stub(c => c.ExecuteRequest<DataResponse<List<Custom>>>($"libraries/{PREFERRED_LIBRARY}/customs/custom1/search?alias=F1", HttpMethod.Get)).Return(response);

            var result = _customsManagement.GetCustom("custom1", "F1");

            Assert.AreSame(result, customList);
        }

        [Test]
        public void GetChildCustom_ExecuteRestApiClientOnProperUriAndHttpMethod()
        {
            var customValue = "1";
            var parentValue = "F1";
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<List<Custom>>>($"libraries/{PREFERRED_LIBRARY}/customs/custom2/search?alias={customValue}&parent_alias={parentValue}", HttpMethod.Get)).Return(new DataResponse<List<Custom>>());

            _customsManagement.GetChildCustom("custom2", customValue, parentValue);

            _restApiClient.VerifyAllExpectations();
        }

        [Test]
        public void GetChildCustom_ReturnsResponseDataFromRestApiClient()
        {
            var customValue = "1";
            var parentValue = "F1";
            var customList = new List<Custom>() { new Custom() };
            var response = new DataResponse<List<Custom>>() { Data = customList };
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<List<Custom>>>($"libraries/{PREFERRED_LIBRARY}/customs/custom2/search?alias={customValue}&parent_alias={parentValue}", HttpMethod.Get)).Return(response);

            var result = _customsManagement.GetChildCustom("custom2", customValue, parentValue);

            Assert.AreSame(result, customList);
        }

        [Test]
        public void CreateCustom_ExecuteRestApiClientOnProperUriAndHttpMethod()
        {
            var custom = new Custom();
            _restApiClient.Expect(c => c.ExecuteRequest<object>($"libraries/{PREFERRED_LIBRARY}/customs/custom1", HttpMethod.Post, custom));

            _customsManagement.CreateCustom("custom1", custom);

            _restApiClient.VerifyAllExpectations();
        }

    }
}
