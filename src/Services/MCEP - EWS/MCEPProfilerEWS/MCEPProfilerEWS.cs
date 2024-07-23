using System;
using System.ServiceProcess;

namespace MCEPProfilerEWS
{
    public partial class MCEPProfilerEWS : ServiceBase
    {
        private MCEPGlobalClasses.MCEPProfilerClass MCEPProfiler;
        private void GetClass()
        {
            if (MCEPProfiler == null)
            {
                MCEPProfiler = new MCEPGlobalClasses.MCEPProfilerClass();
            }
        }
        private void UnloadClass()
        {
            if (MCEPProfiler != null)
            {
                MCEPProfiler = null;
            }
        }
        public MCEPProfilerEWS()
        {
            InitializeComponent();
        }

        private void tmrTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tmrTimer.Enabled = false;
            GetClass();
            if (MCEPGlobalClasses.MCEPConfiguration.CheckDoNotProcessTimes())
            {
                MCEPProfiler.RunProcess();
            }
            bool shouldContinue = !MCEPProfiler.IsCancellationRequested;
            UnloadClass();
            GC.Collect();
            tmrTimer.Enabled = shouldContinue;
        }

        protected override void OnStart(string[] args)
        {
            tmrTimer.Interval = Convert.ToDouble(MCEPGlobalClasses.MCEPConfiguration.GetConfigurationItem("ProfilerTimerLength"));
            tmrTimer.AutoReset = true;
            tmrTimer.Start();
        }

        protected override void OnStop()
        {
            tmrTimer.Stop();
            var mcep = MCEPProfiler;
            if (mcep != null)
            {
                mcep.IsCancellationRequested = true;
            }
            tmrTimer.Enabled = false;
        }

    }
}
