namespace Horizon.Common.Models.Repositories.ProcessingStatus
{
    public class DocumentItem
    {
        public DocumentItem(long id, string name, string path, long size, string error)
        {
            Id = id;
            Name = name;
            Path = path;
            Size = size;
            Error = error;
        }

        public long Id { get; }
        public string Name { get; }
        public long Size { get; }
        public string Error { get; }
        public string Path { get; }
    }
}
