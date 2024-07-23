using System;

namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed partial class OutlookSession
        : OutlookObject
        , MSOutlook.NameSpace
    {

        private readonly MSOutlook.NameSpace ns;
        private readonly OutlookApplication app;
        private OutlookFolders folders;

        #region Constructors

        public OutlookSession(OutlookApplication app, MSOutlook.NameSpace ns)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (ns == null)
                throw new ArgumentNullException("ns");

            this.app = app;
            this.ns = ns;

            this.Init(ns);
        }

        #endregion

        public override OutlookApplication Application
        {
            get { return app; }
        }

        internal MSOutlook.NameSpace InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return ns;
            }
        }

        #region _NameSpace Members



        public void AddStore(object Store)
        {
            ns.AddStore(Store);
        }

        public void AddStoreEx(object Store, MSOutlook.OlStoreType Type)
        {
            ns.AddStoreEx(Store, Type);
        }

        public MSOutlook.AddressLists AddressLists
        {
            get { return ns.AddressLists; }
        }

        MSOutlook.Application MSOutlook._NameSpace.Application
        {
            get { return app; }
        }


        public MSOutlook.OlObjectClass Class
        {
            get { return ns.Class; }
        }

 

        public MSOutlook.Recipient CreateRecipient(string RecipientName)
        {
            return ns.CreateRecipient(RecipientName);
        }

  
    
        public MSOutlook.Recipient CurrentUser
        {
            get { return ns.CurrentUser; }
        }

     
        public void Dial(object ContactItem)
        {
            ns.Dial(ContactItem);
        }

        public MSOutlook.OlExchangeConnectionMode ExchangeConnectionMode
        {
            get { return ns.ExchangeConnectionMode; }
        }

    
        public MSOutlook.Folders Folders
        {
            get 
            {
                if (folders == null)
                    folders = new OutlookFolders(app, ns.Folders);
                return folders;
            }
        }

    
        public MSOutlook.MAPIFolder GetDefaultFolder(MSOutlook.OlDefaultFolders FolderType)
        {
            return Application.GetFolder(ns.GetDefaultFolder(FolderType));
        }

        public MSOutlook.MAPIFolder GetFolderFromID(string EntryIDFolder, object EntryIDStore)
        {
            return Application.GetFolder(ns.GetFolderFromID(EntryIDFolder, EntryIDStore));
        }


        public object GetItemFromID(string entryID)
        {
            return GetItemFromID(entryID, Missing);
        }

        public object GetItemFromID(string entryID, object EntryIDStore)
        {
            return GetItem(() => ns.GetItemFromID(entryID, EntryIDStore));
        }

        public MSOutlook.Recipient GetRecipientFromID(string EntryID)
        {
            return ns.GetRecipientFromID(EntryID);
        }


        public MSOutlook.MAPIFolder GetSharedDefaultFolder(MSOutlook.Recipient Recipient, MSOutlook.OlDefaultFolders FolderType)
        {
            return Application.GetFolder(ns.GetSharedDefaultFolder(Recipient, FolderType));
        }

        public void Logoff()
        {
            ns.Logoff();
        }

        public void Logon(object Profile, object Password, object ShowDialog, object NewSession)
        {
            ns.Logon(Profile, Password, ShowDialog, NewSession) ;
        }

        public object MAPIOBJECT
        {
            get { return ns.MAPIOBJECT; }
        }

        public bool Offline
        {
            get { return ns.Offline; }
        }


        public object Parent
        {
            get 
            { //TODO: Check
                return ns.Parent; 
            }
        }

        public Microsoft.Office.Interop.Outlook.MAPIFolder PickFolder()
        {
            var folder = ns.PickFolder();
            if (folder == null)
                return null;

            return Application.GetFolder(folder);
        }

        public void RefreshRemoteHeaders()
        {
            ns.RefreshRemoteHeaders();
        }

        public void RemoveStore(MSOutlook.MAPIFolder Folder)
        {
            var of = Folder as OutlookFolder;
            if (of != null)
                Folder = of.InternalItem;

            ns.RemoveStore(Folder);
        }

        public MSOutlook.NameSpace Session
        {
            get { return this; }
        }

   
        public MSOutlook.SyncObjects SyncObjects
        {
            get { return ns.SyncObjects; }
        }

        public string Type
        {
            get { return ns.Type; }
        }

        #endregion

        #region NameSpaceEvents_Event Members


        public event Microsoft.Office.Interop.Outlook.NameSpaceEvents_OptionsPagesAddEventHandler OptionsPagesAdd
        {
            add
            {
                ns.OptionsPagesAdd += value;
            }
            remove
            {
                ns.OptionsPagesAdd -= value;
            }
        }

        #endregion
    }
}
