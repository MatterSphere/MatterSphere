using Horizon.Common.Interfaces;

namespace Horizon.Common.Models.Repositories.IndexReport
{
    public class DocumentBucket : IReportItem
    {
        public DocumentBucket(string type, long success, long failed, long emptyContent)
        {
            Type = type;
            Success = success;
            Failed = failed;
            EmptyContent = emptyContent;
        }

        public string Type { get; set; }
        public long Success { get; set; }
        public long Failed { get; set; }
        public long EmptyContent { get; set; }
    }
}
