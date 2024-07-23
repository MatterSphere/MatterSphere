using System;
using System.IO;
using System.Threading;
using Models.Interfaces;

namespace DocumentReader
{
    public class ContentReader : IContentReader
    {
        protected readonly ContentReaderFactory _factory;
        protected readonly int _timeout;
        protected string _content = string.Empty;
        protected bool _completed;

        public ContentReader(int timeout)
        {
            _timeout = timeout;
            _factory = new ContentReaderFactory(_timeout);
        }

        public string GetContent(string path)
        {
            Exception exception = null;
            var readerThread = new Thread(() => Execute(() => Read(path), out exception));
            readerThread.Start();

            readerThread.Join(_timeout);
            if (readerThread.IsAlive)
            {
                readerThread.Abort();
            }

            if (exception != null)
            {
                throw exception;
            }

            return _completed ? _content : string.Empty;
        }

        [System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
        [System.Security.SecurityCritical]
        private void Execute(Action action, out Exception exception)
        {
            exception = null;

            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                exception = ex;
            }
        }

        protected virtual void Read(string path)
        {
            var fileInfo = new FileInfo(path);
            using (var rdr = _factory.GetTextReader(fileInfo))
            {
                _content = rdr.ReadToEnd();
            }

            _completed = true;
        }
    }
}
