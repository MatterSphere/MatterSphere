﻿using System.Collections.Generic;
using System.Dynamic;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common.Handlers
{
    public class AddressHandler : EntityHandler
    {
        public AddressHandler(IDbProvider dbProvider, IMapper mapper) : base(dbProvider, mapper)
        {
        }

        public override void Index(List<ExpandoObject> items)
        {
            var addresses = Map<MSAddress>(items);
            _dbProvider.Index(addresses);
        }
    }
}