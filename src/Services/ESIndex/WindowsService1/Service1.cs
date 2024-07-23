using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using IndexingController;
using Microsoft.Win32;
using Models.Common;
using Models.ElasticsearchModels;
using NLog;

namespace IndexToolService
{
    public partial class Service1 : ServiceBase
    {
        private readonly Controller _controller;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private CancellationTokenSource _cancellationTokenSource;
        private const int _defaultThreadsCount = 0;
        private const int _defaultMaxBulkSize = 2000000;
        private const int _defaultMaxDocumentsCount = 100;

        private Task _processTask;

        public Service1()
        {
            InitializeComponent();

            _cancellationTokenSource = new CancellationTokenSource();
            
            _controller = new Controller(
                ConfigurationManager.ConnectionStrings["connection"].ConnectionString,
                ConfigurationManager.AppSettings["queue"],
                Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["cachePath"]),
                ConfigurationManager.AppSettings["indexURL"],
                Math.Max(GetIntSetting("scanIntervalInSeconds", 1), 1) * 1000,
                GetIntSetting("documentReadTimeoutInSeconds", 60) * 1000,
                GetBoolSetting("useOcrIndexing", false),
                GetIntSetting("documentOcrReadTimeoutInSeconds", 120) * 1000,
                GetIntSetting("dataIndexNumberOfShards", 1),
                GetIntSetting("dataIndexNumberOfReplicas", 0),
                GetIntSetting("userIndexNumberOfShards", 1),
                GetIntSetting("userIndexNumberOfReplicas", 0),
                Math.Max(GetIntSetting("workerQueueBound", 1), 1));

            var parameters = new ParametersData(
                GetThreadsCount(),
                GetLongSetting("maxBulkSize", _defaultMaxBulkSize),
                GetIntSetting("maxDocumentsCount", _defaultMaxDocumentsCount),
                GetBoolSetting("useExtendedLogs", false));
            _controller.SetParameters(parameters);

            _controller.SetElasticsearchClientParameters(new ElasticsearchClientParameters(
                ConfigurationManager.AppSettings["indexURL"],
                ConfigurationManager.AppSettings["elasticsearchApiKey"]));
        }

        protected override void OnStart(string[] args)
        {
            RunProcess();
        }

        protected override void OnStop()
        {
            StopProcess();
            if (ExitCode != 0)
            {
                Environment.Exit(ExitCode); // Abnormal service termination in order to utilize automatic recovery features of Windows services if configured.
            }
        }

        private void RunProcess()
        {
            var cancellationToken = _cancellationTokenSource.Token;
            _processTask = Task.Factory.StartNew(() => {
                try
                {
                    _controller.RunProcess(cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    ExitCode = 13816; // "An unknown error has occurred."
                    Stop();
                }
            }, cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void StopProcess()
        {
            _cancellationTokenSource.Cancel();
            _processTask?.Wait(Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", 10000)));
        }

        private int GetIntSetting(string settingName, int defaultValue)
        {
            int value;
            var success = int.TryParse(ConfigurationManager.AppSettings[settingName], out value);
            return success ? value : defaultValue;
        }

        private long GetLongSetting(string settingName, long defaultValue)
        {
            long value;
            var success = long.TryParse(ConfigurationManager.AppSettings[settingName], out value);
            return success ? value : defaultValue;
        }

        private bool GetBoolSetting(string settingName, bool defaultValue)
        {
            bool value;
            var success = bool.TryParse(ConfigurationManager.AppSettings[settingName], out value);
            return success ? value : defaultValue;
        }

        private int GetThreadsCount()
        {
            var threadsCountSettings = GetIntSetting("maxThreads", _defaultThreadsCount);
            return threadsCountSettings > 0 ? threadsCountSettings : Environment.ProcessorCount;
        }
    }
}
