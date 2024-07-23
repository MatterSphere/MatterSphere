using Horizon.Common.Models.Repositories.ProcessingStatus;

namespace Horizon.Models.ProcessingStatus
{
    public class ErrorCodeModel
    {
        public ErrorCodeModel(ErrorCodeItem item)
        {
            ErrorCode = item.ErrorCode;
            Count = item.Count;
        }

        public string ErrorCode { get; set; }
        public int Count { get; set; }
    }
}
