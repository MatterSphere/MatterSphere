using System;

namespace Fwbs.Office.Outlook
{
    internal static class OutlookConstants
    {
        public static readonly DateTime MAX_DATE = new DateTime(4501, 01, 01);
        public const int MAX_FIELD_NAME_LEN = 65;

        public const string LONG_FIELD_NAME_PREFIX = "_____";
        public const string LONG_FIELD_NAME_SUFFIX = "_____";
        public const string LONG_FIELD_NAME_MARKER = "?";
    }
}
