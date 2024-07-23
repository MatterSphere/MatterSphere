using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Models.Common;
using Models.Interfaces;

namespace FileStorageProvider
{
    public class Provider : ICacheProvider
    {
        private readonly string _path;
        private const string _failedFolder = "Failed";

        public Provider(string path, string conn)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    @"Thomson Reuters ELITE\Index Tool\Cache\");
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

        public void SaveMessage(Message message)
        {
            CheckDirectory();
            File.WriteAllBytes(GetFilePath(message.ID), message.Data);
        }

        public void FailMessage(Message message)
        {
            CheckDirectory(true);
            File.Move(GetFilePath(message.ID), GetFilePath(message.ID, true));
        }

        public IEnumerable<Message> ReadMessages()
        {
            CheckDirectory();

            var result = new List<Message>();
            var directoryInfo = new DirectoryInfo(_path);
            foreach (var fileInfo in directoryInfo.EnumerateFiles("*.xml").OrderBy(f => f.LastWriteTimeUtc))
            {
                result.Add(ReadData(fileInfo.FullName));
            }

            return result;
        }

        private void CheckDirectory(bool failed = false)
        {
            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            if (failed && !Directory.Exists(Path.Combine(_path, _failedFolder)))
            {
                Directory.CreateDirectory(Path.Combine(_path, _failedFolder));
            }
        }

        public Message ReadData(string fullName)
        {
            CheckDirectory();

            var message = new Message
            (
                File.ReadAllBytes(fullName),
                Path.GetFileNameWithoutExtension(fullName)
            );

            return message;
        }

        public void ClearCache(Message message)
        {
            File.Delete(GetFilePath(message.ID));
        }

        private string GetFilePath(string messageId, bool failed = false)
        {
            return failed 
                ? Path.Combine(_path, _failedFolder, $"{messageId}.xml")
                : Path.Combine(_path, $"{messageId}.xml");
        }
    }
}
