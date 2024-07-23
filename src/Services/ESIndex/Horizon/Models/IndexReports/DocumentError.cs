namespace Horizon.Models.IndexReports
{
    public class DocumentError
    {
        public DocumentError(Horizon.Common.Models.Repositories.IndexReport.DocumentError item, int number)
        {
            Id = item.Id;
            Number = number;
            Name = item.Name;
            ErrorDetails = item.ErrorDetails;
            Path = item.Path;
        }

        public long Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string ErrorDetails { get; set; }
        public string Path { get; set; }
    }
}
