using System.Collections.Generic;
using Models.DbModels;
using Models.Interfaces;

namespace IndexingController.Providers
{
    public class DbLogger : IDbLogger
    {
        private IDbProvider _dbProvider;

        public DbLogger(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public void LogDocumentIndex(string entity, DocumentLog[] logs)
        {
            switch (entity.ToLower())
            {
                case "document":
                    entity = "Document";
                    break;
                case "precedent":
                    entity = "Precedent";
                    break;
                default:
                    return;
            }

            _dbProvider.SetDocumentLogs(entity, logs);
        }

        public void StartIndexingProcess()
        {
            _dbProvider.StartIndexingProcess();
        }

        public void CompleteIndexingProcess()
        {
            _dbProvider.CompleteIndexingProcess();
        }

        public void LogMessageProcess(MessageLog log)
        {
            _dbProvider.SetMessageLogs(log);
        }
    }
}
