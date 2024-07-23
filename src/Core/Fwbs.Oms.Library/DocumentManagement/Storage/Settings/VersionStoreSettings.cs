using System;

namespace FWBS.OMS.DocumentManagement.Storage
{

    [StorageSettingsEditor("FWBS.OMS.UI.Windows.DocumentManagement.Storage.VersionStoreSettingsEditor, OMS.UI", SettingsType.Store)]
    public class VersionStoreSettings : StorageSettings
    {
        
        public enum StoreAs
        {
            OriginalOverwrite,
            NewMajorVersion,
            NewVersion,
            NewSubVersion
        }

        public override string Name
        {
            get
            {
                return Session.CurrentSession.Resources.GetResource("VERSIONING", "Versioning", "").Text;
            }
        }

        private const string STORE_AS = "STORE_AS";
        public StoreAs SaveItemAs
        {
            get
            {
                if (Contains(STORE_AS))
                    return (StoreAs)Common.ConvertDef.ToEnum(this[STORE_AS], StoreAs.OriginalOverwrite);
                else
                    return StoreAs.OriginalOverwrite;
            }
            set
            {
                if (value == StoreAs.OriginalOverwrite && CanOverwrite == false)
                    throw new StorageException("EXDOCOVERWRITE", "Unable to overwrite the existing document.");
                
                this[STORE_AS] = value;
            }
        }

        private const string LATEST = "LATEST";
        public bool MarkAsLatest
        {
            get
            {
                if (Contains(LATEST))
                    return Common.ConvertDef.ToBoolean(this[LATEST], true);
                else
                    return true;
            }
            set
            {
                this[LATEST] = value;
            }
        }

        private const string COMMENTS = "COMMENTS";
        public string Comments
        {
            get
            {
                if (Contains(COMMENTS))
                    return Convert.ToString(this[COMMENTS]);
                else
                    return String.Empty;
            }
            set
            {
                if (value == null)
                    this.RemoveSetting(COMMENTS);
                else
                    this[COMMENTS] = value;
            }
        }

        private const string STATUS = "STATUS";
        public string Status
        {
            get
            {
                if (Contains(STATUS))
                    return Convert.ToString(this[STATUS]);
                else
                    return String.Empty;
            }
            set
            {
                if (value == null)
                    RemoveSetting(STATUS);
                else
                {
                    if (value != this.Status)
                    {
                        if (SaveItemAs == StoreAs.OriginalOverwrite && CanChangeStatus == false)
                            throw new StorageException("EXDOCSTATCHANGE", "Unable to change the status of the document when set to overwrite.");
                        else
                            this[STATUS] = value;
                    }
                }
            }
        }



        public bool CanChangeStatus
        {
            get
            {
                if (CanOverwrite)
                {
                    if (Array.IndexOf(DocStatus.Protected, Status) > -1)
                        return Session.CurrentSession.CurrentUser.IsInRoles(new string[] { "CHANGEDOCSTATUS" });
                    else
                    {
                        return true;
                    }
                }
                else
                    return false;
            }
        }

        public bool CanOverwrite
        {
            get
            {
                if ((Array.IndexOf(DocStatus.Protected, Status) > -1) || InternalCanOverwrite == false)
                    return Session.CurrentSession.CurrentUser.IsInRoles(new string[] { User.ROLE_ADMIN, User.ROLE_PARTNER, "DOCOVERWRITER" });
                else
                {
                    return InternalCanOverwrite;
                }
            }
            internal set
            {
                this[CANOVERWRITE] = value;
            }
        }

        private const string CANOVERWRITE = "CANOVERWRITE";
        private bool InternalCanOverwrite
        {
            get
            {
                if (Contains(CANOVERWRITE))
                    return Common.ConvertDef.ToBoolean(this[CANOVERWRITE], false);
                else
                    return false;
            }
        }


        public override bool CanEdit
        {
            get { return true; }
        }
        
    }
}
