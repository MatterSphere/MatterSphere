using System.ComponentModel.Composition;

namespace Fwbs.Documents
{
    [Export(typeof(IDocPropHandler))]
    public sealed class OleDocPropHandler : IDocPropHandler
    {
        #region IDocPropHandler Members

        public bool Handles(System.IO.FileInfo file)
        {
            if (file == null)
                return false;

            return true;
        }

        public IRawDocument CreateDocument(System.IO.FileInfo file)
        {
            return new DSODocument();
        }

        #endregion
    }
}
