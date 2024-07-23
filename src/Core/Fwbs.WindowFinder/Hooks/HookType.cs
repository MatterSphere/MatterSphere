namespace Fwbs
{
    namespace WinFinder.Hooks
    {
        public enum HookType
        {
            CallWindowProc = 4,
            CallWindowProcRet = 21,
            Cbt = 5,
            Debug = 9,
            ForegroundIdle = 11,
            GetMessage = 3,
            JournalPlayback = 1,
            JournalRecord = 0,
            Keyboard = 2,
            KeyboardLL = 13,
            Mouse = 7,
            MouseLL = 14,
            MsgFilter = -1,
            Shell = 10,
            SysMsgFilter = 6
        }
    }
}