using System;

namespace MSIndex.Common.Models
{
    public class MSUser : BaseEntity
    {
        public MSUser()
        {
            ModifiedDate = DateTime.Today;
        }

        [MapKeyAttribute(Key = "usrinits")]
        public string UserInits { get; set; }

        [MapKeyAttribute(Key = "usralias")]
        public string UserAlias { get; set; }

        [MapKeyAttribute(Key = "usrad")]
        public string UserAD { get; set; }

        [MapKeyAttribute(Key = "usrsql")]
        public string UserSql { get; set; }

        [MapKeyAttribute(Key = "usrfullname")]
        public string UserFullname { get; set; }

        [MapKeyAttribute(Key = "usractive")]
        public string UserActive { get; set; }

        [MapKeyAttribute(Key = "usrAccessList")]
        public string UserAccessList { get; set; }
    }
}
