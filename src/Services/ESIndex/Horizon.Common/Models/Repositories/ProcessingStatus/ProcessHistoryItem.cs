using System;

namespace Horizon.Common.Models.Repositories.ProcessingStatus
{
    public class ProcessHistoryItem
    {
        public ProcessHistoryItem(int id, DateTime startDate)
        {
            Id = id;
            StartDate = startDate;
        }

        public int Id { get; }
        public DateTime StartDate { get; }
        public DateTime? FinishDate { get; set; }
        public int Successful { get; set; }
        public int Failed { get; set; }
        public int ContentErrors { get; set; }
    }
}
