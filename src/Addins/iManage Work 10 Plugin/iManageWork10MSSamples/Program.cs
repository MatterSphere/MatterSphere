using System;
using System.Reflection;
using System.Windows.Forms;

namespace iManageWork10MSSamples
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string assemblyName = args.Name;
            if (!assemblyName.Contains(".resources") && assemblyName.Contains("DotNetBrowser"))
            {
                string filename = $"{assemblyName.Split(',')[0]}.dll";
                try
                {
                    return Assembly.LoadFrom($"{AppDomain.CurrentDomain.BaseDirectory}\\{filename}");
                }
                catch (Exception)
                { }
            }

            return null;
        }
    }
}
