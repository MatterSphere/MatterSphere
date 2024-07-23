using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();

            if (DesignMode)
                return;

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Session.CurrentSession.DefaultCulture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            this.BackColor = Color.White;

            if (FWBS.OMS.Session.CurrentSession.IsLoggedIn)
            {
                if (FWBS.OMS.Session.CurrentSession.CurrentUser.RightToLeft)
                {
                    this.RightToLeft = RightToLeft.Yes;
                    this.RightToLeftLayout = true;

                }
                else
                    this.RightToLeft = RightToLeft.No;
            }

            HelpFilePathSetUp();
        }

        private void HelpFilePathSetUp()
        {
            if (string.IsNullOrEmpty(helpProvider1.HelpNamespace))
            {
                string helpPath = Session.CurrentSession.GetHelpPath(this.GetType().Name);
                if (String.IsNullOrEmpty(helpPath))
                {
                    helpProvider1.SetShowHelp(this, false);
                }
                else
                {
                    helpProvider1.HelpNamespace = helpPath;
                    helpProvider1.SetShowHelp(this, true);
                }
            }
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            float scaleFactor = (float)e.DeviceDpiNew / e.DeviceDpiOld;
            Size minimumSize = new Size(Convert.ToInt32(MinimumSize.Width * scaleFactor), Convert.ToInt32(MinimumSize.Height * scaleFactor));
            base.OnDpiChanged(e);
            MinimumSize = minimumSize;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (!Session.CurrentSession.IsShuttingDown)
                base.OnClosing(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!Session.CurrentSession.IsShuttingDown)
                base.OnFormClosing(e);
        }

        protected override void OnClosed(EventArgs e)
        {
            if (!Session.CurrentSession.IsShuttingDown)
                base.OnClosed(e);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (!Session.CurrentSession.IsShuttingDown)
                base.OnFormClosed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Session.CurrentSession.IsShuttingDown)
            {
                if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes)
                    Global.RightToLeftFormConverter(this);
                base.OnLoad(e);
            }
            else
                this.Close();
        }

        protected override void OnShown(EventArgs e)
        {
            if (!Session.CurrentSession.IsShuttingDown)
            {
                base.OnShown(e);
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            if (!Session.CurrentSession.IsShuttingDown)
                base.OnActivated(e);
        }

   
        private Dictionary<string, bool> convertedtortl = new Dictionary<string, bool>();

        public bool IsControlRightToLeft(Control ctrl)
        {
            if (convertedtortl.ContainsKey(ctrl.GetHashCode().ToString()))
                return convertedtortl[ctrl.GetHashCode().ToString()];
            else
                return false;
        }

        public void AddControlToRTL(Control ctrl)
        {
            if (!convertedtortl.ContainsKey(ctrl.GetHashCode().ToString()))
            {
                convertedtortl.Add(ctrl.GetHashCode().ToString(), true);
            }
            else
            {
                convertedtortl[ctrl.GetHashCode().ToString()] = true;
            }
        }
    }

}