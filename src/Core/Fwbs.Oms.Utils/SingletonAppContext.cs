namespace FWBS.OMS.Utils
{
    internal sealed class SingleAppContext : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        public SingleAppContext()
        {
            this.IsSingleInstance = true;
            this.EnableVisualStyles = true;
            this.MainForm = new MainWindow();
        }

        protected override void OnStartupNextInstance(Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs eventArgs)
        {
            eventArgs.BringToForeground = true;
            base.OnStartupNextInstance(eventArgs);

            string[] args = new string[eventArgs.CommandLine.Count];
            eventArgs.CommandLine.CopyTo(args, 0);
            ((MainWindow)this.MainForm).RunCommand(args);
            
        }

        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs eventArgs)
        {
            string[] args = new string[eventArgs.CommandLine.Count];
            eventArgs.CommandLine.CopyTo(args, 0);
            ((MainWindow)this.MainForm).RunCommand(args);

            return base.OnStartup(eventArgs);
        }

    }

}
