using System.Windows.Forms;

namespace Fwbs.Oms.Office.Outlook
{
    public class OutlookAssociatedDocuments : Fwbs.Oms.Office.Common.Panes.AssociatedDocumentsPane
    {
        public OutlookAssociatedDocuments()
        {
            this.Name = "OutlookAssociatedDocuments";
        }

        protected override void InternalRefresh(object activeDoc)
        {
            base.InternalRefresh(activeDoc);

            if((Addin.OMSApplication.GetDocDirection(activeDoc, null) == FWBS.OMS.DocumentDirection.Out) && (Addin.OMSApplication.GetCurrentDocument(activeDoc) == null))
                this.eLastEmailsForAssociates1.DisableEmailActions = false;

        }

        protected override void AttachFile(int DocumentID)
        {
            System.Diagnostics.Debug.WriteLine("Attach");
            Microsoft.Office.Interop.Outlook.MailItem CurrentItem = this.GetCurrentMailItem();

            FWBS.OMS.UI.Windows.Office.OutlookOMS outlook = (FWBS.OMS.UI.Windows.Office.OutlookOMS)Addin.OMSApplication;

            outlook.AddAttachment(CurrentItem, FWBS.OMS.OMSDocument.GetDocument(DocumentID), false);


        }

        protected override void SetCC(int AssociateID)
        {

            FWBS.OMS.Associate Assoc = FWBS.OMS.Associate.GetAssociate(AssociateID);
            FWBS.OMS.Contact Contact = Assoc.Contact;

            string email = Contact.GetEmail("", 0);

            if (email != null && email.Length > 0)
            {
                Microsoft.Office.Interop.Outlook.MailItem CurrentItem = this.GetCurrentMailItem();
                if (CurrentItem.CC != null && CurrentItem.CC.Length > 0)
                    email =  ";"+email;

                CurrentItem.CC += email;
                
            }
            else
            {
                MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("NODFLTMLSTP", "No default email setup", "").Text);
            }

        }

        protected override void SetBCC(int AssociateID)
        {

            FWBS.OMS.Associate Assoc = FWBS.OMS.Associate.GetAssociate(AssociateID);
            FWBS.OMS.Contact Contact = Assoc.Contact;

            string email = Contact.GetEmail("", 0);

            if (email != null && email.Length > 0)
            {
                Microsoft.Office.Interop.Outlook.MailItem CurrentItem = this.GetCurrentMailItem();
                if (CurrentItem.CC != null && CurrentItem.CC.Length > 0)
                    email = ";" + email;

                CurrentItem.BCC += email;

            }
            else
            {
                MessageBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("NODFLTMLSTP", "No default email setup", "").Text);
            }
        }

        private Microsoft.Office.Interop.Outlook.MailItem GetCurrentMailItem()
        {
            return (Microsoft.Office.Interop.Outlook.MailItem)Globals.ThisAddIn.Application.ActiveInspector().CurrentItem;
        }

    }
}
