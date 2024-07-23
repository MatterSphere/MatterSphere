using System.Collections.Generic;
using Models.DbModels;

namespace Models.Interfaces
{
    public interface IDbLogger
    {
        void LogDocumentIndex(string entity, DocumentLog[] logs);
        void StartIndexingProcess();
        void CompleteIndexingProcess();
        void LogMessageProcess(MessageLog log);
    }
}
