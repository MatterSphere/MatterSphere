namespace MSIndex.Common.Models
{
    public class MSAppointment : BaseEntity
    {
        [MapKeyAttribute(Key = "file-id")]
        public long FileId { get; set; }

        [MapKeyAttribute(Key = "client-id")]
        public long ClientId { get; set; }

        [MapKeyAttribute(Key = "associate-id")]
        public long? AssociateId { get; set; }

        [MapKeyAttribute(Key = "document-id")]
        public long? DocumentId { get; set; }

        [MapKeyAttribute(Key = "appointmentType")]
        public string AppointmentType { get; set; }

        [MapKeyAttribute(Key = "appDesc")]
        public string Description { get; set; }

        [MapKeyAttribute(Key = "appLocation")]
        public string Location { get; set; }
    }
}
