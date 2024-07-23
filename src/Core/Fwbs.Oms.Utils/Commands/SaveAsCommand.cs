namespace FWBS.OMS.Utils.Commands
{
    using System.Windows.Forms;

    public class SaveAsCommand : SaveCommand
    {
        public override string Name
        {
            get { return "SAVEAS"; }
        }

        public override void Execute(MainWindow main)
        {
            try
            {
                string file = Param;

                if (System.IO.File.Exists(file) == false)
                    return;

                if (Parent == null || Parent.IsValid == false || Parent.IsHung || Parent.IsVisible == false)
                    Parent = main.ActivateWindow(file);

                FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.ValidateFileExtension(System.IO.Path.GetExtension(file));

                FWBS.OMS.Interfaces.IOMSApp app = (FWBS.OMS.Interfaces.IOMSApp)Apps.ApplicationManager.CurrentManager.GetApplicationInstance("SHELL", true);
                using (FWBS.OMS.UI.Windows.ShellFile sf = new FWBS.OMS.UI.Windows.ShellFile(new System.IO.FileInfo(file)))
                {
                    sf.Name = FileName;

                    FWBS.OMS.UI.Windows.ShellOMS shell = app as FWBS.OMS.UI.Windows.ShellOMS;

                    if (shell != null)
                        shell.SetActiveWindow((IWin32Window)Parent ?? main);

                    if (app.SaveAs(sf, false))
                    {
                        Globals.AddSavedDocument(file);
                    }
                }

                Globals.RemoveChangedDocument(file);
            }
            finally
            {
                if (Parent != null)
                {
                    if (!Parent.IsEnabled)
                        Parent.IsEnabled = true;
                }
            }
        }
    }


}
