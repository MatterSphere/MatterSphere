using System;
using System.Text;
using FWBS.OMS.DocumentManagement.Storage;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.DocumentManagement
{
    public class SendToAuthoriseCommand : FWBS.OMS.Commands.Command
    {
       
        public IStorageItem Item { get; set; }
        public string To { get; set; }

        protected string sentToFullName;
        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {

            FWBS.OMS.Commands.ExecuteResult result = new FWBS.OMS.Commands.ExecuteResult(FWBS.OMS.Commands.CommandStatus.Failed);

            if(!Session.CurrentSession.IsMailEnabled)
                throw new MailDisabledException();

            if (Item == null)
                throw new ArgumentNullException("item");

            OMSDocument doc = Item as OMSDocument;
            DocumentVersion version = Item as DocumentVersion;

            if (doc == null && version == null)
                throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("MSGINVDCTP", "Invalid Document Type", "").Text);
            else if (doc == null)
                doc = version.ParentDocument;

            if (String.IsNullOrEmpty(To))
            {
                User feeEarner = Session.CurrentSession.CurrentFeeEarner;

                if (Session.CurrentSession.CurrentUser.WorksForMatterHandler)
                    feeEarner = doc.OMSFile.PrincipleFeeEarner;

                sentToFullName = feeEarner.FullName;
                To = feeEarner.Email.Trim();
                if (String.IsNullOrEmpty(To))
                    To = feeEarner.FullName.Trim();
            }

            Redemption.MAPIUtils utils = Redemption.RedemptionFactory.Default.CreateMAPIUtils();
            Redemption.RDOSession rdosession = Redemption.RedemptionFactory.Default.CreateRDOSession();
            Redemption.RDOFolder rdofolder = null;
            Redemption.RDOItems rdoitems = null;
            Redemption.RDOMail rdomail = null;

            try
            {
                if (!rdosession.LoggedOn)
                    rdosession.Logon(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

                Associate assoc = doc.Associate;
                OMSFile file = assoc.OMSFile;
                Client cl = file.Client;

                string subject = Session.CurrentSession.Resources.GetResource("DOCAUTHSUBJECT", @"Authorise Document: %1%/%2% %3% (%4%)", "", cl.ClientNo.Trim(), file.FileNo.Trim(), assoc.Addressee.Trim(), Item.DisplayID).Text;

                StringBuilder body = new StringBuilder();
                body.AppendLine();
                body.Append(Session.CurrentSession.Resources.GetResource("DOCAUTHBODY",
@"
***** Document to Authorise *****
%FILE%: %1%/%2% %3%
%4%
Requested By: %5%
Document Id: %6%", "", true, cl.ClientNo.Trim(), file.FileNo.Trim(), assoc.Addressee.Trim(), assoc.AssocHeading.Trim(), Session.CurrentSession.CurrentUser.FullName, Item.DisplayID).Text);

                rdofolder = rdosession.GetDefaultFolder(Redemption.rdoDefaultFolders.olFolderDrafts);
                rdoitems = rdofolder.Items;
                rdomail = rdoitems.Add("IPM.Note");
                rdomail.To = To;
                rdomail.Subject = subject;
                rdomail.Body = body.ToString();

                AddAuthorisationVariables(utils, rdomail, doc, version, false);

                rdomail.Save();
                rdomail.Send();
                utils.DeliverNow((int)Redemption.FlushQueuesEnum.fqUpload);

                Item.AddActivity("AUTHORISE", "SENT", To);

                result.Status = FWBS.OMS.Commands.CommandStatus.Success;

            }
            catch(Exception ex)
            {
                if (ContinueOnError)
                    result.Errors.Add(ex);
                else
                    throw;
            }
            finally
            {
                if (rdomail != null)
                {
                    FWBS.Common.COM.DisposeObject(rdomail);
                    rdomail = null;
                }
                if (rdoitems != null)
                {
                    FWBS.Common.COM.DisposeObject(rdoitems);
                    rdoitems = null;
                }
                if (rdofolder != null)
                {
                    FWBS.Common.COM.DisposeObject(rdofolder);
                    rdofolder = null;
                }
                if (rdosession != null)
                {
                    FWBS.Common.COM.DisposeObject(rdosession);
                    rdosession = null;
                }
                if (utils != null)
                {
                    utils.Cleanup();
                    FWBS.Common.COM.DisposeObject(utils);
                    utils = null;
                }
            }

            return result;

        }

        internal static void AddAuthorisationVariables(Redemption.MAPIUtils utils, Redemption.RDOMail rdomail, OMSDocument doc, DocumentVersion version, bool returnFromAuthorisation)
        {
            object mapiObject = null;
            try
            {
                mapiObject = rdomail.MAPIOBJECT;
                int PROPTAG_AUTHORISE_DOCUMENT = utils.GetIDsFromNames(mapiObject, IOMSApp.PROPTAG_CUSTOM_PROPERTY, IOMSApp.AUTHORISE_DOCUMENT, true);
                int PROPTAG_AUTHORISE_DOCUMENT_VERSION = utils.GetIDsFromNames(mapiObject, IOMSApp.PROPTAG_CUSTOM_PROPERTY, IOMSApp.AUTHORISE_DOCUMENT_VERSION, true);
                int PROPTAG_AUTHORISE_COMPANY = utils.GetIDsFromNames(mapiObject, IOMSApp.PROPTAG_CUSTOM_PROPERTY, IOMSApp.COMPANY, true);
                int PROPTAG_AUTHORISE_DATAKEY = utils.GetIDsFromNames(mapiObject, IOMSApp.PROPTAG_CUSTOM_PROPERTY, IOMSApp.DATAKEY, true);
                int PROPTAG_AUTHORISE_BRANCH = utils.GetIDsFromNames(mapiObject, IOMSApp.PROPTAG_CUSTOM_PROPERTY, IOMSApp.BRANCH, true);
                int PROPTAGE_AUTHORISE_RETURN = utils.GetIDsFromNames(mapiObject, IOMSApp.PROPTAG_CUSTOM_PROPERTY, IOMSApp.AUTHORISE_RETURN, true);

                //NOTE: Cast the doc id into a double as the MAPI set property method does not like a numbers greater than Int32
                int ret1 = utils.HrSetOneProp(mapiObject, PROPTAG_AUTHORISE_COMPANY, (double)Session.CurrentSession.CompanyID, false);
                int ret2 = utils.HrSetOneProp(mapiObject, PROPTAG_AUTHORISE_BRANCH, (double)Session.CurrentSession.SerialNumber, false);
                int ret3 = utils.HrSetOneProp(mapiObject, PROPTAG_AUTHORISE_DOCUMENT, (double)doc.ID, false);
                int ret4 = utils.HrSetOneProp(mapiObject, PROPTAG_AUTHORISE_DATAKEY, Session.CurrentSession.DataKey, false);
                int ret5 = utils.HrSetOneProp(mapiObject, PROPTAGE_AUTHORISE_RETURN, returnFromAuthorisation, false);
                int ret6 = 0;
                if (version != null)
                    ret6 = utils.HrSetOneProp(mapiObject, PROPTAG_AUTHORISE_DOCUMENT_VERSION, version.Id.ToString(), false);

                if (ret1 != 0 || ret2 != 0 || ret3 != 0 || ret4 != 0 || ret5 != 0 || ret6 != 0)
                    throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("AUTHEMLPRPNS", "Document Authorisation properties unable to be set.", "").Text);
            }
            finally
            {
                if (mapiObject != null)
                {
                    FWBS.Common.COM.DisposeObject(mapiObject);
                    mapiObject = null;
                }
            }
        }

    }
}
