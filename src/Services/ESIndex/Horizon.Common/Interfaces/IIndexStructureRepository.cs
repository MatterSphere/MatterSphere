using System.Collections.Generic;
using Horizon.Common.Models.Repositories.IndexStructure;

namespace Horizon.Common.Interfaces
{
    public interface IIndexStructureRepository
    {
        List<IndexInfo> GetIndices();
        List<IndexEntity> GetIndexEntities(short indexId);
        List<IndexFieldRow> GetIndexFields(short entityId);
        List<IndexFieldRow> GetAllIndexFields(short indexId);
        void DeleteIndexField(short entityId, string field);
        bool CheckExtendedData(string tableName, string pkFieldName);
        List<TableField> GetTableFields(string tableName);
        void AddField(IndexField field);
        void UpdateIndexField(IndexField field);
        void UpdateCommonIndexField(short indexType, IndexField field);
        void ChangeIndexingEnabling(short entityId, bool enable);
        void UpdateSummaryTemplate(IndexEntity indexEntity);
        List<string> GetCodeLookupGroups();
        Dictionary<string, string> GetFacetableCodeLookups();
        void CreateFacetableCodeLookup(string cdCode, string cdDesc);
        void UpdateFacetableCodeLookup(string cdCode, string cdDesc);
    }
}
