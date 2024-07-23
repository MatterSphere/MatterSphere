namespace FWBS.OMS.Utils.Commands
{
    public class OpenCommand : FileCommand
    {
        public override string Name
        {
            get { return "OPEN"; }
        }

        public override bool RequiresLogin
        {
            get
            {
                return false;
            }
        }

        public override void Execute(MainWindow main)
        {
            OpenFile(Param, "open");
        }
    }
}
