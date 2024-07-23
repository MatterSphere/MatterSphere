namespace FWBS.OMS.Windows.Reports
{
    internal sealed class SingleAppContext : Microsoft.VisualBasic.ApplicationServices.WindowsFormsApplicationBase
    {
        public SingleAppContext()
        {
            this.IsSingleInstance = true;
            this.EnableVisualStyles = true;
            this.MainForm = new MainForm();
        }

        protected override void OnStartupNextInstance(Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs eventArgs)
        {
            eventArgs.BringToForeground = true;
            base.OnStartupNextInstance(eventArgs);
           
        }

        protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs eventArgs)
        {
            return base.OnStartup(eventArgs);
        }

    }

}
