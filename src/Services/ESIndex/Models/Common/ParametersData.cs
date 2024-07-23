using System.Collections.Generic;

namespace Models.Common
{
    public class ParametersData
    {
        public ParametersData(int threads, long bulk, int documents, bool useExtendedLogs)
        {
            ThreadsCount = threads;
            MaxBulkSize = bulk;
            MaxDocumentsCount = documents;
            UseExtendedLogs = useExtendedLogs;
        }

        public int ThreadsCount { get; }
        public long MaxBulkSize { get; }
        public int MaxDocumentsCount { get; }
        public bool UseExtendedLogs { get; }
        public bool SummaryFieldEnabled { get; set; }
        public Dictionary<string, string> SummaryTemplates { get; set; }
    }
}
