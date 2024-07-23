using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows.DocumentManagement;

namespace FWBS.OMS.Utils.Commands
{
    public class BulkSaveAsCommand : SaveCommand
    {
        private long _fileID;
        private Guid _folderGuid = Guid.Empty;

                
        public override string Name
        {
            get { return "SAVEAS"; }
        }

        public long FileID
        {
            get
            {
                return _fileID;
            }
            set 
            {
                _fileID = value;
            }
        }

        public Guid FolderGuid
        {
            get
            {
                return _folderGuid;
            }
            set
            {
                if (value == null)
                    _folderGuid = Guid.Empty;
                else
                    _folderGuid = value;
            }
        }

        private List<string> files = new List<string>();
        public List<string> Files
        {
            get
            {
                return files;
            }
        }

        public bool ImportIndividual
        {
            get;
            set;
        }

        private NotifyIcon notification;

        public override void OnBusy(NotifyIcon notification)
        {
            this.notification = notification;
        }

        void app_BulkDocumentProcessing(object sender, FWBS.OMS.UI.BulkDocumentProcessArgs e)
        {
            if (notification != null)
            {
                if (e.Name.StartsWith("#"))
                {
                    notification.Icon = Properties.Resources.Save;
                    notification.BalloonTipIcon = ToolTipIcon.Info;
                    notification.BalloonTipTitle = Session.CurrentSession.Resources.GetResource("SAVESUCESS", "Save Sucessfully Completed", "").Text;
                    notification.BalloonTipText = e.Name.TrimStart('#');
                    notification.Text = notification.BalloonTipTitle;
                    notification.ShowBalloonTip(50000);
                }
                else
                {
                    notification.Icon = Properties.Resources.Save;
                    notification.BalloonTipIcon = ToolTipIcon.Info;
                    notification.BalloonTipTitle = Session.CurrentSession.Resources.GetResource("SAVINGDOCUMENT", "Saving Document", "").Text;
                    notification.BalloonTipText = Session.CurrentSession.Resources.GetMessage("MSGSAVINGDOC", "Currently saving file '%1%', click to view wizard.", "", System.IO.Path.GetFileName(e.Name)).Text;
                    notification.Text = notification.BalloonTipTitle;
                    notification.ShowBalloonTip(50000);
                }
            }
        }
        
        public override void Execute(MainWindow main)
        {
            if (ImportIndividual == false)
            {
                System.IO.FileInfo primaryFile = null;

                if (Files.Count > 0)
                {
                    primaryFile = new System.IO.FileInfo(Files[0]);
                    DocumentManagement.Storage.StorageManager.CurrentManager.ValidateFileExtension(primaryFile.FullName);
                    Files.RemoveAt(0);
                }

                List<System.IO.FileInfo> subfiles = new List<System.IO.FileInfo>();
                foreach (string file in Files)
                {
                    if (DocumentManagement.Storage.StorageManager.CurrentManager.IsValidFileExtension(file))
                        subfiles.Add(new System.IO.FileInfo(file));
                }

                if (primaryFile != null)
                {
                    FWBS.OMS.UI.Windows.ShellOMS _appcontroller = new FWBS.OMS.UI.Windows.ShellOMS();
                    Apps.ApplicationManager.CurrentManager.InitialiseInstance("SHELL", _appcontroller);
                    using (FWBS.OMS.UI.Windows.ShellFile sf = new FWBS.OMS.UI.Windows.ShellFile(primaryFile, subfiles.ToArray()))
                    {
                        _appcontroller.SaveAs(sf, false);
                    }
                }
            }
            else
            {
                try
                {

                    FWBS.OMS.UI.Windows.ShellOMS _appcontroller = new FWBS.OMS.UI.Windows.ShellOMS();
                    Apps.ApplicationManager.CurrentManager.InitialiseInstance("SHELL", _appcontroller);
                    _appcontroller.BulkDocumentProcessing += new EventHandler<FWBS.OMS.UI.BulkDocumentProcessArgs>(app_BulkDocumentProcessing);

                    if (Parent == null || Parent.IsValid == false || Parent.IsHung || Parent.IsVisible == false)
                        Parent = main.ActivateWindow(null);

                    BulkDocumentImportTools bulkTools = new BulkDocumentImportTools(Files, _appcontroller);
                    bulkTools.SaveMultipleDocuments();

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
}
