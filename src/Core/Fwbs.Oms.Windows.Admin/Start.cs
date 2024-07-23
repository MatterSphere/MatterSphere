using System;
using System.Windows.Forms;
using FWBS.Common;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for Start.
    /// </summary>
    public class Start
    {
        public static bool Restart = false;
        public Start()
        {
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                AllocConsole();
                ConsoleMain(args);
            }
            else
            {
                frmDockMain.RunLive = true;
                Application.SetCompatibleTextRenderingDefault(false);
                Application.EnableVisualStyles();

                AdminKitTreeNavigationActions actions = new AdminKitTreeNavigationActions();
                Application.Run(new frmDockMain(actions));
            }
        }

        private static void ConsoleMain(string[] args)
        {
            try
            {
                if (args.Length > 2 && args[0] == "-install-packages")
                {
                    Console.WriteLine("[Automation] - Packages installation process is started");

                    string[] paths = new string[args.Length - 2];

                    Array.Copy(args, 2, paths, 0, paths.Length);

                    int dbSelection = int.Parse(args[1]);
                    var packages = PackageDeployAutomation.GetPackages(paths);

                    if (packages.Count > 0)
                    {
                        Session.Login(dbSelection, Environment.UserName, "", false);
                        Session.CurrentSession.IsAutomation = true;

                        FWBS.OMS.UI.Windows.Services.CheckLogin();
                        PackageDeployAutomation.ProcessPackages(packages);

                        Console.WriteLine("[Automation] - Packages installation process is successfully completed.");
                    }
                    else
                    {
                        Console.WriteLine("[Automation] - There is no specified packages for installation.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[Automation] - Packages installation process error. Details" + e);
                throw;
            }
        }
        
      

        [System.Runtime.InteropServices.DllImport("Kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

    }
}
