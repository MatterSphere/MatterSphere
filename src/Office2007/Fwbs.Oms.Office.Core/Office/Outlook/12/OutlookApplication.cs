namespace Fwbs.Office.Outlook
{

    using MSOffice = Microsoft.Office.Core;
    using MSOutlook = Microsoft.Office.Interop.Outlook;


    partial class OutlookApplication
    {

        private OfficeCommandBar contextbar;

        private void app_ItemContextMenuDisplay(Microsoft.Office.Core.CommandBar CommandBar, MSOutlook.Selection Selection)
        {
            if (contextbar != null)
            {
                contextbar.Dispose();
                contextbar = null;
            }

            var ev = ItemContextMenuDisplay;
            if (ev != null)
            {
                contextbar = new OfficeCommandBar(CommandBar);
                ev(contextbar, new OutlookSelection(this, Selection));
            }
        }

        private void app_FolderContextMenuDisplay(Microsoft.Office.Core.CommandBar CommandBar, Microsoft.Office.Interop.Outlook.MAPIFolder Folder)
        {
            var ev = FolderContextMenuDisplay;
            if (ev != null)
            {
                ev(CommandBar, GetFolder(Folder));
            }
        }

  
        public event MSOutlook.ApplicationEvents_11_AttachmentContextMenuDisplayEventHandler AttachmentContextMenuDisplay
        {
            add
            {
                app.AttachmentContextMenuDisplay += value;
            }
            remove
            {
                app.AttachmentContextMenuDisplay -= value;
            }
        }

        public event MSOutlook.ApplicationEvents_11_BeforeFolderSharingDialogEventHandler BeforeFolderSharingDialog
        {
            add
            {
                app.BeforeFolderSharingDialog += value;
            }
            remove
            {
                app.BeforeFolderSharingDialog -= value;
            }
        }

        public event MSOutlook.ApplicationEvents_11_ContextMenuCloseEventHandler ContextMenuClose
        {
            add
            {
                app.ContextMenuClose += value;
            }
            remove
            {
                app.ContextMenuClose -= value;
            }
        }

        public event MSOutlook.ApplicationEvents_11_FolderContextMenuDisplayEventHandler FolderContextMenuDisplay;

        public event MSOutlook.ApplicationEvents_11_ItemContextMenuDisplayEventHandler ItemContextMenuDisplay;

        public event MSOutlook.ApplicationEvents_11_ItemLoadEventHandler ItemLoad
        {
            add
            {
                app.ItemLoad += value;
            }
            remove
            {
                app.ItemLoad -= value;
            }
        }

        public event MSOutlook.ApplicationEvents_11_ShortcutContextMenuDisplayEventHandler ShortcutContextMenuDisplay
        {
            add
            {
                app.ShortcutContextMenuDisplay += value;
            }
            remove
            {
                app.ShortcutContextMenuDisplay -= value;
            }

        }

        public event MSOutlook.ApplicationEvents_11_StoreContextMenuDisplayEventHandler StoreContextMenuDisplay
        {
            add
            {
                app.StoreContextMenuDisplay += value;
            }
            remove
            {
                app.StoreContextMenuDisplay -= value;
            }
        }

        public event MSOutlook.ApplicationEvents_11_ViewContextMenuDisplayEventHandler ViewContextMenuDisplay
        {
            add
            {
                app.ViewContextMenuDisplay += value;
            }
            remove
            {
                app.ViewContextMenuDisplay -= value;
            }
        }

        public bool IsTrusted
        {
            get { return app.IsTrusted; }
        }


        public object GetObjectReference(object Item, MSOutlook.OlReferenceType ReferenceType)
        {
            return LoadedItems.GetItem(() => app.GetObjectReference(Item, ReferenceType));
        }

        public MSOffice.IAssistance Assistance
        {
            get { return app.Assistance; }
        }

        public MSOutlook.TimeZones TimeZones
        {
            get { return app.TimeZones; }
        }

        public string DefaultProfileName
        {
            get { return app.DefaultProfileName; }
        }



    }
}
