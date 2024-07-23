using System;
using System.Windows.Forms;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS.UI.Windows.Admin
{
    static class Start
    {
        internal static bool Restart = false;

        private sealed class SingleAppContext : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
        {
            public SingleAppContext()
            {
                this.IsSingleInstance = true;
                this.EnableVisualStyles = true;
                this.MainForm = new FWBS.OMS.Design.frmDesigner() { Tag = "ED" };
            }

            protected override void OnStartupNextInstance(Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs eventArgs)
            {
                eventArgs.BringToForeground = true;
                if (eventArgs.CommandLine.Count == 1)
                {
                    ((FWBS.OMS.Design.frmDesigner)this.MainForm).frmDesigner_Initialize(true, eventArgs.CommandLine[0], "", "");
                }
                base.OnStartupNextInstance(eventArgs);
            }

            protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs eventArgs)
            {
                this.MainForm.WindowState = FormWindowState.Maximized;
                if (eventArgs.CommandLine.Count == 1)
                {
                    this.MainForm.Shown += (sender, e) =>
                    {
                        ((FWBS.OMS.Design.frmDesigner)sender).frmDesigner_Initialize(true, eventArgs.CommandLine[0], "", "");
                    };
                }
                return base.OnStartup(eventArgs);
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if (Services.CheckLogin() && AdminKitPermissionCheck())
                {
                    SingleAppContext context = new SingleAppContext();
                    context.Run(args);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
            finally
            {
                Disconnect();
            }
        }

        private static bool AdminKitPermissionCheck()
        {
            var session = Session.CurrentSession;
            try
            {
                new SystemPermission(Permission.StandardTypeToString(StandardPermissionType.AdminKit)).Check();
                session.GetType().GetField("_designMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(session, true);
                return true;
            }
            catch (FWBS.OMS.Security.PermissionsException)
            {
                if (session.CurrentUser.IsInRoles("POWER") == false)
                {
                    if (session.AdvancedSecurity)
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("APROLADKIT3", "You must have the Administrator Role in Advanced Security or Power User Role to use the Admin Kit");
                    else
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("APROLADKIT", "You must have the Administrator or Power User Role to use the Admin Kit");
                }
                else if (session.CurrentPowerUserSettings.IsConfigured == false)
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("APROLADKIT4", "The Power User settings have not been configured");
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
            return false;
        }

        private static void Disconnect()
        {
            if (Session.CurrentSession.IsConnected)
            {
                LockState.UnlockObjectsByUser(Session.CurrentSession.CurrentUser.ID);
                Session.CurrentSession.Disconnect();
            }
        }
    }
}
