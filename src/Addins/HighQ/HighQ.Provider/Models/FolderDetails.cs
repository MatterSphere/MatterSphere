namespace FWBS.OMS.HighQ.Models
{
    internal class FolderDetails
    {
        public FolderDetails(int iSheetId, string clientNo, string omsFileNo, string clientColumnTitle,
            string omsFileColumnTitle, string folderColumnTitle)
        {
            this.iSheetId = iSheetId;
            ClientNo = clientNo;
            OmsFileNo = omsFileNo;
            ClientColumnTitle = clientColumnTitle;
            OmsFileColumnTitle = omsFileColumnTitle;
            FolderColumnTitle = folderColumnTitle;
        }

        public int iSheetId { get; }
        public string ClientNo { get; }
        public string OmsFileNo { get; }
        public string ClientColumnTitle { get; }
        public string OmsFileColumnTitle { get; }
        public string FolderColumnTitle { get; }
    }
}
