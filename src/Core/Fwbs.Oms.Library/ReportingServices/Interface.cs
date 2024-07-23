using System.Data;

namespace FWBS.OMS.ReportingServices
{
    public interface ISSRSWS
    {
        char[] PathSeparatorArray { get; }
        string PathSeparatorString { get; }
        bool CheckStatus();
        void CreateSharedDataSource();
        string CreateSubscription(string Report, string SubscriptionDescription, DataTable extensions, DataTable Schedule, DataTable Parameters, string StartTime);
        DataTable DailySheduleQuestions();
        void DeleteReport(string ReportPath);
        void DeleteSubscription(string ID);
        DataSet GetSubscription(string SubscriptionID, string ReportName);
        DataTable ListReports(string Folder);
        DataTable ListSubscriptions();
        DataTable MonthlySheduleQuestions();
        object ProcessSubscriptionQuestions(DataTable questions);
        DataTable ReportParameters(string Report);
        void UpdateSubscription(string SubscriptionID, string SubscriptionDescription, DataTable extensions, DataTable Schedule, DataTable Parameters);
        string[] UploadReport(string ReportToUpload, string DestinationPath, out string reportName);
        DataTable WeeklySheduleQuestions();
        string ReportServerASMX { get; }
        string ReportServer { get; }
        string ReportPath { get; }
        string ReportWebUrlPath { get; }
        DataTable Extensions(FWBS.OMS.ReportingServices.ExtensionsOptions options);
    }
}
