using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class RPCHelper
    {

        public static bool CreateRPCObject<T>(string exename, string channelName, out T rpcobject)
        {
            if (String.IsNullOrEmpty(exename))
                throw new ArgumentNullException("exename");
            if (String.IsNullOrEmpty(channelName))
                throw new ArgumentNullException("channelName");

            rpcobject = default(T);
            var retries = 0;
            string processname = exename.Replace(".exe", "");
            do
            {
                try
                {
                    System.Diagnostics.Trace.WriteLine("EXTRPT: Hello");
                    var type = typeof(RPCHandShake);
                    var test = (RPCHandShake)Activator.GetObject(type, String.Format("{0}/{1}", channelName, type.Name));
                    test.Hello();
                    System.Diagnostics.Trace.WriteLine("EXTRPT: Hello Successful ...");

                    type = typeof(T);
                    rpcobject = (T)Activator.GetObject(typeof(T), String.Format("{0}/{1}", channelName, type.Name));
                    return true;
                }
                catch (System.Runtime.Remoting.RemotingException ex)
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("EXTRPT: No Answer ... {0}", ex.Message));
                    if (AlreadyRunning(processname) == false)
                    {
                        System.Diagnostics.Trace.WriteLine("EXTRPT: Loading External Reports ...");
                        Services.ProcessStart(exename, string.Empty, InputValidation.ValidateEmptyInput);
                        do
                        {
                            Application.DoEvents();
                            System.Diagnostics.Trace.WriteLine("EXTRPT: Waiting for Proccess to Start ...");
                        } while (Process.GetProcesses().Count(n => n.ProcessName.ToLowerInvariant().Contains(processname)) == 0);
                    }
                }
                System.Diagnostics.Trace.WriteLine("EXTRPT: Retrying Hello ...");
                Application.DoEvents();
                System.Threading.Thread.Sleep(200);
                Application.DoEvents();
                retries++;
            } while (retries < 100);
            System.Diagnostics.Trace.WriteLine("EXTRPT: Timed Out ...");
            return false;
        }

        public static bool IsAlreadyRunning(string processname)
        {
            return AlreadyRunning(processname);
        }

        static bool AlreadyRunning(string processname)
        {
            //Changed for TS mode - look at processes for the sessionID
            System.Diagnostics.Process thisProcess = System.Diagnostics.Process.GetCurrentProcess();
            int thisSessionID = thisProcess.SessionId;

            //Get all processes of this application name
            foreach (System.Diagnostics.Process process in System.Diagnostics.Process.GetProcessesByName(processname))
            {
                //Now only look for processes in this session
                if (thisSessionID == process.SessionId)
                {
                    //If not the same process then there is another process running for this session!
                    if (process.Id != thisProcess.Id)
                    {
                        return true;
                    }
                }
            }

            //if code gets here then all is OK to run the application in this session
            return false;
        }
    }
}
