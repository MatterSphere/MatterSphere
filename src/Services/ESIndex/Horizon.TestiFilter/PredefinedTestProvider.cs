using System;
using System.Collections.Generic;
using System.IO;
using Horizon.TestiFilter.Common;

namespace Horizon.TestiFilter
{
    public class PredefinedTestProvider : ITestProvider
    {
        public TestInfo[] GetTestInfoList()
        {
            List<TestInfo> testInfoList = new List<TestInfo>();

            var path = AppDomain.CurrentDomain.BaseDirectory + "PredefinedTestFiles\\";
            var files = Directory.GetFiles(path);

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file).ToLower();

                var testInfo = new TestInfo()
                {
                    ExpectedValue = $"{extension.Substring(1)}_content_test".ToLower(),
                    File = new FileInfo(file)
                };

                testInfoList.Add(testInfo);
            }

            return testInfoList.ToArray();
        }

        public bool Validate(TestInfo testInfo, string content)
        {
            return content.ToLower().Contains(testInfo.ExpectedValue);
        }
    }
}
