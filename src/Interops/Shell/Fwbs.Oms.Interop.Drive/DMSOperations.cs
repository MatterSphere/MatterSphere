using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using DokanNet;

namespace FWBS.OMS.Drive
{
    using Data;
    using FileAccess = DokanNet.FileAccess;
    using TriState = Common.TriState;

    public sealed class DMSOperations : Interfaces.IVirtualDrive, IDokanOperations
    {
        #region DMSOperations members

        private const NtStatus StatusMediaWriteProtected = (NtStatus)0xC00000A2;
        private const FileAccess DataAccess = FileAccess.ReadData | FileAccess.WriteData | FileAccess.AppendData | FileAccess.Execute | FileAccess.GenericExecute | FileAccess.GenericWrite | FileAccess.GenericRead;
        private const FileAccess DataWriteAccess = FileAccess.WriteData | FileAccess.AppendData | FileAccess.Delete | FileAccess.GenericWrite;

        class FileCaching
        {
            public string FilePath;
            public string OriginalFileName;
            public FileInformation FileInfo;
            public TriState Modified;
        }

        class FolderCaching
        {
        }

        class Context
        {
            public bool Writable;
        }

        private readonly List<string> RootFolders = new List<string> { "\\ALL", "\\CLIENT FAVOURITES", "\\CLIENT LAST10", "\\MATTER FAVOURITES", "\\MATTER LAST10" };
        private readonly ConcurrentDictionary<string, FolderCaching> FolderCache = new ConcurrentDictionary<string, FolderCaching>();
        private readonly ConcurrentDictionary<string, long> DocIDCache = new ConcurrentDictionary<string, long>();
        private readonly ConcurrentDictionary<long, FileCaching> FileCache = new ConcurrentDictionary<long, FileCaching>();

        private readonly int _metadataCacheTimeout;
        private MemoryCache _metadataCache;

        private readonly CacheItemPolicy _binaryCachePolicy;
        private readonly CacheItemPolicy _permanentCachePolicy;
        private MemoryCache _binaryCache;

        private readonly int _operationTimeout;
        private BlockingCollection<Connection> _dbConnections;
        private string _mountPoint;
        private FileInfo _securityFile;
        private readonly System.Windows.Forms.Form _utilsWindow;

        #endregion

        public DMSOperations(System.Windows.Forms.Form utilsWindow)
        {
            _utilsWindow = utilsWindow;
            _operationTimeout = Registry.OperationTimeout;
            _metadataCacheTimeout = Registry.MetadataCacheTimeout;
            _binaryCachePolicy = new CacheItemPolicy() { SlidingExpiration = TimeSpan.FromSeconds(Registry.BinaryCacheTimeout) };
            _permanentCachePolicy = new CacheItemPolicy { SlidingExpiration = ObjectCache.NoSlidingExpiration, Priority = CacheItemPriority.NotRemovable };

            if (!Registry.ShowAllFolders)
                RootFolders.RemoveAt(0);
        }

        public bool IsMounted { get; private set; }

        public void Mount(string mountPoint, Connection dbConnection)
        {
            if (string.IsNullOrWhiteSpace(mountPoint))
                throw new ArgumentException(nameof(mountPoint));

            if (IsMounted)
                throw new InvalidOperationException($"The drive is already mounted ({_mountPoint}).");

            _dbConnections = new BlockingCollection<Connection>(Registry.MaxDatabaseConnections);
            for (int i = 0; i < _dbConnections.BoundedCapacity; i++)
            {
                _dbConnections.Add((Connection)dbConnection.Clone());
            }

            _mountPoint = mountPoint;
            _securityFile = Global.GetTempFile();

            using (StreamWriter sw = _securityFile.CreateText())
                sw.Write(mountPoint);

            DokanOptions mountOptions = DokanOptions.RemovableDrive | DokanOptions.CurrentSession | DokanOptions.EnableFCBGC;
            Dokan.Mount(this, _mountPoint, mountOptions, 0, 150, TimeSpan.FromSeconds(_operationTimeout), new DokanNet.Logging.NullLogger());
        }

        public void Unmount()
        {
            try { _dbConnections.CompleteAdding(); } catch { }
            Dokan.RemoveMountPoint(_mountPoint);
            try { _securityFile.Delete(); } catch { }
            try { Directory.Delete(GetPath(string.Empty), true); } catch { }
            ExplorerHelper.RefreshWindows();
        }
        
        #region IDokanOperations

        void IDokanOperations.Cleanup(string fileName, IDokanFileInfo info)
        {
            FileStream fileStream = info.Context as FileStream;
            if (fileStream != null)
            {
                fileStream.Dispose();
                info.Context = null;
                if (info.DeleteOnClose)
                    File.Delete(fileStream.Name);
            }
            else if (info.DeleteOnClose)
            {
                string path = GetPath(fileName);
                if (info.IsDirectory)
                    Directory.Delete(path);
                else
                    File.Delete(path);
            }
        }

        void IDokanOperations.CloseFile(string fileName, IDokanFileInfo info)
        {
            if ((info.Context is Context) && ((Context)info.Context).Writable)
            {
                long docID;
                FileCaching fc;
                if (DocIDCache.TryGetValue(fileName.ToUpper(), out docID) && FileCache.TryGetValue(docID, out fc) && fc.Modified == TriState.True)
                {
                    MemoryStream memStream = _binaryCache.Get(docID.ToString()) as MemoryStream;
                    fc.Modified = TriState.False;
                    if (memStream != null)
                        _utilsWindow.BeginInvoke(new Action<long, MemoryStream>(SaveDocumentToMatterSphere), docID, memStream);
                }
            }
            info.Context = null;
        }

        NtStatus IDokanOperations.CreateFile(string fileName, FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, IDokanFileInfo info)
        {
            long docID;
            bool writeAccessReq = ((access & DataWriteAccess) != 0);
            string fileNameKey = fileName.ToUpper();

            if (fileNameKey == "\\"
                || RootFolders.Contains(fileNameKey)
                || FolderCache.ContainsKey(fileNameKey))
            {
                return (access & FileAccess.Delete) == FileAccess.Delete ? StatusMediaWriteProtected : DokanResult.Success;
            }
            else if (DocIDCache.TryGetValue(fileNameKey, out docID))
            {
                if ((access & (FileAccess.Delete | FileAccess.GenericWrite | FileAccess.Synchronize)) == FileAccess.Delete)
                    return StatusMediaWriteProtected; // Prevent pure deletion.

                if (writeAccessReq && !Registry.IsProcessWriteAllowed(info.ProcessId))
                    return Registry.IsManagedOfficeProcess(info.ProcessId) ? DokanResult.AccessDenied : StatusMediaWriteProtected;

                FileCaching fc;
                if (!FileCache.TryGetValue(docID, out fc) || (fc.Modified == TriState.Null))
                    return DokanResult.FileNotFound;

                info.Context = new Context() { Writable = writeAccessReq };
                return DokanResult.Success;
            }
            else if (Registry.IsProcessWriteAllowed(info.ProcessId))
            {
                string extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension) || extension.Equals(".tmp", StringComparison.OrdinalIgnoreCase)
                    || Path.GetFileName(fileName).StartsWith("~$")
                    || (attributes & (FileAttributes.Hidden | FileAttributes.Temporary)) != 0)
                    return CreateTemporaryFile(GetPath(fileName), access, share, mode, options, attributes, info);
            }

            switch (mode)
            {
                case FileMode.CreateNew:
                case FileMode.Create:
                case FileMode.Append:
                    return StatusMediaWriteProtected;
                case FileMode.Open:
                    return info.IsDirectory ? DokanResult.PathNotFound : DokanResult.FileNotFound;
                case FileMode.OpenOrCreate:
                    return writeAccessReq ? StatusMediaWriteProtected : DokanResult.FileNotFound;
                case FileMode.Truncate:
                    return DokanResult.FileNotFound;
                default:
                    return DokanResult.InternalError;
            }
        }

        private NtStatus CreateTemporaryFile(string filePath, FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, IDokanFileInfo info)
        {
            NtStatus result = DokanResult.Success;
            if (info.IsDirectory)
            {
                try
                {
                    switch (mode)
                    {
                        case FileMode.Open:
                            if (!Directory.Exists(filePath))
                            {
                                try
                                {
                                    if (!File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
                                        return DokanResult.NotADirectory;
                                }
                                catch (Exception)
                                {
                                    return DokanResult.FileNotFound;
                                }
                                return DokanResult.PathNotFound;
                            }

                            new DirectoryInfo(filePath).EnumerateFileSystemInfos().Any();
                            // you can't list the directory
                            break;

                        case FileMode.CreateNew:
                            if (Directory.Exists(filePath))
                                return DokanResult.FileExists;

                            try
                            {
                                File.GetAttributes(filePath).HasFlag(FileAttributes.Directory);
                                return DokanResult.AlreadyExists;
                            }
                            catch (IOException)
                            {
                            }

                            return StatusMediaWriteProtected;
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    return DokanResult.AccessDenied;
                }
            }
            else
            {
                bool pathExists = true, pathIsDirectory = false;
                try
                {
                    pathExists = (Directory.Exists(filePath) || File.Exists(filePath));
                    pathIsDirectory = pathExists ? File.GetAttributes(filePath).HasFlag(FileAttributes.Directory) : false;
                }
                catch (IOException)
                {
                }

                switch (mode)
                {
                    case FileMode.Open:
                        if (pathExists)
                        {
                            // check if driver only wants to read attributes, security info, or open directory
                            if ((access & DataAccess) == 0 || pathIsDirectory)
                            {
                                if (pathIsDirectory && (access & FileAccess.Delete) == FileAccess.Delete && (access & FileAccess.Synchronize) != FileAccess.Synchronize)
                                    return DokanResult.AccessDenied; //It is a DeleteFile request on a directory

                                info.IsDirectory = pathIsDirectory;
                                info.Context = new Context();
                                // must set it to something if you return DokanResult.Success
                                return DokanResult.Success;
                            }
                        }
                        else
                        {
                            return DokanResult.FileNotFound;
                        }
                        break;

                    case FileMode.CreateNew:
                        if (pathExists)
                            return DokanResult.FileExists;
                        else if ((access & DataWriteAccess) == 0)
                            access |= FileAccess.WriteData; // Workaround to PowerPoint issue.
                        break;

                    case FileMode.Truncate:
                        if (!pathExists)
                            return DokanResult.FileNotFound;
                        break;
                }

                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath)); // Doesn't exist in Mirror sample, but added here.
                    info.Context = new FileStream(filePath, mode, (access & DataWriteAccess) == 0 ? System.IO.FileAccess.Read : System.IO.FileAccess.ReadWrite, share, 4096, options);

                    if (pathExists && (mode == FileMode.OpenOrCreate || mode == FileMode.Create))
                        result = DokanResult.AlreadyExists;

                    bool fileCreated = mode == FileMode.CreateNew || mode == FileMode.Create || (!pathExists && mode == FileMode.OpenOrCreate);
                    if (fileCreated)
                    {
                        attributes |= FileAttributes.Archive; // Files are always created as Archive
                        attributes &= ~FileAttributes.Normal; // FILE_ATTRIBUTE_NORMAL is override if any other attribute is set.
                        File.SetAttributes(filePath, attributes);
                    }
                }
                catch (UnauthorizedAccessException) // don't have access rights
                {
                    if (info.Context is FileStream)
                    {
                        ((FileStream)info.Context).Dispose();
                        info.Context = null;
                    }
                    return DokanResult.AccessDenied;
                }
                catch (DirectoryNotFoundException)
                {
                    return DokanResult.PathNotFound;
                }
                catch (Exception)
                {
                    return DokanResult.SharingViolation;
                }
            }
            return result;
        }

        NtStatus IDokanOperations.DeleteDirectory(string fileName, IDokanFileInfo info)
        {
            return StatusMediaWriteProtected;
        }

        NtStatus IDokanOperations.DeleteFile(string fileName, IDokanFileInfo info)
        {
            string fileNameKey = fileName.ToUpper();
            if (fileNameKey == "\\" || RootFolders.Contains(fileNameKey) || FolderCache.ContainsKey(fileNameKey) || DocIDCache.ContainsKey(fileNameKey))
            {
                if (info.IsDirectory || !Registry.IsProcessWriteAllowed(info.ProcessId))
                    return StatusMediaWriteProtected;
            }
            else
            {
                fileNameKey = GetPath(fileName);
                if (Directory.Exists(fileNameKey))
                    return DokanResult.AccessDenied;
                else if (!File.Exists(fileNameKey))
                    return DokanResult.FileNotFound;
                else if (File.GetAttributes(fileNameKey).HasFlag(FileAttributes.Directory))
                    return DokanResult.AccessDenied;
            }
            return DokanResult.Success;
        }

        NtStatus IDokanOperations.FindFiles(string searchPath, out IList<FileInformation> files, IDokanFileInfo info)
        {
            if (searchPath == "\\")
            {
                files = new List<FileInformation>(RootFolders.Count + 2);
                files.Add(NewFolderInfo("."));
                files.Add(NewFolderInfo(".."));
                foreach (string rootFolder in RootFolders)
                {
                    files.Add(NewFolderInfo(rootFolder.TrimStart('\\')));
                }
            }
            else if ((files = _metadataCache.Get(searchPath) as IList<FileInformation>) == null)
            {
                string clNo = string.Empty;
                string fileNo = string.Empty;
                string folderPath = string.Empty;

                int folderDepth = searchPath.Count(x => x == '\\');
                if (folderDepth == 1)
                {
                    folderPath = searchPath;
                }
                else
                {
                    string relativeName = string.Empty;
                    foreach (string rootFolder in RootFolders)
                    {
                        if (searchPath.StartsWith(rootFolder))
                        {
                            relativeName = searchPath.Substring(rootFolder.Length).TrimStart('\\');
                            break;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(relativeName))
                    {
                        int charLocation = relativeName.IndexOf('\\');
                        if (charLocation > 0)
                        {
                            relativeName = relativeName.Substring(0, charLocation).Replace('-', '\\') + relativeName.Substring(charLocation);
                        }
                        else
                        {
                            relativeName = relativeName.Replace('-', '\\');
                        }
                    }

                    string[] splitFileName = relativeName.Split('\\');

                    if (splitFileName.Length == 1)
                    {
                        clNo = splitFileName[0];
                    }
                    else if (splitFileName.Length > 1)
                    {
                        clNo = splitFileName[0];
                        fileNo = splitFileName[1];
                    }

                    if (splitFileName.Length > 2)
                    {
                        for (int i = 2; i< splitFileName.Length; i++)
                        {
                            folderPath += "\\" + splitFileName[i].Trim();
                        }
                    }
                }

                Connection dbConnection = _dbConnections.Take();
                try
                {
                    files = QueryData(dbConnection, clNo, fileNo, folderPath, searchPath);
                }
                finally
                {
                    try { _dbConnections.Add(dbConnection); }
                    catch { dbConnection.Dispose(); throw; }
                }
                _metadataCache.Set(searchPath, files, DateTimeOffset.UtcNow.AddSeconds(_metadataCacheTimeout));
            }
            return DokanResult.Success;
        }

        NtStatus IDokanOperations.FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, IDokanFileInfo info)
        {
            files = new FileInformation[0];
            return DokanResult.NotImplemented;
        }

        NtStatus IDokanOperations.FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info)
        {
            streams = new FileInformation[0];
            return DokanResult.NotImplemented;
        }

        NtStatus IDokanOperations.FlushFileBuffers(string fileName, IDokanFileInfo info)
        {
            try
            {
                (info.Context as FileStream)?.Flush();
                return DokanResult.Success;
            }
            catch (IOException)
            {
                return DokanResult.DiskFull;
            }
        }

        NtStatus IDokanOperations.GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info)
        {
            freeBytesAvailable = 0;// 512 * 1024 * 1024;
            totalNumberOfBytes = 0;// 1024 * 1024 * 1024;
            totalNumberOfFreeBytes = 512 * 1024 * 1024;
            return DokanResult.Success;
        }

        NtStatus IDokanOperations.GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info)
        {
            string fileNameKey = fileName.ToUpper();
            fileInfo = new FileInformation { FileName = fileName };

            if (fileNameKey == "\\" || RootFolders.Contains(fileNameKey) || FolderCache.ContainsKey(fileNameKey))
            {
                fileInfo.Attributes = FileAttributes.Directory;
                fileInfo.LastAccessTime = DateTime.UtcNow;
                return DokanResult.Success;
            }
            
            if (info.Context is FileStream)
            {
                FileStream fileStream = (FileStream)info.Context;
                fileInfo.Attributes = File.GetAttributes(fileStream.Name);
                fileInfo.Length = fileStream.Length;
                long creationTime = 0, lastAccessTime = 0, lastWriteTime = 0;
                if (GetFileTime(fileStream.SafeFileHandle, ref creationTime, ref lastAccessTime, ref lastWriteTime))
                {
                    fileInfo.CreationTime = DateTime.FromFileTimeUtc(creationTime);
                    fileInfo.LastAccessTime = DateTime.FromFileTimeUtc(lastAccessTime);
                    fileInfo.LastWriteTime = DateTime.FromFileTimeUtc(lastWriteTime);
                }
                return DokanResult.Success;
            }
            
            long docID;
            if (DocIDCache.TryGetValue(fileNameKey, out docID))
            {
                FileCaching fc;
                if (FileCache.TryGetValue(docID, out fc) && fc.Modified != TriState.Null)
                {
                    fileInfo = fc.FileInfo;
                    fileInfo.FileName = fileName;
                    return DokanResult.Success;
                }
                else
                {
                    return DokanResult.FileNotFound;
                }
            }

            string filePath = GetPath(fileName);
            FileSystemInfo finfo = new FileInfo(filePath);
            if (!finfo.Exists)
            {
                finfo = new DirectoryInfo(filePath);
                if (!finfo.Exists)
                    return DokanResult.FileNotFound;
            }

            fileInfo = new FileInformation
            {
                FileName = fileName,
                Attributes = finfo.Attributes,
                CreationTime = finfo.CreationTimeUtc,
                LastAccessTime = finfo.LastAccessTimeUtc,
                LastWriteTime = finfo.LastWriteTimeUtc,
                Length = (finfo as FileInfo)?.Length ?? 0,
            };

            return DokanResult.Success;
        }

        NtStatus IDokanOperations.GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            security = null;
            try
            {
                string fileNameKey = fileName.ToUpper();
                if (fileNameKey == "\\" || RootFolders.Contains(fileNameKey) || FolderCache.ContainsKey(fileNameKey) || DocIDCache.ContainsKey(fileNameKey))
                {
                    security = info.IsDirectory
                        ? (FileSystemSecurity)_securityFile.Directory.GetAccessControl(sections)
                        : _securityFile.GetAccessControl(sections);
                }
                else
                {
                    fileNameKey = GetPath(fileName);
                    security = info.IsDirectory
                        ? (FileSystemSecurity)Directory.GetAccessControl(fileNameKey, sections)
                        : File.GetAccessControl(fileNameKey, sections);
                }
                return DokanResult.Success;
            }
            catch (UnauthorizedAccessException)
            {
                return DokanResult.AccessDenied;
            }
            catch (FileNotFoundException)
            {
                return DokanResult.FileNotFound;
            }
            catch (DirectoryNotFoundException)
            {
                return DokanResult.PathNotFound;
            }
        }

        NtStatus IDokanOperations.GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info)
        {
            volumeLabel = "MatterSphere";
            features = FileSystemFeatures.ReadOnlyVolume | FileSystemFeatures.UnicodeOnDisk;
            fileSystemName = "MSFS"; //"NTFS"
            maximumComponentLength = 256;
            return DokanResult.Success;
        }

        NtStatus IDokanOperations.LockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            try
            {
                (info.Context as FileStream)?.Lock(offset, length);
                return DokanResult.Success;
            }
            catch (IOException)
            {
                return DokanResult.AccessDenied;
            }
        }

        NtStatus IDokanOperations.Mounted(IDokanFileInfo info)
        {
            _metadataCache = new MemoryCache("MetadataCache");
            _binaryCache = new MemoryCache("BinaryCache");
            IsMounted = true;
            return DokanResult.Success;
        }

        NtStatus IDokanOperations.MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
        {
            long docID;
            FileCaching fc;
            Stream oldStream, newStream, ctxStream = info.Context as Stream;

            if (!Registry.IsProcessWriteAllowed(info.ProcessId))
                return StatusMediaWriteProtected;

            if (!Path.GetExtension(oldName).Equals(".tmp", StringComparison.OrdinalIgnoreCase) &&
                !Path.GetExtension(newName).Equals(".tmp", StringComparison.OrdinalIgnoreCase))
                return StatusMediaWriteProtected; // Only allow moving files to temp and back

            if (ctxStream != null)
            {
                ctxStream.Dispose();
                info.Context = new Context();
            }

            if (DocIDCache.TryGetValue(newName.ToUpper(), out docID) && FileCache.TryGetValue(docID, out fc))
            {   // Move from temp to drive
                oldStream = File.OpenRead(GetPath(oldName));
                ctxStream = newStream = GetMemoryStream(fc, docID.ToString(), true);
            }
            else if (DocIDCache.TryGetValue(oldName.ToUpper(), out docID) && FileCache.TryGetValue(docID, out fc))
            {   // Move from drive to temp
                string newPath = GetPath(newName);
                if (replace || !File.Exists(newPath))
                    newStream = File.OpenWrite(newPath);
                else
                    return DokanResult.FileExists;
                ctxStream = oldStream = GetMemoryStream(fc, docID.ToString(), true);
            }
            else
            {
                return DokanResult.FileNotFound;
            }

            lock (ctxStream)
            {
                oldStream.Position = 0;
                newStream.Position = 0;
                oldStream.CopyTo(newStream);
                newStream.SetLength(oldStream.Length);

                if (oldStream is FileStream)
                {
                    fc.Modified = TriState.True;
                    fc.FileInfo.Length = newStream.Length;
                    fc.FileInfo.LastWriteTime = DateTime.UtcNow;

                    if (info.Context is Context)
                        ((Context)info.Context).Writable = true;

                    ((FileStream)oldStream).Dispose();
                    File.Delete(((FileStream)oldStream).Name);
                }
                else if (newStream is FileStream)
                {
                    fc.Modified = TriState.Null; // Mark as temporary unavailable
                    ((FileStream)newStream).Dispose();
                }
            }

            return DokanResult.Success;
        }

        NtStatus IDokanOperations.ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
        {
            long docID;
            FileCaching fc;
            bytesRead = 0;

            Stream stream = info.Context as FileStream;
            if (stream == null && DocIDCache.TryGetValue(fileName.ToUpper(), out docID) && FileCache.TryGetValue(docID, out fc))
            {
                stream = GetMemoryStream(fc, docID.ToString(), false);
            }

            if (stream != null)
            {
                lock (stream) //Protect from overlapped read
                {
                    stream.Position = offset;
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
                return DokanResult.Success;
            }

            string filePath = GetPath(fileName);
            if (File.Exists(filePath))
            {
                using (stream = File.OpenRead(filePath))
                {
                    stream.Position = offset;
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
                return DokanResult.Success;
            }

            return DokanResult.FileNotFound;
        }

        NtStatus IDokanOperations.SetAllocationSize(string fileName, long length, IDokanFileInfo info)
        {
            long docID;
            FileCaching fc;
            try
            {
                if (info.Context is FileStream)
                {
                    ((FileStream)info.Context).SetLength(length);
                }
                else if (DocIDCache.TryGetValue(fileName.ToUpper(), out docID) && FileCache.TryGetValue(docID, out fc))
                {
                    MemoryStream memStream = GetMemoryStream(fc, docID.ToString(), true);
                    lock (memStream)
                    {
                        memStream.SetLength(length);
                        fc.FileInfo.Length = length;
                        fc.Modified = TriState.True;
                    }
                }
                return DokanResult.Success;
            }
            catch (IOException)
            {
                return DokanResult.DiskFull;
            }
        }

        NtStatus IDokanOperations.SetEndOfFile(string fileName, long length, IDokanFileInfo info)
        {
            return ((IDokanOperations)this).SetAllocationSize(fileName, length, info);
        }

        NtStatus IDokanOperations.SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info)
        {
            long docID;
            FileCaching fc;
            try
            {
                if (attributes == 0)
                {
                    return DokanResult.Success;
                }
                else if (info.Context is FileStream)
                {
                    File.SetAttributes(((FileStream)info.Context).Name, attributes);
                    return DokanResult.Success;
                }
                else if (DocIDCache.TryGetValue(fileName.ToUpper(), out docID) && FileCache.TryGetValue(docID, out fc))
                {
                    return Registry.IsProcessWriteAllowed(info.ProcessId) ? DokanResult.Success : StatusMediaWriteProtected;
                }
                else
                {
                    File.SetAttributes(GetPath(fileName), attributes);
                    return DokanResult.Success;
                }
            }
            catch (UnauthorizedAccessException)
            {
                return DokanResult.AccessDenied;
            }
            catch (FileNotFoundException)
            {
                return DokanResult.FileNotFound;
            }
            catch (DirectoryNotFoundException)
            {
                return DokanResult.PathNotFound;
            }
        }

        NtStatus IDokanOperations.SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            try
            {
                string fileNameKey = fileName.ToUpper();
                if (fileNameKey == "\\" || RootFolders.Contains(fileNameKey) || FolderCache.ContainsKey(fileNameKey) || DocIDCache.ContainsKey(fileNameKey))
                {
                    return Registry.IsProcessWriteAllowed(info.ProcessId) ? DokanResult.Success : StatusMediaWriteProtected;
                }
                else
                {
                    fileNameKey = GetPath(fileName);
                    if (info.IsDirectory)
                        Directory.SetAccessControl(fileNameKey, (DirectorySecurity)security);
                    else
                        File.SetAccessControl(fileNameKey, (FileSecurity)security);
                }
                return DokanResult.Success;
            }
            catch (UnauthorizedAccessException)
            {
                return DokanResult.AccessDenied;
            }
            catch (FileNotFoundException)
            {
                return DokanResult.FileNotFound;
            }
            catch (DirectoryNotFoundException)
            {
                return DokanResult.PathNotFound;
            }
        }

        NtStatus IDokanOperations.SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info)
        {
            long docID;
            FileCaching fc;
            try
            {
                if (info.Context is FileStream)
                {
                    long ct = creationTime.HasValue ? creationTime.Value.ToFileTime() : 0;
                    long lat = lastAccessTime.HasValue ? lastAccessTime.Value.ToFileTime() : 0;
                    long lwt = lastWriteTime.HasValue ? lastWriteTime.Value.ToFileTime() : 0;
                    if ((ct == 0 && lat == 0 && lwt == 0) || SetFileTime(((FileStream)info.Context).SafeFileHandle, ref ct, ref lat, ref lwt))
                        return DokanResult.Success;
                    else
                        throw Marshal.GetExceptionForHR(Marshal.GetLastWin32Error());
                }
                else if (DocIDCache.TryGetValue(fileName.ToUpper(), out docID) && FileCache.TryGetValue(docID, out fc))
                {
                    if (!Registry.IsProcessWriteAllowed(info.ProcessId))
                        return StatusMediaWriteProtected;

                    if (creationTime.HasValue)
                        fc.FileInfo.CreationTime = creationTime.Value.ToUniversalTime();

                    if (lastAccessTime.HasValue)
                        fc.FileInfo.LastAccessTime = lastAccessTime.Value.ToUniversalTime();

                    if (lastWriteTime.HasValue)
                        fc.FileInfo.LastWriteTime = lastWriteTime.Value.ToUniversalTime();

                    return DokanResult.Success;
                }
                else
                {
                    string filePath = GetPath(fileName);

                    if (creationTime.HasValue)
                        File.SetCreationTime(filePath, creationTime.Value);

                    if (lastAccessTime.HasValue)
                        File.SetLastAccessTime(filePath, lastAccessTime.Value);

                    if (lastWriteTime.HasValue)
                        File.SetLastWriteTime(filePath, lastWriteTime.Value);

                    return DokanResult.Success;
                }
            }
            catch (UnauthorizedAccessException)
            {
                return DokanResult.AccessDenied;
            }
            catch (FileNotFoundException)
            {
                return DokanResult.FileNotFound;
            }
        }

        NtStatus IDokanOperations.UnlockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            try
            {
                (info.Context as FileStream)?.Unlock(offset, length);
                return DokanResult.Success;
            }
            catch (IOException)
            {
                return DokanResult.AccessDenied;
            }
        }

        NtStatus IDokanOperations.Unmounted(IDokanFileInfo info)
        {
            IsMounted = false;
            _metadataCache?.Dispose(); _metadataCache = null;
            _binaryCache?.Dispose(); _binaryCache = null;
            FileCache.Clear();
            FolderCache.Clear();
            DocIDCache.Clear();
            if (_dbConnections != null)
            {
                Connection connection;
                while (_dbConnections.TryTake(out connection))
                {
                    connection.Dispose();
                }
                _dbConnections.Dispose();
                _dbConnections = null;
            }
            return DokanResult.Success;
        }

        NtStatus IDokanOperations.WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
        {
            long docID;
            FileCaching fc = null;
            bytesWritten = 0;

            Stream stream = info.Context as FileStream;
            if (stream == null && DocIDCache.TryGetValue(fileName.ToUpper(), out docID) && FileCache.TryGetValue(docID, out fc))
            {
                stream = GetMemoryStream(fc, docID.ToString(), true);
            }

            if (stream != null)
            {
                lock (stream) // Protect from overlapped write
                {
                    if (offset == -1) // Append
                        stream.Seek(0, SeekOrigin.End);
                    else
                        stream.Position = offset;

                    stream.Write(buffer, 0, buffer.Length);

                    if (stream is MemoryStream)
                    {
                        fc.FileInfo.Length = stream.Length;
                        fc.FileInfo.LastWriteTime = DateTime.UtcNow;
                        fc.Modified = TriState.True;
                    }
                }

                bytesWritten = buffer.Length;
                return DokanResult.Success;
            }

            return StatusMediaWriteProtected;
        }

        #endregion IDokanOperations

        #region Private Methods

        private IList<FileInformation> QueryData(IConnection dbConnection, string clNo, string fileNo, string folderPath, string parentFolder)
        {
            List<FileInformation> files = new List<FileInformation>();
            string pathSeparator = (parentFolder == "\\") ? "" : "\\";

            ExecuteParameters ep = new ExecuteParameters()
            {
                CommandType = CommandType.StoredProcedure,
                Sql = "VirtualDriveFindFiles",
                DefaultDateTimeKind = DateTimeKind.Utc
            };
            ep.Parameters.Add(dbConnection.CreateParameter("@clNo", clNo));
            ep.Parameters.Add(dbConnection.CreateParameter("@fileNo", fileNo));
            ep.Parameters.Add(dbConnection.CreateParameter("@folderPath", folderPath));

            using (IDataReader reader = dbConnection.ExecuteReader(ep))
            {
                while (reader.Read())
                {
                    FileInformation finfo = new FileInformation();
                    finfo.FileName = Convert.ToString(reader[0]);
                    finfo.Attributes = Convert.ToBoolean(reader[1]) ? FileAttributes.Directory : FileAttributes.Normal;

                    finfo.LastAccessTime = Convert.IsDBNull(reader[3]) ? DateTime.UtcNow : Convert.ToDateTime(reader[3]);
                    finfo.LastWriteTime = Convert.IsDBNull(reader[4]) ? DateTime.UtcNow : Convert.ToDateTime(reader[4]);
                    finfo.CreationTime = Convert.IsDBNull(reader[5]) ? DateTime.UtcNow : Convert.ToDateTime(reader[5]);

                    if (finfo.Attributes == FileAttributes.Directory)
                    {
                        string folderNameWithParent = (parentFolder + pathSeparator + finfo.FileName).ToUpper();
                        if (!FolderCache.ContainsKey(folderNameWithParent))
                        {
                            FolderCache.TryAdd(folderNameWithParent, new FolderCaching());
                        }
                    }
                    else
                    {
                        FileCaching fc;
                        long docID = Convert.ToInt64(reader[2]);

                        if (!FileCache.TryGetValue(docID, out fc))
                        {
                            fc = NewFileCachingInfo(finfo.FileName);

                            fc.OriginalFileName = finfo.FileName;
                            fc.FileInfo.LastAccessTime = finfo.LastAccessTime;
                            fc.FileInfo.CreationTime = finfo.CreationTime;
                            fc.FileInfo.LastWriteTime = finfo.LastWriteTime;

                            fc.FilePath = Convert.ToString(reader[6]);
                            if (!string.IsNullOrEmpty(fc.FilePath))
                            {
                                FileInfo fi = new FileInfo(fc.FilePath);
                                fc.FileInfo.Length = finfo.Length = fi.Exists ? fi.Length : 0;
                            }

                            FileCache.TryAdd(docID, fc);
                        }
                        else
                        {
                            if (finfo.LastWriteTime > fc.FileInfo.LastWriteTime && fc.Modified == TriState.False)
                            {
                                _binaryCache.Remove(docID.ToString());

                                fc.FileInfo.LastWriteTime = finfo.LastWriteTime;
                                fc.FilePath = Convert.ToString(reader[6]);
                                if (!string.IsNullOrEmpty(fc.FilePath))
                                {
                                    FileInfo fi = new FileInfo(fc.FilePath);
                                    fc.FileInfo.Length = finfo.Length = fi.Exists ? fi.Length : 0;
                                }
                                else
                                {
                                    fc.FileInfo.Length = finfo.Length = 0;
                                }
                            }

                            if (!finfo.FileName.Equals(fc.OriginalFileName, StringComparison.CurrentCultureIgnoreCase))
                            {
                                fc.OriginalFileName = finfo.FileName;
                                long oldId;
                                foreach (string oldPath in DocIDCache.Where(kvp => kvp.Value == docID).Select(kvp => kvp.Key))
                                {
                                    DocIDCache.TryRemove(oldPath, out oldId);
                                }
                            }

                            finfo.Length = fc.FileInfo.Length;
                            finfo.LastWriteTime = fc.FileInfo.LastWriteTime;
                            finfo.FileName = fc.OriginalFileName;
                        }

                        int copyCount = 0;
                        string fileNameWithDir = parentFolder + pathSeparator + finfo.FileName;
                        while (DocIDCache.GetOrAdd(fileNameWithDir.ToUpper(), docID) != docID)
                        {
                            copyCount++;
                            fileNameWithDir = parentFolder + pathSeparator + Path.GetFileNameWithoutExtension(finfo.FileName) + " - [copy " + copyCount.ToString() + "]" + Path.GetExtension(finfo.FileName);
                        }
                        if (copyCount > 0)
                        {
                            fc.FileInfo.FileName = finfo.FileName = Path.GetFileName(fileNameWithDir);
                        }
                    }

                    files.Add(finfo);
                }
            }
            return files;
        }

        /// <summary>
        /// Leverage existing infrastructure to open/checkout a document to MatterSphere Documents cache folder.
        /// Overwrite local file with the new content from memory stream.
        /// Let OMS.Utils Monitor Save feature to detect local file changes via FileSystemWatcher and initiate SaveCommand.
        /// </summary>
        /// <param name="docID">Document ID</param>
        /// <param name="memStream">New file content</param>
        private void SaveDocumentToMatterSphere(long docID, MemoryStream memStream)
        {
            FileInfo localFileInfo = ShellApp.EditDocument(docID);
            if (localFileInfo != null)
            {
                try
                {
                    using (FileStream fileStream = localFileInfo.OpenWrite())
                    {
                        lock (memStream)
                        {
                            memStream.Position = 0;
                            memStream.CopyTo(fileStream);
                            fileStream.SetLength(memStream.Length);
                        }
                        System.Diagnostics.Debug.WriteLine($"Stored document {docID} to local file {localFileInfo.FullName}.");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
        }

        private string GetPath(string fileName)
        {
            string filePath = Path.Combine(_securityFile.DirectoryName, _mountPoint.Replace(":", ""));
            foreach (string rootFolder in RootFolders)
            {
                if (fileName.StartsWith(rootFolder))
                {
                    filePath += fileName.Substring(rootFolder.Length);
                    break;
                }
            }
            return filePath;
        }

        private MemoryStream GetMemoryStream(FileCaching fc, string key, bool write)
        {
            MemoryStream memStream;
            lock (fc)
            {
                memStream = _binaryCache.Get(key) as MemoryStream;
                if (memStream == null)
                {
                    if ((memStream = NewFileContent(fc.FilePath)) == null)
                        return memStream;

                    fc.FileInfo.Length = memStream.Length;
                    _binaryCache.Set(key, memStream, _binaryCachePolicy);
                }
            }
            if (write)
            {
                _binaryCache.Set(key, memStream, _permanentCachePolicy);
            }
            return memStream;
        }

        private static MemoryStream NewFileContent(string fileName)
        {
            MemoryStream memStream = null;
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                using (FileStream fileStream = fileInfo.OpenRead())
                {
                    memStream = new MemoryStream((int)fileInfo.Length);
                    fileStream.CopyTo(memStream);
                }
            }
            return memStream;
        }

        private static FileInformation NewFolderInfo(string folderName)
        {
            FileInformation finfo = new FileInformation()
            {
                FileName = folderName,
                Attributes = FileAttributes.Directory,
                CreationTime = DateTime.UtcNow,
                LastAccessTime = DateTime.UtcNow,
                LastWriteTime = DateTime.UtcNow                
            };
            return finfo;
        }

        private static FileCaching NewFileCachingInfo(string fileName)
        {
            FileCaching fc = new FileCaching()
            {
                FilePath = string.Empty,
                OriginalFileName = string.Empty,
                Modified = TriState.False,
                FileInfo = new FileInformation()
                {
                    FileName = fileName,
                    Attributes = FileAttributes.Normal,
                    CreationTime = DateTime.UtcNow,
                    LastAccessTime = DateTime.UtcNow,
                    LastWriteTime = DateTime.UtcNow
                }
            };
            return fc;
        }

        [DllImport("Kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool GetFileTime(Microsoft.Win32.SafeHandles.SafeFileHandle hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);

        [DllImport("Kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool SetFileTime(Microsoft.Win32.SafeHandles.SafeFileHandle hFile, ref long lpCreationTime, ref long lpLastAccessTime, ref long lpLastWriteTime);

        #endregion
    }
}
