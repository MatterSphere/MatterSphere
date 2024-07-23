namespace Horizon.Common.Models.Repositories.ProcessingStatus
{
    public class DocumentErrorInfo
    {
        public DocumentErrorInfo(long id, string name, string extension, long size, string errorCode, string errorMessage, string path)
        {
            Id = id;
            Name = name;
            Extension = extension;
            Size = size;
            ErrorCode = errorCode;
            ErrorMessage = errorMessage;
            Path = path;
        }

        public long Id { get; }
        public string Name { get; }
        public string Extension { get; }
        public long Size { get; }
        public string ErrorCode { get; }
        public string ErrorMessage { get; }
        public string Path { get; }
    }
}
