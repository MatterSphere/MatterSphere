using System;
using System.IO;
using System.Text;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    public class ExportDocumentCommand : FWBS.OMS.DocumentManagement.ExportDocumentCommand
    {

        public System.Windows.Forms.IWin32Window Owner { get; set; }

        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            if (string.IsNullOrEmpty(exportLocation))
            {

                System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                
                if(fbd.ShowDialog(Owner) == System.Windows.Forms.DialogResult.Cancel)
                    return new FWBS.OMS.Commands.ExecuteResult( FWBS.OMS.Commands.CommandStatus.Canceled);

                exportLocation = fbd.SelectedPath;
                
            }

            if (!string.IsNullOrEmpty(exportLocation) && !OverwriteExisting)
            {
                

                foreach (IStorageItem item in Documents)
                {
                    string fileName = GetFileName(item);

                    FileInfo info = new FileInfo(Path.Combine(exportLocation, fileName));
                    

                    if (info.Exists)
                        existingDocuments.Add(info);

                }

                if (existingDocuments.Count > 0)
                {

                    string message = "";
                    if (existingDocuments.Count == 1)
                        message = FWBS.OMS.Session.CurrentSession.Resources.GetResource("DOCEXPEXIST1", "Document %1% exists, do you want to overwrite?", "",existingDocuments[0].FullName).Text;
                    else
                    {
                        StringBuilder files = new StringBuilder();
                        foreach (FileInfo file in existingDocuments)
                            files.AppendLine(file.FullName);

                        message = FWBS.OMS.Session.CurrentSession.Resources.GetResource("DOCEXPEXISTSMUL", "The Following Files Exist:"+Environment.NewLine+"%1%"+Environment.NewLine+ "do you want to overwrite?", "", files.ToString()).Text;
                    }
                     
                    OverwriteExisting = System.Windows.Forms.DialogResult.Yes == MessageBox.ShowYesNoQuestion(message);
                }
            }
            return base.Execute();
        }

    }
}
