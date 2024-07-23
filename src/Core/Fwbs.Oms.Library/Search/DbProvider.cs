using System;
using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.Search
{
    public class DbProvider : IDbProvider
    {
        public string[] GetSearchableFields()
        {
            throw new NotImplementedException();
        }

        public string[] GetSuggestableFields()
        {
            throw new NotImplementedException();
        }

        public string[] GetFacetableFields()
        {
            throw new NotImplementedException();
        }

        public FieldTitle[] GetFieldTitles()
        {
            throw new NotImplementedException();
        }

        public string[] GetIndexedEntities()
        {
            return new string[]
            {
                "DOCUMENT", "USERS", "FILE", "CONTACT", "CLIENT", "ASSOCIATE", "ADDRESS", "PRECEDENT", "APPOINTMENT", "TASK"
            };
        }

        public bool IsSummaryFieldEnabled()
        {
            return false;
        }
    }
}
