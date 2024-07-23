using System;

namespace Horizon.Common
{
    public static class LongExtensions
    {
        public static string ToSize(this long size)
        {
            if (size >= 1000000000)
            {
                return $"{((double)size / 1000000000).ToString("F2")} GB";
            }

            if (size >= 1000000)
            {
                return $"{((double)size / 1000000).ToString("F2")} MB";
            }

            if (size >= 1000)
            {
                return $"{((double)size / 1000).ToString("F2")} KB";
            }

            return size > 0
                ? $"{size} Bytes"
                : "";
        }

        public static string ToLabel(this long size)
        {
            if (size >= 1000000)
            {
                return $"{((double)size / 1000000).ToString("F2")}M";
            }

            if (size >= 1000)
            {
                return $"{((double)size / 1000).ToString("F2")}K";
            }

            return size >= 0
                ? $"{size}"
                : "";
        }

        public static string ToTime(this long ticks)
        {
            var time = new TimeSpan(ticks);
            return time.ToLabel();
        }
    }
}
