using System;
using System.Collections.Generic;
using Models.Common;
using Models.ElasticsearchModels;

namespace Models.Interfaces
{
    public interface IElasticsearchProvider
    {
        void CreateUserIndex(List<Field> fields);
        void CreateDataIndex(List<Field> fields);
        bool CheckUserIndex();
        bool CheckDataIndex();
        bool CheckService(out string error);

        ElasticsearchResponse Index(IndexDocument document);
        ElasticsearchResponse BulkIndex(MessageTypeEnum messageType, List<IndexDocument> documents);
        bool Delete(Guid key);
    }
}
