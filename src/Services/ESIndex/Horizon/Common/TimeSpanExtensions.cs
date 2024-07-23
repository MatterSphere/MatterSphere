using System;

namespace Horizon.Common
{
    public static class TimeSpanExtensions
    {
        public static string ToLabel(this TimeSpan time)
        {
            return time.Days > 0
                ? time.ToString(@"d\ hh\:mm\:ss")
                : time.ToString(@"hh\:mm\:ss");
        }
    }
}
