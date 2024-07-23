namespace FWBS.OMS.Utils.Commands
{
    public class ExitCommand : DisconnectCommand
    {
        public override string Name
        {
            get { return "EXIT"; }
        }

        public override bool RequiresLogin
        {
            get { return false; }
        }

        public override void Execute(MainWindow main)
        {
            base.Execute(main);
            System.Windows.Forms.Application.Exit();
        }
    }
}
