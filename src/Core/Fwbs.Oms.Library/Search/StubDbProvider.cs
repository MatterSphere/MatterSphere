using FWBS.Common.Elasticsearch;

namespace FWBS.OMS.Search
{
    public class StubDbProvider : IDbProvider
    {
        public string[] GetSearchableFields()
        {
            return new string [0];
        }

        public string[] GetSuggestableFields()
        {
            return new string[0];
        }

        public string[] GetFacetableFields()
        {
            return new string[0];
        }

        public FieldTitle[] GetFieldTitles()
        {
            return new FieldTitle[0];
        }

        public string[] GetIndexedEntities()
        {
            return new string[0];
        }

        public bool IsSummaryFieldEnabled()
        {
            return false;
        }
    }
}
