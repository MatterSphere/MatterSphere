using System;

namespace Fwbs.Office.Outlook
{
    using System.Runtime.InteropServices;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed partial class OutlookFolder :
        OutlookObject,
        MSOutlook.MAPIFolder
    {

        #region Fields

        private MSOutlook.MAPIFolder folder;
        private OutlookFolders folders;
        private Redemption.RDOFolder2 rdofolder;
        private Redemption.RDOFolderFields folderfields;
        private string entryid;
        private string storeid;
        private OutlookItems items;

        #endregion

        #region Constructors

        public OutlookFolder(MSOutlook.MAPIFolder folder)
        {
            if (folder == null)
                throw new ArgumentNullException("folder");

            this.folder = folder;

            this.Init(this.folder);

            this.entryid = InternalEntryID;
            this.storeid = InternalStoreID;
        }

        #endregion

        #region Overrides

        public override OutlookApplication Application
        {
            get
            {
                return OutlookApplication.GetApplication(InternalItem.Application);
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (hiddenitem != null)
                    {
                        hiddenitem.Dispose();
                        hiddenitem = null;
                    }

                    if (folderfields != null)
                    {
                        if (Marshal.IsComObject(folderfields))
                            Marshal.FinalReleaseComObject(folderfields);
                        folderfields = null;
                    }

                    if (rdofolder != null)
                    {
                        if (Marshal.IsComObject(rdofolder))
                            Marshal.FinalReleaseComObject(rdofolder);
                        rdofolder = null;
                    }

                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }


        #endregion

        #region Redemption

        private void SetupRDOFolder()
        {
            if (rdofolder == null)
                rdofolder = (Redemption.RDOFolder2)Application.RDOSession.GetFolderFromID(entryid, storeid, Type.Missing);
        }

        private Redemption.RDOFolder2 RDOFolder
        {
            get
            {
                SetupRDOFolder();

                return rdofolder;
            }
        }

        internal Redemption.RDOFolderFields FolderFields
        {
            get
            {
                if (folderfields == null)
                    folderfields = RDOFolder.FolderFields;
                return folderfields;
            }
        }


        #endregion

        #region Properties

        public bool IsSearchFolder
        {
            get
            {
                return RDOFolder.FolderKind == Redemption.rdoFolderKind.fkSearch;
            }

        }

        public bool IsDeleted
        {
            get
            {
                try
                {
                    entryid = InternalEntryID;
                    return entryid == null;
                }
                catch (COMException comex)
                {
                    if (comex.ErrorCode == HResults.E_OBJECT_DELETED_OR_MOVED)
                        return true;

                    return false;
                }
            }

        }

        public string EntryID
        {
            get
            {
                return entryid;
            }
        }

        private string InternalEntryID
        {
            get
            {
                return InternalItem.EntryID;
            }
        }

        public string StoreID
        {
            get
            {
                return storeid;
            }
        }

        public string InternalStoreID
        {
            get
            {
                return InternalItem.StoreID;
            }
        }


        internal MSOutlook.MAPIFolder InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return folder;
            }
        }

        #endregion

        #region Methods

        public OutlookItem Import(string file, string defaultSender, string defaultSenderEmail, DateTime? defaultSentDate, bool? defaultSent, bool forceSenderUpdate = false)
        {
            OutlookItem item = null;
            if (String.IsNullOrEmpty(file))
                return item;

            //Uses redemption to import the msg file
            Redemption.SafeMailItem sm = null;
            Redemption.RDOMail rdom = null;

            try
            {
                sm = Redemption.RedemptionFactory.Default.CreateSafeMailItem();

                rdom = RDOFolder.Items.Add(Type.Missing);

                sm.Item = rdom;
                sm.set_Fields(PropertyIds.PR_ICON_INDEX, null);
                sm.Import(file, (int)Redemption.rdoSaveAsType.olMSGUnicode);


                if (defaultSent.HasValue)
                {
                    try
                    {
                        rdom.Sent = defaultSent.Value;
                    }
                    catch (COMException comex)
                    {
                        if (comex.ErrorCode != HResults.MAPI_E_COMPUTED)
                            throw;
                    }
                }
                rdom.UnRead = false;

                if (rdom.SenderName == null || forceSenderUpdate)
                {
                    if (!String.IsNullOrEmpty(defaultSender))
                    {
                        sm.set_Fields(PropertyIds.PR_SENDER_NAME, defaultSender);
                    }
                }

                if (String.IsNullOrEmpty(defaultSenderEmail))
                {
                    defaultSenderEmail = defaultSender;
                }

                if (rdom.SenderEmailAddress == null || forceSenderUpdate)
                {
                    if (!String.IsNullOrEmpty(defaultSenderEmail))
                    {
                        sm.set_Fields(PropertyIds.PR_SENDER_EMAIL_ADDRESS, defaultSenderEmail);
                        sm.set_Fields(PropertyIds.PR_SENT_REPRESENTING_EMAIL_ADDRESS, defaultSenderEmail);
                        if (forceSenderUpdate) rdom.SenderEmailType = "SMTP";
                    }
                }

                if (rdom.SentOn <= OutlookConstants.MAX_DATE)
                {
                    if (defaultSentDate.HasValue)
                    {
                        sm.set_Fields(PropertyIds.PR_CLIENT_SUBMIT_TIME, Helpers.LocalToUtc(defaultSentDate.Value));
                        sm.set_Fields(PropertyIds.PR_CREATION_TIME, Helpers.LocalToUtc(defaultSentDate.Value));
                        sm.set_Fields(PropertyIds.PR_MESSAGE_DELIVERY_TIME, Helpers.LocalToUtc(defaultSentDate.Value));
                    }
                }


                rdom.Save();

                var realitem = Application.Session.InternalItem.GetItemFromID(rdom.EntryID, InternalItem.StoreID);
                item = Application.GetItem(realitem, true);

                return item;
            }
            finally
            {
                if (sm != null)
                {
                    Marshal.FinalReleaseComObject(sm);
                    sm = null;
                }

                if (rdom != null)
                {
                    Marshal.FinalReleaseComObject(rdom);
                    rdom = null;
                }

                if (item != null)
                {
                    item.IsPinned = false;
                }
            }
        }

        #endregion

        #region Fields

        private OutlookItem hiddenitem;

        public OutlookItem GetHiddenMessage(string key, bool create)
        {
            if (hiddenitem != null)
            {
                hiddenitem.SynchroniseRealUserProperties();
                
                return hiddenitem;
            }

            const string HiddenMessageID = "{297A22C6-6245-4c0f-B50F-56998081C2E3}";

            var messages = RDOFolder.HiddenItems;

            var rdomail = messages.Find(String.Format("[Subject] = '{0}_{1}", HiddenMessageID, (key ?? String.Empty).Replace("'", "''") + "'"));
            if (rdomail == null)
            {
                rdomail = messages.Find(String.Format("[Subject] = '{0}_'", HiddenMessageID));
                if (rdomail == null)
                    rdomail = messages.Find(String.Format("[Subject] = '{0}'", HiddenMessageID));
            }

            //Let the folder manage the lifetime of the hidden message item.
            //Having it in the loaded items seems to remove it when selection is changed or
            //Inspectors are closed which refreshes the loaded items list.
            if (rdomail != null)
            {
                hiddenitem = OutlookItemFactory.Create(Application.Session.InternalItem.GetItemFromID(rdomail.EntryID, rdomail.Store.EntryID), true);
                //Going backk to Outlook properties for the hidden storage items
                // So must synchronise the redemption properties with the
                //Outlook ones.
                hiddenitem.SynchroniseRealUserProperties();
                return hiddenitem;
            }

            if (create)
            {
                rdomail = messages.Add("IPM.Note");
                rdomail.Subject = String.Format("{0}_{1}", HiddenMessageID,  (key ?? String.Empty).Replace("'", "''"));
                rdomail.Body = "OMS Folder Settings";
                rdomail.Save();
                hiddenitem = OutlookItemFactory.Create(Application.Session.InternalItem.GetItemFromID(rdomail.EntryID, rdomail.Store.EntryID), true);
                return hiddenitem;
            }
            return hiddenitem;

        }

        #endregion

        #region MAPIFolder Members

        public void AddToFavorites(object fNoUI, object Name)
        {
            InternalItem.AddToFavorites(fNoUI, Name);
        }

        public void AddToPFFavorites()
        {
            InternalItem.AddToPFFavorites();
        }

        public string AddressBookName
        {
            get
            {
                return InternalItem.AddressBookName;
            }
            set
            {
                InternalItem.AddressBookName = value;
            }
        }

        MSOutlook.Application MSOutlook.MAPIFolder.Application
        {
            get { return OutlookApplication.GetApplication(InternalItem.Application); }
        }

        public MSOutlook.OlObjectClass Class
        {
            get { return InternalItem.Class; }
        }

        public MSOutlook.MAPIFolder CopyTo(MSOutlook.MAPIFolder DestinationFolder)
        {
            var of = DestinationFolder as OutlookFolder;
            if (of == null)
                return Application.GetFolder(InternalItem.CopyTo(DestinationFolder));
            else
                return Application.GetFolder(InternalItem.CopyTo(of.InternalItem));
        }

        public Microsoft.Office.Interop.Outlook.View CurrentView
        {
            get { return InternalItem.CurrentView; }
        }

        public bool CustomViewsOnly
        {
            get
            {
                return InternalItem.CustomViewsOnly;
            }
            set
            {
                InternalItem.CustomViewsOnly = value;
            }
        }

        public MSOutlook.OlItemType DefaultItemType
        {
            get { return InternalItem.DefaultItemType; }
        }

        public string DefaultMessageClass
        {
            get { return InternalItem.DefaultMessageClass; }
        }

        public void Delete()
        {
            InternalItem.Delete();
        }

        public string Description
        {
            get
            {
                return InternalItem.Description;
            }
            set
            {
                InternalItem.Description = value;
            }
        }

        public void Display()
        {
            Execute(() => InternalItem.Display());
        }

        private void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (COMException)
            {
                var newobj = Application.Session.InternalItem.GetFolderFromID(entryid, storeid);

                Detach();

                folder = newobj;

                Attach(folder);

                action();
            }
        }

        public string FolderPath
        {
            get { return InternalItem.FullFolderPath; }
        }

        public MSOutlook.Folders Folders
        {
            get
            {
                if (folders == null)
                    folders = new OutlookFolders(Application, InternalItem.Folders);

                return folders;
            }
        }

        public string FullFolderPath
        {
            get { return InternalItem.FullFolderPath; }
        }


        public MSOutlook.Explorer GetExplorer(object DisplayMode)
        {
            var exp = InternalItem.GetExplorer(DisplayMode);
            if (exp == null)
                return null;
            return Application.GetExplorer(exp);
        }



        public bool InAppFolderSyncObject
        {
            get
            {
                return InternalItem.InAppFolderSyncObject;
            }
            set
            {
                InternalItem.InAppFolderSyncObject = value;
            }
        }

        public bool IsSharePointFolder
        {
            get { return InternalItem.IsSharePointFolder; }
        }



        public MSOutlook.Items Items
        {
            get
            {
                if (items == null)
                    items = new OutlookItems(InternalItem.Items);

                return items;
            }
        }

        public object MAPIOBJECT
        {
            get { return InternalItem.MAPIOBJECT; }
        }

        public void MoveTo(MSOutlook.MAPIFolder DestinationFolder)
        {
            var of = DestinationFolder as OutlookFolder;
            if (of == null)
                InternalItem.MoveTo(DestinationFolder);
            else
                InternalItem.MoveTo(of.InternalItem);
        }

        public string Name
        {
            get
            {
                return InternalItem.Name;
            }
            set
            {
                InternalItem.Name = value;
            }
        }

        public object Parent
        {
            get
            {
                //May to to wrap up the folders here
                var parent = InternalItem.Parent;

                var f = parent as MSOutlook.MAPIFolder;
                if (f != null)
                    return Application.GetFolder(f);

                var ns = parent as MSOutlook.NameSpace;
                if (ns != null)
                    return Application.GetSession(ns);

                return parent;
            }
        }



        public MSOutlook.NameSpace Session
        {
            get { return Application.Session; }
        }

        public bool ShowAsOutlookAB
        {
            get
            {
                return InternalItem.ShowAsOutlookAB;
            }
            set
            {
                InternalItem.ShowAsOutlookAB = value;
            }
        }

        public MSOutlook.OlShowItemCount ShowItemCount
        {
            get
            {
                return InternalItem.ShowItemCount;
            }
            set
            {
                InternalItem.ShowItemCount = value;
            }
        }


        public int UnReadItemCount
        {
            get { return InternalItem.UnReadItemCount; }
        }


        public object UserPermissions
        {
            get { return InternalItem.UserPermissions; }
        }

        public MSOutlook.Views Views
        {
            get { return InternalItem.Views; }
        }

        public bool WebViewAllowNavigation
        {
            get
            {
                return InternalItem.WebViewAllowNavigation;
            }
            set
            {
                InternalItem.WebViewAllowNavigation = value;
            }
        }

        public bool WebViewOn
        {
            get
            {
                return InternalItem.WebViewOn;
            }
            set
            {
                InternalItem.WebViewOn = value;
            }
        }

        public string WebViewURL
        {
            get
            {
                return InternalItem.WebViewURL;
            }
            set
            {
                InternalItem.WebViewURL = value;
            }
        }

        #endregion
    }
}
