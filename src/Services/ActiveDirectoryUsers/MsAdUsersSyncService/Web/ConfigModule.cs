using System;
using System.Linq;
using Nancy;
using Nancy.ModelBinding;

namespace MsAdUsersSyncService.Web
{
    public class ConfigModule : NancyModule
    {
        private const string MessageKey = "message";
        private const string ErrorKey = "error";
        private const string ConfigInfoKey = "ci";
        private readonly string[] MatterSphereLoginType = new string[] { "NT", "AAD" };

        /// <summary> Maximum value allowed to set ServiceTimerLength (10 days) </summary>
        /// <remarks> 864'000'000 milliseconds = (10 days * 24 hours * 60 minutes * 60 seconds * 1000 milliseconds) </remarks>
        private const int MaxServiceTimerLength = 10 * 24 * 60 * 60 * 1000;

        private const int MaxDomainNameLength = 48;

        public ConfigModule() : base("/config")
        {
            ConfigInfo configInfo;
            Get["/"] = x =>
            {
                var message = Session[MessageKey] != null ? Session[MessageKey].ToString() : string.Empty;
                var error = Session[ErrorKey] != null ? Session[ErrorKey].ToString() : string.Empty;
                if (Session[ConfigInfoKey] == null)
                {
                    configInfo = new ConfigInfo();
                    configInfo.LoadSettings();
                }
                else
                {
                    configInfo = (ConfigInfo) Session[ConfigInfoKey];
                }

                Session[MessageKey] = string.Empty;
                Session[ErrorKey] = string.Empty;
                Session[ConfigInfoKey] = configInfo;

                var model = new ConfigStatusModel
                {
                    Message = message,
                    Config = configInfo,
                    Error = error
                };

                return View["index.html", model];
            };
            Post["/update"] = parameters =>
            {
                var config = this.Bind<ConfigInfo>();
                string modelValidationResult = CheckModelValidation(config);
                if (!string.IsNullOrWhiteSpace(modelValidationResult))
                {
                    Session[ErrorKey] = modelValidationResult;
                    return Response.AsRedirect("/config");
                }
                else
                {
                    config.SaveSettings();

                    Session[MessageKey] = "Configuration Updated";
                    Session[ConfigInfoKey] = config;
                    return Response.AsRedirect("/config");
                }
            };
        }

        private string CheckModelValidation(ConfigInfo config)
        {
            if (config.ServiceTimerLength < 0 || config.ServiceTimerLength > MaxServiceTimerLength)
                return "ServiceTimerLength length should be in range (0, 864000000) symbols";

            if (config.MatterSphereServer.Length < 1 || config.MatterSphereServer.Length > 128)
                return "MatterSphereServer length should be in range (1, 128) symbols";

            if (config.MatterSphereDatabase.Length < 1 || config.MatterSphereDatabase.Length > 128)
                return "MatterSphereDatabase length should be in range (1, 128) symbols";

            if (config.MatterSphereLoginType.Length < 2 || config.MatterSphereLoginType.Length > 3)
                return "MatterSphereLoginType length should be in range (2, 3) symbols";

            if (!config.MatterSphereLoginType.Equals(MatterSphereLoginType[0], StringComparison.InvariantCultureIgnoreCase) &&
                !config.MatterSphereLoginType.Equals(MatterSphereLoginType[1], StringComparison.InvariantCultureIgnoreCase))
                return "MatterSphereLoginType value can be NT or AAD only.";
            else config.MatterSphereLoginType = config.MatterSphereLoginType.ToUpper();

            if (config.NetbiosSourceUserName.Length < 1 || config.NetbiosSourceUserName.Length > MaxDomainNameLength)
                return $"NetbiosSourceUserName length should be in range (1, {MaxDomainNameLength}) symbols";

            if (config.NetbiosTargetUserName.Length < 1 || config.NetbiosTargetUserName.Length > MaxDomainNameLength)
                return $"NetbiosTargetUserName length should be in range (1, {MaxDomainNameLength}) symbols";

            if (config.NetbiosTargetUserName.Equals(config.NetbiosSourceUserName, StringComparison.InvariantCultureIgnoreCase))
                return $"NetbiosSourceUserName and NetbiosTargetUserName should not be the same. ['{config.NetbiosSourceUserName}']";

            //TODO: Can be added extra validation for rest properties if needed.
            // LogToFileEnabled, LogAllToEventLog, DoNotProcessStart1, ..., DoNotProcessEnd2.

            return string.Empty;
        }
    }

    public class ConfigStatusModel
    {
        public ConfigInfo Config { get; set; }

        public string Message { get; set; }
        public bool HasMessage
        {
            get { return !string.IsNullOrWhiteSpace(Message); }
        }

        public string Error { get; set; }
        public bool HasError
        {
            get { return !string.IsNullOrWhiteSpace(Error); }
        }
    }
}