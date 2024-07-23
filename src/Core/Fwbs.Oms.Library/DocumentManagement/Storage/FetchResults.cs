using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public sealed class FetchResults
    {
        private System.IO.FileInfo local;
        private bool localCopy;
        private IStorageItem item;
        private object tag;
        private DateTime? cacheTime;
        private string additionalCaptionText;
        private bool newerexists;

        public FetchResults(IStorageItem item, System.IO.FileInfo localFile)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (localFile == null)
                throw new ArgumentNullException("localFile");

            this.item = item;
            this.local = localFile;
        }

        public FetchResults(IStorageItem item, object tag)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (tag == null)
                throw new ArgumentNullException("tag");

            this.item = item;
            this.tag = tag;
        }


        public System.IO.FileInfo LocalFile
        {
            get
            {
                return local;
            }
        }

        public object Tag
        {
            get
            {
                return tag;
            }
        }

        public IStorageItem Item
        {
            get
            {
                return item;
            }
        }

        public bool IsLocalCopy
        {
            get
            {
                return localCopy;
            }
            set
            {
                localCopy = value;
            }
        }

        public bool NewerExists
        {
            get
            {
                return newerexists;
            }
            set
            {
                newerexists = value;
            }
        }

        public DateTime? CachedDate
        {
            get
            {
                return cacheTime;
            }
            set
            {
                cacheTime = value;
            }
        }

        public bool HasChanged { get; set; }

        public string AdditionalCaptionText
        {
            get { return additionalCaptionText; }
            set { additionalCaptionText = value; }
        }
    }
}
