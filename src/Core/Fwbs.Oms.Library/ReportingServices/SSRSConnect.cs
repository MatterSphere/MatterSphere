using System;
using System.Data;

namespace FWBS.OMS.ReportingServices
{
    /// <summary>
    /// Possible Distribution Extensions
    /// </summary>
    public enum ExtensionsOptions { ReportServerEmail, ReportServerFileShare }
    
    public class SSRSConnect
    {
        public const string typCheckbox = "Checkbox";
        public const string typDateTime = "DateTime";
        public const string typOptions = "Options";
        public const string typCaption = "Caption";
        public const string typTextbox = "TextBox";
        public const string typCombobox = "Combobox";
        public const string typTimebox = "Timebox";
        
        // Report Server defines the URL to root of the 
        // Reporting Services home page. 
        public static string ReportServerIP
        {
            get
            {
                string path = WebServer.Replace("/reportservice.asmx", "");
                path = WebServer.Replace("/reportservice2005.asmx", "");
                if (String.IsNullOrEmpty(path))
                    throw new Exception(Session.CurrentSession.Resources.GetMessage("ERRRPSRVLCNTST", "Error Reporting Server location has not been set. Please enter the Location by going to the File Menu and choosing Change Subsription Server", "").Text);
                return path;
            }
        }

        public static string Server
        {
            get
            {
                FWBS.Common.ApplicationSetting aei = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "BusinessReporting", "Server", "");
                return aei.ToString();
            }
        }

        public static string WebServer
        {
            get
            {
                FWBS.Common.ApplicationSetting aei = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "BusinessReporting", "WebServer", "");
                return aei.ToString();
            }
        }

        public static void SetReportingServerRegistryKeys(string Server, string WebServer)
        {
            FWBS.Common.ApplicationSetting aei = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "BusinessReporting", "Server", "");
            aei.SetSetting(Server);
            aei = new FWBS.Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "BusinessReporting", "WebServer", "");
            aei.SetSetting(WebServer);
        }

        public static DataTable GetListReports()
        {
            ISSRSWS connect = Create(true);
            return connect.ListReports(connect.ReportPath);
        }

        public static ISSRSWS Create(bool shared)
        {
            ISSRSWS connect;
            if (WebServer.ToLowerInvariant().EndsWith("reportservice.asmx"))
                connect = new FWBS.OMS.ReportingServices.SSRSConnect2005(shared);
            else    
                connect = new FWBS.OMS.ReportingServices.SSRSConnect2008(shared);
            return connect;
        }
    }
}
