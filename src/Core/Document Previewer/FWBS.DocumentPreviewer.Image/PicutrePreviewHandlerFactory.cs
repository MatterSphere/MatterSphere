using System;

namespace Fwbs.Documents.Preview.Image
{
    using System.ComponentModel.Composition;
    using Handlers;

    [Export("BMP", typeof(IPreviewHandlerFactory))]
    [Export("EMF", typeof(IPreviewHandlerFactory))]
    [Export("EXIF", typeof(IPreviewHandlerFactory))]
    [Export("GIF", typeof(IPreviewHandlerFactory))]
    [Export("GIFF", typeof(IPreviewHandlerFactory))]
    [Export("ICO", typeof(IPreviewHandlerFactory))]
    [Export("JPG", typeof(IPreviewHandlerFactory))]
    [Export("JPEG", typeof(IPreviewHandlerFactory))]
    [Export("TIF", typeof(IPreviewHandlerFactory))]
    [Export("TIFF", typeof(IPreviewHandlerFactory))]
    [Export("WMF", typeof(IPreviewHandlerFactory))]
    [Export("PNG", typeof(IPreviewHandlerFactory))]
    public class PicturePreviewHandlerFactory : IPreviewHandlerFactory 
    {
        public const string ClassID = "56A52F44-F1D2-4D03-975A-F7C7F2DA48FD";

        public Guid ID
        {
            get
            {
                return new Guid(ClassID);
            }
        }

        public IPreviewHandler CreateHandler()
        {
            return new PicturePreviewHandlerControl();
        }
    }
}
