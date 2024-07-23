using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceProcess;
using System.Threading;

namespace FWBS.OMS.OMSEXPORT
{
	class Program
	{
		private static OMSExportService _service;
#if DEBUG
        private static ManualResetEvent _stopEvent;

        static void Main(string[] args)
		{
			if (Environment.UserInteractive)
			{
				_stopEvent = new ManualResetEvent(false);
				AllocConsole();
				SetConsoleCtrlHandler(ConsoleHandlerRoutine, true);
				Console.Title = ((AssemblyTitleAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title;
			}

			bool isNewMutexCreated;
			SecurityIdentifier sid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
			MutexSecurity mutexSecurity = new MutexSecurity();
			mutexSecurity.AddAccessRule(new MutexAccessRule(sid, MutexRights.FullControl, AccessControlType.Allow));
			string mutexName = "Global\\" + ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false)[0]).Value;

			using (Mutex mutexOneInstance = new Mutex(false, mutexName, out isNewMutexCreated, mutexSecurity))
			{
				if (mutexOneInstance.WaitOne(0, false))
				{
					System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
					_service = new OMSExportService();
					if (Environment.UserInteractive)
					{
						Console.WriteLine("Start processing.\r\nPress Ctrl+C to shutdown...");
						try
						{
							CallServiceMethod("OnStart", new object[] { args });
							_stopEvent.WaitOne();
							CallServiceMethod("OnStop");
						}
						catch (Exception ex)
						{
							Console.WriteLine(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
						}
						Console.WriteLine("Finished.");
					}
					else
					{
						ServiceBase.Run(_service);
					}
					_service = null;
				}
				else
				{
					Console.WriteLine("Another instance of the application is already running.");
				}
			}

			if (Environment.UserInteractive)
			{
				SetConsoleCtrlHandler(ConsoleHandlerRoutine, false);
				FreeConsole();
				_stopEvent.Close();
			}
		}

		private static void CallServiceMethod(string methodName, params object[] args)
		{
			Type type = _service.GetType();
			MethodInfo method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic);
			method.Invoke(_service, args);
		}

		[DllImport("Kernel32.dll", ExactSpelling=true, SetLastError=true)]
		private static extern bool AllocConsole();
		[DllImport("Kernel32.dll", ExactSpelling=true, SetLastError=true)]
		private static extern bool FreeConsole();
		[DllImport("Kernel32.dll", ExactSpelling=true, SetLastError=true)]
		private static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
		private delegate bool HandlerRoutine(UInt32 dwCtrlType);
		private static HandlerRoutine ConsoleHandlerRoutine = new HandlerRoutine((dwCtrlType) => { _stopEvent.Set(); return true; });

#else
        static void Main(string[] args)
        {
            _service = new OMSExportService();
            ServiceBase.Run(_service);
            _service = null;
        }
#endif
    }
}
