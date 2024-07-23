using System;
using System.Collections.Generic;

namespace Fwbs.Office.Outlook
{
    internal sealed class ChunkedItems : IEnumerable<OutlookItem>
    {
        private OutlookApplication application;

        public static ChunkedItems Create(OutlookApplication application, System.Collections.IEnumerable items, int chunkSize)
        {

            var chunked = new ChunkedItems();

            chunked.Items = new List<object>();

            chunked.Wrappers = new Dictionary<object, OutlookItem>();

            chunked.Chunks = new List<List<object>>();

            chunked.application = application;

            var list = new List<object>();

            int ctr = 0;

            foreach (var item in items)
            {
                ctr++;

                list.Add(item);
                chunked.Items.Add(item);

                if (ctr == chunkSize)
                {
                    chunked.Chunks.Add(list);
                    list = new List<object>();
                    ctr = 0;
                }
            }

            if (!chunked.Chunks.Contains(list))
                chunked.Chunks.Add(list);

            return chunked;
        }

        private List<object> Items { get; set; }

        private Dictionary<object, OutlookItem> Wrappers{get; set;}

        private List<List<object>> Chunks { get; set; }

        public IEnumerator<OutlookItem> GetEnumerator()
        {
            foreach (var chunk in Chunks)
            {
                foreach (var item in chunk)
                {
                    yield return GetItem(item);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public OutlookItem this[int index]
        {
            get
            {
                return GetItem(Items[index]);
            }
        }

        private OutlookItem GetItem(object item)
        {
            OutlookItem wrapper;

            if (Wrappers.TryGetValue(item, out wrapper))
                return wrapper;
            else
            {
                wrapper = application.GetItem(item);
                if (item == null)
                    return null;
                Wrappers.Add(item, wrapper);
                return wrapper;
            }
        }

        public int Count
        {
            get
            {
                return Items.Count;
            }
        }
    }


    public sealed class DetachableItems : IEnumerable<OutlookItem>, IDisposable
    {
        private readonly List<OutlookItem> attached = new List<OutlookItem>();
        private readonly IEnumerable<OutlookItem> items;
        private readonly OutlookApplication application;
        private int count;

        public DetachableItems(OutlookApplication application, IEnumerable<OutlookItem> items)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            if (items == null)
                throw new ArgumentNullException("items");

            this.application = application;
            this.items = items;

            var olitems = items as IOutlookItems;
            if (olitems != null)
                this.count = olitems.Count;

            var arr = items as OutlookItem[];
            if (arr != null)
                this.count = arr.Length;

            var col = items as System.Collections.ICollection;
            if (col != null)
                this.count = col.Count;

            var list = items as System.Collections.IList;
            if (list != null)
                this.count = list.Count;

            var col2 = items as ICollection<OutlookItem>;
            if (col2 != null)
                this.count = col2.Count;

            var list2 = items as IList<OutlookItem>;
            if (list2 != null)
                this.count = list2.Count;


        }

        public int Count
        {
            get
            {
                return count;
            }
        }

        #region IEnumerable<OutlookItem> Members


        public IEnumerator<OutlookItem> GetEnumerator()
        {

            int ctr = 0;
            int chunksize = application.Settings.Memory.MultipleItemChunkSize;

            foreach (var item in items)
            {
                ctr++;

                DetachAll();
                Attach(item);
                yield return item;

                if ((ctr % chunksize) == 0)
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }

            DetachAll();

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Methods


        private void DetachAll()
        {
            while (attached.Count > 0)
            {
                var item = attached[0];
                Detach(item);
            }
        }

        private void Attach(OutlookItem item)
        {
            if (!attached.Contains(item))
            {
                item.Attach();
                attached.Add(item);
            }
        }

        private void Detach(OutlookItem item)
        {
            attached.Remove(item);
            if (item.IsDeleted || item.Saved)
                item.Detach();
        }



        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            DetachAll();
        }

        #endregion
    }

}
