using System;
using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.JsonResponses.AdvancedSearch;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;
using NUnit.Framework;
using Rhino.Mocks;

namespace iManageWork10.ShellTests.RestAPI.RestAPIManagement
{
    [TestFixture]
    public class WorkspacesManagementTest
    {

        private const string LIBRARY = "preflibrary";

        private IRestApiClient _restApiClient;

        private WorkspacesManagement _workspacesManagement;
        
        [SetUp]
        public void SetUp()
        {
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();
            _workspacesManagement = new WorkspacesManagement(_restApiClient);
        }

        #region GetWorkspace

        [Test]
        public void GetWorkspace_WorkspaceIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _workspacesManagement.GetWorkspace(null));
        }

        [Test]
        public void GetWorkspace_WorkspaceIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _workspacesManagement.GetWorkspace(string.Empty));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: workspaceId"));
            Assert.That(ex.ParamName, Is.EqualTo("workspaceId"));
        }

        [Test]
        [TestCase("ws", null)]
        [TestCase("ws", "")]
        [TestCase("1", "not_default_library")]
        public void GetWorkspace_CorrectUrlBuilt(string workspaceId, string library)
        {
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<Workspace>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{workspaceId}"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get),
                    Arg<object>.Is.Null,
                    Arg<string>.Is.Null))
                .Return(new DataResponse<Workspace>());

            _workspacesManagement.GetWorkspace(workspaceId, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region GetWorkspaceUserSecurity

        [Test]
        public void GetWorkspaceUserSecurity_WorkspaceIdNullOrUserIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _workspacesManagement.GetWorkspaceUserSecurity(null, "usrId"));
            Assert.Throws<ArgumentNullException>(() => _workspacesManagement.GetWorkspaceUserSecurity("wrkspcId", null));
            Assert.Throws<ArgumentNullException>(() => _workspacesManagement.GetWorkspaceUserSecurity(null, null));
        }

        [Test]
        public void GetWorkspaceUserSecurity_WorkspaceIdEmptyUserIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException exWorkspaceIdEmpty = Assert.Throws<ArgumentException>(() => _workspacesManagement.GetWorkspaceUserSecurity(string.Empty, "usrId"));
            Assert.That(exWorkspaceIdEmpty.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: workspaceId"));
            Assert.That(exWorkspaceIdEmpty.ParamName, Is.EqualTo("workspaceId"));

            ArgumentException exUserIdEmpty = Assert.Throws<ArgumentException>(() => _workspacesManagement.GetWorkspaceUserSecurity("workspaceId", string.Empty));
            Assert.That(exUserIdEmpty.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: userId"));
            Assert.That(exUserIdEmpty.ParamName, Is.EqualTo("userId"));
        }

        [Test]
        [TestCase("ws", "usr", "")]
        [TestCase("ws", "1", null)]
        [TestCase("1", "2", "not_default_library")]
        public void GetWorkspaceUserSecurity_CorrectUrlBuilt(string workspaceId, string userId, string library)
        {
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<WorkspaceSecurity>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{workspaceId}/users/{userId}/security"),
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get),
                    Arg<object>.Is.Null,
                    Arg<string>.Is.Null))
                .Return(new DataResponse<WorkspaceSecurity>());

            _workspacesManagement.GetWorkspaceUserSecurity(workspaceId, userId, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region SearchWorkspaces

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("not_default_library")]
        public void SearchWorkspaces_CorrectUrlBuilt(string library)
        {
            SearchWorkspacesProperties searchWorkspacesProperties = new SearchWorkspacesProperties();
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<List<Workspace>>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/search"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get), 
                    Arg<object>.Is.Equal(searchWorkspacesProperties),
                    Arg<string>.Is.Null))
                .Return(new DataResponse<List<Workspace>>());

            _workspacesManagement.SearchWorkspaces(searchWorkspacesProperties,  library);

            _restApiClient.VerifyAllExpectations();
        }

        [Test]
        [TestCase("")]
        [TestCase(null)]
        [TestCase("not_default_library")]
        public void SearchWorkspaces_AdvancedSearchCorrectUrlBuilt(string library)
        {
            SearchWorkspacesProperties searchWorkspacesProperties = new SearchWorkspacesProperties() {Libraries = "addLib1,addLib2"};
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<List<Workspace>>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/search"),
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get),
                    Arg<object>.Is.Equal(searchWorkspacesProperties),
                    Arg<string>.Is.Null))
                .Return(new DataResponse<List<Workspace>> {Data = new List<Workspace>()});
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<AdvancedSearchResult<AdvancedSearchWorkspaceData>>>(
                    Arg<string>.Is.Equal($"workspaces/search"),
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Post),
                    Arg<AdvancedSearchWorkspacesProperties>.Is.TypeOf,
                    Arg<string>.Is.Null))
                .Return(new DataResponse<AdvancedSearchResult<AdvancedSearchWorkspaceData>>() { Data =  new AdvancedSearchResult<AdvancedSearchWorkspaceData> {Results = new List<AdvancedSearchWorkspaceData>()} });
            _workspacesManagement.SearchWorkspaces(searchWorkspacesProperties, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region SearchFolders

        [Test]
        public void SearchFolders_WorkspaceIdNull_ArgumentNullExceptionThrown()
        {
            Assert.Throws<ArgumentNullException>(() => _workspacesManagement.SearchFolders(null, new SearchFoldersProperties()));
        }

        [Test]
        public void SearchFolders_WorkspaceIdEmpty_ArgumentExceptionThrown()
        {
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _workspacesManagement.SearchFolders(string.Empty, new SearchFoldersProperties()));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: workspaceId"));
            Assert.That(ex.ParamName, Is.EqualTo("workspaceId"));
        }

        [Test]
        [TestCase("ws", null)]
        [TestCase("ws", "")]
        [TestCase("1", "not_default_library")]
        public void SearchFolders_CorrectUrlBuilt(string workspaceId, string library)
        {
            SearchFoldersProperties searchFoldersProperties = new SearchFoldersProperties();
            
            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<List<Folder>>>(
                    Arg<string>.Is.Equal($"{GetExpectedRootUrl(library)}/{workspaceId}/folders/search"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Get), 
                    Arg<object>.Is.Equal(searchFoldersProperties),
                    Arg<string>.Is.Null))
                .Return(new DataResponse<List<Folder>>());

            _workspacesManagement.SearchFolders(workspaceId, searchFoldersProperties, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        private string GetExpectedRootUrl(string library)
        {
            return $"libraries/{(string.IsNullOrEmpty(library) ? LIBRARY :library)}/workspaces";
        }

    }
}
