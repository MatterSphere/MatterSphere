using System;

namespace FWBS.OMS.Utils.Commands
{
    public class ViewClientCommand : RunCommand
    {
        public override string Name
        {
            get { return "VIEWCLIENT"; }
        }

        public override void Execute(MainWindow main)
        {
            if (Param.Length > 0)
            {
                Client client = Client.GetClient(Param);
                FWBS.OMS.UI.OMSTypeScreen clntScreen = new FWBS.OMS.UI.OMSTypeScreen(client);
                clntScreen.OmsType = client.CurrentClientType;
                clntScreen.DefaultPage = Param2;
                clntScreen.Show(Parent);
            }
            else
                FWBS.OMS.UI.Windows.Services.ShowClient(Parent, true, String.Empty);
        }
    }
}
