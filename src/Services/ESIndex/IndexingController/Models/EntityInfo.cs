using System;
using System.Dynamic;
using Common.Enums;

namespace IndexingController.Models
{
    public class EntityInfo
    {
        public EntityInfo(ExpandoObject model, long id, Guid key, string fileName, long? size, bool deleted)
        {
            Model = model;
            Key = key;
            Id = id;
            FileName = fileName;
            Size = size;
            Deleted = deleted;
        }

        public ExpandoObject Model { get; set; }
        public long Id { get; set; }
        public string FileName { get; set; }
        public long? Size { get; set; }
        
        public bool HasError { get; set; }
        public DocumentProcessErrorTypeEnum DocumentProcessErrorType { get; set; }
        public string Error { get; set; }

        public Guid Key { get; set; }
        public bool Deleted { get; set; }

        public bool ProcessError { get; set; }
        public bool ContentReadingError { get; set; }

        public void SetError(DocumentProcessErrorTypeEnum type, string error, bool contentReadingError = false)
        {
            ContentReadingError = contentReadingError;
            HasError = true;
            DocumentProcessErrorType = type;
            Error = error;
        }
    }
}
