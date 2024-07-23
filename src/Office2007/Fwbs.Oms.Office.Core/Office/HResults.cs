namespace Fwbs.Office
{
    [System.Runtime.InteropServices.ComVisible(false)]
    public static class HResults
    {
        public const int E_FAIL = -2147467259;
        public const int E_UNAVAILABLE = -2147221021;
        public const int E_INVALID_GOTO = -2146823186;
        public const int E_ITEM_MISSING = -2146822454;
        public const int E_COLLECTION_ITEM_MISSING = -2146822347;
        public const int E_UNKNOWN_NAME = -2147352570;
        public const int E_OBJECT_DELETED = -2146822463;
        public const int E_INVALID_MACRO = -2147352573;
        public const int E_OPERATION_FAILED = -1871691771;

        public const int E_OBJECT_NOT_FOUND = -2147221233;          //object could not be found
        public const int E_OBJECT_DELETED_OR_MOVED = -2147221238;
        public const int E_ITEM_ALREADY_MODIFIED = -2147221239;     //items has already been changed by another instance
        public const int E_CANNOT_COPY_ITEMS = -2147352567;
        public const int E_OPERATION_ABORTED = -2147467260;
        public const int E_ARRAY_OUT_OF_BOUNDS = -2147352567;

        public const int E_DISP_UNKNOWN = -2147352570;

        public const int E_QUOTA_EXCEEDED = -2147220731;

        public const int RPC_UNAVAILABLE = -2147023174;

        public const int MAPI_E_COMPUTED = -2147221222;


        internal static bool IsMissing(System.Runtime.InteropServices.COMException comex)
        {
            return (comex.ErrorCode == E_UNKNOWN_NAME || comex.ErrorCode == E_DISP_UNKNOWN);
        }
    }
}
