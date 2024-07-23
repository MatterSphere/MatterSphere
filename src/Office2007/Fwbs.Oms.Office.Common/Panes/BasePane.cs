using System;

namespace Fwbs.Oms.Office.Common.Panes
{
    using FWBS.OMS;
    using FWBS.OMS.UI.Windows;

    public class BasePane : System.Windows.Forms.UserControl
    {
        protected static bool SwitchDpiContext { get; private set; }

        #region Methods

        public static T Create<T>(OfficeOMSAddin addin, string command, object context, bool force)
            where T : BasePane
        {
            if (addin == null)
                return null;

            foreach (Microsoft.Office.Tools.CustomTaskPane pane in addin.Panes)
            {
                if (pane.Control.GetType() == typeof(T) && pane.Window == context)
                {
                    if (force)
                    {
                        pane.Dispose();
                        break;
                    }
                    else
                    {
                        return (T)pane.Control;
                    }
                }
            }

            // Office 2013+ Task Panes are System Aware
            SwitchDpiContext = addin.OMSApplication._dpiAwareness > 0 && addin.OMSApplication.ApplicationVersion > 14;

            using (DPIContextBlock contextBlock = SwitchDpiContext ? new DPIContextBlock(DPIAwareness.DPI_AWARENESS.SYSTEM_AWARE) : null)
            {
                T obj = Activator.CreateInstance<T>();
                obj.Initialise(addin, command);
                return obj;
            }
        }


        private void Initialise(OfficeOMSAddin addin, string command)
        {
            if (addin == null)
                throw new ArgumentNullException("addin");

            this.addin = addin;
            this.panes = addin.Panes;
            this.Command = command;
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        public void Refresh(object activeDoc)
        {
            if (Session.CurrentSession.IsLoggedIn && Panes != null)
            {
                InternalRefresh(activeDoc);
            }
            else if (Pane != null)
            {
                Pane.Visible = false;
                Panes.Remove(Pane);
                Pane.Dispose();
                Pane = null;
            }

            base.Refresh();
        }

        protected virtual void InternalRefresh(object activeDoc) { }

        private void OnVisibleChanged(object sender, EventArgs e)
        {
            Visible = pane.Visible;
            OnVisibleChanged();
        }

        protected virtual void OnVisibleChanged()
        {
        }

        #endregion

        #region Properties

        private OfficeOMSAddin addin = null;
        public OfficeOMSAddin Addin
        {
            get
            {
                return addin;
            }
        }

        private Microsoft.Office.Tools.CustomTaskPane pane = null;
        public Microsoft.Office.Tools.CustomTaskPane Pane
        {
            get
            {
                return pane;
            }
            protected set
            {
                if (pane != null)
                    pane.VisibleChanged -= new EventHandler(OnVisibleChanged);
                
                pane = value;
                
                if (pane != null)
                    pane.VisibleChanged += new EventHandler(OnVisibleChanged);
            }
        }

        private Microsoft.Office.Tools.CustomTaskPaneCollection panes = null;
        public Microsoft.Office.Tools.CustomTaskPaneCollection Panes
        {
            get
            {
                return panes;
            }
        }

        public string Command { get; private set; }

        private bool visible;
        new public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                if (panes == null)
                {
                    visible = false;
                    return;
                }

                visible = value;

                if (value == false)
                {
                    if (pane != null)
                    {
                        if (pane.Visible)
                            pane.Visible = false;
                    }
                }
                else
                {
                    if (pane == null)
                        Refresh(this.Addin.Application);

                    if (pane != null)
                    {
                        if (pane.Visible == false)
                        {
                            Refresh(this.Addin.Application);
                            pane.Visible = true;
                        }
                    }
                }

                if (pane !=null)
                    visible = pane.Visible;
            }
        }

        #endregion

        #region IDisposable Members

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (pane != null)
                    {
                        var ctp = pane;
                        Pane = null;
                        panes.Remove(ctp);
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
