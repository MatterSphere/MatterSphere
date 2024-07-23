using System;
using System.Linq;
using Microsoft.Win32;

namespace Horizon.TestiFilter.Common
{
    public class Extension
    {
        public Extension(string value, string key)
        {
            Value = value;
            Key = key;
        }

        public string Value { get; set; }
        public string Key { get; set; }
        public string Details { get; set; }
        public string Link { get; set; }
        
        public void SetData()
        {
            using (var root = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitProcess ? RegistryView.Registry64 : RegistryView.Registry32))
            {
                using (var clsidKey = root.OpenSubKey(@"CLSID", false))
                {
                    var detail = clsidKey.OpenSubKey(Key);
                    if (detail != null)
                    {
                        Details = detail.GetValue(null) as string;
                        Link = GetLink(detail);
                    }
                };
            };
        }

        private string GetLink(RegistryKey registryKey)
        {
            var persistentAddinsRegistered = registryKey.OpenSubKey("PersistentAddinsRegistered");
            if (persistentAddinsRegistered == null)
            {
                return null;
            }

            var subKeyNames = persistentAddinsRegistered.GetSubKeyNames();
            if (!subKeyNames.Any())
            {
                return null;
            }

            var subKey = persistentAddinsRegistered.OpenSubKey(subKeyNames.First());
            return subKey != null
                ? subKey.GetValue(null).ToString()
                : null;
        }
    }
}
