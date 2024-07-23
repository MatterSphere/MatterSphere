using System;

namespace Fwbs.Oms.Office.Common
{
    public delegate void ControlFlagCallbackDelegate(object sender, ControlFlagCallbackEventArgs e);
    public delegate void ControlVisibleCallbackDelegate(object sender, ControlVisibleCallbackEventArgs e);
    public delegate void ControlToggleCallbackDelegate(object sender, ControlToggleCallbackEventArgs e);
    public delegate void ControlActionCallbackDelegate(object sender, ControlActionCallbackEventArgs e);
    public delegate void ControlStringCallbackDelegate(object sender, ControlStringCallbackEventArgs e);
    public delegate void ControlDynamicMenuCallbackDelegate(object sender, ControlDynamicMenuCallbackEventArgs e);
    public delegate void ControlResourceRequestDelegate(object sender, ControlResourceRequestEventArgs e);

    public class ControlBaseActionCallbackEventArgs 
    {
        private string id;
        private string command;
        private bool handled;
        private object context;

        internal ControlBaseActionCallbackEventArgs(string id, string command, object context)
        {
            this.id = id;
            this.command = command;
            this.context = context;
            this.handled = false;
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        public string Command
        {
            get
            {
                return command;
            }
        }
        public object Context
        {
            get
            {
                return context;
            }
        }
        public bool Handled
        {
            get
            {
                return handled;
            }
            set
            {
                handled = value;
            }
        }

    }

    public class ControlActionCallbackEventArgs : ControlBaseActionCallbackEventArgs
    {
        private bool cancelDefault;

        internal ControlActionCallbackEventArgs(string id, string command, object context, bool cancelDefault = true)
            : base(id, command, context)
        {
            this.cancelDefault = cancelDefault;
        }

        public bool CancelDefault
        {
            get
            {
                return cancelDefault;
            }
            set
            {
                cancelDefault = value;
            }
        }
    }

    public class ControlToggleCallbackEventArgs : ControlBaseActionCallbackEventArgs
    {
        private bool pressed;

        internal ControlToggleCallbackEventArgs(string id, string command, object context, bool pressed) : base(id, command, context)
        {
            this.pressed = pressed;
        }

        public bool Pressed
        {
            get
            {
                return pressed;
            }
        }
    }

    public class ControlFlagCallbackEventArgs : ControlBaseActionCallbackEventArgs
    {
        private bool returnVal;

        internal ControlFlagCallbackEventArgs(string id, string command, object context)
            : base(id, command, context)
        {
            this.returnVal = false;
        }

        public bool ReturnValue
        {
            get
            {
                return returnVal;
            }
            set
            {
                returnVal = value;
            }
        }
    }

    public class ControlVisibleCallbackEventArgs : ControlFlagCallbackEventArgs
    {
        private string filter;

        internal ControlVisibleCallbackEventArgs(string id, string command, object context, string filter)
            : base(id, command, context)
        {
            this.filter = filter;
        }

        public string Filter
        {
            get
            {
                return filter;
            }
        }
    }

    public class ControlStringCallbackEventArgs : ControlBaseActionCallbackEventArgs
    {
        private string returnVal;

        internal ControlStringCallbackEventArgs(string id, string command, object context)
            : base(id, command, context)
        {
            this.returnVal = String.Empty;
        }

        public string ReturnValue
        {
            get
            {
                return returnVal;
            }
            set
            {
                returnVal = value;
            }
        }
    }

    public class ControlDynamicMenuCallbackEventArgs : ControlBaseActionCallbackEventArgs
    {
        private System.Xml.XmlElement control;

        internal ControlDynamicMenuCallbackEventArgs(string id, string command, object context, System.Xml.XmlElement control)
            : base(id, command, context)
        {
            this.control = control;
        }

        public System.Xml.XmlElement Control
        {
            get
            {
                return control;
            }
        }
    }

    public class ControlResourceRequestEventArgs : EventArgs
    {
        private string resource;
        private string config;
        private string commands;
        private string id;

        internal ControlResourceRequestEventArgs(string id)
        {
            this.id = id;
        }

        public string Id
        {
            get
            {
                return id;
            }
        }

        public string Resource
        {
            get
            {
                return resource;
            }
            set
            {
                resource = value;
            }
        }

        public string Commands
        {
            get
            {
                return commands;
            }
            set
            {
                commands= value;
            }
        }


        public string Config
        {
            get
            {
                return config;
            }
            set
            {
                config = value;
            }
        }

    }
}
