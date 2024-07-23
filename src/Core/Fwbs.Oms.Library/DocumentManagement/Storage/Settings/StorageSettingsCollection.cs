using System;
using System.Collections.Generic;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public class StorageSettingsCollection : List<StorageSettings>
    {
        public TSettings GetSettings<TSettings>()
           where TSettings : StorageSettings
        {
            foreach (StorageSettings s in this)
            {
                if (s.GetType() == typeof(TSettings))
                    return (TSettings)s;
            }
            TSettings set = Activator.CreateInstance<TSettings>();
            Add(set);
            return set;
        }


        public void Merge(StorageSettingsCollection parentCollection)
        {

            if (parentCollection == null)
                return;

            foreach (StorageSettings s in parentCollection)
            {
                bool found = false;

                foreach (StorageSettings current in this)
                {
                    if (current.GetType() == s.GetType())
                    {
                        int pos = this.IndexOf(current);
                        this[pos].Merge(s);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    StorageSettings newSet = s.Clone();
                    if (newSet != null)
                        Add(newSet);
                }

            }

        }

        public StorageSettingsCollection Clone()
        {

            using (System.IO.MemoryStream memStream = new System.IO.MemoryStream())
            {
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(memStream, this);

                //set the stream back to the start.
                if (memStream.Length > 0)
                    memStream.Position = 0;

                StorageSettingsCollection clonedSettings = (StorageSettingsCollection)formatter.Deserialize(memStream);
                memStream.Close();

                return clonedSettings;
            }

        }

        public void ValidateSettings(bool isNew)
        {
            foreach (StorageSettings current in this)
            {
                current.ValidateSettings(isNew);
            }
        }

    }
}
