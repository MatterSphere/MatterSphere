using System.Collections.Generic;
using System.Dynamic;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Common.Handlers
{
    public abstract class EntityHandler
    {
        protected readonly IDbProvider _dbProvider;
        private readonly IMapper _mapper;

        protected EntityHandler(IDbProvider dbProvider, IMapper mapper)
        {
            _dbProvider = dbProvider;
            _mapper = mapper;
        }

        public abstract void Index(List<ExpandoObject> items);

        protected T[] Map<T>(List<ExpandoObject> items)  where T : BaseEntity, new()
        {
            var entities = new T[items.Count];
            for (int i = 0; i < items.Count; i++)
            {
                entities[i] = new T();
                _mapper.Map(items[i], entities[i]);
            }

            return entities;
        }
    }
}
