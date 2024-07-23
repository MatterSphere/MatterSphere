namespace FWBS.OMS.UI.Windows
{
    public class OpenSettings
    {
        public readonly PrintSettings Printing;
        private DocOpenMode mode;
        public bool? LatestVersion;
        public bool? CheckOut;

        public OpenSettings()
        {
            Printing = PrintSettings.Default;
            Mode = DocOpenMode.Edit;
        }

        public DocOpenMode Mode
        {
            get
            {
                return mode;
            }
            set
            {
                //Make sure that view and print do not check the document
                //out when exclusive document locking setting is switched on.
                switch (value)
                {
                    case DocOpenMode.View:
                    case DocOpenMode.Print:
                        CheckOut = false;
                        break;
                }

                mode = value;
            }
        }

        public static OpenSettings Default
        {
            get
            {
                OpenSettings settings = new OpenSettings();
                settings.Printing.Mode = PrecPrintMode.Dialog;
                settings.Mode = DocOpenMode.Edit;
                return settings;
            }
        }
    }
}
