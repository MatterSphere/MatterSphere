namespace Fwbs.Documents
{
    using Fwbs.Framework.ComponentModel.Composition;

    [Export(typeof(IDocPropHandler))]
    public sealed class OfficeDocPropHandler : IDocPropHandler
    {
        #region IDocPropHandler Members

        public bool Handles(System.IO.FileInfo file)
        {
            if (file == null)
                return false;

            switch (file.Extension.ToUpperInvariant())
            {
                case ".DOC":
                case ".DOCX":
                case ".DOCM":
                case ".DOT":
                case ".DOTX":
                case ".DOTM":
                case ".XLS":
                case ".XLSX":
                case ".XLSM":
                case ".XLT":
                case ".XLTX":
                case ".XLTM":
                case ".PPT":
                case ".PPTX":
                    return true;
            }

            return false;
        }

        public IRawDocument CreateDocument(System.IO.FileInfo file)
        {
            if (file == null)
                return null;

            if (DocumentInfo.IsZipFile(file))
                return new OfficeXmlDocument();
            else
                return new DSODocument();
        }

        #endregion
    }
}
