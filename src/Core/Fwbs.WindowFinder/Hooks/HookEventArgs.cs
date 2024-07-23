using System;

namespace Fwbs
{
    namespace WinFinder.Hooks
    {
        public class HandledEventArgs : EventArgs
        {
            public bool Handled { get; set; }
        }

        public class HookEventArgs : HandledEventArgs
        {
            private readonly int hookCode;

            public int HookCode
            {
                get { return hookCode; }
            }

            private readonly IntPtr param1;

            public IntPtr Param1
            {
                get { return param1; }
            }

            private readonly IntPtr param2;

            public IntPtr Param2
            {
                get { return param2; }
            }

            public HookEventArgs(int hookCode, IntPtr param1, IntPtr param2)
            {
                this.hookCode = hookCode;
                this.param1 = param1;
                this.param2 = param2;
            }


        }
    }

}
