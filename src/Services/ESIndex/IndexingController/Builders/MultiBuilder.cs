using System.Collections.Generic;
using System.Dynamic;

namespace IndexingController.Builders
{
    public abstract class MultiBuilder
    {
        public readonly string _relationFieldName = "Sys_Rel";
        public readonly string _entityKeyFieldName = "id";
        public readonly string _entityIdFieldName = "mattersphereid";
        public readonly string _entityDeleted = "entityDeleted";

        public abstract IEnumerable<ExpandoObject> CreateEntities(ExpandoObject model);
    }
}
