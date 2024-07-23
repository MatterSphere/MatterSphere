using System.Collections.Generic;
using System.Linq;
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

        [SetUp]
        public void SetUp()
        {
            _searchProperties = new SearchWorkspacesProperties();
            _restApiClient = MockRepository.GenerateMock<IRestApiClient>();
            _command = new FindWorkspaceCommand(_searchProperties);
        }

        [Test]
        public void Execute_WorkspaceNotFound_ThrowsWorkspaceNotFoundException()
        {
            var dataResponse = new DataResponse<List<Workspace>>()
            {
                Data = new List<Workspace>()
            };
            _restApiClient.Expect(c => c.PreferredLibrary).Return(PREF_LIB);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<List<Workspace>>>($"libraries/{PREF_LIB}/workspaces/search", HttpMethod.Get, _searchProperties)).Return(dataResponse);

            Assert.Throws<WorkspaceNotFoundException>(() => _command.Execute(_restApiClient));
        }

        [Test]
        [TestCase(AccessLevel.Read)]
        [TestCase(AccessLevel.ReadWrite)]
        [TestCase(AccessLevel.FullAccess)]
        public void Execute_WorkspaceWithAppropriateFixFound_ReturnFoundWorkspace(AccessLevel accessLevel)
        {
            var dataResponse = GetWorkspacesDataResponse("workspaceId");
            var currentUserResponse = GetCurrentUserProfileResponse("1");
            var securityResponse = GetWorkspaceSecurityResponse(accessLevel);
            var expectedWorkspace = dataResponse.Data.First();
            var currentUser = currentUserResponse.Data;

            _restApiClient.Expect(c => c.PreferredLibrary).Return(PREF_LIB);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<List<Workspace>>>($"libraries/{PREF_LIB}/workspaces/search", HttpMethod.Get, _searchProperties)).Return(dataResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<CurrentUserProfile>>($"libraries/{PREF_LIB}/users/me", HttpMethod.Get)).Return(currentUserResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<WorkspaceSecurity>>($"libraries/{PREF_LIB}/workspaces/{expectedWorkspace.Id}/users/{currentUser.Id}/security", HttpMethod.Get)).Return(securityResponse);

            var actualWorkspace = _command.Execute(_restApiClient);

            Assert.AreSame(expectedWorkspace, actualWorkspace);
        }

        [Test]
        [TestCase(AccessLevel.NoAccess)]
        [TestCase(AccessLevel.ChangeSecurity)]
        [TestCase(AccessLevel.Unknown)]
        public void Execute_WorkspaceFoundWithNotSuitableAccessLevel_ThrowsWorkspaceNotFoundException(AccessLevel accessLevel)
        {
            var dataResponse = GetWorkspacesDataResponse("workspaceId");
            var currentUserResponse = GetCurrentUserProfileResponse("1");
            var securityResponse = GetWorkspaceSecurityResponse(accessLevel);
            var workspace = dataResponse.Data.First();
            var currentUser = currentUserResponse.Data;

            _restApiClient.Expect(c => c.PreferredLibrary).Return(PREF_LIB);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<List<Workspace>>>($"libraries/{PREF_LIB}/workspaces/search", HttpMethod.Get, _searchProperties)).Return(dataResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<CurrentUserProfile>>($"libraries/{PREF_LIB}/users/me", HttpMethod.Get)).Return(currentUserResponse);
            _restApiClient.Expect(c => c.ExecuteRequest<DataResponse<WorkspaceSecurity>>($"libraries/{PREF_LIB}/workspaces/{workspace.Id}/users/{currentUser.Id}/security", HttpMethod.Get)).Return(securityResponse);

            Assert.Throws<WorkspaceNotFoundException>(() => _command.Execute(_restApiClient));
        }

        private DataResponse<List<Workspace>> GetWorkspacesDataResponse(string workspaceId)
        {
            return new DataResponse<List<Workspace>>()
            {
                Data = new List<Workspace>()
                {
                    new Workspace()
                    {
                        Id = workspaceId,
                        Database = PREF_LIB
                    }
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
