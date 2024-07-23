namespace FWBS.OMS.Utils.Commands
{
    public class DisconnectCommand : RunCommand
    {
        public override string Name
        {
            get { return "DISCONNECT"; }
        }

        public override void Execute(MainWindow main)
        {
            main.Disconnect();
        }
    }
}
