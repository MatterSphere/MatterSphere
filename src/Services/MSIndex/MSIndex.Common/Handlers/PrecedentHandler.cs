using System.Collections.Generic;
using System.Dynamic;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common.Handlers
{
    public class PrecedentHandler : EntityHandler
    {
        public PrecedentHandler(IDbProvider dbProvider, IMapper mapper) : base(dbProvider, mapper)
        {
        }

        public override void Index(List<ExpandoObject> items)
        {
            var precedents = Map<MSPrecedent>(items);
            _dbProvider.Index(precedents);
        }
    }
}
