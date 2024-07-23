namespace MSIndex.Common.Models
{
    public class MSDocument : BaseEntity
    {
        [MapKeyAttribute(Key = "associate-id")]
        public long AssociateId { get; set; }

        [MapKeyAttribute(Key = "client-id")]
        public long ClientId { get; set; }

        [MapKeyAttribute(Key = "file-id")]
        public long FileId { get; set; }

        [MapKeyAttribute(Key = "docDeleted")]
        public bool DocDeleted { get; set; }

        [MapKeyAttribute(Key = "documentExtension")]
        public string DocumentExtension { get; set; }

        [MapKeyAttribute(Key = "documentType")]
        public string DocumentType { get; set; }

        [MapKeyAttribute(Key = "docDesc")]
        public string Description { get; set; }

        [MapKeyAttribute(Key = "usrFullName")]
        public string Author { get; set; }
    }
}
