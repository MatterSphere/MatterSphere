using Newtonsoft.Json;

namespace FWBS.OMS.HighQ.Models.Response
{
    internal class TokensMapper
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int AccessTokenExpiresIn { get; set; }

        [JsonProperty("refresh_token_expires_in")]
        public int? RefreshTokenExpiresIn { get; set; }
    }
}
