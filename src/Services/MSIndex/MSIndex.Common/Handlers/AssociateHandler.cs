using System.Collections.Generic;
using System.Dynamic;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common.Handlers
{
    public class AssociateHandler : EntityHandler
    {
        public AssociateHandler(IDbProvider dbProvider, IMapper mapper) : base(dbProvider, mapper)
        {
        }

        public override void Index(List<ExpandoObject> items)
        {
            var associates = Map<MSAssociate>(items);
            _dbProvider.Index(associates);
        }
    }
}
