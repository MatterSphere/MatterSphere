using System;
using System.Collections.Generic;
using System.Linq;
using iManageWork10.Shell.Exceptions;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.JsonResponses.Enums;
using iManageWork10.Shell.RestAPI;
using iManageWork10.Shell.RestAPI.RestAPIManagement;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;
using iManageWork10.Shell.Validators;

namespace iManageWork10.Shell.Commands
{
    public class FindWorkspaceCommand : IManageCommand<Workspace>
    {
        private readonly SearchWorkspacesProperties _searchWorkspacesProperties;

        public FindWorkspaceCommand(SearchWorkspacesProperties searchWorkspacesProperties)
        {
            _searchWorkspacesProperties = searchWorkspacesProperties;
        }

        public Workspace Execute(IRestApiClient restApiClient)
        {
            var workspacesManagement = new WorkspacesManagement(restApiClient);

            var libraries = _searchWorkspacesProperties.Libraries;
            if (!string.IsNullOrEmpty(libraries))
            {
                var validator = new LibrariesValidator(restApiClient);
                try
                {
                    validator.Validate(libraries);
                }
                catch (ArgumentException)
                {
                    _searchWorkspacesProperties.Libraries = null;
                }
            }
            List<Workspace> workspaces = workspacesManagement.SearchWorkspaces(_searchWorkspacesProperties);
            if (workspaces.Any())
            {
                var workspace = workspaces.First();
                var userManagement = new UserManagement(restApiClient);
                var currentUser = userManagement.GetCurrentUserProfile();
                var workspaceAccess = workspacesManagement.GetWorkspaceUserSecurity(workspace.Id, currentUser.Id, workspace.Database).Access;
                if (workspaceAccess == AccessLevel.FullAccess || workspaceAccess == AccessLevel.Read || workspaceAccess == AccessLevel.ReadWrite)
                {
                    return workspace;
                }
            }

            throw new WorkspaceNotFoundException();
        }
    }
}
