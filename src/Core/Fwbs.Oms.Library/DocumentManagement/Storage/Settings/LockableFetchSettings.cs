namespace FWBS.OMS.DocumentManagement.Storage
{
    [StorageSettingsEditor("FWBS.OMS.UI.Windows.DocumentManagement.Storage.LockableFetchSettingsEditor, OMS.UI", SettingsType.Fetch)]
    public class LockableFetchSettings : StorageSettings
    {
        public override string Name
        {
            get
            {
                return Session.CurrentSession.Resources.GetResource("CHECKOUT", "Check Out", "").Text;
            }
        }

        private const string CHECKOUT = "CHECKOUT";
        public bool CheckOut
        {
            get
            {
                if (Contains(CHECKOUT))
                    return Common.ConvertDef.ToBoolean(this[CHECKOUT], false);
                else
                    return false;
            }
            set
            {
                this[CHECKOUT] = value;
            }
        }


        public override bool CanEdit
        {
            get { return true; }
        }
    }
}
