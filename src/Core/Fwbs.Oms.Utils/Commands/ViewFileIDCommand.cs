using System;

namespace FWBS.OMS.Utils.Commands
{
    public class ViewFileIDCommand : RunCommand
    {
        public override string Name
        {
            get { return "VIEWMATTERID"; }
        }


        public override void Execute(MainWindow main)
        {
            if (Param.Length > 0)
            {
                OMSFile file = OMSFile.GetFile(Convert.ToInt64(Param));
                FWBS.OMS.UI.OMSTypeScreen screen = new FWBS.OMS.UI.OMSTypeScreen(file);
                screen.OmsType = file.CurrentFileType;
                screen.DefaultPage = Param2;
                screen.Show(Parent);
            }
            else
                FWBS.OMS.UI.Windows.Services.ShowFile(Parent, true, String.Empty);
        }
    }
}
