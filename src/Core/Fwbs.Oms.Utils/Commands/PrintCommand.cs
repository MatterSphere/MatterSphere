using System;

namespace FWBS.OMS.Utils.Commands
{

    public class PrintCommand : FileCommand
    {
        const int ERROR_NO_APP_ASSOCIATED = 1155; 

        public override string Name
        {
            get { return "PRINT"; }
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
            try
            {
                OpenFile(Param, "print");
            }
            catch (System.ComponentModel.Win32Exception ex32)
            {
                if (ex32.NativeErrorCode == ERROR_NO_APP_ASSOCIATED)
                {
                    if (Session.CurrentSession.IsLoggedIn)
                        throw new OMSException2("PRTCMDNOAPP", "The document %1% could not automatically be printed. The operating system reported that no application is associated with the file extension %2% for printing", ex32, false, Param, System.IO.Path.GetExtension(Param));                  
                    else
                        throw new Exception(string.Format("The document {0} could not automatically be printed. The operating system reported that no application is associated with the file extension {1} for printing", Param, System.IO.Path.GetExtension(Param)), ex32);                  
                }
            }
        }
    }
}
