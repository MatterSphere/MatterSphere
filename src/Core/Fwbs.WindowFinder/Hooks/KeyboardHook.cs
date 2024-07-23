using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Fwbs
{
    namespace WinFinder.Hooks
    {
        public sealed class KeyboardHook : Hook
        {
            private const int WM_KEYUP = 0x0101;
            private const int WM_KEYDOWN = 0x0100;
            private const int WM_SYSKEYUP = 0x0105;
            private const int WM_SYSKEYDOWN = 0x0104;

            
            public event KeyboardEventHandler KeyUp;
            public event KeyboardEventHandler KeyDown;

            public KeyboardHook()
                : base(HookType.KeyboardLL)
            {
            }

            protected override void OnHookInvoked(HookEventArgs e)
            {
                base.OnHookInvoked(e);

                if (e == null || e.Handled)
                    return;

                KeyboardEventArgs args = new KeyboardEventArgs(e);

                if (e.HookCode >= 0)
                {
                    KeyboardEventHandler ev = null;

                    switch (e.Param1.ToInt32())
                    {
                        case WM_KEYUP:
                        case WM_SYSKEYUP:
                            ev = KeyUp;
                            break;
                        case WM_KEYDOWN:
                        case WM_SYSKEYDOWN:
                            ev = KeyDown;
                            break;
                    }

                    if (ev != null)
                        ev(this, args);

                    e.Handled = args.Handled;
                }


                
            }

            public bool IsPressed(Keys key)
            {
                return NativeMethods.GetAsyncKeyState(key) != 0;
            }
        }

#pragma warning disable CS0649
        [StructLayout(LayoutKind.Sequential)]
        internal struct KBDLLHOOKSTRUCT
        {
            public Keys vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public UIntPtr dwExtraInfo;
        }
#pragma warning restore CS0649

        public delegate void KeyboardEventHandler(object sender, KeyboardEventArgs e);

        public class KeyboardEventArgs : HandledEventArgs
        {
            public KeyboardEventArgs(HookEventArgs args)
            {
                if (args == null)
                    throw new ArgumentNullException("args");

                KBDLLHOOKSTRUCT data = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(args.Param2, typeof(KBDLLHOOKSTRUCT));

                this.key = data.vkCode;
                this.flags = (ExtendedKeyFlag)data.flags;
                this.time = data.time;
            }

            private readonly Keys key;
            public Keys Key
            {
                get
                {
                    return key;
                }
            }


            private readonly int time;
            public int Time
            {
                get
                {
                    return time;
                }
            }

            private readonly ExtendedKeyFlag flags;
            public ExtendedKeyFlag Flags
            {
                get
                {
                    return flags;
                }
            }

           


        }

        [Flags]
        public enum ExtendedKeyFlag : int
        {
            LLKHF_EXTENDED = 0x01,
            LLKHF_LOWER_IL_INJECTED = 0x02,
            LLKHF_INJECTED = 0x10,
            LLKHF_ALTDOWN = 0x20,
            LLKHF_UP = 0x80
        }
      
    }



}
