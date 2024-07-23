using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class GetUserResponse : BaseResponse
    {
        [JsonProperty("user")]
        public UserItem User { get; set; }

        public class UserItem
        {
            [JsonProperty("userId")]
            public string UserId { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }
    }
}
