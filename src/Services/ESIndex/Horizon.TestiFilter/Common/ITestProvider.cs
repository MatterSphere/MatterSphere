namespace Horizon.TestiFilter.Common
{
    public interface ITestProvider
    {
        TestInfo[] GetTestInfoList();
        bool Validate(TestInfo testInfo, string content);
    }
}
