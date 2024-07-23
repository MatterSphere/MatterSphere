using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Fwbs.Documents.Preview
{
    internal static class Utils
    {
        public static string FileSize(long bytes)
        {
            double size = bytes;
             
            if (bytes < 1024)
            {
                return bytes.ToString(System.Globalization.CultureInfo.InvariantCulture) + " B";
            }
            else
            {
                size = size / 1024;

                if (size < 1024)

                    return Math.Round(size, 2).ToString(System.Globalization.CultureInfo.InvariantCulture) + " KB";
                else
                {
                    size = size / 1024;
                    return Math.Round(size, 2).ToString(System.Globalization.CultureInfo.InvariantCulture) + " MB";
                }


            }
        }

        [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
        private static extern long FindExecutable(string lpFile, string lpDirectory, StringBuilder lpResult);

        public static string FindExecutable(string filename)
        {
            StringBuilder objResultBuffer = new StringBuilder(1024);
            long lngResult = FindExecutable(filename, null, objResultBuffer);
            if (lngResult >= 32)
            {
                return objResultBuffer.ToString();
            }
            return string.Empty;
        }
    }
}
