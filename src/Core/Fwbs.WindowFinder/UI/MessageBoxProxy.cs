using System;
using System.Windows.Forms;

namespace Fwbs
{
    namespace WinFinder.Internal
    {

        internal class MessageBoxProxy
        {
            private Control parent;

            public MessageBoxProxy(Control parent)
            {
                this.parent = parent;
            }


            private static Boolean IsRightToLeft(Control control)
            {
                if (control.RightToLeft == RightToLeft.Inherit)
                {
                    Control parent = control.Parent;

                    while (parent != null)
                    {
                        if (parent.RightToLeft != RightToLeft.Inherit)
                        {
                            return parent.RightToLeft == RightToLeft.Yes;
                        }

                    }
                    return System.Globalization.CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;
                }
                else
                {
                    return control.RightToLeft == RightToLeft.Yes;
                }
            }



            public DialogResult Show(string text)
            {
                return Show(text,
                    string.Empty,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.None,
                    MessageBoxDefaultButton.Button1);
            }

            public DialogResult Show(string text, string caption)
            {
                return Show(text,
                    caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.None,
                    MessageBoxDefaultButton.Button1);
            }



            public DialogResult Show(string text,
                string caption,
                MessageBoxButtons buttons)
            {
                return Show(text,
                    caption,
                    buttons,
                    MessageBoxIcon.None,
                    MessageBoxDefaultButton.Button1);
            }



            public DialogResult Show(string text,
                string caption,
                MessageBoxButtons buttons,
                MessageBoxIcon icon)
            {
                return Show(text,
                    caption,
                    buttons,
                    icon,

                    MessageBoxDefaultButton.Button1);
            }



            public DialogResult Show(String text,
                String caption,
                MessageBoxButtons messageBoxButtons,
                MessageBoxIcon icon,
                MessageBoxDefaultButton defaultButton)
            {
                MessageBoxOptions options =
                    IsRightToLeft(parent) ?

                        MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading : 0;


                return System.Windows.Forms.MessageBox.Show(parent,
                    text,
                    caption,
                    messageBoxButtons,
                    icon,
                    defaultButton,
                    options);

            }



            public DialogResult Show(IWin32Window owner, string text)
            {

                return Show(owner,

                    text,

                    string.Empty,

                    MessageBoxButtons.OK,

                    MessageBoxIcon.None,

                    MessageBoxDefaultButton.Button1);

            }



            public DialogResult Show(IWin32Window owner, string text, string caption)
            {

                return Show(owner,

                    text,

                    caption,

                    MessageBoxButtons.OK,

                    MessageBoxIcon.None,

                    MessageBoxDefaultButton.Button1);

            }



            public DialogResult Show(IWin32Window owner,

                string text,

                string caption,

                MessageBoxButtons buttons)
            {

                return Show(owner,

                    text,

                    caption,

                    buttons,

                    MessageBoxIcon.None,

                    MessageBoxDefaultButton.Button1);

            }



            public DialogResult Show(IWin32Window owner,

                string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon)
            {

                return Show(owner,

                    text,

                    caption,

                    buttons,

                    icon,

                    MessageBoxDefaultButton.Button1);

            }



            public DialogResult Show(IWin32Window owner,

                string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton)
            {

                MessageBoxOptions options = parent != owner ?

                    0 : (IsRightToLeft(parent) ?

                        MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading : 0);



                return System.Windows.Forms.MessageBox.Show(owner,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options);

            }



            public DialogResult Show(string text,

                string caption, MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options)
            {

                return System.Windows.Forms.MessageBox.Show(parent,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options);

            }




            public DialogResult Show(string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options,

                bool displayHelpButton)
            {

                return System.Windows.Forms.MessageBox.Show(text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options,

                    displayHelpButton);

            }



            public DialogResult Show(string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options,

                string helpFilePath)
            {

                return System.Windows.Forms.MessageBox.Show(parent,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options,

                    helpFilePath);

            }



             public DialogResult Show(IWin32Window owner,

                string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options)
            {

                return System.Windows.Forms.MessageBox.Show(owner,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options);

            }



            public DialogResult Show(string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options,

                string helpFilePath,

                string keyword)
            {

                return System.Windows.Forms.MessageBox.Show(parent,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options,

                    helpFilePath,

                    keyword);

            }



            public DialogResult Show(string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options,

                string helpFilePath,

                HelpNavigator navigator)
            {

                return System.Windows.Forms.MessageBox.Show(parent,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options,

                    helpFilePath,

                    navigator);

            }


            public DialogResult Show(IWin32Window owner,

                string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options,

                string helpFilePath)
            {

                return Show(owner,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options,

                    helpFilePath);

            }



            public DialogResult Show(string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options,

                string helpFilePath,

                HelpNavigator navigator,

                object param)
            {

                return System.Windows.Forms.MessageBox.Show(parent,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options,

                    helpFilePath,

                    navigator,

                    param);

            }


            public DialogResult Show(IWin32Window owner,

                string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options,

                string helpFilePath,

                string keyword)
            {

                return System.Windows.Forms.MessageBox.Show(owner,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options,

                    helpFilePath,

                    keyword);

            }


            public DialogResult Show(IWin32Window owner,

                string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options,

                string helpFilePath,

                HelpNavigator navigator)
            {

                return System.Windows.Forms.MessageBox.Show(owner,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options,

                    helpFilePath,

                    navigator);

            }


            public DialogResult Show(IWin32Window owner,

                string text,

                string caption,

                MessageBoxButtons buttons,

                MessageBoxIcon icon,

                MessageBoxDefaultButton defaultButton,

                MessageBoxOptions options,

                string helpFilePath,

                HelpNavigator navigator,

                object param)
            {

                return System.Windows.Forms.MessageBox.Show(owner,

                    text,

                    caption,

                    buttons,

                    icon,

                    defaultButton,

                    options,

                    helpFilePath,

                    navigator,

                    param);

            }

        }


    }
}