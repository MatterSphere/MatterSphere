using System.Collections.Generic;
using System.Dynamic;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common.Handlers
{
    public class FileHandler : EntityHandler
    {
        public FileHandler(IDbProvider dbProvider, IMapper mapper) : base(dbProvider, mapper)
        {
        }

        public override void Index(List<ExpandoObject> items)
        {
            var files = Map<MSFile>(items);
            _dbProvider.Index(files);
        }
    }
}
