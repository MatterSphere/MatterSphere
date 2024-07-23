using System;
using FWBS.OMS.Interfaces;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class ActionProvider
    {
        public Action<long> GetAction(string action)
        {
            switch (action.ToLower())
            {
                case "open_contact":
                    return OpenContact;
                case "open_client":
                    return OpenClient;
                case "open_file":
                    return OpenFile;
                case "open_document":
                case "open_email":
                    return OpenDocument;
                case "open_document_properties":
                    return OpenDocumentProperties;
                case "open_precedent":
                    return OpenPrecedent;
                case "use_precedent":
                    return UsePrecedent;
                case "open_associate":
                    return OpenAssociate;
                default:
                    return null;
            }
        }

        private void OpenFile(long id)
        {
            FWBS.OMS.OMSFile file = FWBS.OMS.OMSFile.GetFile(id);
            OnNewOMSTypeWindow(file);
        }

        private void OpenClient(long id)
        {
            FWBS.OMS.Client client = FWBS.OMS.Client.GetClient(id);
            OnNewOMSTypeWindow(client);
        }

        private void OpenContact(long id)
        {
            FWBS.OMS.Contact contact = FWBS.OMS.Contact.GetContact(id);
            OnNewOMSTypeWindow(contact);
        }

        private void OpenAssociate(long id)
        {
            FWBS.OMS.Associate associate = FWBS.OMS.Associate.GetAssociate(id);
            OnNewOMSTypeWindow(associate);
        }

        private void OpenDocument(long id)
        {
            FWBS.OMS.UI.Windows.Services.OpenDocument(id, FWBS.OMS.DocOpenMode.View);
        }

        private void OpenDocumentProperties(long id)
        {
            FWBS.OMS.OMSDocument document = FWBS.OMS.OMSDocument.GetDocument(id);
            OnNewOMSTypeWindow(document);
        }

        private void OpenPrecedent(long id)
        {
            FWBS.OMS.UI.Windows.Services.OpenPrecedent(id, FWBS.OMS.DocOpenMode.View);
        }

        private void UsePrecedent(long id)
        {
            Precedent precedent = Precedent.GetPrecedent(id);
            var job = new PrecedentJob(precedent)
            {
                Associate = FWBS.OMS.UI.Windows.Services.SelectAssociate()
            };

            if (job.Associate != null)
            {
                FWBS.OMS.UI.Windows.Services.ProcessJob(null, job);
            }
        }
        
        private void OnNewOMSTypeWindow(IOMSType omsType)
        {
            try
            {
                var eva = new NewOMSTypeWindowEventArgs(omsType);
                var screen = new OMSTypeScreen(eva.OMSObject)
                {
                    DefaultPage = eva.DefaultPage,
                    OmsType = eva.OMSType
                };
                screen.Show(null);
            }
            catch (OMSException2 e)
            {
                if (IsNotFoundOrPermissionErrorCode(e.Code))
                {
                    ErrorBox.Show(e);
                }
                else
                {
                    throw;
                }
            }
        }

        private bool IsNotFoundOrPermissionErrorCode(string errorCode)
        {
            HelpIndexes helpIndex;
            return Enum.TryParse(errorCode, out helpIndex) && (helpIndex == HelpIndexes.ClientNotFound
                                                               || helpIndex == HelpIndexes.OMSFileNotFound
                                                               || helpIndex == HelpIndexes.ContactNotFound
                                                               || helpIndex == HelpIndexes.DocumentNotFound);
        }
    }
}
