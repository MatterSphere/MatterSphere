using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{
    public class DataResponse<T>
    {
        [JsonProperty("data", Required = Required.Always)]
        public T Data { get; set; }
    }
}
