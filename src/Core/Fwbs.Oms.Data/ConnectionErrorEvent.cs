using System;

namespace FWBS.OMS.Data
{
    public delegate void ConnectionErrorEventHandler(object sender, ConnectionErrorEventArgs e);

    public class ConnectionErrorEventArgs : System.ComponentModel.CancelEventArgs
    {
        private readonly Exception _e;

        public ConnectionErrorEventArgs(bool cancel, Exception e) : base(cancel)
        {
            _e = e;
        }
        public Exception ConnectionException
        {
            get
            {
                return _e;
            }
        }
    }
}
