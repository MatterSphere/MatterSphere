using iManageWork10.Shell.RestAPI;

namespace iManageWork10.Shell.Commands
{
    public class CommandsExecuter
    {
        public CommandsExecuter(IRestApiClient restApiClient)
        {
            RestApiClient = restApiClient;
        }

        public IRestApiClient RestApiClient { get; set; }

        public T Execute<T>(IManageCommand<T> command)
        {
            return command.Execute(RestApiClient);
        }
    }
}
