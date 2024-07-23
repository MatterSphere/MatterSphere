using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public interface IStorageItemVersionable : IStorageItem
    {

        event EventHandler<NewVersionEventArgs> NewVersion;

        IStorageItemVersion[] GetVersions();
        IStorageItemVersion GetVersion(Guid id);
        IStorageItemVersion GetVersion(string label);


        void OnNewVersion(NewVersionEventArgs e);
        IStorageItemVersion CreateVersion();
        IStorageItemVersion CreateVersion(IStorageItemVersion original);
        IStorageItemVersion CreateSubVersion(IStorageItemVersion original);
        
        void DeleteVersion(IStorageItemVersion version);

        void SetWorkingVersion(IStorageItemVersion version);
        IStorageItemVersion GetWorkingVersion();

        void SetLatestVersion(IStorageItemVersion current);
        IStorageItemVersion GetLatestVersion();
    }

    public class NewVersionEventArgs : EventArgs
    {
        private readonly IStorageItemVersion version;
        private readonly IStorageItemVersion basedOn;
        private readonly object tag;
        private readonly System.IO.FileInfo file;

        [Obsolete("Please use the constructor with the basedon parameter")]
        public NewVersionEventArgs(IStorageItemVersion version, System.IO.FileInfo file, object tag)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (version == null)
                throw new ArgumentNullException("version");

            this.basedOn = null;
            this.file = file;
            this.version = version;
            this.tag = tag;
        }

        public NewVersionEventArgs(IStorageItemVersion basedOn, IStorageItemVersion version, System.IO.FileInfo file, object tag)
        {
            if (file == null)
                throw new ArgumentNullException("file");
            if (version == null)
                throw new ArgumentNullException("version");

            this.basedOn = basedOn;
            this.file = file;
            this.version = version;
            this.tag = tag;
        }

        public IStorageItemVersion BasedOn
        {
            get
            {
                return this.basedOn;
            }
        }

        public IStorageItemVersion Version
        {
            get
            {
                return this.version;
            }
        }

        public System.IO.FileInfo File
        {
            get
            {
                return this.file;
            }
        }

        public object Tag
        {
            get
            {
                return tag;
            }
        }
    }
}
