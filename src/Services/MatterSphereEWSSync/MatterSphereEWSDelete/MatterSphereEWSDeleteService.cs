using System;
using System.ServiceProcess;

namespace MatterSphereEWSDelete
{
    public partial class MatterSphereEWSDeleteService : ServiceBase
    {
        public MatterSphereEWSDeleteService()
        {
            InitializeComponent();
        }

        private MatterSphereEWS.MatterSphereDelete MSews;
        private void GetClass()
        {
            if (MSews == null)
            {
                MSews = new MatterSphereEWS.MatterSphereDelete();
            }
        }
        private void UnloadClass()
        {
            if (MSews != null)
            {
                MSews = null;
            }
        }

        private void tmrTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tmrTimer.Enabled = false;
            GetClass();
            if (MatterSphereEWS.Config.CheckDoNotProcessTimes())
            {
                MSews.RunProcess();
            }
            bool shouldContinue = !MSews.IsCancellationRequested;
            UnloadClass();
            GC.Collect();
            tmrTimer.Enabled = shouldContinue;
        }

        protected override void OnStart(string[] args)
        {
            tmrTimer.Interval = Convert.ToDouble(MatterSphereEWS.Config.GetConfigurationItem("DeleteServiceTimer"));
            tmrTimer.AutoReset = true;
            tmrTimer.Start();
        }

        protected override void OnStop()
        {
            tmrTimer.Stop();
            var msews = MSews;
            if (msews != null)
            {
                msews.IsCancellationRequested = true;
            }
            tmrTimer.Enabled = false;
        }
    }
}
