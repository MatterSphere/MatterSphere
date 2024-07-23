namespace FWBS.OMS.Utils.Commands
{
    public class TemplateStartCommand : RunCommand
    {
        public override string Name
        {
            get { return "STARTTEMPLATE"; }
        }

        public override void Execute(MainWindow main)
        {   
            Associate assoc = FWBS.OMS.UI.Windows.Services.SelectAssociate();
            if (assoc != null)
                FWBS.OMS.UI.Windows.Services.TemplateStart(null, Param, assoc);
        }
    }
}
