using System;
using System.ServiceProcess;

namespace MCEPStorerEWS
{
    public partial class MCEPStorerEWS : ServiceBase
    {
        private MCEPGlobalClasses.MCEPStorerClass MCEPStorer;
        private void GetClass()
        {
            if (MCEPStorer == null)
            {
                MCEPStorer = new MCEPGlobalClasses.MCEPStorerClass();
            }
        }
        private void UnloadClass()
        {
            if (MCEPStorer != null)
            {
                MCEPStorer = null;
            }
        }
        public MCEPStorerEWS()
        {
            InitializeComponent();
        }

        private void tmrTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tmrTimer.Enabled = false;
            GetClass();
            if (MCEPGlobalClasses.MCEPConfiguration.CheckDoNotProcessTimes())
            {
                MCEPStorer.RunProcess();
            }
            bool shouldContinue = !MCEPStorer.IsCancellationRequested;
            UnloadClass();
            GC.Collect();
            tmrTimer.Enabled = shouldContinue;
        }

        protected override void OnStart(string[] args)
        {
            tmrTimer.Interval = Convert.ToDouble(MCEPGlobalClasses.MCEPConfiguration.GetConfigurationItem("StorerTimerLength"));
            tmrTimer.AutoReset = true;
            tmrTimer.Start();
        }

        protected override void OnStop()
        {
            tmrTimer.Stop();
            var mcep = MCEPStorer;
            if (mcep != null)
            {
                mcep.IsCancellationRequested = true;
            }
            tmrTimer.Enabled = false;
        }
    }
}
