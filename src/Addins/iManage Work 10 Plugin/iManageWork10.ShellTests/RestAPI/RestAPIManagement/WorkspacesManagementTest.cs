using System;
using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;
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
            var workspaceId = "1";
            var libraryId = string.IsNullOrEmpty(library)? LIBRARY : library;
            var searchWorkspacesProperties = new SearchWorkspacesProperties() {Libraries = libraryId};
            var advancedSearchWorkspacesProperties = new AdvancedSearchWorkspacesProperties(searchWorkspacesProperties);
            var workspace = new Workspace() { Id = workspaceId, Database = libraryId };

            var workspaceDataResponse = new WorkspaceDataResponse();
            workspaceDataResponse.Workspaces.Add(workspace);
            
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<Workspace>>(
                $"{GetExpectedRootUrl(library)}/{workspaceId}",
                HttpMethod.Get))
                .Return(new DataResponse<Workspace>());

            _restApiClient
                .Expect(client => client.ExecuteRequest<WorkspaceDataResponse>(
                    Arg<string>.Is.Equal($"workspaces/search"),
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Post),
                    Arg<object>.Is.Equal(advancedSearchWorkspacesProperties),
                    Arg<string>.Is.Null))
                .Return(workspaceDataResponse);

            _workspacesManagement.SearchWorkspaces(advancedSearchWorkspacesProperties, library);
            
            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        #region SearchFolders

        [Test]
        public void SearchFolders_WorkspaceIdNull_ArgumentNullExceptionThrown()
        {
            var searchFoldersProperties = new SearchFoldersProperties();
            var searchFoldersProperties2 = new SearchFoldersProperties2(searchFoldersProperties.Name, null);

            Assert.Throws<ArgumentNullException>(() => _workspacesManagement.SearchFolders(null, searchFoldersProperties2));
        }

        [Test]
        public void SearchFolders_WorkspaceIdEmpty_ArgumentExceptionThrown()
        {
            var searchFoldersProperties = new SearchFoldersProperties();
            var searchFoldersProperties2 = new SearchFoldersProperties2(searchFoldersProperties.Name, string.Empty);
            ArgumentException ex = Assert.Throws<ArgumentException>(() => _workspacesManagement.SearchFolders(string.Empty, searchFoldersProperties2));
            Assert.That(ex.Message, Is.EqualTo("Value cannot be empty.\r\nParameter name: workspaceId"));
            Assert.That(ex.ParamName, Is.EqualTo("workspaceId"));
        }

        [Test]
        [TestCase("ws", null)]
        [TestCase("ws", "")]
        [TestCase("1", "not_default_library")]
        public void SearchFolders_CorrectUrlBuilt(string workspaceId, string library)
        {
            var searchFoldersProperties = new SearchFoldersProperties();
            var searchFoldersProperties2 = new SearchFoldersProperties2(searchFoldersProperties.Name, workspaceId);

            if (string.IsNullOrEmpty(library))
            {
                _restApiClient.Expect(client => client.PreferredLibrary).Return(LIBRARY);
            }
            _restApiClient
                .Expect(client => client.ExecuteRequest<DataResponse<List<Folder>>>(
                    Arg<string>.Is.Equal($"{GetExpectedFolderRootUrl(library)}/folders/search"), 
                    Arg<HttpMethod>.Is.Equal(HttpMethod.Post), 
                    Arg<object>.Is.Equal(searchFoldersProperties2),
                    Arg<string>.Is.Null))
                .Return(new DataResponse<List<Folder>>());

            _workspacesManagement.SearchFolders(workspaceId, searchFoldersProperties2, library);

            _restApiClient.VerifyAllExpectations();
        }

        #endregion

        private string GetExpectedRootUrl(string library)
        {
            return $"libraries/{(string.IsNullOrEmpty(library) ? LIBRARY :library)}/workspaces";
        }

        private string GetExpectedFolderRootUrl(string library = null)
        {
            return $"libraries/{(string.IsNullOrEmpty(library) ? LIBRARY : library)}";
        }

    }
}
