namespace Fwbs.Office.Outlook.Settings
{
    public class ActivationSettings
    {
        public bool ShellExecuteEnabled { get; set; }

        public string ProcessName { get; set; }

        public string ExePath { get; set; }

        public int MaxTries { get; set; }

        public int WaitTimeout { get; set; }

        public string MutexName { get; set; }

        public bool ForceRelease { get; set; }

        public ActivationSettings Clone()
        {
            return (ActivationSettings)MemberwiseClone();
        }
    }
}
