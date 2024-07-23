using FWBS.Common;

namespace FWBS.OMS.DocumentManagement
{
    public sealed class DocumentManager
    {
        private const string DocumentManagementKey = "DocumentManagement";

        private static bool DisablePrint
        {
            get
            {
                var setting = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, DocumentManagementKey, "DisablePrintManagement", false);
                return setting.ToBoolean();
            }
        }

        public static bool RefreshDocumentFields
        {
            get
            {
                var setting = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, DocumentManagementKey, "RefreshDocumentFields", true);
                return setting.ToBoolean();
            }
        }

        private static bool IsLimited
        {
            get
            {
                var setting = new ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, DocumentManagementKey, "DocumentManagementMode", "");
                return setting.GetSetting().ToString().ToUpper() == "LIMITDM";
            }
        }


        public static DocumentManagementMode Mode
        {
            get
            {
                if (!Session.CurrentSession.IsLoggedIn) // If not logged in return NotDefined, logging in can change this setting
                    return DocumentManagementMode.None;

                var mode = DocumentManagementMode.Full;

                // Registry can Override to Reduce DM Functionality

                if (IsLimited)
                {
                    mode &= ~DocumentManagementMode.Save;
                    mode &= ~DocumentManagementMode.Open;
                }

                // Check the Printing Registry for Global Disable of Printer Support and Command Overload.
                if (DisablePrint)
                {
                    mode &= ~DocumentManagementMode.Print;
                }


                return mode;
            }
        }
    }
}
