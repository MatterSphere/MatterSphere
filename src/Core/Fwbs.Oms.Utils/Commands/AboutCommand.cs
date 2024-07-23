namespace FWBS.OMS.Utils.Commands
{
    public class AboutCommand : RunCommand
    {
        public override string Name
        {
            get { return "ABOUT"; }
        }

        public override void Execute(MainWindow main)
        {
            FWBS.OMS.UI.Windows.Services.ShowAbout();
        }
    }
}
