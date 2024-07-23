using System;

namespace FWBS.OMS.Dashboard
{
    public class TaskRow
    {
        public TaskRow(long id, long clientId, long fileId, string clientNo, string fileNo, string description, DateTime due, string type)
        {
            Id = id;
            ClientId = clientId;
            FileId = fileId;
            ClientNo = clientNo;
            FileNo = fileNo;
            Description = description;
            Due = due;
            Type = type;
        }
        
        public long Id { get; private set; }
        public long ClientId { get; private set; }
        public long FileId { get; private set; }
        public string ClientNo { get; private set; }
        public string FileNo { get; private set; }
        public string Description { get; private set; }
        public DateTime Due { get; private set; }
        public string Type { get; private set; }

        public string StatusCode { get; set; }
        public string StatusDescription { get; set; }
        public int CreatedById { get; private set; }
        public string CreatedByName { get; private set; }
        public int AssignedToId { get; private set; }
        public string AssignedToName { get; private set; }
        public string Team { get; set; }
        public DateTime? CompletedDate { get; set; }

        public void SetCreatedBy(int id, string name)
        {
            CreatedById = id;
            CreatedByName = name;
        }

        public void SetAssignedTo(int id, string name)
        {
            AssignedToId = id;
            AssignedToName = name;
        }

        public string Matter
        {
            get { return $"{ClientNo}/{FileNo}"; }
        }
    }
}
