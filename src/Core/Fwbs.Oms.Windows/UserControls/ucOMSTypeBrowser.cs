using System;
using System.Windows.Forms;
using FWBS.OMS.Interfaces;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls
{
    public partial class ucOMSTypeBrowser : UserControl
    {
        public ucOMSTypeBrowser()
        {
            InitializeComponent();
            if (Session.CurrentSession.IsLoggedIn)
            {
                btnDefault.Text = Session.CurrentSession.Resources.GetResource("DEFVIEW", "Default", "").Text;
                btnResetView.Text = Session.CurrentSession.Resources.GetResource("RESETVIEW", "Reset View", "").Text;
            }
        }

        private IOMSType m_current;
        private ucOMSTypeEmbeded embeded = null;
        private EnquiryForm form = null;

        public bool Connect(IOMSType current, string defaulttab)
        {
            if (current == null)
                throw new ArgumentNullException("current");

            bool connected = Connect(current);
            if (connected)
                embeded.SetTabPage(defaulttab);
            return connected;
        }

        public bool Connect(IOMSType current)
        {
            if (current == null)
                throw new ArgumentNullException("current");

            if (form != null)
            {
                if (form.IsDirty)
                {
                    DialogResult d = FWBS.OMS.UI.Windows.MessageBox.ShowYesNoCancel("DIRTYDATAMSG", "Changes have been detected, would you like to save?");
                    switch (d)
                    {   
                        case DialogResult.Cancel:
                            return false;
                        case DialogResult.No:
                            break;
                        case DialogResult.Yes:
                            form.UpdateItem();
                            break;
                        default:
                            break;
                    }
                }
                this.Controls.Remove(form);
                form.Dispose();
                form = null;
            }
            else if (embeded != null)
            {
                if (embeded.CanClose(false))
                {
                    this.Controls.Remove(embeded);
                    embeded.Dispose();
                    embeded = null;
                }
                else
                    return false;
            }

            embeded = new ucOMSTypeEmbeded();
            embeded.Dock = DockStyle.Fill;
            this.Controls.Add(embeded);
            embeded.BringToFront();
            m_current = current;

            embeded.Connect(current);
            return true;
        }

        public bool Connect(string custom, IOMSType current)
        {
            if (String.IsNullOrEmpty(custom))
                throw new ArgumentNullException("custom");

            if (form != null)
            {
                if (form.IsDirty)
                {
                    DialogResult d = FWBS.OMS.UI.Windows.MessageBox.ShowYesNoCancel("DIRTYDATAMSG", "Changes have been detected, would you like to save?");
                    switch (d)
                    {
                        case DialogResult.Cancel:
                            return false;
                        case DialogResult.No:
                            break;
                        case DialogResult.Yes:
                            form.UpdateItem();
                            break;
                        default:
                            break;
                    }
                }
                this.Controls.Remove(form);
                form.Dispose();
                form = null;
            }
            else if (embeded != null)
            {
                if (embeded.CanClose(false))
                {
                    this.Controls.Remove(embeded);
                    embeded.Dispose();
                    embeded = null;
                }
                else
                    return false;
            }


            form = new EnquiryForm();
            form.Enquiry = EnquiryEngine.Enquiry.GetEnquiry(custom, current, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, new FWBS.Common.KeyValueCollection());
            form.Dock = DockStyle.Fill;
            this.Controls.Add(form);
            form.BringToFront();
            return true;
        }

        public IOMSType CurrentType
        {
            get
            {
                return m_current;
            }
        }

        public string TypeCode
        {
            get
            {
                return cmbBrowser.Type;
            }
            set
            {
                cmbBrowser.Type = value;
            }
        }

        public string BrowserSelectedValue
        {
            get
            {
                return Convert.ToString(cmbBrowser.SelectedValue);
            }
            set
            {
                cmbBrowser.SelectedValue = value;
            }
        }
        
        private void cmbBrowser_ActiveChanged(object sender, EventArgs e)
        {
            OnBrowserChanged();
        }

        public event EventHandler ResetViewClick;

        protected void OnResetViewClicked()
        {
            EventHandler rv = ResetViewClick;
            if (rv != null)
                rv(this, EventArgs.Empty);
        }

        public event EventHandler BrowserChanged;

        protected void OnBrowserChanged()
        {
            EventHandler bc = BrowserChanged;
            if (bc != null)
                bc(this, EventArgs.Empty);
        }

        bool browservisible;
        public bool BrowserVisible
        {
            get
            {
                return browservisible;
            }
            set
            {
                browservisible = value;
                ucOMSTypeBrowser_ParentChanged(this, EventArgs.Empty);
            }
        }

        bool resetviewvisible;
        public bool ResetViewVisible
        {
            get
            {
                return resetviewvisible;
            }
            set
            {
                resetviewvisible = value;
                ucOMSTypeBrowser_ParentChanged(this, EventArgs.Empty);
            }
        }

        bool defaultvisible;
        public bool DefaultVisible
        {
            get
            {
                return defaultvisible;
            }
            set
            {
                defaultvisible = value;
                ucOMSTypeBrowser_ParentChanged(this, EventArgs.Empty);
            }
        }

        private void ucOMSTypeBrowser_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent != null)
            {
                cmbBrowser.Visible = browservisible;
                btnResetView.Visible = resetviewvisible;
                btnDefault.Visible = defaultvisible;
                pnlBrowser.Visible = (browservisible || resetviewvisible || defaultvisible);
            }
        }

        private void btnResetView_Click(object sender, EventArgs e)
        {
            OnResetViewClicked();
        }

        private void btnDefault_Click(object sender, EventArgs e)
        {
            OnDefaultClick();
        }

        public event EventHandler DefaultClick;

        private void OnDefaultClick()
        {
            EventHandler ev = DefaultClick;
            if (ev != null)
                ev(this, EventArgs.Empty);
        }
    }
}
