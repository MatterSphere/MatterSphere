using System;

namespace FWBS.OMS.Utils.Commands
{
    public class ViewFileCommand : RunCommand
    {
        public override string Name
        {
            get { return "VIEWMATTER"; }
        }


        public override void Execute(MainWindow main)
        {
            if (Param.Length > 0)
            {
                string[] splitstr = new string[2];
                
                char[] delimiter;
                //TODO: ConfigSetting-/ClientSearch/ClientDelimiter
                delimiter = FWBS.OMS.Session.CurrentSession.GetSessionConfigSetting("/config/clientSearch/clientDelimiter", " ./:-").ToCharArray();
                splitstr = Param.Split(delimiter, 2);
                if (splitstr.Length != 2)
                {
                    FWBS.OMS.UI.Windows.MessageBox.ShowInformation("VFCBADPARAM", "Please specify a client number and file number in the format CLNO/FILENO");
                    return;
                }

                Client client = Client.GetClient(splitstr[0]);

                OMSFile file = client.FindFile(splitstr[1], true);
                if (file == null)
                {
                    throw new OMSException2(HelpIndexes.OMSFileNotFound.ToString(), "", "", null, true, Param);
                }

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
