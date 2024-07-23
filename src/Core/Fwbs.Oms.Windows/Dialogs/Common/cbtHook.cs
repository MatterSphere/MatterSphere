using System;
using System.Runtime.InteropServices;
using System.Text;

namespace FWBS.OMS.UI.Dialogs.Common
{
    public class LocalCbtHook : LocalWindowsHook
    {
        protected IntPtr m_hwnd = IntPtr.Zero;

        protected string m_title = "";

        protected string m_class = "";

        protected bool m_isDialog = false;

        public LocalCbtHook()
            : base(HookType.WH_CBT)
        {
            base.HookInvoked += new LocalWindowsHook.HookEventHandler(this.CbtHookInvoked);
        }

        public LocalCbtHook(LocalWindowsHook.HookProc func)
            : base(HookType.WH_CBT, func)
        {
            base.HookInvoked += new LocalWindowsHook.HookEventHandler(this.CbtHookInvoked);
        }

        private void CbtHookInvoked(object sender, HookEventArgs e)
        {
            CbtHookAction hookCode = (CbtHookAction)e.HookCode;
            IntPtr intPtr = e.wParam;
            IntPtr intPtr1 = e.lParam;
            switch (hookCode)
            {
                case CbtHookAction.HCBT_CREATEWND:
                    {
                        this.HandleCreateWndEvent(intPtr, intPtr1);
                        break;
                    }
                case CbtHookAction.HCBT_DESTROYWND:
                    {
                        this.HandleDestroyWndEvent(intPtr, intPtr1);
                        break;
                    }
                case CbtHookAction.HCBT_ACTIVATE:
                    {
                        this.HandleActivateEvent(intPtr, intPtr1);
                        break;
                    }
            }
        }

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        protected static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        protected static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int nMaxCount);

        private void HandleActivateEvent(IntPtr wParam, IntPtr lParam)
        {
            this.UpdateWindowData(wParam);
            this.OnWindowActivated();
        }

        private void HandleCreateWndEvent(IntPtr wParam, IntPtr lParam)
        {
            this.UpdateWindowData(wParam);
            this.OnWindowCreated();
        }

        private void HandleDestroyWndEvent(IntPtr wParam, IntPtr lParam)
        {
            this.UpdateWindowData(wParam);
            this.OnWindowDestroyed();
        }

        protected virtual void OnWindowActivated()
        {
            if (this.WindowActivated != null)
            {
                CbtEventArgs cbtEventArg = new CbtEventArgs();
                this.PrepareEventData(cbtEventArg);
                this.WindowActivated(this, cbtEventArg);
            }
        }

        protected virtual void OnWindowCreated()
        {
            if (this.WindowCreated != null)
            {
                CbtEventArgs cbtEventArg = new CbtEventArgs();
                this.PrepareEventData(cbtEventArg);
                this.WindowCreated(this, cbtEventArg);
            }
        }

        protected virtual void OnWindowDestroyed()
        {
            if (this.WindowDestroyed != null)
            {
                CbtEventArgs cbtEventArg = new CbtEventArgs();
                this.PrepareEventData(cbtEventArg);
                this.WindowDestroyed(this, cbtEventArg);
            }
        }

        private void PrepareEventData(CbtEventArgs e)
        {
            e.Handle = this.m_hwnd;
            e.Title = this.m_title;
            e.ClassName = this.m_class;
            e.IsDialogWindow = this.m_isDialog;
        }

        private void UpdateWindowData(IntPtr wParam)
        {
            this.m_hwnd = wParam;
            StringBuilder stringBuilder = new StringBuilder() { Capacity = 40 };
            LocalCbtHook.GetClassName(this.m_hwnd, stringBuilder, stringBuilder.Capacity);
            this.m_class = stringBuilder.ToString();
            StringBuilder stringBuilder1 = new StringBuilder() { Capacity = 256 };
            LocalCbtHook.GetWindowText(this.m_hwnd, stringBuilder1, stringBuilder1.Capacity);
            this.m_title = stringBuilder1.ToString();
            this.m_isDialog = this.m_class == "#32770";
        }

        public event LocalCbtHook.CbtEventHandler WindowActivated;

        public event LocalCbtHook.CbtEventHandler WindowCreated;

        public event LocalCbtHook.CbtEventHandler WindowDestroyed;

        public delegate void CbtEventHandler(object sender, CbtEventArgs e);
    }


    public class CbtEventArgs : EventArgs
    {
        public IntPtr Handle;

        public string Title;

        public string ClassName;

        public bool IsDialogWindow;

        public CbtEventArgs()
        {
        }
    }


    public enum CbtHookAction
    {
        HCBT_MOVESIZE,
        HCBT_MINMAX,
        HCBT_QS,
        HCBT_CREATEWND,
        HCBT_DESTROYWND,
        HCBT_ACTIVATE,
        HCBT_CLICKSKIPPED,
        HCBT_KEYSKIPPED,
        HCBT_SYSCOMMAND,
        HCBT_SETFOCUS
    }


    public class HookEventArgs : EventArgs
    {
        public int HookCode;

        public IntPtr wParam;

        public IntPtr lParam;

        public HookEventArgs()
        {
        }
    }


    public enum HookType
    {
        WH_JOURNALRECORD,
        WH_JOURNALPLAYBACK,
        WH_KEYBOARD,
        WH_GETMESSAGE,
        WH_CALLWNDPROC,
        WH_CBT,
        WH_SYSMSGFILTER,
        WH_MOUSE,
        WH_HARDWARE,
        WH_DEBUG,
        WH_SHELL,
        WH_FOREGROUNDIDLE,
        WH_CALLWNDPROCRET,
        WH_KEYBOARD_LL,
        WH_MOUSE_LL
    }


    public class LocalWindowsHook
    {
        protected IntPtr m_hhook = IntPtr.Zero;

        protected LocalWindowsHook.HookProc m_filterFunc = null;

        protected HookType m_hookType;

        public LocalWindowsHook(HookType hook)
        {
            this.m_hookType = hook;
            this.m_filterFunc = new LocalWindowsHook.HookProc(this.CoreHookProc);
        }

        public LocalWindowsHook(HookType hook, LocalWindowsHook.HookProc func)
        {
            this.m_hookType = hook;
            this.m_filterFunc = func;
        }

        [DllImport("User32.dll", ExactSpelling = true)]
        protected static extern IntPtr CallNextHookEx(IntPtr hhook, int code, IntPtr wParam, IntPtr lParam);

        protected IntPtr CoreHookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                HookEventArgs hookEventArg = new HookEventArgs()
                {
                    HookCode = code,
                    wParam = wParam,
                    lParam = lParam
                };
                this.OnHookInvoked(hookEventArg);
            }

            return LocalWindowsHook.CallNextHookEx(this.m_hhook, code, wParam, lParam);
        }

        public void Install()
        {
            Uninstall();
#pragma warning disable 0618
            this.m_hhook = LocalWindowsHook.SetWindowsHookEx(this.m_hookType, this.m_filterFunc, IntPtr.Zero, AppDomain.GetCurrentThreadId());
#pragma warning restore 0618
        }

        protected void OnHookInvoked(HookEventArgs e)
        {
            if (this.HookInvoked != null)
            {
                this.HookInvoked(this, e);
            }
        }

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        protected static extern IntPtr SetWindowsHookEx(HookType code, LocalWindowsHook.HookProc func, IntPtr hInstance, int threadID);

        [DllImport("User32.dll", SetLastError = true, ExactSpelling = true)]
        protected static extern bool UnhookWindowsHookEx(IntPtr hhook);

        public void Uninstall()
        {
            if (this.m_hhook != IntPtr.Zero)
            {
                LocalWindowsHook.UnhookWindowsHookEx(this.m_hhook);
                this.m_hhook = IntPtr.Zero;
            }
        }

        public event LocalWindowsHook.HookEventHandler HookInvoked;

        public delegate void HookEventHandler(object sender, HookEventArgs e);

        public delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);
    }




}
