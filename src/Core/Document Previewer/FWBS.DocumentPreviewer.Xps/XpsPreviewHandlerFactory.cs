using System;

namespace Fwbs.Documents.Preview.Xps
{
    using System.ComponentModel.Composition;
    using Handlers;

    [Export("XPS", typeof(IPreviewHandlerFactory))]
    public class XpsPreviewHandlerFactory : IPreviewHandlerFactory 
    {
        public const string ClassID = "50CEA406-56FF-4208-8F5B-6644DB9D3C3D";

        public Guid ID
        {
            get
            {
                return new Guid(ClassID);
            }

        }

        public IPreviewHandler CreateHandler()
        {
            return new XpsPreviewHandlerControl();
        }
    }
}
