using iManageWork10.Shell.JsonResponses;

namespace iManageWork10.Integration.Util
{
    public static class UriComposer
    {
        public static string ComposeUserBaseUrl(string baseUrl)
        {
            return $"{baseUrl.TrimEnd('/')}/work/web";
        }

        public static string ComposeWorkspaceUrl(string baseUrl, Workspace workspace)
        {
            return $"{baseUrl.TrimEnd('/')}/work/web/r/libraries/{workspace.Database}/workspaces/{workspace.Id}";
        }

        public static string ComposeClientUrl(string baseUrl, string clientNumber, Workspace workspace)
        {
            return $"{baseUrl.TrimEnd('/')}/work/web/r/{IntegrationUtil.GetClientNumberMappingEntity()}/{clientNumber}?exclude_email=true&scope={workspace.Database}";
        }
    }
}
