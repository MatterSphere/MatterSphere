namespace Horizon.Common.Models.Repositories.ProcessingStatus
{
    public class ErrorCodeItem
    {
        public ErrorCodeItem(string errorCode, int count)
        {
            ErrorCode = errorCode;
            Count = count;
        }

        public string ErrorCode { get; }
        public int Count { get; }
    }
}
