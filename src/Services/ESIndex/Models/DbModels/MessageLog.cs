using System;

namespace Models.DbModels
{
    public class MessageLog
    {
        public MessageLog(TimeSpan time, string messageType, int successNumber, int failedNumber, int failedContentNumber = 0, long? size = null)
        {
            Time = time;
            MessageType = messageType;
            Size = size;
            SuccessNumber = successNumber;
            FailedNumber = failedNumber;
            ContentReadingFailedNumber = failedContentNumber;
        }

        public TimeSpan Time { get; set; }
        public string MessageType { get; set; }
        public long? Size { get; set; }
        public int SuccessNumber { get; set; }
        public int FailedNumber { get; set; }
        public int ContentReadingFailedNumber { get; set; }

        public long Ticks
        {
            get { return Time.Ticks; }
        }
    }
}
