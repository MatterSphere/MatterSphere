using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{

    /// <summary>
    /// Displays an error box with details with a specified error.
    /// </summary>
    public sealed class ErrorBox
    {
        private ErrorBox() { }

        /// <summary>
        /// Displays the error box with the specific exception information.
        /// </summary>
        /// <param name="exception">Exception to interigate.</param>
        public static void Show(Exception exception)
        {
            Show(null, exception);
        }

        /// <summary>
        /// Displays the error box with the specific exception information.
        /// </summary>
        /// <param name="owner">Owner window</param>
        /// <param name="exception">Exception object to validate.</param>
        public static void Show(IWin32Window owner, Exception exception)
        {
            using (frmErrorBox frm = new frmErrorBox(exception))
            {
                Form form = owner as Form;
                if (form != null && form.IsDisposed)
                    owner = null;

                frm.ShowDialog(owner);
            }
        }

        /// <summary>
        /// Displays the error box with the specific exception information.
        /// </summary>
        /// <param name="exception">Exception to interigate.</param>
        /// <param name="extratrace">Adds a Extra Trace Log to the Trace Log</param>
        public static void Show(Exception exception, System.Collections.ArrayList extratrace)
        {
            Show(null, exception, extratrace);
        }

        /// <summary>
        /// Displays the error box with the specific exception information.
        /// </summary>
        /// <param name="owner">Owner window</param>
        /// <param name="exception">Exception object to validate.</param>
        /// <param name="extratrace">Adds a Extra Trace Log to the Trace Log</param>
        public static void Show(IWin32Window owner, Exception exception, System.Collections.ArrayList extratrace)
        {
            using (frmErrorBox frm = new frmErrorBox(exception, extratrace))
            {
                frm.ShowDialog(owner);
            }
        }
    }


}
