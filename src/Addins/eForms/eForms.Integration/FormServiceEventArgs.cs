using System;

namespace FWBS.OMS
{
    public enum FormServiceType { Request, Refresh, Cancel }

    public class FormServiceEventArgs : EventArgs
    {
        public FormServiceEventArgs(FormServiceType type)
        {
            this.Type = type;
        }

        public FormServiceEventArgs(FormServiceType type, Exception ex) : this(type)
        {
            this.Exception = ex;
        }

        public FormServiceEventArgs(FormServiceType type, string[] err) : this(type)
        {
            this.Errors = err;
        }

        public string[] Errors { get; private set; }
        public Exception Exception { get; private set; }
        public FormServiceType Type { get; private set; }

        public static FormServiceEventArgs EmptyRequest = new FormServiceEventArgs(FormServiceType.Request);
        public static FormServiceEventArgs EmptyRefresh = new FormServiceEventArgs(FormServiceType.Refresh);
        public static FormServiceEventArgs EmptyCancel = new FormServiceEventArgs(FormServiceType.Cancel);
    }
}
