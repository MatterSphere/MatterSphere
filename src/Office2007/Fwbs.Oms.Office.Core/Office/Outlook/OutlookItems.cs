using System;
using System.Collections.Generic;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed class OutlookItems
        : OutlookObject
        , IOutlookItems
        , MSOutlook.Items
    {
        #region Fields

        private readonly MSOutlook.Items items;
        private readonly ChunkedItems _chunked;

        #endregion

        #region Constructors

        public OutlookItems(MSOutlook.Items items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            this.items = items;

            Init(items);

            this._chunked = ChunkedItems.Create(Application, items, Application.Settings.Memory.MultipleItemChunkSize);
        }

        #endregion

        public override OutlookApplication Application
        {
            get
            {
                return OutlookApplication.GetApplication(items.Application);
            }
        }

        #region _Items Members

        public object Add(object Type)
        {
            return GetItem(() => items.Add(Type));
        }

        MSOutlook.Application MSOutlook._Items.Application
        {
            get { return Application; }
        }

        public MSOutlook.OlObjectClass Class
        {
            get { return items.Class; }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public object Find(string Filter)
        {
            return GetItem(() => items.Find(Filter));
        }

        public object FindNext()
        {
            return GetItem(() => items.FindNext());
        }

        public object GetFirst()
        {
            return GetItem(() => items.GetFirst());
        }

        public object GetLast()
        {
            return GetItem(() => items.GetLast());
        }

        public object GetNext()
        {
            return GetItem(() => items.GetNext());
        }

        public object GetPrevious()
        {
            return GetItem(() => items.GetPrevious());
        }

        public bool IncludeRecurrences
        {
            get
            {
                return items.IncludeRecurrences;
            }
            set
            {
                items.IncludeRecurrences = value;
            }
        }

        public object Parent
        {
            get 
            {
                var folder = items.Parent as MSOutlook.MAPIFolder;
                if (folder == null)
                    return items.Parent;
                else
                    return Application.GetFolder(folder);
            }
        }

        public object RawTable
        {
            get { return items.RawTable; }
        }

        public void Remove(int Index)
        {
            items.Remove(Index);
        }

        public void ResetColumns()
        {
            items.ResetColumns();
        }

        public MSOutlook.Items Restrict(string Filter)
        {
            return new OutlookItems(items.Restrict(Filter));
        }

        public MSOutlook.NameSpace Session
        {
            get { return Application.Session; }
        }

        public void SetColumns(string Columns)
        {
            items.SetColumns(Columns);
        }

        public void Sort(string Property, object Descending)
        {
            items.Sort(Property, Descending);
        }

        public object this[object Index]
        {
            get 
            {
                return GetItem(() => items[Index]);
            }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            return _chunked.GetEnumerator();
        }

        #endregion

        #region ItemsEvents_Event Members

        public event MSOutlook.ItemsEvents_ItemAddEventHandler ItemAdd
        {
            add
            {
                items.ItemAdd += value;
            }
            remove
            {
                items.ItemAdd -= value;
            }
        }

        public event MSOutlook.ItemsEvents_ItemChangeEventHandler ItemChange
        {
            add
            {
                items.ItemChange += value;
            }
            remove
            {
                items.ItemChange -= value;
            }
        }

        public event MSOutlook.ItemsEvents_ItemRemoveEventHandler ItemRemove
        {
            add
            {
                items.ItemRemove += value;
            }
            remove
            {
                items.ItemRemove -= value;
            }
        }

        #endregion

        #region IOutlookItems Members


        public OutlookItem this[int index]
        {
            get
            {
                return GetItem(() => items[index]);
            }
        }

        #endregion

        #region IEnumerable<OutlookItem> Members

        IEnumerator<OutlookItem> IEnumerable<OutlookItem>.GetEnumerator()
        {
            return _chunked.GetEnumerator();
        }

        #endregion
    }
}
