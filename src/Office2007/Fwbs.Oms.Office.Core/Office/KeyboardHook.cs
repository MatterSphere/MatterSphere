using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Fwbs.Office.Hooks
{
    /// <summary>
    /// This class is a replacement for Fwbs.WinFinder.Hooks.KeyboardHook.
    /// Low level keyboard hook WH_KEYBOARD_LL doesn't work in Office 2013 and above, so use WH_KEYBOARD instead.
    /// </summary>
    sealed class KeyboardHook : IDisposable
    {
        private IntPtr _hHook;
        private HookProc _hookProc;

        public event KeyboardEventHandler KeyDown;
        public event KeyboardEventHandler KeyUp;

        public KeyboardHook()
        {
            _hookProc = new HookProc(KeyboardProc);
        }

        public void Dispose()
        {
            UnInstall();
            _hookProc = null;
        }

        public void Install()
        {
            UnInstall();
#pragma warning disable 0618
            _hHook = SetWindowsHookEx(WH_KEYBOARD, _hookProc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
#pragma warning restore 0618
        }

        public void UnInstall()
        {
            if (_hHook != IntPtr.Zero)
            {
                UnhookWindowsHookEx(_hHook);
                _hHook = IntPtr.Zero;
            }
        }

        private IntPtr KeyboardProc(int code, IntPtr wParam, UIntPtr lParam)
        {
            if (code == 0)
            {
                KeyboardEventArgs args = new KeyboardEventArgs(GetMessageTime(), wParam, lParam);
                ((args.Flags & ExtendedKeyFlag.LLKHF_UP) == 0 ? KeyDown : KeyUp)?.Invoke(this, args);

                if (args.Handled)
                    return (IntPtr)1;
            }

            return CallNextHookEx(_hHook, code, wParam, lParam);
        }

        public bool IsPressed(Keys key)
        {
            return GetAsyncKeyState(key) != 0;
        }

        #region NativeMethods

        private const int WH_KEYBOARD = 2;
        private delegate IntPtr HookProc(int code, IntPtr wParam, UIntPtr lParam);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int dwThreadID);

        [DllImport("User32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wParam, UIntPtr lParam);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern int GetMessageTime();

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern short GetAsyncKeyState(Keys vKey);

        #endregion NativeMethods
    }

    delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

    sealed class KeyboardEventArgs : System.ComponentModel.HandledEventArgs
    {
        internal KeyboardEventArgs(int time, IntPtr wParam, UIntPtr lParam)
        {
            Time = time;
            Key = (Keys)wParam;
            Flags = (ExtendedKeyFlag)((lParam.ToUInt32() >> 24) & 0xA1);
        }

        public Keys Key { get; private set; }

        public ExtendedKeyFlag Flags { get; private set; }

        public int Time { get; private set;  }
    }

    // Needed for interchangeability of Fwbs.WinFinder.Hooks.KeyboardHook and Fwbs.Office.Hooks.KeyboardHook code.
    [Flags]
    enum ExtendedKeyFlag : int
    {
        LLKHF_EXTENDED = 0x01,
        LLKHF_ALTDOWN = 0x20,
        LLKHF_UP = 0x80
    }
}
