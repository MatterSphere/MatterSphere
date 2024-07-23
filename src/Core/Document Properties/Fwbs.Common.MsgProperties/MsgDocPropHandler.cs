using Fwbs.Framework.ComponentModel.Composition;

namespace Fwbs.Documents
{
    [Export(typeof(IDocPropHandler))]
    public sealed class MsgDocPropHandler : IDocPropHandler
    {
        #region IDocPropHandler Members

        public bool Handles(System.IO.FileInfo file)
        {
            if (file == null)
                return false;

            switch (file.Extension.ToUpperInvariant())
            {
                case ".MSG":
                case ".OFT":
                    return true;
            }

            return false;
        }

        public IRawDocument CreateDocument(System.IO.FileInfo file)
        {
            return new MsgDocument();
        }

        #endregion
    }
}
