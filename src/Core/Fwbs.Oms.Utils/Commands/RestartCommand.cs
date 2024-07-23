namespace FWBS.OMS.Utils.Commands
{
    public class RestartCommand : DisconnectCommand
    {
        public override string Name
        {
            get { return "RESTART"; }
        }

        public override bool RequiresLogin
        {
            get { return false; }
        }

        public override void Execute(MainWindow main)
        {
            base.Execute(main);
            System.Windows.Forms.Application.Restart();
        }
    }
}
