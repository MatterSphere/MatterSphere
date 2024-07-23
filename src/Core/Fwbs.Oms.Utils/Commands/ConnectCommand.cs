namespace FWBS.OMS.Utils.Commands
{
    public class ConnectCommand : RunCommand
    {
        public override string Name
        {
            get { return "CONNECT"; }
        }

        public override void Execute(MainWindow main)
        {
            main.Connect();
        }
    }
}
