using System;

namespace ClaimsPortal.BulkTransfer
{
    public class TransferData
    {
        public string ClaimID { get; set; }
        public string ProcessType { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime DateOfTransfer { get; set; }
        public string FullMessage { get; set; }
    }
}
