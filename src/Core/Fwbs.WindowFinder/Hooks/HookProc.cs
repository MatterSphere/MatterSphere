using System;

namespace Fwbs
{
    namespace WinFinder.Hooks
    {
        public delegate IntPtr HookProc(int code, IntPtr param1, IntPtr param2);

        public delegate void HookEventHandler(object sender, HookEventArgs e);
    }
}
