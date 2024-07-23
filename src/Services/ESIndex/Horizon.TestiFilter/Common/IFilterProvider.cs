using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace Horizon.TestiFilter.Common
{
    public class IFilterProvider
    {
        private delegate bool SearchKey(string name);

        // Finding a Filter Handler for a Given File Extension 
        // https://docs.microsoft.com/en-us/windows/win32/search/-search-ifilter-registering-filters#finding-a-filter-handler-for-a-given-file-extension

        public List<Filter> GetFilters()
        {
            var extensions = GetExtensions();
            foreach (var extension in extensions)
            {
                extension.SetData();
            }

            extensions = extensions.Where(extension => extension.Link != null).ToList();

            return GetFilters(extensions);
        }

        public Filter GetFilter(string extensionName)
        {
            Extension extension = null;
            string keyDefaultValue = null;
            string inprocServer32DefaultValue = null;

            using (var rootKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitProcess ? RegistryView.Registry64 : RegistryView.Registry32))
            {
                var extensionKey = rootKey.GetSubKeyNames()
                .Where(name => name.ToLower() == extensionName)
                .Select(Registry.ClassesRoot.OpenSubKey).FirstOrDefault();
                if (extensionKey == null)
                {
                    return null;
                }

                var value = GetDefaultValue(extensionKey, "PersistentHandler");
                if (value == null)
                {
                    return null;
                }

                extension = new Extension(GetName(extensionKey), value);
                extension.SetData();
                if (extension.Link == null)
                {
                    return null;
                }

                var clsid = rootKey.OpenSubKey(@"CLSID");
                if (clsid == null)
                {
                    return null;
                }

                var key = GetRegistryKeys(clsid, it => extension.Link.ToLower() == it.ToLower()).FirstOrDefault();
                if (key == null)
                {
                    return null;
                }

                var inprocServer32 = key.OpenSubKey(@"InprocServer32");
                if (inprocServer32 == null)
                {
                    clsid = rootKey.OpenSubKey(@"WOW6432Node\CLSID");
                    if (clsid == null)
                    {
                        return null;
                    }

                    key = GetRegistryKeys(clsid, it => extension.Link.ToLower() == it.ToLower()).FirstOrDefault();
                    if (key == null)
                    {
                        return null;
                    }

                    keyDefaultValue = key.GetValue(null) as string;

                    inprocServer32 = key.OpenSubKey(@"InprocServer32");
                    if (inprocServer32 == null)
                    {
                        return null;
                    }
                }

                inprocServer32DefaultValue = inprocServer32.GetValue(null) as string;
            }

            return new Filter(
                extension: extension.Value,
                details: extension.Details,
                description: keyDefaultValue,
                path: inprocServer32DefaultValue);
        }

        private List<Extension> GetExtensions()
        {
            var extensions = new List<Extension>();
            using (var rootKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitProcess ? RegistryView.Registry64 : RegistryView.Registry32))
            {
                var extensionKeys = GetRegistryKeys(rootKey, key => key.Substring(0, 1) == ".");
                foreach (var extensionKey in extensionKeys)
                {
                    var value = GetDefaultValue(extensionKey, "PersistentHandler");
                    if (value != null)
                    {
                        extensions.Add(new Extension(GetName(extensionKey), value));
                    }
                }
            };

            return extensions.ToList();
        }

        private List<RegistryKey> GetRegistryKeys(RegistryKey root, SearchKey searchKey)
        {
            return root.GetSubKeyNames().Where(name => searchKey(name))
                .Select(root.OpenSubKey).Where(it => it != null).ToList();
        }

        private string GetDefaultValue(RegistryKey key, string subKeyName)
        {
            var subKey = key.OpenSubKey(subKeyName);

            return subKey?.GetValue(null) as string;
        }

        private List<Filter> GetFilters(List<Extension> extensions)
        {
            var filters = new List<Filter>();
            foreach (var extension in extensions)
            {
                Filter filter = GetFilter(extension);

                if (filter != null)
                {
                    filters.Add(filter);
                }
            }

            return filters;
        }

        private Filter GetFilter(Extension extension)
        {
            Filter filter = null;

            using (var root = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, Environment.Is64BitProcess ? RegistryView.Registry64 : RegistryView.Registry32))
            {
                using (var clsidKey = root.OpenSubKey(@"CLSID", false))
                {
                    var key = GetRegistryKeys(clsidKey, subKey => extension.Link.ToLower() == subKey.ToLower()).FirstOrDefault();
                    if (key != null)
                    {
                        filter = GetFilter(key, extension);
                    }
                };
            };

            return filter;
        }

        private Filter GetFilter(RegistryKey key, Extension extension)
        {
            var inprocServer32 = key.OpenSubKey(@"InprocServer32");
            if (inprocServer32 == null)
            {
                return null;
            }

            return new Filter(
                extension: extension.Value,
                details: extension.Details ?? key.GetValue(null) as string,
                description: key.GetValueNames().Any(it => it == string.Empty) ? key.GetValue(null) as string : null,
                path: inprocServer32.GetValueNames().Any(it => it == string.Empty) ? inprocServer32.GetValue(null) as string : null);
        }

        private string GetName(RegistryKey key)
        {
            return key.Name.Split('\\').Last();
        }
    }
}
