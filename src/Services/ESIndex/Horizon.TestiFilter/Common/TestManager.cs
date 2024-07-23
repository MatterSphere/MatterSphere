using System;
using System.Configuration;
using System.Threading.Tasks;
using DocumentReader;

namespace Horizon.TestiFilter.Common
{
    public sealed class TestManager
    {
        private readonly ITestProvider _testProvider;
        private readonly Action<TestCallback> _callback;
        private readonly int _documentReadTimeout;
        private readonly int _documentOcrReadTimeout;
        private readonly bool _useOcrIndexing;
        private static bool _factoryInitialized;

        private TestManager()
        {
            if (!_factoryInitialized)
            {
                ContentReaderFactory.Startup();
                _factoryInitialized = true;
            }
        }

        public static void Destroy()
        {
            if (_factoryInitialized)
            {
                ContentReaderFactory.Shutdown();
            }
        }

        public TestManager(ITestProvider testProvider, Action<TestCallback> callback) : this()
        {
            _testProvider = testProvider;
            _callback = callback;

            if (!int.TryParse(ConfigurationManager.AppSettings["documentReadTimeoutInSeconds"], out _documentReadTimeout))
                _documentReadTimeout = 60;
            if (!int.TryParse(ConfigurationManager.AppSettings["documentOcrReadTimeoutInSeconds"], out _documentOcrReadTimeout))
                _documentOcrReadTimeout = 120;
            bool.TryParse(ConfigurationManager.AppSettings["useOcrIndexing"], out _useOcrIndexing);
            _documentReadTimeout *= 1000;
            _documentOcrReadTimeout *= 1000;
        }

        public void RunTests()
        {
            var testInfoList = _testProvider.GetTestInfoList();

            foreach (var testInfo in testInfoList)
            {
                var testCallback = Test(testInfo);

                _callback?.Invoke(testCallback);
            }
        }

        public async Task RunTestsAsync()
        {
            var testInfoList = _testProvider.GetTestInfoList();

            foreach (var testInfo in testInfoList)
            {
                var callback = await Task.Run(() => Test(testInfo));

                _callback?.Invoke(callback);
            }
        }

        private TestCallback Test(TestInfo testInfo)
        {
            var testCallback = new TestCallback { File = testInfo.File, ExpectedValue = testInfo.ExpectedValue };

            try
            {
                ContentReader reader = new ContentReader(_documentReadTimeout);
                string content = reader.GetContent(testInfo.File.FullName);
                if (_useOcrIndexing && string.IsNullOrWhiteSpace(content))
                {
                    reader = new OcrContentReader(_documentOcrReadTimeout);
                    content = reader.GetContent(testInfo.File.FullName);
                }

                testCallback.ResultValue = content;
                testCallback.TestPassed = _testProvider.Validate(testInfo, testCallback.ResultValue);
                testCallback.Error = testCallback.TestPassed
                    ? null
                    : new Exception($"The content does not have expected value. Result value: '{testCallback.ResultValue}', Expected value: '{testCallback.ExpectedValue}'.");
            }
            catch (Exception e)
            {
                testCallback.TestPassed = false;
                testCallback.Error = e;
            }

            return testCallback;
        }
    }
}
