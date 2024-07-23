using System;
using System.IO;

namespace Horizon.TestiFilter.Common
{
    public class TestCallback
    {
        public FileInfo File { get; set; }
        public string ResultValue { get; set; }
        public string ExpectedValue { get; set; }
        public bool TestPassed { get; set; }
        public Exception Error { get; set; }
    }
}
