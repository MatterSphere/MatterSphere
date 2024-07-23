using System;
using System.Collections.Generic;
using FWBS.OMS.DocumentManagement;
using FWBS.OMS.DocumentManagement.Storage;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Utils.Commands
{
    public class OpenDocumentCommand : RunCommand
    {
        public override string Name
        {
            get { return "OPENDOCUMENT"; }
        }

        public override void Execute(MainWindow main)
        {
            long docid;

            string sdocid = Param.Trim();
            string label = Param2.Trim();
            string docCheckedOutMsg = "";

            int versionseploc =  Param.IndexOf('.');
            if (versionseploc > -1)
            {
                sdocid = Param.Substring(0, versionseploc).Trim();
                label = Param.Substring(versionseploc + 1).Trim();
            }

            if (long.TryParse(sdocid, out docid))
            {
                OMSDocument doc = OMSDocument.GetDocument(docid);

                var lockableitem = (IStorageItemLockable)doc;
                User checkedoutby = lockableitem.CheckedOutBy;
                if (checkedoutby != null && checkedoutby.ID != Session.CurrentSession.CurrentUser.ID)
                {
                    docCheckedOutMsg = Session.CurrentSession.Resources.GetMessage("MSGDOCCHECKOUT", "Document %1% is already checked out by '%2%'", String.Empty, doc.DisplayID, checkedoutby.FullName).Text;
                }


                if (!String.IsNullOrEmpty(label))
                {
                    DocumentVersion version = (DocumentVersion)doc.GetVersion(label);
                    if (version == null)
                        throw new FWBS.OMS.DocumentManagement.Storage.StorageException("MSGDOCVERINV", "Version '%1%' does not exist for document '%2%'.", null, label, sdocid);

                    StorageSettingsCollection settings = doc.GetStorageProvider().GetDefaultSettings(version, SettingsType.Fetch);
                    VersionFetchSettings versettings = settings.GetSettings<FWBS.OMS.DocumentManagement.Storage.VersionFetchSettings>();

                    //Make sure specific version is specified otherwise the default OpenDocument 
                    //class will use latest version.
                    versettings.Version = VersionFetchSettings.FetchAs.Specific;
                    versettings.VersionLabel = label;
                    version.ApplySettings(settings);

                    FWBS.OMS.UI.Windows.Services.OpenDocument(version, DocOpenMode.Edit);
                }
                else
                    FWBS.OMS.UI.Windows.Services.OpenDocument(doc, DocOpenMode.Edit);

                if (!String.IsNullOrEmpty(docCheckedOutMsg))
                {
                    MessageBox.Show(docCheckedOutMsg, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                }
                else
                {
                    List<OMSDocument> docList = new List<OMSDocument>() { doc };
                    FWBS.OMS.UI.Windows.Services.CheckForUnsupportedFiles(docList, true);
                }
            }
            else
            {
                FWBS.OMS.UI.Windows.Services.ShowOpenDocument(Parent, null);
            }
        }
    }
}
