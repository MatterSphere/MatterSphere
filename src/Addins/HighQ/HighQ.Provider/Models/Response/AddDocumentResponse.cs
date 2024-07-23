using Newtonsoft.Json;

namespace FWBS.OMS.HighQ.Models.Response
{
    internal class AddDocumentResponse
    {
        [JsonProperty("fileid")]
        public int DocumentId { get; set; }
    }
}
