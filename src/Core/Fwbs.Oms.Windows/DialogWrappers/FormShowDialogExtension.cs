using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FWBS.OMS.UI.DialogWrappers
{
    public static class FormShowDialogExtension
    {
        /// <summary>
        /// Show the From as a Semi-Modal Dialog.
        /// </summary>
        /// <param name="form">The form to be shown.</param>
        /// <param name="owner">Form's owner.</param>
        /// <returns>DialogResult</returns>
        public static DialogResult ShowModal(this Form form, IWin32Window owner)
        {
            DialogResult result = DialogResult.Cancel;
            owner = CheckOwner(owner);
            var formClosedEvent = new System.Threading.ManualResetEvent(false);
            try
            {
                form.FormClosed += (s, e) => { formClosedEvent.Set(); };
                ShowModeless(form, owner);
                EnableWindow(owner.Handle, false);
                WaitWithMessageLoop(formClosedEvent.SafeWaitHandle);
                result = form.DialogResult;
            }
            catch
            {
                formClosedEvent.Set();
            }
            finally
            {
                try { EnableWindow(owner.Handle, true); }
                catch { result = DialogResult.Ignore; }
            }
            formClosedEvent.Close();
            return result;
        }

        /// <summary>
        /// Show the From as a Modeless Dialog.
        /// </summary>
        /// <param name="form">The form to be shown.</param>
        /// <param name="owner">Form's owner.</param>
        public static void ShowModeless(this Form form, IWin32Window owner)
        {
            owner = CheckOwner(owner);
            Windows.Services.MainWindow.AddOwnedForm(form);
            if (IntPtr.Size == 8)
                SetWindowLongPtr(form.Handle, GWL_HWNDPARENT, owner.Handle);
            else
                SetWindowLong(form.Handle, GWL_HWNDPARENT, owner.Handle.ToInt32());
            form.Show(owner);
        }

        private static IWin32Window CheckOwner(IWin32Window owner)
        {
            if (!(owner is Form))
            {
                Control ctrl = null;
                if (owner is ContainerControl)
                    ctrl = ((ContainerControl)owner).ParentForm;
                else if (owner is Control)
                    ctrl = ((Control)owner).TopLevelControl;
                if (ctrl != null)
                    owner = ctrl;
            }
            return owner;
        }

        private static bool WaitWithMessageLoop(SafeHandle handle)
        {
            uint dwRet;
            IntPtr[] pHandles = new IntPtr[] { handle.DangerousGetHandle() };

            while (true)
            {
                dwRet = MsgWaitForMultipleObjects(1, pHandles, false, 0xFFFFFFFF/*INFINITE*/, 0x04FF/*QS_ALLINPUT*/);
                if (dwRet == 0)         // The event was signaled
                    return true;
                else if (dwRet == 1)    // Windows message was received
                    Application.DoEvents();
                else                    // Timeout or something else happened
                    return false;
            }
        }

        private const int GWL_HWNDPARENT = -8;

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("User32.dll", ExactSpelling = true)]
        private static extern bool EnableWindow(IntPtr hWnd, bool bEnable);

        [DllImport("User32.dll", SetLastError = true, ExactSpelling = true)]
        private static extern uint MsgWaitForMultipleObjects(int nCount, IntPtr[] pHandles, bool fWaitAll, uint dwMilliseconds, uint dwWakeMask);
    }
}