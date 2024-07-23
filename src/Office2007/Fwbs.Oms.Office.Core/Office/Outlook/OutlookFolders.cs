using System;
using System.Collections.Generic;
using System.Linq;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed class OutlookFolders :
        OutlookObject,
        MSOutlook.Folders,
        IEnumerable<OutlookFolder>
    {

        #region Fields

        private MSOutlook.Folders olfolders;
        private readonly OutlookApplication app;

        #endregion

        #region Constructors

        internal OutlookFolders(OutlookApplication app, MSOutlook.Folders folders)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (folders == null)
                throw new ArgumentNullException("folders");

            this.app = app;
            this.olfolders = folders;

            Init(olfolders);
        }


        #endregion

        #region OfficeObject


        public override OutlookApplication Application
        {
            get { return app; }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            foreach (var folder in olfolders.Cast<MSOutlook.MAPIFolder>())
                yield return Application.GetFolder(folder);

            yield break;
        }

        #endregion

        #region Overrides

        protected override void OnAttachEvents()
        {
            base.OnAttachEvents();

            if (olfolders != null)
            {
                olfolders.FolderAdd += olfolders_FolderAdd;
                olfolders.FolderRemove += olfolders_FolderRemove;
            }
        }

        protected override void OnDetachEvents()
        {
            base.OnDetachEvents();


            if (olfolders != null)
            {
                try { olfolders.FolderAdd -= olfolders_FolderAdd; }
                catch  { }
                try { olfolders.FolderRemove -= olfolders_FolderRemove; }
                catch  { }
            }

        }

        #endregion

        #region Captured Events

        private void olfolders_FolderRemove()
        {
            var ev = FolderRemove;
            if (ev != null)
                ev();
        }

        private void olfolders_FolderAdd(Microsoft.Office.Interop.Outlook.MAPIFolder Folder)
        {
            var ev = FolderAdd;
            var folder = Application.GetFolder(Folder);
            if (ev != null)
                ev(folder);
        }

        #endregion


        #region _Folders Members

        public MSOutlook.MAPIFolder Add(string Name)
        {
            return Add(Name, Type.Missing);
        }

        public MSOutlook.MAPIFolder Add(string Name, object Type)
        {
            return Application.GetFolder(olfolders.Add(Name, Type));
        }

        public MSOutlook.MAPIFolder GetFirst()
        {
            return Application.GetFolder(olfolders.GetFirst());
        }

        public MSOutlook.MAPIFolder GetLast()
        {
            return Application.GetFolder(olfolders.GetLast());
        }

        public MSOutlook.MAPIFolder GetNext()
        {
            return Application.GetFolder(olfolders.GetNext());
        }

        public MSOutlook.MAPIFolder GetPrevious()
        {
            return Application.GetFolder(olfolders.GetPrevious());
        }

        public object RawTable
        {
            get { return olfolders.RawTable; }
        }

        public void Remove(int Index)
        {
            var folder = olfolders[Index];
            olfolders.Remove(Index);
        }

        public MSOutlook.MAPIFolder this[object Index]
        {
            get 
            {
                MSOutlook.MAPIFolder folder = (Index is string) ? olfolders.GetItem((string)Index) : olfolders[Index];
                return Application.GetFolder(folder);
            }
        }

        #endregion

        #region FoldersEvents_Event Members

        public event MSOutlook.FoldersEvents_FolderAddEventHandler FolderAdd;

        public event MSOutlook.FoldersEvents_FolderChangeEventHandler FolderChange
        {
            add
            {
                olfolders.FolderChange += value;
            }
            remove
            {
                olfolders.FolderChange -= value;
            }
        }

        public event MSOutlook.FoldersEvents_FolderRemoveEventHandler FolderRemove;

        #endregion

        #region _Folders Members



        MSOutlook.Application MSOutlook._Folders.Application
        {
            get
            {
                return app;
            }
        }

        public MSOutlook.OlObjectClass Class
        {
            get { return olfolders.Class; }
        }

        public int Count
        {
            get { return olfolders.Count; }
        }

        public object Parent
        {
            get 
            {
                var folder = olfolders.Parent as MSOutlook.MAPIFolder;
                if (folder != null)
                    return Application.GetFolder(folder);

                return olfolders.Parent; 
            }
        }

        public MSOutlook.NameSpace Session
        {
            get { return Application.Session; }
        }

        #endregion

        #region IEnumerable<OutlookFolder> Members

        IEnumerator<OutlookFolder> IEnumerable<OutlookFolder>.GetEnumerator()
        {
            foreach (var exp in olfolders.Cast<MSOutlook.MAPIFolder>())
                yield return Application.GetFolder(exp);

            yield break;
        }

        #endregion
    }
}
