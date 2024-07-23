using System;
using System.Diagnostics;

namespace Fwbs
{
    namespace WinFinder.Hooks
    {
        internal sealed class HookSafeHandle : Microsoft.Win32.SafeHandles.CriticalHandleZeroOrMinusOneIsInvalid
        {

            public HookSafeHandle(HookType type, HookProc callback)
            {
                using (Process process = Process.GetCurrentProcess())
                using (ProcessModule mainModule = process.MainModule)
                {
                    IntPtr h = NativeMethods.SetWindowsHookEx(type, callback, mainModule.BaseAddress, 0);
                    SetHandle(h);
                    Trace.TraceInformation("Key Hook - {0} Installed", h);
                }
            }

            protected override bool ReleaseHandle()
            {
                bool ret = NativeMethods.UnhookWindowsHookEx(handle);
                Trace.TraceInformation("Key Hook - {0} Release {1}", handle, ret ? "Success" : "Failed");
                return ret;
            }
        }
    }
}
