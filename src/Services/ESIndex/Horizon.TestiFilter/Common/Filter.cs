using System;
using System.Diagnostics;
using System.Linq;

namespace Horizon.TestiFilter.Common
{
    public class Filter
    {
        public Filter(string extension, string details, string description, string path)
        {
            Extension = extension;
            Details = details;
            Description = description;
            Path = path;
        }

        public string Extension { get; private set; }
        public string Details { get; private set; }
        public string Description { get; private set; }
        public string Path { get; private set; }

        public string FileName
        {
            get
            {
                if (FileVersionInfo == null && string.IsNullOrEmpty(Path))
                {
                    return null;
                }

                if (!string.IsNullOrEmpty(Path))
                {
                    return Path.Split('\\').Last();
                }

                return null;
            }
        }

        public string FileDescription
        {
            get { return FileVersionInfo?.FileDescription; }
        }

        public string FileVersion
        {
            get { return (FileVersionInfo?.FileVersion ?? string.Empty).Split(' ')[0]; }
        }

        public string Company
        {
            get { return FileVersionInfo?.CompanyName; }
        }

        private bool fileVersionInfoChecked;
        private FileVersionInfo _fileVersionInfo;
        private FileVersionInfo FileVersionInfo
        {
            get
            {
                if (!fileVersionInfoChecked)
                {
                    fileVersionInfoChecked = true;
                    if (!string.IsNullOrEmpty(Path))
                    {
                        try
                        {
                            _fileVersionInfo = FileVersionInfo.GetVersionInfo(Path);
                        }
                        catch
                        {
                        }
                    }
                }

                return _fileVersionInfo;
            }
        }
    }
}
