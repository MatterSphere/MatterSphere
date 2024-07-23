using System;
using System.ServiceProcess;

namespace DocumentArchivingService
{
    public partial class DocumentArchiveService : ServiceBase
    {
        public DocumentArchiveService()
        {
            InitializeComponent();
        }

        private DocumentArchivingClass.DocumentArchiving DocumentArchiving;
        private void GetClass()
        {
            if (DocumentArchiving == null)
            {
                DocumentArchiving = new DocumentArchivingClass.DocumentArchiving();
            }
        }
        private void UnloadClass()
        {
            if (DocumentArchiving != null)
            {
                DocumentArchiving = null;
            }
        }

        private void tmrTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tmrTimer.Enabled = false;
            GetClass();
            if (DocumentArchivingClass.DocumentArchivingConfiguration.CheckDoNotProcessTimes())
            {
                DocumentArchiving.RunProcess();
            }
            bool shouldContinue = !DocumentArchiving.IsCancellationRequested;
            UnloadClass();
            GC.Collect();
            tmrTimer.Enabled = shouldContinue;
        }

        protected override void OnStart(string[] args)
        {
            tmrTimer.Interval = Convert.ToDouble(DocumentArchivingClass.DocumentArchivingConfiguration.GetConfigurationItem("SyncTimerLength"));
            tmrTimer.AutoReset = true;
            tmrTimer.Start();
        }

        protected override void OnStop()
        {
            tmrTimer.Stop();
            var docArc = DocumentArchiving;
            if (docArc != null)
            {
                docArc.IsCancellationRequested = true;
            }
            tmrTimer.Enabled = false;
        }
    }
}
