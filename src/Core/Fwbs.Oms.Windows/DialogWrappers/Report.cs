using System;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows.Forms;
using FWBS.OMS.UI.Factory;

namespace FWBS.OMS.UI.Windows
{
    partial class Services
    {
        public sealed class Reports
        {
            private const string ChannelName = "ipc://Fwbs.MatterCentre.Server.{0}";
            private const string ExeName = "omsexternalreports.exe";

            public static void OpenReport(string reportCode)
            {
                OpenReport(reportCode, FWBS.OMS.Session.CurrentSession, null, false, null);
            }


            public static void OpenReport(string reportCode, object parent)
            {
                OpenReport(reportCode, parent, null, false, null);

            }
            
            public static void OpenReport(string reportCode, object parent, FWBS.Common.KeyValueCollection param, bool runNow)
            {
                OpenReport(reportCode, parent, param, runNow, null);
            }


            public static void OpenReport(string reportCode, object parent, FWBS.Common.KeyValueCollection param, bool runNow, IWin32Window owner)
            {
                System.Diagnostics.Trace.WriteLine("EXTRPT: Creating IpcChannel");
                IpcChannel chan = new IpcChannel(String.Format("Fwbs.MatterCentre.Client.{0}",Process.GetCurrentProcess().SessionId));
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
                        System.Diagnostics.Trace.WriteLine("EXTRPT: Serialized OMS Object Successful ");
                    } 

                    ReportingStartupCommand command;
                    System.Diagnostics.Trace.WriteLine("EXTRPT: Starting Call to RPC Server");
                    if (RPCHelper.CreateRPCObject<ReportingStartupCommand>(ExeName, String.Format(ChannelName,Process.GetCurrentProcess().SessionId), out command))
                    {
                        IntPtr handle = IntPtr.Zero;
                        if (owner != null)
                        {
                            handle = owner.Handle;
                        }
                        else
                        {
                            if (Application.OpenForms != null && Application.OpenForms.Count > 0)
                                handle = Application.OpenForms[Application.OpenForms.Count - 1].Handle;
                        }
                        System.Diagnostics.Trace.WriteLine("EXTRPT: Executing Report");
                        command.Execute(fiparent, reportCode, param, runNow, false, handle);
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
            


            public static void OpenReportingServerReport(string ReportCode, object parent, FWBS.Common.KeyValueCollection param, bool RunNow)
            {
                FWBS.Common.KeyValueCollection k = new FWBS.Common.KeyValueCollection();
                if (parent == null) parent = FWBS.OMS.Session.CurrentSession;
                k.Add("reportcode", ReportCode);
                k.Add("parent", parent);
                if (param == null)
                    k.Add("param", new FWBS.Common.KeyValueCollection());
                else
                    k.Add("param", param);
                k.Add("runnow", RunNow);
                Services.Run("CMDADVOPNRSREP", null, k);
            }



        }
    }
}
