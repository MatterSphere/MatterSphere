namespace FWBS.Common.Elasticsearch
{
    public interface IDbProvider
    {
        string[] GetSearchableFields();
        string[] GetSuggestableFields();
        string[] GetFacetableFields();
        FieldTitle[] GetFieldTitles();
        string[] GetIndexedEntities();
        bool IsSummaryFieldEnabled();
    }
}
