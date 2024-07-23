namespace Horizon.Common.Models.Repositories.IndexReport
{
    public class DocumentError
    {
        public DocumentError(long id, string name, string path, string details)
        {
            Id = id;
            Name = name;
            Path = path;
            ErrorDetails = details;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string ErrorDetails { get; set; }
    }
}
