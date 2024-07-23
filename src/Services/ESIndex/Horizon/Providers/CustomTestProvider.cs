using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Horizon.TestiFilter.Common;

namespace Horizon.Providers
{
    public class CustomTestProvider : ITestProvider
    {
        private readonly IEnumerable<FileInfo> _fileList;
        public CustomTestProvider(IEnumerable<FileInfo> fileList)
        {
            _fileList = fileList;
        }

        public TestInfo[] GetTestInfoList()
        {
            return _fileList.Select(info => new TestInfo() { File = info, ExpectedValue = null }).ToArray();
        }

        public bool Validate(TestInfo testInfo, string content)
        {
            Regex regex = new Regex("\\w");
            return !string.IsNullOrEmpty(content) || regex.IsMatch(content);
        }
    }
}
