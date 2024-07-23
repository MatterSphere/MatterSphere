using System;

namespace FWBS.OMS.Interfaces
{
    public delegate void OMSAppSavedEventHandler(IOMSApp app, OMSAppSavedEventArgs e);

    public class OMSAppSavedEventArgs : EventArgs
    {
        private object _rawdoc = null;
        private DocumentManagement.Storage.IStorageItem _item = null;

        public OMSAppSavedEventArgs(DocumentManagement.Storage.IStorageItem storeItem, object rawDocument)
        {
            _rawdoc = rawDocument;
            _item = storeItem;
        }

        public DocumentManagement.Storage.IStorageItem StoreItem
        {
            get
            {
                return _item;
            }
        }

        public object RawDocument
        {
            get
            {
                return _rawdoc;
            }
        }
    }

}
