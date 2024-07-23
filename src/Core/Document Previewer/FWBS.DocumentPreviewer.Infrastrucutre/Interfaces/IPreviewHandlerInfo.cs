using System.Collections.Generic;

namespace Fwbs.Documents.Preview.Handlers
{
    public interface IPreviewHandlerInfo
    {
        void SetCultureData(Dictionary<string, string> cultureData);
        void SetPreviewSupport(bool fullPreviewSupport);
        void SetAdditionalProperties(Dictionary<string, string> additionalProperties);
    }
}
