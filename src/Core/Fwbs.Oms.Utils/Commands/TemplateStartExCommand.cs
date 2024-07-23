using System;

namespace FWBS.OMS.Utils.Commands
{
    public class TemplateStartExCommand : RunCommand
    {
        public override string Name
        {
            get { return "STARTPREC"; }
        }

        public override void Execute(MainWindow main)
        {
            long precid;
            if (Int64.TryParse(Param, out precid))
            {
                Associate assoc = FWBS.OMS.UI.Windows.Services.SelectAssociate();
                if (assoc != null)
                {
                    Precedent prec = Precedent.GetPrecedent(precid);
                    FWBS.OMS.UI.Windows.Services.TemplateStart(null, prec, assoc);
                }
            }
        }
    }
}
