using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace Fwbs.Documents.Preview
{
    internal class ReadOnlyStream : Stream
    {
        private IStream stream;

        public ReadOnlyStream(IStream stream)
        {
            this.stream = stream;
        }

        protected override void Dispose(bool disposing)
        {
            stream = null;
            base.Dispose(disposing);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (stream == null)
                throw new ObjectDisposedException("stream");
            if (buffer == null)
                throw new ArgumentNullException("buffer");
            if (offset < 0)
                throw new ArgumentNullException("offset");
            if (count <= 0)
                return 0;

            int bytesRead = 0;
            IntPtr ptr = new IntPtr(bytesRead);
            if (offset == 0)
            {
                if (count > buffer.Length)
                    throw new ArgumentOutOfRangeException("count");
                stream.Read(buffer, count, ptr);
            }
            else
            {
                byte[] tempBuffer = new byte[count];
                stream.Read(tempBuffer, count, ptr);
                if (bytesRead > 0)
                    Array.Copy(tempBuffer, 0, buffer, offset, bytesRead);
            }
            return bytesRead;
        }

        public override bool CanRead
        { get { return stream != null; } }
        public override bool CanSeek
        { get { return stream != null; } }
        public override bool CanWrite
        { get { return false; } }

        public override long Length
        {
            get
            {
                if (stream == null)
                    throw new ObjectDisposedException("stream");

                STATSTG stats;
                stream.Stat(out stats, 1);
                return stats.cbSize;
            }
        }

        public override long Position
        {
            get
            {
                if (stream == null)
                    throw new ObjectDisposedException("stream");
                return Seek(0, SeekOrigin.Current);
            }
            set
            {
                if (stream == null)
                    throw new ObjectDisposedException("stream");
                Seek(value, SeekOrigin.Begin);
            }
        }

        //used to be unsafe
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (stream == null)
                throw new ObjectDisposedException("stream");
            long pos = 0;
            IntPtr posPtr = new IntPtr(pos);//(void*)&pos
            stream.Seek(offset, (int)origin, posPtr);
            return pos;
        }

        public override void Flush()
        {
            if (stream == null)
                throw new ObjectDisposedException("stream");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }
    }

}
