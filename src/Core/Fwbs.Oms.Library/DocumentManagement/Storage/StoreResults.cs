using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public sealed class StoreResults
    {
        private IStorageItem item;
        private System.IO.FileInfo local;
        private string token;

        [Obsolete("Please use the Constructor which takes the FileInfo object")]
        public StoreResults(IStorageItem item, string token): this(item, token, null)
        {}

        public StoreResults(IStorageItem item, string token, System.IO.FileInfo local)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (token == null)
                token = String.Empty;

            this.item = item;
            this.item.Token = token;
            this.token = token;
            this.local = local;
        }

        public string Token
        {
            get
            {
                return token;
            }
        }

        public IStorageItem Item
        {
            get
            {
                return item;
            }
        }

        public System.IO.FileInfo LocalFile
        {
            get
            {
                return local;
            }
        }

        public void Update()
        {
            item.Update();
        }
    }
}
