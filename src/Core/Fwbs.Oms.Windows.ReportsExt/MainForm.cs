using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Windows.Reports
{
    public partial class MainForm : BaseForm
    {
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

       
        public MainForm()
        {
            InitializeComponent();
            IpcChannel chan = new IpcChannel(String.Format("Fwbs.MatterCentre.Server.{0}",Process.GetCurrentProcess().SessionId));
            ChannelServices.RegisterChannel(chan, true);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ReportingStartupCommand), "ReportingStartupCommand", WellKnownObjectMode.SingleCall);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(ReportAutomationCommand), "ReportAutomationCommand", WellKnownObjectMode.SingleCall);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(RPCHandShake), "RPCHandShake", WellKnownObjectMode.SingleCall);


            FWBS.OMS.Session.CurrentSession.Connected += new EventHandler(CurrentSession_Connected);
            FWBS.OMS.Session.CurrentSession.Disconnected += new EventHandler(CurrentSession_Disconnected);
            this.Text = FWBS.OMS.Global.ApplicationName;
            AssemblyCopyrightAttribute cpy = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));
            lblCopyright.Text = cpy.Copyright;
            lblFrameworkVersion.Text = System.Runtime.InteropServices.RuntimeEnvironment.GetSystemVersion();
        }

        void CurrentSession_Disconnected(object sender, EventArgs e)
        {
            var disconnectedIcon = FWBS.OMS.UI.Windows.Images.GetAdminMenuItem(51, UI.Windows.Images.IconSize.Size48);
            this.Icon = disconnectedIcon;
            this.notifyIcon1.Icon = disconnectedIcon;
        }

        void CurrentSession_Connected(object sender, EventArgs e)
        {
            var connectedIcon = FWBS.OMS.UI.Windows.Images.GetAdminMenuItem(37, UI.Windows.Images.IconSize.Size48);
            this.Icon = connectedIcon;
            this.notifyIcon1.Icon = connectedIcon;
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;
            ShowMain();
        }

        private void ShowMain()
        {
            this.Show();
            SetForegroundWindow(this.Handle);
            this.WindowState = FormWindowState.Normal;
        }

        private void timReports_Tick(object sender, EventArgs e)
        {
            timReports.Enabled = false;
            try
            {
                if (ReportAutomationParams.Reports.Count > 0)
                {
                    try
                    {
                        var par = ReportAutomationParams.Reports.Peek();
                        frmLoading loading = new frmLoading();
                        loading.Show(par);
                        Application.DoEvents();
                        if (FWBS.OMS.UI.Windows.Services.CheckLogin())
                        {
                            SetForegroundWindow(par.Handle);
                            var auto = new FWBS.OMS.UI.Windows.Reports.AutomationExecute();
                            if (par.ExportFormatType.HasValue)
                            {
                                loading.Description = "Exporting Report";
                                auto.Export(par.ReportCode, par.Param, par.Parent, par.ExportFormatType.Value, par.Destination);
                            }
                            else
                            {
                                loading.Description = "Printing Report";
                                auto.Print(par.ReportCode, par.Param, par.Parent, par.Copies, par.Margins, par.PrinterName);
                            }
                            loading.Close();
                            timShutdown.Enabled = false;
                            timShutdown.Enabled = true;
                        }
                        else
                        {
                            loading.Close();
                            timShutdown.Enabled = false;
                            timShutdown.Enabled = true;
                        }
                    }
                    finally
                    {
                        if (ReportAutomationParams.Reports.Count > 0)
                            ReportAutomationParams.Reports.Dequeue();
                    }
                }
                else if (ReportStartupParams.Reports.Count > 0)
                {
                    var par = ReportStartupParams.Reports.Dequeue();
                    frmLoading loading = new frmLoading();
                    loading.Show(par);
                    Application.DoEvents();
                    if (FWBS.OMS.UI.Windows.Services.CheckLogin())
                    {
                        SetForegroundWindow(this.Handle);
                        loading.Close();
                        var frm = FWBS.OMS.UI.Windows.Reports.ExternalServices.OpenReport(par);
                        count++;
                        frm.FormClosed += new FormClosedEventHandler(frm_FormClosed);
                        timShutdown.Enabled = false;
                    }
                    else
                    {
                        loading.Close();
                        timShutdown.Enabled = false;
                        timShutdown.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                SetForegroundWindow(this.Handle);
                ErrorBox.Show(this, ex);
            }
            finally
            {
                timReports.Enabled = true;
            }
        }

        private void frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            timShutdown.Enabled = false;
            timShutdown.Enabled = true;
        }

        private int count = 0;

        private void timShutdown_Tick(object sender, EventArgs e)
        {
            noshutdown = false;
            FWBS.OMS.Session.CurrentSession.Disconnect();
            timShutdown.Enabled = false;
            Application.Exit();
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
            else
                this.Show();
        }

        private void mnuOpen_Click(object sender, EventArgs e)
        {
            ShowMain();
        }

        private void mnuExit_Click(object sender, EventArgs e)
        {
            noshutdown = false;
            Application.Exit();   
        }

        private bool noshutdown = true;
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            e.Cancel = noshutdown;
        }
    }
}
