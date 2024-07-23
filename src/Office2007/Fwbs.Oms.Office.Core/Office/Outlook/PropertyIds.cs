namespace Fwbs.Office.Outlook
{
    internal static class PropertyIds
    {
        internal const int PR_ICON_INDEX = 0x10800003;
        internal const int PR_SENDER_NAME = 0xC1A001E;
        internal const int PR_SENDER_EMAIL_ADDRESS = 0xC1F001E;
        internal const int PR_SENT_REPRESENTING_EMAIL_ADDRESS = 0x65001E;
        internal const int PR_SENT_REPRESENTING_NAME = 4325406;
        internal const int PR_MESSAGE_DELIVERY_TIME = 0xE060040;
        internal const int PR_CREATION_TIME = 0x30070040;
        internal const int PR_CLIENT_SUBMIT_TIME = 0x390040;
        internal const int PR_MESSAGE_FLAGS = 0x0e070003;
        internal const int PR_TRANSPORT_MESSAGE_HEADERS = 0x007D001E;

        internal const int PR_DISPLAY_BCC = 0x0E02001E;
        internal const int PR_DISPLAY_CC = 0x0E03001E;
        internal const int PR_DISPLAY_NAME = 0x3001001E;
        internal const int PR_DISPLAY_NAME_PREFIX = 0x3A45001E;
        internal const int PR_DISPLAY_TO = 0x0E04001E;

        internal const int PR_PARENT_ENTRYID = 0x0E090102;
        internal const int PR_STORE_ENTRYID = 0xFFB0102;
        internal const int PR_MAPPING_SIGNATURE = 0x0FF80102;

        [System.Flags]
        public enum MessageFlags
        {
            Read = 0x001,
            UnModified = 0x002,
            Submit = 0x004,
            UnSent = 0x008,
            HasAttach = 0x010,
            FromMe = 0x020,
            Associated = 0x040,
            Resend = 0x080,
            RnPending = 0x100,
            NrnPending = 0x200
        }
    }
}
