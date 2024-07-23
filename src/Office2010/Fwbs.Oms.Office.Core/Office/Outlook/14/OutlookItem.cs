namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookItem
    {
        #region ItemEvents_10_Event Members

        public event MSOutlook.ItemEvents_10_AfterWriteEventHandler AfterWrite
        {
            add
            {
                obj.AfterWrite += value;
            }
            remove
            {
                obj.AfterWrite -= value;
            }
        }

        public event MSOutlook.ItemEvents_10_BeforeReadEventHandler BeforeRead
        {
            add
            {
                obj.BeforeRead += value;
            }
            remove
            {
                obj.BeforeRead -= value;
            }
        }

        #endregion


        public string ConversationID
        {
            get { return GetProperty<string>("ConversationID"); }
        }


        public MSOutlook.Conversation GetConversation()
        {
            return Invoke<MSOutlook.Conversation>("GetConversation");
        }

        public object RTFBody
        {
            get
            {
                return GetProperty<object>("RTFBody");
            }
            set
            {
                SetProperty("RTFBody", value);
            }
        }
    }
}
