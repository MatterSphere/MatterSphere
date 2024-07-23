using System.Collections.Generic;
using System.Dynamic;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common.Handlers
{
    public class UserHandler : EntityHandler
    {
        public UserHandler(IDbProvider dbProvider, IMapper mapper) : base(dbProvider, mapper)
        {
        }

        public override void Index(List<ExpandoObject> items)
        {
            var users = Map<MSUser>(items);
            _dbProvider.Index(users);
        }
    }
}
