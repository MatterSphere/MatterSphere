using System;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public partial class OutlookPost :
        OutlookItem
        , MSOutlook.PostItem
    {

        #region Fields

        private MSOutlook._PostItem  post;


        #endregion

        #region Constructors

        public OutlookPost(MSOutlook.PostItem post)
            : base(post)
        {
        }

        #endregion

        new private MSOutlook._PostItem InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return post;
            }
        }

        #region Redemption

        new protected internal Redemption.SafePostItem SafeItem
        {
            get
            {
                return (Redemption.SafePostItem)base.SafeItem;
            }
        }

        new protected internal Redemption.RDOPostItem RDOItem
        {
            get
            {
                return (Redemption.RDOPostItem)base.RDOItem;
            }
        }

        #endregion


        #region _PostItem Members


        MSOutlook.Application MSOutlook._PostItem.Application
        {
            get { return Application; }
        }

        public void ClearConversationIndex()
        {
            InternalItem.ClearConversationIndex();
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

        new public MSOutlook.MailItem Forward()
        {
            return (OutlookMail)GetItem(() => InternalItem.Forward());
        }



        public void Post()
        {
            InternalItem.Post();
        }



        new public MSOutlook.MailItem Reply()
        {
            return (OutlookMail)GetItem(() => InternalItem.Reply());
        }

        public string SenderEmailType
        {
            get { return InternalItem.SenderEmailType; }
        }

        public bool SetACLs()
        {
            return InternalItem.SetACLs();
        }



        #endregion
    }
}
