namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookItem
    {

        public event MSOutlook.ItemEvents_10_AttachmentRemoveEventHandler AttachmentRemove
        {
            add
            {
                obj.AttachmentRemove += value;
            }
            remove
            {
                obj.AttachmentRemove -= value;
            }
        }

        public event MSOutlook.ItemEvents_10_BeforeAttachmentAddEventHandler BeforeAttachmentAdd
        {
            add
            {
                obj.BeforeAttachmentAdd += value;
            }
            remove
            {
                obj.BeforeAttachmentAdd -= value;
            }
        }

        public event MSOutlook.ItemEvents_10_BeforeAttachmentPreviewEventHandler BeforeAttachmentPreview
        {
            add
            {
                obj.BeforeAttachmentPreview += value;
            }
            remove
            {
                obj.BeforeAttachmentPreview -= value;
            }
        }


        public event MSOutlook.ItemEvents_10_BeforeAttachmentReadEventHandler BeforeAttachmentRead
        {
            add
            {
                obj.BeforeAttachmentRead += value;
            }
            remove
            {
                obj.BeforeAttachmentRead -= value;
            }
        }

        public event MSOutlook.ItemEvents_10_BeforeAttachmentWriteToTempFileEventHandler BeforeAttachmentWriteToTempFile
        {
            add
            {
                obj.BeforeAttachmentWriteToTempFile += value;
            }
            remove
            {
                obj.BeforeAttachmentWriteToTempFile -= value;
            }
        }

        public event MSOutlook.ItemEvents_10_BeforeAutoSaveEventHandler BeforeAutoSave
        {
            add
            {
                obj.BeforeAutoSave += value;
            }
            remove
            {
                obj.BeforeAutoSave -= value;
            }
        }

        public event MSOutlook.ItemEvents_10_UnloadEventHandler Unload
        {
            add
            {
                obj.Unload += value;
            }
            remove
            {
                obj.Unload -= value;
            }

        }
        public MSOutlook.Account SendUsingAccount
        {
            get
            {
                return GetProperty<MSOutlook.Account>("SendUsingAccount");
            }
            set
            {
                SetProperty("SendUsingAccount", value);
            }
        }

        public MSOutlook.PropertyAccessor PropertyAccessor
        {
            get
            {
                return GetProperty<MSOutlook.PropertyAccessor>("PropertyAccessor");
            }
        }
    }
}
