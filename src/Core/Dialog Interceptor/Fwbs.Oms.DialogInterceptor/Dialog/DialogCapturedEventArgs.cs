using System;

namespace Fwbs.Oms.DialogInterceptor
{
    public delegate void DialogCapturedEventHandler(object sender, DialogCapturedEventArgs e);

    public sealed class DialogCapturedEventArgs : EventArgs
    {
        internal DialogCapturedEventArgs(Dialog dialog)
        {
            if (dialog == null)
                throw new ArgumentNullException("dialog");

            this.dialog = dialog;
        }

        private readonly Dialog dialog;
        public Dialog Dialog
        {
            get
            {
                return dialog;
            }
        }
    }


}
