using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using FWBS.OMS.ReportingServices;
using FWBS.OMS.UI.Factory;

namespace FWBS.OMS.UI.Windows.Reports
{
    /// <summary>
    /// Summary description for Services.
    /// </summary>
    public class Services
    {
        internal const string ChannelName = "ipc://Fwbs.MatterCentre.Server.{0}";
        internal const string ExeName = "omsexternalreports.exe";

        private Services()
        {

        }

        public static void OpenReport(string reportCode)
        {
            OpenReport(reportCode, null, null, false, false, Size.Empty);
        }

        public static void OpenReport(string reportCode, object parent)
        {
            OpenReport(reportCode, parent, null, false, false, Size.Empty);
        }

        public static void OpenReport(string reportCode, object parent, FWBS.Common.KeyValueCollection param, bool runNow)
        {
            OpenReport(reportCode, parent, param, runNow, false, Size.Empty);
        }

        public static void OpenReport(string reportCode, object parent, FWBS.Common.KeyValueCollection param, bool runNow, bool printNow, Size reportSize)
        {
            System.Diagnostics.Trace.WriteLine("EXTRPT: Creating IpcChannel");
            IpcChannel chan = new IpcChannel(String.Format("Fwbs.MatterCentre.Client.{0}", Process.GetCurrentProcess().SessionId));
            try
            {
                System.Diagnostics.Trace.WriteLine("EXTRPT: Registering IpcChannel");
                ChannelServices.RegisterChannel(chan, true);
                OMSObjectFactoryItem fiparent = null;
                if (parent != null)
                {
                    System.Diagnostics.Trace.WriteLine("EXTRPT: Creating Serializable OMS Object to pass as Parent");
                    var typename = parent.GetType().Name;
                    fiparent = OMSObjectFactory.CreateFactoryItem(parent);
                    System.Diagnostics.Trace.WriteLine("EXTRPT: Serialized OMS Object Successful");
                }
                ReportingStartupCommand command;
                System.Diagnostics.Trace.WriteLine("EXTRPT: Starting Call to RPC Server");
                if (RPCHelper.CreateRPCObject<ReportingStartupCommand>(ExeName, String.Format(ChannelName,Process.GetCurrentProcess().SessionId), out command))
                {
                    IntPtr owner = IntPtr.Zero;

                    System.Diagnostics.Trace.WriteLine("EXTRPT: Executing Report");
                    command.Execute(fiparent, reportCode, param, runNow, printNow, owner);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
            finally
            {
                ChannelServices.UnregisterChannel(chan);
            }
        }

        public static void OpenReportingServerReport(string ReportCode, object Parent, FWBS.Common.KeyValueCollection param, bool RunNow)
        {
            try
            {
                using (frmOpenReportsRS fop = new frmOpenReportsRS())
                {
                    fop.ucReportsViewRS1.OpenReport(SSRSConnect.ReportServerIP, ReportCode, Parent, param);
                    fop.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                throw new OMSException2("ERRRPTNOTFND", "Report '%1%' cannot be found ...", ex, true, ReportCode);
            }
        }
    }
}
