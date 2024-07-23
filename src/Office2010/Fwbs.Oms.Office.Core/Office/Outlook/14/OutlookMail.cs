namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookMail
    {
        #region _MailItem Members


        public string PermissionTemplateGuid
        {
            get
            {
                return InternalItem.PermissionTemplateGuid;
            }
            set
            {
                InternalItem.PermissionTemplateGuid = value;
            }
        }

        public System.DateTime RetentionExpirationDate
        {
            get { return InternalItem.RetentionExpirationDate; }
        }

        public string RetentionPolicyName
        {
            get { return InternalItem.RetentionPolicyName; }
        }

        public MSOutlook.AddressEntry Sender
        {
            get
            {
                return InternalItem.Sender;
            }
            set
            {
                InternalItem.Sender = value;
            }
        }

        #endregion
    }
}
