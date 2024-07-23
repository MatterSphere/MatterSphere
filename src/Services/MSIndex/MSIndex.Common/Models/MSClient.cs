namespace MSIndex.Common.Models
{
    public class MSClient : BaseEntity
    {
        [MapKeyAttribute(Key = "address-id")]
        public long AddressId { get; set; }

        [MapKeyAttribute(Key = "clientType")]
        public string ClientType { get; set; }

        [MapKeyAttribute(Key = "clName")]
        public string Name { get; set; }

        [MapKeyAttribute(Key = "clNo")]
        public string Number { get; set; }

        [MapKeyAttribute(Key = "clNotes")]
        public string Notes { get; set; }
    }
}
