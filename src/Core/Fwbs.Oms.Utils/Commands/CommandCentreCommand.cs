namespace FWBS.OMS.Utils.Commands
{
    public class CommandCentreCommand : RunCommand
    {
        public override string Name
        {
            get { return "COMMANDCENTRE"; }
        }

        public override void Execute(MainWindow main)
        {
            if(!FWBS.OMS.UI.Windows.Services.CheckLogin())
                return;

            User usr = Session.CurrentSession.CurrentUser;

            FWBS.OMS.UI.OMSTypeScreen commandCentre = new FWBS.OMS.UI.OMSTypeScreen(usr);
            commandCentre.OmsType = usr.CommandCentre;
            commandCentre.DefaultPage = Param;
            commandCentre.Show(Parent);


        }
    }
}
