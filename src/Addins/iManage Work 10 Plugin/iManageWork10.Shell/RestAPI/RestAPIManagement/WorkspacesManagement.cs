using System.Collections.Generic;
using System.Net.Http;
using iManageWork10.Shell.JsonResponses;
using iManageWork10.Shell.RestAPI.RestAPIManagement.RequestProperties;

namespace iManageWork10.Shell.RestAPI.RestAPIManagement
{
    public class WorkspacesManagement : RestApiManagement
    {
        public WorkspacesManagement(IRestApiClient restApiClient) : base(restApiClient)
        { }

        public List<Workspace> SearchWorkspaces(SearchWorkspacesProperties searchWorkspacesProperties, string library = null)
        {
            var workspaces = new List<Workspace>();
            var advancedSearchWorkspaces = AdvancedSearchWorkspaces(new AdvancedSearchWorkspacesProperties(searchWorkspacesProperties));
            
            foreach (var advancedSearchWorkspace in advancedSearchWorkspaces)
            {
                workspaces.Add(GetWorkspace(advancedSearchWorkspace.Id, advancedSearchWorkspace.Database));
            }
            return workspaces;
        }

        public List<Workspace> AdvancedSearchWorkspaces(AdvancedSearchWorkspacesProperties advancedSearchWorkspacesProperties)
        {
            return _restApiClient.ExecuteRequest<WorkspaceDataResponse>("workspaces/search", HttpMethod.Post, advancedSearchWorkspacesProperties).Workspaces;
        }

        public Workspace GetWorkspace(string workspaceId, string library = null)
        {
            ValidateUriParameter(nameof(workspaceId), workspaceId);
            return _restApiClient.ExecuteRequest<DataResponse<Workspace>>($"{GetRootUrl(library)}/{workspaceId}", HttpMethod.Get).Data;
        }

        public WorkspaceSecurity GetWorkspaceUserSecurity(string workspaceId, string userId, string library = null)
        {
            ValidateUriParameter(nameof(workspaceId), workspaceId);
            ValidateUriParameter(nameof(userId), userId);
            return _restApiClient.ExecuteRequest<DataResponse<WorkspaceSecurity>>($"{GetRootUrl(library)}/{workspaceId}/users/{userId}/security", HttpMethod.Get).Data;
        }

        public List<Folder> SearchFolders(string workspaceId, SearchFoldersProperties searchFoldersProperties, string library = null)
        {
            ValidateUriParameter(nameof(workspaceId), workspaceId);
            var searchFoldersProperties2 = new SearchFoldersProperties2(searchFoldersProperties.Name, workspaceId);
            // {{urlPrefix}}/api/v2/customers/{{customerId}}/libraries/{{libraryId}}/folders/search
            return _restApiClient.ExecuteRequest<DataResponse<List<Folder>>>($"{base.GetRootUrl(library)}/folders/search", HttpMethod.Post, searchFoldersProperties2).Data;

        }

        public Workspace CreateWorkspace(Workspace workspace, string library = null)
        {
            return _restApiClient.ExecuteRequest<DataResponse<Workspace>>($"{GetRootUrl(library)}", HttpMethod.Post, workspace).Data;
        }

        protected override string GetRootUrl(string library = null)
        {
            return $"{base.GetRootUrl(library)}/workspaces";
        }
    }

}