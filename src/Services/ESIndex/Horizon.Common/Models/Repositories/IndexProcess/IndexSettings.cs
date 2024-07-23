using System;

namespace Horizon.Common.Models.Repositories.IndexProcess
{
    public class IndexSettings
    {
        public IndexSettings()
        {

        }

        public IndexSettings(long batchSize,
                             bool orderFromOldItems,
                             DateTime? documentDateLimit,
                             DateTime? previousDocumentDateLimit,
                             bool summaryFieldEnabled)
        {
            BatchSize = batchSize;
            ProcessOrderFromOldItems = orderFromOldItems;
            DocumentDateLimit = documentDateLimit;
            PreviousDocumentDateLimit = previousDocumentDateLimit;
            SummaryFieldEnabled = summaryFieldEnabled;
        }

        public long BatchSize { get; set; }
        public bool ProcessOrderFromOldItems { get; set; }
        public DateTime? DocumentDateLimit { get; set; }
        public DateTime? PreviousDocumentDateLimit { get; set; }
        public bool SummaryFieldEnabled { get; set; }
    }
}
