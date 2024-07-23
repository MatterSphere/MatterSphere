namespace MSIndex.Common.Models
{
    public class MSAddress : BaseEntity
    {
        [MapKeyAttribute(Key = "sc")]
        public string Address { get; set; }
    }
}
