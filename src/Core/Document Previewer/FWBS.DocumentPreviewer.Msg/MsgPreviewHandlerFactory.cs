using System;

namespace Fwbs.Documents.Preview.Msg
{
    using System.ComponentModel.Composition;
    using Handlers;

    [Export("OFT", typeof(IPreviewHandlerFactory))]
    [Export("MSG", typeof(IPreviewHandlerFactory))]
    public class MsgPreviewHandlerFactory : IPreviewHandlerFactory
    {
        public const string ClassID = "9FA19F34-A9F3-4291-AF0C-9AC48C2D4407";

        private readonly IPreviewer previewer;

        public Guid ID
        {
            get
            {
                return new Guid(ClassID);
            }
        }

        [ImportingConstructor]
        public MsgPreviewHandlerFactory(IPreviewer previewer)
        {
            this.previewer = previewer;
        }

        public IPreviewHandler CreateHandler()
        {
            return new MsgPreviewHandlerWrapper(previewer);
        }
    }
}
