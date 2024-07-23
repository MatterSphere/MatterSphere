using System;

namespace Fwbs
{
    namespace WinFinder.Hooks
    {
        public abstract class Hook : IDisposable
        {
            #region Fields

            private HookSafeHandle handle;
            private HookType type;
            private HookProc callback;

            #endregion

            #region Events

            public event HookEventHandler HookInvoked;

            protected virtual void OnHookInvoked(HookEventArgs e)
            {
                HookEventHandler ev = HookInvoked;

                if (ev != null)
                    ev(this, e);
            }

            #endregion

            #region Constructors

            protected Hook(HookType type)
            {
                this.type = type;
                this.callback = new HookProc(this.CoreHookProc);
            }

            #endregion

            #region Methods

            protected IntPtr CoreHookProc(int code, IntPtr param1, IntPtr param2)
            {
                if (code < 0)
                    return CallNextHook(code, param1, param2);

                HookEventArgs args = new HookEventArgs(code, param1, param2);
                // Let clients determine what to do
                OnHookInvoked(args);

                if (args.Handled)
                    return (IntPtr)1;

                // Yield to the next hook in the chain
                return CallNextHook(code, param1, param2);
            }

            protected IntPtr CallNextHook(int code, IntPtr param1, IntPtr param2)
            {
                return NativeMethods.CallNextHookEx(handle, code, param1, param2);
            }

            public void Install()
            {
                handle = new HookSafeHandle(type, callback);
            }

            public void UnInstall()
            {
                if (handle != null)
                {
                    handle.Close();
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                UnInstall();
            }

            #endregion
        }

    }

}
