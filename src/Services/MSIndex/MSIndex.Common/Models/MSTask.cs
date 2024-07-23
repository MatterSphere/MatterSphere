namespace MSIndex.Common.Models
{
    public class MSTask : BaseEntity
    {
        [MapKeyAttribute(Key = "file-id")]
        public long FileId { get; set; }

        [MapKeyAttribute(Key = "document-id")]
        public long DocumentId { get; set; }

        [MapKeyAttribute(Key = "taskType")]
        public string TaskType { get; set; }

        [MapKeyAttribute(Key = "tskDesc")]
        public string Description { get; set; }

        [MapKeyAttribute(Key = "tskNotes")]
        public string Notes { get; set; }
    }
}
