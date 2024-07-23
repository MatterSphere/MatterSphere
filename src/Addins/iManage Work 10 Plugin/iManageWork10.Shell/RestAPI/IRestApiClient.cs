using System.Net.Http;

namespace iManageWork10.Shell.RestAPI
{
    public interface IRestApiClient
    {
        bool Connect();
        bool Disconnect();
        string AuthToken { get; }
        string PreferredLibrary { get; }
        T ExecuteRequest<T>(string url, HttpMethod httpMethod, object payload = null, string localFilePath = null);
    }
}