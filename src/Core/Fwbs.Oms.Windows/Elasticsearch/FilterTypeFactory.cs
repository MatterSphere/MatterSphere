using System;
using System.Collections.Generic;
using System.Linq;
using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.UI.Elasticsearch
{
    class FilterTypeFactory
    {
        private readonly IDbProvider _dbProvider;

        public FilterTypeFactory(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public EntityTypeEnum[] LoadHiddenEntities()
        {
            var entities = _dbProvider.GetIndexedEntities();
            return GetHiddenFilters(entities);
        }

        private EntityTypeEnum[] GetHiddenFilters(string[] entities)
        {
            var filters = new List<EntityTypeEnum>();
            if (!entities.Contains("appointment", StringComparer.OrdinalIgnoreCase))
            {
                filters.Add(EntityTypeEnum.Appointment);
            }

            if (!entities.Contains("email", StringComparer.OrdinalIgnoreCase))
            {
                filters.Add(EntityTypeEnum.Email);
            }

            if (!entities.Contains("note", StringComparer.OrdinalIgnoreCase))
            {
                filters.Add(EntityTypeEnum.Note);
            }

            if (!entities.Contains("task", StringComparer.OrdinalIgnoreCase))
            {
                filters.Add(EntityTypeEnum.Task);
            }

            return filters.ToArray();
        }
    }
}
