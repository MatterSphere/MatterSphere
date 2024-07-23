using System;
using System.Collections.Generic;
using System.Linq;

namespace Fwbs.Office.Outlook
{
    using System.Runtime.InteropServices;

    internal sealed class OutlookItemsContainer
    {
        private Dictionary<string, OutlookItem> itemsbyid = new Dictionary<string, OutlookItem>();
        private Dictionary<string, OutlookItem> newitemsbyid = new Dictionary<string, OutlookItem>();

        private readonly OutlookApplication parent;

        public OutlookItemsContainer(OutlookApplication parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            this.parent = parent;
        }

        #region Items


        private OutlookItem AddItem(object obj)
        {
            return GetItem(obj, false);
        }

        public OutlookItem GetItem(Func<object> action)
        {
            return GetItem(action, false);
        }

        public OutlookItem GetItem(Func<object> action, bool pin)
        {

            if (action == null)
                throw new ArgumentNullException("action");

            try
            {
                return this.GetItem(action(), pin);
            }
            catch (COMException comex)
            {
                if (comex.ErrorCode != HResults.E_QUOTA_EXCEEDED)
                    throw;

                Refresh();

                return this.GetItem(action(), pin);
            }
        }

        private OutlookItem GetItem(object obj)
        {
            return GetItem(obj, false);
        }

        private OutlookItem GetItem(object obj, bool pin)
        {
            OutlookItem item = obj as OutlookItem;
            if (item != null)
                return item;

            string entryid = String.Empty;
            string tempid = String.Empty;

            try
            {
                entryid = GetEntryId(obj, out tempid);

                if (!string.IsNullOrEmpty(entryid) || !string.IsNullOrEmpty(tempid))
                {
                    item = GetItemFromEntryId(entryid, tempid);

                    if (item != null)
                    {
                        item.IsPinned = pin;

                        OutlookItemFactory.AttachOrDispose(obj, item);

                        return item;
                    }
                }
            }
            catch (COMException comex)
            {
                if (comex.ErrorCode == HResults.E_QUOTA_EXCEEDED)
                {
                    throw;
                }
            }

            item = OutlookItemFactory.Create(obj, pin);
            if (item != null)
            {
                if (item.IsNew)
                    newitemsbyid.Add(item.TemporaryID, item);
                else
                    itemsbyid.Add(entryid, item);
            }

            return item;


        }

        internal void OnNewEntryId(OutlookItem item)
        {
            var entryId = item.EntryID;
            itemsbyid[entryId] = item;

            if (newitemsbyid.ContainsKey(item.TemporaryID))
                newitemsbyid.Remove(item.TemporaryID);

        }


        internal OutlookItem GetItemFromEntryId(string entryId, string tempId)
        {
            OutlookItem item;

            if (!String.IsNullOrEmpty(tempId) && newitemsbyid.TryGetValue(tempId, out item))
                return item;

            if (!String.IsNullOrEmpty(entryId) && itemsbyid.TryGetValue(entryId, out item))
                return item;

            foreach (var newitem in newitemsbyid.Values.ToArray())
            {
                if (newitem.IsDetached || newitem.IsDeleted)
                {
                    newitemsbyid.Remove(newitem.TemporaryID);
                    continue;
                }

                if (newitem.EntryID.Equals(entryId, StringComparison.OrdinalIgnoreCase))
                {
                    OnNewEntryId(newitem);
                    return newitem;
                }
            }

            return null;
        }

        private static string GetEntryId(object obj)
        {
            string tempid;
            return GetEntryId(obj, out tempid);
        }

        private static string GetEntryId(object obj, out string tempId)
        {
            tempId = null;
            int tries = 0;

        tryagain:

            try
            {
                var id = OutlookItem.GetPropertyEx<string>(obj, "EntryID");

                if (Marshal.IsComObject(obj))
                    tempId = (string)Marshal.GetComObjectData(obj, "TempID");

                return id;
            }
            catch (COMException comex)
            {
                if (comex.ErrorCode == HResults.E_QUOTA_EXCEEDED)
                {

                    if (++tries > 3)
                        throw;

                    Helpers.Cleanup();
                    goto tryagain;
                }

                if (Marshal.IsComObject(obj))
                {
                    tempId = (string)Marshal.GetComObjectData(obj, "TempID");
                    return tempId;
                }

                return String.Empty;
            }
        }

        private void RemoveItem(object obj)
        {

            if (obj == null)
                return;

            string entryid = String.Empty;
            string tempid;

            var oi = obj as OutlookItem;
            if (oi != null)
            {
                if (!CanRemove(oi) || parent.IsProcessing)
                    return;

                entryid = oi.EntryID;
                tempid = oi.TemporaryID;

                if (!oi.IsDetached)
                {
                    obj = oi.InternalItem;
                }

                if (InternalRemoveItem(entryid, tempid))
                    return;


            }

            entryid = GetEntryId(obj, out tempid);

            if (!parent.IsProcessing)
                InternalRemoveItem(entryid, tempid);


        }

        private bool InternalRemoveItem(string entryid, string tempid)
        {
            OutlookItem item;

            bool ret = false;

            if (!String.IsNullOrEmpty(entryid))
            {
                if (itemsbyid.TryGetValue(entryid, out item))
                {
                    itemsbyid.Remove(entryid);
                    item.Dispose();
                    ret = true;
                }
            }

            if (!String.IsNullOrEmpty(tempid))
            {
                if (newitemsbyid.TryGetValue(tempid, out item))
                {
                    newitemsbyid.Remove(tempid);
                    if (!item.IsDisposed)
                        item.Dispose();
                    ret = true;

                }
            }


            return ret;
        }

        public void Refresh()
        {
            Refresh(new OutlookItem[0]);
        }
        public void Refresh(params OutlookItem[] keepAlive)
        {
            object selecteditem = null;

            var explorer = (OutlookExplorer)parent.ActiveExplorer();
            if (explorer != null && !explorer.IsDetached && !explorer.IsDisposed)
            {
                var exp = explorer.InternalItem;
                if (exp != null && exp.Selection.Count > 0)
                    selecteditem = exp.Selection[1];
            }

            var items = LoadedItems.ToArray();

            //Clear out the in memory cache if the items are still selected or still visible or otherwise specified to be kept alive.
            foreach (var item in items)
            {
                if (!CanRemove(item) ||
                    (selecteditem != null && item.EntryID == GetEntryId(selecteditem)) ||
                    (keepAlive != null && keepAlive.Any(i => i == item || i.TemporaryID == item.TemporaryID || i.EntryID == item.EntryID)))
                    continue;

                RemoveItem(item);
            }

            parent.AutoCleanup();

        }

        private bool CanRemove(OutlookItem item)
        {
            if (item.IsPinned)
                return false;

            if (item.Inspector != null && item.Inspector.Visible)
                return false;

            if (!item.IsDetached && item.IsDeleted)
                return true;

            return true;
        }

        public void SetItems(IEnumerable<object> selectedItems)
        {

            List<object> selected = null;

            if (selectedItems == null)
                selected = new List<object>();
            else
                selected = new List<object>(selectedItems);

            var comp = new OutlookItemComparer();

            var sel = selected.ConvertAll<OutlookItem>(obj => GetItem(() => (GetItem(obj)))).Where(s => s != null);

            var loadeditems = (from itm in itemsbyid.Values
                               let insp = GetInspector(itm)
                               where insp == null || insp.Visible == false
                               select itm).ToArray();

            var toleave = loadeditems.Intersect(sel, comp);

            var tounload = loadeditems.Except(toleave, comp);


            var toload = sel.Except(tounload, comp)
                .Except(toleave, comp);

            foreach (var itm in toload)
                AddItem(itm);

            foreach (var itm in tounload)
                RemoveItem(itm);

            parent.AutoCleanup();

        }

        private OutlookInspector GetInspector(object obj)
        {
            if (obj == null)
                return null;

            var item = GetItem(obj);
            if (item == null)
                return null;

            if (item.IsDetached || item.IsDisposed || item.IsDeleted)
                return null;

            return item.Inspector;

        }


        public IEnumerable<OutlookItem> LoadedItems
        {
            get
            {
                foreach (var item in itemsbyid.Values)
                    yield return item;
                foreach (var item in newitemsbyid.Values)
                    yield return item;
            }
        }


        public void Dispose()
        {
            foreach (var item in itemsbyid.Values)
                item.Dispose();
            foreach (var item in newitemsbyid.Values)
                item.Dispose();
            itemsbyid.Clear();
            newitemsbyid.Clear();
        }

        private sealed class OutlookItemComparer : IEqualityComparer<OutlookItem>
        {
            #region IEqualityComparer<OutlookItem> Members

            public bool Equals(OutlookItem x, OutlookItem y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null)
                    return false;
                if (y == null)
                    return false;

                return x.GetHashCode() == y.GetHashCode();
            }

            public int GetHashCode(OutlookItem obj)
            {
                if (obj == null)
                    return 0;

                return obj.GetHashCode();
            }

            #endregion
        }

        #endregion

    }
}
