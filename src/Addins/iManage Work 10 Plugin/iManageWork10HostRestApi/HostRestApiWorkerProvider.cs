using iManageWork10.Shell.RestAPI;

namespace iManageWork10HostRestApi
{
    public class HostRestApiWorkerProvider : RestApiWorkerProvider
    {
        private readonly string _serverUrl;
        private readonly string _clientId;
        private readonly string _extensionId;

        public HostRestApiWorkerProvider(string serverUrl, string clientId = null, string extensionId = null)
        {
            _serverUrl = serverUrl;
            _clientId = clientId;
            _extensionId = extensionId;
        }

        public override RestApiWorker GetRestApiWorker()
        {
            return new HostRestApiWorker(_serverUrl, _clientId, _extensionId);
        }
    }
}
