using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace ClaimsPortal.BulkTransfer
{
    public class Notifications
    {
        private const string TRANSFERRED_CLAIM_PATTERN_IN_NOTIFICATIONS = @"(The claim )(\d{1,16})( was transferred from )(\w+)( to )(\w+)";
        private string processType;


        public Notifications(string processType)
        {
            this.processType = processType;
        }


        /// <summary>
        /// Send in a DataTable of notifications from a GetNotificationsList method call and get a list of TransferData objects back if any transferred claim notifications were found.
        /// </summary>
        /// <param name="notifications"></param>
        public List<TransferData> Get(DataTable notifications)
        {
            var pattern = new Regex(TRANSFERRED_CLAIM_PATTERN_IN_NOTIFICATIONS);

            List<DataRow> transferredClaimNotifications = new List<DataRow>();

            foreach (DataRow notificationRow in notifications.Rows)
            {
                if (pattern.Match(notificationRow["notificationMessage"].ToString()).Success)
                {
                    transferredClaimNotifications.Add(notificationRow);
                }
            }

            var transferDataObjects = new List<TransferData>();

            foreach (DataRow transferredClaimRow in transferredClaimNotifications)
            {
                var match = pattern.Match(transferredClaimRow["notificationMessage"].ToString());
                var dataTransferObject = BuildTransferDataObject(match, Convert.ToDateTime(transferredClaimRow["notificationDateTime"].ToString()));
                transferDataObjects.Add(dataTransferObject);
            }

            return transferDataObjects;
        }


        private TransferData BuildTransferDataObject(System.Text.RegularExpressions.Match match, DateTime transferDate)
        {
            return new TransferData()
            {
                ProcessType = processType,
                ClaimID = match.Groups[2].Value.PadLeft(16, '0'),
                Source = match.Groups[4].Value,
                Destination = match.Groups[6].Value,
                DateOfTransfer = transferDate,
                FullMessage = string.Format("The claim {0} was transferred from {1} to {2}", match.Groups[2].Value, match.Groups[4].Value, match.Groups[6].Value)
            };
        }
    }
}
