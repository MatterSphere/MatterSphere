using System;

namespace FWBS.OMS.Windows.Reports
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                SingleAppContext context = new SingleAppContext();
                context.Run(args);
            }
            catch
            {
            }
        }
    }
}
