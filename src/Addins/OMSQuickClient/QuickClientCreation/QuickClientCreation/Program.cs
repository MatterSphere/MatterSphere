using System;
using System.Windows.Forms;

namespace QuickClientCreation
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
            if (Actions.CheckLogin())
                Application.Run(new Actions());
            else
                Application.Exit();

        }
    }
}
