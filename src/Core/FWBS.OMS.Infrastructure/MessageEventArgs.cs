using System;

namespace FWBS.OMS
{
    /// <summary>
    /// Message delegate.
    /// </summary>
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    /// <summary>
    /// Message event arguments.
    /// </summary>
    public class MessageEventArgs : EventArgs
    {
        private readonly string _message;
        private readonly Exception _exception;
        private readonly MessageSeverity _severity;

        private MessageEventArgs() { }

        public MessageEventArgs(string message)
            : this(message, MessageSeverity.None)
        {
        }

        public MessageEventArgs(string message, MessageSeverity severity)
        {
            _message = message;
            _exception = null;
            _severity = severity;
        }

        public MessageEventArgs(Exception exception)
            : this(exception, MessageSeverity.Error)
        {
        }

        public MessageEventArgs(Exception exception, MessageSeverity severity)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            _message = exception.Message;
            _exception = exception;
            _severity = severity;
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public Exception Exception
        {
            get
            {
                return _exception;
            }
        }

        public MessageSeverity Severity
        {
            get
            {
                return _severity;
            }
        }

    }

    public enum MessageSeverity
    {
        None = 0,
        Information,
        Warning,
        Error

    }
}
