using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace Fwbs.Oms.DialogInterceptor
{
    using Fwbs.WinFinder;

    internal partial class HiddenWindow : Form
    {
        public HiddenWindow()
        {
            InitializeComponent();

            this.Text = TITLE;

            base.CreateHandle();
        }

        private const string TITLE = "{9029E4B1-6954-4b8b-B57F-7ABE954A9D4A}";
        private readonly uint WM_DLGINIT = NativeMethods.RegisterWindowMessage("WM_DLGINIT");
        private readonly uint WM_DLGSHOW = NativeMethods.RegisterWindowMessage("WM_DLGSHOW");
        private readonly uint WM_DLGDESTROYED = NativeMethods.RegisterWindowMessage("WM_DLGDESTROYED");
        private WinFinder.SafeLibraryHandle hmodule;
        private EventWaitHandle secondaryUnhookEvent;


        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            hmodule.Close();
            if (secondaryUnhookEvent != null)
            {
                secondaryUnhookEvent.Set();
                secondaryUnhookEvent.Close();
            }
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_DLGSHOW)
            {
                Window win = WindowFactory.GetWindow(m.WParam);
                Dialog dlg = DialogFactory.CreateDialog(win);
                if (dlg != null)
                    Interceptor.OnDialogCaptured(new DialogCapturedEventArgs(dlg));
            }
            else
                base.WndProc(ref m);
        }

        internal void Hook()
        {

            //ChangeWindowMessageFilter only exists on Vista onwards. ChangeWindowMessageFilter will
            //return true on earlier platforms.  ChangeWindowMessageFilter basically allows the
            //added messages to be received from lower priveleged applications using UAC and UIPI.
            if (NativeMethods.ChangeWindowMessageFilter(WM_DLGINIT, NativeMethods.MSGFLT_ADD) &&
                NativeMethods.ChangeWindowMessageFilter(WM_DLGSHOW, NativeMethods.MSGFLT_ADD) &&
                NativeMethods.ChangeWindowMessageFilter(WM_DLGDESTROYED, NativeMethods.MSGFLT_ADD))
            {

                try
                {
                    hmodule = NativeMethods.LoadLibrary(NativeMethods.PrimaryHookDLL);
                    if (hmodule.IsInvalid)
                    {
                        string localpath = System.IO.Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, NativeMethods.PrimaryHookDLL);
                        hmodule = NativeMethods.LoadLibrary(localpath);

                        if (hmodule.IsInvalid)
                        {
                            int res = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                            if (res != 0)
                                System.Runtime.InteropServices.Marshal.ThrowExceptionForHR(res);

                        }
                    }


                }
                catch (Exception ex)
                {
                    throw new HookException(Properties.Resources.ExceptionHookIsNotInstalled, ex);
                }
            }

            if (hmodule.IsInvalid)
                throw new HookException(Properties.Resources.ExceptionHookIsNotInstalled);

        }

        /// <summary>
        /// Injects secondary DialogInterceptor Hook for another bitness processes.
        /// Launches 32-bit Rundll32 child process from 64-bit parent OMS.Utils or 64-bit Rundll32 child process from 32-bit parent OMS.Utils.
        /// >rundll32.exe OMS.DialogInterceptor.Hook.dll,SetSecondaryHook UtilsPID
        /// Secondary hook will be removed when the event is signaled or OMS.Utils process exits.
        /// </summary>
        internal void SecondaryHook()
        {
            bool wow64Redir = false;
            IntPtr wow64Value = IntPtr.Zero;
            try
            {
                Process process = Process.GetCurrentProcess();
                string eventName = string.Format(@"Local\{0}.EXE:{1}", process.ProcessName.ToUpperInvariant(), (uint)process.Id);

                string rundll32 = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.Is64BitProcess ? Environment.SpecialFolder.SystemX86 : Environment.SpecialFolder.System),
                    "rundll32.exe");
                string cmdargs = string.Format("\"{0}\",SetSecondaryHook {1}",
                    System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, NativeMethods.SecondaryHookDLL), (uint)process.Id);

                secondaryUnhookEvent = new EventWaitHandle(false, EventResetMode.ManualReset, eventName);
                wow64Redir = Environment.Is64BitProcess == false && NativeMethods.Wow64DisableWow64FsRedirection(ref wow64Value);
                process = Process.Start(new ProcessStartInfo(rundll32, cmdargs) { CreateNoWindow = true, UseShellExecute = false });
            }
            catch (Exception ex)
            {
                throw new HookException(Properties.Resources.ExceptionHookIsNotInstalled, ex);
            }
            finally
            {
                if (wow64Redir)
                    NativeMethods.Wow64RevertWow64FsRedirection(wow64Value);
            }
        }
    }
}
