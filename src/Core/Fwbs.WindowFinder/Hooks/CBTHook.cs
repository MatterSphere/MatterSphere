using System;

namespace Fwbs
{
    namespace WinFinder.Hooks
    {
        public sealed class CbtHook : Hook
        {
            #region Events

            public event CbtEventHandler WindowCreated;
            public event CbtEventHandler WindowDestroyed;
            public event CbtEventHandler WindowActivated;

            private void OnWindowCreated(CbtEventArgs e)
            {
                CbtEventHandler ev = WindowCreated;
                if (ev != null)
                    ev(this, e);
            }
            private void OnWindowDestroyed(CbtEventArgs e)
            {
                CbtEventHandler ev = WindowDestroyed;
                if (ev != null)
                    ev(this, e);
            }
            private void OnWindowActivated(CbtEventArgs e)
            {
                CbtEventHandler ev = WindowActivated;
                if (ev != null)
                    ev(this, e);
            }


            #endregion

            public CbtHook()
                : base(HookType.Cbt)
            {
            }

            protected override void OnHookInvoked(HookEventArgs e)
            {
                base.OnHookInvoked(e);

                if (e == null || e.Handled)
                    return;

                var args = new CbtEventArgs(e);

                CbtHookAction code = (CbtHookAction)e.HookCode;

                // Handle hook events (only a few of available actions)
                switch (code)
                {
                    case CbtHookAction.HCBT_CREATEWND:
                        OnWindowCreated(args);
                        break;
                    case CbtHookAction.HCBT_DESTROYWND:
                        OnWindowDestroyed(args);
                        break;
                    case CbtHookAction.HCBT_ACTIVATE:
                        OnWindowActivated(args);
                        break;
                }

                e.Handled = args.Handled;

                
            }
        }


        public delegate void CbtEventHandler(object sender, CbtEventArgs e);

        public class CbtEventArgs : HandledEventArgs
        {
            private readonly IntPtr handle;
            private Window win;

            public CbtEventArgs(HookEventArgs args)
            {
                if (args == null)
                    throw new ArgumentNullException("args");

                this.handle = args.Param1;
                this.win = WindowFactory.GetWindow(handle);
                this.Handled = args.Handled;
            }

            public Window Window
            {
                get
                {
                    return win;
                }
            }

            public IntPtr Handle
            {
                get
                {
                    return handle;
                }
            }


        }

        internal enum CbtHookAction : int
        {
            HCBT_MOVESIZE = 0,
            HCBT_MINMAX = 1,
            HCBT_QS = 2,
            HCBT_CREATEWND = 3,
            HCBT_DESTROYWND = 4,
            HCBT_ACTIVATE = 5,
            HCBT_CLICKSKIPPED = 6,
            HCBT_KEYSKIPPED = 7,
            HCBT_SYSCOMMAND = 8,
            HCBT_SETFOCUS = 9
        }

    }
}
