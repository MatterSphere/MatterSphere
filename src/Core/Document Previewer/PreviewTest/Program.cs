using System;
using System.Reflection;
using System.Windows.Forms;

namespace PreviewTest
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolveEventHandler;
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);

			{
				Form1 form = new Form1();
				Application.Run(form);

				// get rid of resources
				form.Close();
				form.Dispose();
				form = null;
			}
			// collect
			GC.Collect();
			GC.WaitForPendingFinalizers();
            GC.Collect();
			// get a memory snapshot for memory leaks or whatever
			MessageBox.Show("All disposed?");
		}

		static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			// what is this for?	
		}

        private static Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly assembly = null;
            try
            {
                string name = args.Name.Split(',')[0] + ".dll";
                string path = System.IO.Path.Combine(@"c:\OMSCoreAssemblies", name);
                if (System.IO.File.Exists(path))
                {
                    assembly = Assembly.LoadFrom(path);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to resolve assembly. {0}", ex);
            }
            return assembly;
        }
    }
}
