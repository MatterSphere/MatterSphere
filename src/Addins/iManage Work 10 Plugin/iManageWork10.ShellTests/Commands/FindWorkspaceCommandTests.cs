using System.Net.Http;
using iManageWork10.Shell.Commands;
using iManageWork10.Shell.Exceptions;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.JsonResponses.Enums;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;
using NUnit.Framework;
using Rhino.Mocks;

namespace iManageWork10.ShellTests.Commands
{
    [TestFixture]
    public class FindWorkspaceCommandTests
    {
        private const string PREF_LIB = "preflib";
        private FindWorkspaceCommand _command;
        private SearchWorkspacesProperties _searchProperties;
        private IRestApiClient _restApiClient;
        private AdvancedSearchWorkspacesProperties _advancedSearchWorkspacesProperties;

        [SetUp]
        public void SetUp()
        {
            _searchProperties = new SearchWorkspacesProperties();
            _advancedSearchWorkspacesProperties = new AdvancedSearchWorkspacesProperties(_searchProperties);
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();
            _command = new FindWorkspaceCommand(_advancedSearchWorkspacesProperties);
        }

        [Test]
        public void Execute_WorkspaceNotFound_ThrowsWorkspaceNotFoundException()
        {
            var workspaceDataResponse = new WorkspaceDataResponse();
            _restApiClient.Expect(c => c.PreferredLibrary).Return(PREF_LIB);
            _restApiClient.Expect(client => client.ExecuteRequest<WorkspaceDataResponse>("workspaces/search", HttpMethod.Post, _advancedSearchWorkspacesProperties)).Return(workspaceDataResponse);

            Assert.Throws<WorkspaceNotFoundException>(() => _command.Execute(_restApiClient));
        }

        [Test]
        [TestCase(AccessLevel.Read)]
        [TestCase(AccessLevel.ReadWrite)]
        [TestCase(AccessLevel.FullAccess)]
        public void Execute_WorkspaceWithAppropriateAccessFound_ReturnFoundWorkspace(AccessLevel accessLevel)
        {
            var dataResponse = GetWorkspaceDataResponse("workspaceId");
            var currentUserResponse = GetCurrentUserProfileResponse("1");
            var securityResponse = GetWorkspaceSecurityResponse(accessLevel);
            var expectedWorkspace = dataResponse.Data;
            var currentUser = currentUserResponse.Data;
            var workspaceDataResponse = new WorkspaceDataResponse();
            workspaceDataResponse.Workspaces.Add(expectedWorkspace);

            _restApiClient.Expect(c => c.PreferredLibrary).Return(PREF_LIB);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<Workspace>>($"libraries/{PREF_LIB}/workspaces/{expectedWorkspace.Id}", HttpMethod.Get)).Return(dataResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<CurrentUserProfile>>($"libraries/{PREF_LIB}/users/me", HttpMethod.Get)).Return(currentUserResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<WorkspaceSecurity>>($"libraries/{PREF_LIB}/workspaces/{expectedWorkspace.Id}/users/{currentUser.Id}/security", HttpMethod.Get)).Return(securityResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<WorkspaceDataResponse>("workspaces/search", HttpMethod.Post, _advancedSearchWorkspacesProperties)).Return(workspaceDataResponse);

            var actualWorkspace = _command.Execute(_restApiClient);

            Assert.AreSame(expectedWorkspace, actualWorkspace);
        }

        [Test]
        [TestCase(AccessLevel.NoAccess)]
        [TestCase(AccessLevel.ChangeSecurity)]
        [TestCase(AccessLevel.Unknown)]
        public void Execute_WorkspaceFoundWithNotSuitableAccessLevel_ThrowsWorkspaceNotFoundException(AccessLevel accessLevel)
        {
            var dataResponse = GetWorkspaceDataResponse("workspaceId");
            var currentUserResponse = GetCurrentUserProfileResponse("1");
            var securityResponse = GetWorkspaceSecurityResponse(accessLevel);
            var workspace = dataResponse.Data;
            var currentUser = currentUserResponse.Data;
            var workspaceDataResponse = new WorkspaceDataResponse();
            workspaceDataResponse.Workspaces.Add(workspace);

            _restApiClient.Expect(c => c.PreferredLibrary).Return(PREF_LIB);
            _restApiClient.Expect(c => c.ExecuteRequest<WorkspaceDataResponse>("workspaces/search", HttpMethod.Post, _advancedSearchWorkspacesProperties)).Return(workspaceDataResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<CurrentUserProfile>>($"libraries/{PREF_LIB}/users/me", HttpMethod.Get)).Return(currentUserResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<WorkspaceSecurity>>($"libraries/{PREF_LIB}/workspaces/{workspace.Id}/users/{currentUser.Id}/security", HttpMethod.Get)).Return(securityResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<Workspace>>($"libraries/{PREF_LIB}/workspaces/{workspace.Id}", HttpMethod.Get)).Return(dataResponse);

            Assert.Throws<WorkspaceNotFoundException>(() => _command.Execute(_restApiClient));
        }

        private DataResponse<Workspace> GetWorkspaceDataResponse(string workspaceId)
        {
            return new DataResponse<Workspace>()
            {
                Data = new Workspace()
                { 
                    Id = workspaceId, 
                    Database = PREF_LIB 
                }
            };
        }

        private DataResponse<CurrentUserProfile> GetCurrentUserProfileResponse(string userId)
        {
            return new DataResponse<CurrentUserProfile>()
            {
                Data = new CurrentUserProfile()
                {
                    Id = userId
                }
            };
        }

        private DataResponse<WorkspaceSecurity> GetWorkspaceSecurityResponse(AccessLevel accessLevel)
        {
            return new DataResponse<WorkspaceSecurity>()
            {
                Data = new WorkspaceSecurity()
                {
                    Access = accessLevel
                }
            };
        }
    }
}
