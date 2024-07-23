namespace MSIndex.Common.Models
{
    public class MSFile : BaseEntity
    {
        [MapKeyAttribute(Key = "client-id")]
        public long ClientId { get; set; }

        [MapKeyAttribute(Key = "fileType")]
        public string FileType { get; set; }

        [MapKeyAttribute(Key = "fileStatus")]
        public string FileStatus { get; set; }

        [MapKeyAttribute(Key = "fileDesc")]
        public string Description { get; set; }

        [MapKeyAttribute(Key = "fileNotes")]
        public string Notes { get; set; }
    }
}
