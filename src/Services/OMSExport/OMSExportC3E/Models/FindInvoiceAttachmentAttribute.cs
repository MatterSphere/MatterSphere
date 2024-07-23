using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class FindInvoiceAttachmentAttribute
    {
        [JsonProperty("InvMaster.InvIndex")]
        public int InvoiceIndex { get; set; }

        [JsonProperty("InvMaster.InvNumber")]
        public string InvoiceNumber { get; set; }

        [JsonProperty("InvMaster.IsReversed")]
        public int IsReversed { get; set; }

        [JsonProperty("NxAttachment.NxAttachmentID")]
        public string AttachmentID { get; set; }

        [JsonProperty("NxAttachment.FileName")]
        public string FileName { get; set; }

        [JsonProperty("NxAttachment.Description")]
        public string Description { get; set; }

        [JsonProperty("Matter.MatterID")]
        public string MatterID { get; set; }

        [JsonProperty("Matter.MattIndex")]
        public int MattIndex { get; set; }
    }
}
