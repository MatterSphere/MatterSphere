using System;

namespace Fwbs.Documents.Preview.Zip
{
    using System.ComponentModel.Composition;
    using Handlers;

    [Export("ZIP", typeof(IPreviewHandlerFactory))]
    public class ZipPreviewHandlerFactory : IPreviewHandlerFactory 
    {
        public const string ClassID = "9BBEABFD-6145-4f2e-B66C-F55A49BDB8F8";

        public Guid ID
        {
            get
            {
                return new Guid(ClassID);
            }

        }

        public IPreviewHandler CreateHandler()
        {
            return new ZipPreviewHandlerControl();
        }
    }
}
