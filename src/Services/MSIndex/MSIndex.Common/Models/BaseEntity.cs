using System;

namespace MSIndex.Common.Models
{
    public class BaseEntity
    {
        [MapKeyAttribute(Key = "mattersphereid")]
        public long MatterSphereId { get; set; }

        [MapKeyAttribute(Key = "modifieddate")]
        public DateTime ModifiedDate { get; set; }
        
        [MapKeyAttribute(Key = "ugdp")]
        public string UGDP { get; set; }

        [MapKeyAttribute(Key = "op")]
        public char Operation { get; set; }
    }
}
