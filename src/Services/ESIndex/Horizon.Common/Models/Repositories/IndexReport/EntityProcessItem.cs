using System;

namespace Horizon.Common.Models.Repositories.IndexReport
{
    public class EntityProcessItem
    {
        public EntityProcessItem(string entity, DateTime date, int success, int failed, long messages, long size = 0)
        {
            Entity = entity;
            StartDate = date;
            Success = success;
            Failed = failed;
            Messages = messages;
            Size = size;
        }

        public string Entity { get; set; }
        public DateTime StartDate { get; set; }
        public int Success { get; set; }
        public int Failed { get; set; }
        public long Size { get; set; }
        public long Messages { get; set; }
    }
}
