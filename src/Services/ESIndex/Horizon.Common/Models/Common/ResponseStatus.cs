namespace Horizon.Common.Models.Common
{
    public class ResponseStatus
    {
        public ResponseStatus()
        {
            IsSuccess = true;
        }

        public ResponseStatus(string message)
        {
            IsSuccess = false;
            ErrorMessage = message;
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
