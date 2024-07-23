using System;
using System.IO;
using System.Reflection;
using IFilterTextReader;

namespace DocumentReader
{
    public class ContentReaderFactory
    {
        // Make a job object to sandbox the IFilter code
        private static readonly Job _job;
        static ContentReaderFactory()
        {
            _job = new Job();
            _job.AddProcess(System.Diagnostics.Process.GetCurrentProcess().Handle);
        }

        private readonly FilterReaderTimeout _filterReaderTimeout;
        private readonly int _docReadTimeout;

        public ContentReaderFactory(int docReadTimeout = -1)
        {
            _filterReaderTimeout = docReadTimeout == -1
                ? FilterReaderTimeout.NoTimeout
                : FilterReaderTimeout.TimeoutWithException;
            _docReadTimeout = docReadTimeout;
        }

        public TextReader GetTextReader(FileInfo fileInfo)
        {
            var extension = fileInfo.Extension.ToUpperInvariant();
            switch (extension)
            {
                case ".OFT":
                    return new AsposeEmailReader(fileInfo.FullName);
                case ".DOT":
                case ".DOTM":
                case ".DOTX":
                    return new AsposeWordReader(fileInfo.FullName);
                case ".XLT":
                case ".XLTM":
                case ".XLTX":
                    return new AsposeExcelReader(fileInfo.FullName);
                default:
                    return new FilterReader(fileInfo.FullName, extension, new FilterReaderOptions
                    {
                        DisableEmbeddedContent = true,
                        IncludeProperties = false,
                        ReadIntoMemory = false,
                        ReaderTimeout = _filterReaderTimeout,
                        Timeout = _docReadTimeout
                    });
            }
        }

        #region Foxit PDF iFilter Workaround

        // Workaround to Foxit PDF iFilter bug that crashes application on shutdown
        // if DLL unload is performed in a different thread - not the one where this DLL was loaded.
        // Manually initialize PDF iFilter at application startup.
        public static void Startup()
        {
            try
            {
                const string pdfContent = "%PDF-1.\rtrailer <</ Root <</ Pages <</ Kids[<</ MediaBox[0 0 3 3] >>] >>>>>>";
                using (var stream = new MemoryStream(System.Text.Encoding.ASCII.GetBytes(pdfContent)))
                using (var reader = new FilterReader(stream, ".pdf", null))
                {
                }
            }
            catch
            {
            }
        }

        // Manually unload all filter libraries before application shutdown in the same thread as Startup.
        public static void Shutdown()
        {
            Type filterLoader = typeof(FilterReader).Assembly.GetType("IFilterTextReader.FilterLoader");
            filterLoader.InvokeMember("Clear", BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Static, null, null, null);
        }

        #endregion
    }
}
