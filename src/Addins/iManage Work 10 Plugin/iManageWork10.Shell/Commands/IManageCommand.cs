using iManageWork10.Shell.RestAPI;

namespace iManageWork10.Shell.Commands
{
    public interface IManageCommand<T>
    {
        T Execute(IRestApiClient restApiClient);
    }
}
