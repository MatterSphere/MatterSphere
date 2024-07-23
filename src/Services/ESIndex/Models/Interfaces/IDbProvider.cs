using System.Collections.Generic;
using Models.DbModels;

namespace Models.Interfaces
{
    public interface IDbProvider
    {
        #region Index structure
        string GetUserIndexName();
        string GetDataIndexName();
        List<IndexField> GetIndexFields(IndexTypeEnum indexType);
        List<string> GetSuggestableFields();
        #endregion

        #region Logs
        void SetDocumentLogs(string entity, DocumentLog[] messageLogs);
        void StartIndexingProcess();
        void CompleteIndexingProcess();
        void SetMessageLogs(MessageLog messageLog);
        #endregion

        #region Horizon

        List<BlacklistCriterion> GetBlacklist();

        #endregion

        bool GetSummarySetting();
        Dictionary<string, string> GetSummaryTemplates();
    }
}
