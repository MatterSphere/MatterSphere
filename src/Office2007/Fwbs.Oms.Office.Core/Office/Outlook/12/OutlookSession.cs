namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    partial class OutlookSession
    {
        public event Microsoft.Office.Interop.Outlook.NameSpaceEvents_AutoDiscoverCompleteEventHandler AutoDiscoverComplete
        {
            add
            {
                ns.AutoDiscoverComplete += value;
            }
            remove
            {
                ns.AutoDiscoverComplete -= value;
            }
        }

        public MSOutlook.Accounts Accounts
        {
            get { return ns.Accounts; }
        }

        public MSOutlook.OlAutoDiscoverConnectionMode AutoDiscoverConnectionMode
        {
            get { return ns.AutoDiscoverConnectionMode; }
        }

        public string AutoDiscoverXml
        {
            get { return ns.AutoDiscoverXml; }
        }

        public MSOutlook.Categories Categories
        {
            get { return ns.Categories; }
        }

        public bool CompareEntryIDs(string FirstEntryID, string SecondEntryID)
        {
            return ns.CompareEntryIDs(FirstEntryID, SecondEntryID);
        }

        public string CurrentProfileName
        {
            get { return ns.CurrentProfileName; }
        }


        public MSOutlook.SharingItem CreateSharingItem(object Context, object Provider)
        {
            return ns.CreateSharingItem(Context, Provider);
        }

        public MSOutlook.Store DefaultStore
        {
            get { return ns.DefaultStore; }
        }

        public string ExchangeMailboxServerName
        {
            get { return ns.ExchangeMailboxServerName; }
        }

        public string ExchangeMailboxServerVersion
        {
            get { return ns.ExchangeMailboxServerVersion; }
        }


        public MSOutlook.AddressEntry GetAddressEntryFromID(string ID)
        {
            return ns.GetAddressEntryFromID(ID);
        }


        public MSOutlook.AddressList GetGlobalAddressList()
        {
            return ns.GetGlobalAddressList();
        }

        public MSOutlook.SelectNamesDialog GetSelectNamesDialog()
        {
            return ns.GetSelectNamesDialog();
        }

        public MSOutlook.Store GetStoreFromID(string ID)
        {
            return ns.GetStoreFromID(ID);
        }


        public MSOutlook.MAPIFolder OpenSharedFolder(string Path, object Name, object DownloadAttachments, object UseTTL)
        {
            return Application.GetFolder(ns.OpenSharedFolder(Path, Name, DownloadAttachments, UseTTL));
        }

        public object OpenSharedItem(string Path)
        {
            //TODO: Check to see what this returns
            return ns.OpenSharedItem(Path);
        }

        public void SendAndReceive(bool showProgressDialog)
        {
            ns.SendAndReceive(showProgressDialog);
        }

        public MSOutlook.Stores Stores
        {
            get { return ns.Stores; }
        }



    }
}
