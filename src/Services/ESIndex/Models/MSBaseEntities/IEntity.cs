using System;

namespace Models.MSBaseEntities
{
    public interface IEntity
    {
        Guid _id { get; set; }
        long mattersphereid { get; set; }
        DateTime modifieddate { get; set; }
        string objecttype { get; set; }

        IEntity Clone();
    }
}
