using System;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.DocumentManagement
{
    public class ReplyToAuthoriseCommand : FWBS.OMS.Commands.Command
    {
         private Redemption.RDOSession session;
        private bool sessionSet = false;
        public Redemption.RDOSession Session
        {
            get
            {
                return session;
            }
            set
            {
                session = value;
                sessionSet = session != null;
            }
        }


        public bool Send { get; set; }
        public bool Authorised { get; set; }
        public IStorageItem Item { get; set; }
        
        public string To { get; set; }
        public string OriginalMessage { get; set; }

        bool replySet = false;
        private Redemption.RDOMail reply;
        public Redemption.RDOMail ReplyItem
        {
            get
            {
                return reply;
            }
            set
            {
                reply = value;
                replySet = reply != null;
            }
        }

        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            FWBS.OMS.Commands.ExecuteResult result = new FWBS.OMS.Commands.ExecuteResult(FWBS.OMS.Commands.CommandStatus.Failed);

            if (Item == null)
            {
                if (ContinueOnError)
                {
                    result.Errors.Add(new ArgumentNullException("Item"));
                    return result;
                }
                else
                    throw new ArgumentNullException("Item");
            }

            if (string.IsNullOrEmpty(To))
            {
                if (ContinueOnError)
                {
                    result.Errors.Add(new ArgumentNullException("To"));
                    return result;
                }
                else
                    throw new ArgumentNullException("To");

            }

            OMSDocument doc = Item as OMSDocument;
            DocumentVersion version = Item as DocumentVersion;

            if (doc == null && version == null)
            {
                var errorMessage = FWBS.OMS.Session.CurrentSession.Resources.GetMessage("MSGINVDCTP", "Invalid Document Type", "").Text;
                if (ContinueOnError)
                {
                    result.Errors.Add(new ArgumentNullException(errorMessage));
                    return result;
                }
                else
                    throw new NotSupportedException(errorMessage);
            }
            else if (doc == null)
                doc = version.ParentDocument;



            if (!sessionSet)
                session = Redemption.RedemptionFactory.Default.CreateRDOSession();

            if (!session.LoggedOn)
                session.Logon(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);

            string authText = null;

            if (Authorised)
            {

                authText = FWBS.OMS.Session.CurrentSession.Resources.GetResource("AUTHORISED", "Authorised", "").Text;
                version.AddActivity("AUTHORISE", "ACCEPTED", To);
            }
            else
            {
                authText = FWBS.OMS.Session.CurrentSession.Resources.GetResource("REJECTED", "Rejected", "").Text;
                version.AddActivity("AUTHORISE", "FAILED", To);
            }

            Redemption.MAPIUtils utils = Redemption.RedemptionFactory.Default.CreateMAPIUtils();
            Redemption.RDOFolder rdofolder = null;
            Redemption.RDOItems rdoitems = null;
            try
            {
                Associate assoc = doc.Associate;
                OMSFile file = assoc.OMSFile;
                Client client = file.Client;

                string subject = FWBS.OMS.Session.CurrentSession.Resources.GetResource("AUTHRPLYSUB", "Document %1%/%2% %3% (%4%) - %5%", "", client.ClientNo.Trim(), file.FileNo.Trim(), assoc.Addressee.Trim(), Item.DisplayID, authText).Text;

                if (!replySet)
                {
                    rdofolder = session.GetDefaultFolder(Redemption.rdoDefaultFolders.olFolderDrafts);
                    rdoitems = rdofolder.Items;
                    reply = rdoitems.Add("IPM.Note");
                }

                SendToAuthoriseCommand.AddAuthorisationVariables(utils, reply, doc, version, true);

                reply.To = To;
                reply.Subject = subject;
                reply.Body = Environment.NewLine + OriginalMessage;

                if (Send)
                {
                    reply.Save();
                    reply.Send();
                    utils.DeliverNow((int)Redemption.FlushQueuesEnum.fqUpload);
                    result.Status = FWBS.OMS.Commands.CommandStatus.Success;
                }
                else
                {
                    result.Status = FWBS.OMS.Commands.CommandStatus.None;
                }
                
            }
            finally
            {
                if (!replySet && Send && reply != null)
                {
                    FWBS.Common.COM.DisposeObject(reply);
                    reply = null;
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
                if (!sessionSet && session != null)
                {
                    FWBS.Common.COM.DisposeObject(session);
                    session = null;
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

        protected bool DisplayReply()
        {
            if (reply != null)
            {
                reply.Display(Type.Missing, Type.Missing);
                return true;
            }
            return false;
        }
    }
}
