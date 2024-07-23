using System;
using System.Diagnostics;
using FWBS.OMS;

namespace Fwbs.Office.Outlook
{
    public sealed class OutlookActivation
    {
        private System.Threading.Mutex mutex;
        private bool isaddin;
        private Settings.ActivationSettings settings;
        private bool created;

        public OutlookActivation()
        {
            this.settings = new Settings.ActivationSettings();

            Settings.ApplicationSettings.Load(settings);

            var proc = System.Diagnostics.Process.GetCurrentProcess();

            isaddin = proc.ProcessName.Equals(settings.ProcessName, StringComparison.OrdinalIgnoreCase);

            mutex = new System.Threading.Mutex(isaddin, settings.MutexName, out created);
        }

        public void Release()
        {
            if (created)
            {
                try
                {
                    mutex.ReleaseMutex();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
            created = false;
        }

        public void SpawnAndWait()
        {
            if (!settings.ShellExecuteEnabled)
                return;

            if (isaddin)
                throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("CNTSPWNOUTL", "Cannot spawn outlook if the process is currently the addin instance.", "").Text);

            System.Diagnostics.Process.Start(settings.ExePath);

            for (int ctr = 1; ctr <= settings.MaxTries; ctr++)
            {
                if (mutex.WaitOne(settings.WaitTimeout))
                    return;
            }
            
            throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("OUTHSLNGRSP", "Outlook has taken too long to respond.", "").Text);
        }
    }
}
