using System;
using System.Collections.Generic;
using System.Linq;

namespace Fwbs.Office.Outlook
{

    using MSOutlook = Microsoft.Office.Interop.Outlook;

    internal sealed class OutlookFoldersContainer
    {
        private Dictionary<string, OutlookFolder> folders = new Dictionary<string, OutlookFolder>();
  
        private readonly OutlookApplication parent;

        public OutlookFoldersContainer(OutlookApplication parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            this.parent = parent;
        }

        #region Items

        public void Refresh(params OutlookFolder[] keepAlive)
        {
            var items = folders.Values.ToArray();
            var exp = parent.ActiveExplorer();

            foreach (var item in items)
            {
                if (exp != null && exp.IsFolderSelected(item))
                    continue;

                if (keepAlive != null)
                {
                    if (Array.IndexOf<OutlookFolder>(keepAlive, item) >= 0)
                        continue;
                }

                RemoveFolder(item);
                
            }

            parent.AutoCleanup();
        }

        private OutlookFolder AddFolder(MSOutlook.MAPIFolder folder)
        {
            return GetFolder(folder);
        }

        public OutlookFolder GetFolder(MSOutlook.MAPIFolder folder)
        {
            var of  = folder as OutlookFolder;
            if (of != null)
                return of;

            if (folder == null)
                return null;
            
            var entryid = GetEntryId(folder);

            OutlookFolder item;
            if (folders.TryGetValue(entryid, out item))
            {
                return item;
            }

            item = new OutlookFolder(folder);
            folders.Add(entryid, item);
            
            return item;
            

        }


        internal void RemoveFolder(MSOutlook.MAPIFolder folder)
        {
            if (folder == null)
                return;

            var entryid = GetEntryId(folder);

            OutlookFolder item;
            if (folders.TryGetValue(entryid, out item))
            {
                folders.Remove(entryid);
                item.Dispose();
            }

        }

        private static string GetEntryId(MSOutlook.MAPIFolder folder)
        {
            return folder.StoreID + folder.EntryID;
        }

        public IEnumerable<OutlookFolder> LoadedFolders
        {
            get
            {
                foreach (var item in folders.Values)
                    yield return item;
            }
        }


        #endregion

        public void Dispose()
        {
            foreach (var item in folders.Values)
                item.Dispose();
            folders.Clear();
        }

    }
}
