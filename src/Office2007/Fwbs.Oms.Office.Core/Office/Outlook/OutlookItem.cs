using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Redemption;
using MSOutlook = Microsoft.Office.Interop.Outlook;

namespace Fwbs.Office.Outlook
{
    public partial class OutlookItem : OutlookObject, MSOutlook.ItemEvents_10_Event
    {

        #region Fields

        private MSOutlook.ItemEvents_10_Event obj;
        private Redemption.IRDOMailEvents_Event rdoitem;
        private Redemption._ISafeItem safeitem;
        private MSOutlook.Application application;
        private object parent;

        private string entryid;
        private readonly string temporaryid;
        private string storeid;
        private string folderid;
        private string subject;
        private bool inputaddress;
        private MSOutlook.OlObjectClass cls;
        private string messageclass;
        private string sendername;

        #endregion

        #region Constructors

        public OutlookItem(MSOutlook.ItemEvents_10_Event obj)
            : base(true)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            this.temporaryid = Guid.NewGuid().ToString();


            Init(obj);
            Build(obj);

        }

        protected override void Init(object obj)
        {

        }

        private void Build(MSOutlook.ItemEvents_10_Event item)
        {

            var disp = CreateCOMWrapper(item);

            application = GetApplication(disp.Item1, disp.Item2);
            parent = GetParent(disp.Item1, disp.Item2);
            entryid = GetEntryID(disp.Item1, disp.Item2);
            storeid = GetStoreID(disp.Item1, disp.Item2);
            folderid = GetFolderID(disp.Item1, disp.Item2);
            subject = GetSubject(disp.Item1, disp.Item2);
            cls = GetClass(disp.Item1, disp.Item2);

            if (!(item is MSOutlook.StorageItem))
            {
                messageclass = GetMessageClass(disp.Item1, disp.Item2);
                sendername = GetSenderName(disp.Item1, disp.Item2);

            }
            //Do not dispose of COM wrappers.

        }

        #endregion

        #region Properties

        public override OutlookApplication Application
        {
            get
            {
                return OutlookApplication.GetApplication(application);
            }
        }


        internal MSOutlook.ItemEvents_10_Event InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return obj;
            }
        }

        public string Subject
        {
            get
            {
                return Get(ref subject, GetSubject);
            }
            set
            {
                Set(ref subject, SetSubject, value);
            }
        }

        public string Body
        {
            get
            {
                return GetProperty<string>("Body");
            }
            set
            {
                SetProperty("Body", value);
            }
        }


        public MSOutlook.Application InternalApplication
        {
            get
            {
                return application;
            }
        }

        private MSOutlook.UserProperties userprops;
        public MSOutlook.UserProperties UserProperties
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                if (InternalItem is MSOutlook.StorageItem || (Application.Settings.IsConnected() == false && Application.IsAddinInstance) )
                {
                    return RealUserProperties;
                }
                else
                {
                    if (userprops == null)
                        userprops = new RedemptionUserProperties(this);
                    return userprops;
                }
            }
        }

        public MSOutlook.UserProperties RealUserProperties
        {
            get
            {
                return GetProperty<MSOutlook.UserProperties>("UserProperties");
            }
        }

        internal void SynchroniseRealUserProperties()
        {
            if (RealUserProperties.Count != 0)
                return;

            if (userprops == null)
                userprops = new RedemptionUserProperties(this);

            foreach (var redprop in userprops.OfType<MSOutlook.UserProperty>())
            {
                MSOutlook.UserProperty realprop = null;

                try
                {
                    realprop = this.RealUserProperties[redprop.Name];
                    if (realprop != null)
                        realprop.Delete();
                }
                catch (COMException)
                {
                }

                realprop = RealUserProperties.Add(redprop.Name, redprop.Type, false, Type.Missing);
                realprop.Value = redprop.Value;

            }

            Invoke<object>("Save");
        }

        public MSOutlook.Inspector GetInspector
        {
            get
            {
                //This code was put in incase the inspector thinks it has been deleted or corrupted.
                if (insp != null)
                {
                    if (insp.IsDeleted)
                    {
                        Application.RemoveInspector(insp);
                        insp = null;
                    }
                }

                if (insp == null)
                {
                    //Should force the load of the inspector.
                    var olinsp = GetProperty<MSOutlook.Inspector>("GetInspector");
                    if (insp == null)
                        insp = Application.GetInspector(olinsp);
                }
                return insp;

            }
        }

        public MSOutlook.Attachments Attachments
        {
            get
            {
                CheckIfDetached();

                try
                {
                    return GetProperty<MSOutlook.Attachments>("Attachments");
                }
                catch (COMException comex)
                {
                    if (HResults.IsMissing(comex))
                        return new EmptyAttachments();
                    throw;
                }
            }
        }

        internal Redemption.Attachments SafeAttachments
        {
            get
            {
                return GetPropertyEx<Redemption.Attachments>(SafeItem, "Attachments");
            }
        }

        public string Categories
        {
            get
            {
                return GetProperty<string>("Categories");
            }
            set
            {
                SetProperty("Categories", value);
            }
        }


        public MSOutlook.Actions Actions
        {
            get
            {
                return GetProperty<MSOutlook.Actions>("Actions");
            }
        }


        public bool AutoResolvedWinner
        {
            get
            {
                return GetProperty<bool>("AutoResolvedWinner");
            }
        }


        public string BillingInformation
        {
            get
            {
                return GetProperty<string>("BillingInformation");
            }
            set
            {
                SetProperty("BillingInformation", value);
            }
        }


        public MSOutlook.OlObjectClass Class
        {
            get
            {
                return Get(ref cls, GetClass);
            }
        }


        public string Companies
        {
            get
            {
                return GetProperty<string>("Companies");
            }
            set
            {
                SetProperty("Companies", value);
            }

        }


        public MSOutlook.Conflicts Conflicts
        {
            get
            {
                return GetProperty<MSOutlook.Conflicts>("Conflicts");
            }
        }

        public string ConversationIndex
        {
            get
            {
                try
                {
                    return GetProperty<string>("ConversationIndex");
                }
                catch (COMException comex)
                {
                    if (HResults.IsMissing(comex))
                        return null;

                    throw;
                }
            }
        }

        public string ConversationTopic
        {
            get
            {
                try
                {
                    return GetProperty<string>("ConversationTopic");
                }
                catch (COMException comex)
                {
                    if (HResults.IsMissing(comex))
                        return null;

                    throw;
                }
            }
        }

        public DateTime CreationTime
        {
            get
            {
                return Helpers.LocalToLocal(GetProperty<DateTime>("CreationTime"));
            }
        }


        public MSOutlook.OlDownloadState DownloadState
        {
            get
            {
                return GetProperty<MSOutlook.OlDownloadState>("DownloadState");
            }
        }

        public bool IsNew
        {
            get
            {
                return String.IsNullOrEmpty(EntryID);
            }
        }

        public bool IsDraft
        {
            get
            {
                return IsNew == false && Sent == false;
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
                catch (InvalidComObjectException)
                {
                    return true;
                }
            }
        }


        public string EntryID
        {
            get
            {
                return Get(ref entryid, GetEntryID);
            }
        }

        internal string TemporaryID
        {
            get
            {
                return temporaryid;
            }
        }

        private string InternalEntryID
        {
            get
            {
                return GetProperty<string>("EntryID");
            }
        }

        public MSOutlook.FormDescription FormDescription
        {
            get
            {
                return GetProperty<MSOutlook.FormDescription>("FormDescription");
            }
        }


        public MSOutlook.OlImportance Importance
        {
            get
            {
                return GetProperty<MSOutlook.OlImportance>("Importance");
            }
            set
            {
                SetProperty("Importance", value);
            }
        }


        public int InternetCodepage
        {
            get
            {
                return GetProperty<int>("InternetCodepage");
            }
            set
            {
                SetProperty("InternetCodepage", value);
            }
        }

        public bool IsConflict
        {
            get
            {
                return GetProperty<bool>("IsConflict");
            }
        }


        public MSOutlook.ItemProperties ItemProperties
        {
            get
            {
                return GetProperty<MSOutlook.ItemProperties>("ItemProperties");
            }
        }


        public DateTime LastModificationTime
        {
            get
            {
                return Helpers.LocalToLocal(GetProperty<DateTime>("LastModificationTime"));
            }
        }

        public MSOutlook.Links Links
        {
            get
            {
                return GetProperty<MSOutlook.Links>("Links");
            }
        }

        public object MAPIOBJECT
        {
            get
            {
                try
                {
                    return GetProperty<object>("MAPIOBJECT");
                }
                catch (COMException)
                {
                    return GetPropertyEx<object>(RDOItem, "MAPIOBJECT");
                }
            }
        }

        public MSOutlook.OlRemoteStatus MarkForDownload
        {
            get
            {
                return GetProperty<MSOutlook.OlRemoteStatus>("MarkForDownload");
            }
            set
            {
                SetProperty("MarkForDownload", value);
            }
        }


        public string MessageClass
        {
            get
            {
                return Get(ref messageclass, GetMessageClass);
            }
            set
            {
                Set(ref messageclass, SetMessageClass, value);
            }
        }

        public string MessageHeaders
        {
            get
            {
                return (string)SafeItem.get_Fields(PropertyIds.PR_TRANSPORT_MESSAGE_HEADERS);
            }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_TRANSPORT_MESSAGE_HEADERS, value);
            }
        }

        public string Mileage
        {
            get
            {
                return GetProperty<string>("Mileage");
            }
            set
            {
                SetProperty("Mileage", value);
            }
        }


        public int OutlookInternalVersion
        {
            get
            {
                return GetProperty<int>("OutlookInternalVersion");
            }
        }


        public string OutlookVersion
        {
            get
            {
                return GetProperty<string>("OutlookVersion");
            }
        }

        public virtual OutlookFolder Folder
        {
            get
            {
                return GetParentFolder(Parent) as OutlookFolder;
            }
        }

        private object GetParentFolder(object current)
        {
            var folder = current as OutlookFolder;
            if (folder != null)
                return folder;


            return null;
        }

        public object Parent
        {
            get
            {
                var parent = Get(ref this.parent, GetParent);

                var folder = parent as MSOutlook.MAPIFolder;
                if (folder != null)
                    return Application.GetFolder(folder);

                var ns = parent as MSOutlook.NameSpace;
                if (ns != null)
                    return Application.GetSession(ns);


                var item = parent as MSOutlook.ItemEvents_10_Event;
                if (item != null)
                    return Application.GetItem(item);

                return parent;
            }
        }



        public MSOutlook.Recipients Recipients
        {
            get
            {
                try
                {
                    return GetProperty<MSOutlook.Recipients>("Recipients");
                }
                catch (COMException comex)
                {
                    if (comex.ErrorCode == HResults.E_DISP_UNKNOWN)
                        return null;
                    throw;
                }
            }
        }

        public bool Saved
        {
            get
            {
                return GetProperty<bool>("Saved");
            }
        }





        public MSOutlook.OlSensitivity Sensitivity
        {
            get
            {
                return GetProperty<MSOutlook.OlSensitivity>("Sensitivity");
            }
            set
            {
                SetProperty("Sensitivity", value);
            }
        }

        public MSOutlook.NameSpace Session
        {
            get
            {
                return Application.Session;
            }
        }


        public int Size
        {
            get
            {
                return GetProperty<int>("Size");
            }
        }


        public bool UnRead
        {
            get
            {
                return GetProperty<bool>("UnRead");
            }
            set
            {

                SetProperty("UnRead", value);
            }
        }


        public bool ReminderOverrideDefault
        {
            get
            {
                return GetProperty<bool>("ReminderOverrideDefault");
            }
            set
            {
                SetProperty("ReminderOverrideDefault", value);
            }
        }

        public bool ReminderPlaySound
        {
            get
            {
                return GetProperty<bool>("ReminderPlaySound");
            }
            set
            {
                SetProperty("ReminderPlaySound", value);
            }
        }

        public bool ReminderSet
        {
            get
            {
                return GetProperty<bool>("ReminderSet");
            }
            set
            {
                SetProperty("ReminderSet", value);
            }
        }

        public string ReminderSoundFile
        {
            get
            {
                return GetProperty<string>("ReminderSoundFile");
            }
            set
            {
                SetProperty("ReminderSoundFile", value);
            }
        }

        public bool NoAging
        {
            get
            {
                return GetProperty<bool>("NoAging");
            }
            set
            {
                SetProperty("NoAging", value);
            }
        }

        public virtual DateTime ReceivedTime
        {
            get { return Helpers.UtcToLocal(SafeItem.get_Fields(PropertyIds.PR_MESSAGE_DELIVERY_TIME)); }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_MESSAGE_DELIVERY_TIME, Helpers.LocalToUtc(value));

            }
        }

        public string SenderName
        {
            get
            {
                return Get(ref sendername, GetSenderName);
            }
            set
            {
                Set(ref sendername, SetSenderName, value);
            }
        }


        public virtual string SenderEmailAddress
        {
            get
            {
                var val = (string)SafeItem.get_Fields(PropertyIds.PR_SENDER_EMAIL_ADDRESS);
                if (String.IsNullOrEmpty(val))
                    val = (string)SafeItem.get_Fields(PropertyIds.PR_SENT_REPRESENTING_EMAIL_ADDRESS);
                return val;
            }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_SENDER_EMAIL_ADDRESS, value);
                SafeItem.set_Fields(PropertyIds.PR_SENT_REPRESENTING_EMAIL_ADDRESS, value);
            }
        }

        public virtual bool Sent
        {
            get
            {
                var flags = (PropertyIds.MessageFlags)SafeItem.get_Fields(PropertyIds.PR_MESSAGE_FLAGS);

                return (flags | PropertyIds.MessageFlags.UnSent) != flags;
            }
            set
            {
                var flags = (PropertyIds.MessageFlags)SafeItem.get_Fields(PropertyIds.PR_MESSAGE_FLAGS);

                if (value)
                    flags &= ~PropertyIds.MessageFlags.UnSent;
                else
                    flags |= PropertyIds.MessageFlags.UnSent;

                SafeItem.set_Fields(PropertyIds.PR_MESSAGE_FLAGS, flags);

            }
        }


        public virtual DateTime SentOn
        {
            get
            {
                return Helpers.UtcToLocal(SafeItem.get_Fields(PropertyIds.PR_CLIENT_SUBMIT_TIME));
            }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_CLIENT_SUBMIT_TIME, Helpers.LocalToUtc(value));
            }
        }

        public virtual string SentOnBehalfOfName
        {
            get
            {
                return (string)SafeItem.get_Fields(PropertyIds.PR_SENT_REPRESENTING_NAME);
            }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_SENT_REPRESENTING_NAME, value);
            }
        }


        public virtual MSOutlook.OlBodyFormat BodyFormat
        {
            get
            {
                return MSOutlook.OlBodyFormat.olFormatPlain;
            }
            set { }
        }

        public virtual string HTMLBody
        {
            get
            {
                return Body;
            }
            set
            {
                Body = value;
            }
        }

        public virtual string To
        {
            get { return (string)SafeItem.get_Fields(PropertyIds.PR_DISPLAY_TO); }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_DISPLAY_TO, value);
            }
        }


        public virtual bool InputAddress
        {

            get
            {
                return inputaddress;
            }


            set
            {
                inputaddress = value;
                if (value == true)
                {

                    To = string.Empty;

                }




            }

        }



        public virtual string BCC
        {
            get { return (string)SafeItem.get_Fields(PropertyIds.PR_DISPLAY_BCC); }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_DISPLAY_BCC, value);

            }
        }

        public virtual string CC
        {
            get { return (string)SafeItem.get_Fields(PropertyIds.PR_DISPLAY_CC); }
            set
            {
                SafeItem.set_Fields(PropertyIds.PR_DISPLAY_CC, value);

            }
        }



        #endregion
        
        #region Methods

        public void Close(MSOutlook.OlInspectorClose SaveMode)
        {
            Invoke<object>("Close", SaveMode);
        }

        public object Copy()
        {
            return GetItem(() => Invoke<object>("Copy"));
        }

        public void Delete()
        {
            Delete(false);
        }

        public void Delete(bool permanent)
        {
            if (permanent)
                InvokeEx<object>(RDOItem, "Delete", Redemption.redDeleteFlags.dfSoftDelete);
            else
                Invoke<object>("Delete", null);
        }


        public void Display()
        {
            Display(false);
        }
        public void Display(object Modal)
        {
            var pinned = IsPinned;

            try
            {
                IsPinned = true;

                Invoke<object>("Display", Modal);

                if (insp == null)
                {
                    Inspector = Application.GetInspector(GetInspector);
                }
            }
            finally
            {
                IsPinned = pinned;
            }
        }

        public object Move(MSOutlook.MAPIFolder DestFldr)
        {
            var of = DestFldr as OutlookFolder;
            if (of != null)
                DestFldr = of.InternalItem;

            return GetItem(() => Invoke<object>("Move", DestFldr));
        }


        public void PrintOut()
        {
            Invoke<object>("PrintOut");
        }

        public void Save()
        {
            ForceSave();

            if (entryid != this.InternalEntryID)
            {
                entryid = this.InternalEntryID;

                if (userprops is RedemptionUserProperties)
                    ((RedemptionUserProperties)userprops).Rebuild();

                Application.LoadedItems.OnNewEntryId(this);
            }

        }

        private void ForceSave()
        {
            try
            {
                if (rdoitem != null)
                {
                    InvokeEx<object>(rdoitem, "Save");
                }

                Invoke<object>("Save");

            }
            catch (COMException comex)
            {

                //Occasionaly gets in a state of deleted.
                if (comex.ErrorCode == HResults.E_OBJECT_DELETED_OR_MOVED)
                    return;

                if (comex.ErrorCode != HResults.E_ITEM_ALREADY_MODIFIED)
                    throw;

                //The following code does not work for storage items.
                if (InternalItem is MSOutlook.StorageItem)
                    return;

                var hasinspector = this.Inspector != null;

                var list = UserProperties.Cast<MSOutlook.UserProperty>()
                    .Select(up => new
                    {
                        Name = up.Name,
                        Type = up.Type,
                        Value = up.Value
                    })
                    .ToArray();

                //items has already been changed by another instance
                Close(MSOutlook.OlInspectorClose.olDiscard);
                Refresh();


                foreach (var p in list)
                {
                    var name = p.Name;
                    var type = p.Type;
                    var val = p.Value;

                    //p.Delete();

                    UserProperties.Add(name, type, false, Type.Missing).Value = val;
                }

                try
                {
                    Invoke<object>("Save");
                }
                catch (COMException comex2)
                {
                    //given up trying, a background service keeps modifying our item
                    if (comex2.ErrorCode == HResults.E_OBJECT_DELETED_OR_MOVED)
                        return;

                    if (comex2.ErrorCode != HResults.E_ITEM_ALREADY_MODIFIED)
                        throw;

                    Trace.WriteLine(string.Format("Outlook message '{0}' with subject '{1}' has been modified by another process after an attempt to resolve the concurrency issue.  Metadata may be lost.", this.EntryID, this.Subject));

                }

                if (hasinspector)
                    this.Display();

            }
        }

        public void SaveAs(string Path)
        {
            SaveAs(Path, MSOutlook.OlSaveAsType.olMSGUnicode);
            
        }
        public void SaveAs(string Path, object Type)
        {
            Invoke<object>("SaveAs", Path, Type);
        }

        public void Send()
        {
            Invoke<object>("Send");
        }

        public void ShowCategoriesDialog()
        {
            Invoke<object>("ShowCategoriesDialog");
        }


        #endregion

        #region Overrides

        protected override void OnAttachEvents()
        {
            base.OnAttachEvents();

            if (obj != null)
            {

                obj.BeforeDelete += obj_BeforeDelete;
                obj.Close += obj_Close;
                obj.CustomAction += obj_CustomAction;
                obj.Forward += obj_Forward;
                obj.Open += obj_Open;
                obj.Reply += obj_Reply;
                obj.ReplyAll += obj_ReplyAll;
            }
        }

        protected override void OnDetachEvents()
        {
            base.OnDetachEvents();

            if (obj != null)
            {

                try { obj.BeforeDelete -= obj_BeforeDelete; }
                catch { }
                try { obj.Close -= obj_Close; }
                catch { }
                try { obj.CustomAction -= obj_CustomAction; }
                catch { }
                try { obj.Forward -= obj_Forward; }
                catch { }
                try { obj.Open -= obj_Open; }
                catch { }
                try { obj.Reply -= obj_Reply; }
                catch { }
                try { obj.ReplyAll -= obj_ReplyAll; }
                catch { }
            }

        }

        public override bool Equals(object obj)
        {
            OutlookItem oi = obj as OutlookItem;
            if (oi == null)
                return false;
            else
                return oi.GetHashCode() == GetHashCode() || object.ReferenceEquals(obj, this);
        }

        public override int GetHashCode()
        {
            string id = this.EntryID;
            if (String.IsNullOrEmpty(id))
                return obj.GetHashCode();
            else
                return id.GetHashCode();
        }


        protected override void Dispose(bool disposing)
        {
            try
            {
                IsPinned = false;

                if (disposing)
                {
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion

        #region ItemEvents_10_Event Members

        #region Passthrough Events

        public event MSOutlook.ItemEvents_10_AttachmentAddEventHandler AttachmentAdd
        {
            add
            {
                obj.AttachmentAdd += value;
            }
            remove
            {
                obj.AttachmentAdd -= value;
            }
        }

        public event MSOutlook.ItemEvents_10_AttachmentReadEventHandler AttachmentRead
        {
            add
            {
                obj.AttachmentRead += value;
            }
            remove
            {
                obj.AttachmentRead -= value;
            }
        }



        public event MSOutlook.ItemEvents_10_BeforeAttachmentSaveEventHandler BeforeAttachmentSave
        {
            add
            {
                obj.BeforeAttachmentSave += value;
            }
            remove
            {
                obj.BeforeAttachmentSave -= value;
            }
        }



        public event MSOutlook.ItemEvents_10_BeforeCheckNamesEventHandler BeforeCheckNames
        {
            add
            {
                obj.BeforeCheckNames += value;
            }
            remove
            {
                obj.BeforeCheckNames -= value;
            }
        }




        public event MSOutlook.ItemEvents_10_CustomPropertyChangeEventHandler CustomPropertyChange
        {
            add
            {
                obj.CustomPropertyChange += value;
            }
            remove
            {
                obj.CustomPropertyChange -= value;
            }
        }

        public event MSOutlook.ItemEvents_10_PropertyChangeEventHandler PropertyChange
        {
            add
            {
                obj.PropertyChange += value;
            }
            remove
            {
                obj.PropertyChange -= value;
            }
        }

        public event MSOutlook.ItemEvents_10_ReadEventHandler Read
        {
            add
            {
                obj.Read += value;
            }
            remove
            {
                obj.Read -= value;
            }
        }

        event MSOutlook.ItemEvents_10_SendEventHandler MSOutlook.ItemEvents_10_Event.Send
        {
            add
            {
                obj.Send += value;
            }
            remove
            {
                obj.Send -= value;
            }
        }



        public event MSOutlook.ItemEvents_10_WriteEventHandler Write
        {
            add
            {
                obj.Write += value;
            }
            remove
            {
                obj.Write -= value;
            }
        }

        #endregion

        #region Overridden Events


        public event MSOutlook.ItemEvents_10_CustomActionEventHandler CustomAction;
        public event MSOutlook.ItemEvents_10_ForwardEventHandler Forward;
        public event MSOutlook.ItemEvents_10_BeforeDeleteEventHandler BeforeDelete;
        public event MSOutlook.ItemEvents_10_ReplyEventHandler Reply;
        public event MSOutlook.ItemEvents_10_ReplyAllEventHandler ReplyAll;
        public event MSOutlook.ItemEvents_10_OpenEventHandler Open;
        private MSOutlook.ItemEvents_10_CloseEventHandler close;

        event MSOutlook.ItemEvents_10_CloseEventHandler MSOutlook.ItemEvents_10_Event.Close
        {
            add
            {
                close += value;
            }
            remove
            {
                close -= value;
            }
        }

        #endregion

        #endregion

        #region Redemption

        internal protected Redemption.IRDOMailEvents_Event RDOItem
        {
            get
            {
                SetupRDOItem();

                return rdoitem;
            }
        }

        internal protected Redemption._ISafeItem SafeItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return safeitem;
            }
        }

        private void SetupRDOItem()
        {
            if (rdoitem == null)
            {
                rdoitem = OutlookApplication.GetApplication(InternalApplication).RDOSession.GetMessageFromID(EntryID, Type.Missing, Type.Missing);
            }
        }

        #endregion

        #region Inspector

        private OutlookInspector insp;

        public OutlookInspector Inspector
        {
            get
            {
                return insp;
            }
            internal set
            {
                if (insp == value)
                    return;

                DetachInspectorEvents();

                insp = value;

                AttachInspectorEvents();

                if (value != null)
                {
                    Attach(insp.InternalItem.CurrentItem);

                    AttachEvents();
                }
                else
                {
                    Detach();
                }
            }
        }

        private void AttachInspectorEvents()
        {
            if (insp != null)
            {
                insp.Closed += insp_Closed;
            }
        }

        private void DetachInspectorEvents()
        {
            if (insp != null)
            {
                insp.Closed -= insp_Closed;
            }
        }

        private void insp_Closed()
        {
            try
            {
                DetachInspectorEvents();

                insp = null;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "Outlook.Item.Inspector.Closed");
                throw;
            }
        }

        #endregion

        #region Captured Events


        private void obj_ReplyAll(object Response, ref bool Cancel)
        {
            try
            {
                var item = Application.GetItem(Response, true);

                try
                {
                    var e = new BeforeReplyItemEventArgs(this, false, item);
                    e.Cancel = Cancel;
                    Application.OnBeforeReplyItem(e);
                    if (e.Handled)
                    {
                        Cancel = e.Cancel;
                        return;
                    }

                    var ev = ReplyAll;
                    if (ev != null)
                        ev(item, ref Cancel);
                }
                finally
                {
                    item.IsPinned = false;
                }
            }
            catch (Exception ex)
            {
                Cancel = true;
                // MNw - If you delete a rule logged in or after these handlers are connected this will fail with an error :(            
                MessageBox.Show(Inspector, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void obj_Reply(object Response, ref bool Cancel)
        {
            try
            {
                var item = Application.GetItem(Response, true);

                try
                {
                    var e = new BeforeReplyItemEventArgs(this, false, item);
                    e.Cancel = Cancel;
                    Application.OnBeforeReplyItem(e);
                    if (e.Handled)
                    {
                        Cancel = e.Cancel;
                        return;
                    }

                    var ev = Reply;
                    if (ev != null)
                        ev(item, ref Cancel);
                }
                finally
                {
                    item.IsPinned = false;
                }
            }
            catch (Exception ex)
            {
                Cancel = true;
                // MNw - If you delete a rule logged in or after these handlers are connected this will fail with an error :(            
                MessageBox.Show(Inspector, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void obj_Forward(object forward, ref bool Cancel)
        {
            try
            {
                var item = Application.GetItem(forward, true);

                try
                {
                    var e = new BeforeForwardItemEventArgs(this, false, item);
                    e.Cancel = Cancel;
                    Application.OnBeforeForwardItem(e);
                    if (e.Handled)
                    {
                        Cancel = e.Cancel;
                        return;
                    }

                    var ev = Forward;
                    if (ev != null)
                        ev(item, ref Cancel);
                }
                finally
                {
                    item.IsPinned = false;
                }
            }
            catch (Exception ex)
            {
                Cancel = true;
                // MNw - If you delete a rule logged in or after these handlers are connected this will fail with an error :(            
                MessageBox.Show(Inspector, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void obj_CustomAction(object Action, object Response, ref bool Cancel)
        {
            var ev = CustomAction;
            if (ev != null)
                ev(Action, Application.GetItem(Response), ref Cancel);
        }


        internal bool DeleteRequested { get; set; }


        private void obj_BeforeDelete(object Item, ref bool Cancel)
        {
            try
            {
                if (DeleteRequested)
                {
                    DeleteRequested = false;
                    return;
                }


                var item = this;

                var e = new BeforeItemEventArgs(item, false);
                e.Cancel = Cancel;
                Application.OnBeforeDeleteItem(e);
                if (e.Handled)
                {
                    Cancel = e.Cancel;
                    return;
                }

                var ev = BeforeDelete;
                if (ev != null)
                    ev(item, ref Cancel);

            }
            catch (Exception ex)
            {
                Cancel = true;
                // MNw - If you delete a rule logged in or after these handlers are connected this will fail with an error :(            
                MessageBox.Show(Inspector, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void obj_Open(ref bool Cancel)
        {
            try
            {
                var e = new BeforeItemEventArgs(this, false);
                e.Cancel = Cancel;
                Application.OnBeforeOpenItem(e);
                if (e.Handled)
                {
                    Cancel = e.Cancel;
                    return;
                }

                var ev = Open;
                if (ev != null)
                    ev(ref Cancel);

            }
            catch (Exception ex)
            {
                Cancel = true;
                // MNw - If you delete a rule logged in or after these handlers are connected this will fail with an error :(            
                MessageBox.Show(Inspector, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void obj_Close(ref bool Cancel)
        {
            try
            {
                if (IsDeleted)
                    return;

                var e = new BeforeItemEventArgs(this, false);
                e.Cancel = Cancel;
                Application.OnBeforeCloseItem(e);
                if (e.Handled)
                {
                    Cancel = e.Cancel;
                    return;
                }

                var ev = close;
                if (ev != null)
                    ev(ref Cancel);

            }
            catch (COMException comex)
            {
                //If already failed to save due to being deleted from the explorer view.
                if (comex.ErrorCode == HResults.E_OPERATION_FAILED)
                    return;
                throw;
            }
            catch (Exception ex)
            {
                if (IsDeleted)
                    return;

                Cancel = true;
                // MNw - If you delete a rule logged in or after these handlers are connected this will fail with an error :(            
                MessageBox.Show(Inspector, ex.Message, Application.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #endregion

        #region Detachable


        protected static internal COMDisposable<MSOutlook.ItemEvents_10_Event, Redemption._ISafeItem> CreateCOMWrapper(MSOutlook.ItemEvents_10_Event item)
        {
            var safeitem = OutlookItemFactory.CreateSafeItem(item);
            return new COMDisposable<MSOutlook.ItemEvents_10_Event, _ISafeItem>(item, safeitem, false);
        }

        protected internal COMDisposable<MSOutlook.ItemEvents_10_Event, Redemption._ISafeItem> CreateCOMWrapper()
        {
            if (IsDetached)
            {
                var item = GetItemByIDs(EntryID, StoreID);
                var safeitem = OutlookItemFactory.CreateSafeItem(obj);
                return new COMDisposable<MSOutlook.ItemEvents_10_Event, _ISafeItem>(item, safeitem, false);
            }
            else
            {
                return new COMDisposable<MSOutlook.ItemEvents_10_Event, _ISafeItem>(this.obj, this.safeitem, true);
            }
        }

        private MSOutlook.ItemEvents_10_Event GetItemByIDs(string thisEntryID, string thisStoreID)
        {
            MSOutlook.ItemEvents_10_Event item;
            try
            {
                item = (MSOutlook.ItemEvents_10_Event)Application.Session.InternalItem.GetItemFromID(thisEntryID, thisStoreID);
            }
            catch
            {
                try
                {
                    item = (MSOutlook.ItemEvents_10_Event)Application.Session.InternalItem.GetItemFromID(thisEntryID);
                }
                catch
                {
                    throw;
                }
            }
            return item;
        }

        public void Refresh()
        {
            Detach();
            Attach();
        }

        protected override bool CanDetach
        {
            get
            {
                if (insp == null)
                    return true;
                else
                    return false;
            }
        }

        public void Attach()
        {
            if (IsDetached)
            {
                try
                {
                    var item = GetItemByIDs(EntryID, StoreID);
                    Attach(item);
                }
                catch (COMException)
                {
                    throw;
                }
            }
        }

        protected override void OnAttach(object obj)
        {
            base.OnAttach(obj);

            this.obj = (MSOutlook.ItemEvents_10_Event)obj;
            this.safeitem = OutlookItemFactory.CreateSafeItem(obj);

            if (Marshal.IsComObject(obj))
                Marshal.SetComObjectData(obj, "TempID", temporaryid);
        }

        protected override void OnDetach()
        {

            if (rdoitem != null)
            {
                Marshal.FinalReleaseComObject(rdoitem);
                rdoitem = null;
            }

            if (safeitem != null)
            {
                Marshal.ReleaseComObject(safeitem);
                safeitem = null;
            }

            if (obj != null)
            {
                if (Marshal.IsComObject(obj))
                    MarshalDelComObjectData(obj, "TempID");

                obj = null;
            }

            insp = null;

            if (userprops != null)
            {
                var disp = userprops as IDisposable;
                if (disp != null)
                    disp.Dispose();

                if (Marshal.IsComObject(userprops))
                {
                    Marshal.FinalReleaseComObject(userprops);
                }

                userprops = null;
            }

            base.OnDetach();

        }

        private static void MarshalDelComObjectData(object obj, object key)
        {
            var objectToDataMap = obj.GetType().GetField("m_ObjectToDataMap", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)?.GetValue(obj) as System.Collections.Hashtable;
            if (objectToDataMap != null)
                objectToDataMap.Remove(key);
        }

        #endregion

        public string FolderID
        {
            get
            {
                return Get(ref folderid, GetFolderID);

            }
        }

        public string StoreID
        {
            get
            {
                return Get(ref storeid, GetStoreID);
            }
        }

        public bool CheckSpellingOnSend
        {
            get
            {
                if (!RequiresSpellCheck.HasValue)
                    return Application.Settings.CheckSpellingOnSend;

                return RequiresSpellCheck.Value;
            }
        }

        public bool? RequiresSpellCheck { get; set; }

        private bool ispinned;
        internal bool IsPinned
        {
            get
            {
                return ispinned;
            }
            set
            {
                ispinned = value;
            }
        }

        public virtual string ExtractTextPreview()
        {
            try
            {
                return Body;
            }
            catch (COMException)
            {
                return string.Empty;
            }
        }

        public virtual void AddBodyText(string text, bool toStart)
        {
            if (String.IsNullOrEmpty(text))
                return;

            var body = GetBodyText();

            if (toStart)
                body = text + body;
            else
                body += text;

            Body = body;
        }

        public virtual void SetBodyText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return;


            Body = text;
        }

        public virtual string GetBodyText()
        {
            try
            {
                return Body;
            }
            catch (COMException)
            {
                return String.Empty;
            }
        }


        public void Synchronise()
        {
            if (Application.IsAddinInstance)
                return;

            var exp = Application.ActiveExplorer() as OutlookExplorer;
            if (exp == null || exp.Window == null)
                throw new InvalidOperationException(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("EXPLREQTOSCHR", "An active explorer is required to synchronise the item.", "").Text);


            COPYDATASTRUCT cps = new COPYDATASTRUCT();
            // Initialize unmanged memory to hold the struct.
            cps.lpData = this.EntryID + "+" + this.StoreID;
            cps.cbData = cps.lpData.Length + 1;

            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(COPYDATASTRUCT)));

            try
            {
                Marshal.StructureToPtr(cps, ptr, false);

                exp.Window.SendMessage(WinFinder.WindowMessage.CopyData, new IntPtr(OutlookApplication.WM_REFRESH_PROPS), ptr);

            }
            finally
            {
                // Free the unmanaged memory.
                Marshal.FreeHGlobal(ptr);
            }
        }

        internal void SynchroniseProperties()
        {

            try
            {
                var rup = this.UserProperties as RedemptionUserProperties;
                if (rup != null)
                    rup.Rebuild();
            }
            catch (Exception)
            {
            }
        }


        #region Getters & Setters

        private T Get<T>(ref T stored, Func<MSOutlook.ItemEvents_10_Event, _ISafeItem, T> getter)
        {
            try
            {
                if (IsDetached)
                    return stored;
                else
                {
                    stored = getter(obj, safeitem);
                    return stored;
                }
            }
            catch (COMException comex)
            {
                if (comex.ErrorCode == HResults.E_OBJECT_DELETED_OR_MOVED)
                {
                    return stored;
                }

                throw;
            }
        }

        private void Set<T>(ref T stored, Action<MSOutlook.ItemEvents_10_Event, _ISafeItem, T> setter, T value)
        {
            CheckIfDetached();

            if (IsDetached)
                stored = value;
            else
                setter(obj, safeitem, value);
        }

        private bool IsNote(MSOutlook.ItemEvents_10_Event item)
        {
            return IsNote(item, null);
        }

        private bool IsNote(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem)
        {
            MSOutlook.OlObjectClass itemClass = GetClass(item, safeItem);
            return itemClass == MSOutlook.OlObjectClass.olNote;
        }

        protected virtual MSOutlook.Application GetApplication(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem)
        {
            return GetPropertyEx<MSOutlook.Application>(item, "Application");
        }

        protected virtual object GetParent(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem)
        {
            return GetPropertyEx<object>(item, "Parent");
        }

        protected virtual string GetEntryID(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem)
        {
            return GetPropertyEx<string>(item, "EntryID");
        }

        protected virtual MSOutlook.OlObjectClass GetClass(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem)
        {
            return GetPropertyEx<MSOutlook.OlObjectClass>(item, "Class");
        }

        protected virtual MSOutlook.OlObjectClass GetClass(MSOutlook.ItemEvents_10_Event item)
        {
            return GetClass(item, null);
        }

        protected virtual string GetStoreID(MSOutlook.ItemEvents_10_Event item, Redemption._ISafeItem safeitem)
        {
            if (IsNote(item))
                return null;

            var ret = (object[])safeitem.get_Fields(PropertyIds.PR_STORE_ENTRYID);
            if (ret == null)
                return null;
            var data = ret.Cast<byte>();
            var buff = new System.Text.StringBuilder();
            foreach (byte b in data)
            {
                buff.Append(String.Format("{0:X2}", b));
            }

            return buff.ToString();
        }

        protected virtual string GetFolderID(MSOutlook.ItemEvents_10_Event item, Redemption._ISafeItem safeItem)
        {
            if (IsNote(item, safeItem))
                return null;

            object[] ret = GetPropertyEx<object[]>(safeItem, "Fields", PropertyIds.PR_PARENT_ENTRYID);
            if (ret == null)
                return null;
            var data = ret.Cast<byte>();
            System.Text.StringBuilder buff = new System.Text.StringBuilder();
            foreach (byte b in data)
            {
                buff.Append(String.Format("{0:X2}", b));
            }

            return buff.ToString();
        }        

        protected virtual string GetSubject(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem)
        {
            return GetPropertyEx<string>(item, "Subject");
        }

        protected virtual void SetSubject(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem, string value)
        {
            SetPropertyEx(item, "Subject", value);
        }

        protected virtual string GetMessageClass(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem)
        {
            return GetPropertyEx<string>(item, "MessageClass");
        }

        protected virtual void SetMessageClass(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem, string value)
        {
            SetPropertyEx(item, "MessageClass", value);
        }

        protected virtual string GetSenderName(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem)
        {
            if (IsNote(item, safeItem))
                return null;

            return (string)safeItem.get_Fields(PropertyIds.PR_SENDER_NAME);
        }

        protected virtual void SetSenderName(MSOutlook.ItemEvents_10_Event item, _ISafeItem safeItem, string value)
        {
            safeItem.set_Fields(PropertyIds.PR_SENDER_NAME, value);
        }

        #endregion

        private class EmptyAttachments : MSOutlook.Attachments
        {
            #region Attachments Members

            public Microsoft.Office.Interop.Outlook.Attachment Add(object Source, object Type, object Position, object DisplayName)
            {
                throw new NotSupportedException();
            }

            public Microsoft.Office.Interop.Outlook.Application Application
            {
                get { throw new NotImplementedException(); }
            }

            public Microsoft.Office.Interop.Outlook.OlObjectClass Class
            {
                get { return Microsoft.Office.Interop.Outlook.OlObjectClass.olAttachments; }
            }

            public int Count
            {
                get { return 0; }
            }

            public object Parent
            {
                get { return null; }
            }

            public void Remove(int Index)
            {
                throw new NotSupportedException();
            }

            public Microsoft.Office.Interop.Outlook.NameSpace Session
            {
                get { throw new NotImplementedException(); }
            }

            public Microsoft.Office.Interop.Outlook.Attachment this[object Index]
            {
                get { throw new NotSupportedException(); }
            }

            #endregion

            #region IEnumerable Members

            public System.Collections.IEnumerator GetEnumerator()
            {
                yield break;
            }

            #endregion
        }
    }

}
