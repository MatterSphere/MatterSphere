using System;

namespace Fwbs.Documents.Preview.Handlers
{
    public interface IPreviewHandlerFactory
    {
        Guid ID { get; }

        IPreviewHandler CreateHandler();
    }
}
