using System;
using System.Collections.Generic;

namespace FWBS.Common
{
    public static class SpecialFolders
    {
        private static readonly Dictionary<Environment.SpecialFolder, System.IO.DirectoryInfo> folders = new Dictionary<Environment.SpecialFolder,System.IO.DirectoryInfo>();

        public static void SetFolderPath(Environment.SpecialFolder folder, string path)
        {
            lock(folders)
            {
                if (String.IsNullOrEmpty(path))
                {
                    if (folders.ContainsKey(folder))
                        folders.Remove(folder);
                }
                else
                {
                    if (folders.ContainsKey(folder))
                        folders[folder] = new System.IO.DirectoryInfo(path);
                    else
                        folders.Add(folder, new System.IO.DirectoryInfo(path));
                }
            }
        }

        public static string GetFolderPath(Environment.SpecialFolder folder)
        {
            lock (folders)
            {
                if (folders.ContainsKey(folder))
                    return folders[folder].FullName;
                else
                    return Environment.GetFolderPath(folder);
            }
        }

        
    }
}
