using Horizon.Common;

namespace Horizon.Models.IndexReports
{
    public class DocumentErrorBucket
    {
        public DocumentErrorBucket(Common.Models.Repositories.IndexReport.DocumentErrorBucket item)
        {
            ErrorType = item.ErrorType;
            var details = ExceptionConverter.GetExceptionDescription(item.ErrorType);
            ErrorDescription = details.Explanation;
            Recommendation = details.Recommendation;
            Number = item.Number;
        }

        public string ErrorType { get; set; }
        public string ErrorDescription { get; set; }
        public long Number { get; set; }
        public string Recommendation { get; set; }
    }
}
