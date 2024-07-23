using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{
    public class HttpError
    {
        [JsonProperty("error")]
        public HttpErrorData Error { get; set; }

        public override string ToString()
        {
            return $"[Error: {Error}]";
        }
    }

    public class HttpErrorData
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("code_message")]
        public string CodeMessage { get; set; }

        public override string ToString()
        {
            return $"[Code: {Code}, CodeMessage: {CodeMessage}]";
        }
    }
}