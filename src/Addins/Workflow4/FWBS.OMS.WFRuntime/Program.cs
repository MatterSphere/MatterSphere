using System;
using System.Collections.Generic;

namespace FWBS.OMS.WFRuntime
{
    internal class Program
	{
		#region Main
		[STAThread]
		static int Main(string[] args)
		{
			int retValue = 0;

			FWBS.OMS.Workflow.SplashWindow wnd = new FWBS.OMS.Workflow.SplashWindow("Getting ready to run workflow");
			wnd.Show();
			System.Threading.Thread.Sleep(500);

			try
			{
				// do we have a session object?
				if (FWBS.OMS.Session.CurrentSession != null)
				{
					// yes, are we connected?
					if (!FWBS.OMS.Session.CurrentSession.IsLoggedIn)
					{
						// connect
						FWBS.OMS.Session.CurrentSession.Connect();
					}
					// get workflow bits
					string wfCode = AppDomain.CurrentDomain.GetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFCODE) as string;	// used to create cache sub directory
					string wfXaml = AppDomain.CurrentDomain.GetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFXAML) as string;	// workflow xaml
					HashSet<string> wfDistributions = AppDomain.CurrentDomain.GetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFDISTRIBUTIONS) as HashSet<string>;
					HashSet<string> wfScriptCodes = AppDomain.CurrentDomain.GetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFSCRIPTCODES) as HashSet<string>;
					HashSet<string> wfReferences = AppDomain.CurrentDomain.GetData(FWBS.OMS.Workflow.Constants.APPDOMAIN_WFREFERENCES) as HashSet<string>;

					// anything to run?
					if (!string.IsNullOrEmpty(wfXaml))
					{
						if (wfDistributions == null)
						{
							wfDistributions = new HashSet<string>();
						}

						if (wfScriptCodes == null)
						{
							wfScriptCodes = new HashSet<string>();
						}

						if (wfReferences == null)
						{
							wfReferences = new HashSet<string>();
						}

						// create a default context
						ContextFactory cf = new ContextFactory();
						OMS.Context ctx = cf.CreateContext(null);

						wnd.Close();

						// Run the workflow
						//	The workflow is given 1 day to run, if it hasn't completed within that time it will timeout
						//	1 day should be enough for the developer/BA to test his synchronous workflow ...
						FWBS.OMS.Workflow.WFRuntime.Execute(TimeSpan.FromDays(1), null, wfXaml, wfDistributions, wfReferences, wfScriptCodes, ctx);
					}
				}
			}
			catch (System.Threading.ThreadAbortException)
			{
				// thread is being aborted - tha AppDomain is being unloaded!
				System.Threading.Thread.ResetAbort();
				wnd.Close();
			}
			catch (Exception ex)
			{
				wnd.Close();
				// display error message
				FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
			}

			// we have to manually shutdown the dispatcher, otherwise on some machines the unloading of the domain will FAIL!
			if (!wnd.Dispatcher.HasShutdownStarted)
			{
				wnd.Dispatcher.InvokeShutdown();
			}
			// exit all other wpf frames
			System.Windows.Threading.Dispatcher.ExitAllFrames();

			// garbage collect everything so that there is no 'funny' finalizers to stop the appdomain being unloaded
			GC.Collect();
			GC.WaitForPendingFinalizers();

			return retValue;
		}
		#endregion
	}
}
