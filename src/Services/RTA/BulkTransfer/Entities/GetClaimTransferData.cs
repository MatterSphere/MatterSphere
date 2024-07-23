using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClaimsPortal.BulkTransfer
{
    public class GetClaimTransferData
    {
        private const string TRANSFERRED_CLAIM_PATTERN_IN_CLAIM_XML = @"(This claim has been transferred from )(\w+)( to )(\w+)( on )(\d{2}[/]\d{2}[/]\d{4})";
        protected string processType;


        public GetClaimTransferData(string processType)
        {
            this.processType = processType;
        }


        /// <summary>
        /// Get the transfer data from claim xml
        /// </summary>
        /// <param name="claimID"></param>
        /// <param name="claimData"></param>
        /// <returns></returns>
        public List<TransferData> Get(string claimID, string claimData)
        {
            var pattern = new Regex(TRANSFERRED_CLAIM_PATTERN_IN_CLAIM_XML);
            var matches = pattern.Matches(claimData);

            var transferData = new List<TransferData>();

            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    transferData.Add(BuildTransferDataObject(claimID, match));
                }
            }

            return transferData;
        }


        private TransferData BuildTransferDataObject(string claimID, System.Text.RegularExpressions.Match match)
        {
            return new TransferData()
            {
                ProcessType = processType,
                ClaimID = claimID.PadLeft(16, '0'),
                Source = match.Groups[2].Value,
                Destination = match.Groups[4].Value,
                DateOfTransfer = Convert.ToDateTime(match.Groups[6].Value),
                FullMessage = string.Format("This claim has bee transferred from {0} to {1} on {2}", match.Groups[2].Value, match.Groups[4].Value, match.Groups[6].Value)
            };
        }
    }
}
