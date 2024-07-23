using System;
using System.ServiceProcess;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using NLog;

namespace MSIndex.IndexService
{
    public partial class IndexService : ServiceBase
    {
        private readonly Controller.Controller _controller;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _processTask;

        public IndexService()
        {
            InitializeComponent();

            int scanInterval;
            int.TryParse(ConfigurationManager.AppSettings["scanIntervalInSeconds"], out scanInterval);

            _controller = new Controller.Controller(
                ConfigurationManager.ConnectionStrings["msConnection"].ConnectionString,
                ConfigurationManager.ConnectionStrings["cdsConnection"].ConnectionString,
                ConfigurationManager.AppSettings["queue"],
                Environment.ExpandEnvironmentVariables(ConfigurationManager.AppSettings["cachePath"]),
                Math.Max(scanInterval, 1) * 1000);
        }

        protected override void OnStart(string[] args)
        {
            RunProcess();
        }

        protected override void OnStop()
        {
            _cancellationTokenSource.Cancel();
            _processTask?.Wait(Convert.ToInt32(Microsoft.Win32.Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", 10000)));
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
    }
}
