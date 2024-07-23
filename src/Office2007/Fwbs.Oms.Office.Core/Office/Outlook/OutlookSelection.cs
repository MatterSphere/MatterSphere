using System;
using System.Collections.Generic;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed partial class OutlookSelection : IDisposable, IOutlookItems, MSOutlook.Selection
    {
        #region Fields

        private ChunkedItems chunks;
        private OutlookApplication application;
        private object parent;

        #endregion

        #region Constructors

        public OutlookSelection(OutlookApplication application, MSOutlook.Selection selection)
        {
            if (application == null)
                throw new ArgumentNullException("application");

            if (selection == null)
                throw new ArgumentNullException("application");

            this.application = application;
            this.parent = selection.Parent;
            this.chunks = ChunkedItems.Create(application, selection, application.Settings.Memory.MultipleItemChunkSize);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            application.AutoCleanup();
        }

        #endregion

        #region IEnumerable<OLItem> Members

        public IEnumerator<OutlookItem> GetEnumerator()
        {
            return chunks.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion


        #region IOLItems Members

        public int Count
        {
            get
            {
                return chunks.Count;
            }
        }

        public OutlookItem this[int index]
        {
            get
            {
                index--;

                return chunks[index];
            }
        }

        public OutlookApplication Application
        {
            get
            {
                return application;
            }
        }

        #endregion

        #region Selection Members

        MSOutlook.Application Microsoft.Office.Interop.Outlook.Selection.Application
        {
            get { return Application; }
        }

        public MSOutlook.OlObjectClass Class
        {
            get {return MSOutlook.OlObjectClass.olSelection; }
        }

        public object Parent
        {
            get { return parent;  }
        }

        public MSOutlook.NameSpace Session
        {
            get { return application.Session; }
        }

        public object this[object Index]
        {
            get 
            {
                return this[(int)Index];
            }
        }

        #endregion
    }

}