using System;

namespace FWBS.OMS.Dashboard
{
    public class MatterRow
    {
        public MatterRow(long clientId, long fileId, string clientNo, string fileNo, string description)
        {
            ClientId = clientId;
            FileId = fileId;
            ClientNo = clientNo;
            FileNo = fileNo;
            Description = description;
        }

        public long ClientId { get; private set; }
        public long FileId { get; private set; }
        public string ClientNo { get; private set; }
        public string FileNo { get; private set; }
        public string Description { get; private set; }
        public DateTime? ReviewDate { get; set; }

        public string Number
        {
            get { return $"{ClientNo}/{FileNo}"; }
        }
    }
}
