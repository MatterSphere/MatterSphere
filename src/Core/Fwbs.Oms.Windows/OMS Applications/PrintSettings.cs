namespace FWBS.OMS.UI.Windows
{
    public class PrintSettings
    {
        public PrecPrintMode Mode;      

        private bool bulkprintmode;
        private int copiestoprint;

        public int CopiesToPrint
        {
            get
            {
                return copiestoprint;
            }

            set
            {
                copiestoprint = value;

            }

        }
                

        public bool BulkPrintMode
        {

            get
            {
                return bulkprintmode;
            }


            set
            {
                bulkprintmode = value;

            }

        }



        public PrintSettings()
        {
            Mode = PrecPrintMode.None;           
        }

        public static PrintSettings Default
        {
            get
            {
                PrintSettings settings = new PrintSettings();               
                settings.Mode = PrecPrintMode.Dialog;
                settings.BulkPrintMode = false;
                settings.CopiesToPrint = 1;                                                                                                                                       
                return settings;
            }
        }

        public static PrintSettings BulkPrint
        {
            get
            {
                PrintSettings settings = new PrintSettings();
                settings.Mode = PrecPrintMode.None;
                settings.BulkPrintMode = true;
                settings.CopiesToPrint = 0;
                return settings;
            }
        }
    }
}
