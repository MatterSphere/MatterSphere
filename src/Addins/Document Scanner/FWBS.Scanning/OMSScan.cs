namespace FWBS.Scanning
{
    [System.Runtime.InteropServices.Guid("15573BF1-C9C5-48d8-81C9-0BB287615C9B")]
    public class OMSScan : FWBS.OMS.UI.Windows.ShellOMS
    {
        private string _previewtext = "";

        public OMSScan(string PreviewText)
        {
            _previewtext = PreviewText;
        }

        public override string ExtractPreview(object obj)
        {
            return _previewtext;
        }

        protected override FWBS.OMS.DocumentDirection GetActiveDocDirection(object obj, FWBS.OMS.Precedent precedent)
        {
            return FWBS.OMS.DocumentDirection.In;
        }
    }
}
