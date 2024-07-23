namespace MSIndex.Common.Models
{
    public class MSAssociate : BaseEntity
    {
        [MapKeyAttribute(Key = "file-id")]
        public long FileId { get; set; }

        [MapKeyAttribute(Key = "contact-id")]
        public long ContactId { get; set; }

        [MapKeyAttribute(Key = "associateType")]
        public string AssociateType { get; set; }

        [MapKeyAttribute(Key = "assocHeading")]
        public string Heading { get; set; }

        [MapKeyAttribute(Key = "assocSalut")]
        public string Salutation { get; set; }

        [MapKeyAttribute(Key = "assocAddressee")]
        public string Addressee { get; set; }

        [MapKeyAttribute(Key = "assocNotes")]
        public string Notes { get; set; }
    }
}
