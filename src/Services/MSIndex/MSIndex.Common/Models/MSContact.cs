namespace MSIndex.Common.Models
{
    public class MSContact : BaseEntity
    {
        [MapKeyAttribute(Key = "contactType")]
        public string ContactType { get; set; }

        [MapKeyAttribute(Key = "address-id")]
        public long AddressId { get; set; }

        [MapKeyAttribute(Key = "contName")]
        public string Name { get; set; }

        [MapKeyAttribute(Key = "contNotes")]
        public string Notes { get; set; }
    }
}
