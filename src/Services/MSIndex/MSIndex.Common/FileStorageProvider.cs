using System;
using System.IO;
using MSIndex.Common.Interfaces;

namespace MSIndex.Common
{
    public class FileStorageProvider : IFileStorageProvider
    {
        private readonly string _path;

        public FileStorageProvider(string path, string conn)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    @"3E MatterSphere\MS Index Tool\Cache\");
            }
            try
            {
                var builder = new System.Data.SqlClient.SqlConnectionStringBuilder(conn);
                string database = string.Format("{0}@{1}", builder.InitialCatalog, builder.DataSource == "." ? "(local)" : builder.DataSource).ToLowerInvariant();
                _path = Path.Combine(path, database.GetHashCode().ToString("X8"));
            }
            catch
            {
                _path = path;
            }
        }

        #region IFileStorageProvider

        public void SaveData(byte[] message)
        {
            CheckDirectory();
            File.WriteAllBytes(GetFilePath(), message);
        }

        public byte[] ReadData()
        {
            CheckDirectory();

            byte[] buffer = null;
            if (File.Exists(GetFilePath()))
                buffer = File.ReadAllBytes(GetFilePath());

            return buffer;
        }

        public void ClearCache()
        {
            CheckDirectory();

            if (File.Exists(GetFilePath()))
                File.Delete(GetFilePath());
        }

        #endregion

        private void CheckDirectory()
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
        }

        private string GetFilePath()
        {
            return Path.Combine(_path, "Message.xml");
        }
    }
}
