using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{
    public class CurrentUserProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("preferred_library")]
        public string PreferredLibrary { get; set; }

        [JsonProperty("config_database")]
        public string ConfigDatabase { get; set; }

        [JsonProperty("user_domain")]
        public string UserDomain { get; set; }

        [JsonProperty("my_favorites_id")]
        public string MyFavoritesId { get; set; }

        [JsonProperty("my_matter_id")]
        public string MyMattersId { get; set; }

        public override string ToString()
        {
            return $"{GetType()}: [Id: {Id}, Email: {Email}, FullName: {FullName}, Location: {Location}, " +
                   $"Phone: {Phone}, PreferredLibrary: {PreferredLibrary}, ConfigDatabase: {ConfigDatabase}, " +
                   $"UserDomain: {UserDomain}, MyFavoritesId: {MyFavoritesId}, MyMattersId: {MyMattersId}]";
        }
    }    
}