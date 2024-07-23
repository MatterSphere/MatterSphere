using System;

namespace FWBS.OMS.DocumentManagement.Storage
{
    [StorageSettingsEditor("FWBS.OMS.UI.Windows.DocumentManagement.Storage.VersionFetchSettingsEditor, OMS.UI", SettingsType.Fetch)]
    public class VersionFetchSettings : StorageSettings
    {
        public enum FetchAs
        {
            Latest,
            Current,
            Specific
        }

        public override string Name
        {
            get
            {
                return Session.CurrentSession.Resources.GetResource("VERSIONING", "Versioning", "").Text;
            }
        }

        private const string FETCH_AS = "FETCH_AS";
        public FetchAs Version
        {
            get
            {
                if (Contains(FETCH_AS))
                    return (FetchAs)Common.ConvertDef.ToEnum(this[FETCH_AS], FetchAs.Current);
                else
                    return FetchAs.Current;
            }
            set
            {
                this[FETCH_AS] = value;
            }
        }

        private const string LABEL = "LABEL";
        public string VersionLabel
        {
            get
            {
                if (Contains(LABEL))
                {
                    string val = Convert.ToString(this[LABEL]);
                    if (String.IsNullOrEmpty(val))
                        val = "1";
                    return val;
                }
                else
                    return "1";
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    RemoveSetting(LABEL);
                else
                    this[LABEL] = value;
            }
        }

       
        public override bool CanEdit
        {
            get { return true; }
        }
    }
}
