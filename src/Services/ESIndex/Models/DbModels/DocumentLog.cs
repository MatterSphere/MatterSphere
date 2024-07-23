using System;
using System.IO;
using Common.Enums;

namespace Models.DbModels
{
    public class DocumentLog
    {
        public DocumentLog(long id, string fileName, TimeSpan time, long? size)
        {
            EntityId = id;
            FileName = fileName;
            Time = time;
            Size = size;

            if (!string.IsNullOrWhiteSpace(fileName))
            {
                var extension = Path.GetExtension(fileName);
                Extension = !string.IsNullOrWhiteSpace(extension)
                    ? extension.Substring(1)
                    : extension;
            }
        }

        public long EntityId { get; set; }
        public string FileName { get; set; }
        public TimeSpan Time { get; set; }
        public long? Size { get; set; }
        public string ErrorMessage { get; private set; }
        public string ErrorCode { get; private set; }
        public string Extension { get; private set; }
        public bool HasError { get; private set; }

        public long Ticks
        {
            get { return Time.Ticks; }
        }

        public bool HasContent
        {
            get { return Size.HasValue && Size > 0; }
        }

        public void SetError(DocumentProcessErrorTypeEnum errorType, string error)
        {
            ErrorCode = errorType.ToString();
            ErrorMessage = error;
            HasError = true;
        }
    }
}
