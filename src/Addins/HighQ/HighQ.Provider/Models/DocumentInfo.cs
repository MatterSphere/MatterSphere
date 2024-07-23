namespace FWBS.OMS.HighQ.Models
{
    internal class DocumentInfo
    {
        public DocumentInfo(long clientId, string clientNo, string omsFileNo, string fullPath, string description)
        {
            ClientId = clientId;
            ClientNo = clientNo;
            OmsFileNo = omsFileNo;
            FullPath = fullPath;
            Description = description;
        }
        
        public long ClientId { get; }
        public string ClientNo { get; }
        public string OmsFileNo { get; }
        public string FullPath { get; }
        public string Description { get; }
    }
}
