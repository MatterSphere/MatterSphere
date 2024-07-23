using System;

namespace Horizon.Common.Models.Repositories.ProcessingStatus
{
    public class ProcessHistoryItemDetail
    {
        public ProcessHistoryItemDetail(string entity, DateTime startDate)
        {
            Entity = entity;
            StartDate = startDate;
        }

        public string Entity { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
        public int ContentErrors { get; set; }
        public long Size { get; set; }
    }
}
