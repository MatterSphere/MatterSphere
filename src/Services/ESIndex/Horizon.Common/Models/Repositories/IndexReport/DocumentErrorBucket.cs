namespace Horizon.Common.Models.Repositories.IndexReport
{
    public class DocumentErrorBucket
    {
        public DocumentErrorBucket(string type, long number)
        {
            ErrorType = type;
            Number = number;
        }

        public string ErrorType { get; set; }
        public long Number { get; set; }
    }
}
