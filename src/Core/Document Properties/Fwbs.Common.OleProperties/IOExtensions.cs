using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Fwbs.Documents
{
    public static class IOExtensions
    {
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool GetVolumeInformation(
          string RootPathName,
          StringBuilder VolumeNameBuffer,
          int VolumeNameSize,
          out uint VolumeSerialNumber,
          out uint MaximumComponentLength,
          out FileSystemFeature FileSystemFlags,
          StringBuilder FileSystemNameBuffer,
          int nFileSystemNameSize);

        [Flags]
        private enum FileSystemFeature : uint
        {
            /// <summary>
            /// The file system supports case-sensitive file names.
            /// </summary>
            CaseSensitiveSearch = 1,
            /// <summary>
            /// The file system preserves the case of file names when it places a name on disk.
            /// </summary>
            CasePreservedNames = 2,
            /// <summary>
            /// The file system supports Unicode in file names as they appear on disk.
            /// </summary>
            UnicodeOnDisk = 4,
            /// <summary>
            /// The file system preserves and enforces access control lists (ACL).
            /// </summary>
            PersistentACLS = 8,
            /// <summary>
            /// The file system supports file-based compression.
            /// </summary>
            FileCompression = 0x10,
            /// <summary>
            /// The file system supports disk quotas.
            /// </summary>
            VolumeQuotas = 0x20,
            /// <summary>
            /// The file system supports sparse files.
            /// </summary>
            SupportsSparseFiles = 0x40,
            /// <summary>
            /// The file system supports re-parse points.
            /// </summary>
            SupportsReparsePoints = 0x80,
            /// <summary>
            /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
            /// </summary>
            VolumeIsCompressed = 0x8000,
            /// <summary>
            /// The file system supports object identifiers.
            /// </summary>
            SupportsObjectIDs = 0x10000,
            /// <summary>
            /// The file system supports the Encrypted File System (EFS).
            /// </summary>
            SupportsEncryption = 0x20000,
            /// <summary>
            /// The file system supports named streams.
            /// </summary>
            NamedStreams = 0x40000,
            /// <summary>
            /// The specified volume is read-only.
            /// </summary>
            ReadOnlyVolume = 0x80000,
            /// <summary>
            /// The volume supports a single sequential write.
            /// </summary>
            SequentialWriteOnce = 0x100000,
            /// <summary>
            /// The volume supports transactions.
            /// </summary>
            SupportsTransactions = 0x200000,
        }

        public static bool SupportsNamedStreams(System.IO.DirectoryInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            var root = System.IO.Path.GetPathRoot(info.FullName);
            if (!root.EndsWith(@"\"))
                root += @"\";

            const int MAX_PATH = 0x104;
            var volname = new System.Text.StringBuilder(MAX_PATH + 1);
            var fsname = new System.Text.StringBuilder(MAX_PATH + 1);
            UInt32 MaxComponentLength;
            FileSystemFeature FileSystemFlags;

            UInt32 sernum;

            if (!GetVolumeInformation(root, volname, volname.Capacity, out sernum, out MaxComponentLength, out FileSystemFlags, fsname, fsname.Capacity))
                return false;


            return (FileSystemFlags | FileSystemFeature.NamedStreams) == FileSystemFlags;
        }

        public static bool SupportsNamedStreams(System.IO.FileInfo info)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            return SupportsNamedStreams(info.Directory);
        }
    }
}
