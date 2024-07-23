using System;

namespace Fwbs.Documents.Preview.Word
{
    using System.ComponentModel.Composition;
    using Handlers;

    [Export("DOCX", typeof(IPreviewHandlerFactory))]
    [Export("DOCM", typeof(IPreviewHandlerFactory))]
    [Export("DOC", typeof(IPreviewHandlerFactory))]
    [Export("DOTX", typeof(IPreviewHandlerFactory))]
    [Export("DOTM", typeof(IPreviewHandlerFactory))]
    [Export("DOT", typeof(IPreviewHandlerFactory))]
    public class WordPreviewHandlerFactory : IPreviewHandlerFactory 
    {
        public const string ClassID = "A55997F4-7034-479F-90AC-F32603032604";

        public Guid ID
        {
            get
            {
                return new Guid(ClassID);
            }
        }

        public IPreviewHandler CreateHandler()
        {
            return new WordPreviewHandlerControl();
        }
    }
}
