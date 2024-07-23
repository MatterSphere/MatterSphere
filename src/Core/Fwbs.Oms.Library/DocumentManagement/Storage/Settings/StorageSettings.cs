using System;
using System.Collections.Generic;

namespace FWBS.OMS.DocumentManagement.Storage
{
    [Serializable]
    public abstract class StorageSettings
    {
        private Dictionary<string, object> settings = new Dictionary<string, object>();

        public virtual string Name
        {
            get
            {
                return GetType().Name;
            }
        }

        public abstract bool CanEdit{get;}

        protected object this[string setting]
        {
            get
            {
                return settings[setting];
            }
            set
            {
                if (CanEdit)
                    settings[setting] = value;
                else
                    throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("UNBLRDNLSTNG", "Unable to change a read only setting.", "").Text);
            }
        }

        protected void RemoveSetting(string setting)
        {
            if (!CanEdit)
                throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("UNBLRDNLSTNG", "Unable to change a read only setting.", "").Text);

            if (settings.ContainsKey(setting))
                settings.Remove(setting);

        }

        protected Dictionary<string, object> InternalSettings
        {
            get
            {
                return settings;
            }
        }

        protected bool Contains(string setting)
        {
            return settings.ContainsKey(setting);
        }

        public virtual SettingsType SettingsType 
        {
            get
            {
                  object[] attrs = GetType().GetCustomAttributes(typeof(StorageSettingsEditorAttribute), false);
                  if (attrs.Length > 0)
                  {
                      return ((StorageSettingsEditorAttribute)attrs[0]).SettingsType;
                  }

                  return SettingsType.Store;
            }
        }

        private bool settingsScreenVisible = true;
        public bool SettingsScreenVisible
        {
            get { return settingsScreenVisible;}
            set { settingsScreenVisible = value;}
        }

        public virtual void Merge(StorageSettings parentSettings)
        {
            foreach (KeyValuePair<string, object> setting in parentSettings.InternalSettings)
            {
                if (settings.ContainsKey(setting.Key))
                    settings[setting.Key] = setting.Value;
                else
                    settings.Add(setting.Key, setting.Value);
            }
        }

        public virtual StorageSettings Clone()
        {
            
             if (!this.GetType().IsSerializable)
                 return null;

             using (System.IO.MemoryStream str = new System.IO.MemoryStream())
             {

                 System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                 formatter.Serialize(str, this);

                 //set the stream back to the start.
                 if (str.Length > 0)
                     str.Position = 0;

                 StorageSettings clonedSetings = formatter.Deserialize(str) as StorageSettings;
                 str.Close();

                 return clonedSetings;
             }

        }

        public virtual void ValidateSettings(bool isNew)
        {
        }

    }
}
