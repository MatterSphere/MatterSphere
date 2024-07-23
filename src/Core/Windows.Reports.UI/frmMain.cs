using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Reports
{
    /// <summary>
    /// Summary description for frmMain.
    /// </summary>
    public class frmMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            ReportsTreeNavigationActions actions = new ReportsTreeNavigationActions();
            Application.Run(new frmDockMain(actions, true));
        }
    }
}
