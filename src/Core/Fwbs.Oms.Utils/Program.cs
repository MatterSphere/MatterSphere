using System;

namespace FWBS.OMS.Utils
{
    /// <summary>
    /// A utilities class used to perform monotonous tasks within the OMS system.
    /// </summary>
    class Program
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
            catch(Exception)
            {
            }

        }

    }
}
