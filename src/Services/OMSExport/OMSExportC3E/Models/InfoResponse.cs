using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class InfoResponse : BaseResponse
    {
        [JsonProperty("api")]
        public InfoResponseApi Api { get; set; }

        [JsonProperty("environment")]
        public object Environment { get; set; }

        [JsonProperty("framework")]
        public InfoResponseFramework Framework { get; set; }

        [JsonProperty("hosting")]
        public InfoResponseHosting Hosting { get; set; }

        [JsonProperty("system")]
        public InfoResponseSystem System { get; set; }

        [JsonProperty("webApplication")]
        public InfoResponseWebApplication WebApplication { get; set; }

        public class InfoResponseApi
        {
            [JsonProperty("version")]
            public string Version { get; set; }
        }

        public class InfoResponseFramework
        {
            [JsonProperty("buildId")]
            public string BuildId { get; set; }

            [JsonProperty("baseUri")]
            public string BaseUri { get; set; }

            [JsonProperty("version")]
            public string Version { get; set; }
        }

        public class InfoResponseHosting
        {
            [JsonProperty("hostType")]
            public string HostType { get; set; }

            [JsonProperty("authenticationType")]
            public string AuthenticationType { get; set; }

            [JsonProperty("authority")]
            public string Authority { get; set; }
        }

        public class InfoResponseSystem
        {
            [JsonProperty("timeZone")]
            public string TimeZone { get; set; }

            [JsonProperty("language")]
            public string Language { get; set; }
        }

        public class InfoResponseWebApplication
        {
            [JsonProperty("ciam")]
            public InfoResponseCiam Ciam { get; set; }

            [JsonProperty("aad")]
            public InfoResponseAad Aad { get; set; }

            [JsonProperty("autoSignoutInterval")]
            public string AutoSignoutInterval { get; set; }

            public class InfoResponseCiam
            {
                [JsonProperty("authority")]
                public string Authority { get; set; }

                [JsonProperty("clientId")]
                public string ClientId { get; set; }

                [JsonProperty("audience")]
                public string Audience { get; set; }

                [JsonProperty("scope")]
                public string Scope { get; set; }

                [JsonProperty("loginHint")]
                public string LoginHint { get; set; }

                [JsonProperty("connection")]
                public string Connection { get; set; }

                [JsonProperty("maxAge")]
                public string MaxAge { get; set; }
            }

            public class InfoResponseAad
            {
                [JsonProperty("clientId")]
                public string ClientId { get; set; }

                [JsonProperty("tenant")]
                public string Tenant { get; set; }

                [JsonProperty("pamTenant")]
                public string PamTenant { get; set; }

                [JsonProperty("instance")]
                public string Instance { get; set; }

                [JsonProperty("scope")]
                public string Scope { get; set; }

                [JsonProperty("maxAge")]
                public string MaxAge { get; set; }
            }
        }
    }
}
