using System;
using System.Collections.Generic;
using System.Linq;

namespace Fwbs.Office.Outlook
{
    using System.Runtime.InteropServices;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public partial class OutlookMail :
        OutlookItem
        , MSOutlook.MailItem
    {

        #region Fields

        private MSOutlook._MailItem mail;

        #endregion

        #region Constructors

        public OutlookMail(MSOutlook.MailItem mail)
            : base(mail)
        {
        }

        #endregion

        #region _MailItem Members

        MSOutlook.Application MSOutlook._MailItem.Application
        {
            get
            {
                return base.Application;
            }
        }

        new private MSOutlook._MailItem InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return mail;
            }
        }

        public bool AlternateRecipientAllowed
        {
            get
            {
                return InternalItem.AlternateRecipientAllowed;
            }
            set
            {
                InternalItem.AlternateRecipientAllowed = value;
            }
        }

        public bool AutoForwarded
        {
            get
            {
                return InternalItem.AutoForwarded;
            }
            set
            {
                InternalItem.AutoForwarded = value;
            }
        }

        public override string BCC
        {
            get
            {
                return GetCurrentRecipients(InternalItem.BCC, MSOutlook.OlMailRecipientType.olBCC);               
            }
            set
            {
                InternalItem.BCC = value;
            }
        }

        public override MSOutlook.OlBodyFormat BodyFormat
        {
            get
            {
                return InternalItem.BodyFormat;
            }
            set
            {
                InternalItem.BodyFormat = value;
            }
        }

        public override string CC
        {
            get
            {
                return GetCurrentRecipients(InternalItem.CC, MSOutlook.OlMailRecipientType.olCC);                 
            }
            set
            {
                InternalItem.CC = value;
            }
        }

        public void ClearConversationIndex()
        {
            InternalItem.ClearConversationIndex();
        }



        public DateTime DeferredDeliveryTime
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.DeferredDeliveryTime);
            }
            set
            {
                InternalItem.DeferredDeliveryTime = Helpers.LocalToLocal(value);
            }
        }

        public bool DeleteAfterSubmit
        {
            get
            {
                return InternalItem.DeleteAfterSubmit;
            }
            set
            {
                InternalItem.DeleteAfterSubmit = value;
            }
        }

        public bool EnableSharedAttachments
        {
            get
            {
                return InternalItem.EnableSharedAttachments;
            }
            set
            {
                InternalItem.EnableSharedAttachments = value;
            }
        }

        public DateTime ExpiryTime
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.ExpiryTime);
            }
            set
            {
                InternalItem.ExpiryTime = Helpers.LocalToLocal(value);
            }
        }

        public DateTime FlagDueBy
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.FlagDueBy);
            }
            set
            {
                InternalItem.FlagDueBy = Helpers.LocalToLocal(value);
            }
        }

        public MSOutlook.OlFlagIcon FlagIcon
        {
            get
            {
                return InternalItem.FlagIcon;
            }
            set
            {
                InternalItem.FlagIcon = value;
            }
        }

        public string FlagRequest
        {
            get
            {
                return InternalItem.FlagRequest;
            }
            set
            {
                InternalItem.FlagRequest = value;
            }
        }

        public MSOutlook.OlFlagStatus FlagStatus
        {
            get
            {
                return InternalItem.FlagStatus;
            }
            set
            {
                InternalItem.FlagStatus = value;
            }
        }

        new public MSOutlook.MailItem Forward()
        {
            return (OutlookMail)Application.GetItem(InternalItem.Forward());
        }

        public override string HTMLBody
        {
            get
            {
                return InternalItem.HTMLBody;
            }
            set
            {
                InternalItem.HTMLBody = value;
            }
        }

        public bool HasCoverSheet
        {
            get
            {
                return InternalItem.HasCoverSheet;
            }
            set
            {
                InternalItem.HasCoverSheet = value;
            }
        }

        public bool IsIPFax
        {
            get
            {
                return InternalItem.IsIPFax;
            }
            set
            {
                InternalItem.IsIPFax = value;
            }
        }


        public bool OriginatorDeliveryReportRequested
        {
            get
            {
                return InternalItem.OriginatorDeliveryReportRequested;
            }
            set
            {
                InternalItem.OriginatorDeliveryReportRequested = value;
            }
        }

        public MSOutlook.OlPermission Permission
        {
            get
            {
                return InternalItem.Permission;
            }
            set
            {
                InternalItem.Permission = value;
            }
        }

        public MSOutlook.OlPermissionService PermissionService
        {
            get
            {
                return InternalItem.PermissionService;
            }
            set
            {
                InternalItem.PermissionService = value;
            }
        }

        public bool ReadReceiptRequested
        {
            get
            {
                return InternalItem.ReadReceiptRequested;
            }
            set
            {
                InternalItem.ReadReceiptRequested = value;
            }
        }

        public string ReceivedByEntryID
        {
            get { return InternalItem.ReceivedByEntryID; }
        }

        public string ReceivedByName
        {
            get { return InternalItem.ReceivedByName; }
        }

        public string ReceivedOnBehalfOfEntryID
        {
            get { return InternalItem.ReceivedOnBehalfOfEntryID; }
        }

        public string ReceivedOnBehalfOfName
        {
            get { return InternalItem.ReceivedOnBehalfOfName; }
        }

        public override DateTime ReceivedTime
        {
            get { return Helpers.LocalToLocal(InternalItem.ReceivedTime); }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_MESSAGE_DELIVERY_TIME, Helpers.LocalToUtc(value));
            }
        }

        public bool RecipientReassignmentProhibited
        {
            get
            {
                return InternalItem.RecipientReassignmentProhibited;
            }
            set
            {
                InternalItem.RecipientReassignmentProhibited = value;
            }
        }


        public DateTime ReminderTime
        {
            get
            {
                return Helpers.LocalToLocal(InternalItem.ReminderTime);
            }
            set
            {
                InternalItem.ReminderTime = Helpers.LocalToLocal(value);
            }
        }

        public MSOutlook.OlRemoteStatus RemoteStatus
        {
            get
            {
                return InternalItem.RemoteStatus;
            }
            set
            {
                InternalItem.RemoteStatus = value;
            }
        }

        new public MSOutlook.MailItem Reply()
        {
            return (OutlookMail)GetItem(() => InternalItem.Reply());
        }

        new public MSOutlook.MailItem ReplyAll()
        {
            return (OutlookMail)GetItem(() => InternalItem.ReplyAll());
        }

        public string ReplyRecipientNames
        {
            get { return InternalItem.ReplyRecipientNames; }
        }

        public MSOutlook.Recipients ReplyRecipients
        {
            get { return InternalItem.ReplyRecipients; }
        }

        public MSOutlook.MAPIFolder SaveSentMessageFolder
        {
            get
            {
                return Application.GetFolder(InternalItem.SaveSentMessageFolder);
            }
            set
            {
                var of = value as OutlookFolder;
                if (of == null)
                    InternalItem.SaveSentMessageFolder = value;
                else
                    InternalItem.SaveSentMessageFolder = of.InternalItem;
            }
        }

        public override string SenderEmailAddress
        {
            get { return InternalItem.SenderEmailAddress; }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_SENDER_EMAIL_ADDRESS, value);
                SafeItem.set_Fields(PropertyIds.PR_SENT_REPRESENTING_EMAIL_ADDRESS, value);
            }
        }

        public string SenderEmailType
        {
            get { return InternalItem.SenderEmailType; }
        }

         public override bool Sent
        {
            get { return InternalItem.Sent; }
            set
            {
                RDOItem.Sent = true;
            }
        }

        public override DateTime SentOn
        {
            get { return Helpers.LocalToLocal(InternalItem.SentOn); }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_CLIENT_SUBMIT_TIME, Helpers.LocalToUtc(value));
            }
        }

        public int? IconIndex
        {
            get
            {
                try
                {
                    return (int?)SafeItem.get_Fields(PropertyIds.PR_ICON_INDEX);
                }
                catch (COMException)
                {
                    return null;
                }
            }
            set
            {
                if (value.HasValue)
                    SafeItem.set_Fields(PropertyIds.PR_ICON_INDEX, null);
                else
                    SafeItem.set_Fields(PropertyIds.PR_ICON_INDEX, value.Value);
            }
        }

        public override string SentOnBehalfOfName
        {
            get
            {
                return InternalItem.SentOnBehalfOfName;
            }
            set
            {
                InternalItem.SentOnBehalfOfName = value;
            }
        }

        public bool Submitted
        {
            get { return InternalItem.Submitted; }
        }


    

        public override string To
        {
            get
            {
               return GetCurrentRecipients(InternalItem.To, MSOutlook.OlMailRecipientType.olTo);               
            }
            set
            {
                InternalItem.To = value;
            }
        }

        private string GetCurrentRecipients(string original, MSOutlook.OlMailRecipientType mailRecipientType)
        {
            var list = new List<String>();
            foreach (var recip in Recipients.Cast<MSOutlook.Recipient>())
            {
                if (recip.Type == (int)mailRecipientType)
                {
                    try
                    {
                        string name = recip.AddressEntry.Name;
                        list.Add(name);
                    }
                    catch (COMException)
                    {
                        list.Add(recip.Name);
                    }
                }
            }

            if (list.Count == 0)
                return original;

            return String.Join(";", list.ToArray());
        }



        public string VotingOptions
        {
            get
            {
                return InternalItem.VotingOptions;
            }
            set
            {
                InternalItem.VotingOptions = value;
            }
        }

        public string VotingResponse
        {
            get
            {
                return InternalItem.VotingResponse;
            }
            set
            {
                InternalItem.VotingResponse = value;
            }
        }

        new public DateTime CreationTime
        {
            get
            {
                return Helpers.LocalToLocal(base.CreationTime);
            }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_CREATION_TIME, Helpers.LocalToUtc(value));
            }
        }

        #endregion

        #region Overrides


        public override void AddBodyText(string text, bool toStart)
        {
            if (String.IsNullOrEmpty(text))
                return;

            string body = GetBodyText();

            switch (BodyFormat)
            {
                case MSOutlook.OlBodyFormat.olFormatHTML:
                    {
                        text = text.Replace(Environment.NewLine, "<br/>");
                        if (toStart)
                        {
                            int pos = body.IndexOf("<body");
                            if (pos >= 0)
                                pos = body.IndexOf('>', pos) + 1;
                            if (pos > 0)
                                body = body.Insert(pos, text);
                            else
                                body = text + body;
                        }
                        else
                        {
                            int pos = body.LastIndexOf("</body>");
                            if (pos >= 0)
                                body = body.Insert(pos, text);
                            else
                                body += text;
                        }
                        HTMLBody = body;
                    }
                    break;
                default:
                    {
                        if (toStart)
                            body = text + body;
                        else
                            body += text;

                        Body = body;
                    }
                    break;
            }

        }


        public override void SetBodyText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return;

            //FIX - 210605 - Invalid Operation Exception at Olswangs
            switch (BodyFormat)
            {

                case MSOutlook.OlBodyFormat.olFormatHTML:
                    HTMLBody = text;
                    break;
                default:
                    Body = text;
                    break;
            }
        }

        public override string GetBodyText()
        {
            try
            {
                switch (BodyFormat)
                {
                    case MSOutlook.OlBodyFormat.olFormatHTML:
                        return HTMLBody;
                    default:
                        return Body;
                }
            }
            catch (COMException)
            {
                return String.Empty;
            }
        }

        protected override void OnAttach(object obj)
        {
            base.OnAttach(obj);

            mail = (MSOutlook.MailItem)obj;
        }

        protected override void OnDetach()
        {
            mail = null;

            base.OnDetach();
        }
     
        #endregion

        #region Redemption

        new protected internal Redemption.SafeMailItem SafeItem
        {
            get
            {
                return (Redemption.SafeMailItem)base.SafeItem;
            }
        }

        new protected internal Redemption.RDOMail RDOItem
        {
            get
            {
                return (Redemption.RDOMail)base.RDOItem;
            }
        }

        #endregion

        #region Getters & Setters

        protected override string GetSenderName(MSOutlook.ItemEvents_10_Event item, Redemption._ISafeItem safeItem)
        {
            var mail = (MSOutlook.MailItem)item;
            return mail.SenderName;
        }

        protected override void SetSenderName(MSOutlook.ItemEvents_10_Event item, Redemption._ISafeItem safeItem, string value)
        {
            safeItem.set_Fields(PropertyIds.PR_SENDER_NAME, value);
        }

        #endregion

    }
}
