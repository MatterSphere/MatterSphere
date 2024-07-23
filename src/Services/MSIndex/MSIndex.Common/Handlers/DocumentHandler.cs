using System.Collections.Generic;
using System.Dynamic;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common.Handlers
{
    public class DocumentHandler : EntityHandler
    {
        public DocumentHandler(IDbProvider dbProvider, IMapper mapper) : base(dbProvider, mapper)
        {
        }

        public override void Index(List<ExpandoObject> items)
        {
            var documents = Map<MSDocument>(items);
            _dbProvider.Index(documents);
        }
    }
}
