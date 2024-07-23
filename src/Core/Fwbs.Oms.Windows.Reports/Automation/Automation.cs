using System;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;
using System.Windows.Forms;
using FWBS.OMS.UI.Factory;

namespace FWBS.OMS.UI.Windows.Reports
{
    public struct aPageMargins
    {
        public int bottomMargin;
        public int leftMargin;
        public int rightMargin;
        public int topMargin;
        public aPageMargins(int l, int t, int r, int b)
        {
            bottomMargin = b;
            leftMargin = l;
            topMargin = t;
            rightMargin = r;
        }
    }

    public enum aExportFormatType
    {
        NoFormat = 0,
        CrystalReport = 1,
        RichText = 2,
        WordForWindows = 3,
        Excel = 4,
        PortableDocFormat = 5,
        HTML32 = 6,
        HTML40 = 7,
        ExcelRecord = 8,
    }

    public class Automation
    {

        public void Print(string reportCode, FWBS.Common.KeyValueCollection param, object parent, int copies, aPageMargins? margins, string printerName)
        {
            IpcChannel chan = new IpcChannel(String.Format("Fwbs.MatterCentre.Client.{0}", Process.GetCurrentProcess().SessionId));
            ChannelServices.RegisterChannel(chan, true);
            try
            {
                OMSObjectFactoryItem fiparent = null;
                if (parent != null)
                {
                    var typename = parent.GetType().Name;
                    fiparent = OMSObjectFactory.CreateFactoryItem(parent);
                }
                ReportAutomationCommand command;
                if (RPCHelper.CreateRPCObject<ReportAutomationCommand>(Services.ExeName, String.Format(Services.ChannelName, Process.GetCurrentProcess().SessionId), out command))
                {
                    IntPtr owner = IntPtr.Zero;
                    if (Application.OpenForms != null && Application.OpenForms.Count > 0)
                        owner = Application.OpenForms[Application.OpenForms.Count - 1].Handle;
                    var id = command.Print(reportCode, param, fiparent, copies, margins, printerName, owner);
                    do
                    {
                        Application.DoEvents();
                    } while (command.IsAutomationActive(id));
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

        public void Export(string reportCode, FWBS.Common.KeyValueCollection param, OMSObjectFactoryItem parent, aExportFormatType exportFormatType, string destination)
        {
            IpcChannel chan = new IpcChannel(String.Format("Fwbs.MatterCentre.Client.{0}", Process.GetCurrentProcess().SessionId));
            ChannelServices.RegisterChannel(chan, true);
            try
            {
                OMSObjectFactoryItem fiparent = null;
                if (parent != null)
                {
                    var typename = parent.GetType().Name;
                    fiparent = OMSObjectFactory.CreateFactoryItem(parent);
                }
                ReportAutomationCommand command;
                if (RPCHelper.CreateRPCObject<ReportAutomationCommand>(Services.ExeName, String.Format(Services.ChannelName, Process.GetCurrentProcess().SessionId), out command))
                {
                    IntPtr owner = IntPtr.Zero;
                    if (Application.OpenForms != null && Application.OpenForms.Count > 0)
                        owner = Application.OpenForms[Application.OpenForms.Count - 1].Handle;
                    var id = command.Export(reportCode, param, fiparent, exportFormatType, destination, owner);
                    do
                    {
                        Application.DoEvents();
                    } while (command.IsAutomationActive(id));
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
    }

}
