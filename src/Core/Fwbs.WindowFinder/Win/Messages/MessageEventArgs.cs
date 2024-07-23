using System;

namespace Fwbs
{

    namespace WinFinder
    {
        public class MessageEventArgs : EventArgs
        {
            private readonly System.Windows.Forms.Message msg;
            public MessageEventArgs(System.Windows.Forms.Message message)
            {
                if (message == null)
                    throw new ArgumentNullException(nameof(message));

                this.msg = message;
            }

            public bool Handled { get; set; }

            public System.Windows.Forms.Message Message
            {
                get
                {
                    return msg;
                }
            }
        }
    }
}
