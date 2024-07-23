using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class BaseResponse
    {
        public virtual bool Success
        {
            get { return string.IsNullOrWhiteSpace(ErrorMessage); }
            set { }
        }

        [JsonIgnore]
        public virtual string ErrorMessage { get; set; }

        [JsonIgnore]
        public byte[] RawBytes { get; set; }

        [JsonIgnore]
        public string Dump { get; set; }
    }
}
