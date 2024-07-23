using CsvHelper.Configuration.Attributes;

namespace Horizon.Models.ReportProvider
{
    public class ReportItem
    {
        public ReportItem(string extension, long successNumber, long errorNumber)
        {
            Extension = extension;
            SuccessNumber = successNumber;
            ErrorNumber = errorNumber;
        }

        public ReportItem(string message, long number)
        {
            Message = message;
            ErrorNumber = number;
        }

        [Name("Extension")]
        public string Extension { get; set; }

        [Name("Success Number")]
        public long SuccessNumber { get; set; }

        [Name("Error Number")]
        public long ErrorNumber { get; set; }

        [Name("Message")]
        public string Message { get; set; }
    }
}
