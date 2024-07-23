namespace FWBS.OMS.Utils.Commands
{
    public class OpenLocalDocumentCommand : RunCommand
    {
        public override string Name
        {
            get { return "OPENLOCALDOCUMENT"; }
        }

        public override bool RequiresLogin
        {
            get { return false; }
        }

        public override void Execute(MainWindow main)
        {
            FWBS.OMS.UI.Windows.DocumentManagement.DocumentPicker picker = new FWBS.OMS.UI.Windows.DocumentManagement.DocumentPicker();
            System.IO.FileInfo[] files = picker.ShowLocal(Parent, false);
            if (files != null)
            {
                foreach (System.IO.FileInfo file in files)
                {
                    if (System.IO.File.Exists(file.FullName))
                    {
                        OpenFile(file.FullName, "open");
                    }
                }
            }
        }
    }
}
