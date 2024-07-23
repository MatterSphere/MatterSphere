namespace FWBS.OMS.DocumentManagement.Storage
{

    [StorageSettingsEditor("FWBS.OMS.UI.Windows.DocumentManagement.Storage.LockableStoreSettingsEditor, OMS.UI", SettingsType.Store)]
    public class LockableStoreSettings : StorageSettings
    {

        public override string Name
        {
            get
            {
                return Session.CurrentSession.Resources.GetResource("CHECKINGIN", "Checking In", "").Text;
            }
        }

        private const string CHECKIN = "CHECKIN";
        public bool CheckIn
        {
            get
            {
                if (Contains(CHECKIN))
                    return Common.ConvertDef.ToBoolean(this[CHECKIN], true);
                else
                    return true;
            }
            set
            {
                this[CHECKIN] = value;
            }
        }

        public override bool CanEdit
        {
            get { return true; }
        }

        
    }
}
