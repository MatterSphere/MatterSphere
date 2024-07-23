namespace FWBS.OMS.Utils.Commands
{
    class CreateFileCommand : RunCommand
    {
        public override string Name
        {
            get { return "CREATEMATTER"; }
        }

        public override void Execute(MainWindow main)
        {
            if (Param.Length > 0)
            {
                Client client = Client.GetClient(Param);
                FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(client);
            }
            else
            {
                FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(true);
            }
        }

    }
}
