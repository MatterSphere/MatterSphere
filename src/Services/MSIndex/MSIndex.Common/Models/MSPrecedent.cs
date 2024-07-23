namespace MSIndex.Common.Models
{
    public class MSPrecedent : BaseEntity
    {
        [MapKeyAttribute(Key = "precedentType")]
        public string PrecedentType { get; set; }

        [MapKeyAttribute(Key = "precLibrary")]
        public string Library { get; set; }

        [MapKeyAttribute(Key = "precCategory")]
        public string Category { get; set; }

        [MapKeyAttribute(Key = "precSubCategory")]
        public string SubCategory { get; set; }

        [MapKeyAttribute(Key = "precDeleted")]
        public bool Deleted { get; set; }

        [MapKeyAttribute(Key = "precedentExtension")]
        public string Extension { get; set; }

        [MapKeyAttribute(Key = "precDesc")]
        public string Description { get; set; }

        [MapKeyAttribute(Key = "precTitle")]
        public string Title { get; set; }
    }
}
