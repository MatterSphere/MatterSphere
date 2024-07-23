using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{

    /// <summary>
    /// Displays an input box.
    /// </summary>
    public sealed class InputBox
    {
        /// <summary>
        /// The Cancel Return Text
        /// </summary>
        public static readonly string CancelText = ((Char)27).ToString();

        private InputBox() { }

        /// <summary>
        /// Displays the input box with the specific information.
        /// </summary>
        /// <param name="owner">The owner form</param>
        /// <param name="prompt">The prompt to use at the top of the box.</param>
        /// <param name="title">The title of the input box.</param>
        /// <param name="defVal">The default value to use.</param>
        /// <param name="maxLength">The maximum length allowed.</param>
        /// <param name="required">A Value must be entered to ok the form</param>
        /// <param name="displayCancel">Display the cancel button (only makes a different if required)</param>
        public static string Show(IWin32Window owner, string prompt, string title, string defVal, int maxLength, bool required, bool displayCancel)
        {
            if (string.IsNullOrEmpty(title))
                title = FWBS.OMS.Global.ApplicationName;

            using (frmInputBox frmIB = new frmInputBox(prompt, defVal, required, displayCancel))
            {
                frmIB.Text = title;
                frmIB.TextBox.MaxLength = maxLength;
                frmIB.ShowDialog(owner);
                if (frmIB.DialogResult == DialogResult.OK)
                    return frmIB.TextBox.Text;
                else
                    return CancelText;
            }
        }
        
        public static string Show(IWin32Window owner, string prompt, string title, string defVal, int maxLength, bool required)
        {
            return Show(owner, prompt, title, defVal, maxLength, required, !required);
        }

        public static string Show(IWin32Window owner, string prompt, string title, string defVal)
        {
            return Show(owner, prompt, title, defVal, 0, false);
        }

        public static string Show(IWin32Window owner, string prompt, string title)
        {
            return Show(owner, prompt, title, "", 0, false);
        }

        public static string Show(IWin32Window owner, string prompt)
        {
            return Show(owner, prompt, FWBS.OMS.Global.ApplicationName, "", 0, false);
        }

        public static string Show(string prompt, string title, string defVal, int maxLength)
        {
            return Show(null, prompt, title, defVal, maxLength, false);
        }

        public static string Show(string prompt, string title, string defVal)
        {
            return Show(null, prompt, title, defVal, 0, false);
        }

        public static string Show(string prompt, string title)
        {
            return Show(null, prompt, title, "", 0, false);
        }

        public static string Show(string prompt)
        {
            return Show(null, prompt, FWBS.OMS.Global.ApplicationName, "", 0, false);
        }
    }

}
