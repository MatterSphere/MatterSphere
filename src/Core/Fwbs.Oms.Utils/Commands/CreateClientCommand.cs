namespace FWBS.OMS.Utils.Commands
{
    class CreateClientCommand : RunCommand
    {
        public override string Name
        {
            get { return "CREATECLIENT"; }
        }

        public override void Execute(MainWindow main)
        {
            FWBS.OMS.UI.Windows.Services.Wizards.CreateClient();
        }
    }
}
