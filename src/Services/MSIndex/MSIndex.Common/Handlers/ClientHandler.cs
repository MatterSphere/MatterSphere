using System.Collections.Generic;
using System.Dynamic;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common.Handlers
{
    public class ClientHandler : EntityHandler
    {
        public ClientHandler(IDbProvider dbProvider, IMapper mapper) : base(dbProvider, mapper)
        {
        }

        public override void Index(List<ExpandoObject> items)
        {
            var clients = Map<MSClient>(items);
            _dbProvider.Index(clients);
        }
    }
}
