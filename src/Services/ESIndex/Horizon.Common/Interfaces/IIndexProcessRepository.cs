using System.Collections.Generic;
using Horizon.Common.Models.Repositories.Blacklist;
using Horizon.Common.Models.Repositories.IndexProcess;

namespace Horizon.Common.Interfaces
{
    public interface IIndexProcessRepository
    {
        IEnumerable<BlacklistItem> GetBlacklist();
        void AddBlacklistItem(BlacklistItem item);
        void RemoveBlacklistGroup(string extension);
        void RemoveBlacklistItem(string extension, string metadata = null, string encoding = null);
        IEnumerable<string> GetExtensionsForReindexing();
        void AddExtensionForReindexing(string extension);
        void ReindexAllFailedDocuments();
        IndexSettings GetIndexSettings();
        void SaveIndexSettings(IndexSettings settings);
    }
}
