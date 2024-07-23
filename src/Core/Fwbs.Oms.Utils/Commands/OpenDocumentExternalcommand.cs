using System;
using FWBS.OMS.DocumentManagement;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.Utils.Commands
{
    public class OpenDocumentExternalCommand : RunCommand
    {
        public override string Name
        {
            get { return "OPENDOCUMENTEXT"; }
        }

        public override void Execute(MainWindow main)
        {
            
            string sdocid = Param.Trim();
            string label = Param2.Trim();

            int versionseploc = Param.IndexOf('.');
            if (versionseploc > -1)
            {
                sdocid = Param.Substring(0, versionseploc).Trim();
                label = Param.Substring(versionseploc + 1).Trim();
            }


            OMSDocument doc = OMSDocument.GetDocument(sdocid);

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


        }


    }
}
